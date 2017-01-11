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

namespace Demos.WPF.CSharp.PertChartView.MainFeatures
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            PertChartItem item0 = PertChartView.Items[0];

            PertChartItem item1 = PertChartView.Items[1];
            item1.Predecessors.Add(new PredecessorItem { Item = item0, DisplayedText = "A", Content = "Task A", Effort = TimeSpan.Parse("4") });

            PertChartItem item2 = PertChartView.Items[2];
            item2.Predecessors.Add(new PredecessorItem { Item = item0, DisplayedText = "B", Content = "Task B", Effort = TimeSpan.Parse("2") });

            PertChartItem item3 = PertChartView.Items[3];
            item3.Predecessors.Add(new PredecessorItem { Item = item2, DisplayedText = "C", Content = "Task C", Effort = TimeSpan.Parse("1") });

            PertChartItem item4 = PertChartView.Items[4];
            item4.Predecessors.Add(new PredecessorItem { Item = item1, DisplayedText = "D", Content = "Task D", Effort = TimeSpan.Parse("5") });
            item4.Predecessors.Add(new PredecessorItem { Item = item2, DisplayedText = "E", Content = "Task E", Effort = TimeSpan.Parse("3") });
            item4.Predecessors.Add(new PredecessorItem { Item = item3, DisplayedText = "F", Content = "Task F", Effort = TimeSpan.Parse("2") });
        }

        public MainWindow(string theme) : this()
        {
            if (theme == null || theme == "Default" || theme == "Aero")
                return;
            var themeResourceDictionary = new ResourceDictionary { Source = new Uri("/" + GetType().Assembly.GetName().Name + ";component/Themes/" + theme + ".xaml", UriKind.Relative) };
            PertChartView.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }

        // Control area commands.
        private void SetColorButton_Click(object sender, RoutedEventArgs e)
        {
            PertChartItem item = PertChartView.Items[2];
            DlhSoft.Windows.Controls.Pert.PertChartView.SetShapeFill(item, Resources["CustomShapeFill"] as Brush);
            DlhSoft.Windows.Controls.Pert.PertChartView.SetShapeStroke(item, Resources["CustomShapeStroke"] as Brush);
            DlhSoft.Windows.Controls.Pert.PertChartView.SetTextForeground(item, Resources["CustomShapeStroke"] as Brush);
        }
        private void SetDependencyColorButton_Click(object sender, RoutedEventArgs e)
        {
            PredecessorItem predecessorItem = PertChartView.Items[2].Predecessors[0];
            DlhSoft.Windows.Controls.Pert.PertChartView.SetDependencyLineStroke(predecessorItem, Resources["CustomDependencyLineStroke"] as Brush);
            DlhSoft.Windows.Controls.Pert.PertChartView.SetDependencyTextForeground(predecessorItem, Resources["CustomDependencyLineStroke"] as Brush);
        }
        private void CriticalPathCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SetColorButton.IsEnabled = false;
            SetDependencyColorButton.IsEnabled = false;
            foreach (PertChartItem item in PertChartView.ManagedItems)
            {
                SetCriticalPathHighlighting(item, false);
                if (item.Predecessors != null)
                {
                    foreach (PredecessorItem predecessorItem in item.Predecessors)
                        SetCriticalPathHighlighting(predecessorItem, false);
                }
            }
            foreach (PertChartItem item in PertChartView.GetCriticalItems())
                SetCriticalPathHighlighting(item, true);
            foreach (PredecessorItem predecessorItem in PertChartView.GetCriticalDependencies())
                SetCriticalPathHighlighting(predecessorItem, true);
        }
        private void CriticalPathCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (PredecessorItem predecessorItem in PertChartView.GetCriticalDependencies())
                SetCriticalPathHighlighting(predecessorItem, false);
            foreach (PertChartItem item in PertChartView.GetCriticalItems())
                SetCriticalPathHighlighting(item, false);
            SetDependencyColorButton.IsEnabled = true;
            SetColorButton.IsEnabled = true;
        }
        private void SetCriticalPathHighlighting(PertChartItem item, bool isHighlighted)
        {
            DlhSoft.Windows.Controls.Pert.PertChartView.SetShapeFill(item, isHighlighted ? Resources["CustomShapeFill"] as Brush : PertChartView.ShapeFill);
            DlhSoft.Windows.Controls.Pert.PertChartView.SetShapeStroke(item, isHighlighted ? Resources["CustomShapeStroke"] as Brush : PertChartView.ShapeStroke);
            DlhSoft.Windows.Controls.Pert.PertChartView.SetTextForeground(item, isHighlighted ? Resources["CustomShapeStroke"] as Brush : PertChartView.TextForeground);
        }
        private void SetCriticalPathHighlighting(PredecessorItem predecessorItem, bool isHighlighted)
        {
            DlhSoft.Windows.Controls.Pert.PertChartView.SetDependencyLineStroke(predecessorItem, isHighlighted ? Resources["CustomDependencyLineStroke"] as Brush : PertChartView.DependencyLineStroke);
            DlhSoft.Windows.Controls.Pert.PertChartView.SetDependencyTextForeground(predecessorItem, isHighlighted ? Resources["CustomDependencyLineStroke"] as Brush : PertChartView.DependencyTextForeground);
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PertChartView.Print("PertChartView Sample Document");
        }
        private void ExportImageButton_Click(object sender, RoutedEventArgs e)
        {
            PertChartView.Export((Action)delegate
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog { Title = "Export Image To", Filter = "PNG image files|*.png" };
                if (saveFileDialog.ShowDialog() != true)
                    return;
                BitmapSource bitmapSource = PertChartView.GetExportBitmapSource(96 * 2);
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
