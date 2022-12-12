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

namespace Dots_1._0v
{
    public enum GameColors
    {
        Player1,
        Player1Light,
        Player2,
        Player2Light,
        Background,
        BackgroundLight,
        Default,
        GridBackground,
    }

    public static class CurrentColors
    {
        public static ColorTheme Theme { get; private set; }

        static CurrentColors()
        {
            SetThemeById(0);
        }

        static public void SetThemeById(int i)
        {
            Theme = ColorTheme.GetAllThemes()[i];
            Theme.ResetGradients();

            getOpposite = new Dictionary<string, string>();
            getOpposite.Add(Theme[GameColors.Player1], Theme[GameColors.Player2]);
            getOpposite.Add(Theme[GameColors.Player2], Theme[GameColors.Player1]);

            lightFrom = new Dictionary<string, string>();
            lightFrom.Add(Theme[GameColors.Player1], Theme[GameColors.Player1Light]);
            lightFrom.Add(Theme[GameColors.Player2], Theme[GameColors.Player2Light]);
            lightFrom.Add(Theme[GameColors.Background], Theme[GameColors.BackgroundLight]);
        }

        static public Dictionary<string, string> lightFrom;

        static public Dictionary<string, string> getOpposite;

        static public string MainColorOfPlayer(int num) 
            => (num == 0) ? GetMainFirstPlayerColor : GetMainSecondPlayerColor;
        
        static public string LightColorOfPlayer(int num)
            => (num == 0) ? GetLightFirstPlayerColor : GetLightSecondPlayerColor;

        public static string GetMainFirstPlayerColor { get { return Theme.FirstPlayer; } }
        public static string GetMainSecondPlayerColor { get { return Theme.SecondPlayer; } }

        public static string GetLightFirstPlayerColor { get { return Theme.LightFirstPlayer; } }
        public static string GetLightSecondPlayerColor { get { return Theme.LightSecondPlayer; } }

        public static string GetBackgroundColor { get { return Theme.Background; } }
        public static string GetGridBackgroundColor { get { return Theme.GridBackground; } }
        public static string GetLightBackgroundColor { get { return Theme.BackgroundLight; } }

        public static string GetDefaultColor { get { return Theme.Default; } }

        public static LinearGradientBrush MainGBrush { get { return Theme.MainGradient; } }
        public static LinearGradientBrush LightGBrush { get { return Theme.LightGradient; } }

        public static SolidColorBrush ConvertStringToBrush(string color)
        {
            if (new BrushConverter().IsValid(color))
                return (SolidColorBrush)(new BrushConverter().ConvertFromString(color));
            return Brushes.White;
        }

        public static Color ConvertStringToColor(string color)
        {
            if (new ColorConverter().IsValid(color))
                return (Color)ColorConverter.ConvertFromString(color);
            return Colors.White;
        }
    }
}
