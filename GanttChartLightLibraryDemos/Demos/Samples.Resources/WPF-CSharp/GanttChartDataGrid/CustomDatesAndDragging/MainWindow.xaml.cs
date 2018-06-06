using System;
using System.Windows;
using DlhSoft.Windows.Controls;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using DlhSoft.Windows.Data;
using System.Windows.Input;

namespace Demos.WPF.CSharp.GanttChartDataGrid.CustomDatesAndDragging
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CustomGanttChartItem item0 = GanttChartDataGrid.Items[0] as CustomGanttChartItem;

            CustomGanttChartItem item1 = GanttChartDataGrid.Items[1] as CustomGanttChartItem;
            item1.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item1.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            item1.CustomStart = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item1.CustomFinish = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem item2 = GanttChartDataGrid.Items[2] as CustomGanttChartItem;
            item2.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item2.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item2.Predecessors.Add(new PredecessorItem { Item = item1 });
            item2.CustomStart = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item2.CustomFinish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem item3 = GanttChartDataGrid.Items[3] as CustomGanttChartItem;
            item3.Predecessors.Add(new PredecessorItem { Item = item0, DependencyType = DependencyType.StartStart });
            item3.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item3.Finish = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("16:00:00"));
            item3.CustomStart = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item3.CustomFinish = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem item4 = GanttChartDataGrid.Items[4] as CustomGanttChartItem;
            item4.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item4.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"));
            item4.CustomStart = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item4.CustomFinish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));

            CustomGanttChartItem item6 = GanttChartDataGrid.Items[6] as CustomGanttChartItem;
            item6.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item6.Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));
            item6.CustomStart = DateTime.Today.AddDays(+1).Add(TimeSpan.Parse("08:00:00"));
            item6.CustomFinish = DateTime.Today.AddDays(5).Add(TimeSpan.Parse("12:00:00"));

            CustomGanttChartItem item7 = GanttChartDataGrid.Items[7] as CustomGanttChartItem;
            item7.Start = DateTime.Today.AddDays(4);
            item7.IsMilestone = true;
            item7.Predecessors.Add(new PredecessorItem { Item = item4 });
            item7.Predecessors.Add(new PredecessorItem { Item = item6 });

            for (int i = 3; i <= 25; i++)
            {
                GanttChartDataGrid.Items.Add(
                    new CustomGanttChartItem
                    {
                        Content = "Task " + i,
                        Indentation = i % 3 == 0 ? 0 : 1,
                        Start = DateTime.Today.AddDays(i <= 8 ? (i - 4) * 2 : i - 8),
                        Finish = DateTime.Today.AddDays((i <= 8 ? (i - 4) * 2 + (i > 8 ? 6 : 1) : i - 2) + 2),
                        CustomStart = DateTime.Today.AddDays(i <= 8 ? (i - 4) * 2 : i - 8),
                        CustomFinish = DateTime.Today.AddDays((i <= 8 ? (i - 4) * 2 + (i > 8 ? 6 : 1) : i - 2) + 2)
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

        private void CustomBarThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            var item = thumb.DataContext as CustomGanttChartItem;
            var customStart = item.CustomStart;
            var customStartPosition = GanttChartDataGrid.GetPosition(customStart);
            var visibilitySchedule = GanttChartDataGrid.GetVisibilitySchedule();
            var customDuration = GanttChartDataGrid.GetEffort(item.CustomStart, item.CustomFinish, visibilitySchedule);
            customStartPosition += e.HorizontalChange;
            customStart = GanttChartDataGrid.GetDateTime(customStartPosition);
            item.CustomStart = customStart;
            item.CustomFinish = GanttChartDataGrid.GetFinish(customStart, customDuration, visibilitySchedule);

            var scrollViewer = GanttChartDataGrid.GanttChartView.ScrollViewer;
            var mousePosition = Mouse.GetPosition(scrollViewer);
            const double autoScrollMargin = 10;
            if (mousePosition.X < autoScrollMargin)
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - autoScrollMargin);
            else if (mousePosition.X > scrollViewer.ActualWidth - autoScrollMargin)
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + autoScrollMargin);
        }
    }
}
