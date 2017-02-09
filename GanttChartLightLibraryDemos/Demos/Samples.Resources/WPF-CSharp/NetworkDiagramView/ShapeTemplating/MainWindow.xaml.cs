using System;
using System.Windows;
using DlhSoft.Windows.Controls.Pert;

namespace Demos.WPF.CSharp.NetworkDiagramView.ShapeTemplating
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CustomNetworkDiagramItem item0 = NetworkDiagramView.Items[0] as CustomNetworkDiagramItem;
            item0.EarlyStart = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item0.EarlyFinish = item0.EarlyStart;
            item0.LateStart = item0.EarlyStart;
            item0.LateFinish = item0.LateStart;
            item0.ActualStart = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item0.ActualFinish = item0.ActualStart;
            item0.AssignmentsContent = "N/A";

            CustomNetworkDiagramItem item1 = NetworkDiagramView.Items[1] as CustomNetworkDiagramItem;
            item1.Effort = TimeSpan.Parse("08:00:00");
            item1.EarlyStart = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item1.EarlyFinish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            item1.LateStart = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item1.LateFinish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            item1.ActualStart = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item1.ActualFinish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            item1.Slack = TimeSpan.Zero;
            item1.AssignmentsContent = "Resource 1";
            item1.Predecessors.Add(new NetworkPredecessorItem { Item = item0 });

            CustomNetworkDiagramItem item2 = NetworkDiagramView.Items[2] as CustomNetworkDiagramItem;
            item2.Effort = TimeSpan.Parse("04:00:00");
            item2.EarlyStart = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item2.EarlyFinish = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));
            item2.LateStart = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));
            item2.LateFinish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            item2.ActualStart = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item2.ActualFinish = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));
            item2.Slack = TimeSpan.Parse("04:00:00");
            item2.AssignmentsContent = "Resource 2";
            item2.Predecessors.Add(new NetworkPredecessorItem { Item = item0 });

            CustomNetworkDiagramItem item3 = NetworkDiagramView.Items[3] as CustomNetworkDiagramItem;
            item3.Effort = TimeSpan.Parse("16:00:00");
            item3.EarlyStart = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item3.EarlyFinish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item3.LateStart = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item3.LateFinish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item3.ActualStart = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item3.ActualFinish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item3.Slack = TimeSpan.Zero;
            item3.AssignmentsContent = "Resource 1, Resource 2";
            item3.Predecessors.Add(new NetworkPredecessorItem { Item = item1 });
            item3.Predecessors.Add(new NetworkPredecessorItem { Item = item2 });

            CustomNetworkDiagramItem item4 = NetworkDiagramView.Items[4] as CustomNetworkDiagramItem;
            item4.Effort = TimeSpan.Parse("04:00:00");
            item4.EarlyStart = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item4.EarlyFinish = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            item4.LateStart = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"));
            item4.LateFinish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item4.ActualStart = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"));
            item4.ActualFinish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item4.Slack = TimeSpan.Parse("12:00:00");
            item4.AssignmentsContent = "Resource 2";
            item4.Predecessors.Add(new NetworkPredecessorItem { Item = item1 });

            CustomNetworkDiagramItem item5 = NetworkDiagramView.Items[5] as CustomNetworkDiagramItem;
            item5.EarlyStart = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("08:00:00"));
            item5.EarlyFinish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("08:00:00"));
            item5.LateStart = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"));
            item5.LateFinish = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"));
            item5.ActualStart = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00"));
            item5.ActualFinish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00"));
            item5.Slack = TimeSpan.Parse("12:00:00");
            item5.AssignmentsContent = "Resource 2";
            item5.Predecessors.Add(new NetworkPredecessorItem { Item = item3 });
            item5.Predecessors.Add(new NetworkPredecessorItem { Item = item4 });

            CustomNetworkDiagramItem item6 = NetworkDiagramView.Items[6] as CustomNetworkDiagramItem;
            item6.Effort = TimeSpan.Parse("2.00:00:00");
            item6.EarlyStart = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"));
            item6.EarlyFinish = DateTime.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"));
            item6.LateStart = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"));
            item6.LateFinish = DateTime.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"));
            item6.ActualStart = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"));
            item6.ActualFinish = DateTime.Today.AddDays(10).Add(TimeSpan.Parse("10:00:00"));
            item6.Slack = TimeSpan.Zero;
            item6.AssignmentsContent = "Resource 1";
            item6.Predecessors.Add(new NetworkPredecessorItem { Item = item5 });

            CustomNetworkDiagramItem item7 = NetworkDiagramView.Items[7] as CustomNetworkDiagramItem;
            item7.Effort = TimeSpan.Parse("20:00:00");
            item7.EarlyStart = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"));
            item7.EarlyFinish = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"));
            item7.LateStart = DateTime.Today.AddDays(8).Add(TimeSpan.Parse("08:00:00"));
            item7.LateFinish = DateTime.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"));
            item7.ActualStart = DateTime.Today.AddDays(7).Add(TimeSpan.Parse("12:00:00"));
            item7.ActualFinish = DateTime.Today.AddDays(9).Add(TimeSpan.Parse("16:00:00"));
            item7.Slack = TimeSpan.Parse("1.04:00:00");
            item7.AssignmentsContent = "Resource 2";
            item7.Predecessors.Add(new NetworkPredecessorItem { Item = item5 });

            CustomNetworkDiagramItem item8 = NetworkDiagramView.Items[8] as CustomNetworkDiagramItem;
            item8.LateFinish = DateTime.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"));
            item8.LateStart = item8.LateFinish;
            item8.EarlyFinish = item8.LateFinish;
            item8.EarlyStart = item8.EarlyFinish;
            item8.ActualFinish = DateTime.Today.AddDays(10).Add(TimeSpan.Parse("10:00:00"));
            item8.ActualStart = item8.ActualFinish;
            item8.AssignmentsContent = "N/A";
            item8.Predecessors.Add(new NetworkPredecessorItem { Item = item6 });
            item8.Predecessors.Add(new NetworkPredecessorItem { Item = item7 });
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
            NetworkDiagramView.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }
    }
}
