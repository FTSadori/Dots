using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dots_1._0v
{
    public class CellsField
    {
        public int ROWS;
        public int COLS;

        public Field from;

        public ObservableCollection<CellsViewModel> Cells { get; set; } = new ObservableCollection<CellsViewModel>();

        public CellsField(Field fromField)
        {
            from = fromField;
            ROWS = Field.ROWS - 1;
            COLS = Field.COLS - 1;
            
            for (int i = 0; i < ROWS; ++i)
            {
                for (int j = 0; j < COLS; ++j)
                {
                    Cells.Add(new CellsViewModel(i, j, "0,0", "1,1",
                        CurrentColors.GetBackgroundColor, CurrentColors.GetBackgroundColor));
                }
            }
        }
        private void ChangeCellAt(int i, int j, CellsViewModel model)
        {
            Cells.RemoveAt(i * COLS + j);
            Cells.Insert(i * COLS + j, model);
        }

        public void CheckAllCells()
        {
            for (int i = 0; i < ROWS; ++i)
            {
                for (int j = 0; j < COLS; ++j)
                {
                    if (isEmptyCellAt(i, j)) continue;

                    if (CheckForFullPatterns(i, j)) continue;

                    CheckForHalfPatterns(i, j);
                }
            }
        }

        private bool isEmptyCellAt(int i, int j)
        {
            string defaultColor = CurrentColors.Theme[GameColors.Default];
            int defaults = 0;

            foreach (int _i in new int[]{ i, i + 1 })
                foreach (int _j in new int[]{ j, j + 1 })
                    defaults += (from.dotsField[_i, _j].BackgroundColor == defaultColor) ? 1 : 0;

            return defaults > 1;
        }

        private bool CheckForFullPatterns(int i, int j)
        {
            int k = -1;
            if (CheckPattern(i, j, "0000")) k = 0;
            else if (CheckPattern(i, j, "1111")) k = 1;
            else return false;

            ChangeCellAt(i, j, new CellsViewModel(i, j, "0,0", "1,1", CurrentColors.MainColorOfPlayer(k), CurrentColors.MainColorOfPlayer(k)));
            from.playersDomination[k] += 1.0f;
            return true;
        }

        private bool CheckForHalfPatterns(int i, int j)
        {
            string[][] patterns = { new string[] { "a00*", "*00a", "0*a0", "0a*0" },
                                    new string[] { "b11*", "*11b", "1*b1", "1b*1" }};

            for (int num = 0; num < 2; ++num)
            {
                foreach (string pattern in patterns[num])
                {
                    if (CheckPattern(i, j, pattern))
                    {
                        int a = pattern.IndexOf('*');
                        int b = pattern.IndexOf((num == 0) ? 'a' : 'b');

                        ChangeCellAt(i, j, new CellsViewModel(i, j, $"{b % 2},{b / 2}", $"{a % 2},{a / 2}",
                            CurrentColors.MainColorOfPlayer(num),
                            CurrentColors.GetBackgroundColor));

                        from.playersDomination[num] += 0.5f;
                        return true;
                    }
                }
            }
            return false;
        }

        // pattern містить чотири символи, для TL TR BL BR відповідно:
        // - "0" будь-який колір першого гравця
        // - "1" будь-який колір другого гравця
        // - "a" світлий колір першого гравця
        // - "b" світлий колір другого гравця
        // інше - будь-який колір взагалі
        private bool CheckPattern(int i, int j, string pattern)
        {
            int lightCounter = 0;
            
            int[,] IJ = { { i, j }, { i, j + 1 }, { i + 1, j }, { i + 1, j + 1 } };

            for (int num = 0; num < IJ.GetLength(0); ++num)
            {
                string color = from.dotsField.ColorAt(IJ[num, 0], IJ[num, 1]);

                switch (pattern[num])
                {
                    case '0':
                    case '1':
                        GameColors m = (pattern[num] == '0') ? GameColors.Player1 : GameColors.Player2;

                        if (!(color == CurrentColors.Theme[m] ||
                              color == CurrentColors.lightFrom[CurrentColors.Theme[m]]))
                            return false;

                        if (color == CurrentColors.lightFrom[CurrentColors.Theme[m]]) 
                            lightCounter++;
                        break;
                    case 'a':
                    case 'b':
                        if (color != CurrentColors.LightColorOfPlayer(pattern[num] - 'a'))
                            return false;
                        break;
                    default:
                        break;
                }
            }
            if (!pattern.Contains('*') && lightCounter < 2)
                return false;

            return true;
        }
    }
}
