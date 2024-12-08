using DlhSoft.Windows.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Demos.WPF.CSharp.ScheduleChartDataGrid.MainFeatures
{
    class CustomGanttChartItem : GanttChartItem, INotifyPropertyChanged
    {
        private ImageSource icon;
        public ImageSource Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                OnPropertyChanged("Icon");
            }
        }

        private string note;
        public string Note
        {
            get { return note; }
            set
            {
                note = value;
                OnPropertyChanged("Note");
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
