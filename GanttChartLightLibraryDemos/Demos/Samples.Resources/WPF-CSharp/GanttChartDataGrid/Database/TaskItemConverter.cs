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

namespace GanttChartDataGridDatabaseSample
{
    public class TaskItemConverter : IValueConverter
    {
        // Retrieve a GanttChartItem based on Task data context.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Task task = value as Task;
            return (from i in GanttChartItems
                    where i.Tag == task
                    select i).SingleOrDefault();
        }

        // Retrieve a Task data item based on a GanttChartItem.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GanttChartItem item = value as GanttChartItem;
            return item.Tag as Task;
        }

        public IEnumerable<GanttChartItem> GanttChartItems { get; private set; }

        public static TaskItemConverter GetInstance(IEnumerable<GanttChartItem> ganttChartItems)
        {
            return new TaskItemConverter { GanttChartItems = ganttChartItems };
        }
    }
}
