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
using DlhSoft.Windows.Controls.Pert;
using Microsoft.Win32;
using System.IO;
using System.Windows.Threading;

namespace Demos.WPF.CSharp.NetworkDiagramView.MainFeatures
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            NetworkDiagramItem item0 = NetworkDiagramView.Items[0];
            item0.EarlyStart = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item0.EarlyFinish = item0.EarlyStart;
            item0.LateStart = item0.EarlyStart;
            item0.LateFinish = item0.LateStart;
            item0.AssignmentsContent = "N/A";

            NetworkDiagramItem item1 = NetworkDiagramView.Items[1];
            item1.Effort = TimeSpan.Parse("08:00:00");
            item1.EarlyStart = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item1.EarlyFinish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            item1.LateStart = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item1.LateFinish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            item1.Slack = TimeSpan.Zero;
            item1.AssignmentsContent = "Resource 1";
            item1.Predecessors.Add(new NetworkPredecessorItem { Item = item0 });

            NetworkDiagramItem item2 = NetworkDiagramView.Items[2];
            item2.Effort = TimeSpan.Parse("04:00:00");
            item2.EarlyStart = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item2.EarlyFinish = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));
            item2.LateStart = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));
            item2.LateFinish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            item2.Slack = TimeSpan.Parse("04:00:00");
            item2.AssignmentsContent = "Resource 2";
            item2.Predecessors.Add(new NetworkPredecessorItem { Item = item0 });

            NetworkDiagramItem item3 = NetworkDiagramView.Items[3];
            item3.Effort = TimeSpan.Parse("16:00:00");
            item3.EarlyStart = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item3.EarlyFinish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item3.LateStart = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item3.LateFinish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item3.Slack = TimeSpan.Zero;
            item3.AssignmentsContent = "Resource 1, Resource 2";
            item3.Predecessors.Add(new NetworkPredecessorItem { Item = item1 });
            item3.Predecessors.Add(new NetworkPredecessorItem { Item = item2 });

            NetworkDiagramItem item4 = NetworkDiagramView.Items[4];
            item4.Effort = TimeSpan.Parse("04:00:00");
            item4.EarlyStart = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item4.EarlyFinish = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            item4.LateStart = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"));
            item4.LateFinish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item4.Slack = TimeSpan.Parse("12:00:00");
            item4.AssignmentsContent = "Resource 2";
            item4.Predecessors.Add(new NetworkPredecessorItem { Item = item1 });

            NetworkDiagramItem item5 = NetworkDiagramView.Items[5];
            item5.Effort = TimeSpan.Parse("12:00:00");
            item5.EarlyStart = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("08:00:00"));
            item5.EarlyFinish = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"));
            item5.LateStart = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("08:00:00"));
            item5.LateFinish = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"));
            item5.Slack = TimeSpan.Zero;
            item5.AssignmentsContent = "Resource 2";
            item5.Predecessors.Add(new NetworkPredecessorItem { Item = item3 });
            item5.Predecessors.Add(new NetworkPredecessorItem { Item = item4 });

            NetworkDiagramItem item6 = NetworkDiagramView.Items[6];
            item6.Effort = TimeSpan.Parse("2.00:00:00");
            item6.EarlyStart = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"));
            item6.EarlyFinish = DateTime.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"));
            item6.LateStart = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"));
            item6.LateFinish = DateTime.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"));
            item6.Slack = TimeSpan.Zero;
            item6.AssignmentsContent = "Resource 1";
            item6.Predecessors.Add(new NetworkPredecessorItem { Item = item5 });

            NetworkDiagramItem item7 = NetworkDiagramView.Items[7];
            item7.Effort = TimeSpan.Parse("20:00:00");
            item7.EarlyStart = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"));
            item7.EarlyFinish = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"));
            item7.LateStart = DateTime.Today.AddDays(8).Add(TimeSpan.Parse("08:00:00"));
            item7.LateFinish = DateTime.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"));
            item7.Slack = TimeSpan.Parse("1.04:00:00");
            item7.AssignmentsContent = "Resource 2";
            item7.Predecessors.Add(new NetworkPredecessorItem { Item = item5 });

            NetworkDiagramItem item8 = NetworkDiagramView.Items[8];
            item8.LateFinish = DateTime.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"));
            item8.LateStart = item8.LateFinish;
            item8.EarlyFinish = item8.LateFinish;
            item8.EarlyStart = item8.EarlyFinish;
            item8.AssignmentsContent = "N/A";
            item8.Predecessors.Add(new NetworkPredecessorItem { Item = item6 });
            item8.Predecessors.Add(new NetworkPredecessorItem { Item = item7 });

            // Optionally, reposition start and finish milestones between the first and second rows of the view.
            // Dispatcher.BeginInvoke((Action)delegate { NetworkDiagramView.RepositionEnds(); }, DispatcherPriority.Background);
        }

        public MainWindow(string theme) : this()
        {
            if (theme == null || theme == "Default" || theme == "Aero")
                return;
            var themeResourceDictionary = new ResourceDictionary { Source = new Uri("/" + GetType().Assembly.GetName().Name + ";component/Themes/" + theme + ".xaml", UriKind.Relative) };
            NetworkDiagramView.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }

        // Control area commands.
        private void SetColorButton_Click(object sender, RoutedEventArgs e)
        {
            NetworkDiagramItem item = NetworkDiagramView.Items[2];
            DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetShapeFill(item, Resources["CustomShapeFill"] as Brush);
            DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetShapeStroke(item, Resources["CustomShapeStroke"] as Brush);
        }
        private void SetDependencyColorButton_Click(object sender, RoutedEventArgs e)
        {
            NetworkPredecessorItem predecessorItem = NetworkDiagramView.Items[2].Predecessors[0];
            DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetDependencyLineStroke(predecessorItem, Resources["CustomDependencyLineStroke"] as Brush);
        }
        private void CriticalPathCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SetColorButton.IsEnabled = false;
            foreach (NetworkDiagramItem item in NetworkDiagramView.ManagedItems)
            {
                SetCriticalPathHighlighting(item, false);
                if (item.Predecessors != null)
                {
                    foreach (NetworkPredecessorItem predecessorItem in item.Predecessors)
                        SetCriticalPathHighlighting(predecessorItem, false);
                }
            }
            foreach (NetworkDiagramItem item in NetworkDiagramView.GetCriticalItems())
                SetCriticalPathHighlighting(item, true);
            foreach (NetworkPredecessorItem predecessorItem in NetworkDiagramView.GetCriticalDependencies())
                SetCriticalPathHighlighting(predecessorItem, true);
        }
        private void CriticalPathCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (NetworkPredecessorItem predecessorItem in NetworkDiagramView.GetCriticalDependencies())
                SetCriticalPathHighlighting(predecessorItem, false);
            foreach (NetworkDiagramItem item in NetworkDiagramView.GetCriticalItems())
                SetCriticalPathHighlighting(item, false);
            SetColorButton.IsEnabled = true;
        }
        private void SetCriticalPathHighlighting(NetworkDiagramItem item, bool isHighlighted)
        {
            DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetShapeFill(item, isHighlighted ? Resources["CustomShapeFill"] as Brush : NetworkDiagramView.ShapeFill);
            DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetShapeStroke(item, isHighlighted ? Resources["CustomShapeStroke"] as Brush : NetworkDiagramView.ShapeStroke);
        }
        private void SetCriticalPathHighlighting(NetworkPredecessorItem predecessorItem, bool isHighlighted)
        {
            DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetDependencyLineStroke(predecessorItem, isHighlighted ? Resources["CustomDependencyLineStroke"] as Brush : NetworkDiagramView.DependencyLineStroke);
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            NetworkDiagramView.Print("NetworkDiagramView Sample Document");
        }
        private void ExportImageButton_Click(object sender, RoutedEventArgs e)
        {
            NetworkDiagramView.Export((Action)delegate
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog { Title = "Export Image To", Filter = "PNG image files|*.png" };
                if (saveFileDialog.ShowDialog() != true)
                    return;
                BitmapSource bitmapSource = NetworkDiagramView.GetExportBitmapSource(96 * 2);
                using (Stream stream = saveFileDialog.OpenFile())
                {
                    PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
                    pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    pngBitmapEncoder.Save(stream);
                }
            });
        }
    }
}
