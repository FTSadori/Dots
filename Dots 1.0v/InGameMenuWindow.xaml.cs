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

namespace Dots_1._0v
{
    /// <summary>
    /// Логика взаимодействия для InGameMenuWindow.xaml
    /// </summary>
    public partial class InGameMenuWindow : Window
    {
        public InGameMenuWindow()
        {
            InitializeComponent();
        }

        private void ReturnButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GiveUpButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
