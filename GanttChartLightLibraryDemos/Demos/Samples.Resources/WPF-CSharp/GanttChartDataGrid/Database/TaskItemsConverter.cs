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
    public class TaskItemsConverter : IValueConverter
    {
        // Retrieve a collection of GanttChartItem based on Task data context.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<Task> tasks = (value as IEnumerable<Task>).OrderBy(t => t.Index);
            ObservableCollection<GanttChartItem> items = new ObservableCollection<GanttChartItem>();
            foreach (Task task in tasks)
            {
                GanttChartItem item = new GanttChartItem { Tag = task };
                SetGanttChartItem(item);
                items.Add(item);
            }
            foreach (GanttChartItem item in items)
                SetGanttChartItemPredecessors(item, items);
            items.CollectionChanged += Items_CollectionChanged;
            return items;
        }

        // When the GanttChartItem collection changes, update data context accordingly.
        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    IEnumerable<GanttChartItem> items = sender as IEnumerable<GanttChartItem>;
                    foreach (GanttChartItem item in e.NewItems)
                    {
                        Task task = new Task { Name = item.Content as string ?? "New Task", Start = item.Start, Finish = item.Finish, Completion = item.Start, Assignments = string.Empty, Index = GetNextTaskIndex(items) };
                        item.Tag = task;
                        SetGanttChartItem(item);
                        SetGanttChartItemPredecessors(item, items);
                        Context.Tasks.AddObject(task);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (GanttChartItem item in e.OldItems)
                    {
                        Task task = item.Tag as Task;
                        Context.Tasks.DeleteObject(task);
                    }
                    break;
            }
        }

        private void SetGanttChartItem(GanttChartItem item)
        {
            Task task = item.Tag as Task;
            BindingOperations.SetBinding(item, GanttChartItem.IndentationProperty, new Binding("Indentation") { Source = task, Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(item, GanttChartItem.ContentProperty, new Binding("Name") { Source = task, Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(item, GanttChartItem.StartProperty, new Binding("Start") { Source = task, Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(item, GanttChartItem.FinishProperty, new Binding("Finish") { Source = task, Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(item, GanttChartItem.CompletedFinishProperty, new Binding("Completion") { Source = task, Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(item, GanttChartItem.IsMilestoneProperty, new Binding("IsMilestone") { Source = task, Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(item, GanttChartItem.AssignmentsContentProperty, new Binding("Assignments") { Source = task, Mode = BindingMode.TwoWay });
        }

        private void SetGanttChartItemPredecessors(GanttChartItem item, IEnumerable<GanttChartItem> items)
        {
            Task task = item.Tag as Task;
            BindingOperations.SetBinding(item, GanttChartItem.PredecessorsProperty, new Binding("Predecessors") { Source = task, Converter = PredecessorItemsConverter.GetInstance(task, items) });
        }

        private int GetNextTaskIndex(IEnumerable<GanttChartItem> items)
        {
            items = items.Where(i => i.Tag != null);
            if (!items.Any())
                return 0;
            return (from i in items
                    let t = i.Tag as Task
                    select t.Index).Max() + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public DatabaseEntities Context { get; private set; }

        public static TaskItemsConverter GetInstance(DatabaseEntities context)
        {
            return new TaskItemsConverter { Context = context };
        }
    }
}
