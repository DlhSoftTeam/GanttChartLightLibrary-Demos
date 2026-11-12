using System;
using System.Windows;
using DlhSoft.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Demos.WPF.CSharp.GanttChartDataGrid.AssignmentsTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string applicationName = GetType().Assembly.GetName().Name;

            // Associate each resource with a portrait used by the assignment template.
            GanttChartDataGrid.AssignableResources = new ObservableCollection<string>
            {
                "Clarissa Candelaria", "Steven Rush", "Tyson Lamberson", "Joanna Mcamis"
            };
            GanttChartDataGrid.ResourceImages = new Dictionary<string, ImageSource>
            {
                { "Clarissa Candelaria", new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Clarissa.png", applicationName), UriKind.Absolute)) },
                { "Steven Rush", new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Steven.png", applicationName), UriKind.Absolute)) },
                { "Tyson Lamberson", new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Tyson.png", applicationName), UriKind.Absolute)) },
                { "Joanna Mcamis", new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/Joanna.png", applicationName), UriKind.Absolute)) }
            };
            // Set this property to false to display resource names beside the portraits.
            // In image-only mode, tooltips show resource names and allocation percentages.
            GanttChartDataGrid.DisplayAssignedResourceImagesOnly = true;

            GanttChartItem itemRequirements = GanttChartDataGrid.Items[1];
            itemRequirements.Start = DateTime.Today.AddHours(8);
            itemRequirements.Finish = DateTime.Today.AddDays(3).AddHours(16);
            itemRequirements.CompletedFinish = DateTime.Today.AddHours(12);
            itemRequirements.AssignmentsContent = "Clarissa Candelaria, Steven Rush [50%]";

            GanttChartItem itemInterfaceDesign = GanttChartDataGrid.Items[2];
            itemInterfaceDesign.Start = DateTime.Today.AddDays(2).AddHours(8);
            itemInterfaceDesign.Finish = DateTime.Today.AddDays(5).AddHours(16);
            itemInterfaceDesign.CompletedFinish = itemInterfaceDesign.Start;
            itemInterfaceDesign.AssignmentsContent = "Steven Rush";
            itemInterfaceDesign.Predecessors.Add(new PredecessorItem { Item = itemRequirements });

            GanttChartItem itemImplementation = GanttChartDataGrid.Items[4];
            itemImplementation.Start = DateTime.Today.AddDays(4).AddHours(8);
            itemImplementation.Finish = DateTime.Today.AddDays(8).AddHours(16);
            itemImplementation.CompletedFinish = itemImplementation.Start;
            itemImplementation.AssignmentsContent = "Clarissa Candelaria, Steven Rush [50%]";
            itemImplementation.Predecessors.Add(new PredecessorItem { Item = itemInterfaceDesign });

            GanttChartItem itemTesting = GanttChartDataGrid.Items[5];
            itemTesting.Start = DateTime.Today.AddDays(8).AddHours(8);
            itemTesting.Finish = DateTime.Today.AddDays(12).AddHours(16);
            itemTesting.CompletedFinish = itemTesting.Start;
            itemTesting.AssignmentsContent = "Tyson Lamberson";
            itemTesting.Predecessors.Add(new PredecessorItem { Item = itemImplementation });

            GanttChartItem itemDocumentation = GanttChartDataGrid.Items[6];
            itemDocumentation.Start = DateTime.Today.AddDays(4).AddHours(8);
            itemDocumentation.Finish = DateTime.Today.AddDays(8).AddHours(16);
            itemDocumentation.CompletedFinish = itemDocumentation.Start;
            itemDocumentation.AssignmentsContent = "Joanna Mcamis [50%]";
            itemDocumentation.Predecessors.Add(new PredecessorItem { Item = itemInterfaceDesign });

            GanttChartItem itemReleaseReview = GanttChartDataGrid.Items[7];
            itemReleaseReview.Start = DateTime.Today.AddDays(10).AddHours(16);
            itemReleaseReview.CompletedFinish = itemReleaseReview.Start;
            itemReleaseReview.AssignmentsContent = "Clarissa Candelaria, Tyson Lamberson, Joanna Mcamis";
            itemReleaseReview.Predecessors.Add(new PredecessorItem { Item = itemTesting });
            itemReleaseReview.Predecessors.Add(new PredecessorItem { Item = itemDocumentation });
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
    }
}
