using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dots_1._0v
{
    /// <summary>
    /// Логика взаимодействия для AccountMenu.xaml
    /// </summary>
    public partial class AccountMenu : Window
    {
        const double diagramSize = 500;
        public AccountMenu()
        {
            InitializeComponent();

            Account main = AccountHandler.CurrentAccount;
            DataContext = main;

            if (main.GamesNumber != 0)
            {
                SetAnimationFor(BlockF, diagramSize / main.GamesNumber * main.FirstsWins, 1.0);
                SetAnimationFor(BlockF_, diagramSize / main.GamesNumber * main.SecondsGiveUps, 1.0);
                SetAnimationFor(BlockD, diagramSize / main.GamesNumber * main.Draws, 1.0);
                SetAnimationFor(BlockS_, diagramSize / main.GamesNumber * main.FirstsGiveUps, 1.0);
                SetAnimationFor(BlockS, diagramSize / main.GamesNumber * main.SecondsWins, 1.0);
            }
        }

        private void SetAnimationFor(TextBlock block, double to, double secondsDuration)
        {
            DoubleAnimation animation = new DoubleAnimation(0.0, to, new Duration(TimeSpan.FromSeconds(secondsDuration)));
            block.BeginAnimation(TextBlock.WidthProperty, animation);
        }

        private DoubleAnimation GetDoubleAnimation(double from, double to, double secondsDuration)
        {
            DoubleAnimation animation = new DoubleAnimation(from, to, new Duration(TimeSpan.FromSeconds(secondsDuration)));
            animation.RepeatBehavior = RepeatBehavior.Forever;
            animation.AutoReverse = true;
            return animation;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.SetBackgroundAnimation(sender as Grid);
            MainWindow.SetBackgroundAnimation(GradientBack);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AccountExit_Click(object sender, RoutedEventArgs e)
        {
            AccountHandler.CurrentAccount = null;
            Close();
        }
    }
}
