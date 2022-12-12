using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dots_1._0v
{
    public class Field
    {
        public static int ROWS = 10;
        public static int COLS = 17;
        public static int CellSize { get; set; } = 39;

        public float[] playersDomination = { 0.0f, 0.0f };
        
        public CellsField cellsField;
        public DotsField dotsField;

        public ObservableCollection<CellsViewModel> Cells { get { return cellsField.Cells; } }
        public ObservableCollection<DotsViewModel> Dots { get { return dotsField.Dots; } }



        public Field()
        {
            Dot.SetCountersZero();
            cellsField = new CellsField(this);
            dotsField = new DotsField(this);
        }
    }
}
