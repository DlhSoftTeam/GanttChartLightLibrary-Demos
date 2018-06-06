using System;
using System.Windows;
using DlhSoft.Windows.Controls;

namespace Demos.WPF.CSharp.GanttChartDataGrid.Baseline
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
            item6.BaselineStart = item6.Start;
            item6.BaselineFinish = item6.Finish;

            GanttChartItem item7 = GanttChartDataGrid.Items[7];
            item7.Start = DateTime.Today.AddDays(4);
            item7.IsMilestone = true;
            item7.Predecessors.Add(new PredecessorItem { Item = item4 });
            item7.Predecessors.Add(new PredecessorItem { Item = item6 });
            item7.BaselineStart = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));

            GanttChartItem item8 = GanttChartDataGrid.Items[8];
            item8.Start = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));
            item8.Finish = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("14:00:00"));
            item8.AssignmentsContent = "Resource 1 [50%], Resource 2 [75%]";
            item8.BaselineStart = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));
            item8.BaselineFinish = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("14:00:00"));

            GanttChartItem item9 = GanttChartDataGrid.Items[9];
            item9.Start = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("08:00:00"));
            item9.Finish = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"));
            item9.AssignmentsContent = "Resource 1";
            item9.BaselineStart = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"));
            item9.BaselineFinish = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"));

            int start;
            int finish;
            for (int i = 6; i <= 25; i++)
            {
                start = i <= 8 ? (i - 4) * 2 : i - 8;
                finish = (i <= 8 ? (i - 4) * 2 + (i > 8 ? 6 : 1) : i - 2) + 2;
                GanttChartDataGrid.Items.Add(
                    new GanttChartItem
                    {
                        Content = "Task " + i,
                        Indentation = i % 3 == 0 ? 0 : 1,
                        Start = DateTime.Today.AddDays(start),
                        Finish = DateTime.Today.AddDays(finish),
                        CompletedFinish = DateTime.Today.AddDays(i <= 8 ? (i - 4) * 2 : i - 8).AddDays(i % 6 == 4 ? 3 : 0),
                        BaselineStart = DateTime.Today.AddDays(start - 2),
                        BaselineFinish = DateTime.Today.AddDays(finish - 2)
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
    }
}
