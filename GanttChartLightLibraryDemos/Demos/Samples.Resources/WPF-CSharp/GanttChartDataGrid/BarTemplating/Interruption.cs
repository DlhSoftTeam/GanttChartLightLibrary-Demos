using System;
using System.ComponentModel;
using DlhSoft.Windows.Controls;

namespace Demos.WPF.CSharp.GanttChartDataGrid.BarTemplating
{
    class Interruption : INotifyPropertyChanged
    {
        public CustomGanttChartItem Item { get; internal set; }
        public IGanttChartView GanttChartView { get { return Item != null ? Item.GanttChartView : null; } }

        private DateTime start;
        public DateTime Start
        {
            get { return start; }
            set
            {
                start = value;
                OnPropertyChanged("Start");
            }
        }
        public double ComputedLeft
        {
            get { return GanttChartView.GetPosition(Start) - GanttChartView.GetPosition(Item.Start); }
        }

        private DateTime finish;
        public DateTime Finish
        {
            get { return finish; }
            set
            {
                finish = value;
                OnPropertyChanged("Finish");
            }
        }
        public double ComputedWidth
        {
            get { return GanttChartView.GetPosition(Finish) - GanttChartView.GetPosition(Item.Start) - ComputedLeft; }
        }

        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            switch (propertyName)
            {
                case "Start":
                    OnPropertyChanged("ComputedLeft");
                    break;
            }
            switch (propertyName)
            {
                case "Start":
                case "Finish":
                    OnPropertyChanged("ComputedWidth");
                    break;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
