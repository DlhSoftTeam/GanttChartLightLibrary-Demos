using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DlhSoft.Windows.Controls;
using DlhSoft.Windows.Data;
using System.Windows.Threading;
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;

namespace Demos.WPF.CSharp.GanttChartDataGrid.MainFeatures_GCV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DateTime dateTime = DateTime.Today;

        public MainWindow()
        {
            InitializeComponent();

            while (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday)
                dateTime = dateTime.AddDays(1);

            GanttChartItem item0 = GanttChartDataGrid.Items[0];
            item0.IsExpanded = false;

            GanttChartItem item1 = GanttChartDataGrid.Items[1];
            item1.Start = dateTime.Add(TimeSpan.Parse("08:00:00"));
            item1.Finish = dateTime.Add(TimeSpan.Parse("16:00:00"));
            item1.CompletedFinish = dateTime.Add(TimeSpan.Parse("12:00:00"));
            item1.AssignmentsContent = "Resource 1";

            GanttChartItem item2 = GanttChartDataGrid.Items[2];
            item2.Start = dateTime.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item2.Finish = dateTime.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            // Important note: CompletedFinish value defaults to dateTime, therefore you should always set it to a Start (or a value between Start and Finish) when you initialize a past task item! In this example we don't set it as the task is in the future.
            item2.AssignmentsContent = "Resource 1, Resource 2";
            item2.Predecessors.Add(new PredecessorItem { Item = item1 });

            GanttChartItem item3 = GanttChartDataGrid.Items[3];
            item3.Predecessors.Add(new PredecessorItem { Item = item0, DependencyType = DependencyType.StartStart });

            GanttChartItem item4 = GanttChartDataGrid.Items[4];
            item4.Start = dateTime.Add(TimeSpan.Parse("08:00:00"));
            item4.Finish = dateTime.AddDays(2).Add(TimeSpan.Parse("12:00:00"));
            item4.CompletedFinish = dateTime.AddDays(1).Add(TimeSpan.Parse("12:00:00"));

            GanttChartItem item6 = GanttChartDataGrid.Items[6];
            item6.Start = dateTime.Add(TimeSpan.Parse("08:00:00"));
            item6.Finish = dateTime.AddDays(3).Add(TimeSpan.Parse("12:00:00"));
            item6.BaselineStart = item6.Start;
            item6.BaselineFinish = item6.Finish;

            GanttChartItem item7 = GanttChartDataGrid.Items[7];
            item7.Start = dateTime.AddDays(4);
            item7.IsMilestone = true;
            item7.Predecessors.Add(new PredecessorItem { Item = item4 });
            item7.Predecessors.Add(new PredecessorItem { Item = item6 });
            item7.BaselineStart = dateTime.AddDays(3).Add(TimeSpan.Parse("12:00:00"));

            GanttChartItem item8 = GanttChartDataGrid.Items[8];
            item8.Start = dateTime.AddDays(3).Add(TimeSpan.Parse("12:00:00"));
            item8.Finish = dateTime.AddDays(6).Add(TimeSpan.Parse("14:00:00"));
            item8.AssignmentsContent = "Resource 1 [50%], Resource 2 [75%]";
            item8.BaselineStart = dateTime.Add(TimeSpan.Parse("12:00:00"));
            item8.BaselineFinish = dateTime.AddDays(4).Add(TimeSpan.Parse("14:00:00"));

            GanttChartItem item9 = GanttChartDataGrid.Items[9];
            item9.Start = dateTime.AddDays(6).Add(TimeSpan.Parse("08:00:00"));
            item9.Finish = dateTime.AddDays(6).Add(TimeSpan.Parse("16:00:00"));
            item9.AssignmentsContent = "Resource 1";
            item9.BaselineStart = dateTime.AddDays(4).Add(TimeSpan.Parse("12:00:00"));
            item9.BaselineFinish = dateTime.AddDays(6).Add(TimeSpan.Parse("16:00:00"));

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
            //        new TimeInterval(dateTime.AddDays(1), dateTime.AddDays(1).Add(TimeOfDay.MaxValue)), // Holiday: full day.
            //        new TimeInterval(dateTime.AddDays(3), dateTime.AddDays(5).Add(TimeSpan.Parse("12:00:00"))) // Custom time interval off: full and partial day accepted.
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
            //        new TimeInterval(dateTime.AddDays(4), dateTime.AddDays(4).Add(TimeOfDay.MaxValue)), // Holiday: full day.
            //        new TimeInterval(dateTime.AddDays(8), dateTime.AddDays(10).Add(TimeSpan.Parse("12:00:00"))) // Custom time interval off: full and partial day accepted.
            //    },
            //    (date) => { // Optionally, specific nonworking intervals based on date parameter: recurrent breaks and holidays accepted.
            //        if (date.Day % 10 == 0) // First recurrence expression: on decade end days.
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

            for (int i = 5; i <= 25; i++)
            {
                GanttChartDataGrid.Items.Add(
                    new GanttChartItem
                    {
                        Content = "Task " + i,
                        Indentation = i >= 8 && i % 3 == 2 ? 0 : 1,
                        Start = dateTime.AddDays(i <= 8 ? (i - 4) * 3 : i - 8),
                        Finish = dateTime.AddDays((i <= 8 ? (i - 4) * 3 + (i > 8 ? 6 : 1) : i - 2) + 1)
                    });
            }

            // You may uncomment the next lines of code to test the component performance:
            // for (int i = 26; i <= 4096; i++)
            // {
            //     GanttChartDataGrid.Items.Add(
            //         new GanttChartItem
            //         {
            //             Content = "Task " + i,
            //             Start = dateTime.AddDays(5 + i / 20),
            //             Finish = dateTime.AddDays(5 + i / 10 + 1)
            //         });
            // }

            // Optionally, define assignable resources.
            GanttChartDataGrid.AssignableResources = new ObservableCollection<string> { "Resource 1", "Resource 2", "Resource 3", 
                                                                                        "Material 1", "Material 2" };

            // Optionally, define the quantity values to consider when leveling resources, indicating maximum material amounts available for use at the same time.
            GanttChartDataGrid.ResourceQuantities = new Dictionary<string, double> { { "Material 1", 4 }, { "Material 2", double.PositiveInfinity } };
            item4.AssignmentsContent = "Resource 1, Material 1 [300%]";
            item6.AssignmentsContent = "Material 1 [250%], Material 2";

            // Optionally, define task and resource costs.
            // GanttChartDataGrid.TaskInitiationCost = 5;
            item4.ExecutionCost = 50;
            // GanttChartDataGrid.DefaultResourceUsageCost = 1;
            // GanttChartDataGrid.SpecificResourceUsageCosts = new Dictionary<string, double> { { "Resource 1", 2 }, { "Material 1", 7 } };
            GanttChartDataGrid.DefaultResourceHourCost = 10;
            GanttChartDataGrid.SpecificResourceHourCosts = new Dictionary<string, double> { { "Resource 1", 20 }, { "Material 2", 0.5 } };

            // Optionally, set AreHierarchyConstraintsEnabled to false to increase performance when you perform hierarchy validation in your application.
            GanttChartDataGrid.AreHierarchyConstraintsEnabled = false;

            // Initialize the control area.
            ScalesComboBox.SelectedIndex = 0;
            ShowWeekendsCheckBox.IsChecked = true;
            BaselineCheckBox.IsChecked = true;
            EnableDependencyConstraintsCheckBox.IsChecked = true;
        }

        public MainWindow(string theme) : this()
        {
            if (theme == null || theme == "Default" || theme == "Aero")
                return;
            themeResourceDictionary = new ResourceDictionary { Source = new Uri("/" + GetType().Assembly.GetName().Name + ";component/Themes/" + theme + ".xaml", UriKind.Relative) };
            GanttChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }

        private ResourceDictionary themeResourceDictionary;

        // Control area commands.
        private void EditButton_Click(object sender, RoutedEventArgs e)
        { }
        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        { }
        private void InsertNewButton_Click(object sender, RoutedEventArgs e)
        { }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        { }
        private void IncreaseIndentationButton_Click(object sender, RoutedEventArgs e)
        { }
        private void DecreaseIndentationButton_Click(object sender, RoutedEventArgs e)
        { }
        private void SetColorButton_Click(object sender, RoutedEventArgs e)
        { }
        private void CopyButton_Click(object sender, RoutedEventArgs e)
        { }
        private void PasteButton_Click(object sender, RoutedEventArgs e)
        { }
        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        { }
        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        { }
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
        { }
        private void ScheduleChartButton_Click(object sender, RoutedEventArgs e)
        {
            double originalOpacity = Opacity;
            Opacity = 0.5;
            GanttChartDataGrid.UnassignedScheduleChartItemContent = "(Unassigned)"; // Optional
            ObservableCollection<ScheduleChartItem> scheduleChartItems = GanttChartDataGrid.GetScheduleChartItems();
            Window scheduleChartWindow =
                new Window
                {
                    Owner = Application.Current.MainWindow, Title = "Schedule Chart", WindowStartupLocation = WindowStartupLocation.CenterOwner, Width = 640, Height = 480, ResizeMode = ResizeMode.CanResize,
                    Content = new ScheduleChartDataGrid
                    {
                        Items = scheduleChartItems, DataGridWidth = new GridLength(0.2, GridUnitType.Star),
                        UseMultipleLinesPerRow = true, AreIndividualItemAppearanceSettingsApplied = true, IsAlternatingItemBackgroundInverted = true, UnassignedScheduleChartItemContent = GanttChartDataGrid.UnassignedScheduleChartItemContent // Optional
                    }
                };
            if (themeResourceDictionary != null)
                (scheduleChartWindow.Content as FrameworkElement).Resources.MergedDictionaries.Add(themeResourceDictionary);
            scheduleChartWindow.ShowDialog();
            GanttChartDataGrid.UpdateChangesFromScheduleChartItems(scheduleChartItems);
            GanttChartDataGrid.DisposeScheduleChartItems(scheduleChartItems);
            Opacity = originalOpacity;
        }
        private void LoadChartButton_Click(object sender, RoutedEventArgs e)
        {
            double originalOpacity = Opacity;
            Opacity = 0.5;
            ObservableCollection<LoadChartItem> loadChartItems = GanttChartDataGrid.GetLoadChartItems();
            ObservableCollection<LoadChartItem> selectedLoadChartItemContainer = new ObservableCollection<LoadChartItem>();
            ComboBox resourceComboBox = new ComboBox { ItemsSource = loadChartItems, DisplayMemberPath = "Content", Margin = new Thickness(4) };
            resourceComboBox.SelectionChanged += delegate
            {
                selectedLoadChartItemContainer.Clear();
                selectedLoadChartItemContainer.Add(resourceComboBox.SelectedItem as LoadChartItem);
            };
            if (resourceComboBox.Items.Count > 0)
                resourceComboBox.SelectedIndex = 0;
            DockPanel dockPanel = new DockPanel();
            dockPanel.Children.Add(resourceComboBox);
            DockPanel.SetDock(resourceComboBox, Dock.Top);
            dockPanel.Children.Add(new LoadChartView { Items = selectedLoadChartItemContainer, ItemHeight = 170, BarHeight = 166, Height = 230, Margin = new Thickness(4, 0, 4, 4), VerticalAlignment = VerticalAlignment.Top });
            Window loadChartWindow =
                new Window
                {
                    Owner = Application.Current.MainWindow, Title = "Load Chart", WindowStartupLocation = WindowStartupLocation.CenterOwner, Width = 640, Height = 300, ResizeMode = ResizeMode.CanMinimize,
                    Content = dockPanel
                };
            if (themeResourceDictionary != null)
            {
                var loadChartView = dockPanel.Children[dockPanel.Children.Count - 1] as LoadChartView;
                loadChartView.Resources.MergedDictionaries.Add(themeResourceDictionary);
                loadChartView.BarHeight -= 15;
                loadChartView.ItemHeight -= 15;
            }
            loadChartWindow.ShowDialog();
            GanttChartDataGrid.DisposeLoadChartItems(loadChartItems);
            Opacity = originalOpacity;
        }
        private void EditResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            double originalOpacity = Opacity;
            Opacity = 0.5;
            var dockPanel = new DockPanel();
            var resourceItems = GanttChartDataGrid.AssignableResources;
            var resourceListBox = new ListBox { ItemsSource = resourceItems, SelectionMode = SelectionMode.Extended, TabIndex = 2, Margin = new Thickness(4) };
            var commandsDockPanel = new DockPanel { Margin = new Thickness(4, 0, 4, 4) };
            DockPanel.SetDock(commandsDockPanel, Dock.Bottom);
            dockPanel.Children.Add(commandsDockPanel);
            var newResourceTextBox = new TextBox { TabIndex = 0, Margin = new Thickness(0, 0, 4, 0) };
            var addResourceButton = new Button { Content = "Add", IsDefault = true, TabIndex = 1, Margin = new Thickness(0, 0, 4, 0) };
            DockPanel.SetDock(addResourceButton, Dock.Right);
            var deleteResourcesButton = new Button { Content = "Delete", TabIndex = 3, Margin = new Thickness(4, 0, 0, 0) };
            DockPanel.SetDock(deleteResourcesButton, Dock.Right);
            commandsDockPanel.Children.Add(deleteResourcesButton);
            commandsDockPanel.Children.Add(addResourceButton);
            commandsDockPanel.Children.Add(newResourceTextBox);
            dockPanel.Children.Add(resourceListBox);
            addResourceButton.Click += delegate 
            { 
                var newResource = newResourceTextBox.Text;
                if (!string.IsNullOrEmpty(newResource) && !resourceItems.Contains(newResource))
                    resourceItems.Add(newResource); 
                newResourceTextBox.Clear();
                resourceListBox.SelectedItem = newResource;
            };
            deleteResourcesButton.Click += delegate
            {
                List<string> removedResources = new List<string>();
                foreach (string resource in resourceListBox.SelectedItems)
                    removedResources.Add(resource);
                foreach (string resource in removedResources)
                    resourceItems.Remove(resource);
                newResourceTextBox.Clear();
            };
            Window resourceWindow =
                new Window
                {
                    Owner = Application.Current.MainWindow, Title = "Resources", WindowStartupLocation = WindowStartupLocation.CenterOwner, Width = 640, Height = 300, ResizeMode = ResizeMode.CanMinimize,
                    Content = dockPanel
                };
            resourceWindow.ShowDialog();
            Opacity = originalOpacity;
        }
        private void PertChartButton_Click(object sender, RoutedEventArgs e)
        {
            double originalOpacity = Opacity;
            Opacity = 0.5;
            // Optionally, specify a maximum indentation level to consider when generating PERT items as a parameter to the GetPertChartItems method call.
            ObservableCollection<DlhSoft.Windows.Controls.Pert.PertChartItem> pertChartItems = GanttChartDataGrid.GetPertChartItems();
            var pertChartView = new DlhSoft.Windows.Controls.Pert.PertChartView { Items = pertChartItems, PredecessorToolTipTemplate = Resources["PertChartPredecessorToolTipTemplate"] as DataTemplate };
            Window pertChartWindow =
                new Window
                {
                    Owner = Application.Current.MainWindow, Title = "PERT Chart", WindowStartupLocation = WindowStartupLocation.CenterOwner, Width = 640, Height = 480, ResizeMode = ResizeMode.CanResize,
                    Content = pertChartView
                };
            pertChartView.AsyncPresentationCompleted += delegate(object senderCompleted, EventArgs eCompleted)
            {
                // Optionally, highlight the critical path.
                foreach (var item in pertChartView.GetCriticalItems())
                {
                    DlhSoft.Windows.Controls.Pert.PertChartView.SetShapeStroke(item, Brushes.Red);
                    DlhSoft.Windows.Controls.Pert.PertChartView.SetTextForeground(item, Brushes.Red);
                }
                foreach (var predecessorItem in pertChartView.GetCriticalDependencies())
                {
                    DlhSoft.Windows.Controls.Pert.PertChartView.SetDependencyLineStroke(predecessorItem, Brushes.Red);
                    DlhSoft.Windows.Controls.Pert.PertChartView.SetDependencyTextForeground(predecessorItem, Brushes.Red);
                }
            };
            if (themeResourceDictionary != null)
                (pertChartWindow.Content as FrameworkElement).Resources.MergedDictionaries.Add(themeResourceDictionary);
            pertChartWindow.ShowDialog();
            GanttChartDataGrid.DisposePertChartItems(pertChartItems);
            Opacity = originalOpacity;
        }
        private void NetworkDiagramButton_Click(object sender, RoutedEventArgs e)
        {
            double originalOpacity = Opacity;
            Opacity = 0.5;
            // Optionally, specify a maximum indentation level to consider when generating network items as a parameter to the GetNetworkDiagramItems method call.
            ObservableCollection<DlhSoft.Windows.Controls.Pert.NetworkDiagramItem> networkDiagramItems = GanttChartDataGrid.GetNetworkDiagramItems();
            DlhSoft.Windows.Controls.Pert.NetworkDiagramView networkDiagramView = new DlhSoft.Windows.Controls.Pert.NetworkDiagramView { Items = networkDiagramItems };
            Window networkDiagramWindow =
                new Window
                {
                    Owner = Application.Current.MainWindow, Title = "Network Diagram", WindowStartupLocation = WindowStartupLocation.CenterOwner, Width = 960, Height = 600, ResizeMode = ResizeMode.CanResize,
                    Content = networkDiagramView
                };
            networkDiagramView.AsyncPresentationCompleted += delegate(object senderCompleted, EventArgs eCompleted)
            {
                // Optionally, reposition start and finish milestones between the first and second rows of the view.
                networkDiagramView.RepositionEnds();
                // Optionally, highlight the critical path.
                foreach (var item in networkDiagramView.GetCriticalItems())
                    DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetShapeStroke(item, Brushes.Red);
                foreach (var predecessorItem in networkDiagramView.GetCriticalDependencies())
                    DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetDependencyLineStroke(predecessorItem, Brushes.Red);
            };
            if (themeResourceDictionary != null)
                (networkDiagramWindow.Content as FrameworkElement).Resources.MergedDictionaries.Add(themeResourceDictionary);
            networkDiagramWindow.ShowDialog();
            GanttChartDataGrid.DisposeNetworkDiagramItems(networkDiagramItems);
            Opacity = originalOpacity;
        }
        private void ProjectStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            var statistics = string.Format("Start:\t{0:d}\nFinish:\t{1:d}\nEffort:\t{2:0.##}h\nCompl.:\t{3:0.##%}\nCost:\t${4:0.##}", 
                GanttChartDataGrid.GetProjectStart(), GanttChartDataGrid.GetProjectFinish(), 
                GanttChartDataGrid.GetProjectEffort().TotalHours, GanttChartDataGrid.GetProjectCompletion(), 
                GanttChartDataGrid.GetProjectCost());
            MessageBox.Show(statistics, "Project statistics", MessageBoxButton.OK, MessageBoxImage.Information);
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
            SaveFileDialog saveFileDialog = new SaveFileDialog { Title = "Save Project XML", Filter = "Project XML files|*.xml", DefaultExt = ".xml" };
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
            OpenFileDialog openFileDialog = new OpenFileDialog { Title = "Load Project XML", Filter = "Project XML files|*.xml", Multiselect = false };
            if (openFileDialog.ShowDialog() != true)
                return;
            var assignableResources = GanttChartDataGrid.AssignableResources;
            using (Stream stream = openFileDialog.OpenFile())
            {
                GanttChartDataGrid.LoadProjectXml(stream, assignableResources);
            }
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.Print("GanttChartDataGrid Sample Document");
        }
        private void ExportImageButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartDataGrid.Export((Action)delegate
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog { Title = "Export Image To", Filter = "PNG image files|*.png" };
                if (saveFileDialog.ShowDialog() != true)
                    return;
                BitmapSource bitmapSource = GanttChartDataGrid.GetExportBitmapSource(96 * 2);
                using (Stream stream = saveFileDialog.OpenFile())
                {
                    PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
                    pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    pngBitmapEncoder.Save(stream);
                }
            });
        }
    }
}
