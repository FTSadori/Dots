using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Media;

namespace Dots_1._0v
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            SettingsWindow.LoadSettings();
            
            InitializeComponent();
            DataContext = this;
            AccountHandler.CheckFiles();
            
            if (!StaticSoundPlayer.isBackPlaying)
                StaticSoundPlayer.StartBackgroundMusic(MusicEnable.menuSong);
        }
        private void GridLoaded(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            SetLettersAnimation(grid);
            SetBackgroundAnimation(grid);
            SetBackgroundAnimation(GradientBack);
        }
        static public void SetBackgroundAnimation(Grid grid)
        {
            ColorAnimation colorAnimation = new ColorAnimation(
                CurrentColors.ConvertStringToColor(CurrentColors.GetGridBackgroundColor),
                CurrentColors.ConvertStringToColor(CurrentColors.GetBackgroundColor),
                new Duration(TimeSpan.FromSeconds(1.5)));
            colorAnimation.RepeatBehavior = RepeatBehavior.Forever;
            colorAnimation.AutoReverse = true;

            grid.Background = new SolidColorBrush(Colors.White);
            grid.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }
        static public void SetBackgroundAnimation(TextBlock gradientBack)
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0.5);
            brush.EndPoint = new Point(1, 0.5);
            brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.GetMainFirstPlayerColor), 0.0));
            brush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.3));
            brush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.7));
            brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.GetMainSecondPlayerColor), 1.0));

            gradientBack.Background = brush;
            gradientBack.Opacity = 0.0;

            DoubleAnimation OpacityChangeAnimation = new DoubleAnimation(0.0, 0.2, new Duration(TimeSpan.FromSeconds(2.5)));
            OpacityChangeAnimation.RepeatBehavior = RepeatBehavior.Forever;
            OpacityChangeAnimation.AutoReverse = true;

            gradientBack.BeginAnimation(TextBlock.OpacityProperty, OpacityChangeAnimation);
        }
        private void SetLettersAnimation(Grid grid)
        {
            double timeGap = 0;
            
            foreach (UIElement element in grid.Children)
            {
                TextBlock textBlock = element as TextBlock;
                if (textBlock != null)
                {
                    if (textBlock.Name.StartsWith("Letter"))
                    {
                        ThicknessAnimation thicknessAnimation =
                            new ThicknessAnimation(new Thickness(textBlock.Margin.Left, 0.0, textBlock.Margin.Right, 50.0),
                                                   new Thickness(textBlock.Margin.Left, 50.0, textBlock.Margin.Right, 0.0),
                                                   new Duration(TimeSpan.FromSeconds(1.0)));
                        thicknessAnimation.RepeatBehavior = RepeatBehavior.Forever;
                        thicknessAnimation.AutoReverse = true;

                        thicknessAnimation.BeginTime = TimeSpan.FromMilliseconds(timeGap++ * 150);
                        textBlock.BeginAnimation(TextBlock.MarginProperty, (AnimationTimeline)thicknessAnimation.GetAsFrozen());
                    }
                }
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AccountHandler.IsLoggedIn)
            {
                AccountButton_Click(sender, e);
            }
            else
            {
                GameWindow gameWindow = new GameWindow();
                gameWindow.Show();
                this.Close();
            }
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            StaticSoundPlayer.PlaySound(SoundEnable.niceClick);
            Thread.Sleep(300);
            this.Close();
        }
        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            StaticSoundPlayer.PlaySound(SoundEnable.mouseEnterSound);
            SettingsWindow n = new SettingsWindow();
            n.Show();
            this.Close();
        }
        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            StaticSoundPlayer.PlaySound(SoundEnable.niceClick);
            if (!AccountHandler.IsLoggedIn)
            {
                LogInForm form = new LogInForm();
                form.ShowDialog();
            }
            else
            {
                AccountMenu accountMenu = new AccountMenu();
                this.Close();
                accountMenu.Show();
            }
        }

        private void StatsButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerStatsWindow s = new();
            s.Show();
            Close();
        }

        // // //
        private void PlayButton_MouseEnter(object sender, MouseEventArgs e)
        {
            StaticSoundPlayer.PlaySound(SoundEnable.mouseEnterSound);
            ChangeDotsAlignment(Dot1l, Dot1r);
        }
        private void PlayButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeDotsAlignment(Dot1l, Dot1r);
        }
        private void SettingButton_MouseEnter(object sender, MouseEventArgs e)
        {
            StaticSoundPlayer.PlaySound(SoundEnable.mouseEnterSound);
            ChangeDotsAlignment(Dot2l, Dot2r);
        }
        private void SettingButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeDotsAlignment(Dot2l, Dot2r);
        }
        private void ExitButton_MouseEnter(object sender, MouseEventArgs e)
        {
            StaticSoundPlayer.PlaySound(SoundEnable.mouseEnterSound);
            ChangeDotsAlignment(Dot3l, Dot3r);
        }
        private void ExitButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeDotsAlignment(Dot3l, Dot3r);
        }
        private void RoundButton_MouseEnter(object sender, MouseEventArgs e)
        {
            StaticSoundPlayer.PlaySound(SoundEnable.mouseEnterSound);
        }
        // // //
        private void ChangeDotsAlignment(Button DotLeft, Button DotRight)
        {
            DotLeft.HorizontalAlignment = (DotLeft.HorizontalAlignment == HorizontalAlignment.Left) ?
                HorizontalAlignment.Right : HorizontalAlignment.Left;
            DotRight.HorizontalAlignment = (DotRight.HorizontalAlignment == HorizontalAlignment.Left) ?
                HorizontalAlignment.Right : HorizontalAlignment.Left;
        }
    }
}
