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

namespace Demos.WPF.CSharp.ScheduleChartView.MainFeatures
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GanttChartItem task1 = ScheduleChartView.Items[0].GanttChartItems[0];
            task1.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            task1.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            task1.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));

            GanttChartItem task21 = ScheduleChartView.Items[0].GanttChartItems[1];
            task21.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            task21.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            task21.AssignmentsContent = "50%";

            GanttChartItem task22 = ScheduleChartView.Items[1].GanttChartItems[0];
            task22.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            task22.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));

            // You may uncomment the next lines of code to test the component performance:
            // for (int i = 3; i <= 1024; i++)
            // {
            //    ScheduleChartItem item = new ScheduleChartItem();
            //    for (int j = 1; j <= (i - 1) % 4 + 1; j++)
            //    {
            //        item.GanttChartItems.Add(
            //            new GanttChartItem
            //            {
            //                Content = "Task " + i + "." + j + " (Resource " + i + ")",
            //                Start = DateTime.Today.AddDays(i + (i - 1) * (j - 1)),
            //                Finish = DateTime.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1)
            //            });
            //    }
            //    ScheduleChartView.Items.Add(item);
            // }

            // Initialize the control area.
            ScalesComboBox.SelectedIndex = 0;
            ShowWeekendsCheckBox.IsChecked = true;
        }

        public MainWindow(string theme) : this()
        {
            if (theme == null || theme == "Default" || theme == "Aero")
                return;
            themeResourceDictionary = new ResourceDictionary { Source = new Uri("/" + GetType().Assembly.GetName().Name + ";component/Themes/" + theme + ".xaml", UriKind.Relative) };
            ScheduleChartView.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }

        private ResourceDictionary themeResourceDictionary;

        // Control area commands.
        private void SetColorButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartItem item1 = ScheduleChartView.Items[0].GanttChartItems[1];
            GanttChartView.SetStandardBarFill(item1, Resources["CustomStandardBarFill"] as Brush);
            GanttChartView.SetStandardBarStroke(item1, Resources["CustomStandardBarStroke"] as Brush);
            GanttChartItem item2 = ScheduleChartView.Items[1].GanttChartItems[0];
            GanttChartView.SetStandardBarFill(item2, Resources["CustomStandardBarFill"] as Brush);
            GanttChartView.SetStandardBarStroke(item2, Resources["CustomStandardBarStroke"] as Brush);
        }
        private void ScaleTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = ScalesComboBox.SelectedItem as ComboBoxItem;
            string scalesResourceKey = selectedComboBoxItem.Tag as string;
            ScaleCollection scales = Resources[scalesResourceKey] as ScaleCollection;
            ScheduleChartView.Scales = scales;
        }
        private void ZoomCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            originalZoom = ScheduleChartView.HourWidth;
            ScheduleChartView.HourWidth = originalZoom * 2;
        }
        private void ZoomCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ScheduleChartView.HourWidth = originalZoom;
        }
        private double originalZoom;
        private void IncreaseTimelinePageButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleChartView.TimelinePageFinish += pageUpdateAmount;
            ScheduleChartView.TimelinePageStart += pageUpdateAmount;
        }
        private void DecreaseTimelinePageButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleChartView.TimelinePageFinish -= pageUpdateAmount;
            ScheduleChartView.TimelinePageStart -= pageUpdateAmount;
        }
        private readonly TimeSpan pageUpdateAmount = TimeSpan.FromDays(7);
        private void ShowWeekendsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ScheduleChartView.VisibleWeekStart = DayOfWeek.Sunday;
            ScheduleChartView.VisibleWeekFinish = DayOfWeek.Saturday;
            WorkOnWeekendsCheckBox.IsEnabled = true;
        }
        private void ShowWeekendsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            WorkOnWeekendsCheckBox.IsChecked = false;
            WorkOnWeekendsCheckBox.IsEnabled = false;
            ScheduleChartView.VisibleWeekStart = DayOfWeek.Monday;
            ScheduleChartView.VisibleWeekFinish = DayOfWeek.Friday;
        }
        private void WorkOnWeekendsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ScheduleChartView.WorkingWeekStart = DayOfWeek.Sunday;
            ScheduleChartView.WorkingWeekFinish = DayOfWeek.Saturday;
        }
        private void WorkOnWeekendsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ScheduleChartView.WorkingWeekStart = DayOfWeek.Monday;
            ScheduleChartView.WorkingWeekFinish = DayOfWeek.Friday;
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleChartView.Print("ScheduleChartView Sample Document");
        }
        private void ExportImageButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleChartView.Export((Action)delegate
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog { Title = "Export Image To", Filter = "PNG image files|*.png" };
                if (saveFileDialog.ShowDialog() != true)
                    return;
                BitmapSource bitmapSource = ScheduleChartView.GetExportBitmapSource(96 * 2);
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
