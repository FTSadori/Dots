using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Dots_1._0v
{
    public class DotsViewModel : INotifyPropertyChanged
    {
        private string backgroundColor;

        public string BackgroundColor { 
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                OnPropertyChanged("BackgroundColor");
            } 
        }
        public int Row { get; set; }
        public int Column { get; set; }
        public string Content { get; set; } = "";
        public bool Occupied { get; set; } = false;

        public DotsViewModel(string background, int row, int column)
        {
            BackgroundColor = background;
            Row = row;
            Column = column;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
