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
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace GanttChartDataGridDatabaseSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Initialize data.
            LoadData();
        }

        // Hosts the data context used for data binding and change recording.
        private DatabaseEntities context;

        private void LoadMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Clear and reinitialize data.
            ClearData();
            LoadData();
        }
        private void ClearData()
        {
            // Clear the data context.
            GanttChartDataGrid.ClearValue(GanttChartDataGrid.ItemsProperty);
            GanttChartDataGrid.DataContext = null;

            if (context != null)
            {
                // Clear the existing database context.
                context.Dispose();
                context = null;
            }
        }
        private void LoadData()
        {
            // Create a new database context.
            context = new DatabaseEntities();

            // Set up the GanttChartDataGrid.DataContext for reference purposes.
            GanttChartDataGrid.DataContext = context;

            // Bind GanttChartDataGrid.Items to Tasks using TaskItemsConverter instance (see TaskItemsConverter source code).
            GanttChartDataGrid.SetBinding(GanttChartDataGrid.ItemsProperty, new Binding("Tasks") { Converter = TaskItemsConverter.GetInstance(context) });

            // Apply template to ensure we are able to compute overall project values and use scrolling features.
            GanttChartDataGrid.ApplyTemplate();

            // Scroll the view to properly display the project.
            DateTime overallStart = GanttChartDataGrid.GetProjectStart();
            if (overallStart >= DateTime.Today)
                overallStart = DateTime.Today;
            GanttChartDataGrid.SetTimelinePage(overallStart.AddDays(-1), overallStart.AddDays(60 + 2));
            GanttChartDataGrid.ScrollTo(DateTime.Today);
        }
        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Save the data changes to the database.
            SaveData();
        }
        private void SaveData()
        {
            // Save changes on the database context.
            context.SaveChanges();
        }

        // Edit commands.
        private void AddNewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            GanttChartItem item = new GanttChartItem { Content = "New Task", Start = DateTime.Today, Finish = DateTime.Today.AddDays(1) };
            GanttChartDataGrid.Items.Add(item);
            GanttChartDataGrid.SelectedItem = item;
        }
        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            List<GanttChartItem> items = new List<GanttChartItem>();
            foreach (GanttChartItem item in GanttChartDataGrid.SelectedItems)
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
        private void IncreaseIndentationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            List<GanttChartItem> items = new List<GanttChartItem>();
            foreach (GanttChartItem item in GanttChartDataGrid.SelectedItems)
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
                    {
                        ClearDependencies(item);
                        item.Indentation++;
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Cannot increase indentation for {0} because the previous item is its parent item; increase the indentation of its parent item first.", item), "Information", MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox.Show(string.Format("Cannot increase indentation for {0} because it is the first item; insert an item before this one first.", item), "Information", MessageBoxButton.OK);
                }
            }
        }
        private void DecreaseIndentationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            List<GanttChartItem> items = new List<GanttChartItem>();
            foreach (GanttChartItem item in GanttChartDataGrid.SelectedItems)
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
                    {
                        ClearDependencies(item);
                        item.Indentation--;
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Cannot increase indentation for {0} because the next item is one of its child items; decrease the indentation of its child items first.", item), "Information", MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox.Show(string.Format("Cannot decrease indentation for {0} because it is on the first indentation level, being a root item.", item), "Information", MessageBoxButton.OK);
                }
            }
        }
        private void ClearDependencies(GanttChartItem item)
        {
            bool hadDependencies = false;
            if (item.Predecessors.Any())
                hadDependencies = true;
            foreach (PredecessorItem predecessorItem in item.Predecessors.ToArray().Reverse())
                item.Predecessors.Remove(predecessorItem);
            List<PredecessorItem> successorPredecessorItems = new List<PredecessorItem>(GanttChartDataGrid.GetSuccessorPredecessorItems(item));
            foreach (PredecessorItem successorPredecessorItem in successorPredecessorItems)
            {
                hadDependencies = true;
                successorPredecessorItem.DependentItem.Predecessors.Remove(successorPredecessorItem);
            }
            if (hadDependencies)
                MessageBox.Show(string.Format("Dependencies are removed from {0}, in order to preserve hierarchical data consistency.", item), "Information", MessageBoxButton.OK);
        }
    }
}
