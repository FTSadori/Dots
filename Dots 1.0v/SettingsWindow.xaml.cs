using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dots_1._0v
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, ICloseableView
    {
        static public bool Inverted { get; set; } = false;
        static public int CurrentThemeNum { get; set; } = 0;
        static public string FieldSizeString { get { return Field.ROWS + "x" + Field.COLS; } }
        static public string CurrentTheme { get { return CurrentColors.Theme.ThemeName; } }
        static public string SoundVolume { get { return Math.Round(StaticSoundPlayer.soundPlayer.Volume * 100).ToString() + "%"; } }
        static public string MusicVolume { get { return Math.Round(StaticSoundPlayer.backgroundMusic.Volume * 100).ToString() + "%"; } }

        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static void LoadSettings()
        {
            if (!File.Exists("Settings.json"))
            {
                StaticSoundPlayer.soundPlayer.Volume = 0.4;
                StaticSoundPlayer.backgroundMusic.Volume = 0.4;
                return;
            }

            Dictionary<string, int> d = JsonSerializer.Deserialize<Dictionary<string, int>>(File.ReadAllText("Settings.json"));
            Field.ROWS = d["FieldRows"];
            Field.COLS = d["FieldCols"];
            StaticSoundPlayer.soundPlayer.Volume = Convert.ToDouble(d["SoundVolume"]) / 100;
            StaticSoundPlayer.backgroundMusic.Volume = Convert.ToDouble(d["MusicVolume"]) / 100;
            CurrentColors.SetThemeById(d["ChoosedThemeId"]);
            CurrentThemeNum = d["ChoosedThemeId"];
            
            if (d["Inverted"] == 1)
            {
                Inverted = true;
                string firstMainColor = CurrentColors.GetMainFirstPlayerColor;
                string firstLightColor = CurrentColors.GetLightFirstPlayerColor;
                CurrentColors.Theme.FirstPlayer = CurrentColors.Theme.SecondPlayer;
                CurrentColors.Theme.SecondPlayer = firstMainColor;
                CurrentColors.Theme.LightFirstPlayer = CurrentColors.Theme.LightSecondPlayer;
                CurrentColors.Theme.LightSecondPlayer = firstLightColor;
                CurrentColors.Theme.ResetGradients();
            }
        }

        public static void SaveSettings()
        {
            Dictionary<string, int> d = new();
            d["FieldRows"] = Field.ROWS;
            d["FieldCols"] = Field.COLS;
            d["SoundVolume"] = Convert.ToInt32(StaticSoundPlayer.soundPlayer.Volume * 100);
            d["MusicVolume"] = Convert.ToInt32(StaticSoundPlayer.backgroundMusic.Volume * 100);
            d["ChoosedThemeId"] = CurrentThemeNum;
            d["Inverted"] = Inverted ? 1 : 0;

            File.WriteAllText("Settings.json", JsonSerializer.Serialize(d));
        }

        private bool openMenu = true;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (openMenu)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.SetBackgroundAnimation(sender as Grid);
            MainWindow.SetBackgroundAnimation(GradientBack);
        }

        private void ThemeChange_Click(object sender, RoutedEventArgs e)
        {
            StaticSoundPlayer.PlaySound(SoundEnable.selectSound);
            ++CurrentThemeNum;
            if (CurrentThemeNum == ColorTheme.GetAllThemes().Count)
                CurrentThemeNum = 0;
            CurrentColors.SetThemeById(CurrentThemeNum);
            CurrentThemeName.Text = CurrentTheme;
            Inverted = false;

            SettingsWindow s = new();
            s.Show();

            SaveSettings();

            Close(false);
        }
        
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close(true);
        }
        private void SoundChange_Click(object sender, RoutedEventArgs e)
        {
            if (StaticSoundPlayer.soundPlayer.Volume == 1.0)
                StaticSoundPlayer.soundPlayer.Volume = 0.0;
            else
                StaticSoundPlayer.soundPlayer.Volume += 0.2;
            
            SoundButton.Content = SoundVolume;
            StaticSoundPlayer.PlaySound(SoundEnable.selectSound);
            SaveSettings();
        }

        private void MusicChange_Click(object sender, RoutedEventArgs e)
        {
            if (StaticSoundPlayer.backgroundMusic.Volume == 1.0)
                StaticSoundPlayer.backgroundMusic.Volume = 0.0;
            else
                StaticSoundPlayer.backgroundMusic.Volume += 0.2;

            MusicButton.Content = MusicVolume;
            StaticSoundPlayer.PlaySound(SoundEnable.selectSound);
            SaveSettings();
        }

        public void InvertColors_Click(object sender, RoutedEventArgs e)
        {
            string firstMainColor = CurrentColors.GetMainFirstPlayerColor;
            string firstLightColor = CurrentColors.GetLightFirstPlayerColor;
            CurrentColors.Theme.FirstPlayer = CurrentColors.Theme.SecondPlayer;
            CurrentColors.Theme.SecondPlayer = firstMainColor;
            CurrentColors.Theme.LightFirstPlayer = CurrentColors.Theme.LightSecondPlayer;
            CurrentColors.Theme.LightSecondPlayer = firstLightColor;
            Inverted = !Inverted;

            CurrentColors.Theme.ResetGradients();

            SettingsWindow s = new();
            s.Show();

            SaveSettings();

            Close(false);
        }

        private void FieldChange_Click(object sender, RoutedEventArgs e)
        {
            StaticSoundPlayer.PlaySound(SoundEnable.selectSound);
            if (Field.ROWS == 16)
            {
                Field.ROWS = 4;
                Field.COLS = 8;
            }
            else
            {
                Field.ROWS += 2;
                Field.COLS += 3;
            }
            FieldSizeButton.Content = FieldSizeString;
            SaveSettings();
        }

        public void Close(bool confirm)
        {
            openMenu = confirm;
            Close();
        }
    }
}
