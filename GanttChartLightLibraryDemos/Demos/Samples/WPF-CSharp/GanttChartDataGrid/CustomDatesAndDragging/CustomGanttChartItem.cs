using DlhSoft.Windows.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Demos.WPF.CSharp.GanttChartDataGrid.CustomDatesAndDragging
{
    class CustomGanttChartItem : GanttChartItem, INotifyPropertyChanged
    {
        private DateTime customStart = DateTime.Today;
        public DateTime CustomStart
        {
            get { return customStart; }
            set
            {
                if (value > CustomFinish)
                    CustomFinish = value;
                customStart = value;
                OnPropertyChanged(nameof(CustomStart));
            }
        }
        public double ComputedCustomBarLeft
        {
            get { return GanttChartView.GetPosition(CustomStart) - GanttChartView.GetPosition(Start); }
        }

        private DateTime customFinish = DateTime.Today;
        public DateTime CustomFinish
        {
            get { return customFinish; }
            set
            {
                if (value < CustomStart)
                    CustomStart = value;
                customFinish = value;
                OnPropertyChanged(nameof(CustomFinish));
            }
        }
        public double ComputedCustomBarWidth
        {
            get { return GanttChartView.GetPosition(CustomFinish) - GanttChartView.GetPosition(CustomStart); }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Start):
                case nameof(CustomStart):
                    OnPropertyChanged(nameof(ComputedCustomBarLeft));
                    break;
            }
            switch (propertyName)
            {
                case nameof(CustomStart):
                case nameof(CustomFinish):
                    OnPropertyChanged(nameof(ComputedCustomBarWidth));
                    break;
            }
        }

        // Alternatively (required, if you have mouse wheel zooming enabled), refresh the user interface from a central handler.
        protected override void OnBarChanged()
        {
            OnPropertyChanged(nameof(ComputedCustomBarLeft));
            OnPropertyChanged(nameof(ComputedCustomBarWidth));
            base.OnBarChanged();
        }
    }
}
