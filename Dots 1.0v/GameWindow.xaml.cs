using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Dots_1._0v
{
    /// <summary>
    /// Логика взаимодействия для GameWindow.xaml
    /// </summary>
    interface ICloseableView
    {
        void Close(bool confirm);
    }

    public partial class GameWindow : Window, ICloseableView
    {
        // Номер поточного гравця
        private int CurrentPlayer = 0;

        // Загальна кількість гравців
        public int PlayerNum = 2;

        private Field field;

        // Об'єкти поля (клітинки та крапки)

        // Градієнт кольорів першого гравця
        public GameWindow()
        {
            InitializeComponent();

            field = new Field();
            DataContext = field;
            StaticSoundPlayer.PlaySound(SoundEnable.weaponsPull);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.SetBackgroundAnimation(sender as Grid);
            MainWindow.SetBackgroundAnimation(BackGradient);
        }

        public void ChangeColor()
        {
            CurrentPlayer = (CurrentPlayer + 1) % PlayerNum;

            CurrentTurnText.Foreground = CurrentColors.ConvertStringToBrush(CurrentColors.MainColorOfPlayer(CurrentPlayer));
            CurrentTurnText_Back.Foreground = CurrentColors.ConvertStringToBrush(CurrentColors.LightColorOfPlayer(CurrentPlayer));
            
            CurrentTurnText_Back.Text = CurrentTurnText.Text = "Хiд гравця " + (CurrentPlayer + 1);
        }
       
        private void ChangePlayerDominationTextFilds()
        {
            FirstPlayerDomination_Back.Text = FirstPlayerDomination.Text = field.playersDomination[0].ToString() + " кл";
            SecondPlayerDomination_Back.Text = SecondPlayerDomination.Text = field.playersDomination[1].ToString() + " кл";
        }

        private void GameEnd()
        {
            EndWindow window;
            if (field.playersDomination[0] != field.playersDomination[1])
            {
                CurrentTurnText.Text = CurrentTurnText_Back.Text = "Кінець гри";
                if (field.playersDomination[0] > field.playersDomination[1])
                {
                    AccountHandler.CurrentAccount.FirstsWins++;
                    window = new EndWindow(GameEnding.WINNER1);
                }
                else
                {
                    AccountHandler.CurrentAccount.SecondsWins++;
                    window = new EndWindow(GameEnding.WINNER2);
                }
            }
            else
            {
                AccountHandler.CurrentAccount.Draws++;

                window = new EndWindow(GameEnding.DRAW);

                CurrentTurnText.Text = CurrentTurnText_Back.Text = "Нічия";
            }

            AccountHandler.SaveCurrent();
            window.ShowDialog();

            this.Close(false);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dot dot = (Dot)sender;
            if (dot == null) return;

            int i = dot.Row;
            int j = dot.Column;

            if (!field.dotsField[i, j].Occupied)
            {
                StaticSoundPlayer.PlaySound(SoundEnable.selectSound);
                AccountHandler.CurrentAccount.ClickedDotsNumber++;

                field.dotsField[i, j].BackgroundColor = CurrentColors.MainColorOfPlayer(CurrentPlayer);
                field.dotsField[i, j].Occupied = true;

                bool gameContinue = field.dotsField.GameFunc(CurrentColors.MainColorOfPlayer((CurrentPlayer + 1) % 2));

                if (field.dotsField.GameFunc(CurrentColors.MainColorOfPlayer(CurrentPlayer)))
                    gameContinue = true;

                ChangeColor();

                field.playersDomination[0] = field.playersDomination[1] = 0.0f;

                field.cellsField.CheckAllCells();

                ChangePlayerDominationTextFilds();

                if (!gameContinue) GameEnd();
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            if (button == null) return;

            button.Width += 5;
            button.Height += 5;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            if (button == null) return;

            button.Width -= 5;
            button.Height -= 5;
        }

        private bool PauseResult()
        {
            string currTurn = CurrentTurnText.Text;
            CurrentTurnText.Text = CurrentTurnText_Back.Text = "Пауза";

            InGameMenuWindow inGameMenuWindow = new InGameMenuWindow();
            bool? result = inGameMenuWindow.ShowDialog();

            if (result != null)
            {
                if ((bool)result)
                {
                    CurrentTurnText.Text = CurrentTurnText_Back.Text = "Кінець гри";

                    EndWindow endWindow;
                    if (CurrentPlayer == 0)
                    {
                        endWindow = new EndWindow(GameEnding.WINNER2);
                        AccountHandler.CurrentAccount.FirstsGiveUps++;
                    }
                    else
                    {
                        endWindow = new EndWindow(GameEnding.WINNER1);
                        AccountHandler.CurrentAccount.SecondsGiveUps++;
                    }
                    endWindow.ShowDialog();

                    AccountHandler.SaveCurrent();
                    return true;
                }
            }
            CurrentTurnText.Text = CurrentTurnText_Back.Text = currTurn;
            return false;
        }

        private void PauseButtonClick(object sender, RoutedEventArgs e)
        {
            if (PauseResult()) Close(false);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!confirmClosing)
                e.Cancel = false;
            else 
                e.Cancel = !PauseResult();
        
            if (!e.Cancel)
            {
                AccountHandler.CurrentAccount.GamesNumber++;
                AccountHandler.SaveCurrent();

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }

        private bool confirmClosing = true;
        public void Close(bool confirm)
        {
            confirmClosing = confirm;
            Close();
        }
    }
}
