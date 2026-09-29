using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Demos.WPF.CSharp.GanttChartDataGrid.MainFeatures
{
    public partial class DateTimePicker : UserControl
    {
        private static readonly string[] hours = Enumerable.Range(0, 24).Select(value => value.ToString("00")).ToArray();
        private static readonly string[] minutes = Enumerable.Range(0, 60).Select(value => value.ToString("00")).ToArray();
        private static readonly WeakReference activePicker = new WeakReference(null);
        private DateTime displayMonth;
        private DateTime menuMonth;
        private DateTime selectedDate;
        private DataGrid editingGrid;
        private DataGridCell editingCell;

        public DateTimePicker()
        {
            InitializeComponent();
            // A standalone popup can open against its visible anchor. DateTimePopup
            // stops mouse routing to that anchor without requiring a hidden parent.
            ((Panel)PickerPopup.Parent).Children.Remove(PickerPopup);
            PickerPopup.PlacementTarget = PickerButton;
            Unloaded += (sender, e) => PickerPopup.IsOpen = false;
            HourSelector.ItemsSource = hours;
            MinuteSelector.ItemsSource = minutes;
        }

        public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register(
            "SelectedDateTime", typeof(DateTime?), typeof(DateTimePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public DateTime? SelectedDateTime
        {
            get { return (DateTime?)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }

        public event EventHandler<DateTimeChangingEventArgs> SelectedDateTimeChanging;

        private Button CalendarButton(string text)
        {
            return new Button { Content = text, Style = (Style)Resources["CalendarButton"], MinHeight = 24, FontSize = 11 };
        }

        private void RenderMonth()
        {
            MonthHeading.Content = displayMonth.ToString("MMMM yyyy", CultureInfo.CurrentCulture);
            PreviousMonthButton.IsEnabled = displayMonth > DateTime.MinValue;
            NextMonthButton.IsEnabled = displayMonth.Year < 9999 || displayMonth.Month < 12;
            WeekdayLabels.Children.Clear();
            DayButtons.Children.Clear();
            var format = CultureInfo.CurrentCulture.DateTimeFormat;
            int firstDay = (int)format.FirstDayOfWeek;
            for (int i = 0; i < 7; i++)
                WeekdayLabels.Children.Add(new TextBlock { Text = format.AbbreviatedDayNames[(firstDay + i) % 7], HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0, 2, 0, 2) });
            int offset = ((int)displayMonth.DayOfWeek - firstDay + 7) % 7;
            for (int i = 0; i < 42; i++)
            {
                long ticks = displayMonth.Ticks + (long)(i - offset) * TimeSpan.TicksPerDay;
                if (ticks < DateTime.MinValue.Ticks || ticks > DateTime.MaxValue.Ticks)
                {
                    DayButtons.Children.Add(new Border());
                    continue;
                }
                var date = new DateTime(ticks);
                var button = CalendarButton(date.Day.ToString(CultureInfo.CurrentCulture));
                button.Tag = date;
                button.Foreground = date.Month == displayMonth.Month ? (Brush)Resources["CalendarTextBrush"] : Brushes.Gray;
                button.Background = date == selectedDate ? (Brush)Resources["CalendarSelectionBrush"] : Brushes.Transparent;
                if (date == DateTime.Today) button.BorderBrush = (Brush)Resources["CalendarTodayBrush"];
                System.Windows.Automation.AutomationProperties.SetName(button, date.ToString("D", CultureInfo.CurrentCulture));
                button.Click += Day_Click;
                DayButtons.Children.Add(button);
            }
        }

        private void Day_Click(object sender, RoutedEventArgs e)
        {
            selectedDate = (DateTime)((Button)sender).Tag;
            displayMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
            RenderMonth();
            e.Handled = true;
        }

        private void RefreshMonthMenu()
        {
            NearbyMonths.Children.Clear();
            int index = (menuMonth.Year - 1) * 12 + menuMonth.Month - 1;
            for (int i = Math.Max(0, index - 3); i <= Math.Min(119987, index + 3); i++)
            {
                var month = new DateTime(i / 12 + 1, i % 12 + 1, 1);
                var button = CalendarButton(month.ToString("MMMM yyyy", CultureInfo.CurrentCulture));
                button.Tag = month;
                button.FontWeight = FontWeights.SemiBold;
                if (month == displayMonth) button.Background = (Brush)Resources["CalendarSelectionBrush"];
                button.Click += Month_Click;
                NearbyMonths.Children.Add(button);
            }
        }

        private void Month_Click(object sender, RoutedEventArgs e)
        {
            displayMonth = (DateTime)((Button)sender).Tag;
            ShowMonthMenu(false);
            RenderMonth();
            e.Handled = true;
        }

        private void ShowMonthMenu(bool show)
        {
            MonthMenu.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            DaysPanel.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
        }

        private void MonthHeading_Click(object sender, RoutedEventArgs e)
        {
            bool show = MonthMenu.Visibility != Visibility.Visible;
            menuMonth = displayMonth;
            RefreshMonthMenu();
            ShowMonthMenu(show);
            e.Handled = true;
        }

        private void PreviousYear_Click(object sender, RoutedEventArgs e)
        {
            if (menuMonth.Year > 1) menuMonth = menuMonth.AddYears(-1);
            RefreshMonthMenu();
            e.Handled = true;
        }

        private void NextYear_Click(object sender, RoutedEventArgs e)
        {
            if (menuMonth.Year < 9999) menuMonth = menuMonth.AddYears(1);
            RefreshMonthMenu();
            e.Handled = true;
        }

        private void MoveMonth(int delta)
        {
            if ((delta < 0 && displayMonth == DateTime.MinValue) || (delta > 0 && displayMonth.Year == 9999 && displayMonth.Month == 12)) return;
            displayMonth = displayMonth.AddMonths(delta);
            RenderMonth();
            if (MonthMenu.Visibility == Visibility.Visible) { menuMonth = displayMonth; RefreshMonthMenu(); }
        }

        private void PreviousMonth_Click(object sender, RoutedEventArgs e) { MoveMonth(-1); e.Handled = true; }
        private void NextMonth_Click(object sender, RoutedEventArgs e) { MoveMonth(1); e.Handled = true; }

        private void PickerButton_Click(object sender, RoutedEventArgs e)
        {
            PickerPopup.IsOpen = true;
            e.Handled = true;
        }

        private static T FindAncestor<T>(DependencyObject element) where T : DependencyObject
        {
            for (var parent = VisualTreeHelper.GetParent(element); parent != null; parent = VisualTreeHelper.GetParent(parent))
                if (parent is T) return (T)parent;
            return null;
        }

        private void KeepCellEditing(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (PickerPopup.IsOpen && editingCell != null && e.Column == editingCell.Column && e.Row == FindAncestor<DataGridRow>(editingCell)) e.Cancel = true;
        }

        private void PickerPopup_Closed(object sender, EventArgs e)
        {
            if (ReferenceEquals(activePicker.Target, this)) activePicker.Target = null;
            ShowMonthMenu(false);
            if (editingGrid != null) editingGrid.CellEditEnding -= KeepCellEditing;
            editingGrid = null;
            editingCell = null;
        }

        private void PickerPopup_Opened(object sender, EventArgs e)
        {
            var previous = activePicker.Target as DateTimePicker;
            if (previous != null && previous != this) previous.PickerPopup.IsOpen = false;
            activePicker.Target = this;
            editingCell = FindAncestor<DataGridCell>(this);
            editingGrid = FindAncestor<DataGrid>(this);
            if (editingGrid != null) editingGrid.CellEditEnding += KeepCellEditing;
            var value = SelectedDateTime ?? DateTime.Today;
            selectedDate = value.Date;
            displayMonth = new DateTime(value.Year, value.Month, 1);
            ShowMonthMenu(false);
            HourSelector.SelectedIndex = value.Hour;
            MinuteSelector.SelectedIndex = value.Minute;
            RenderMonth();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (HourSelector.SelectedIndex < 0 || MinuteSelector.SelectedIndex < 0) return;
            var value = selectedDate.AddHours(HourSelector.SelectedIndex).AddMinutes(MinuteSelector.SelectedIndex);
            var changing = new DateTimeChangingEventArgs(value);
            SelectedDateTimeChanging?.Invoke(this, changing);
            if (changing.Cancel) return;
            var grid = editingGrid;
            // Release the edit guard before writing and committing the actual grid cell.
            PickerPopup.IsOpen = false;
            SetCurrentValue(SelectedDateTimeProperty, value);
            GetBindingExpression(SelectedDateTimeProperty)?.UpdateSource();
            if (grid != null)
            {
                grid.CommitEdit(DataGridEditingUnit.Cell, true);
                grid.CommitEdit(DataGridEditingUnit.Row, true);
            }
            e.Handled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) { PickerPopup.IsOpen = false; e.Handled = true; }
        private void PickerPopup_PreviewKeyDown(object sender, KeyEventArgs e) { if (e.Key == Key.Escape) e.Handled = true; }
    }

    public class DateTimePopup : System.Windows.Controls.Primitives.Popup
    {
        protected override DependencyObject GetUIParentCore()
        {
            // Popup normally routes input back to PlacementTarget when it has no
            // logical parent; the Gantt grid must not intercept calendar input.
            return null;
        }
    }

    public class DateTimeChangingEventArgs : CancelEventArgs
    {
        public DateTimeChangingEventArgs(DateTime selectedDateTime) { SelectedDateTime = selectedDateTime; }
        public DateTime SelectedDateTime { get; private set; }
    }
}
