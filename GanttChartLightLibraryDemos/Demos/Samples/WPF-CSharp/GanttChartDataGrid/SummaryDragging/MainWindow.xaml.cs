using System;
using System.Windows;
using DlhSoft.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Linq;

namespace Demos.WPF.CSharp.GanttChartDataGrid.SummaryDragging
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GanttChartItem item0 = GanttChartDataGrid.Items[0];

            GanttChartItem item1 = GanttChartDataGrid.Items[1];
            item1.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item1.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            item1.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));
            item1.AssignmentsContent = "Resource 1";

            GanttChartItem item2 = GanttChartDataGrid.Items[2];
            item2.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item2.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item2.AssignmentsContent = "Resource 1, Resource 2";
            item2.Predecessors.Add(new PredecessorItem { Item = item1 });

            GanttChartItem item3 = GanttChartDataGrid.Items[3];
            item3.Predecessors.Add(new PredecessorItem { Item = item0, DependencyType = DependencyType.StartStart });

            GanttChartItem item4 = GanttChartDataGrid.Items[4];
            item4.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item4.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"));

            GanttChartItem item6 = GanttChartDataGrid.Items[6];
            item6.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item6.Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));

            GanttChartItem item7 = GanttChartDataGrid.Items[7];
            item7.Start = DateTime.Today.AddDays(4);
            item7.IsMilestone = true;
            item7.Predecessors.Add(new PredecessorItem { Item = item4 });
            item7.Predecessors.Add(new PredecessorItem { Item = item6 });

            for (int i = 3; i <= 25; i++)
            {
                GanttChartDataGrid.Items.Add(
                    new GanttChartItem
                    {
                        Content = "Task " + i,
                        Indentation = i % 3 == 0 ? 0 : 1,
                        Start = DateTime.Today.AddDays(i <= 8 ? (i - 4) * 2 : i - 8),
                        Finish = DateTime.Today.AddDays((i <= 8 ? (i - 4) * 2 + (i > 8 ? 6 : 1) : i - 2) + 2),
                        CompletedFinish = DateTime.Today.AddDays(i <= 8 ? (i - 4) * 2 : i - 8).AddDays(i % 6 == 4 ? 3 : 0)
                    });
            }
        }

        private string theme = "Generic-bright";
        public MainWindow(string theme) : this()
        {
            this.theme = theme;
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

        // Used to record information/state while summary item dragging operations occur.
        // DraggedSummaryItem indicates the summary item being dragged, and draggedChildItemOffsets indicate the leaf child items and their effort offsets compared to the dragged summary item.
        private GanttChartItem draggedSummaryItem;
        private Dictionary<GanttChartItem, TimeSpan> draggedChildItemOffsets = new Dictionary<GanttChartItem, TimeSpan>();

        private void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            var element = sender as Thumb;
            // Record the summary item that the end user started to drag, and store the original offsets for the start date-time values
            // of its leaf child items (of all levels) - to be used upon DragDelta event handler.
            draggedSummaryItem = element?.DataContext as GanttChartItem;
            if (draggedSummaryItem == null)
                return;
            draggedChildItemOffsets = draggedSummaryItem.AllChildren.Cast<GanttChartItem>().Where(i => !i.HasChildren)
                .ToDictionary(i => i, i => GanttChartDataGrid.GetEffort(draggedSummaryItem.Start, i.Start));
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (draggedSummaryItem == null)
                return;
            // Determine the updated summary start date-time based on the visibility schedule (visible days and hours of the timeline)
            // and considering the horizontally dragged distance (e.HorizontalChange) reported by the thumb.
            var draggedDurationInHours = e.HorizontalChange / GanttChartDataGrid.HourWidth;
            var draggedDuration = TimeSpan.FromHours(draggedDurationInHours);
            var visibilitySchedule = GanttChartDataGrid.GetVisibilitySchedule();
            var updatedSummaryStart = GanttChartDataGrid.GetFinish(draggedSummaryItem.Start, draggedDuration, visibilitySchedule);
            // Then, update all leaf child item start date-times according to the updated start value of the summary and 
            // considering their original effort offsets.
            foreach (GanttChartItem item in draggedChildItemOffsets.Keys)
            {
                var updatedStart = GanttChartDataGrid.GetFinish(updatedSummaryStart, draggedChildItemOffsets[item]);
                item.RescheduleToStart(updatedStart);
            }
        }
    }
}
