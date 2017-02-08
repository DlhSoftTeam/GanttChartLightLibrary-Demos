using System;
using System.Windows.Media;
using DlhSoft.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;

namespace Demos.WPF.CSharp.GanttChartDataGrid.BarTemplating
{
    class CustomGanttChartItem : GanttChartItem, INotifyPropertyChanged
    {
        public CustomGanttChartItem()
        {
            Markers.CollectionChanged += Markers_CollectionChanged;
            Interruptions.CollectionChanged += Interruptions_CollectionChanged;
        }

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

        private DateTime estimatedStart = DateTime.Today;
        public DateTime EstimatedStart
        {
            get { return estimatedStart; }
            set
            {
                if (value > EstimatedFinish)
                    EstimatedFinish = value;
                estimatedStart = value;
                OnPropertyChanged("EstimatedStart");
            }
        }
        public double ComputedEstimatedBarLeft
        {
            get { return GanttChartView.GetPosition(EstimatedStart) - GanttChartView.GetPosition(Start); }
        }

        private DateTime estimatedFinish = DateTime.Today;
        public DateTime EstimatedFinish
        {
            get { return estimatedFinish; }
            set
            {
                if (value < EstimatedStart)
                    EstimatedStart = value;
                estimatedFinish = value;
                OnPropertyChanged("EstimatedFinish");
            }
        }
        public double ComputedEstimatedBarWidth
        {
            get { return GanttChartView.GetPosition(EstimatedFinish) - GanttChartView.GetPosition(EstimatedStart); }
        }

        ObservableCollection<Marker> markers = new ObservableCollection<Marker>();
        public ObservableCollection<Marker> Markers { get { return markers; } }
        private void Markers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Marker marker in e.NewItems)
                        marker.Item = this;
                    break;
            }
        }

        ObservableCollection<Interruption> interruptions = new ObservableCollection<Interruption>();
        public ObservableCollection<Interruption> Interruptions { get { return interruptions; } }
        private void Interruptions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (Interruption interruption in e.NewItems)
                        {
                            interruption.Item = this;
                            interruption.PropertyChanged += Interruption_PropertyChanged;
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (Interruption interruption in e.OldItems)
                            interruption.PropertyChanged -= Interruption_PropertyChanged;
                        break;
                    }
            }
            OnPropertyChanged("ComputedInterruptedBars");
        }
        private void Interruption_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("ComputedInterruptedBars");
        }
        public IEnumerable<InterruptedBar> ComputedInterruptedBars
        {
            get
            {
                var interruptions = from i in Interruptions
                                    where (i.ComputedLeft > 0 || i.ComputedLeft + i.ComputedWidth > 0) &&
                                          (i.ComputedLeft < ComputedBarWidth || i.ComputedLeft + i.ComputedWidth < ComputedBarWidth)
                                    orderby i.ComputedLeft
                                    select i;
                double previousRight = 0;
                foreach (Interruption interruption in interruptions)
                {
                    if (interruption.ComputedLeft > previousRight)
                        yield return new InterruptedBar { Item = this, Left = previousRight, Width = interruption.ComputedLeft - previousRight };
                    previousRight = interruption.ComputedLeft + interruption.ComputedWidth;
                }
                if (ComputedBarWidth > previousRight)
                    yield return new InterruptedBar { Item = this, Left = previousRight, Width = ComputedBarWidth - previousRight };
            }
        }
        public class InterruptedBar
        {
            public CustomGanttChartItem Item { get; internal set; }
            public IGanttChartView GanttChartView { get { return Item != null ? Item.GanttChartView : null; } }
            public double Left { get; set; }
            public double Width { get; set; }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case "Start":
                case "EstimatedStart":
                    OnPropertyChanged("ComputedEstimatedBarLeft");
                    break;
            }
            switch (propertyName)
            {
                case "EstimatedStart":
                case "EstimatedFinish":
                    OnPropertyChanged("ComputedEstimatedBarWidth");
                    break;
            }
            switch (propertyName)
            {
                case "Start":
                    foreach (Marker marker in Markers)
                        marker.OnPropertyChanged("ComputedLeft");
                    foreach (Interruption interruption in Interruptions)
                    {
                        interruption.OnPropertyChanged("ComputedLeft");
                        interruption.OnPropertyChanged("ComputedWidth");
                    }
                    break;
            }
            switch (propertyName)
            {
                case "Start":
                case "Finish":
                    Dispatcher.BeginInvoke((Action)delegate
                    {
                        OnPropertyChanged("ComputedInterruptedBars");
                    });
                    break;
            }
        }

        // Alternatively (required, if you have mouse wheel zooming enabled), refresh the user interface from a central handler.
        // protected override void OnBarChanged()
        // {
        //     OnPropertyChanged("ComputedEstimatedBarLeft");
        //     OnPropertyChanged("ComputedEstimatedBarWidth");
        //     OnPropertyChanged("ComputedInterruptedBars");
        //     base.OnBarChanged();
        // }
    }
}
