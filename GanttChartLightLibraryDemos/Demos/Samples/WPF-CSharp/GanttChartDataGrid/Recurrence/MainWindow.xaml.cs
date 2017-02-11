using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using DlhSoft.Windows.Controls;
using System.Collections.ObjectModel;

namespace Demos.WPF.CSharp.GanttChartDataGrid.Recurrence
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RecurrentGanttChartItem item1 = GanttChartDataGrid.Items[1] as RecurrentGanttChartItem;
            item1.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item1.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            // Set OccurrenceCount to indicate the number of occurrences that should be generated for the task.
            item1.OccurrenceCount = 4;
            RecurrentGanttChartItem item2 = GanttChartDataGrid.Items[2] as RecurrentGanttChartItem;
            item2.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item2.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item2.OccurrenceCount = 3;
            RecurrentGanttChartItem item4 = GanttChartDataGrid.Items[4] as RecurrentGanttChartItem;
            item4.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item4.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"));
            item4.OccurrenceCount = int.MaxValue;
            RecurrentGanttChartItem item6 = GanttChartDataGrid.Items[6] as RecurrentGanttChartItem;
            item6.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item6.Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));
            // You may set OccurrenceCount to MaxValue to indicate that virtually unlimited occurrences should be generated.
            item6.OccurrenceCount = int.MaxValue;
            RecurrentGanttChartItem item7 = GanttChartDataGrid.Items[7] as RecurrentGanttChartItem;
            item7.Start = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("08:00:00"));
            item7.Finish = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("16:00:00"));
            // Set RecurrenceType to indicate the type of recurrence to use when generating occurrences for the task.
            item7.RecurrenceType = RecurrenceType.Daily;
            item7.OccurrenceCount = 2;

            // Component ApplyTemplate is called in order to complete loading of the user interface, after the main ApplyTemplate that initializes the custom theme, and using an asynchronous action to allow further constructor initializations if they exist (such as setting up the theme name to load).
            Dispatcher.BeginInvoke((Action)delegate
            {
                ApplyTemplate();

                // Apply template to be able to access the internal GanttChartView control.
                GanttChartDataGrid.ApplyTemplate();

                // Set up the internally managed occurrence item collection to be displayed in the chart area (instead of GanttChartDataGrid.Items).
                GanttChartDataGrid.GanttChartView.Items = ganttChartItemOccurrences;
                UpdateOccurrences();
            });
        }

        private string theme = "Generic-bright";
        public MainWindow(string theme) : this()
        {
            this.theme = "Cyan-green";
            ApplyTemplate();
        }
        public override void OnApplyTemplate()
        {
            LoadTheme();
            base.OnApplyTemplate();
        }
        private void LoadTheme()
        {
            if (theme == null || theme == "Default" || theme == "Aero")
                return;
            var themeResourceDictionary = new ResourceDictionary { Source = new Uri("/" + GetType().Assembly.GetName().Name + ";component/Themes/" + theme + ".xaml", UriKind.Relative) };
            GanttChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }

        // Stores the internally generated task occurrences displayed in the GanttChartView control.
        private ObservableCollection<GanttChartItem> ganttChartItemOccurrences = new ObservableCollection<GanttChartItem>();

        private void GanttChartDataGrid_TimelinePageChanged(object sender, EventArgs e)
        {
            UpdateOccurrences();
        }

        private void GanttChartDataGrid_ItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            foreach (GanttChartItem item in GanttChartDataGrid.Items)
                UpdateOccurrences(item as RecurrentGanttChartItem, false);
        }

        // Update the managed occurrence item collection for all DataGrid items, according to the current timeline page displayed in the chart area, based on their recurrence settings.
        private void UpdateOccurrences()
        {
            foreach (GanttChartItem item in GanttChartDataGrid.Items)
                UpdateOccurrences(item as RecurrentGanttChartItem, false);
        }

        // Update the managed occurrence item collection for the specified recurrent item, according to the current timeline page displayed in the chart area, based on their recurrence settings.
        private void UpdateOccurrences(RecurrentGanttChartItem item, bool preserveExistingOccurrences)
        {
            if (item == null)
                return;

            if (!preserveExistingOccurrences)
            {
                List<GanttChartItem> occurrences = (from i in ganttChartItemOccurrences
                                                    where i.Tag == item
                                                    select i).ToList();
                foreach (GanttChartItem occurrence in occurrences)
                    ganttChartItemOccurrences.Remove(occurrence);
            }

            int existingOccurrenceCount = ganttChartItemOccurrences.Count(i => i.Tag == item);
            if (item.OccurrenceCount >= existingOccurrenceCount)
            {
                for (int i = existingOccurrenceCount; i < item.OccurrenceCount; i++)
                {
                    DateTime start = item.Start, finish = item.Finish;
                    switch (item.RecurrenceType)
                    {
                        case RecurrenceType.Daily:
                            start = start.AddDays(i);
                            finish = finish.AddDays(i);
                            break;
                        case RecurrenceType.Weekly:
                            start = start.AddDays(7 * i);
                            finish = finish.AddDays(7 * i);
                            break;
                        case RecurrenceType.Monthly:
                            start = start.AddMonths(i);
                            finish = finish.AddMonths(i);
                            break;
                        case RecurrenceType.Yearly:
                            start = start.AddYears(i);
                            finish = finish.AddYears(i);
                            break;
                    }

                    // Avoid creating occurrences that would fall after the current timeline page (i.e. in the far future).
                    if (start >= GanttChartDataGrid.TimelinePageFinish)
                        break;

                    // Create and link a new Gantt Chart occurrence to its DataGrid item using Tag and Content properties.
                    GanttChartItem occurrence = new GanttChartItem { Start = start, Finish = finish, CompletedFinish = start, Tag = item };
                    BindingOperations.SetBinding(occurrence, GanttChartItem.ContentProperty, new Binding("Content") { Source = item });
                    ganttChartItemOccurrences.Add(occurrence);

                    // After having the new occurrence initialized as an element within GanttChartView, bind its visibility and position properties to the linked item values, accordingly (use a delegate to postpone code for the rendering phase).
                    Dispatcher.BeginInvoke((Action)delegate
                    {
                        BindingOperations.SetBinding(occurrence, GanttChartItem.IsVisibleProperty, new Binding("IsVisible") { Source = item, Mode = BindingMode.OneWay });
                        BindingOperations.SetBinding(occurrence, GanttChartItem.DisplayRowIndexProperty, new Binding("ActualDisplayRowIndex") { Source = item });
                    });
                }
            }
            else
            {
                List<GanttChartItem> occurrences = (from i in ganttChartItemOccurrences
                                                    where i.Tag == item
                                                    select i).Skip(item.OccurrenceCount).ToList();
                foreach (GanttChartItem occurrence in occurrences)
                    ganttChartItemOccurrences.Remove(occurrence);
            }
        }
    }
}
