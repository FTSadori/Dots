using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media.Animation;
using System.Media;

namespace Dots_1._0v
{
    internal class Dot : Button
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool Occupied { get; set; } = false;

        public Dot()
        {
            if (ColCounter == Field.COLS)
            {
                ColCounter = 0;
                Row = ++RowCounter;
            }

            Row = RowCounter;
            Column = ColCounter++;
        }

        private static int ColCounter = 0;
        private static int RowCounter = 0;

        public static void SetCountersZero()
        {
            ColCounter = 0;
            RowCounter = 0;
        }
    }
}
