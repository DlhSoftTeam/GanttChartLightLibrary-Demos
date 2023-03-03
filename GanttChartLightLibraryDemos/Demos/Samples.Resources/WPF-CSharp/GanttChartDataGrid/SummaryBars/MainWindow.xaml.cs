using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using DlhSoft.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

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

            GanttChartItem item5 = GanttChartDataGrid.Items[5];

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
                        item.Tag = GanttChartDataGrid.GetAllChildren(item).ToArray();
                        continue;
                    }
                    GanttChartItem clone = new GanttChartItem();
                    BindingOperations.SetBinding(clone, GanttChartItem.ContentProperty, new Binding("Content") { Source = item });
                    BindingOperations.SetBinding(clone, GanttChartItem.StartProperty, new Binding("Start") { Source = item, Mode = BindingMode.OneTime });
                    BindingOperations.SetBinding(clone, GanttChartItem.FinishProperty, new Binding("Finish") { Source = item, Mode = BindingMode.OneTime });
                    BindingOperations.SetBinding(clone, GanttChartItem.CompletedFinishProperty, new Binding("CompletedFinish") { Source = item, Mode = BindingMode.OneTime });
                    BindingOperations.SetBinding(clone, GanttChartItem.IsMilestoneProperty, new Binding("IsMilestone") { Source = item, Mode = BindingMode.OneTime });
                    BindingOperations.SetBinding(clone, GanttChartItem.AssignmentsContentProperty, new Binding("AssignmentsContent") { Source = item });
                    BindingOperations.SetBinding(clone, GanttChartItem.DisplayRowIndexProperty, new Binding("ActualDisplayRowIndex") { Source = item });
                    BindingOperations.SetBinding(clone, GanttChartItem.IsVisibleProperty, new Binding("IsVisible") { Source = item });
                    startPropertyDescriptor.AddValueChanged(clone, CloneStartChanged);
                    startPropertyDescriptor.AddValueChanged(item, ItemStartChanged);
                    if (!item.IsMilestone)
                    {
                        finishPropertyDescriptor.AddValueChanged(clone, CloneFinishChanged);
                        finishPropertyDescriptor.AddValueChanged(item, ItemFinishChanged);
                        completedFinishPropertyDescriptor.AddValueChanged(clone, CloneCompletedFinishChanged);
                        completedFinishPropertyDescriptor.AddValueChanged(item, ItemCompletedFinishChanged);
                    }
                    isMilestonePropertyDescriptor.AddValueChanged(item, ItemIsMilestoneChanged);
                    // Store clones as item tags for reference purposes.
                    item.Tag = clone;
                    // Store parents of each cloned item for reference purposes.
                    clone.Tag = new ItemHierarchyInfo { Item = item, ParentItems = GanttChartDataGrid.GetAllParents(item).ToArray() };
                    ganttChartItemClones.Add(clone);
                }
                GanttChartDataGrid.GanttChartView.Items = ganttChartItemClones;

                // Initialize expansion notification handler on summary items in the DataGrid.
                foreach (GanttChartItem item in GanttChartDataGrid.Items)
                {
                    if (item.HasChildren)
                        item.ExpansionChanged += Item_ExpansionChanged;
                }

                item0.IsExpanded = false;
                item5.IsExpanded = false;
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


        // Miscellaneous helpers.
        private DependencyPropertyDescriptor startPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(GanttChartItem.StartProperty, typeof(GanttChartItem));
        private DependencyPropertyDescriptor finishPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(GanttChartItem.FinishProperty, typeof(GanttChartItem));
        private DependencyPropertyDescriptor completedFinishPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(GanttChartItem.CompletedFinishProperty, typeof(GanttChartItem));
        private DependencyPropertyDescriptor isMilestonePropertyDescriptor = DependencyPropertyDescriptor.FromProperty(GanttChartItem.IsMilestoneProperty, typeof(GanttChartItem));
        private void CloneStartChanged(object sender, EventArgs e)
        {
            var clone = sender as GanttChartItem;
            var hierarchyInfo = clone.Tag as ItemHierarchyInfo;
            var item = hierarchyInfo.Item;
            if (clone.Start != item.Start)
                item.SetCurrentValue(GanttChartItem.StartProperty, clone.Start);
        }
        private void ItemStartChanged(object sender, EventArgs e)
        {
            var item = sender as GanttChartItem;
            var clone = item.Tag as GanttChartItem;
            if (item.Start != clone.Start)
                clone.SetCurrentValue(GanttChartItem.StartProperty, item.Start);
        }
        private void CloneFinishChanged(object sender, EventArgs e)
        {
            var clone = sender as GanttChartItem;
            var hierarchyInfo = clone.Tag as ItemHierarchyInfo;
            var item = hierarchyInfo.Item;
            if (clone.Finish != item.Finish)
                item.SetCurrentValue(GanttChartItem.FinishProperty, clone.Finish);
        }
        private void ItemFinishChanged(object sender, EventArgs e)
        {
            var item = sender as GanttChartItem;
            var clone = item.Tag as GanttChartItem;
            if (item.Finish != clone.Finish)
                clone.SetCurrentValue(GanttChartItem.FinishProperty, item.Finish);
        }
        private void CloneCompletedFinishChanged(object sender, EventArgs e)
        {
            var clone = sender as GanttChartItem;
            var hierarchyInfo = clone.Tag as ItemHierarchyInfo;
            var item = hierarchyInfo.Item;
            if (clone.Finish != item.Finish)
                item.SetCurrentValue(GanttChartItem.FinishProperty, clone.Finish);
            if (clone.CompletedFinish != item.CompletedFinish)
                item.SetCurrentValue(GanttChartItem.CompletedFinishProperty, clone.CompletedFinish);
        }
        private void ItemCompletedFinishChanged(object sender, EventArgs e)
        {
            ItemFinishChanged(sender, EventArgs.Empty);
            var item = sender as GanttChartItem;
            var clone = item.Tag as GanttChartItem;
            if (item.Finish != clone.Finish)
                clone.SetCurrentValue(GanttChartItem.FinishProperty, item.Finish);
            if (item.CompletedFinish != clone.CompletedFinish)
                clone.SetCurrentValue(GanttChartItem.CompletedFinishProperty, item.CompletedFinish);
        }
        private void ItemIsMilestoneChanged(object sender, EventArgs e)
        {
            var item = sender as GanttChartItem;
            var clone = item.Tag as GanttChartItem;
            if (!item.IsMilestone)
            {
                clone.SetCurrentValue(GanttChartItem.StartProperty, item.Start);
                clone.SetCurrentValue(GanttChartItem.FinishProperty, item.Finish);
                clone.SetCurrentValue(GanttChartItem.CompletedFinishProperty, item.CompletedFinish);
                finishPropertyDescriptor.AddValueChanged(clone, CloneFinishChanged);
                finishPropertyDescriptor.AddValueChanged(item, ItemFinishChanged);
                completedFinishPropertyDescriptor.AddValueChanged(clone, CloneCompletedFinishChanged);
                completedFinishPropertyDescriptor.AddValueChanged(item, ItemCompletedFinishChanged);
            }
            else
            {
                finishPropertyDescriptor.RemoveValueChanged(clone, CloneFinishChanged);
                finishPropertyDescriptor.RemoveValueChanged(item, ItemFinishChanged);
                completedFinishPropertyDescriptor.RemoveValueChanged(clone, CloneCompletedFinishChanged);
                completedFinishPropertyDescriptor.RemoveValueChanged(item, ItemCompletedFinishChanged);
                item.SetCurrentValue(GanttChartItem.StartProperty, clone.Start);
                item.SetCurrentValue(GanttChartItem.FinishProperty, clone.Finish);
                item.SetCurrentValue(GanttChartItem.CompletedFinishProperty, clone.CompletedFinish);
            }
            if (item.IsMilestone != clone.IsMilestone)
                clone.SetCurrentValue(GanttChartItem.IsMilestoneProperty, item.IsMilestone);
        }
        private class ItemHierarchyInfo
        {
            public GanttChartItem Item { get; set; }
            public IEnumerable<GanttChartItem> ParentItems { get; set; }
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
                        IEnumerable<GanttChartItem> parentItems = (clone.Tag as ItemHierarchyInfo).ParentItems;
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
