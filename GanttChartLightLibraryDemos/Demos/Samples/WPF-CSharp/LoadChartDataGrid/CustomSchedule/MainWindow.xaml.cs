using System;
using System.Windows;
using DlhSoft.Windows.Controls;
using DlhSoft.Windows.Data;

namespace Demos.WPF.CSharp.LoadChartDataGrid.CustomSchedule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadChartDataGrid.TimelinePageStart = DateTime.Today;
            LoadChartDataGrid.TimelinePageFinish = DateTime.Today.AddMonths(2);
            LoadChartDataGrid.VisibleDayStart = TimeOfDay.Parse("09:00:00");
            LoadChartDataGrid.VisibleDayFinish = TimeOfDay.Parse("15:00:00");

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
    }
}
