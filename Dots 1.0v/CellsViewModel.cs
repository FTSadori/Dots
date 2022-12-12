using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dots_1._0v
{
    public class CellsViewModel
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        
        public string StartColor { get; set; }
        public string EndColor { get; set; }

        public CellsViewModel(int row, int column, string startPoint, string endPoint, string startColor, string endColor)
        {
            Row = row;
            Column = column;
            StartPoint = startPoint;
            EndPoint = endPoint;
            StartColor = startColor;
            EndColor = endColor;
        }
    }
}
