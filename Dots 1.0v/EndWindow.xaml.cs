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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Media.Animation;

namespace Dots_1._0v
{
    public enum GameEnding
    {
        DRAW,
        WINNER1,
        WINNER2,
    }

    /// <summary>
    /// Логика взаимодействия для EndWindow.xaml
    /// </summary>
    public partial class EndWindow : Window
    {
        private GameEnding Ending = GameEnding.WINNER1;

        public EndWindow()
        {
            InitializeComponent();

            SetUpColors();
        }

        public EndWindow(GameEnding ending)
        {
            Ending = ending;

            InitializeComponent();

            SetUpColors();
        }

        private void SetUpColors()
        {
            MainGrid.Background = GetBackgroundGradient();

            Background = CurrentColors.ConvertStringToBrush(CurrentColors.GetGridBackgroundColor);
            EndButton.BorderBrush = GetMainGradient();
            EndButton.Background = GetBackgroundGradient();
            EndButton.Foreground = GetMainGradient();
        }

        private LinearGradientBrush GetBackgroundGradient()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0.5);
            brush.EndPoint = new Point(1, 0.5);

            brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.GetGridBackgroundColor), 0.1f));
            brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.GetGridBackgroundColor), 0.9f));

            switch (Ending)
            {
                case GameEnding.DRAW:
                    brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.GetMainFirstPlayerColor), 0f));
                    brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.GetMainSecondPlayerColor), 1f));
                    break;
                case GameEnding.WINNER1:
                case GameEnding.WINNER2:
                    int winner = Convert.ToInt32(Ending.ToString().Last().ToString()) - 1;
                    brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.MainColorOfPlayer(winner)), 0f));
                    brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.LightColorOfPlayer(winner)), 1f));
                    break;
            }
            return brush;
        }

        private LinearGradientBrush GetMainGradient(bool doStandard = true, bool first = false)
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0.5);
            brush.EndPoint = new Point(1, 0.5);
            
            switch (Ending)
            {
                case GameEnding.DRAW:
                    if (doStandard)
                    {
                        brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.GetMainFirstPlayerColor), 0f));
                        brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.GetMainSecondPlayerColor), 1f));
                    }
                    else
                    {
                        if (first)
                        {
                            brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.GetMainFirstPlayerColor), 0f));
                            brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.GetLightFirstPlayerColor), 1f));
                        }
                        else
                        {
                            brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.GetMainSecondPlayerColor), 0f));
                            brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.GetLightSecondPlayerColor), 1f));
                        }
                    }
                    break;
                case GameEnding.WINNER1:
                case GameEnding.WINNER2:
                    int winner = Convert.ToInt32(Ending.ToString().Last().ToString()) - 1;
                    brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.MainColorOfPlayer(winner)), 0f));
                    brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(CurrentColors.LightColorOfPlayer(winner)), 1f));
                    break;
            }
            return brush;
        }

        private void GridLoaded(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            
            DoubleAnimation UpDownAnimation = new DoubleAnimation(0.0, 100.0, new Duration(TimeSpan.FromSeconds(1.0)));
            UpDownAnimation.RepeatBehavior = RepeatBehavior.Forever;
            UpDownAnimation.AutoReverse = true;

            DoubleAnimation OpacityChangeAnimation = new DoubleAnimation(1.0, 0.2, new Duration(TimeSpan.FromSeconds(2.0)));
            OpacityChangeAnimation.RepeatBehavior = RepeatBehavior.Forever;
            OpacityChangeAnimation.AutoReverse = true;

            int columnsTimeGap = 0;
            int lettersTimeGap = 0;
            bool first = false;

            foreach (UIElement element in grid.Children)
            {
                TextBlock textBlock = element as TextBlock;
                if (textBlock != null)
                {
                    if (textBlock.Name.StartsWith("Col"))
                    {
                        textBlock.Background = GetMainGradient(false, first = !first);
                        textBlock.Height = 0;

                        UpDownAnimation.BeginTime = TimeSpan.FromMilliseconds(columnsTimeGap++ * 150);
                        textBlock.BeginAnimation(TextBlock.HeightProperty, (AnimationTimeline)UpDownAnimation.GetAsFrozen());
                    }
                    else if (textBlock.Name.StartsWith("L"))
                    {
                        textBlock.Foreground = GetMainGradient();
                        int index = Convert.ToInt32(textBlock.Name.Substring(1));
                        if (index < Ending.ToString().Length)
                           textBlock.Text = Ending.ToString()[index].ToString();

                        OpacityChangeAnimation.BeginTime = TimeSpan.FromMilliseconds(++lettersTimeGap * 200);
                        textBlock.BeginAnimation(TextBlock.OpacityProperty, (AnimationTimeline)OpacityChangeAnimation.GetAsFrozen());
                    }
                }
            }
        }

        private void ReturnButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
