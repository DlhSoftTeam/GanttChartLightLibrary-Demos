using System;
using System.Windows;
using DlhSoft.Windows.Controls;
using System.Collections.ObjectModel;
using DlhSoft.Windows.Data;
using System.Windows.Threading;
using System.Windows.Media;

namespace Demos.WPF.CSharp.ScheduleChartDataGrid.ShiftScheduling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            int engineerCount = 8, managerCount = 3;
            for (var i = 1; i <= engineerCount; i++)
                ScheduleChartDataGrid.Items.Add(new ScheduleChartItem { Content = "Engineer #" + i });
            for (var i = 1; i <= managerCount; i++)
                ScheduleChartDataGrid.Items.Add(new ScheduleChartItem { Content = "Manager #" + i });

            DateTime date = DateTime.Today;
            int year = DateTime.Today.Year, month = DateTime.Today.Month;

            DateTime timelinePageStart = new DateTime(year, month, 1);
            DateTime timelinePageFinish = timelinePageStart.AddDays(7);
            ScheduleChartDataGrid.SetTimelinePage(timelinePageStart, timelinePageFinish);
            ScheduleChartDataGrid.DisplayedTime = timelinePageStart;

            // Set up the actual shifts for engineers and managers (resource assignments).
            Brush engineerMorning = new SolidColorBrush(Color.FromArgb(128, 104, 168, 96));
            Brush engineerAfternoon = new SolidColorBrush(Color.FromArgb(128, 239, 156, 80));
            Brush engineerNight = new SolidColorBrush(Color.FromArgb(128, 80, 108, 164));
            Brush managerMorning = new SolidColorBrush(Color.FromArgb(32, 128, 128, 128));
            Brush managerAfternoon = new SolidColorBrush(Color.FromArgb(128, 128, 128, 128));
            Brush managerNight = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));

            for (int i = 0; i < engineerCount; i++)
            {
                ScheduleChartItem scheduleChartItem = ScheduleChartDataGrid.Items[i];
                for (DateTime d = timelinePageStart.Date.AddDays(-1).AddHours(23).AddHours((i % 4) * 8); d < timelinePageFinish; d = d.AddDays(1).AddHours(8))
                {
                    string shiftType = d.Hour <= 8 ? "morning" : (d.Hour <= 16 ? "afternoon" : "night");
                    GanttChartItem task = new GanttChartItem { Content = "Engineering " + shiftType + " shift", Start = d, Finish = d.AddHours(8) };
                    switch (shiftType)
                    {
                        case "morning":
                            GanttChartView.SetStandardBarFill(task, engineerMorning);
                            break;
                        case "afternoon":
                            GanttChartView.SetStandardBarFill(task, engineerAfternoon);
                            break;
                        case "night":
                            GanttChartView.SetStandardBarFill(task, engineerNight);
                            break;
                    }
                    scheduleChartItem.GanttChartItems.Add(task);
                }
            }

            for (int i = 0; i < managerCount; i++)
            {
                var scheduleChartItem = ScheduleChartDataGrid.Items[engineerCount + i];

                for (DateTime d = timelinePageStart.Date.AddDays(-1).AddHours(23).AddHours((i % 4) * 8); d < timelinePageFinish; d = d.AddDays(1).AddHours(8))
                {
                    string shiftType = d.Hour <= 8 ? "morning" : (d.Hour <= 16 ? "afternoon" : "night");
                    GanttChartItem task = new GanttChartItem { Content = "Management " + shiftType + " shift", Start = d, Finish = d.AddHours(8) };
                    switch (shiftType)
                    {
                        case "morning":
                            GanttChartView.SetStandardBarFill(task, managerMorning);
                            break;
                        case "afternoon":
                            GanttChartView.SetStandardBarFill(task, managerAfternoon);
                            break;
                        case "night":
                            GanttChartView.SetStandardBarFill(task, managerNight);
                            break;
                    }
                    scheduleChartItem.GanttChartItems.Add(task);
                }
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

            // Enlarge HeaderHeight to for 3 scales to fit (by default it is initialized for 2 scales).
            ScheduleChartDataGrid.HeaderHeight *= 3f / 2f;
        }
        private void LoadTheme()
        {
            if (theme == null || theme == "Default" || theme == "Aero")
                return;
            var themeResourceDictionary = new ResourceDictionary { Source = new Uri("/" + GetType().Assembly.GetName().Name + ";component/Themes/" + theme + ".xaml", UriKind.Relative) };
            ScheduleChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }

        private void ScheduleChartDataGrid_TimelinePageChanged(object sender, EventArgs e)
        {
            // Add custom time intervals.
            Dispatcher.BeginInvoke((Action)delegate
            {
                Scale hoursScale = ScheduleChartDataGrid.Scales[3];
                hoursScale.Intervals.Clear();
                for (DateTime dateTime = ScheduleChartDataGrid.TimelinePageStart.Date.AddDays(-1).AddHours(23); dateTime <= ScheduleChartDataGrid.TimelinePageFinish; dateTime = dateTime.AddHours(8))
                {
                    var start = dateTime;
                    if (start < ScheduleChartDataGrid.TimelinePageStart)
                        start = ScheduleChartDataGrid.TimelinePageStart;
                    hoursScale.Intervals.Add(new ScaleInterval(start, dateTime.AddHours(8)) { HeaderContent = dateTime.ToString("HH") });
                }
            },
            DispatcherPriority.Render);
        }
    }
}
