using System;
using System.Windows;
using DlhSoft.Windows.Controls;
using System.Collections.ObjectModel;

namespace Demos.WPF.CSharp.GanttChartDataGrid.MultipleBarsPerLine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            for (int i = 1; i <= 32; i++)
            {
                GanttChartItem gridItem = new GanttChartItem { Content = String.Format("Item {0}", i) };
                gridItems.Add(gridItem);
                for (int j = 1; j <= 3; j++)
                {
                    GanttChartItem chartItem = new GanttChartItem
                    {
                        Content = String.Format("{0}.{1}", i, j),
                        Start = DateTime.Today.AddDays(j * (i + 4)),
                        Finish = DateTime.Today.AddDays(4 + j * (i + 4)),
                        DisplayRowIndex = i - 1
                    };
                    chartItems.Add(chartItem);
                }
            }

            // Component ApplyTemplate is called in order to complete loading of the user interface, after the main ApplyTemplate that initializes the custom theme, and using an asynchronous action to allow further constructor initializations if they exist (such as setting up the theme name to load).
            Dispatcher.BeginInvoke((Action)delegate
            {
                ApplyTemplate();

                // Apply template to be able to access the internal GanttChartView control.
                GanttChartDataGrid.ApplyTemplate();

                // Set up grid items and separately the internally computed bars to be displayed in the chart area (instead of GanttChartDataGrid.Items).
                // For optimization, also set DisplayRowCount on the inner GanttChartView component.
                GanttChartDataGrid.Items = gridItems;
                GanttChartDataGrid.GanttChartView.DisplayRowCount = gridItems.Count;
                GanttChartDataGrid.GanttChartView.Items = chartItems;
            });
        }

        private ObservableCollection<GanttChartItem> gridItems = new ObservableCollection<GanttChartItem>();
        private ObservableCollection<GanttChartItem> chartItems = new ObservableCollection<GanttChartItem>();

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
    }
}
