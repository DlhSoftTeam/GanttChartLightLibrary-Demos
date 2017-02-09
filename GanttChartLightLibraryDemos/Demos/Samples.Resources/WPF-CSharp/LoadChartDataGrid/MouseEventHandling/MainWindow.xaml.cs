using System;
using System.Windows;
using DlhSoft.Windows.Controls;

namespace Demos.WPF.CSharp.LoadChartDataGrid.MouseEventHandling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AllocationItem allocation11 = LoadChartDataGrid.Items[0].GanttChartItems[0];
            allocation11.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            allocation11.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));

            AllocationItem allocation112 = LoadChartDataGrid.Items[0].GanttChartItems[1];
            allocation112.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            allocation112.Finish = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            allocation112.Units = 1.5;

            AllocationItem allocation12 = LoadChartDataGrid.Items[0].GanttChartItems[2];
            allocation12.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            allocation12.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            allocation12.Units = 0.5;

            AllocationItem allocation13 = LoadChartDataGrid.Items[0].GanttChartItems[3];
            allocation13.Start = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("08:00:00"));
            allocation13.Finish = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("16:00:00"));

            AllocationItem allocation22 = LoadChartDataGrid.Items[1].GanttChartItems[0];
            allocation22.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            allocation22.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));

            for (int i = 3; i <= 16; i++)
            {
                LoadChartItem item = new LoadChartItem { Content = "Resource " + i };
                for (int j = 1; j <= (i - 1) % 4 + 1; j++)
                {
                    item.GanttChartItems.Add(
                        new AllocationItem
                        {
                            Content = "Task " + i + "." + j + (((i + j) % 2 == 1 ? " [200%]" : string.Empty)),
                            Start = DateTime.Today.AddDays(i + (i - 1) * (j - 1)),
                            Finish = DateTime.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1),
                            Units = 1 + (i + j) % 2
                        });
                }
                LoadChartDataGrid.Items.Add(item);
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
            LoadChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }

        private void LoadChartDataGrid_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point controlPosition = e.GetPosition(LoadChartDataGrid);
            Point contentPosition = e.GetPosition(LoadChartDataGrid.ChartContentElement);

            DateTime dateTime = LoadChartDataGrid.GetDateTime(contentPosition.X);
            LoadChartItem itemRow = LoadChartDataGrid.GetItemAt(contentPosition.Y) as LoadChartItem;

            AllocationItem item = null;
            FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
            if (frameworkElement != null)
                item = frameworkElement.DataContext as AllocationItem;

            if (controlPosition.X < LoadChartDataGrid.ActualWidth - LoadChartDataGrid.GanttChartView.ActualWidth)
                return;
            string message = String.Empty;
            if (controlPosition.Y < LoadChartDataGrid.HeaderHeight)
                message = string.Format("You have clicked the chart scale header at date and time {0:g}.", dateTime);
            else if (item != null)
                message = string.Format("You have clicked the allocation item '{0}' of resource item '#{1}' at date and time {2:g}.", item, itemRow.ActualDisplayRowIndex + 1, dateTime > item.Finish ? item.Finish : dateTime);
            else if (itemRow != null)
                message = string.Format("You have clicked at date and time {0:g} within the row of item '#{1}'.", dateTime, itemRow.ActualDisplayRowIndex + 1);
            else
                message = string.Format("You have clicked at date and time {0:g} within an empty area of the chart.", dateTime);

            NotificationsTextBox.AppendText(string.Format("{0}{1}", NotificationsTextBox.Text.Length > 0 ? "\n" : string.Empty, message));
            NotificationsTextBox.ScrollToEnd();
        }
    }
}
