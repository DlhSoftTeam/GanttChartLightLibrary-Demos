using System;
using System.Windows;
using DlhSoft.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Demos.WPF.CSharp.GanttChartDataGrid.UndoRedo
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
            foreach (GanttChartItem item in items)
            {
                if (item.HasChildren)
                {
                    MessageBox.Show(string.Format("Cannot delete {0} because it has child items; remove its child items first.", item), "Information", MessageBoxButton.OK);
                    continue;
                }
                GanttChartDataGrid.Items.Remove(item);
            }
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
    }
}
