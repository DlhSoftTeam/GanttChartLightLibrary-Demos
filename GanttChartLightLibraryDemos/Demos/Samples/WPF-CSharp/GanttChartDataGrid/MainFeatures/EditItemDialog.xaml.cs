using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DlhSoft.Windows.Controls;
using ChartControl = DlhSoft.Windows.Controls.GanttChartDataGrid;

namespace Demos.WPF.CSharp.GanttChartDataGrid.MainFeatures
{
    public partial class EditItemDialog : Window
    {
        public ChartControl Chart { get; set; }
        public IList<string> AssignableResources { get; internal set; }
        private GanttChartItem Task { get { return (GanttChartItem)DataContext; } }
        private readonly ObservableCollection<DependencyDraft> dependencies = new ObservableCollection<DependencyDraft>();
        private readonly ObservableCollection<ResourceDraft> resources = new ObservableCollection<ResourceDraft>();
        private readonly HashSet<DependencyDraft> pendingDependencies = new HashSet<DependencyDraft>();
        private bool validatingDependency;
        private string originalDependencies;
        private string originalAssignments;

        public EditItemDialog()
        {
            InitializeComponent();
            dependencies.CollectionChanged += (sender, e) =>
            {
                if (e.OldItems != null) foreach (DependencyDraft row in e.OldItems) row.PropertyChanged -= DependencyChanged;
                if (e.NewItems != null) foreach (DependencyDraft row in e.NewItems) row.PropertyChanged += DependencyChanged;
            };
            Loaded += OnLoaded;
            Closing += OnClosing;
            EditorTabs.SelectionChanged += (sender, e) => { foreach (var r in resources) r.RefreshCost(); };
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DirectionColumn.ItemsSource = new[] { "Predecessor", "Successor" };
            DependencyTypeColumn.ItemsSource = Enum.GetValues(typeof(DependencyType));
            RelatedTaskColumn.ItemsSource = Chart.Items.Where(t => t != Task).Select(t => new TaskChoice { Item = t, Label = t.IndexString + " - " + t.Content }).ToList();
            DependenciesGrid.ItemsSource = dependencies;
            AssignmentsGrid.ItemsSource = resources;
            ReloadDependencies();
            ReloadResources();
        }

        private void ReloadDependencies()
        {
            foreach (var row in dependencies) row.PropertyChanged -= DependencyChanged;
            dependencies.Clear();
            foreach (var p in Task.Predecessors)
                dependencies.Add(new DependencyDraft { Direction = "Predecessor", Task = p.Item, Type = p.DependencyType, LagHours = p.Lag.TotalHours, OriginalDependency = p, OriginalOwner = Task });
            foreach (var other in Chart.Items)
                foreach (var p in other.Predecessors.Where(p => p.Item == Task))
                    dependencies.Add(new DependencyDraft { Direction = "Successor", Task = other, Type = p.DependencyType, LagHours = p.Lag.TotalHours, OriginalDependency = p, OriginalOwner = other });
            originalDependencies = DependencySignature();
        }

        private string DependencySignature()
        {
            return string.Join(";", dependencies.Select(d => d.Direction + ":" + (d.Task == null ? "?" : d.Task.Index.ToString()) + ":" + d.Type + ":" + d.LagHours.ToString("R", CultureInfo.InvariantCulture)).OrderBy(s => s));
        }

        private void ReloadResources()
        {
            resources.Clear();
            var assigned = Chart.GetAssignments(Task).ToDictionary(a => a.Key, a => a.Value);
            foreach (var name in (AssignableResources ?? new string[0]).Concat(assigned.Keys).Distinct())
            {
                double allocation;
                bool selected = assigned.TryGetValue(name, out allocation);
                resources.Add(new ResourceDraft(Task, Chart) { Name = name, Assigned = selected, AllocationPercent = selected ? allocation * 100 : 100 });
            }
            originalAssignments = AssignmentSignature();
        }

