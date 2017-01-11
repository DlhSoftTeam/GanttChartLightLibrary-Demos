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

namespace Demos.WPF.CSharp.LoadChartView.MainFeatures
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AllocationItem allocation11 = LoadChartView.Items[0].GanttChartItems[0];
            allocation11.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            allocation11.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));

            AllocationItem allocation112 = LoadChartView.Items[0].GanttChartItems[1];
            allocation112.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            allocation112.Finish = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            allocation112.Units = 1.5;

            AllocationItem allocation12 = LoadChartView.Items[0].GanttChartItems[2];
            allocation12.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            allocation12.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            allocation12.Units = 0.5;

            AllocationItem allocation13 = LoadChartView.Items[0].GanttChartItems[3];
            allocation13.Start = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("08:00:00"));
            allocation13.Finish = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("16:00:00"));

            AllocationItem allocation22 = LoadChartView.Items[1].GanttChartItems[0];
            allocation22.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            allocation22.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));

            // You may uncomment the next lines of code to test the component performance:
            // for (int i = 3; i <= 1024; i++)
            // {
            //    LoadChartItem item = new LoadChartItem();
            //    for (int j = 1; j <= (i - 1) % 4 + 1; j++)
            //    {
            //        item.GanttChartItems.Add(
            //            new AllocationItem
            //            {
            //                Content = "Task " + i + "." + j + (((i + j) % 2 == 1 ? " [200%]" : string.Empty) + " (Resource " + i + ")"),
            //                Start = DateTime.Today.AddDays(i + (i - 1) * (j - 1)),
            //                Finish = DateTime.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1),
            //                Units = 1 + (i + j) % 2
            //            });
            //    }
            //    LoadChartView.Items.Add(item);
            // }

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
            LoadChartView.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }

        // Control area commands.
        private void ScaleTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = ScalesComboBox.SelectedItem as ComboBoxItem;
            string scalesResourceKey = selectedComboBoxItem.Tag as string;
            ScaleCollection scales = Resources[scalesResourceKey] as ScaleCollection;
            LoadChartView.Scales = scales;
        }
        private void ZoomCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            originalZoom = LoadChartView.HourWidth;
            LoadChartView.HourWidth = originalZoom * 2;
        }
        private void ZoomCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadChartView.HourWidth = originalZoom;
        }
        private double originalZoom;
        private void IncreaseTimelinePageButton_Click(object sender, RoutedEventArgs e)
        {
            LoadChartView.TimelinePageFinish += pageUpdateAmount;
            LoadChartView.TimelinePageStart += pageUpdateAmount;
        }
        private void DecreaseTimelinePageButton_Click(object sender, RoutedEventArgs e)
        {
            LoadChartView.TimelinePageFinish -= pageUpdateAmount;
            LoadChartView.TimelinePageStart -= pageUpdateAmount;
        }
        private readonly TimeSpan pageUpdateAmount = TimeSpan.FromDays(7);
        private void ShowWeekendsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            LoadChartView.VisibleWeekStart = DayOfWeek.Sunday;
            LoadChartView.VisibleWeekFinish = DayOfWeek.Saturday;
        }
        private void ShowWeekendsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadChartView.VisibleWeekStart = DayOfWeek.Monday;
            LoadChartView.VisibleWeekFinish = DayOfWeek.Friday;
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            LoadChartView.Print("LoadChartView Sample Document");
        }
        private void ExportImageButton_Click(object sender, RoutedEventArgs e)
        {
            LoadChartView.Export((Action)delegate
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog { Title = "Export Image To", Filter = "PNG image files|*.png" };
                if (saveFileDialog.ShowDialog() != true)
                    return;
                BitmapSource bitmapSource = LoadChartView.GetExportBitmapSource(96 * 2);
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
