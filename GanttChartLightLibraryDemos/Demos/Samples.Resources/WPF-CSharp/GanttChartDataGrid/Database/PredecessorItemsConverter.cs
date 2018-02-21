using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DlhSoft.Windows.Controls;
using System.ComponentModel;
using System.Linq;
using System.Collections.Specialized;

namespace GanttChartDataGridDatabaseSample
{
    public class PredecessorItemsConverter : IValueConverter
    {
        // Retrieve a PredecessorItemCollection based on Predecessor data context.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<Predecessor> predecessors = (value as IEnumerable<Predecessor>).OrderBy(p => p.Task.Index);
            PredecessorItemCollection items = new PredecessorItemCollection();
            foreach (Predecessor predecessor in predecessors)
            {
                PredecessorItem item = new PredecessorItem { Tag = predecessor };
                SetPredecessorItem(item);
                items.Add(item);
            }
            items.CollectionChanged += Items_CollectionChanged;
            return items;
        }

        // When the PredecessorItem collection changes, update data context accordingly.
        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    IEnumerable<PredecessorItem> items = sender as IEnumerable<PredecessorItem>;
                    foreach (PredecessorItem item in e.NewItems)
                    {
                        Task task = item.Item.Tag as Task;
                        Predecessor predecessor = new Predecessor { Task = task, DependencyType = (int)DependencyType.FinishStart };
                        item.Tag = predecessor;
                        SetPredecessorItem(item);
                        ContextTask.Predecessors.Add(predecessor);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (PredecessorItem item in e.OldItems)
                    {
                        Predecessor predecessor = item.Tag as Predecessor;
                        ContextTask.Predecessors.Remove(predecessor);
                    }
                    break;
            }
        }

        private void SetPredecessorItem(PredecessorItem item)
        {
            Predecessor predecessor = item.Tag as Predecessor;
            BindingOperations.SetBinding(item, PredecessorItem.ItemProperty, new Binding("Task") { Source = predecessor, Mode = BindingMode.TwoWay, Converter = TaskItemConverter.GetInstance(GanttChartItems) });
            BindingOperations.SetBinding(item, PredecessorItem.DependencyTypeProperty, new Binding("DependencyType") { Source = predecessor, Mode = BindingMode.TwoWay, Converter = NumericDependencyTypeConverter.Instance });
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public Task ContextTask { get; private set; }
        public IEnumerable<GanttChartItem> GanttChartItems { get; private set; }

        public static PredecessorItemsConverter GetInstance(Task contextTask, IEnumerable<GanttChartItem> ganttChartItems)
        {
            return new PredecessorItemsConverter { ContextTask = contextTask, GanttChartItems = ganttChartItems };
        }
    }
}
