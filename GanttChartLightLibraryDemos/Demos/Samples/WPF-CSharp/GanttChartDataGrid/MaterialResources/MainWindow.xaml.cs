using System;
using System.Windows;
using DlhSoft.Windows.Controls;
using System.Windows.Threading;
using DlhSoft.Windows.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Demos.WPF.CSharp.GanttChartDataGrid.MaterialResources
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);

            for (int i = 1; i <= 16; i++)
            {
                GanttChartDataGrid.Items.Add(
                    new GanttChartItem
                    {
                        Content = "Print job #" + i,
                        Start = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 8, 0, 0)
                    });
            }

            // Set up resources.
            GanttChartDataGrid.AssignableResources = new ObservableCollection<string> { "Printer", "Paper", "Supervisor" };
            GanttChartDataGrid.ResourceQuantities = new Dictionary<string, double> { { "Printer", 5 }, { "Paper", double.PositiveInfinity }, { "Supervisor", 2} };
            // Define printing cost for 100 sheets of paper (default quantity used for cost by design).
            GanttChartDataGrid.SpecificResourceHourCosts = new Dictionary<string, double> { { "Paper", 5 } };

            // Assign a printer, the number of pages to pront on each print job, and part of the time of a supervisor needed to overview the printing jobs.
            // Update finish times of the task to based on their estimated durations, considering this ratio: 15 sheets of paper per minute.
            var sheetsOfPaperRequiredForPrintJobs = new decimal[] { 50, 20, 30, 60, 25, 10, 30, 50, 60, 80, 100, 25, 30, 30, 120, 80 };
            for (int i = 0; i < GanttChartDataGrid.Items.Count; i++)
            {
                var requiredSheetsOfPaper = sheetsOfPaperRequiredForPrintJobs[i];
                GanttChartDataGrid.Items[i].AssignmentsContent = "Printer, Paper [" + requiredSheetsOfPaper + "%], Supervisor [50%]";
                GanttChartDataGrid.Items[i].Finish = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 8, Convert.ToInt16(Math.Ceiling(requiredSheetsOfPaper / 15)), 0);
            }

            // Set working and visible time to 24 hours/day and 7 day/week, and nonworking time as not highlighted (as there is only working time).
            GanttChartDataGrid.WorkingDayStart = GanttChartDataGrid.VisibleDayStart = TimeOfDay.MinValue;
            GanttChartDataGrid.WorkingDayFinish = GanttChartDataGrid.VisibleDayFinish = TimeOfDay.MaxValue;
            GanttChartDataGrid.WorkingWeekStart = GanttChartDataGrid.VisibleWeekStart = DayOfWeek.Sunday;
            GanttChartDataGrid.WorkingWeekFinish = GanttChartDataGrid.VisibleWeekFinish = DayOfWeek.Saturday;
            GanttChartDataGrid.IsNonworkingTimeHighlighted = false;

            // Set timeline page start and displayed time.
            GanttChartDataGrid.SetTimelinePage(dateTime.AddHours(-1), dateTime.AddHours(2));
            GanttChartDataGrid.DisplayedTime = dateTime.AddMinutes(-dateTime.Minute);

            // Setup scales.
            Scale hourQuarterScale = GanttChartDataGrid.GetScale(0);
            hourQuarterScale.Intervals.Clear();
            for (dateTime = GanttChartDataGrid.TimelinePageStart; dateTime <= GanttChartDataGrid.TimelinePageFinish; dateTime = dateTime.AddMinutes(15))
                hourQuarterScale.Intervals.Add(new ScaleInterval(dateTime, dateTime.AddMinutes(15)) { HeaderContent = dateTime.ToString("g") });

            Scale minuteScale = GanttChartDataGrid.GetScale(1);
            minuteScale.Intervals.Clear();
            for (dateTime = GanttChartDataGrid.TimelinePageStart; dateTime <= GanttChartDataGrid.TimelinePageFinish; dateTime = dateTime.AddMinutes(3))
                minuteScale.Intervals.Add(new ScaleInterval(dateTime, dateTime.AddMinutes(3)) { HeaderContent = dateTime.ToString("mm") });
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

        private void LevelResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.LevelResources(DateTime.Today.AddHours(8));
        }
    }
}
