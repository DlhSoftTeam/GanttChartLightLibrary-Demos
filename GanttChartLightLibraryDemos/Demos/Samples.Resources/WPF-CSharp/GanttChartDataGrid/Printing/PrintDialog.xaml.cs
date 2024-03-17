using DlhSoft.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Demos.WPF.CSharp.GanttChartDataGrid.Printing
{
    /// <summary>
    /// Interaction logic for PrintDialog.xaml
    /// </summary>
    public partial class PrintDialog : Window
    {
        public PrintDialog()
        {
            InitializeComponent();
        }

        public MainWindow MainWindow => Owner as MainWindow;
        public DlhSoft.Windows.Controls.GanttChartDataGrid GanttChartDataGrid => MainWindow.GanttChartDataGrid;
        public List<ColumnSelector> GridColumns { get; set; } = new List<ColumnSelector>();
        public DateTime TimelinePageStart { get; set; }
        public DateTime TimelinePageFinish { get; set; }

        private const double PrintingThreshold = 12000000;

        public void Load()
        {
            TimelinePageStart = GanttChartDataGrid.Items.Any() ? GanttChartDataGrid.GetProjectStart().AddDays(-1) : GanttChartDataGrid.TimelinePageStart;
            TimelinePageFinish = GanttChartDataGrid.Items.Any() ? GanttChartDataGrid.GetProjectFinish().AddDays(7) : GanttChartDataGrid.TimelinePageFinish;

            foreach (var column in GanttChartDataGrid.Columns)
            {
                if (column.Header == null) continue;
                GridColumns.Add(new ColumnSelector
                {
                    Header = column.Header.ToString() == "" ? "Index" : column.Header.ToString(),
                    IsSelected = true,
                    Column = column
                });
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimelinePageStart >= TimelinePageFinish)
            {
                MessageBox.Show("The selected dates are incorrect. Please choose a valid timeline.", "Information", MessageBoxButton.OK);
                return;
            }

            var oldStart = GanttChartDataGrid.TimelinePageStart;
            var oldFinish = GanttChartDataGrid.TimelinePageFinish;

            IEnumerable<GanttChartItem> itemsForHiding = null;
            IEnumerable<DataGridColumn> visibleColumns = null;
            try
            {
                var dialog = new System.Windows.Controls.PrintDialog();
                // Optional: dialog.PrintTicket.PageOrientation = PageOrientation.Landscape;

                visibleColumns = GridColumns.Where(c => c.IsSelected).Select(c => c.Column);
                itemsForHiding = GanttChartDataGrid.Items
                    .Where(i => i.Finish < TimelinePageStart || i.Start > TimelinePageFinish)
                    .ToArray();

                var itemsCount = GanttChartDataGrid.Items.Count - itemsForHiding.Count();
                var timelineHours = GanttChartDataGrid.GetEffort(TimelinePageStart, TimelinePageFinish, GanttChartDataGrid.GetVisibilitySchedule()).TotalHours;
                var gridWidth = visibleColumns.Sum(c => c.ActualWidth);

                if (itemsCount * GanttChartDataGrid.ItemHeight * (gridWidth + timelineHours * GanttChartDataGrid.HourWidth) > PrintingThreshold)
                {
                    MessageBox.Show("The printed output would be too big. Please select a shorter timeline and/or fewer columns.", "Information", MessageBoxButton.OK);
                    return;
                }

                if (dialog.ShowDialog() == true)
                {
                    GanttChartDataGrid.SetTimelinePage(TimelinePageStart, TimelinePageFinish);

                    foreach (var column in GridColumns)
                    {
                        if (!column.IsSelected)
                            column.Column.Visibility = Visibility.Collapsed;
                    }

                    foreach (var item in itemsForHiding)
                        item.IsHidden = true;

                    var exportedSize = GanttChartDataGrid.GetExportSize();

                    //Printing on a single page, if the content fits (considering the margins defined in PrintingTemplate as well).
                    if (exportedSize.Width + 2 * 48 <= dialog.PrintableAreaWidth && exportedSize.Height + 2 * 32 <= dialog.PrintableAreaHeight)
                    {
                        GanttChartDataGrid.Export((Action)delegate
                        {
                            // Get a DrawingVisual representing the Gantt Chart content.
                            var exportedVisual = GanttChartDataGrid.GetExportDrawingVisual();
                            // Apply necessary transforms for the content to fit into the output page.
                            exportedVisual.Transform = GetPageFittingTransform(dialog);
                            // Actually print the visual.
                            var container = new Border();
                            container.Padding = new Thickness(48, 32, 48, 32);
                            container.Child = new Rectangle { Fill = new VisualBrush(exportedVisual), Width = exportedSize.Width, Height = exportedSize.Height };
                            dialog.PrintVisual(container, "Gantt Chart Document");
                        });
                    }
                    else
                    {
                        var documentPaginator = new DlhSoft.Windows.Controls.GanttChartDataGrid.DocumentPaginator(GanttChartDataGrid);
                        documentPaginator.PageSize = new Size(dialog.PrintableAreaWidth, dialog.PrintableAreaHeight);
                        dialog.PrintDocument(documentPaginator, "Gantt Chart Document");
                    }

                    Close();
                }
            }
            finally
            {
                Dispatcher.BeginInvoke((Action)delegate
                {
                    foreach (var columnSelector in GridColumns)
                    {
                        if (!columnSelector.IsSelected)
                            columnSelector.Column.Visibility = Visibility;
                    }

                    GanttChartDataGrid.SetTimelinePage(oldStart, oldFinish);

                    foreach (var item in itemsForHiding)
                        item.IsHidden = false;
                });
            }
        }

        private TransformGroup GetPageFittingTransform(System.Windows.Controls.PrintDialog printDialog)
        {
            // Determine scale to apply for page fitting.
            var scale = GetPageFittingScaleRatio(printDialog);
            // Set up a transform group in order to allow multiple transforms, if needed.
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform(scale, scale));
            // Optionally, add other transforms, such as supplemental translation, scale, or rotation as you need for the output presentation.
            return transformGroup;
        }

        private double GetPageFittingScaleRatio(System.Windows.Controls.PrintDialog printDialog)
        {
            // Determine the appropriate scale to apply based on export size and printable area size.
            var outputSize = GanttChartDataGrid.GetExportSize();
            var scaleX = printDialog.PrintableAreaWidth / outputSize.Width;
            var scaleY = printDialog.PrintableAreaHeight / outputSize.Height;
            var scale = Math.Min(scaleX, scaleY);
            return scale;
        }
    }

    public class ColumnSelector
    {
        public string Header { get; set; }
        public bool IsSelected { get; set; }
        public DataGridColumn Column { get; set; }
    }
}
