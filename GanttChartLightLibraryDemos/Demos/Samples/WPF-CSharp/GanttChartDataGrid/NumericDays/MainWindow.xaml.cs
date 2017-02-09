using System;
using System.Windows;
using DlhSoft.Windows.Controls;
using System.Windows.Threading;

namespace Demos.WPF.CSharp.GanttChartDataGrid.NumericDays
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
            // Use the numeric day origin (defined as a static value) for date and time values of Gantt Chart items.
            item1.Start = NumericDayOrigin;
            item1.Finish = NumericDayOrigin.AddDays(1);
            item1.CompletedFinish = NumericDayOrigin.AddDays(1);
            item1.AssignmentsContent = "Resource 1";

            GanttChartItem item2 = GanttChartDataGrid.Items[2];
            item2.Start = NumericDayOrigin.AddDays(1);
            item2.Finish = NumericDayOrigin.AddDays(3);
            item2.CompletedFinish = NumericDayOrigin.AddDays(1);
            item2.AssignmentsContent = "Resource 1, Resource 2";
            item2.Predecessors.Add(new PredecessorItem { Item = item1 });

            GanttChartItem item3 = GanttChartDataGrid.Items[3];
            item3.Predecessors.Add(new PredecessorItem { Item = item0, DependencyType = DependencyType.StartStart });

            GanttChartItem item4 = GanttChartDataGrid.Items[4];
            item4.Start = NumericDayOrigin;
            item4.Finish = NumericDayOrigin.AddDays(3);
            item4.CompletedFinish = NumericDayOrigin;

            GanttChartItem item6 = GanttChartDataGrid.Items[6];
            item6.Start = NumericDayOrigin;
            item6.Finish = NumericDayOrigin.AddDays(3);
            item6.CompletedFinish = NumericDayOrigin;

            GanttChartItem item7 = GanttChartDataGrid.Items[7];
            item7.Start = NumericDayOrigin.AddDays(4);
            item7.IsMilestone = true;
            item7.Predecessors.Add(new PredecessorItem { Item = item4 });
            item7.Predecessors.Add(new PredecessorItem { Item = item6 });

            GanttChartItem item8 = GanttChartDataGrid.Items[6];
            item8.Start = NumericDayOrigin;
            item8.Finish = NumericDayOrigin.AddDays(3);
            item8.CompletedFinish = NumericDayOrigin;

            GanttChartItem item9 = GanttChartDataGrid.Items[6];
            item9.Start = NumericDayOrigin;
            item9.Finish = NumericDayOrigin.AddDays(3);
            item9.CompletedFinish = NumericDayOrigin;

            GanttChartItem item10 = GanttChartDataGrid.Items[6];
            item10.Start = NumericDayOrigin;
            item10.Finish = NumericDayOrigin.AddDays(3);
            item10.CompletedFinish = NumericDayOrigin;

            for (int i = 6; i <= 25; i++)
            {
                GanttChartDataGrid.Items.Add(
                    new GanttChartItem
                    {
                        Content = "Task " + i,
                        Indentation = i % 3 == 0 ? 0 : 1,
                        Start = NumericDayOrigin.AddDays(i <= 8 ? (i - 4) * 3 : i - 8),
                        Finish = NumericDayOrigin.AddDays((i <= 8 ? (i - 4) * 3 + (i > 8 ? 6 : 1) : i - 2) + 3),
                        CompletedFinish = NumericDayOrigin.AddDays(i <= 8 ? (i - 4) * 3 : i - 8).AddDays(i % 6 == 1 ? 3 : 0)
                    });
            }

            // Set timeline page start and displayed time to the numeric day origin.
            GanttChartDataGrid.SetTimelinePage(NumericDayOrigin, NumericDayOrigin.AddDays(45));
            GanttChartDataGrid.DisplayedTime = NumericDayOrigin;
        }

        private ResourceDictionary themeResourceDictionary;
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
            themeResourceDictionary = new ResourceDictionary { Source = new Uri("/" + GetType().Assembly.GetName().Name + ";component/Themes/" + theme + ".xaml", UriKind.Relative) };
            GanttChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }

        private static DateTime NumericDayOrigin { get { return NumericDayStringConverter.Origin; } }

        private void GanttChartDataGrid_TimelinePageChanged(object sender, EventArgs e)
        {
            // Use Dispatcher.BeginInvoke in order to ensure that scale objects and their interval header items are properly created before setting their HeaderContent values.
            // Use DispatcherPriority.Render to apply the changes when rendering the view.
            Dispatcher.BeginInvoke((Action)delegate
            {
                if (GanttChartDataGrid.Scales.Count <= 2)
                    return;
                // Scales use one based indexes because a special scale (non working highlighting) is inserted at position zero during control initialization (behind the scenes).
                Scale weekScale = GanttChartDataGrid.Scales[1];
                foreach (ScaleInterval i in weekScale.Intervals)
                    i.HeaderContent = i.TimeInterval.Start.Date >= NumericDayOrigin ? string.Format("Week {0}", (int)(i.TimeInterval.Start.Date - NumericDayOrigin).TotalDays / 7 + 1) : string.Empty;
                Scale dayScale = GanttChartDataGrid.Scales[2];
                foreach (ScaleInterval i in dayScale.Intervals)
                    i.HeaderContent = i.TimeInterval.Start.Date >= NumericDayOrigin ? string.Format("{0:00}", ((int)(i.TimeInterval.Start.Date - NumericDayOrigin).TotalDays + 1) % 100) : string.Empty;
            },
            DispatcherPriority.Render);
        }
    }
}
