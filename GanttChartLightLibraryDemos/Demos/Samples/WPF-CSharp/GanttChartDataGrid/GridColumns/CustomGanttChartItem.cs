using DlhSoft.Windows.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Demos.WPF.CSharp.GanttChartDataGrid.GridColumns
{
    class CustomGanttChartItem : GanttChartItem, INotifyPropertyChanged
    {
        public CustomGanttChartItem()
        {
            // Alternatively, attached properties (e.g. MyValue3 and MyValue4) can be provided (e.g. by GanttChartItemAttachments).
            // Optionally, also associate a tag object for defaults of other properties that may usually come from a different system (e.g. MyValue5, and MyValue6).
            Tag = new CustomDataObject();
        }

        private int myValue1;
        public int MyValue1
        {
            get { return myValue1; }
            set
            {
                if (value < 0)
                    value = 0;
                myValue1 = value;
                OnPropertyChanged("MyValue1");
            }
        }

        private string myValue2;
        public string MyValue2
        {
            get { return myValue2; }
            set
            {
                myValue2 = value;
                OnPropertyChanged("MyValue2");
            }
        }
    }
}
