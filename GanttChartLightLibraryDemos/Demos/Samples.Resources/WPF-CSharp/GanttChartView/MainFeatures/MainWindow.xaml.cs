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

namespace Demos.WPF.CSharp.GanttChartView.MainFeatures
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GanttChartItem item0 = GanttChartView.Items[0];

            GanttChartItem item1 = GanttChartView.Items[1];
            item1.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item1.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            item1.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));
            item1.AssignmentsContent = "Resource 1";

            GanttChartItem item2 = GanttChartView.Items[2];
            item2.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item2.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item2.AssignmentsContent = "Resource 1, Resource 2";
            item2.Predecessors.Add(new PredecessorItem { Item = item1 });

            GanttChartItem item3 = GanttChartView.Items[3];
            item3.Predecessors.Add(new PredecessorItem { Item = item0, DependencyType = DependencyType.StartStart });

            GanttChartItem item4 = GanttChartView.Items[4];
            item4.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item4.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"));

            GanttChartItem item6 = GanttChartView.Items[6];
            item6.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item6.Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));

            GanttChartItem item7 = GanttChartView.Items[7];
            item7.Start = DateTime.Today.AddDays(4);
            item7.IsMilestone = true;
            item7.Predecessors.Add(new PredecessorItem { Item = item4 });
            item7.Predecessors.Add(new PredecessorItem { Item = item6 });

            for (int i = 3; i <= 23; i++)
            {
                GanttChartView.Items.Add(
                    new GanttChartItem
                    {
                        Content = "Task " + i,
                        Indentation = i % 3 == 0 ? 0 : 1,
                        Start = DateTime.Today.AddDays(i <= 8 ? (i - 4) * 3 : i - 8),
                        Finish = DateTime.Today.AddDays((i <= 8 ? (i - 4) * 3 + (i > 8 ? 6 : 1) : i - 2) + 1),
                        CompletedFinish = DateTime.Today.AddDays(i <= 8 ? (i - 4) * 3 : i - 8).AddDays(i % 6 == 4 ? 3 : 0)
                    });
            }

            // You may uncomment the next lines of code to test the component performance:
            // for (int i = 24; i <= 4096; i++)
            // {
            //    GanttChartView.Items.Add(
            //        new GanttChartItem
            //        {
            //            Content = "Task " + i,
            //            Indentation = i % 5 == 0 ? 0 : 1,
            //            IsExpanded = i % 10 == 0 ? false : true,
            //            Start = DateTime.Today.AddDays(i),
            //            Finish = DateTime.Today.AddDays(i * 1.2 + 1)
            //        });
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
            GanttChartView.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }

        // Control area commands.
        private void SetColorButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartItem item = GanttChartView.Items[4];
            DlhSoft.Windows.Controls.GanttChartView.SetStandardBarFill(item, Resources["CustomStandardBarFill"] as Brush);
            DlhSoft.Windows.Controls.GanttChartView.SetStandardBarStroke(item, Resources["CustomStandardBarStroke"] as Brush);
        }
        private void ScaleTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = ScalesComboBox.SelectedItem as ComboBoxItem;
            string scalesResourceKey = selectedComboBoxItem.Tag as string;
            ScaleCollection scales = Resources[scalesResourceKey] as ScaleCollection;
            GanttChartView.Scales = scales;
        }
        private void ZoomCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            originalZoom = GanttChartView.HourWidth;
            GanttChartView.HourWidth = originalZoom * 2;
        }
        private void ZoomCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            GanttChartView.HourWidth = originalZoom;
        }
        private double originalZoom;
        private void IncreaseTimelinePageButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartView.TimelinePageFinish += pageUpdateAmount;
            GanttChartView.TimelinePageStart += pageUpdateAmount;
        }
        private void DecreaseTimelinePageButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartView.TimelinePageFinish -= pageUpdateAmount;
            GanttChartView.TimelinePageStart -= pageUpdateAmount;
        }
        private readonly TimeSpan pageUpdateAmount = TimeSpan.FromDays(7);
        private void ShowWeekendsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartView.VisibleWeekStart = DayOfWeek.Sunday;
            GanttChartView.VisibleWeekFinish = DayOfWeek.Saturday;
            WorkOnWeekendsCheckBox.IsEnabled = true;
        }
        private void ShowWeekendsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            WorkOnWeekendsCheckBox.IsChecked = false;
            WorkOnWeekendsCheckBox.IsEnabled = false;
            GanttChartView.VisibleWeekStart = DayOfWeek.Monday;
            GanttChartView.VisibleWeekFinish = DayOfWeek.Friday;
        }
        private void WorkOnWeekendsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GanttChartView.WorkingWeekStart = DayOfWeek.Sunday;
            GanttChartView.WorkingWeekFinish = DayOfWeek.Saturday;
        }
        private void WorkOnWeekendsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            GanttChartView.WorkingWeekStart = DayOfWeek.Monday;
            GanttChartView.WorkingWeekFinish = DayOfWeek.Friday;
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartView.Print("GanttChartView Sample Document");
        }
        private void ExportImageButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartView.Export((Action)delegate
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog { Title = "Export Image To", Filter = "PNG image files|*.png" };
                if (saveFileDialog.ShowDialog() != true)
                    return;
                BitmapSource bitmapSource = GanttChartView.GetExportBitmapSource(96 * 2);
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
