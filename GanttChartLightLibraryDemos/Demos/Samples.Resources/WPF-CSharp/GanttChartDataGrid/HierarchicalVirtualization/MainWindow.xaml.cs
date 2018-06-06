using System;
using System.Linq;
using System.Windows;
using DlhSoft.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Demos.WPF.CSharp.GanttChartDataGrid.HierarchicalVirtualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int year = DateTime.Now.Year, month = DateTime.Now.Month;

        public MainWindow()
        {
            InitializeComponent();

            // Load root items only, and ensure IsExpanded is set to false on all of them.
            // For each root item that is going to be a summary node, load a dummy child item as well sharing the 
            // (pre-cached) aggregated values of the child items, to ensure root items are displayed as summary items. 
            // We will remove the dummy items once the summary items are expanded (see below).
            GanttChartDataGrid.Items = new ObservableCollection<GanttChartItem>() { new GanttChartItem() { Content = "Task 1", IsExpanded = false },
                                            new GanttChartItem() { Content = "Task 1 hierarchy placeholder", IsHidden = true, Indentation = 1,
                                                                   Start = new DateTime(year, month, 2, 8, 0, 0), Finish = new DateTime(year, month, 5, 12, 0, 0) },
                                            new GanttChartItem() { Content = "Task 2", IsExpanded = false },
                                            new GanttChartItem() { Content = "Task 2 hierarchy placeholder", IsHidden = true, Indentation = 1,
                                                                   Start = new DateTime(year, month, 2, 8, 0, 0), Finish = new DateTime(year, month, 14, 16, 0, 0) },
                                            new GanttChartItem() { Content = "Task 3", Start = new DateTime(year, month, 15, 16, 0, 0), IsMilestone = true }};

            GanttChartDataGrid.TimelinePageStart = new DateTime(year, month, 1);
            GanttChartDataGrid.DisplayedTime = new DateTime(year, month, 1);
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

        // Detect expansion changes and load child items (summary child items will be loaded in collapsed state, 
        // with dummy nodes as well, to make this work recursively).
        private void GanttChartDataGrid_ItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            GanttChartItem item = sender as GanttChartItem;
            if (e.PropertyName == "IsExpanded" && item.IsExpanded)
            {
                int index = GanttChartDataGrid.Items.IndexOf(item) + 1;
                GanttChartItem nextItem = index < GanttChartDataGrid.Items.Count ? GanttChartDataGrid.Items[index] : null;

                // Ensure we do not try loading hierarchy for the last node nor reload hierarchies for previously expanded nodes.
                if (nextItem == null || !nextItem.IsHidden)
                    return;

                // Determine the actual sub-items of the expanded summary item.
                var subItems = new List<GanttChartItem>();
                switch (item.Content as string)
                {
                    case "Task 1":
                        subItems.Add(new GanttChartItem { Content = "Task 1.1", Start = new DateTime(year, month, 2, 8, 0, 0), Finish = new DateTime(year, month, 4, 16, 0, 0) });
                        subItems.Add(new GanttChartItem { Content = "Task 1.2", Start = new DateTime(year, month, 3, 8, 0, 0), Finish = new DateTime(year, month, 5, 12, 0, 0) });
                        break;
                    case "Task 2":
                        subItems.Add(new GanttChartItem { Content = "Task 2.1", Start = new DateTime(year, month, 2, 8, 0, 0), Finish = new DateTime(year, month, 8, 16, 0, 0),
                                                          CompletedFinish = new DateTime(year, month, 5, 16, 0, 0), AssignmentsContent = "Resource 1, Resource 2 [50%]" });
                        subItems.Add(new GanttChartItem { Content = "Task 2.2" });
                        // When needed, add subsquent levels of hierarchy placeholders.
                        subItems.Add(new GanttChartItem { Content = "Task 2.2 hierarchy placeholder", IsHidden = true, Start = new DateTime(year, month, 11, 8, 0, 0),
                                                          Finish = new DateTime(year, month, 14, 16, 0, 0) });
                        break;
                    case "Task 2.2":
                        subItems.Add(new GanttChartItem { Content = "Task 2.2.1", Start = new DateTime(year, month, 11, 8, 0, 0), Finish = new DateTime(year, month, 14, 16, 0, 0),
                                                          CompletedFinish = new DateTime(year, month, 14, 16, 0, 0), AssignmentsContent = "Resource 2" });
                        subItems.Add(new GanttChartItem { Content = "Task 2.2.2", Start = new DateTime(year, month, 12, 12, 0, 0), Finish = new DateTime(year, month, 14, 16, 0, 0),
                                                         AssignmentsContent = "Resource 2" });
                        break;
                }

                // Replace the original hierarchy placeholder with the actual sub-items and prepare subsquent hierarchy levels.
                foreach (var subItem in subItems)
                {
                    subItem.Indentation = nextItem.Indentation;
                    if (subItem.IsHidden)
                        subItem.Indentation++;
                    subItem.IsExpanded = false;
                    GanttChartDataGrid.Items.Insert(index++, subItem);
                }
                GanttChartDataGrid.Items.Remove(nextItem);
            }
        }
    }
}