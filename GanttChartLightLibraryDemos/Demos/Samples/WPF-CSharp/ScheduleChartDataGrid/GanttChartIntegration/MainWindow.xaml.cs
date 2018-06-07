using System;
using System.Windows;
using DlhSoft.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Demos.WPF.CSharp.ScheduleChartDataGrid.GanttChartIntegration
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

            GanttChartItem item8 = GanttChartDataGrid.Items[8];
            item8.Start = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));
            item8.Finish = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("14:00:00"));
            item8.AssignmentsContent = "Resource 1 [50%], Resource 2 [75%]";

            GanttChartItem item9 = GanttChartDataGrid.Items[9];
            item9.Start = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("08:00:00"));
            item9.Finish = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"));
            item9.AssignmentsContent = "Resource 1";

            GanttChartItem item10 = GanttChartDataGrid.Items[10];
            item10.Start = DateTime.Today.AddDays(7).Add(TimeSpan.Parse("08:00:00"));
            item10.Finish = DateTime.Today.AddDays(28).Add(TimeSpan.Parse("16:00:00"));
            item10.Predecessors.Add(new PredecessorItem { Item = item9 });

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

        private ResourceDictionary themeResourceDictionary;
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
            themeResourceDictionary = new ResourceDictionary { Source = new Uri("/" + GetType().Assembly.GetName().Name + ";component/Themes/" + theme + ".xaml", UriKind.Relative) };
            GanttChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }

        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            GanttChartItem item = new GanttChartItem { Content = "New Task", Start = DateTime.Today, Finish = DateTime.Today.AddDays(1) };
            GanttChartDataGrid.Items.Add(item);
            GanttChartDataGrid.SelectedItem = item;
            GanttChartDataGrid.ScrollTo(item);
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
            GanttChartDataGrid.Items.Insert(GanttChartDataGrid.SelectedIndex, item);
            GanttChartDataGrid.SelectedItem = item;
            GanttChartDataGrid.ScrollTo(item);
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
            GanttChartDataGrid.BeginInit();
            foreach (GanttChartItem item in items)
            {
                if (item.HasChildren)
                {
                    MessageBox.Show(string.Format("Cannot delete {0} because it has child items; remove its child items first.", item), "Information", MessageBoxButton.OK);
                    continue;
                }
                GanttChartDataGrid.Items.Remove(item);
            }
            GanttChartDataGrid.EndInit();
        }

        // Show a Schedule Chart in a dialog window based on Gantt Chart items.
        // Upon closing the dialog, update Gantt Chart items according to the changes done in the Schedule Chart.
        private void ScheduleChartButton_Click(object sender, RoutedEventArgs e)
        {
            double originalOpacity = Opacity;
            Opacity = 0.5;
            GanttChartDataGrid.UnassignedScheduleChartItemContent = "(Unassigned)"; // Optional
            ObservableCollection<ScheduleChartItem> scheduleChartItems = GanttChartDataGrid.GetScheduleChartItems();
            Window scheduleChartWindow =
                new Window
                {
                    Owner = Application.Current.MainWindow, Title = "Schedule Chart", WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Width = 1280, Height = 480, ResizeMode = ResizeMode.CanResize,
                    Content = new DlhSoft.Windows.Controls.ScheduleChartDataGrid
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
    }
}
