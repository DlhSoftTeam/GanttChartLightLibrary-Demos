﻿using DlhSoft.Windows.Controls;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Demos.WPF.CSharp.GanttChartDataGrid.BarTemplating
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

            CustomGanttChartItem item0 = GanttChartDataGrid.Items[0] as CustomGanttChartItem;

            CustomGanttChartItem item1 = GanttChartDataGrid.Items[1] as CustomGanttChartItem;
            item1.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item1.Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00"));
            item1.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));
            item1.AssignmentsContent = "Resource 1";
            item1.Icon = new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Check.png", applicationName), UriKind.Absolute));
            item1.EstimatedStart = DateTime.Today.AddDays(-1).Add(TimeSpan.Parse("08:00:00"));
            item1.EstimatedFinish = DateTime.Today.AddDays(-1).Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem item2 = GanttChartDataGrid.Items[2] as CustomGanttChartItem;
            item2.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item2.Finish = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("16:00:00"));
            item2.AssignmentsContent = "Resource 1, Resource 2";
            item2.Predecessors.Add(new PredecessorItem { Item = item1 });
            item2.Icon = new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Person.png", applicationName), UriKind.Absolute));
            item2.Note = "This task is very important.";
            item2.EstimatedStart = DateTime.Today.AddDays(1 - 1).Add(TimeSpan.Parse("08:00:00"));
            item2.EstimatedFinish = DateTime.Today.AddDays(2 + 1).Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem item3 = GanttChartDataGrid.Items[3] as CustomGanttChartItem;
            item3.Predecessors.Add(new PredecessorItem { Item = item0, DependencyType = DependencyType.StartStart });

            CustomGanttChartItem item4 = GanttChartDataGrid.Items[4] as CustomGanttChartItem;
            item4.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item4.Finish = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"));
            item4.EstimatedStart = DateTime.Today.AddDays(+1).Add(TimeSpan.Parse("08:00:00"));
            item4.EstimatedFinish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));
            item4.Markers.Add(new Marker { DateTime = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("09:00:00")), Icon = new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Warning.png", applicationName), UriKind.Absolute)), Note = "Validation required" });
            item4.Markers.Add(new Marker { DateTime = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("14:00:00")), Icon = new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Error.png", applicationName), UriKind.Absolute)), Note = "Impossible to finish the task" });

            CustomGanttChartItem item5 = GanttChartDataGrid.Items[5] as CustomGanttChartItem;

            CustomGanttChartItem item6 = GanttChartDataGrid.Items[6] as CustomGanttChartItem;
            item6.Start = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("08:00:00"));
            item6.Finish = DateTime.Today.AddDays(8).Add(TimeSpan.Parse("12:00:00"));
            item6.EstimatedStart = DateTime.Today.AddDays(+1).Add(TimeSpan.Parse("08:00:00"));
            item6.EstimatedFinish = DateTime.Today.AddDays(8 - 1).Add(TimeSpan.Parse("12:00:00"));
            item6.Interruptions.Add(new Interruption { Start = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("14:00:00")), Finish = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("10:00:00")) });

            CustomGanttChartItem item7 = GanttChartDataGrid.Items[7] as CustomGanttChartItem;
            item7.Start = DateTime.Today.AddDays(5);
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
                        Start = DateTime.Today.AddDays(i <= 8 ? (i - 4) * 3 : i - 8),
                        Finish = DateTime.Today.AddDays((i <= 8 ? (i - 4) * 3 + (i > 8 ? 6 : 1) : i - 2) + 1 + 2),
                        CompletedFinish = DateTime.Today.AddDays(i <= 8 ? (i - 4) * 3 : i - 8).AddDays(i % 6 == 1 ? 3 : 0)
                    });
            }

            // Optionally, set up summary background brushes as well.
            item0.Background = Brushes.Blue;
            item3.Background = Brushes.Yellow;
            item5.Background = Brushes.Green;
            for (var i = 8; i <= 30; i += 3)
                (GanttChartDataGrid.Items[i] as CustomGanttChartItem).Background = i % 2 == 0 ? Brushes.Orange : Brushes.Brown;
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
