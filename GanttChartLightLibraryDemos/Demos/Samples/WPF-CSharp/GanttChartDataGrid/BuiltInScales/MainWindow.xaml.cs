using System;
using System.Windows;
using DlhSoft.Windows.Controls;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;

namespace Demos.WPF.CSharp.GanttChartDataGrid.BuiltInScales
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

        private void Root_Loaded(object sender, RoutedEventArgs e)
        {
            MajorScaleTypeComboBox.SelectedItem = ScaleType.Weeks;
            MinorScaleTypeComboBox.SelectedItem = ScaleType.Days;

            MajorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.ShortDate;
            MinorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.DayOfWeekInitial;

            ZoomLevelTextBox.Text = "5";

            UpdateScaleComboBox.SelectedIndex = 1; // 15min
        }

        private void MajorScaleTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetMajorScale();
        }
        private void SetMajorScale()
        {
            if (GanttChartDataGrid == null)
                return;
            ScaleType scaleType = (ScaleType)MajorScaleTypeComboBox.SelectedItem;
            GanttChartDataGrid.GetScale(0).ScaleType = GetActualScaleType(scaleType);
            UpdateFromSelectedMajorScaleType(scaleType);
        }
        private void UpdateFromSelectedMajorScaleType(ScaleType scaleType)
        {
            switch (scaleType)
            {
                case ScaleType.Years:
                    MajorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Year;
                    MinorScaleTypeComboBox.SelectedItem = ScaleType.Months;
                    break;
                case ScaleType.Quarters:
                    MajorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.YearMonth;
                    MinorScaleTypeComboBox.SelectedItem = ScaleType.Months;
                    break;
                case ScaleType.Months:
                    MajorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Month;
                    MinorScaleTypeComboBox.SelectedItem = ScaleType.Weeks;
                    break;
                case ScaleType.Weeks:
                    MajorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.ShortDate;
                    MinorScaleTypeComboBox.SelectedItem = ScaleType.Days;
                    break;
                case ScaleType.Days:
                    MajorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Day;
                    MinorScaleTypeComboBox.SelectedIndex = 5;
                    break;
                case ScaleType.Hours:
                    MajorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Hour;
                    MinorScaleTypeComboBox.SelectedItem = ScaleType.Days;
                    break;
            }
        }

        private void MinorScaleTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetMinorScale();
        }
        private void SetMinorScale()
        {
            if (GanttChartDataGrid == null || MinorScaleTypeComboBox.SelectedItem == null)
                return;
            var scaleType = (ScaleType)MinorScaleTypeComboBox.SelectedItem;
            GanttChartDataGrid.GetScale(1).ScaleType = GetActualScaleType(scaleType);
            UpdateFromSelectedMinorScaleType(scaleType);
        }
        private void UpdateFromSelectedMinorScaleType(ScaleType scaleType)
        {
            switch (scaleType)
            {
                case ScaleType.Years:
                    MinorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Year;
                    ZoomLevelTextBox.Text = "0.5";
                    break;
                case ScaleType.Months:
                    MinorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Month;
                    ZoomLevelTextBox.Text = "1";
                    break;
                case ScaleType.Weeks:
                    MinorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Day;
                    ZoomLevelTextBox.Text = "2";
                    break;
                case ScaleType.Days:
                    MinorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.DayOfWeekInitial;
                    ZoomLevelTextBox.Text = "5";
                    break;
                case ScaleType.Hours:
                    MinorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Hour;
                    ZoomLevelTextBox.Text = "25";
                    break;
            }
        }

        private ScaleType GetActualScaleType(ScaleType value)
        {
            return value != ScaleType.Weeks || MondayBasedCheckBox.IsChecked != true ? value : ScaleType.WeeksStartingMonday;
        }

        private void MajorScaleHeaderFormatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GanttChartDataGrid == null)
                return;
            TimeScaleTextFormat headerFormat = (TimeScaleTextFormat)MajorScaleHeaderFormatComboBox.SelectedItem;
            GanttChartDataGrid.GetScale(0).HeaderContentFormat = headerFormat;
        }
        private void MinorScaleHeaderFormatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GanttChartDataGrid == null)
                return;
            TimeScaleTextFormat headerFormat = (TimeScaleTextFormat)MinorScaleHeaderFormatComboBox.SelectedItem;
            GanttChartDataGrid.GetScale(1).HeaderContentFormat = headerFormat;
        }

        private void MajorScaleSeparatorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (GanttChartDataGrid == null)
                return;
            if (MajorScaleSeparatorCheckBox.IsChecked == true)
                GanttChartDataGrid.GetScale(0).BorderThickness = new Thickness(0, 0, 1, 0);
            else
                GanttChartDataGrid.GetScale(0).BorderThickness = new Thickness(0);
        }
        private void MinorScaleSeparatorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (GanttChartDataGrid == null)
                return;
            if (MinorScaleSeparatorCheckBox.IsChecked == true)
            {
                GanttChartDataGrid.GetScale(1).BorderThickness = new Thickness(0, 0, 1, 0);
                GanttChartDataGrid.GetScale(1).BorderBrush = GanttChartDataGrid.GetScale(0).BorderBrush;
            }
            else
            {
                GanttChartDataGrid.GetScale(1).BorderThickness = new Thickness(0);
            }
        }

        private void MondayBasedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SetMajorScale();
            SetMinorScale();
        }

        private void NonworkingDaysCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (GanttChartDataGrid == null)
                return;
            GanttChartDataGrid.IsNonworkingTimeHighlighted = NonworkingDaysCheckBox.IsChecked == true;
        }
        private void CurrentTimeVisibleCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (GanttChartDataGrid == null)
                return;
            GanttChartDataGrid.IsCurrentTimeLineVisible = CurrentTimeVisibleCheckBox.IsChecked == true;
        }

        private void ZoomLevelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (GanttChartDataGrid == null)
                return;
            double hourWidth;
            if (double.TryParse(ZoomLevelTextBox.Text.Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out hourWidth))
            {
                if (hourWidth > 0)
                    GanttChartDataGrid.HourWidth = hourWidth;
            }
        }

        private void UpdateScaleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GanttChartDataGrid == null)
                return;
            var selectedItem = UpdateScaleComboBox.SelectedItem as ComboBoxItem;
            switch (selectedItem.Content as string)
            {
                case "Free":
                    GanttChartDataGrid.UpdateScaleInterval = TimeSpan.Zero;
                    break;
                case "15 min":
                    GanttChartDataGrid.UpdateScaleInterval = TimeSpan.Parse("00:15:00");
                    break;
                case "Hour":
                    GanttChartDataGrid.UpdateScaleInterval = TimeSpan.Parse("01:00:00");
                    break;
                case "Day":
                    GanttChartDataGrid.UpdateScaleInterval = TimeSpan.Parse("1.00:00:00");
                    break;
            }
        }
    }
}
