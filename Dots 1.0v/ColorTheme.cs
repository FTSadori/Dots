using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Windows.Media;

namespace Dots_1._0v
{

    public class ColorTheme
    {
        public string ThemeName { get; set; } = "Glamour";
        public string FirstPlayer { get; set; } = "DeepPink";
        public string SecondPlayer { get; set; } = "DeepSkyBlue";
        public string LightFirstPlayer { get; set; } = "Pink";
        public string LightSecondPlayer { get; set; } = "SkyBlue";
        public string Background { get; set; } = "#FF100025";
        public string BackgroundLight { get; set; } = "DarkSlateGray";
        public string Default { get; set; } = "White";
        public string GridBackground { get; set; } = "Black";

        public LinearGradientBrush MainGradient { get; set; }
        
        public LinearGradientBrush LightGradient { get; set; }

        public ColorTheme()
        {
            ResetGradients();
        }

        public void ResetGradients()
        {
            LinearGradientBrush brush = new();
            brush.StartPoint = new System.Windows.Point(0, 0.5);
            brush.EndPoint = new System.Windows.Point(1, 0.5);
            brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(FirstPlayer), 0f));
            brush.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(SecondPlayer), 1f));
            MainGradient = brush;

            LinearGradientBrush brush1 = new();
            brush1.StartPoint = new System.Windows.Point(0, 0.5);
            brush1.EndPoint = new System.Windows.Point(1, 0.5);
            brush1.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(LightFirstPlayer), 0f));
            brush1.GradientStops.Add(new GradientStop(CurrentColors.ConvertStringToColor(LightSecondPlayer), 1f));
            LightGradient = brush1;
        }

        public string this[GameColors c] { get
            {
                switch (c)
                {
                    case GameColors.Player1:         return FirstPlayer;
                    case GameColors.Player1Light:    return LightFirstPlayer;
                    case GameColors.Player2:         return SecondPlayer;
                    case GameColors.Player2Light:    return LightSecondPlayer;
                    case GameColors.Background:      return Background; 
                    case GameColors.BackgroundLight: return BackgroundLight;
                    case GameColors.Default:         return Default;
                    case GameColors.GridBackground:  return GridBackground;
                    default: return "Black";
                }
            } 
        }
        static string ExeFullPath => System.IO.Path.GetDirectoryName(Environment.ProcessPath);

        static public List<ColorTheme> GetAllThemes()
        {
            string[] lines = File.ReadAllLines(ExeFullPath + @"\ColorThemes.json");
            List<ColorTheme> arr = new List<ColorTheme>();

            foreach (string line in lines)
            {
                ColorTheme? theme = JsonSerializer.Deserialize<ColorTheme>(line);
                if (theme == null) continue;
                arr.Add(theme);
            }
            return arr;
        }

        static public void AddToFile(ColorTheme colorTheme)
        {
            File.AppendAllText(ExeFullPath + @"\ColorThemes.json", JsonSerializer.Serialize(colorTheme));
        }
    }
}
