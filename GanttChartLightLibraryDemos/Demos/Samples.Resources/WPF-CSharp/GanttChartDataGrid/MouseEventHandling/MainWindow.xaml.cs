using System;
using System.Windows;
using DlhSoft.Windows.Controls;
using DlhSoft.Windows.Shapes;

namespace Demos.WPF.CSharp.GanttChartDataGrid.MouseEventHandling
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

            for (int i = 3; i <= 23; i++)
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

        private void GanttChartDataGrid_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point controlPosition = e.GetPosition(GanttChartDataGrid);
            Point contentPosition = e.GetPosition(GanttChartDataGrid.ChartContentElement);

            DateTime dateTime = GanttChartDataGrid.GetDateTime(contentPosition.X);
            GanttChartItem itemRow = GanttChartDataGrid.GetItemAt(contentPosition.Y);

            GanttChartItem item = null;
            PredecessorItem predecessorItem = null;
            FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
            if (frameworkElement != null)
            {
                item = frameworkElement.DataContext as GanttChartItem;
                DependencyArrowLine.LineSegment dependencyLine = frameworkElement.DataContext as DependencyArrowLine.LineSegment;
                if (dependencyLine != null)
                    predecessorItem = dependencyLine.Parent.DataContext as PredecessorItem;
            }

            if (controlPosition.X < GanttChartDataGrid.ActualWidth - GanttChartDataGrid.GanttChartView.ActualWidth)
                return;
            string message = String.Empty;
            if (controlPosition.Y < GanttChartDataGrid.HeaderHeight)
                message = string.Format("You have clicked the chart scale header at date and time {0:g}.", dateTime);
            else if (item != null && item.HasChildren)
                message = string.Format("You have clicked the summary task item '{0}' at date and time {1:g}.", item, dateTime < item.Start ? item.Start : (dateTime > item.Finish ? item.Finish : dateTime));
            else if (item != null && item.IsMilestone)
                message = string.Format("You have clicked the milestone task item '{0}' at date and time {1:g}.", item, item.Start);
            else if (item != null)
                message = string.Format("You have clicked the standard task item '{0}' at date and time {1:g}.", item, dateTime > item.Finish ? item.Finish : dateTime);
            else if (predecessorItem != null)
                message = string.Format("You have clicked the task dependency line between '{0}' and '{1}'.", predecessorItem.DependentItem, predecessorItem.Item);
            else if (itemRow != null)
                message = string.Format("You have clicked at date and time {0:g} within the row of item '{1}'.", dateTime, itemRow);
            else
                message = string.Format("You have clicked at date and time {0:g} within an empty area of the chart.", dateTime);

            NotificationsTextBox.AppendText(string.Format("{0}{1}", NotificationsTextBox.Text.Length > 0 ? "\n" : string.Empty, message));
            NotificationsTextBox.ScrollToEnd();
        }
    }
}
