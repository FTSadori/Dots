using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dots_1._0v
{
    public class DotsField
    {
        private Field from;

        public int ROWS;
        public int COLS;

        public ObservableCollection<DotsViewModel> Dots { get; set; } = new ObservableCollection<DotsViewModel>();

        public string ColorAt(int i, int j) => Dots[i * COLS + j].BackgroundColor;
        public DotsViewModel this[int i, int j] => Dots[i * COLS + j];
        
        public bool[,] checkedDots;

        private void SetCheckedDotsFalse()
        {
            for (int i = 0; i < ROWS; ++i)
            {
                for (int j = 0; j < COLS; ++j)
                {
                    checkedDots[i, j] = false;
                }
            }
        }

        public DotsField(Field fromField)
        {
            from = fromField;
            ROWS = Field.ROWS;
            COLS = Field.COLS;
            checkedDots = new bool[ROWS, COLS];

            for (int i = 0; i < ROWS; ++i)
            {
                for (int j = 0; j < COLS; ++j)
                {
                    Dots.Add(new DotsViewModel(CurrentColors.Theme[GameColors.Default], i, j));
                }
            }
        }

        private bool NotOccupiedFrom(int i, int j, string color)
        {
            checkedDots[i, j] = true;

            int[,] IJ = { { i, j + 1 }, { i, j - 1 }, { i + 1, j }, { i - 1, j } };

            bool isFree = false;

            for (int num = 0; num < IJ.GetLength(0); ++num)
            {
                if (IJ[num, 0] < 0 || IJ[num, 1] < 0 || IJ[num, 0] >= ROWS || IJ[num, 1] >= COLS) return true;

                if (!checkedDots[IJ[num, 0], IJ[num, 1]] &&
                    (ColorAt(IJ[num, 0], IJ[num, 1]) == color ||
                    ColorAt(IJ[num, 0], IJ[num, 1]) == CurrentColors.Theme[GameColors.Default] ||
                    CurrentColors.lightFrom.Any(col => col.Value == ColorAt(IJ[num, 0], IJ[num, 1])))
                    )
                {
                    isFree = isFree || NotOccupiedFrom(IJ[num, 0], IJ[num, 1], color);
                }
            }

            return isFree;
        }

        public bool GameFunc(string forColor)
        {
            SetCheckedDotsFalse();

            bool hereIsEmptyDots = false;

            for (int i = 0; i < ROWS; ++i)
            {
                for (int j = 0; j < COLS; ++j)
                {
                    if (ColorAt(i, j) == CurrentColors.Theme[GameColors.Default])
                        hereIsEmptyDots = true;

                    if (ColorAt(i, j) != forColor)
                        continue;

                    if (!NotOccupiedFrom(i, j, ColorAt(i, j)))
                    {
                        string color = CurrentColors.lightFrom[CurrentColors.getOpposite[forColor]];

                        for (int _i = 0; _i < ROWS; ++_i)
                        {
                            for (int _j = 0; _j < COLS; ++_j)
                            {
                                if (checkedDots[_i, _j])
                                {
                                    this[_i, _j].BackgroundColor = color;
                                    this[_i, _j].Occupied = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        SetCheckedDotsFalse();
                    }
                }
            }

            return hereIsEmptyDots;
        }
    }
}
