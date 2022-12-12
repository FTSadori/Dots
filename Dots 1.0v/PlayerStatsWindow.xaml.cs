using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    /// Логика взаимодействия для PlayerStatsWindow.xaml
    /// </summary>
    public partial class PlayerStatsWindow : Window
    {
        private static List<Account> playersList = new();
        public static ObservableCollection<Account> SortedByFirst { get; set; } = new();
        public static ObservableCollection<Account> SortedBySecond { get; set; } = new();

        public static void RedoPlayers()
        {
            string[] lines = File.ReadAllLines(AccountHandler.ExeFullPath + AccountHandler.UsersPath);
            playersList.Clear();
            lines.ToList()
                .Where(l => l.Trim().Length > 0)
                .ToList()
                .ForEach(l => playersList.Add(JsonSerializer.Deserialize<Account>(l)));

            SortedByFirst.Clear();
            SortedBySecond.Clear();

            playersList.OrderByDescending(p => p.FirstsWins + p.SecondsGiveUps)
                    .Take(Math.Min(playersList.Count, 10))
                    .ToList()
                    .ForEach(p => SortedByFirst.Add(p));

            playersList.OrderByDescending(p => p.SecondsWins + p.FirstsGiveUps)
                   .Take(Math.Min(playersList.Count, 10))
                   .ToList()
                   .ForEach(p => SortedBySecond.Add(p));
        }

        public PlayerStatsWindow()
        {
            InitializeComponent();
            DataContext = this; 
            RedoPlayers();
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.SetBackgroundAnimation(sender as Grid);
            MainWindow.SetBackgroundAnimation(GradientBack);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow m = new();
            m.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
