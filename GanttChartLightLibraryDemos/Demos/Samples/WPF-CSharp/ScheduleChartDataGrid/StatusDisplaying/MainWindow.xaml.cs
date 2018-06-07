using System;
using System.Windows;
using DlhSoft.Windows.Controls;
using System.Windows.Media;

namespace Demos.WPF.CSharp.ScheduleChartDataGrid.StatusDisplaying
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DateTime date = DateTime.Today;
            int year = date.Year, month = date.Month;

            Brush startedStatusBar = Brushes.YellowGreen;
            Brush issuesStatusBar = Brushes.Red;
            Brush maintenanceStatusBar = Brushes.Orange;

            ScheduleChartItem resource1 = new ScheduleChartItem { Content = "Resource 1" };

            GanttChartItem r1S1 = new GanttChartItem { Content = "Resource 1: started", Start = new DateTime(year, month, 2, 8, 0, 0), Finish = new DateTime(year, month, 8, 16, 0, 0) };
            GanttChartView.SetStandardBarFill(r1S1, startedStatusBar);
            resource1.GanttChartItems.Add(r1S1);

            GanttChartItem r1M1 = new GanttChartItem { Content = "Resource 1: maintenance", Start = new DateTime(year, month, 9, 8, 0, 0), Finish = new DateTime(year, month, 9, 16, 0, 0) };
            GanttChartView.SetStandardBarFill(r1M1, maintenanceStatusBar);
            resource1.GanttChartItems.Add(r1M1);

            GanttChartItem r1S2 = new GanttChartItem { Content = "Resource 1: started", Start = new DateTime(year, month, 10, 8, 0, 0), Finish = new DateTime(year, month, 13, 16, 0, 0) };
            GanttChartView.SetStandardBarFill(r1S2, startedStatusBar);
            resource1.GanttChartItems.Add(r1S2);

            GanttChartItem r1I1 = new GanttChartItem { Content = "Resource 1: issues", Start = new DateTime(year, month, 14, 8, 0, 0), Finish = new DateTime(year, month, 14, 16, 0, 0) };
            GanttChartView.SetStandardBarFill(r1I1, issuesStatusBar);
            resource1.GanttChartItems.Add(r1I1);

            GanttChartItem r1M2 = new GanttChartItem { Content = "Resource 1: maintenance", Start = new DateTime(year, month, 15, 8, 0, 0), Finish = new DateTime(year, month, 16, 16, 0, 0) };
            GanttChartView.SetStandardBarFill(r1M2, maintenanceStatusBar);
            resource1.GanttChartItems.Add(r1M2);

            GanttChartItem r1S3 = new GanttChartItem { Content = "Resource 1: started", Start = new DateTime(year, month, 16, 8, 0, 0), Finish = new DateTime(year, month, 22, 16, 0, 0) };
            GanttChartView.SetStandardBarFill(r1S3, startedStatusBar);
            resource1.GanttChartItems.Add(r1S3);

            ScheduleChartDataGrid.Items.Add(resource1);

            ScheduleChartItem resource2 = new ScheduleChartItem { Content = "Resource 2" };

            GanttChartItem r2S1 = new GanttChartItem { Content = "Resource 2: started", Start = new DateTime(year, month, 2, 8, 0, 0), Finish = new DateTime(year, month, 8, 16, 0, 0) };
            GanttChartView.SetStandardBarFill(r2S1, startedStatusBar);
            resource2.GanttChartItems.Add(r2S1);

            GanttChartItem r2I1 = new GanttChartItem { Content = "Resource 2: issues", Start = new DateTime(year, month, 9, 8, 0, 0), Finish = new DateTime(year, month, 12, 16, 0, 0) };
            GanttChartView.SetStandardBarFill(r2I1, issuesStatusBar);
            resource2.GanttChartItems.Add(r2I1);

            GanttChartItem r2M1 = new GanttChartItem { Content = "Resource 2: maintenance", Start = new DateTime(year, month, 13, 8, 0, 0), Finish = new DateTime(year, month, 14, 16, 0, 0) };
            GanttChartView.SetStandardBarFill(r2M1, maintenanceStatusBar);
            resource2.GanttChartItems.Add(r2M1);

            GanttChartItem r2S2 = new GanttChartItem { Content = "Resource 2: started", Start = new DateTime(year, month, 15, 8, 0, 0), Finish = new DateTime(year, month, 22, 16, 0, 0) };
            GanttChartView.SetStandardBarFill(r2S2, startedStatusBar);
            resource2.GanttChartItems.Add(r2S2);

            ScheduleChartDataGrid.Items.Add(resource2);

            ScheduleChartItem resource3 = new ScheduleChartItem { Content = "Resource 3" };
            GanttChartItem r3S1 = new GanttChartItem { Content = "Resource 3: started", Start = new DateTime(year, month, 2, 8, 0, 0), Finish = new DateTime(year, month, 22, 16, 0, 0) };
            GanttChartView.SetStandardBarFill(r3S1, startedStatusBar);
            resource3.GanttChartItems.Add(r3S1);

            ScheduleChartDataGrid.Items.Add(resource3);

            for (int i = 4; i <= 16; i++)
            {
                ScheduleChartItem item = new ScheduleChartItem { Content = "Resource " + i };

                GanttChartItem ts1 = new GanttChartItem { Content = "Resource " + i + ": started", Start = new DateTime(year, month, 2, 8, 0, 0), Finish = new DateTime(year, month, 5, 16, 0, 0) };
                GanttChartView.SetStandardBarFill(ts1, startedStatusBar);
                item.GanttChartItems.Add(ts1);

                GanttChartItem ti1 = new GanttChartItem { Content = "Resource " + i + ": issues", Start = new DateTime(year, month, 6, 8, 0, 0), Finish = new DateTime(year, month, 6, 16, 0, 0) };
                GanttChartView.SetStandardBarFill(ti1, issuesStatusBar);
                item.GanttChartItems.Add(ti1);

                GanttChartItem tm1 = new GanttChartItem { Content = "Resource " + i + ": maintenance", Start = new DateTime(year, month, 7, 8, 0, 0), Finish = new DateTime(year, month, 7, 16, 0, 0) };
                GanttChartView.SetStandardBarFill(tm1, maintenanceStatusBar);
                item.GanttChartItems.Add(tm1);

                GanttChartItem ts2 = new GanttChartItem { Content = "Resource " + i + ": started", Start = new DateTime(year, month, 8, 8, 0, 0), Finish = new DateTime(year, month, 15, 16, 0, 0) };
                GanttChartView.SetStandardBarFill(ts2, startedStatusBar);
                item.GanttChartItems.Add(ts2);

                GanttChartItem ti2 = new GanttChartItem { Content = "Resource " + i + ": issues", Start = new DateTime(year, month, 16, 8, 0, 0), Finish = new DateTime(year, month, 16, 16, 0, 0) };
                GanttChartView.SetStandardBarFill(ti2, issuesStatusBar);
                item.GanttChartItems.Add(ti2);

                GanttChartItem ts3 = new GanttChartItem { Content = "Resource " + i + ": started", Start = new DateTime(year, month, 17, 8, 0, 0), Finish = new DateTime(year, month, 22, 16, 0, 0) };
                GanttChartView.SetStandardBarFill(ts3, startedStatusBar);
                item.GanttChartItems.Add(ts3);

                ScheduleChartDataGrid.Items.Add(item);
            }

            ScheduleChartDataGrid.DisplayedTime = new DateTime(year, month, 1);
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
            ScheduleChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }
    }
}
