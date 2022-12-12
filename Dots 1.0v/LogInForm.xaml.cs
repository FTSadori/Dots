using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
    /// Логика взаимодействия для LogInForm.xaml
    /// </summary>
    public partial class LogInForm : Window
    {
        public LogInForm()
        {
            InitializeComponent();
        }

        private DoubleAnimation GetDoubleAnimation(double from, double to, double secondsDuration)
        {
            DoubleAnimation animation = new DoubleAnimation(from, to, new Duration(TimeSpan.FromSeconds(secondsDuration)));
            animation.RepeatBehavior = RepeatBehavior.Forever;
            animation.AutoReverse = true;
            return animation;
        }
        private ColorAnimation GetColorAnimation(Color from, Color to, double secondsDuration)
        {
            ColorAnimation animation = new ColorAnimation(from, to, new Duration(TimeSpan.FromSeconds(secondsDuration)));
            animation.RepeatBehavior = RepeatBehavior.Forever;
            animation.AutoReverse = true;
            return animation;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Grid grid = (Grid)sender;

            DoubleAnimation OpacityBackAnimation = GetDoubleAnimation(0.0, 0.3, 2.0);
            DoubleAnimation OpacityGlowAnimation = GetDoubleAnimation(0.5, 1.0, 2.0);

            double backTimeGap = 1000;
            int backCount = 0;

            foreach (UIElement element in grid.Children)
            {
                Image image = element as Image;
                if (image != null)
                {
                    OpacityBackAnimation.BeginTime = TimeSpan.FromMilliseconds(backTimeGap * backCount++);
                    image.BeginAnimation(Image.OpacityProperty, (AnimationTimeline)OpacityBackAnimation.GetAsFrozen());
                }
            }

            backGlow.BeginAnimation(OpacityProperty, OpacityGlowAnimation);
            grid.Background.BeginAnimation(SolidColorBrush.ColorProperty, GetColorAnimation(Colors.Violet, Colors.DarkViolet, 3.0));
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountHandler.TryToAddNewUser(Box_Username.Text, Box_Email.Text, Box_Password.Password, Box_PasswordConf.Password))
            {
                RegTitle.Text = "Success";
                MessageBox.Show("Success");
            }
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            AutTitle.Text = "Sign In";
            MainGrid.RowDefinitions[1].Height = new GridLength(0);
            MainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.RowDefinitions[2].Height = new GridLength(0);
            MainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
        }

        private void AutSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AccountHandler.TryAuthorization(Aut_Login.Text, Aut_Password.Password))
            {
                MessageBox.Show("Wrong login or password");
            }
            else
            {
                this.Close();
                MessageBox.Show("Success");
            }
        }
    }
}
