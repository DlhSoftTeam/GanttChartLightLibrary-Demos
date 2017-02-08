using System;
using DlhSoft.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Linq;

namespace Demos.WPF.CSharp.PertChartView.MultiTasksPerLine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            Tasks = new ObservableCollection<GanttChartItem>
            {
                new GanttChartItem { Content = "Task 1", Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00")) },
                new GanttChartItem { Content = "Task 2", Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00")) },
                new GanttChartItem { Content = "Milestone", Start = DateTime.Today.Add(TimeSpan.Parse("16:00:00")), IsMilestone = true },
                new GanttChartItem { Content = "Task 3", Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00")), Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00")) },
                new GanttChartItem { Content = "Task 4", Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00")), Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00")) }
            };
            Tasks[2].Predecessors.Add(new PredecessorItem { Item = Tasks[0] });
            Tasks[2].Predecessors.Add(new PredecessorItem { Item = Tasks[1] });
            Tasks[3].Predecessors.Add(new PredecessorItem { Item = Tasks[2] });
            Tasks[4].Predecessors.Add(new PredecessorItem { Item = Tasks[2] });
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
            PertChartView.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }

        public ObservableCollection<GanttChartItem> Tasks { get; set; }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabControl.SelectedItem == PertChartTabItem)
            {
                // Get PERT Chart items from Gantt Chart. The collection may contain generic links, i.e. virtual effort tasks.
                var taskEvents = GanttChartDataGrid.GetPertChartItems();
                OptimizeTasks(taskEvents); // Comment this line to see default behavior of DlhSoft Gantt Chart Light Library components.
                PertChartView.Items = taskEvents;
            }
        }

        // Optimize tasks between task events, by removing generic links and replacing them by multiple dependencies between the same two task events as appropriate.
        private static void OptimizeTasks(ObservableCollection<DlhSoft.Windows.Controls.Pert.PertChartItem> taskEvents)
        {
            foreach (var taskEvent in taskEvents.Where(te => te.Predecessors != null).ToArray())
            {
                var tasks = taskEvent.Predecessors;

                // When a task event has only virtual effort links to other events, link the previous events directly to the current event.
                if (tasks.Any() && tasks.All(t => t.IsEffortVirtual))
                {
                    var previousTaskEvents = tasks.Select(t => t.Item).ToArray();
                    var previousTasks = previousTaskEvents.SelectMany(pte => pte.Predecessors).ToArray();
                    foreach (var pte in previousTaskEvents)
                        taskEvents.Remove(pte);
                    taskEvent.Predecessors.Clear();

                    for (var i = 0; i < previousTasks.Length; i++)
                    {
                        var pt = previousTasks[i];
                        taskEvent.Predecessors.Add(pt);

                        // Set line index values to dependency lines sharing the same start and end events to be able to compute points to be used when displaying polygonal dependency lines accordingly.
                        TaskEventExtensions.SetLineIndex(pt, i);

                        // Whenever dependency line points are computed being required in the UI, we'll update them accordingly, inserting intermediary points to respect line indexes using vertical positioning.
                        DependencyPropertyDescriptor computedDependencyLinePointsPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(DlhSoft.Windows.Controls.Pert.PredecessorItem.ComputedDependencyLinePointsProperty, typeof(DlhSoft.Windows.Controls.Pert.PredecessorItem));
                        computedDependencyLinePointsPropertyDescriptor.AddValueChanged(pt, (sender, e) =>
                        {
                            var task = sender as DlhSoft.Windows.Controls.Pert.PredecessorItem;
                            var points = task.ComputedDependencyLinePoints;
                            if (points.Count < 2)
                                return;
                            Point fp = points.First(), lp = points.Last();
                            double width = lp.X - fp.X;
                            points.Insert(1, new Point(fp.X + width * DistanceRateToIntermediaryPoints, fp.Y + TaskEventExtensions.GetLineIndex(pt) * DistanceBetweenLines));
                            points.Insert(points.Count - 1, new Point(lp.X - width * DistanceRateToIntermediaryPoints, fp.Y + TaskEventExtensions.GetLineIndex(pt) * DistanceBetweenLines));
                        });
                    }
                }
            }
        }

        public const double DistanceBetweenLines = 30;
        public const double DistanceRateToIntermediaryPoints = 0.06;
    }

    // Allows storing line index values for predecessor items (dependency lines).
    public static class TaskEventExtensions
    {
        public static int GetLineIndex(DependencyObject obj)
        {
            return (int)obj.GetValue(LineIndexProperty);
        }

        public static void SetLineIndex(DependencyObject obj, int value)
        {
            obj.SetValue(LineIndexProperty, value);
        }

        public static readonly DependencyProperty LineIndexProperty =
            DependencyProperty.RegisterAttached("LineIndex", typeof(int), typeof(TaskEventExtensions), new PropertyMetadata(0));
    }
}
