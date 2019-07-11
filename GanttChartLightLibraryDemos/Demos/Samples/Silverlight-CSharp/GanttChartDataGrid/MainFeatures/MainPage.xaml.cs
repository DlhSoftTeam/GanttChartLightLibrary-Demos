using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DlhSoft.Windows.Controls;
using DlhSoft.Windows.Data;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.IO;

namespace GanttChartDataGridSample
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            // Create and set the assignable resource collection as page resource before the InitializeComponent call in order to be able to refer it from XAML using the StaticResource markup extension.
            var assignableResources = new ObservableCollection<string>();
            Resources.Add("AssignableResources", assignableResources);

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
            // Important note: CompletedFinish value defaults to DateTime.Today, therefore you should always set it to a Start (or a value between Start and Finish) when you initialize a past task item! In this example we don't set it as the task is in the future.
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

            // You may uncomment the next lines of code to set default schedule and timeline visibility settings for the Gantt Chart.
            // // Working week: between Tuesday and Saturday.
            // GanttChartDataGrid.WorkingWeekStart = DayOfWeek.Tuesday;
            // GanttChartDataGrid.WorkingWeekFinish = DayOfWeek.Saturday;
            // // Working day: between 9 AM and 5 PM.
            // GanttChartDataGrid.VisibleDayStart = GanttChartDataGrid.WorkingDayStart = TimeOfDay.Parse("09:00:00");
            // GanttChartDataGrid.VisibleDayFinish = GanttChartDataGrid.WorkingDayFinish = TimeOfDay.Parse("17:00:00");
            // // Optionally, generic nonworking intervals.
            // GanttChartDataGrid.NonworkingIntervals = new ObservableCollection<TimeInterval>(
            //    new TimeInterval[] {
            //        new TimeInterval(DateTime.Today.AddDays(1), DateTime.Today.AddDays(1).Add(TimeOfDay.MaxValue)), // Holiday: full day.
            //        new TimeInterval(DateTime.Today.AddDays(3), DateTime.Today.AddDays(5).Add(TimeSpan.Parse("12:00:00"))) // Custom time interval off: full and partial day accepted.
            //    });
            // // Optionally, specific nonworking intervals based on date parameter: recurrent breaks and holidays accepted.
            // GanttChartDataGrid.NonworkingDayIntervalProvider = (date) => { 
            //    if (date.Day % 3 == 0) // First recurrence expression: on the end of every set of three days in the month.
            //        return new DayTimeInterval[] { 
            //            new DayTimeInterval(TimeOfDay.MinValue, TimeOfDay.Parse("12:00:00")), // Large interval off: first part of day.
            //            new DayTimeInterval(TimeOfDay.Parse("12:30:00"), TimeOfDay.Parse("13:00:00")) // Short break: fast lunch time.
            //        };
            //    else if (date.DayOfWeek != DayOfWeek.Wednesday) // Second recurrence expression: every day except Wednesdays.
            //        return new DayTimeInterval[] { 
            //            new DayTimeInterval(TimeOfDay.Parse("12:00:00"), TimeOfDay.Parse("13:00:00")) // Break: regular lunch time.
            //        };
            //    return null; // Otherwise use regular timing only.
            // };
            // // Alternatively, add working day time breaks using AddWorkingDayTimeBreak or AddWorkingDayTimeBreaks methods.
            // GanttChartDataGrid.AddWorkingDayTimeBreak(TimeOfDay.Parse("11:30"), TimeOfDay.Parse("12:30"));

            // You may uncomment the next lines of code to set specific schedule settings for Task 3.
            // item8.Schedule = new Schedule(
            //    DayOfWeek.Sunday, DayOfWeek.Thursday, // Working week: between Sunday and Thursday.
            //    TimeSpan.Parse("10:00:00"), TimeSpan.Parse("14:00:00"), // Working day: between 10 AM and 2 PM.
            //    new TimeInterval[] { // Optionally, generic nonworking intervals.
            //        new TimeInterval(DateTime.Today.AddDays(4), DateTime.Today.AddDays(4).Add(TimeOfDay.MaxValue)), // Holiday: full day.
            //        new TimeInterval(DateTime.Today.AddDays(8), DateTime.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"))) // Custom time interval off: full and partial day accepted.
            //    },
            //    (date) => { // Optionally, specific nonworking intervals based on date parameter: recurrent breaks and holidays accepted.
            //        if (date.Day % 10 == 0) // First recurrence expression: every ten days.
            //            return new DayTimeInterval[] { 
            //                new DayTimeInterval(TimeOfDay.MinValue, TimeOfDay.Parse("12:00:00")), // Large interval off: first part of day.
            //                new DayTimeInterval(TimeOfDay.Parse("12:30:00"), TimeOfDay.Parse("13:00:00")) // Short break: fast lunch time.
            //            };
            //        else if (date.DayOfWeek != DayOfWeek.Monday) // Second recurrence expression: every day except Mondays.
            //            return new DayTimeInterval[] { 
            //                new DayTimeInterval(TimeOfDay.Parse("12:00:00"), TimeOfDay.Parse("13:00:00")) // Break: regular lunch time.
            //            };
            //        return null; // Otherwise use regular timing only.
            //    });
            // GanttChartDataGrid.IsIndividualItemNonworkingTimeHighlighted = true;

            // You may uncomment the next lines of code to test the component performance:
            // for (int i = 5; i <= 4096; i++)
            // {
            //     GanttChartDataGrid.Items.Add(
            //         new GanttChartItem
            //         {
            //             Content = "Task " + i,
            //             Start = DateTime.Today.AddDays(5 + i / 20),
            //             Finish = DateTime.Today.AddDays(5 + i / 10 + 1)
            //         });
            // }

            // Optionally, define assignable resources.
            foreach (string resource in new string[] { "Resource 1", "Resource 2", "Resource 3", "Material 1", "Material 2" })
                assignableResources.Add(resource);
            GanttChartDataGrid.AssignableResources = assignableResources;

            // Optionally, define the quantity values to consider when leveling resources, indicating maximum material amounts available for use at the same time.
            GanttChartDataGrid.ResourceQuantities = new Dictionary<string, double> { { "Material 1", 4 }, { "Material 2", double.PositiveInfinity } };
            item4.AssignmentsContent = "Material 1 [300%]";
            item6.AssignmentsContent = "Material 1 [250%], Material 2";

            // Optionally, define task and resource costs.
            // GanttChartDataGrid.TaskInitiationCost = 5;
            item4.ExecutionCost = 50;
            // GanttChartDataGrid.DefaultResourceUsageCost = 1;
            // GanttChartDataGrid.SpecificResourceUsageCosts = new Dictionary<string, double> { { "Resource 1", 2 }, { "Material 1", 7 } };
            GanttChartDataGrid.DefaultResourceHourCost = 10;
            GanttChartDataGrid.SpecificResourceHourCosts = new Dictionary<string, double> { { "Resource 1", 20 }, { "Material 2", 0.5 } };

            // Optionally, set AreHierarchyConstraintsEnabled to false to increase performance when you perform hierarchy validation in your application logic.
            GanttChartDataGrid.AreHierarchyConstraintsEnabled = false;

            // Initialize the control area.
            ScalesComboBox.SelectedIndex = 0;
            ShowWeekendsCheckBox.IsChecked = true;
            BaselineCheckBox.IsChecked = true;
            EnableDependencyConstraintsCheckBox.IsChecked = true;
        }

        // Control area commands.
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartItem selectedItem = GanttChartDataGrid.SelectedItem as GanttChartItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Cannot edit as the selection is empty; you should select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            EditItemDialog editItemDialog = new EditItemDialog { DataContext = selectedItem };
            editItemDialog.Show();
        }
        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartItem item = new GanttChartItem { Content = "New Task", Start = DateTime.Today, Finish = DateTime.Today.AddDays(1) };
            GanttChartDataGrid.Items.Add(item);
            Dispatcher.BeginInvoke((Action)delegate
            {
                GanttChartDataGrid.SelectedItem = item;
                GanttChartDataGrid.ScrollTo(item);
                GanttChartDataGrid.ScrollTo(item.Start);
            });
        }
        private void InsertNewButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartItem selectedItem = GanttChartDataGrid.SelectedItem as GanttChartItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Cannot insert a new item before selection as the selection is empty; you can either add a new item to the end of the list instead, or select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            GanttChartItem item = new GanttChartItem { Content = "New Task", Indentation = selectedItem.Indentation, Start = DateTime.Today, Finish = DateTime.Today.AddDays(1) };
            GanttChartDataGrid.Items.Insert(GanttChartDataGrid.SelectedIndex, item);
            Dispatcher.BeginInvoke((Action)delegate
            {
                GanttChartDataGrid.SelectedItem = item;
                GanttChartDataGrid.ScrollTo(item);
                GanttChartDataGrid.ScrollTo(item.Start);
            });
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            List<GanttChartItem> items = new List<GanttChartItem>();
            foreach (GanttChartItem item in GanttChartDataGrid.GetSelectedItems())
                items.Add(item);
            if (items.Count <= 0)
            {
                MessageBox.Show("Cannot delete the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            items.Reverse();
            foreach (GanttChartItem item in items)
            {
                if (item.HasChildren)
                {
                    MessageBox.Show(string.Format("Cannot delete {0} because it has child items; remove its child items first.", item), "Information", MessageBoxButton.OK);
                    continue;
                }
                GanttChartDataGrid.Items.Remove(item);
            }
        }
        private void IncreaseIndentationButton_Click(object sender, RoutedEventArgs e)
        {
            List<GanttChartItem> items = new List<GanttChartItem>();
            foreach (GanttChartItem item in GanttChartDataGrid.GetSelectedItems())
                items.Add(item);
            if (items.Count <= 0)
            {
                MessageBox.Show("Cannot increase indentation for the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            foreach (GanttChartItem item in items)
            {
                int index = GanttChartDataGrid.IndexOf(item);
                if (index > 0)
                {
                    GanttChartItem previousItem = GanttChartDataGrid[index - 1];
                    if (item.Indentation <= previousItem.Indentation)
                        item.Indentation++;
                    else
                        MessageBox.Show(string.Format("Cannot increase indentation for {0} because the previous item is its parent item; increase the indentation of its parent item first.", item), "Information", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show(string.Format("Cannot increase indentation for {0} because it is the first item; insert an item before this one first.", item), "Information", MessageBoxButton.OK);
                }
            }
        }
        private void DecreaseIndentationButton_Click(object sender, RoutedEventArgs e)
        {
            List<GanttChartItem> items = new List<GanttChartItem>();
            foreach (GanttChartItem item in GanttChartDataGrid.GetSelectedItems())
                items.Add(item);
            if (items.Count <= 0)
            {
                MessageBox.Show("Cannot decrease indentation for the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            items.Reverse();
            foreach (GanttChartItem item in items)
            {
                if (item.Indentation > 0)
                {
                    int index = GanttChartDataGrid.IndexOf(item);
                    GanttChartItem nextItem = index < GanttChartDataGrid.Items.Count - 1 ? GanttChartDataGrid[index + 1] : null;
                    if (nextItem == null || item.Indentation >= nextItem.Indentation)
                        item.Indentation--;
                    else
                        MessageBox.Show(string.Format("Cannot increase indentation for {0} because the next item is one of its child items; decrease the indentation of its child items first.", item), "Information", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show(string.Format("Cannot decrease indentation for {0} because it is on the first indentation level, being a root item.", item), "Information", MessageBoxButton.OK);
                }
            }
        }
        private void SetColorButton_Click(object sender, RoutedEventArgs e)
        {
            List<GanttChartItem> items = new List<GanttChartItem>();
            foreach (GanttChartItem item in GanttChartDataGrid.GetSelectedItems())
                items.Add(item);
            if (items.Count <= 0)
            {
                MessageBox.Show("Cannot set a custom bar color to the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            foreach (GanttChartItem item in items)
            {
                GanttChartView.SetStandardBarFill(item, Resources["CustomStandardBarFill"] as Brush);
                GanttChartView.SetStandardBarStroke(item, Resources["CustomStandardBarStroke"] as Brush);
                if (item.HasChildren || item.IsMilestone)
                {
                    MessageBox.Show(string.Format("The custom bar color was set to {0}, but its appearance won't change until it becomes a standard task (currently it is a {1} task).", item, item.HasChildren ? "summary" : "milestone"), "Information", MessageBoxButton.OK);
                    continue;
                }
            }
        }
        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (GanttChartDataGrid.GetSelectedItemCount() <= 0)
            {
                MessageBox.Show("Cannot copy selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            GanttChartDataGrid.Copy();
        }
        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.Paste();
        }
        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (GanttChartDataGrid.GetSelectedItemCount() <= 0)
            {
                MessageBox.Show("Cannot move as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            if (GanttChartDataGrid.GetSelectedItemCount() > 1)
            {
                MessageBox.Show("Cannot move as the selection is multiple; select a single item to move.", "Information", MessageBoxButton.OK);
                return;
            }
            var item = GanttChartDataGrid.SelectedItem;
            GanttChartDataGrid.MoveUp(item, true, true);
            Dispatcher.BeginInvoke((Action)delegate
            {
                GanttChartDataGrid.SelectedItem = item;
            });
        }
        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (GanttChartDataGrid.GetSelectedItemCount() <= 0)
            {
                MessageBox.Show("Cannot move as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            if (GanttChartDataGrid.GetSelectedItemCount() > 1)
            {
                MessageBox.Show("Cannot move as the selection is multiple; select a single item to move.", "Information", MessageBoxButton.OK);
                return;
            }
            var item = GanttChartDataGrid.SelectedItem;
            GanttChartDataGrid.MoveDown(item, true, true);
            Dispatcher.BeginInvoke((Action)delegate
            {
                GanttChartDataGrid.SelectedItem = item;
            });
        }
        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            if (GanttChartDataGrid.CanUndo())
                GanttChartDataGrid.Undo();
            else
                MessageBox.Show("Currently there is no recorded action in the undo queue; perform an action first.", "Information", MessageBoxButton.OK);
        }
        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            if (GanttChartDataGrid.CanRedo())
                GanttChartDataGrid.Redo();
            else
                MessageBox.Show("Currently there is no recorded action in the redo queue; perform an action and undo it first.", "Information", MessageBoxButton.OK);
        }
        private void ScaleTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = ScalesComboBox.SelectedItem as ComboBoxItem;
            string scalesResourceKey = selectedComboBoxItem.Tag as string;
            ScaleCollection scales = Resources[scalesResourceKey] as ScaleCollection;
            GanttChartDataGrid.Scales = scales;
        }
        private void ZoomCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            originalZoom = GanttChartDataGrid.HourWidth;
            GanttChartDataGrid.HourWidth = originalZoom * 2;
        }
        private void ZoomCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.HourWidth = originalZoom;
        }
        private double originalZoom;
        private void IncreaseTimelinePageButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.TimelinePageFinish += pageUpdateAmount;
            GanttChartDataGrid.TimelinePageStart += pageUpdateAmount;
        }
        private void DecreaseTimelinePageButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.TimelinePageFinish -= pageUpdateAmount;
            GanttChartDataGrid.TimelinePageStart -= pageUpdateAmount;
        }
        private void FitToTimelinePageCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ZoomCheckBox.IsChecked = false;
            ZoomCheckBox.IsEnabled = false;
            GanttChartDataGrid.IsFittingToTimelinePageEnabled = true;
        }
        private void FitToTimelinePageCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.IsFittingToTimelinePageEnabled = false;
            ZoomCheckBox.IsEnabled = true;
        }
        private readonly TimeSpan pageUpdateAmount = TimeSpan.FromDays(7);
        private void ShowWeekendsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.VisibleWeekStart = DayOfWeek.Sunday;
            GanttChartDataGrid.VisibleWeekFinish = DayOfWeek.Saturday;
            WorkOnWeekendsCheckBox.IsEnabled = true;
        }
        private void ShowWeekendsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            WorkOnWeekendsCheckBox.IsChecked = false;
            WorkOnWeekendsCheckBox.IsEnabled = false;
            GanttChartDataGrid.VisibleWeekStart = DayOfWeek.Monday;
            GanttChartDataGrid.VisibleWeekFinish = DayOfWeek.Friday;
        }
        private void WorkOnWeekendsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.WorkingWeekStart = DayOfWeek.Sunday;
            GanttChartDataGrid.WorkingWeekFinish = DayOfWeek.Saturday;
        }
        private void WorkOnWeekendsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.WorkingWeekStart = DayOfWeek.Monday;
            GanttChartDataGrid.WorkingWeekFinish = DayOfWeek.Friday;
        }
        private void BaselineCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.IsBaselineVisible = true;
        }
        private void BaselineCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.IsBaselineVisible = false;
        }
        private void CriticalPathCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            EditButton.IsEnabled = false;
            AddNewButton.IsEnabled = false;
            InsertNewButton.IsEnabled = false;
            IncreaseIndentationButton.IsEnabled = false;
            DecreaseIndentationButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            PasteButton.IsEnabled = false;
            UndoButton.IsEnabled = false;
            RedoButton.IsEnabled = false;
            SetColorButton.IsEnabled = false;
            SplitRemainingWorkButton.IsEnabled = false;
            LevelResourcesButton.IsEnabled = false;
            EnableDependencyConstraintsCheckBox.IsEnabled = false;
            ScheduleChartButton.IsEnabled = false;
            LoadProjectXmlButton.IsEnabled = false;
            GanttChartDataGrid.IsReadOnly = true;
            TimeSpan criticalDelay = TimeSpan.FromHours(8);
            foreach (GanttChartItem item in GanttChartDataGrid.Items)
            {
                if (item.HasChildren)
                    continue;
                SetCriticalPathHighlighting(item, GanttChartDataGrid.IsCritical(item, criticalDelay));
            }
            if (CriticalPathCheckBox.IsChecked == true)
                MessageBox.Show("Gantt Chart items are temporarily read only while critical path is highlighted.", "Information", MessageBoxButton.OK);
        }
        private void CriticalPathCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (GanttChartItem item in GanttChartDataGrid.Items)
            {
                if (item.HasChildren)
                    continue;
                SetCriticalPathHighlighting(item, false);
            }
            GanttChartDataGrid.IsReadOnly = false;
            EditButton.IsEnabled = true;
            AddNewButton.IsEnabled = true;
            InsertNewButton.IsEnabled = true;
            IncreaseIndentationButton.IsEnabled = true;
            DecreaseIndentationButton.IsEnabled = true;
            DeleteButton.IsEnabled = true;
            PasteButton.IsEnabled = true;
            UndoButton.IsEnabled = true;
            RedoButton.IsEnabled = true;
            SetColorButton.IsEnabled = true;
            SplitRemainingWorkButton.IsEnabled = true;
            LevelResourcesButton.IsEnabled = true;
            EnableDependencyConstraintsCheckBox.IsEnabled = true;
            ScheduleChartButton.IsEnabled = true;
            LoadProjectXmlButton.IsEnabled = true;
        }
        private void SetCriticalPathHighlighting(GanttChartItem item, bool isHighlighted)
        {
            if (!item.IsMilestone)
            {
                GanttChartView.SetStandardBarFill(item, isHighlighted ? Resources["CustomStandardBarFill"] as Brush : GanttChartDataGrid.StandardBarFill);
                GanttChartView.SetStandardBarStroke(item, isHighlighted ? Resources["CustomStandardBarStroke"] as Brush : GanttChartDataGrid.StandardBarStroke);
            }
            else
            {
                GanttChartView.SetMilestoneBarFill(item, isHighlighted ? Resources["CustomStandardBarFill"] as Brush : GanttChartDataGrid.MilestoneBarFill);
                GanttChartView.SetMilestoneBarStroke(item, isHighlighted ? Resources["CustomStandardBarStroke"] as Brush : GanttChartDataGrid.MilestoneBarStroke);
            }
        }
        private void SplitRemainingWorkButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartItem selectedItem = GanttChartDataGrid.SelectedItem as GanttChartItem;
            if (selectedItem == null || selectedItem.HasChildren || selectedItem.IsMilestone || !selectedItem.HasStarted || selectedItem.IsCompleted)
            {
                MessageBox.Show("Cannot split work as the selection is empty or the selected item does not represent a standard task in progress; you should select an appropriate item first.", "Information", MessageBoxButton.OK);
                return;
            }
            GanttChartDataGrid.SplitRemainingWork(selectedItem, " (rem. work)", " (compl. work)");
        }
        private void ScheduleChartButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.UnassignedScheduleChartItemContent = "(Unassigned)"; // Optional
            scheduleChartItems = GanttChartDataGrid.GetScheduleChartItems();
            ChildWindow scheduleChartWindow =
                new ChildWindow
                {
                    Title = "Schedule Chart", Width = 640, Height = 480,
                    Content = new ScheduleChartDataGrid
                    { 
                        Items = scheduleChartItems, DataGridWidth = new GridLength(0.2, GridUnitType.Star), 
                        UseMultipleLinesPerRow = true, AreIndividualItemAppearanceSettingsApplied = true, IsAlternatingItemBackgroundInverted = true, UnassignedScheduleChartItemContent = GanttChartDataGrid.UnassignedScheduleChartItemContent // Optional
                    }
                };
            scheduleChartWindow.Closed += ScheduleChartWindow_Closed;
            scheduleChartWindow.Show();
        }
        private ObservableCollection<ScheduleChartItem> scheduleChartItems;
        private void ScheduleChartWindow_Closed(object sender, EventArgs e)
        {
            GanttChartDataGrid.UpdateChangesFromScheduleChartItems(scheduleChartItems);
            GanttChartDataGrid.DisposeScheduleChartItems(scheduleChartItems);
            scheduleChartItems = null;
        }
        private void LoadChartButton_Click(object sender, RoutedEventArgs e)
        {
            loadChartItems = GanttChartDataGrid.GetLoadChartItems();
            ObservableCollection<LoadChartItem> selectedLoadChartItemContainer = new ObservableCollection<LoadChartItem>();
            ComboBox resourceComboBox = new ComboBox { ItemsSource = loadChartItems, DisplayMemberPath = "Content", Margin = new Thickness(0, 0, 0, 4) };
            resourceComboBox.SelectionChanged += delegate
            {
                selectedLoadChartItemContainer.Clear();
                selectedLoadChartItemContainer.Add(resourceComboBox.SelectedItem as LoadChartItem);
            };
            if (resourceComboBox.Items.Count > 0)
                resourceComboBox.SelectedIndex = 0;
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.Children.Add(resourceComboBox);
            Grid.SetRow(resourceComboBox, 0);
            LoadChartView loadChartView = new LoadChartView { Items = selectedLoadChartItemContainer, ItemHeight = 176, BarHeight = 172, Height = 240, VerticalAlignment = VerticalAlignment.Top };
            grid.Children.Add(loadChartView);
            Grid.SetRow(loadChartView, 1);
            ChildWindow loadChartWindow =
                new ChildWindow
                {
                    Title = "Load Chart", Width = 640, Height = 300,
                    Content = grid
                };
            loadChartWindow.Closed += LoadChartWindow_Closed;
            loadChartWindow.Show();
        }
        private ObservableCollection<LoadChartItem> loadChartItems;
        private void LoadChartWindow_Closed(object sender, EventArgs e)
        {
            GanttChartDataGrid.DisposeLoadChartItems(loadChartItems);
            loadChartItems = null;
        }
        private void EditResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<string> resourceItems = GanttChartDataGrid.AssignableResources;
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
            var resourceListBox = new ListBox { ItemsSource = resourceItems, SelectionMode = SelectionMode.Extended, TabIndex = 2, Margin = new Thickness(0, 0, 0, 4) };
            var commandsGrid = new Grid();
            commandsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            commandsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0, GridUnitType.Auto) });
            commandsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0, GridUnitType.Auto) });
            Grid.SetRow(commandsGrid, 1);
            grid.Children.Add(commandsGrid);
            var newResourceTextBox = new TextBox { TabIndex = 0, Margin = new Thickness(0, 0, 4, 0) };
            var addResourceButton = new Button { Content = "Add", TabIndex = 1, Margin = new Thickness(0, 0, 4, 0) };
            Grid.SetColumn(addResourceButton, 1);
            var deleteResourcesButton = new Button { Content = "Delete", TabIndex = 3, Margin = new Thickness(4, 0, 0, 0) };
            Grid.SetColumn(deleteResourcesButton, 2);
            commandsGrid.Children.Add(deleteResourcesButton);
            commandsGrid.Children.Add(addResourceButton);
            commandsGrid.Children.Add(newResourceTextBox);
            grid.Children.Add(resourceListBox);
            addResourceButton.Click += delegate
            {
                var newResource = newResourceTextBox.Text;
                if (!string.IsNullOrEmpty(newResource) && !resourceItems.Contains(newResource))
                    resourceItems.Add(newResource);
                newResourceTextBox.Text = string.Empty;
                resourceListBox.SelectedItem = newResource;
            };
            deleteResourcesButton.Click += delegate
            {
                List<string> removedResources = new List<string>();
                foreach (string resource in resourceListBox.SelectedItems)
                    removedResources.Add(resource);
                foreach (string resource in removedResources)
                    resourceItems.Remove(resource);
                newResourceTextBox.Text = string.Empty;
            };
            ChildWindow resourceWindow =
                new ChildWindow
                {
                    Title = "Resources", Width = 640, Height = 300,
                    Content = grid
                };
            resourceWindow.Show();
        }
        private void PertChartButton_Click(object sender, RoutedEventArgs e)
        {
            // Optionally, specify a maximum indentation level to consider when generating PERT items as a parameter to the GetPertChartItems method call.
            pertChartItems = GanttChartDataGrid.GetPertChartItems();
            var pertChartView = new DlhSoft.Windows.Controls.Pert.PertChartView { Items = pertChartItems, PredecessorToolTipTemplate = Resources["PertChartPredecessorToolTipTemplate"] as DataTemplate };
            ChildWindow pertChartWindow =
                new ChildWindow
                {
                    Title = "PERT Chart", Width = 640, Height = 480,
                    Content = pertChartView
                };
            pertChartView.AsyncPresentationCompleted += delegate(object senderCompleted, EventArgs eCompleted)
            {
                // Optionally, highlight the critical path.
                Brush redBrush = new SolidColorBrush(Colors.Red);
                foreach (var item in pertChartView.GetCriticalItems())
                {
                    DlhSoft.Windows.Controls.Pert.PertChartView.SetShapeStroke(item, redBrush);
                    DlhSoft.Windows.Controls.Pert.PertChartView.SetTextForeground(item, redBrush);
                }
                foreach (var predecessorItem in pertChartView.GetCriticalDependencies())
                {
                    DlhSoft.Windows.Controls.Pert.PertChartView.SetDependencyLineStroke(predecessorItem, redBrush);
                    DlhSoft.Windows.Controls.Pert.PertChartView.SetDependencyTextForeground(predecessorItem, redBrush);
                }
            };
            pertChartWindow.Closed += PertChartWindow_Closed;
            pertChartWindow.Show();
        }
        private ObservableCollection<DlhSoft.Windows.Controls.Pert.PertChartItem> pertChartItems;
        private void PertChartWindow_Closed(object sender, EventArgs e)
        {
            GanttChartDataGrid.DisposePertChartItems(pertChartItems);
            pertChartItems = null;
        }
        private void NetworkDiagramButton_Click(object sender, RoutedEventArgs e)
        {
            // Optionally, specify a maximum indentation level to consider when generating network items as a parameter to the GetNetworkDiagramItems method call.
            networkDiagramItems = GanttChartDataGrid.GetNetworkDiagramItems();
            var networkDiagramView = new DlhSoft.Windows.Controls.Pert.NetworkDiagramView { Items = networkDiagramItems };
            ChildWindow networkDiagramWindow =
                new ChildWindow
                {
                    Title = "Network Diagram", Width = 960, Height = 600,
                    Content = networkDiagramView
                };
            networkDiagramView.AsyncPresentationCompleted += delegate(object senderCompleted, EventArgs eCompleted)
            {
                // Optionally, reposition start and finish milestones between the first and second rows of the view.
                networkDiagramView.RepositionEnds();
                // Optionally, highlight the critical path.
                Brush redBrush = new SolidColorBrush(Colors.Red);
                foreach (var item in networkDiagramView.GetCriticalItems())
                    DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetShapeStroke(item, redBrush);
                foreach (var predecessorItem in networkDiagramView.GetCriticalDependencies())
                    DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetDependencyLineStroke(predecessorItem, redBrush);
            };
            networkDiagramWindow.Closed += NetworkDiagramWindow_Closed;
            networkDiagramWindow.Show();
        }
        private ObservableCollection<DlhSoft.Windows.Controls.Pert.NetworkDiagramItem> networkDiagramItems;
        private void NetworkDiagramWindow_Closed(object sender, EventArgs e)
        {
            GanttChartDataGrid.DisposeNetworkDiagramItems(networkDiagramItems);
            networkDiagramItems = null;
        }
        private void ProjectStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            var statistics = string.Format("Start:\t{0:d}\nFinish:\t{1:d}\nEffort:\t{2:0.##}h\nCompl.:\t{3:0.##%}\nCost:\t${4:0.##}",
                GanttChartDataGrid.GetProjectStart(), GanttChartDataGrid.GetProjectFinish(),
                GanttChartDataGrid.GetProjectEffort().TotalHours, GanttChartDataGrid.GetProjectCompletion(),
                GanttChartDataGrid.GetProjectCost());
            MessageBox.Show(statistics, "Project statistics", MessageBoxButton.OK);
        }
        private void LevelResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.LevelResources();
        }
        private void EnableDependencyConstraintsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.AreTaskDependencyConstraintsEnabled = true;
        }
        private void EnableDependencyConstraintsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.AreTaskDependencyConstraintsEnabled = false;
        }
        private void SaveProjectXmlButton_Click(object sender, RoutedEventArgs e)
        {
            // Select a Project XML file to save data to.
            SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Project XML files|*.xml", DefaultExt = ".xml" };
            if (saveFileDialog.ShowDialog() != true)
                return;
            var assignableResources = GanttChartDataGrid.AssignableResources;
            using (Stream stream = saveFileDialog.OpenFile())
            {
                GanttChartDataGrid.SaveProjectXml(stream, assignableResources);
            }
        }
        private void LoadProjectXmlButton_Click(object sender, RoutedEventArgs e)
        {
            // Select a Project XML file to load data from.
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Project XML files|*.xml", Multiselect = false };
            if (openFileDialog.ShowDialog() != true)
                return;
            var assignableResources = GanttChartDataGrid.AssignableResources;
            using (Stream stream = openFileDialog.File.OpenRead())
            {
                GanttChartDataGrid.LoadProjectXml(stream, assignableResources);
                GanttChartDataGrid.ScrollToVerticalOffset(0);
            }
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            // Optionally, to rotate the print output and simulate Landscape printing mode (when the end user keeps Portrait selection in the Print dialog), append the rotate parameter set to true to the method call: rotate: true.
            GanttChartDataGrid.Print("GanttChartDataGrid Sample Document");
        }
        private void ExportImageButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.Export((Action)delegate
            {
                ChildWindow childWindow = 
                    new ChildWindow 
                    { 
                        Title = "Exported Image",
                        Content = 
                            new ScrollViewer 
                            {
                                Content = 
                                    new Image
                                    {
                                        Source = GanttChartDataGrid.GetExportBitmapSource(), Stretch = Stretch.None, 
                                        HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top
                                    }, 
                                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto, VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                                Padding = new Thickness(0)
                            }
                    };
                childWindow.Show();
            });
        }
    }
}
