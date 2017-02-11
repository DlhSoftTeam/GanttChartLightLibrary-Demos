using System;
using System.Windows;
using System.Windows.Media.Imaging;
using DlhSoft.Windows.Controls;

namespace Demos.WPF.CSharp.ScheduleChartDataGrid.BarTemplating
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string applicationName = GetType().Namespace;

            CustomGanttChartItem task1 = ScheduleChartDataGrid.Items[0].GanttChartItems[0] as CustomGanttChartItem;
            task1.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            task1.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            task1.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));
            task1.Icon = new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Check.png", applicationName), UriKind.Absolute));
            task1.EstimatedStart = DateTime.Today.AddDays(-1).Add(TimeSpan.Parse("08:00:00"));
            task1.EstimatedFinish = DateTime.Today.AddDays(-1).Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem task21 = ScheduleChartDataGrid.Items[0].GanttChartItems[1] as CustomGanttChartItem;
            task21.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            task21.Finish = DateTime.Today.AddDays(5).Add(TimeSpan.Parse("16:00:00"));
            task21.AssignmentsContent = "50%";
            task21.Icon = new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Person.png", applicationName), UriKind.Absolute));
            task21.Markers.Add(new Marker { DateTime = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("09:00:00")), Icon = new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Warning.png", applicationName), UriKind.Absolute)), Note = "Validation required for Task 2" });
            task21.Markers.Add(new Marker { DateTime = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("14:00:00")), Icon = new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Error.png", applicationName), UriKind.Absolute)), Note = "Impossible to finish Task 2" });

            CustomGanttChartItem task22 = ScheduleChartDataGrid.Items[1].GanttChartItems[0] as CustomGanttChartItem;
            task22.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            task22.Finish = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("16:00:00"));
            task22.Note = "This assignment is very important.";
            task22.EstimatedStart = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            task22.EstimatedFinish = DateTime.Today.AddDays(4 - 2).Add(TimeSpan.Parse("12:00:00"));
            task22.Interruptions.Add(new Interruption { Start = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("10:00:00")), Finish = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("13:00:00")) });

            for (int i = 3; i <= 16; i++)
            {
                ScheduleChartItem item = new ScheduleChartItem { Content = "Resource " + i };
                for (int j = 1; j <= (i - 1) % 4 + 1; j++)
                {
                    item.GanttChartItems.Add(new CustomGanttChartItem
                    {
                        Content = "Task " + i + "." + j,
                        Start = DateTime.Today.AddDays(i + (i - 1) * (j - 1)),
                        Finish = DateTime.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1),
                        CompletedFinish = DateTime.Today.AddDays(i + (i - 1) * (j - 1)).AddDays((i + j) % 5 == 2 ? 2 : 0)
                    });
                }
                ScheduleChartDataGrid.Items.Add(item);
            }
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
            ScheduleChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }
    }
}
