using System;
using System.Linq;
using System.Windows;
using DlhSoft.Windows.Controls;
using System.Windows.Controls;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Demos.WPF.CSharp.GanttChartDataGrid.ReadOnlyVisibilityBehavior
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

            GanttChartItem item7 = GanttChartDataGrid.Items[7];
            item7.Start = DateTime.Today.AddDays(4);
            item7.IsMilestone = true;
            item7.Predecessors.Add(new PredecessorItem { Item = item4 });
            item7.Predecessors.Add(new PredecessorItem { Item = item6 });

            for (int i = 3; i <= 25; i++)
            {
                GanttChartDataGrid.Items.Add(
                    new GanttChartItem
                    {
                        Content = "Task " + i,
                        Indentation = i % 3 == 0 ? 0 : 1,
                        Start = DateTime.Today.AddDays(i <= 8 ? (i - 4) * 2 : i - 8),
                        Finish = DateTime.Today.AddDays((i <= 8 ? (i - 4) * 2 + (i > 8 ? 6 : 1) : i - 2) + 2),
                        CompletedFinish = DateTime.Today.AddDays(i <= 8 ? (i - 4) * 2 : i - 8).AddDays(i % 6 == 4 ? 3 : 0)
                    });
            }

            GanttChartDataGrid.AssignableResources = new ObservableCollection<string> { "Resource 1", "Resource 2", "Resource 3" };
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

        private void ReadOnlyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.IsReadOnly = (ReadOnlyCheckBox.IsChecked == true);
        }

        private void GridReadOnlyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.DataTreeGrid.IsReadOnly = (GridReadOnlyCheckBox.IsChecked == true);
        }

        private void ChartReadOnlyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.GanttChartView.IsReadOnly = (ChartReadOnlyCheckBox.IsChecked == true);
        }

        private void SetReadOnlyStateForColumns(string[] columnNames, bool isReadOnly)
        {
            foreach (var column in GanttChartDataGrid.Columns.Where(c => columnNames.Contains(c.Header as string)))
                column.IsReadOnly = isReadOnly;
        }

        private void TaskNameColumnReadOnlyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            bool isReadOnly = (TaskNameColumnReadOnlyCheckBox.IsChecked == true);
            SetReadOnlyStateForColumns(new[] { "Task" }, isReadOnly);
        }

        private void SchedulingColumnsReadOnlyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            bool isReadOnly = (SchedulingColumnsReadOnlyCheckBox.IsChecked == true);
            SetReadOnlyStateForColumns(new[] { "Start", "Finish", "Milestone", "Compl." }, isReadOnly);
        }

        private void AssignmentsColumnReadOnlyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            bool isReadOnly = (AssignmentsColumnReadOnlyCheckBox.IsChecked == true);
            SetReadOnlyStateForColumns(new[] { "Assignments" }, isReadOnly);
        }

        private void EffortIsPreservedWhenChangingStartFromGridCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //..In chart effort is not preserved
            var columnStart = GanttChartDataGrid.Columns.Single(c => c.Header as string == "Start");
            var columnStartPreservingEffort = GanttChartDataGrid.Columns.Single(c => c.Header as string == "Start Preserving Effort");
            
            if (EffortIsPreservedWhenChangingStartFromGridCheckBox.IsChecked == true)
            {
                columnStart.Visibility = Visibility.Collapsed;
                columnStartPreservingEffort.Visibility = Visibility.Visible;
            }
            else
            {
                columnStart.Visibility = Visibility.Visible;
                columnStartPreservingEffort.Visibility = Visibility.Collapsed;
            }
        }

        private void StartReadOnlyInChartCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.IsTaskStartReadOnly = (StartReadOnlyInChartCheckBox.IsChecked == true);
        }

        private void CompletionReadOnlyInChartCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.IsTaskCompletionReadOnly = (CompletionReadOnlyInChartCheckBox.IsChecked == true);
        }

        private void DurationReadOnlyInChartCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.IsTaskFinishReadOnly = (DurationReadOnlyInChartCheckBox.IsChecked == true);
            if (GanttChartDataGrid.IsTaskFinishReadOnly)
                DisableUpdatingDurationByStartDraggingCheckBox.IsChecked = true;
        }

        private void DisableUpdatingDurationByStartDraggingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.IsDraggingTaskStartEndsEnabled = (DisableUpdatingDurationByStartDraggingCheckBox.IsChecked == false);
            if (GanttChartDataGrid.IsDraggingTaskStartEndsEnabled)
                DurationReadOnlyInChartCheckBox.IsChecked = false;
        }

        private void HideDependenciesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.AreTaskDependenciesVisible = (HideDependenciesCheckBox.IsChecked == false);
        }

        private void DisableCreatingStartDependenciesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.AllowCreatingStartDependencies = (DisableCreatingStartDependenciesCheckBox.IsChecked == false);
        }

        private void DisableCreatingToFinishDependenciesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.AllowCreatingToFinishDependencies = (DisableCreatingToFinishDependenciesCheckBox.IsChecked == false);
        }

        private void DisableChartScrollingOnGridRowClickingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.IsRowClickTimeScrollingEnabled = (DisableChartScrollingOnGridRowClickingCheckBox.IsChecked == false);
        }

        private void SetChartBarItemAsReadOnly_Click(object sender, RoutedEventArgs e)
        {
            List<GanttChartItem> items = new List<GanttChartItem>();
            foreach (GanttChartItem item in GanttChartDataGrid.GetSelectedItems())
                items.Add(item);
            if (items.Count <= 0)
            {
                MessageBox.Show("Please select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            foreach (GanttChartItem item in items)
            {
                item.IsBarReadOnly = true;
                GanttChartView.SetStandardBarFill(item, Brushes.Green);
                GanttChartView.SetStandardBarStroke(item, Brushes.Green);
                if (item.IsMilestone)
                {
                    GanttChartView.SetMilestoneBarFill(item, Brushes.YellowGreen);
                }
                if (item.IsSummaryEnabled)
                {
                    GanttChartView.SetSummaryBarFill(item, Brushes.DarkBlue);
                    GanttChartView.SetSummaryBarStroke(item, Brushes.DarkBlue);
                }
            }
        }
    }
}
