using DlhSoft.Windows.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Demos.WPF.CSharp.ScheduleChartDataGrid.ResourceMovingAndHierarchy
{
    class CustomScheduleChartItem : ScheduleChartItem, INotifyPropertyChanged
    {
        private ImageSource image;
        public ImageSource Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        private double dragOpacity = 1.0;
        public double DragOpacity
        {
            get { return dragOpacity; }
            set { dragOpacity = value; OnPropertyChanged("DragOpacity"); }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
