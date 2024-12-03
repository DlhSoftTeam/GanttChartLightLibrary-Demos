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
using Microsoft.Win32;
using System.IO;
using DlhSoft.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Collections;

namespace Demos.WPF.CSharp.ScheduleChartDataGrid.MainFeatures
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

            #region ganttchartitems
            ScheduleChartItem unassignedScheduleChartItem = ScheduleChartDataGrid.Items[0];

            CustomGanttChartItem item1 = new CustomGanttChartItem
            { Content = "Project delivery", Start = DateTime.Today.AddDays(1), Finish = DateTime.Today.AddDays(5) };
            unassignedScheduleChartItem.GanttChartItems.Add(item1);

            CustomGanttChartItem item2 = new CustomGanttChartItem
            { Content = "Maintaince", Start = DateTime.Today.AddDays(2), Finish = DateTime.Today.AddDays(6) };
            unassignedScheduleChartItem.GanttChartItems.Add(item2);

            CustomGanttChartItem item3 = new CustomGanttChartItem
            { Content = "Marketing", Start = DateTime.Today.AddDays(3), IsMilestone = true };
            unassignedScheduleChartItem.GanttChartItems.Add(item3);

            CustomGanttChartItem item4 = new CustomGanttChartItem
            { Content = "Colors", Start = DateTime.Today.AddDays(4), Finish = DateTime.Today.AddDays(8) };
            unassignedScheduleChartItem.GanttChartItems.Add(item4);

            CustomGanttChartItem item5 = new CustomGanttChartItem
            { Content = "Logo", Start = DateTime.Today.AddDays(5), Finish = DateTime.Today.AddDays(9) };
            unassignedScheduleChartItem.GanttChartItems.Add(item5);

            CustomGanttChartItem item6 = new CustomGanttChartItem
            {
                Content = "Samples app",
                Start = DateTime.Today.AddDays(6),
                Finish = DateTime.Today.AddDays(10),
                Icon = new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Flag.png", applicationName), UriKind.Absolute))
            };
            unassignedScheduleChartItem.GanttChartItems.Add(item6);

            CustomGanttChartItem item7 = new CustomGanttChartItem
            { Content = "Screenshots", Start = DateTime.Today.AddDays(7), Finish = DateTime.Today.AddDays(11) };
            unassignedScheduleChartItem.GanttChartItems.Add(item7);

            CustomGanttChartItem item8 = new CustomGanttChartItem
            { Content = "Videos", Start = DateTime.Today.AddDays(8), Finish = DateTime.Today.AddDays(12) };
            unassignedScheduleChartItem.GanttChartItems.Add(item8);

            #endregion

            CustomGanttChartItem task21 = ScheduleChartDataGrid.Items[2].GanttChartItems[0] as CustomGanttChartItem;
            task21.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            task21.Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00"));
            task21.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem task22 = ScheduleChartDataGrid.Items[2].GanttChartItems[1] as CustomGanttChartItem;
            task22.Start = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("08:00:00"));
            task22.Finish = DateTime.Today.AddDays(8).Add(TimeSpan.Parse("16:00:00"));
            task22.CompletedFinish = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"));
            task22.Icon = new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Person.png", applicationName), UriKind.Absolute));
            task22.Note = "This task is very important.";

            var pred21 = new PredecessorItem();
            pred21.Item = task21;
            task22.Predecessors.Add(pred21);

            CustomGanttChartItem task31 = ScheduleChartDataGrid.Items[3].GanttChartItems[0] as CustomGanttChartItem;
            task31.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            task31.Finish = DateTime.Today.AddDays(8).Add(TimeSpan.Parse("16:00:00"));
            task31.Icon = new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Person.png", applicationName), UriKind.Absolute));
            task31.AssignmentsContent = "50%";

            CustomGanttChartItem task51 = ScheduleChartDataGrid.Items[5].GanttChartItems[0] as CustomGanttChartItem;
            task51.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            task51.Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00"));
            task51.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem task52 = ScheduleChartDataGrid.Items[5].GanttChartItems[1] as CustomGanttChartItem;
            task52.Start = DateTime.Today.AddDays(5).Add(TimeSpan.Parse("12:00:00"));
            task52.Finish = DateTime.Today.AddDays(8).Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem task53 = ScheduleChartDataGrid.Items[5].GanttChartItems[2] as CustomGanttChartItem;
            task53.Start = DateTime.Today.AddDays(9).Add(TimeSpan.Parse("12:00:00"));
            task53.Finish = DateTime.Today.AddDays(12).Add(TimeSpan.Parse("16:00:00"));

            var pred51 = new PredecessorItem { Item = task51 };
            task52.Predecessors.Add(pred51);

            var succ53 = new PredecessorItem { Item = task53, DependencyType = DependencyType.FinishFinish };
            task52.Predecessors.Add(succ53);

            CustomGanttChartItem task61 = ScheduleChartDataGrid.Items[6].GanttChartItems[0] as CustomGanttChartItem;
            task61.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            task61.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            task61.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem task62 = ScheduleChartDataGrid.Items[6].GanttChartItems[1] as CustomGanttChartItem;
            task62.Start = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));
            task62.Finish = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"));
            task62.Icon = new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Flag.png", applicationName), UriKind.Absolute));

            CustomGanttChartItem task63 = ScheduleChartDataGrid.Items[6].GanttChartItems[2] as CustomGanttChartItem;
            task63.Start = DateTime.Today.AddDays(7).Add(TimeSpan.Parse("12:00:00"));
            task63.Finish = DateTime.Today.AddDays(9).Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem task71 = ScheduleChartDataGrid.Items[7].GanttChartItems[0] as CustomGanttChartItem;
            task71.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            task71.Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00"));
            task71.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem task81 = ScheduleChartDataGrid.Items[8].GanttChartItems[0] as CustomGanttChartItem;
            task81.Start = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));
            task81.Finish = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem task91 = ScheduleChartDataGrid.Items[9].GanttChartItems[0] as CustomGanttChartItem;
            task91.Start = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));
            task91.Finish = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem task111 = ScheduleChartDataGrid.Items[11].GanttChartItems[0] as CustomGanttChartItem;
            task111.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            task111.Finish = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"));
            task111.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem task112 = ScheduleChartDataGrid.Items[11].GanttChartItems[1] as CustomGanttChartItem;
            task112.Start = DateTime.Today.AddDays(7).Add(TimeSpan.Parse("12:00:00"));
            task112.Finish = DateTime.Today.AddDays(12).Add(TimeSpan.Parse("16:00:00"));

            //Code for enable creation of dependencies between tasks
            ScheduleChartDataGrid.DependencyCreationValidator = (i1, i2) => i1 != i2;

            // You may uncomment the next lines of code to test the component performance:
            // for (int i = 17; i <= 1024; i++)
            // {
            //    ScheduleChartItem item = new ScheduleChartItem { Content = "Resource " + i };
            //    for (int j = 1; j <= 10; j++)
            //    {
            //        item.GanttChartItems.Add(
            //            new GanttChartItem
            //            {
            //                Content = "T " + j,
            //                Start = DateTime.Today.AddDays(j).Add(TimeSpan.Parse("08:00:00")),
            //                Finish = DateTime.Today.AddDays(j).Add(TimeSpan.Parse("14:00:00"))
            //            });
            //    }
            //    ScheduleChartDataGrid.Items.Add(item);
            // }

            // Optionally, define custom schedules for resources, used when scheduling items assigned to those resources.
            // ScheduleChartDataGrid.Items[1].Schedule = new Schedule(
            //     DayOfWeek.Tuesday, DayOfWeek.Saturday, // Working week: between Tuesday and Saturday.
            //     TimeSpan.Parse("07:00:00"), TimeSpan.Parse("15:00:00"), // Working day: between 7 AM and 3 PM.
            //     new TimeInterval[] { // Optionally, generic nonworking intervals.
            //         new TimeInterval(DateTime.Today.AddDays(14), DateTime.Today.AddDays(14).Add(TimeOfDay.MaxValue)), // Holiday: full day.
            //         new TimeInterval(DateTime.Today.AddDays(18), DateTime.Today.AddDays(20).Add(TimeSpan.Parse("12:00:00"))) // Custom time interval off: full and partial day accepted.
            //     },
            //     (date) => { // Optionally, specific nonworking intervals based on date parameter: recurrent breaks and holidays accepted.
            //         if (date.Day % 15 == 0) // First recurrence expression: on decade end days.
            //             return new DayTimeInterval[] { 
            //                 new DayTimeInterval(TimeOfDay.MinValue, TimeOfDay.Parse("12:00:00")), // Large interval off: first part of day.
            //                 new DayTimeInterval(TimeOfDay.Parse("12:00:00"), TimeOfDay.Parse("12:30:00")) // Short break: fast lunch time.
            //             };
            //         else if (date.DayOfWeek != DayOfWeek.Monday) // Second recurrence expression: every day except Mondays.
            //             return new DayTimeInterval[] { 
            //                 new DayTimeInterval(TimeOfDay.Parse("11:30:00"), TimeOfDay.Parse("12:30:00")) // Break: regular lunch time.
            //             };
            //         return null; // Otherwise use regular timing only.
            //     });

            // Optionally, set AreHierarchyConstraintsEnabled to false to increase performance when you perform hierarchy validation in your application.
            ScheduleChartDataGrid.AreHierarchyConstraintsEnabled = false;

            // Initialize the control area.
            ScalesComboBox.SelectedIndex = 0;
            ShowWeekendsCheckBox.IsChecked = true;
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
            ScheduleChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary);
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

        private GanttChartItem SelectedItem;

        private void ScheduleChartDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point controlPosition = e.GetPosition(ScheduleChartDataGrid);
            if (controlPosition.X < ScheduleChartDataGrid.ActualWidth - ScheduleChartDataGrid.GanttChartView.ActualWidth)
                return;

            GanttChartItem item = null;
            FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
            if (frameworkElement != null)
                item = frameworkElement.DataContext as GanttChartItem;

            if (SelectedItem != null)
            {
                SetIsSelected(SelectedItem, false);
                SelectedItem = null;
            }

            if (item == null)
                return;

            SelectedItem = item;
            SetIsSelected(SelectedItem, true);
        }

        public static readonly DependencyProperty OpacityProperty = DependencyProperty.RegisterAttached("Opacity", typeof(double), typeof(MainWindow), new PropertyMetadata(1.0));
        public static double GetOpacity(DependencyObject obj)
        {
            return (double)obj.GetValue(OpacityProperty);
        }
        public static void SetOpacity(DependencyObject obj, double value)
        {
            obj.SetValue(OpacityProperty, value);
        }
        // Control area commands.
        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleChartItem item = new ScheduleChartItem { Content = "New Resource" };
            item.GanttChartItems.Add(new GanttChartItem { Content = "New Task", Start = DateTime.Today, Finish = DateTime.Today.AddDays(1) });
            ScheduleChartDataGrid.Items.Add(item);
            ScheduleChartDataGrid.SelectedItem = item;
            ScheduleChartDataGrid.ScrollTo(item.GanttChartItems[0]);
            ScheduleChartDataGrid.ScrollTo(item.GanttChartItems[0].Start);
        }
        private void InsertNewButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleChartItem selectedItem = ScheduleChartDataGrid.SelectedItem as ScheduleChartItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Cannot insert a new item before selection as the selection is empty; you can either add a new item to the end of the list instead, or select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            ScheduleChartItem item = new ScheduleChartItem { Content = "New Resource" };
            item.GanttChartItems.Add(new GanttChartItem { Content = "New Task", Start = DateTime.Today, Finish = DateTime.Today.AddDays(1) });
            ScheduleChartDataGrid.Items.Insert(selectedItem.Index, item);
            ScheduleChartDataGrid.SelectedItem = item;
            ScheduleChartDataGrid.ScrollTo(item.GanttChartItems[0]);
            ScheduleChartDataGrid.ScrollTo(item.GanttChartItems[0].Start);
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            List<ScheduleChartItem> items = new List<ScheduleChartItem>();
            foreach (ScheduleChartItem item in ScheduleChartDataGrid.GetSelectedItems())
                items.Add(item);
            if (items.Count <= 0)
            {
                MessageBox.Show("Cannot delete the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            items.Reverse();
            foreach (ScheduleChartItem item in items)
                ScheduleChartDataGrid.Items.Remove(item);
        }

        private void SetColorButton_Click(object sender, RoutedEventArgs e)
        {
            List<ScheduleChartItem> items = new List<ScheduleChartItem>();
            foreach (ScheduleChartItem item in ScheduleChartDataGrid.GetSelectedItems())
                items.Add(item);
            if (items.Count <= 0)
            {
                MessageBox.Show("Cannot set a custom bar color to the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            foreach (ScheduleChartItem item in items)
            {
                foreach (GanttChartItem ganttChartItem in item.GanttChartItems)
                {
                    GanttChartView.SetStandardBarFill(ganttChartItem, Resources["CustomStandardBarFill"] as Brush);
                    GanttChartView.SetStandardBarStroke(ganttChartItem, Resources["CustomStandardBarStroke"] as Brush);
                }
            }
        }
        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (ScheduleChartDataGrid.GetSelectedItemCount() <= 0)
            {
                MessageBox.Show("Cannot copy selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            ScheduleChartDataGrid.Copy();
        }
        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleChartDataGrid.Paste();
        }
        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            if (ScheduleChartDataGrid.CanUndo())
                ScheduleChartDataGrid.Undo();
            else
                MessageBox.Show("Currently there is no recorded action in the undo queue; perform an action first.", "Information", MessageBoxButton.OK);
        }
        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            if (ScheduleChartDataGrid.CanRedo())
                ScheduleChartDataGrid.Redo();
            else
                MessageBox.Show("Currently there is no recorded action in the redo queue; perform an action and undo it first.", "Information", MessageBoxButton.OK);
        }
        private void ScaleTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = ScalesComboBox.SelectedItem as ComboBoxItem;
            string scalesResourceKey = selectedComboBoxItem.Tag as string;
            ScaleCollection scales = Resources[scalesResourceKey] as ScaleCollection;
            ScheduleChartDataGrid.Scales = scales;
        }
        private void ZoomCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            originalZoom = ScheduleChartDataGrid.HourWidth;
            ScheduleChartDataGrid.HourWidth = originalZoom * 2;
        }
        private void ZoomCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ScheduleChartDataGrid.HourWidth = originalZoom;
        }
        private double originalZoom;
        private void IncreaseTimelinePageButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleChartDataGrid.TimelinePageFinish += pageUpdateAmount;
            ScheduleChartDataGrid.TimelinePageStart += pageUpdateAmount;
        }
        private void DecreaseTimelinePageButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleChartDataGrid.TimelinePageFinish -= pageUpdateAmount;
            ScheduleChartDataGrid.TimelinePageStart -= pageUpdateAmount;
        }
        private readonly TimeSpan pageUpdateAmount = TimeSpan.FromDays(7);
        private void ShowWeekendsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ScheduleChartDataGrid.VisibleWeekStart = DayOfWeek.Sunday;
            ScheduleChartDataGrid.VisibleWeekFinish = DayOfWeek.Saturday;
            WorkOnWeekendsCheckBox.IsEnabled = true;
        }
        private void ShowWeekendsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            WorkOnWeekendsCheckBox.IsChecked = false;
            WorkOnWeekendsCheckBox.IsEnabled = false;
            ScheduleChartDataGrid.VisibleWeekStart = DayOfWeek.Monday;
            ScheduleChartDataGrid.VisibleWeekFinish = DayOfWeek.Friday;
        }
        private void WorkOnWeekendsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ScheduleChartDataGrid.WorkingWeekStart = DayOfWeek.Sunday;
            ScheduleChartDataGrid.WorkingWeekFinish = DayOfWeek.Saturday;
        }
        private void WorkOnWeekendsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ScheduleChartDataGrid.WorkingWeekStart = DayOfWeek.Monday;
            ScheduleChartDataGrid.WorkingWeekFinish = DayOfWeek.Friday;
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            var oldStart = ScheduleChartDataGrid.TimelinePageStart;
            var oldFinish = ScheduleChartDataGrid.TimelinePageFinish; 

            try
            {
                var dialog = new System.Windows.Controls.PrintDialog();
                // Optional: dialog.PrintTicket.PageOrientation = PageOrientation.Landscape;

                var timelinePageStart = ScheduleChartDataGrid.GetProjectStart().AddDays(-1);
                var timelinePageFinish = ScheduleChartDataGrid.GetProjectFinish().AddDays(1);

                var timelineHours = ScheduleChartDataGrid.GetEffort(ScheduleChartDataGrid.GetProjectStart(), ScheduleChartDataGrid.GetProjectFinish(), ScheduleChartDataGrid.GetVisibilitySchedule()).TotalHours;
                var gridWidth = ScheduleChartDataGrid.Columns.Sum(c => c.ActualWidth);

                if (dialog.ShowDialog() == true)
                {
                    ScheduleChartDataGrid.SetTimelinePage(timelinePageStart, timelinePageFinish);
                    var exportedSize = ScheduleChartDataGrid.GetExportSize();

                    //Printing on a single page, if the content fits (considering the margins defined in PrintingTemplate as well).
                    if (exportedSize.Width + 2 * 48 <= dialog.PrintableAreaWidth && exportedSize.Height + 2 * 32 <= dialog.PrintableAreaHeight)
                    {
                        ScheduleChartDataGrid.Export((Action)delegate
                        {
                            // Get a DrawingVisual representing the Gantt Chart content.
                            var exportedVisual = ScheduleChartDataGrid.GetExportDrawingVisual();
                            // Apply necessary transforms for the content to fit into the output page.
                            exportedVisual.Transform = GetPageFittingTransform(dialog);
                            // Actually print the visual.
                            var container = new Border();
                            container.Padding = new Thickness(48, 32, 48, 32);
                            container.Child = new Rectangle { Fill = new VisualBrush(exportedVisual), Width = exportedSize.Width, Height = exportedSize.Height };
                            dialog.PrintVisual(container, "Schedule Chart Document");
                        });
                    }
                    else
                    {
                        var documentPaginator = new DlhSoft.Windows.Controls.GanttChartDataGrid.DocumentPaginator(ScheduleChartDataGrid);
                        documentPaginator.PageSize = new Size(dialog.PrintableAreaWidth, dialog.PrintableAreaHeight);
                        dialog.PrintDocument(documentPaginator, "Schedule Chart Document");
                    }

                    Close();
                }
            }
            finally
            {
                Dispatcher.BeginInvoke((Action)delegate
                {
                    ScheduleChartDataGrid.SetTimelinePage(oldStart, oldFinish);
                });
            }
            //ScheduleChartDataGrid.Print("ScheduleChartDataGrid Sample Document");
        }
        private TransformGroup GetPageFittingTransform(System.Windows.Controls.PrintDialog printDialog)
        {
            // Determine scale to apply for page fitting.
            var scale = GetPageFittingScaleRatio(printDialog);
            // Set up a transform group in order to allow multiple transforms, if needed.
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform(scale, scale));
            // Optionally, add other transforms, such as supplemental translation, scale, or rotation as you need for the output presentation.
            return transformGroup;
        }

        private double GetPageFittingScaleRatio(System.Windows.Controls.PrintDialog printDialog)
        {
            // Determine the appropriate scale to apply based on export size and printable area size.
            var outputSize = ScheduleChartDataGrid.GetExportSize();
            var scaleX = printDialog.PrintableAreaWidth / outputSize.Width;
            var scaleY = printDialog.PrintableAreaHeight / outputSize.Height;
            var scale = Math.Min(scaleX, scaleY);
            return scale;
        }
        private void ExportImageButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleChartDataGrid.Export((Action)delegate
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog { Title = "Export Image To", Filter = "PNG image files|*.png" };
                if (saveFileDialog.ShowDialog() != true)
                    return;
                BitmapSource bitmapSource = ScheduleChartDataGrid.GetExportBitmapSource(96 * 2);
                using (Stream stream = saveFileDialog.OpenFile())
                {
                    PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
                    pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    pngBitmapEncoder.Save(stream);
                }
            });
        }
        private void AddNewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleChartItem selectedItem = ScheduleChartDataGrid.SelectedItem as ScheduleChartItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Cannot add a new task if the selection of a resource is empty; you can either add a new resource to the end of the list instead, or select an item first.", "Information", MessageBoxButton.OK);
                return;
            }

            GanttChartItem task = new CustomGanttChartItem { Content = "New Task", Start = DateTime.Today, Finish = DateTime.Today.AddDays(1) };
            selectedItem.GanttChartItems.Insert(0, task);
            ScheduleChartDataGrid.ScrollTo(task.Start);
        }

        private static void DeleteTask(CustomGanttChartItem itemToDelete)
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(5) };
            timer.Tick += (ts, te) =>
            {
                var opacity = GetOpacity(itemToDelete);
                opacity -= 0.05;
                if (opacity <= 0)
                {
                    timer.Stop();

                    var parent = itemToDelete.ScheduleChartItem;
                    parent.GanttChartItems.Remove(itemToDelete);
                }
                SetOpacity(itemToDelete, opacity);
            };
            timer.Start();
        }

        private void DeleteTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CustomGanttChartItem itemToDelete = (sender as MenuItem).DataContext as CustomGanttChartItem;
            if (itemToDelete == null) { return; }
            if (MessageBox.Show("Are you sure you want to delete this task?", "Question", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            DeleteTask(itemToDelete);
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem == null) {
                MessageBox.Show("Cannot delete a task if the selection is empty.", "Information", MessageBoxButton.OK);
                return; 
            }

            CustomGanttChartItem itemToDelete = SelectedItem as CustomGanttChartItem;
            if (itemToDelete == null) { return; }
            if (MessageBox.Show("Are you sure you want to delete this task?", "Question", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            DeleteTask(itemToDelete);
        }
    }
}
