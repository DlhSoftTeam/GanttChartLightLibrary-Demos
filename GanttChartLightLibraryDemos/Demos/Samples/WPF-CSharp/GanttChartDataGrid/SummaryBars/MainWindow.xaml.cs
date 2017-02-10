using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using DlhSoft.Windows.Controls;
using System.Collections.ObjectModel;

namespace Demos.WPF.CSharp.GanttChartDataGrid.SummaryBars
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

            // Turn off asynchronous presentation and apply template to ensure task hierarchy initialization and to be able to access the internal GanttChartView control.
            GanttChartDataGrid.IsAsyncPresentationEnabled = false;

            // Component ApplyTemplate is called in order to complete loading of the user interface, after the main ApplyTemplate that initializes the custom theme, and using an asynchronous action to allow further constructor initializations if they exist (such as setting up the theme name to load).
            Dispatcher.BeginInvoke((Action)delegate
            {
                ApplyTemplate();
                GanttChartDataGrid.ApplyTemplate();

                // Set up the internally managed leaf item clones to be displayed in the chart area (instead of GanttChartDataGrid.Items).
                foreach (GanttChartItem item in GanttChartDataGrid.Items)
                {
                    if (item.HasChildren)
                    {
                        // Store children of each summary task for reference purposes.
                        item.Tag = GanttChartDataGrid.GetAllChildren(item);
                        continue;
                    }
                    GanttChartItem clone = new GanttChartItem();
                    BindingOperations.SetBinding(clone, GanttChartItem.ContentProperty, new Binding("Content") { Source = item });
                    BindingOperations.SetBinding(clone, GanttChartItem.StartProperty, new Binding("Start") { Source = item });
                    BindingOperations.SetBinding(clone, GanttChartItem.FinishProperty, new Binding("Finish") { Source = item });
                    BindingOperations.SetBinding(clone, GanttChartItem.CompletedFinishProperty, new Binding("CompletedFinish") { Source = item });
                    BindingOperations.SetBinding(clone, GanttChartItem.IsMilestoneProperty, new Binding("IsMilestone") { Source = item });
                    BindingOperations.SetBinding(clone, GanttChartItem.AssignmentsContentProperty, new Binding("AssignmentsContent") { Source = item });
                    BindingOperations.SetBinding(clone, GanttChartItem.DisplayRowIndexProperty, new Binding("ActualDisplayRowIndex") { Source = item });
                    BindingOperations.SetBinding(clone, GanttChartItem.IsVisibleProperty, new Binding("IsVisible") { Source = item });
                    BindingOperations.SetBinding(item, GanttChartItem.StartProperty, new Binding("Start") { Source = clone });
                    BindingOperations.SetBinding(item, GanttChartItem.FinishProperty, new Binding("Finish") { Source = clone });
                    BindingOperations.SetBinding(item, GanttChartItem.CompletedFinishProperty, new Binding("CompletedFinish") { Source = clone });
                    BindingOperations.SetBinding(item, GanttChartItem.IsMilestoneProperty, new Binding("IsMilestone") { Source = clone });
                    // Store clones as item tags for reference purposes.
                    item.Tag = clone;
                    // Store parents of each cloned item for reference purposes.
                    clone.Tag = GanttChartDataGrid.GetAllParents(item);
                    ganttChartItemClones.Add(clone);
                }
                GanttChartDataGrid.GanttChartView.Items = ganttChartItemClones;

                // Initialize expansion notification handler on summary items in the DataGrid.
                foreach (GanttChartItem item in GanttChartDataGrid.Items)
                {
                    if (item.HasChildren)
                        item.ExpansionChanged += Item_ExpansionChanged;
                }
            });
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

        // Stores the clones of leaf Gantt Chart items.
        private ObservableCollection<GanttChartItem> ganttChartItemClones = new ObservableCollection<GanttChartItem>();

        private void Item_ExpansionChanged(object sender, EventArgs e)
        {
            GanttChartItem summaryItem = sender as GanttChartItem;
            IEnumerable<GanttChartItem> childItems = summaryItem.Tag as IEnumerable<GanttChartItem>;
            if (!summaryItem.IsExpanded)
            {
                // When a summary item gets collapsed, show child item clones in the chart area in the summary row.
                foreach (GanttChartItem childItem in childItems)
                {
                    GanttChartItem clone = childItem.Tag as GanttChartItem;
                    if (clone == null)
                        continue;
                    BindingOperations.SetBinding(clone, GanttChartItem.DisplayRowIndexProperty, new Binding("ActualDisplayRowIndex") { Source = summaryItem });
                    BindingOperations.SetBinding(clone, GanttChartItem.IsVisibleProperty, new Binding("IsVisible") { Source = summaryItem });
                    clone.AssignmentsContent = null;
                }
            }
            else
            {
                // When a summary item gets expanded, show child item clones in the chart area in their own row (or in the row of their first visible parent).
                foreach (GanttChartItem childItem in childItems)
                {
                    GanttChartItem clone = childItem.Tag as GanttChartItem;
                    if (clone == null)
                        continue;
                    if (childItem.IsVisible)
                    {
                        BindingOperations.SetBinding(clone, GanttChartItem.DisplayRowIndexProperty, new Binding("ActualDisplayRowIndex") { Source = childItem });
                        BindingOperations.SetBinding(clone, GanttChartItem.IsVisibleProperty, new Binding("IsVisible") { Source = childItem });
                        BindingOperations.SetBinding(clone, GanttChartItem.AssignmentsContentProperty, new Binding("AssignmentsContent") { Source = childItem });
                    }
                    else
                    {
                        IEnumerable<GanttChartItem> parentItems = clone.Tag as IEnumerable<GanttChartItem>;
                        foreach (GanttChartItem parentItem in parentItems)
                        {
                            if (parentItem.IsVisible)
                            {
                                BindingOperations.SetBinding(clone, GanttChartItem.DisplayRowIndexProperty, new Binding("ActualDisplayRowIndex") { Source = parentItem });
                                BindingOperations.SetBinding(clone, GanttChartItem.IsVisibleProperty, new Binding("IsVisible") { Source = parentItem });
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