        private string AssignmentSignature()
        {
            return string.Join(";", resources.Select(r => r.Name + ":" + r.Assigned + ":" + r.AllocationPercent.ToString("R", CultureInfo.InvariantCulture)));
        }

        private void AddDependency_Click(object sender, RoutedEventArgs e)
        {
            dependencies.Add(new DependencyDraft { Direction = "Predecessor", Type = DependencyType.FinishStart });
            DependenciesGrid.SelectedItem = dependencies.Last();
        }

        private void RemoveDependency_Click(object sender, RoutedEventArgs e)
        {
            var selected = DependenciesGrid.SelectedItem as DependencyDraft;
            if (selected != null) RejectDependency(selected);
        }

        private void DependencyChanged(object sender, PropertyChangedEventArgs e)
        {
            var row = (DependencyDraft)sender;
            if (validatingDependency || row.Task == null || !pendingDependencies.Add(row)) return;
            // Let the ComboBox finish transferring its selection before committing/removing a row.
            Dispatcher.BeginInvoke(new Action(() =>
            {
                pendingDependencies.Remove(row);
                if (!dependencies.Contains(row)) return;
                validatingDependency = true;
                try
                {
                    if (!Commit(DependenciesGrid)) return;
                    ValidateDependencyChange(row, ConfirmChanges,
                        message => MessageBox.Show(this, message, "Invalid dependency", MessageBoxButton.OK, MessageBoxImage.Warning));
                }
                finally { validatingDependency = false; }
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        private void RejectDependency(DependencyDraft row)
        {
            if (row.OriginalDependency != null)
                row.OriginalOwner.Predecessors.Remove(row.OriginalDependency);
            dependencies.Remove(row);
            ReloadDependencies();
        }

        private static bool Commit(DataGrid grid)
        {
            return grid.CommitEdit(DataGridEditingUnit.Cell, true) && grid.CommitEdit(DataGridEditingUnit.Row, true);
        }

        private List<Link> ProposedLinks()
        {
            return BuildLinks(dependencies);
        }

        private List<Link> BuildLinks(IEnumerable<DependencyDraft> drafts)
        {
            var result = Chart.Items.SelectMany(t => t.Predecessors.Where(p => t != Task && p.Item != Task)
                .Select(p => new Link { From = p.Item, To = t, Type = p.DependencyType, Lag = p.Lag })).ToList();
            foreach (var d in drafts)
            {
                if (d.Task == null || d.Task == Task || !Chart.Items.Contains(d.Task))
                    throw new InvalidOperationException("Choose another task for every dependency.");
                if (double.IsNaN(d.LagHours) || double.IsInfinity(d.LagHours) || Math.Abs(d.LagHours) > 876000)
                    throw new InvalidOperationException("Lag must be a finite number between -876000 and 876000 hours.");
                var link = new Link { From = d.Direction == "Predecessor" ? d.Task : Task, To = d.Direction == "Predecessor" ? Task : d.Task, Type = d.Type, Lag = TimeSpan.FromHours(d.LagHours) };
                if (result.Any(l => l.From == link.From && l.To == link.To))
                    throw new InvalidOperationException("The same pair of tasks cannot have duplicate dependencies.");
                result.Add(link);
            }
            return result;
        }

        // Expand summary-task endpoints to descendants as their dates are linked by the hierarchy.
        // This also rejects ancestor/descendant links and indirect cycles through summaries.
        internal static bool HasCycle(IList<GanttChartItem> items, IEnumerable<Link> links)
        {
            var graph = items.ToDictionary(t => t, t => new HashSet<GanttChartItem>());
            foreach (var link in links)
            {
                var from = items.Where(t => t == link.From || IsDescendant(items, t, link.From)).ToList();
                var to = items.Where(t => t == link.To || IsDescendant(items, t, link.To)).ToList();
                foreach (var a in from) foreach (var b in to) graph[a].Add(b);
            }
            var marks = new Dictionary<GanttChartItem, int>();
            return items.Any(t => Visit(t, graph, marks));
        }

        private static bool IsDescendant(IList<GanttChartItem> items, GanttChartItem item, GanttChartItem parent)
        {
            int start = items.IndexOf(parent), end = items.IndexOf(item);
            if (start < 0 || end <= start) return false;
            for (int i = start + 1; i <= end; i++)
                if (items[i].Indentation <= parent.Indentation) return false;
            return true;
        }

        private static bool Visit(GanttChartItem item, Dictionary<GanttChartItem, HashSet<GanttChartItem>> graph, Dictionary<GanttChartItem, int> marks)
        {
            int mark;
            if (marks.TryGetValue(item, out mark)) return mark == 1;
            marks[item] = 1;
            foreach (var next in graph[item]) if (Visit(next, graph, marks)) return true;
            marks[item] = 2;
            return false;
        }

        private void ValidateDependencyChange(DependencyDraft row, Func<string, bool> confirm, Action<string> warn)
        {
            if (DependencySignature() == originalDependencies) return;
            try
            {
                var links = BuildLinks(dependencies.Where(d => d.Task != null));
                if (HasCycle(Chart.Items.ToList(), links))
                    throw new InvalidOperationException("This change creates a circular dependency (possibly through a summary task). The selected dependency will be removed.");
                DlhSoft.Windows.Controls.GanttChartView preview;
                IsEnabled = false;
                try { preview = BuildPreview(links); }
                finally { IsEnabled = true; }
                var message = new StringBuilder("Dependencies for " + Task.IndexString + " - " + Task.Content + ":\n\n");
                message.AppendLine("Current:");
                foreach (var line in CurrentLinks().Select(Describe)) message.AppendLine(line);
                message.AppendLine("\nProposed:");
                var changedLinks = links.Where(l => l.From == Task || l.To == Task).ToList();
                if (changedLinks.Count == 0) message.AppendLine("No dependencies.");
                foreach (var line in changedLinks.Select(Describe)) message.AppendLine(line);
                message.AppendLine("\nSchedule changes:");
                int changes = 0;
                for (int i = 0; i < Chart.Items.Count; i++)
                {
                    var before = Chart.Items[i]; var after = preview.Items[i];
                    if (before.Start == after.Start && before.Finish == after.Finish) continue;
                    changes++;
                    message.AppendLine(string.Format("{0} - {1}:\n  Start: {2:g} → {3:g}\n  Finish: {4:g} → {5:g}", before.IndexString, before.Content, before.Start, after.Start, before.Finish, after.Finish));
                }
                if (changes == 0) message.AppendLine("No start or finish dates change.");
                message.AppendLine("\nApply these changes?");
                message.AppendLine("If you decline, this dependency will be removed.");
                if (changes > 0 && !confirm(message.ToString()))
                {
                    RejectDependency(row);
                    return;
                }
                // All validation and scheduling preview happen on separate objects before this point.
                bool enforce = Chart.AreTaskDependencyConstraintsEnabled;
                Chart.AreTaskDependencyConstraintsEnabled = false;
                try
                {
                    foreach (var item in Chart.Items)
                        foreach (var p in item.Predecessors.Where(p => item == Task || p.Item == Task).ToList())
                            if (!changedLinks.Any(l => l.To == item && l.From == p.Item && l.Type == p.DependencyType && l.Lag == p.Lag)) item.Predecessors.Remove(p);
                    foreach (var l in changedLinks)
                        if (!l.To.Predecessors.Any(p => p.Item == l.From && p.DependencyType == l.Type && p.Lag == l.Lag))
                            l.To.Predecessors.Add(new PredecessorItem { Item = l.From, DependencyType = l.Type, Lag = l.Lag });
                }
                finally { Chart.AreTaskDependencyConstraintsEnabled = enforce; }
                if (enforce) Chart.EnsureDependencyConstraints();
                ReloadDependencies();
                foreach (var r in resources) r.RefreshCost();
            }
            catch (InvalidOperationException ex)
            {
                warn(ex.Message + "\nThe dependency has been removed.");
                RejectDependency(row);
            }
        }

        private IEnumerable<Link> CurrentLinks()
        {
            return Chart.Items.SelectMany(t => t.Predecessors.Where(p => t == Task || p.Item == Task).Select(p => new Link { From = p.Item, To = t, Type = p.DependencyType, Lag = p.Lag }));
        }

        private static string Describe(Link l)
        {
            return string.Format("{0} - {1} → {2} - {3} ({4}, lag {5:0.##} h)", l.From.IndexString, l.From.Content, l.To.IndexString, l.To.Content, l.Type, l.Lag.TotalHours);
        }

        private DlhSoft.Windows.Controls.GanttChartView BuildPreview(List<Link> links)
        {
            var preview = new DlhSoft.Windows.Controls.GanttChartView { AreTaskDependencyConstraintsEnabled = false, AreHierarchyConstraintsEnabled = Chart.AreHierarchyConstraintsEnabled, Schedule = Chart.Schedule, ResourceSchedules = Chart.ResourceSchedules };
            preview.ApplyTemplate();
            preview.BeginUpdateItems();
            foreach (var t in Chart.Items)
                preview.Items.Add(new GanttChartItem { Content = t.Content, Indentation = t.Indentation, Start = t.Start, Finish = t.Finish, CompletedFinish = t.CompletedFinish, IsMilestone = t.IsMilestone, AssignmentsContent = t.AssignmentsContent, Schedule = t.Schedule, AreDependencyConstraintsEnabled = t.AreDependencyConstraintsEnabled, HasFixedEffort = t.HasFixedEffort });
            foreach (var l in links)
                preview.Items[Chart.Items.IndexOf(l.To)].Predecessors.Add(new PredecessorItem { Item = preview.Items[Chart.Items.IndexOf(l.From)], DependencyType = l.Type, Lag = l.Lag });
            preview.EndUpdateItems();
            preview.AreTaskDependencyConstraintsEnabledMaxSteps = Chart.AreTaskDependencyConstraintsEnabledMaxSteps;
            preview.AreTaskDependencyConstraintsEnabled = Chart.AreTaskDependencyConstraintsEnabled;
            if (preview.AreTaskDependencyConstraintsEnabled) preview.EnsureDependencyConstraints();
            // The library queues hierarchy and scheduling work on the dispatcher.
            preview.Dispatcher.Invoke(new Action(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            if (preview.AreTaskDependencyConstraintsEnabled) preview.EnsureDependencyConstraints();
            preview.Dispatcher.Invoke(new Action(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            return preview;
        }

        private bool ConfirmChanges(string text)
        {
            var dialog = new Window { Title = "Confirm dependency changes", Owner = this, Width = 680, Height = 520, WindowStartupLocation = WindowStartupLocation.CenterOwner, FontFamily = FontFamily, FontSize = 13 };
            var panel = new DockPanel { Margin = new Thickness(20) };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 16, 0, 0) };
            var yes = new Button { Content = "Apply changes", Padding = new Thickness(18, 8, 18, 8), Margin = new Thickness(0, 0, 10, 0) };
            var no = new Button { Content = "Remove dependency", Padding = new Thickness(18, 8, 18, 8), IsCancel = true, IsDefault = true };
            yes.Click += (s, e) => dialog.DialogResult = true;
            no.Click += (s, e) => dialog.DialogResult = false;
            buttons.Children.Add(yes); buttons.Children.Add(no); DockPanel.SetDock(buttons, Dock.Bottom); panel.Children.Add(buttons);
            panel.Children.Add(new TextBox { Text = text, IsReadOnly = true, TextWrapping = TextWrapping.Wrap, VerticalScrollBarVisibility = ScrollBarVisibility.Auto, BorderThickness = new Thickness(0) });
            dialog.Content = panel;
            return dialog.ShowDialog() == true;
        }

        private void ApplyAssignments_Click(object sender, RoutedEventArgs e)
        {
            if (!Commit(AssignmentsGrid)) return;
            if (resources.Any(r => double.IsNaN(r.AllocationPercent) || double.IsInfinity(r.AllocationPercent) || r.AllocationPercent < 0 || (r.Assigned && r.AllocationPercent == 0)))
            {
                MessageBox.Show(this, "Assigned resources require a positive, finite allocation percentage.", "Invalid allocation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Let the library format resource names and fractional units for the current culture.
            var draft = new GanttChartItem();
            foreach (var r in resources.Where(r => r.Assigned))
                Chart.AddAssignment(draft, new KeyValuePair<string, double>(r.Name, r.AllocationPercent / 100));
            Task.AssignmentsContent = draft.AssignmentsContent;
            ReloadResources();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (!Commit(DependenciesGrid) || !Commit(AssignmentsGrid)) { e.Cancel = true; return; }
            if (DependencySignature() != originalDependencies || AssignmentSignature() != originalAssignments)
                e.Cancel = MessageBox.Show(this, "Discard unapplied dependency and assignment edits? Definition changes have already been applied.", "Unapplied changes", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) { Close(); }

        public sealed class TaskChoice { public GanttChartItem Item { get; set; } public string Label { get; set; } }
        public sealed class DependencyDraft : INotifyPropertyChanged
        {
            private string direction;
            private GanttChartItem task;
            private DependencyType type;
            private double lagHours;
            public string Direction { get { return direction; } set { if (direction == value) return; direction = value; Changed("Direction"); } }
            public GanttChartItem Task { get { return task; } set { if (task == value) return; task = value; Changed("Task"); } }
            public DependencyType Type { get { return type; } set { if (type == value) return; type = value; Changed("Type"); } }
            public double LagHours { get { return lagHours; } set { if (lagHours.Equals(value)) return; lagHours = value; Changed("LagHours"); } }
            internal PredecessorItem OriginalDependency;
            internal GanttChartItem OriginalOwner;
            public event PropertyChangedEventHandler PropertyChanged;
            private void Changed(string name) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name)); }
        }
        internal sealed class Link
        {
            public GanttChartItem From; public GanttChartItem To; public DependencyType Type; public TimeSpan Lag;
        }
        public sealed class ResourceDraft : INotifyPropertyChanged
        {
            private readonly GanttChartItem task;
            private readonly ChartControl chart;
            private bool assigned;
            private double allocationPercent;
            public ResourceDraft(GanttChartItem task, ChartControl chart) { this.task = task; this.chart = chart; }
            public string Name { get; set; }
            public bool Assigned { get { return assigned; } set { assigned = value; RefreshCost(); } }
            public double AllocationPercent { get { return allocationPercent; } set { allocationPercent = value; RefreshCost(); } }
            public string ResourceType { get { return chart.ResourceQuantities != null && chart.ResourceQuantities.ContainsKey(Name) ? "Material" : "Human"; } }
            public double Cost
            {
                get
                {
                    if (!Assigned) return 0;
                    double usage = chart.DefaultResourceUsageCost, hourly = chart.DefaultResourceHourCost, value;
                    if (chart.SpecificResourceUsageCosts != null && chart.SpecificResourceUsageCosts.TryGetValue(Name, out value)) usage = value;
                    if (chart.SpecificResourceHourCosts != null && chart.SpecificResourceHourCosts.TryGetValue(Name, out value)) hourly = value;
                    return AllocationPercent / 100 * (usage + (task.IsMilestone ? 0 : task.Effort.TotalHours * hourly));
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            public void RefreshCost() { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Cost")); }
        }
    }
}
