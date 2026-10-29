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

namespace Demos.WPF.CSharp.GanttChartDataGrid.MainFeatures
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
            GanttChartDataGrid.PreparingCellForEdit += GanttChartDataGrid_PreparingCellForEdit;

            DateTime monthStart = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));

            GanttChartItem itemPlanning = GanttChartDataGrid.Items[0];

            GanttChartItem itemAnalysis = GanttChartDataGrid.Items[1];
            itemAnalysis.Start = monthStart.AddHours(8);
            itemAnalysis.Finish = monthStart.AddDays(3).AddHours(16);
            itemAnalysis.CompletedFinish = itemAnalysis.Start;
            itemAnalysis.AssignmentsContent = "Clarissa Candelaria [50%]";

            GanttChartItem itemRequirements = GanttChartDataGrid.Items[2];
            itemRequirements.Start = monthStart.AddHours(8);
            itemRequirements.Finish = monthStart.AddDays(4).AddHours(16);
            itemRequirements.CompletedFinish = itemRequirements.Start;
            itemRequirements.AssignmentsContent = "Clarissa Candelaria [50%], Tyson Lamberson";

            GanttChartItem itemReview = GanttChartDataGrid.Items[3];
            itemReview.Start = monthStart.AddDays(5).AddHours(16);
            itemReview.CompletedFinish = itemReview.Start;
            itemReview.AssignmentsContent = "Clarissa Candelaria";

            GanttChartItem itemArhitecture = GanttChartDataGrid.Items[4];
            itemArhitecture.Start = monthStart.AddDays(3).AddHours(8);
            itemArhitecture.Finish = monthStart.AddDays(6).AddHours(12);
            itemArhitecture.CompletedFinish = itemArhitecture.Start;
            itemArhitecture.AssignmentsContent = "Steven Rush [50%], Meeting room";

            GanttChartItem itemDesign = GanttChartDataGrid.Items[5];
            itemDesign.Start = monthStart.AddDays(5).AddHours(10);
            itemDesign.Finish = monthStart.AddDays(8).AddHours(12);
            itemDesign.CompletedFinish = itemDesign.Start;
            itemDesign.AssignmentsContent = "Steven Rush [50%]";

            GanttChartItem itemDevelopment = GanttChartDataGrid.Items[6];

            GanttChartItem itemStartdevelopment = GanttChartDataGrid.Items[7];
            itemStartdevelopment.Start = monthStart.AddHours(8);
            itemStartdevelopment.CompletedFinish = itemStartdevelopment.Start;
            itemStartdevelopment.AssignmentsContent = "Steven Rush";

            GanttChartItem itemDatetimes = GanttChartDataGrid.Items[8];
            itemDatetimes.Start = monthStart.AddDays(3).AddHours(8);
            itemDatetimes.Finish = monthStart.AddDays(6).AddHours(12);
            itemDatetimes.CompletedFinish = monthStart.AddDays(4).AddHours(12);
            itemDatetimes.AssignmentsContent = "Joanna Mcamis";

            GanttChartItem itemSchedules = GanttChartDataGrid.Items[9];
            itemSchedules.Start = monthStart.AddDays(3).AddHours(8);
            itemSchedules.Finish = monthStart.AddDays(6).AddHours(12);
            itemSchedules.CompletedFinish = monthStart.AddDays(4).AddHours(12);
            itemSchedules.AssignmentsContent = "Clarissa Candelaria";

            GanttChartItem itemAutomationtestingfunctions = GanttChartDataGrid.Items[10];
            itemAutomationtestingfunctions.Start = monthStart.AddDays(3).AddHours(8);
            itemAutomationtestingfunctions.Finish = monthStart.AddDays(11).AddHours(12);
            itemAutomationtestingfunctions.CompletedFinish = itemAutomationtestingfunctions.Start;
            itemAutomationtestingfunctions.AssignmentsContent = "Tyson Lamberson [50%]";

            GanttChartItem itemChart = GanttChartDataGrid.Items[11];

            GanttChartItem itemBars = GanttChartDataGrid.Items[12];
            itemBars.Start = monthStart.AddDays(1).AddHours(8);
            itemBars.Finish = monthStart.AddDays(4).AddHours(16);
            itemBars.CompletedFinish = monthStart.AddDays(6).AddHours(16);
            itemBars.AssignmentsContent = "Clarissa Candelaria [50%]";

            GanttChartItem itemSummarybars = GanttChartDataGrid.Items[13];
            itemSummarybars.Start = monthStart.AddDays(5).AddHours(8);
            itemSummarybars.Finish = monthStart.AddDays(8).AddHours(16);
            itemSummarybars.CompletedFinish = itemSummarybars.Start;
            itemSummarybars.AssignmentsContent = "Steven Rush [50%]";

            GanttChartItem itemChartReview = GanttChartDataGrid.Items[14];
            itemChartReview.Start = monthStart.AddDays(8).AddHours(16);
            itemChartReview.CompletedFinish = itemChartReview.Start;

            GanttChartItem itemLinks = GanttChartDataGrid.Items[15];
            itemLinks.Start = monthStart.AddDays(6).AddHours(8);
            itemLinks.Finish = monthStart.AddDays(9).AddHours(16);
            itemLinks.CompletedFinish = itemLinks.Start;
            itemLinks.AssignmentsContent = "Steven Rush [50%]";

            GanttChartItem itemDiagramfunctions = GanttChartDataGrid.Items[16];
            itemDiagramfunctions.Start = monthStart.AddDays(4).AddHours(8);
            itemDiagramfunctions.Finish = monthStart.AddDays(7).AddHours(12);
            itemDiagramfunctions.CompletedFinish = itemDiagramfunctions.Start;
            itemDiagramfunctions.AssignmentsContent = "Tyson Lamberson [50%]";

            GanttChartItem itemQualityassurance = GanttChartDataGrid.Items[17];
            itemQualityassurance.Start = monthStart.AddDays(1).AddHours(8);
            itemQualityassurance.Finish = monthStart.AddDays(16).AddHours(16);
            itemQualityassurance.CompletedFinish = itemQualityassurance.Start;
            itemQualityassurance.AssignmentsContent = "Denis Kaelin, Printer";

            GanttChartItem itemProjectdelivery = GanttChartDataGrid.Items[18];
            itemProjectdelivery.Start = monthStart.AddDays(9).AddHours(8);
            itemProjectdelivery.Finish = monthStart.AddDays(11).AddHours(12);
            itemProjectdelivery.CompletedFinish = itemProjectdelivery.Start;
            itemProjectdelivery.AssignmentsContent = "Clarissa Candelaria, Meeting room";

            GanttChartItem itemMaintenance = GanttChartDataGrid.Items[19];
            itemMaintenance.Start = monthStart.AddDays(11).AddHours(12);
            itemMaintenance.Finish = monthStart.AddDays(17).AddHours(12);
            itemMaintenance.CompletedFinish = itemMaintenance.Start;

            GanttChartItem itemMarketing = GanttChartDataGrid.Items[20];
            itemMarketing.Start = monthStart.AddDays(9).AddHours(12);
            itemMarketing.Finish = monthStart.AddDays(14).AddHours(12);
            itemMarketing.CompletedFinish = itemMarketing.Start;

            GanttChartItem itemPreparations = GanttChartDataGrid.Items[21];
            itemPreparations.Start = monthStart.AddDays(9).AddHours(8);
            itemPreparations.CompletedFinish = itemPreparations.Start;
            itemPreparations.AssignmentsContent = "Joanna Mcamis, Alicia Rock";

            GanttChartItem itemColors = GanttChartDataGrid.Items[22];
            itemColors.Start = monthStart.AddDays(10).AddHours(8);
            itemColors.Finish = monthStart.AddDays(13).AddHours(12);
            itemColors.CompletedFinish = itemColors.Start;
            itemColors.AssignmentsContent = "Joanna Mcamis [25%]";

            GanttChartItem itemLogo = GanttChartDataGrid.Items[23];
            itemLogo.Start = monthStart.AddDays(10).AddHours(8);
            itemLogo.Finish = monthStart.AddDays(13).AddHours(12);
            itemLogo.CompletedFinish = itemLogo.Start;
            itemLogo.AssignmentsContent = "Alicia Rock [25%]";

            GanttChartItem itemSamplesapp = GanttChartDataGrid.Items[24];
            itemSamplesapp.Start = monthStart.AddDays(10).AddHours(8);
            itemSamplesapp.Finish = monthStart.AddDays(15).AddHours(12);
            itemSamplesapp.CompletedFinish = itemSamplesapp.Start;
            itemSamplesapp.AssignmentsContent = "Clarissa Candelaria";

            GanttChartItem itemScreenshots = GanttChartDataGrid.Items[25];
            itemScreenshots.Start = monthStart.AddDays(11).AddHours(8);
            itemScreenshots.Finish = monthStart.AddDays(14).AddHours(16);
            itemScreenshots.CompletedFinish = itemScreenshots.Start;
            itemScreenshots.AssignmentsContent = "Joanna Mcamis";

            GanttChartItem itemVideos = GanttChartDataGrid.Items[26];
            itemVideos.Start = monthStart.AddDays(14).AddHours(8);
            itemVideos.Finish = monthStart.AddDays(17).AddHours(16);
            itemVideos.CompletedFinish = itemVideos.Start;
            itemVideos.AssignmentsContent = "Alicia Rock [50%]";

            itemDevelopment.Predecessors.Add(new PredecessorItem { Item = itemPlanning, DependencyType = DependencyType.StartStart });
            itemArhitecture.Predecessors.Add(new PredecessorItem { Item = itemRequirements, Lag = TimeSpan.FromHours(2) });
            itemDatetimes.Predecessors.Add(new PredecessorItem { Item = itemArhitecture });
            itemDatetimes.Predecessors.Add(new PredecessorItem { Item = itemDesign });
            itemAutomationtestingfunctions.Predecessors.Add(new PredecessorItem { Item = itemSchedules });

            // Initialize baselines for the tasks used by the baseline display example.
            itemStartdevelopment.BaselineStart = itemStartdevelopment.Start;
            itemDatetimes.BaselineStart = itemDatetimes.Start;
            itemDatetimes.BaselineFinish = itemDatetimes.Finish;
            itemSchedules.BaselineStart = itemSchedules.Start;
            itemSchedules.BaselineFinish = itemSchedules.Finish;
            itemAutomationtestingfunctions.BaselineStart = itemAutomationtestingfunctions.Start;
            itemAutomationtestingfunctions.BaselineFinish = itemAutomationtestingfunctions.Finish;

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
            // GanttChartDataGrid.NonworkingDayIntervalProvider = (date) =>
            // {
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

            // You may uncomment the next lines of code to set specific schedule settings for Schedules.
            // itemSchedules.Schedule = new Schedule(
            //    DayOfWeek.Sunday, DayOfWeek.Thursday, // Working week: between Sunday and Thursday.
            //    TimeSpan.Parse("10:00:00"), TimeSpan.Parse("14:00:00"), // Working day: between 10 AM and 2 PM.
            //    new TimeInterval[] { // Optionally, generic nonworking intervals.
            //        new TimeInterval(DateTime.Today.AddDays(4), DateTime.Today.AddDays(4).Add(TimeOfDay.MaxValue)), // Holiday: full day.
            //        new TimeInterval(DateTime.Today.AddDays(8), DateTime.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"))) // Custom time interval off: full and partial day accepted.
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

            // Optionally, disable auto-scheduling for individual items (i.e. turn on manual scheduling) by setting item.AreDependencyConstraintsEnabled = false (overriding component.AreTaskDependencyConstraintsEnabled == true).
            // itemRequirements.AreDependencyConstraintsEnabled = false;

            // You may uncomment the next lines of code to test the component performance:
            // for (int i = 26; i <= 4096; i++)
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
            GanttChartDataGrid.AssignableResources = new ObservableCollection<string> { "Clarissa Candelaria", "Tyson Lamberson", "Steven Rush", 
                                                                                        "Joanna Mcamis", "Denis Kaelin", "Alicia Rock", "Meeting room", "Printer" };
            // Optionally, display only images for assigned resources, instead of images and names (names are displayed as tooltips).
            //GanttChartDataGrid.DisplayAssignedResourceImagesOnly = true;
            GanttChartDataGrid.ResourceImages = new Dictionary<string, ImageSource>
            {
                { "Clarissa Candelaria", new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Clarissa.png", applicationName), UriKind.Absolute)) },
                { "Tyson Lamberson", new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Tyson.png", applicationName), UriKind.Absolute)) },
                { "Steven Rush", new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Steven.png", applicationName), UriKind.Absolute)) },
                { "Joanna Mcamis", new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Joanna.png", applicationName), UriKind.Absolute)) },
                { "Denis Kaelin", new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Denis.png", applicationName), UriKind.Absolute)) },
                { "Alicia Rock", new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Alicia.png", applicationName), UriKind.Absolute)) },
                { "Meeting room", new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/MeetingRoom.png", applicationName), UriKind.Absolute)) },
                { "Printer", new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Printer.png", applicationName), UriKind.Absolute)) }
            };

            // Optionally, define the quantity values to consider when leveling resources, indicating maximum material amounts available for use at the same time.
            GanttChartDataGrid.ResourceQuantities = new Dictionary<string, double> { { "Meeting room", 4 }, { "Printer", double.PositiveInfinity } };

            // Optionally, define task and resource costs.
            // GanttChartDataGrid.TaskInitiationCost = 5;
            itemArhitecture.ExecutionCost = 50;
            // GanttChartDataGrid.DefaultResourceUsageCost = 1;
            // GanttChartDataGrid.SpecificResourceUsageCosts = new Dictionary<string, double> { { "Clarissa Candelaria", 2 }, { "Meeting room", 7 } };
            GanttChartDataGrid.DefaultResourceHourCost = 10;
            GanttChartDataGrid.SpecificResourceHourCosts = new Dictionary<string, double> { { "Clarissa Candelaria", 20 }, { "Printer", 0.5 } };

            // Optionally, set HasFixedEffort to automatically update item assignment allocation units rather than effort upon duration changes.
            // itemStartdevelopment.HasFixedEffort = true;

            // Optionally, set RequiresMinSchedulingBreaks to ensure specific items are scheduled with the minimum possible number of nonworking time breaks (e.g. nights, weekends).
            // itemStartdevelopment.RequiresMinSchedulingBreaks = true;

            // Optionally, set LevelingPrority values to be considered upon leveling resources (default is zero).
            // itemStartdevelopment.LevelingPriority = 1000;

            // Optionally, set AreHierarchyConstraintsEnabled to false to increase performance when you perform hierarchy validation in your application.
            GanttChartDataGrid.AreHierarchyConstraintsEnabled = false;

            // Initialize the control area.
            ScalesComboBox.SelectedIndex = 0;
            ShowWeekendsCheckBox.IsChecked = true;
            BaselineCheckBox.IsChecked = true;
            EnableDependencyConstraintsCheckBox.IsChecked = true;
        }

        private ResourceDictionary themeResourceDictionary;
        private string theme = "Coastal-blue";
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
            if (themeResourceDictionary != null)
            {
                GanttChartDataGrid.Resources.MergedDictionaries.Remove(themeResourceDictionary);
                themeResourceDictionary = null;
            }
            if (theme == null || theme == "Default" || theme == "Aero")
                return;
            themeResourceDictionary = new ResourceDictionary { Source = new Uri("/" + GetType().Assembly.GetName().Name + ";component/Themes/" + theme + ".xaml", UriKind.Relative) };
            GanttChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));
        public static bool GetIsSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectedProperty);
        }
        public static void SetIsSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSelectedProperty, value);
        }

        private GanttChartItem highlightedItem;
        private DataTemplate taskToolTipTemplate;
        private bool isCompletionThumbDragging;

        private void CompletionThumb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!isCompletionThumbDragging)
                taskToolTipTemplate = GanttChartDataGrid.ToolTipTemplate;

            isCompletionThumbDragging = true;
            GanttChartDataGrid.ToolTipTemplate = Resources["CompletionOnlyToolTipTemplate"] as DataTemplate;
        }

        private void CompletionThumb_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RestoreTaskToolTipTemplate();
        }

        private void CompletionThumb_LostMouseCapture(object sender, MouseEventArgs e)
        {
            RestoreTaskToolTipTemplate();
        }

        private void RestoreTaskToolTipTemplate()
        {
            if (!isCompletionThumbDragging)
                return;

            isCompletionThumbDragging = false;
            if (taskToolTipTemplate != null)
                GanttChartDataGrid.ToolTipTemplate = taskToolTipTemplate;
        }

        private void GanttChartDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RestoreTaskToolTipTemplate();

            Point controlPosition = e.GetPosition(GanttChartDataGrid);
            if (controlPosition.X < GanttChartDataGrid.ActualWidth - GanttChartDataGrid.GanttChartView.ActualWidth)
                return;

            var frameworkElement = e.OriginalSource as FrameworkElement;
            var item = frameworkElement?.DataContext as GanttChartItem;

            if (highlightedItem != null)
            {
                SetIsSelected(highlightedItem, false);
                highlightedItem = null;
            }

            if (item == null)
                return;

            highlightedItem = item;
            SetIsSelected(highlightedItem, true);
        }

        // Control area commands.
        private void GanttChartDataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            ApplyCellEditorPadding(e.EditingElement);
        }

        private static void ApplyCellEditorPadding(DependencyObject element)
        {
            // Date/time editors manage their own spacing, including the popup controls.
            if (element is DateTimePicker)
                return;

            var textBox = element as TextBox;
            if (textBox != null)
            {
                textBox.Padding = new Thickness(4, 5, 4, 5);
                textBox.VerticalContentAlignment = VerticalAlignment.Center;
                return;
            }

            // Include the task-name editor generated by the hierarchical column.
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
                ApplyCellEditorPadding(VisualTreeHelper.GetChild(element, i));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartItem selectedItem = GanttChartDataGrid.SelectedItem as GanttChartItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Cannot edit as the selection is empty; you should select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            EditItemDialog editItemDialog = new EditItemDialog { Owner = Application.Current.MainWindow, DataContext = selectedItem, Chart = GanttChartDataGrid, AssignableResources = GanttChartDataGrid.AssignableResources };
            editItemDialog.ShowDialog();
        }
        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartItem item = new GanttChartItem { Content = "New Task", Start = DateTime.Today, Finish = DateTime.Today.AddDays(1) };
            GanttChartDataGrid.Items.Add(item);
            GanttChartDataGrid.SelectedItem = item;
            GanttChartDataGrid.ScrollTo(item);
            GanttChartDataGrid.ScrollTo(item.Start);
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
            GanttChartDataGrid.Items.Insert(selectedItem.Index, item);
            GanttChartDataGrid.SelectedItem = item;
            GanttChartDataGrid.ScrollTo(item);
            GanttChartDataGrid.ScrollTo(item.Start);
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
            var removableItems = new List<GanttChartItem>();
            foreach (GanttChartItem item in items)
            {
                if (item.HasChildren)
                {
                    MessageBox.Show(string.Format("Cannot delete {0} because it has child items; remove its child items first.", item), "Information", MessageBoxButton.OK);
                    continue;
                }
                removableItems.Add(item);
            }
            if (removableItems.Count == 0)
                return;

            foreach (var item in removableItems)
                GanttChartDataGrid.Items.Remove(item);
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
            // GanttChartDataGrid.BeginInit();
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
            // GanttChartDataGrid.EndInit();
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
            // GanttChartDataGrid.BeginInit();
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
            // GanttChartDataGrid.EndInit();
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
                MessageBox.Show("Cannot move selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            var selectedItems = GanttChartDataGrid.GetSelectedItems().ToArray();
            if (selectedItems.Select(i => i.Parent).Distinct().Count() > 1)
            {
                MessageBox.Show("Cannot move selected item(s) as the selection includes items that have different parents; select only root items or only items having the same parent.", "Information", MessageBoxButton.OK);
                return;
            }
            // GanttChartDataGrid.BeginInit();
            foreach (GanttChartItem item in selectedItems)
                GanttChartDataGrid.MoveUp(item, true, true);
            // GanttChartDataGrid.EndInit();
            GanttChartDataGrid.SelectedItems.Clear();
            foreach (GanttChartItem item in selectedItems)
                GanttChartDataGrid.SelectedItems.Add(item);
        }
        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (GanttChartDataGrid.GetSelectedItemCount() <= 0)
            {
                MessageBox.Show("Cannot move selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            var selectedItems = GanttChartDataGrid.GetSelectedItems().Reverse().ToArray();
            if (selectedItems.Select(i => i.Parent).Distinct().Count() > 1)
            {
                MessageBox.Show("Cannot move selected item(s) as the selection includes items that have different parents; select only root items or only items having the same parent.", "Information", MessageBoxButton.OK);
                return;
            }
            // GanttChartDataGrid.BeginInit();
            foreach (GanttChartItem item in selectedItems)
                GanttChartDataGrid.MoveDown(item, true, true);
            // GanttChartDataGrid.EndInit();
            GanttChartDataGrid.SelectedItems.Clear();
            foreach (GanttChartItem item in selectedItems)
                GanttChartDataGrid.SelectedItems.Add(item);
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
            GanttChartDataGrid.ScrollToVerticalOffset(0);
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
