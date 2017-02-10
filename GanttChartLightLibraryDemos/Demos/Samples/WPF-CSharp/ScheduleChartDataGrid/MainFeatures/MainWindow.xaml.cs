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

            GanttChartItem task1 = ScheduleChartDataGrid.Items[0].GanttChartItems[0];
            task1.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            task1.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            task1.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));

            GanttChartItem task21 = ScheduleChartDataGrid.Items[0].GanttChartItems[1];
            task21.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            task21.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            task21.AssignmentsContent = "50%";

            GanttChartItem task22 = ScheduleChartDataGrid.Items[1].GanttChartItems[0];
            task22.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            task22.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));

            for (int i = 3; i <= 16; i++)
            {
                ScheduleChartItem item = new ScheduleChartItem { Content = "Resource " + i };
                for (int j = 1; j <= (i - 1) % 4 + 1; j++)
                {
                    item.GanttChartItems.Add(
                        new GanttChartItem
                        {
                            Content = "Task " + i + "." + j,
                            Start = DateTime.Today.AddDays(i + (i - 1) * (j - 1)),
                            Finish = DateTime.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1),
                            CompletedFinish = DateTime.Today.AddDays(i + (i - 1) * (j - 1)).AddDays((i + j) % 5 == 2 ? 2 : 0)
                        });
                }
                ScheduleChartDataGrid.Items.Add(item);
            }

            // You may uncomment the next lines of code to test the component performance:
            // for (int i = 17; i <= 1024; i++)
            // {
            //    ScheduleChartItem item = new ScheduleChartItem { Content = "Resource " + i };
            //    for (int j = 1; j <= (i - 1) % 4 + 1; j++)
            //    {
            //        item.GanttChartItems.Add(
            //            new GanttChartItem
            //            {
            //                Content = "Task " + i + "." + j,
            //                Start = DateTime.Today.AddDays(i + (i - 1) * (j - 1)),
            //                Finish = DateTime.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1)
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

            ScheduleChartDataGrid.DisplayedTime = DateTime.Today.AddDays(-1);
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

        // Control area commands.
        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleChartItem item = new ScheduleChartItem { Content = "New Resource" };
            item.GanttChartItems.Add(new GanttChartItem { Content = "New Task", Start = DateTime.Today, Finish = DateTime.Today.AddDays(1) });
            ScheduleChartDataGrid.Items.Add(item);
            ScheduleChartDataGrid.SelectedItem = item;
            ScheduleChartDataGrid.ScrollTo(item.GanttChartItems[0]);
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
            ScheduleChartDataGrid.Items.Insert(ScheduleChartDataGrid.SelectedIndex, item);
            ScheduleChartDataGrid.SelectedItem = item;
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
            // ScheduleChartDataGrid.BeginInit();
            foreach (ScheduleChartItem item in items)
                ScheduleChartDataGrid.Items.Remove(item);
            // ScheduleChartDataGrid.EndInit();
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
            ScheduleChartDataGrid.Print("ScheduleChartDataGrid Sample Document");
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
    }
}
