using System;
using DlhSoft.Windows.Controls;

namespace Demos.WPF.CSharp.GanttChartDataGrid.Recurrence
{
    public class RecurrentGanttChartItem : GanttChartItem
    {
        private RecurrenceType recurrenceType = RecurrenceType.Weekly;
        public RecurrenceType RecurrenceType { get { return recurrenceType; } set { recurrenceType = value; OnPropertyChanged("RecurrenceType"); } }

        private int occurrenceCount = 1;
        public int OccurrenceCount { get { return occurrenceCount; } set { occurrenceCount = Math.Max(1, value); OnPropertyChanged("OccurrenceCount"); } }
    }
}
