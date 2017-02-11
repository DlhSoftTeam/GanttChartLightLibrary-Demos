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
using System.Collections.ObjectModel;

namespace Demos.WPF.CSharp.LoadChartDataGrid.MainFeatures
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AllocationItem allocation11 = LoadChartDataGrid.Items[0].GanttChartItems[0];
            allocation11.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            allocation11.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));

            AllocationItem allocation112 = LoadChartDataGrid.Items[0].GanttChartItems[1];
            allocation112.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            allocation112.Finish = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            allocation112.Units = 1.5;

            AllocationItem allocation12 = LoadChartDataGrid.Items[0].GanttChartItems[2];
            allocation12.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            allocation12.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            allocation12.Units = 0.5;

            AllocationItem allocation13 = LoadChartDataGrid.Items[0].GanttChartItems[3];
            allocation13.Start = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("08:00:00"));
            allocation13.Finish = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("16:00:00"));

            AllocationItem allocation22 = LoadChartDataGrid.Items[1].GanttChartItems[0];
            allocation22.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            allocation22.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));

            for (int i = 3; i <= 16; i++)
            {
                LoadChartItem item = new LoadChartItem { Content = "Resource " + i };
                for (int j = 1; j <= (i - 1) % 4 + 1; j++)
                {
                    item.GanttChartItems.Add(
                        new AllocationItem
                        {
                            Content = "Task " + i + "." + j + ((i + j) % 2 == 1 ? " [200%]" : string.Empty),
                            Start = DateTime.Today.AddDays(i + (i - 1) * (j - 1)),
                            Finish = DateTime.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1),
                            Units = 1 + (i + j) % 2
                        });
                }
                LoadChartDataGrid.Items.Add(item);
            }

            // You may uncomment the next lines of code to test the component performance:
            // for (int i = 17; i <= 1024; i++)
            // {
            //    LoadChartItem item = new LoadChartItem { Content = "Resource " + i };
            //    for (int j = 1; j <= (i - 1) % 4 + 1; j++)
            //    {
            //        item.GanttChartItems.Add(
            //            new AllocationItem
            //            {
            //                Content = "Task " + i + "." + j + ((i + j) % 2 == 1 ? " [200%]" : string.Empty),
            //                Start = DateTime.Today.AddDays(i + (i - 1) * (j - 1)),
            //                Finish = DateTime.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1),
            //                Units = 1 + (i + j) % 2
            //            });
            //    }
            //    LoadChartDataGrid.Items.Add(item);
            // }

            // Initialize the control area.
            ScalesComboBox.SelectedIndex = 0;
            ShowWeekendsCheckBox.IsChecked = true;
        }

        private string theme = "Generic-bright";
        public MainWindow(string theme) : this()
        {
            this.theme = "Cyan-green";
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
            LoadChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }

        // Control area commands.
        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            LoadChartItem item = new LoadChartItem { Content = "New Resource" };
            item.GanttChartItems.Add(new AllocationItem { Content = "New Task", Start = DateTime.Today, Finish = DateTime.Today.AddDays(1) });
            LoadChartDataGrid.Items.Add(item);
            LoadChartDataGrid.SelectedItem = item;
            LoadChartDataGrid.ScrollTo(item.GanttChartItems[0]);
        }
        private void InsertNewButton_Click(object sender, RoutedEventArgs e)
        {
            LoadChartItem selectedItem = LoadChartDataGrid.SelectedItem as LoadChartItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Cannot insert a new item before selection as the selection is empty; you can either add a new item to the end of the list instead, or select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            LoadChartItem item = new LoadChartItem { Content = "New Resource" };
            item.GanttChartItems.Add(new AllocationItem { Content = "New Task", Start = DateTime.Today, Finish = DateTime.Today.AddDays(1) });
            LoadChartDataGrid.Items.Insert(LoadChartDataGrid.SelectedIndex, item);
            LoadChartDataGrid.SelectedItem = item;
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            List<LoadChartItem> items = new List<LoadChartItem>();
            foreach (LoadChartItem item in LoadChartDataGrid.GetSelectedItems())
                items.Add(item);
            if (items.Count <= 0)
            {
                MessageBox.Show("Cannot delete the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            items.Reverse();
            // LoadChartDataGrid.BeginInit();
            foreach (LoadChartItem item in items)
                LoadChartDataGrid.Items.Remove(item);
            // LoadChartDataGrid.EndInit();
        }
        private void ScaleTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = ScalesComboBox.SelectedItem as ComboBoxItem;
            string scalesResourceKey = selectedComboBoxItem.Tag as string;
            ScaleCollection scales = Resources[scalesResourceKey] as ScaleCollection;
            LoadChartDataGrid.Scales = scales;
        }
        private void ZoomCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            originalZoom = LoadChartDataGrid.HourWidth;
            LoadChartDataGrid.HourWidth = originalZoom * 2;
        }
        private void ZoomCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadChartDataGrid.HourWidth = originalZoom;
        }
        private double originalZoom;
        private void IncreaseTimelinePageButton_Click(object sender, RoutedEventArgs e)
        {
            LoadChartDataGrid.TimelinePageFinish += pageUpdateAmount;
            LoadChartDataGrid.TimelinePageStart += pageUpdateAmount;
        }
        private void DecreaseTimelinePageButton_Click(object sender, RoutedEventArgs e)
        {
            LoadChartDataGrid.TimelinePageFinish -= pageUpdateAmount;
            LoadChartDataGrid.TimelinePageStart -= pageUpdateAmount;
        }
        private readonly TimeSpan pageUpdateAmount = TimeSpan.FromDays(7);
        private void ShowWeekendsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            LoadChartDataGrid.VisibleWeekStart = DayOfWeek.Sunday;
            LoadChartDataGrid.VisibleWeekFinish = DayOfWeek.Saturday;
        }
        private void ShowWeekendsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadChartDataGrid.VisibleWeekStart = DayOfWeek.Monday;
            LoadChartDataGrid.VisibleWeekFinish = DayOfWeek.Friday;
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            LoadChartDataGrid.Print("LoadChartDataGrid Sample Document");
        }
        private void ExportImageButton_Click(object sender, RoutedEventArgs e)
        {
            LoadChartDataGrid.Export((Action)delegate
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog { Title = "Export Image To", Filter = "PNG image files|*.png" };
                if (saveFileDialog.ShowDialog() != true)
                    return;
                BitmapSource bitmapSource = LoadChartDataGrid.GetExportBitmapSource(96 * 2);
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
