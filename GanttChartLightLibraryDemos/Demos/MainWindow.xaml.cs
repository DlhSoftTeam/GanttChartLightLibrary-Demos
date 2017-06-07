using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                var queryString = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;
                initialSelection = !string.IsNullOrEmpty(queryString) ? queryString.Substring(1).Replace('-', ' ') : null;
            }
            catch (DeploymentException) { }
            InitializeComponent();
        }

        private string initialSelection;

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(initialSelection))
            {
                foreach (TreeViewItem item in TreeView.Items)
                {
                    if (item.HasItems && item.Tag as string == initialSelection)
                    {
                        item.IsSelected = true;
                        break;
                    }
                }
            }
            if (TreeView.SelectedItem == null)
            {
                var firstItem = TreeView.Items[0] as TreeViewItem;
                firstItem.IsSelected = true;
            }
            TreeView.Focus();
        }
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = e.NewValue as TreeViewItem;
            if (selectedItem == null)
                return;
            if (selectedItem.HasItems)
            {
                var previouslySelectedChildItem = e.OldValue as TreeViewItem;
                if (previouslySelectedChildItem != null && previouslySelectedChildItem.HasItems)
                    previouslySelectedChildItem = null;
                SelectComponent(selectedItem, previouslySelectedChildItem);
                return;
            }
            else
            {
                if (previousSelectedParentItem != null)
                    previousSelectedParentItem.Style = previousSelectedParentItemStyle;
                var parentItem = selectedItem.Parent as TreeViewItem;
                previousSelectedParentItem = parentItem;
                previousSelectedParentItemStyle = parentItem.Style;
                parentItem.Style = Resources["SelectedTreeViewItemParentStyle"] as Style;
                Dispatcher.BeginInvoke((Action)LoadFiles);
                Dispatcher.BeginInvoke((Action)LoadContent);
            }
        }
        private void SelectComponent(TreeViewItem selectedItem, TreeViewItem previouslySelectedChildItem)
        {
            TreeViewItem itemToSelect = null;
            if (previouslySelectedChildItem != null)
            {
                foreach (TreeViewItem item in selectedItem.Items)
                {
                    if (item.Tag == previouslySelectedChildItem.Tag)
                    {
                        itemToSelect = item;
                        break;
                    }
                }
            }
            if (itemToSelect == null)
                itemToSelect = selectedItem.Items[0] as TreeViewItem;
            itemToSelect.IsSelected = true;
            selectedItem.IsExpanded = true;
        }
        private TreeViewItem previousSelectedParentItem;
        private Style previousSelectedParentItemStyle;
        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            var expandedItem = sender as TreeViewItem;
            if (!expandedItem.HasItems || !expandedItem.IsExpanded)
                return;
            expandedItem.IsSelected = true;
            foreach (TreeViewItem item in TreeView.Items)
            {
                if (item.HasItems && item.IsExpanded && item != expandedItem)
                    item.IsExpanded = false;
            }
        }

        private void TechnologyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)LoadTreeView);
            Dispatcher.BeginInvoke((Action)LoadFiles);
            Dispatcher.BeginInvoke((Action)LoadContent);
        }

        private void LoadTreeView()
        {
            TreeView.Items.Clear();
            var selectedTechnologyItem = TechnologyComboBox?.SelectedItem as ComboBoxItem;
            if (selectedTechnologyItem == null)
                return;
            var technology = selectedTechnologyItem.Tag as string;
            var technologySeparatorIndex = technology.IndexOf('-');
            var platform = technology.Substring(0, technologySeparatorIndex);
            var programmingLanguage = technology.Substring(technologySeparatorIndex + 1);
            ComponentInfo[] components = null;
            switch (platform)
            {
                case "WPF":
                    switch (programmingLanguage)
                    {
                        case "CSharp":
                            components = new[]
                            {
                                new ComponentInfo
                                {
                                    Name = "GanttChartDataGrid",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "BasicUsage", Title = "Basic usage", Description = "Shows how to load the component with minimum configuration" },
                                        new SampleInfo { Tag = "DataBinding", Title = "Data binding", Description = "Shows how you can data bind the component to a custom task item collection" },
                                        new SampleInfo { Tag = "CustomAppearance", Title = "Custom appearance", Description = "Shows how you can set colors for all or for individual task items" },
                                        new SampleInfo { Tag = "CustomSchedule", Title = "Custom schedule and scales", Description = "Shows how you can define working and nonworking times for scheduling task items and custom scales and headers for the chart view" },
                                        new SampleInfo { Tag = "Sorting", Title = "Sorting", Description = "Shows how you can hierarchically sort task items in the grid and chart view" },
                                        new SampleInfo { Tag = "Filtering", Title = "Filtering", Description = "Shows how you can hide specific task items for filtering purposes" },
                                        new SampleInfo { Tag = "ChangeNotifications", Title = "Change notifications", Description = "Shows how you can detect changes and perform custom actions when they occur" },
                                        new SampleInfo { Tag = "MouseEventHandling", Title = "Mouse event handling", Description = "Shows how you can determine the item and other elements bound to the cursor position for handling mouse events" },
                                        new SampleInfo { Tag = "Templating", Title = "Default templates", Description = "Source code providing expanded default XAML templates for specific component elements" },
                                        new SampleInfo { Tag = "BarTemplating", Title = "Bar templating", Description = "Shows how you can define XAML templates for task bars displayed in the chart view" },
                                        new SampleInfo { Tag = "MinuteScale", Title = "Minute scale", Description = "Shows how you can customize scales and display task bars bound to hours and minutes" },
                                        new SampleInfo { Tag = "NumericDays", Title = "Numeric days", Description = "Shows how you can customize scales to display project week and day numbers instead of dates" },
                                        new SampleInfo { Tag = "Recurrence", Title = "Recurrence", Description = "Shows how you can define custom code to generate and display recurrent task items and chart bars" },
                                        new SampleInfo { Tag = "SummaryBars", Title = "Summary bars", Description = "Shows how you can display child bars instead of summary bars when node items are collapsed" },
                                        new SampleInfo { Tag = "SummaryValues", Title = "Summary values", Description = "Shows how you can hierarchically summarize custom values such as custom task costs" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "GanttChartView",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "ScheduleChartDataGrid",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "BasicUsage", Title = "Basic usage", Description = "Shows how to load the component with minimum configuration" },
                                        new SampleInfo { Tag = "DataBinding", Title = "Data binding", Description = "Shows how you can data bind the component to a custom resource item collection" },
                                        new SampleInfo { Tag = "CustomAppearance", Title = "Custom appearance", Description = "Shows how you can set colors for all or for individual task items" },
                                        new SampleInfo { Tag = "CustomSchedule", Title = "Custom schedule and scales", Description = "Shows how you can define working and nonworking times for scheduling task items and custom scales and headers for the chart view" },
                                        new SampleInfo { Tag = "Hierarchy", Title = "Hierarchy", Description = "Shows how you can hierarchically display resource groups in the grid" },
                                        new SampleInfo { Tag = "MultipleLinesPerRow", Title = "Multiple lines per row", Description = "Shows how you can configure the component to display chart task bars using multiple lines per resource row automatically enlarging individual grid row height values" },
                                        new SampleInfo { Tag = "MouseEventHandling", Title = "Mouse event handling", Description = "Shows how you can determine the item and other elements bound to the cursor position for handling mouse events" },
                                        new SampleInfo { Tag = "BarTemplating", Title = "Bar templating", Description = "Shows how you can define XAML templates for task bars displayed in the chart view" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "ScheduleChartView",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "LoadChartDataGrid",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "CustomAppearance", Title = "Custom appearance", Description = "Shows how you can set colors for different types of allocation items" },
                                        new SampleInfo { Tag = "CustomSchedule", Title = "Custom schedule and scales", Description = "Shows how you can define working and nonworking times and custom scales and headers for the chart view" },
                                        new SampleInfo { Tag = "MouseEventHandling", Title = "Mouse event handling", Description = "Shows how you can determine the item and other elements bound to the cursor position for handling mouse events" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "LoadChartView",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "SingleItem", Title = "Single item", Description = "Shows how you can set up a single displayed item in the chart view" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "PertChartView",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "MultiTasksPerLine", Title = "Multiple tasks per line", Description = "Shows how you can extend task lines into multiple parallel items displayed between the same task event shapes, especially useful to avoid diagram complexity when generating items from a Gantt Chart source" },
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "NetworkDiagramView",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "ShapeTemplating", Title = "Shape templating", Description = "Shows how you can define XAML templates for task shapes displayed in the view, optionally enabling item property editing as needed" }
                                    }
                                }
                            };
                            break;
                        case "VisualBasic":
                            components = new[]
                            {
                                new ComponentInfo
                                {
                                    Name = "GanttChartDataGrid",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" }
                                    }
                                }
                            };
                            break;
                    }
                    break;
                case "Silverlight":
                    switch (programmingLanguage)
                    {
                        case "CSharp":
                            components = new[]
                            {
                                new ComponentInfo
                                {
                                    Name = "GanttChartDataGrid",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" }
                                    }
                                }
                            };
                            break;
                        case "VisualBasic":
                            components = new[]
                            {
                                new ComponentInfo
                                {
                                    Name = "GanttChartDataGrid",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" }
                                    }
                                }
                            };
                            break;
                    }
                    break;
            }
            if (components == null)
                return;
            bool isFirst = true;
            foreach (var component in components.Where(c => c.Features != null))
            {
                var componentItem = new TreeViewItem { Header = component.Name, Tag = component.Name, IsExpanded = isFirst };
                foreach (var feature in component.Features)
                {
                    componentItem.Items.Add(new TreeViewItem { Header = feature.Title, Tag = feature.Tag, ToolTip = feature.Description, IsSelected = isFirst });
                    isFirst = false;
                }
                TreeView.Items.Add(componentItem);
            }
        }

        internal class ComponentInfo
        {
            public string Name { get; set; }
            public SampleInfo[] Features { get; set; }
        }
        internal class SampleInfo
        {
            public string Tag { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }

        private void LoadFiles()
        {
            var fileItemCount = FilesListBox.Items.Count;
            var remainingItems = new[] { FilesListBox.Items[0], FilesListBox.Items[fileItemCount - 2], FilesListBox.Items[fileItemCount - 1] };
            var removingItems = new List<ListBoxItem>();
            foreach (ListBoxItem item in FilesListBox.Items)
            {
                if (!remainingItems.Contains(item))
                    removingItems.Add(item);
            }
            foreach (var item in removingItems)
                FilesListBox.Items.Remove(item);
            var selectedTreeViewItem = TreeView.SelectedItem as TreeViewItem;
            var selectedTreeViewParentItem = selectedTreeViewItem?.Parent as TreeViewItem;
            var selectedTechnologyItem = TechnologyComboBox?.SelectedItem as ComboBoxItem;
            if (selectedTreeViewItem == null || selectedTreeViewParentItem == null || selectedTechnologyItem == null)
                return;
            var component = selectedTreeViewParentItem.Tag as string;
            var feature = selectedTreeViewItem.Tag as string;
            var technology = selectedTechnologyItem.Tag as string;
            var isSilverlight = technology.StartsWith("Silverlight");
            var isVisualBasic = technology.EndsWith("VisualBasic");
            string[] fileItems = null;
            switch (component)
            {
                case "GanttChartDataGrid":
                    switch (feature)
                    {
                        case "MainFeatures":
                            fileItems = new[] {
                                "Main" + (!isSilverlight ? "Window" : "Page") + ".xaml",
                                "Main" + (!isSilverlight ? "Window" : "Page") + ".xaml" + (!isVisualBasic ? ".cs" : ".vb"),
                                "EditItemDialog.xaml",
                                "EditItemDialog.xaml" + (!isVisualBasic ? ".cs" : ".vb")
                            };
                            break;
                        case "DataBinding":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml.cs", "CustomTaskItem.cs" };
                            break;
                        case "BarTemplating":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml.cs", "CustomGanttChartItem.cs", "Interruption.cs", "Marker.cs" };
                            break;
                        case "NumericDays":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml.cs", "NumericDayStringConverter.cs" };
                            break;
                        case "Recurrence":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml.cs", "RecurrentGanttChartItem.cs", "RecurrenceType.cs", "UnlimitedIntConverter.cs" };
                            break;
                        case "SummaryValues":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml.cs", "CustomGanttChartItem.cs" };
                            break;
                    }
                    break;
                case "ScheduleChartDataGrid":
                    switch (feature)
                    {
                        case "BarTemplating":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml.cs", "CustomGanttChartItem.cs", "Interruption.cs", "Marker.cs" };
                            break;
                        case "DataBinding":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml.cs", "CustomResourceItem.cs" };
                            break;
                    }
                    break;
                case "NetworkDiagramView":
                    switch (feature)
                    {
                        case "ShapeTemplating":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml.cs", "CustomNetworkDiagramItem.cs" };
                            break;
                    }
                    break;
            }
            if (fileItems == null)
                fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml.cs" };
            int index = 1;
            foreach (var fileItem in fileItems)
                FilesListBox.Items.Insert(index++, new ListBoxItem { Content = fileItem, Tag = fileItem });
            if (!isSilverlight)
                FilesListBox.Items.Insert(index++, new ListBoxItem { Content = "AppResources.xaml", Tag = "AppResources.xaml" });
            if (FilesListBox.SelectedIndex > 0)
                FilesListBox.SelectedIndex = 0;
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)LoadContent);
        }

        private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)LoadContent);
        }

        private void LoadContent()
        {
            var selectedTreeViewItem = TreeView.SelectedItem as TreeViewItem;
            var selectedTreeViewParentItem = selectedTreeViewItem?.Parent as TreeViewItem;
            var selectedTechnologyItem = TechnologyComboBox?.SelectedItem as ComboBoxItem;
            if (selectedTreeViewItem == null || selectedTreeViewParentItem == null || selectedTechnologyItem == null)
                return;
            var component = selectedTreeViewParentItem.Tag as string;
            var feature = selectedTreeViewItem.Tag as string;
            var technology = selectedTechnologyItem.Tag as string;
            var technologySeparatorIndex = technology.IndexOf('-');
            var platform = technology.Substring(0, technologySeparatorIndex);
            var programmingLanguage = technology.Substring(technologySeparatorIndex + 1);
            var selectedFileItem = FilesListBox.SelectedItem as ListBoxItem;
            if (selectedFileItem == null || selectedFileItem.Visibility != Visibility.Visible)
            {
                var runListBoxItem = FilesListBox.Items[0] as ListBoxItem;
                FilesListBox.SelectedIndex = runListBoxItem.Visibility == Visibility.Visible ? 0 : 1;
                return;
            }
            var selectedFileUrl = selectedFileItem.Tag as string;
            if (selectedFileUrl == null)
            {
                if (containerWindow != null)
                    containerWindow.Close();
                var selectedThemeItem = ThemeComboBox.SelectedItem as ComboBoxItem;
                var theme = selectedThemeItem?.Tag as string;
                var path = "Demos." + platform + "." + programmingLanguage + "." + component + "." + feature;
                containerWindow = Activator.CreateInstance(path, path + ".MainWindow", false, BindingFlags.Default, null, new[] { theme }, null, null).Unwrap() as Window;
                ContentPresenter.Content = containerWindow.Content;
                ContentPresenter.Visibility = Visibility.Visible;
                ContentTextBox.Visibility = Visibility.Hidden;
                ContentTextBox.Text = null;
            }
            else
            {
                var file = selectedFileItem.Tag as string;
                try
                {
                    var resourceStreamInfo = Application.GetResourceStream(new Uri("/Samples.Resources/" + technology + "/" + component + "/" + feature + "/" + selectedFileUrl, UriKind.Relative));
                    using (var resourceStreamReader = new StreamReader(resourceStreamInfo.Stream))
                    {
                        ContentTextBox.Text = resourceStreamReader.ReadToEnd();
                    }
                }
                catch (IOException) { }
                ContentTextBox.Visibility = Visibility.Visible;
                ContentPresenter.Visibility = Visibility.Hidden;
                ContentPresenter.Content = null;
            }
        }

        private void GetZipButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTreeViewItem = TreeView.SelectedItem as TreeViewItem;
            var selectedTreeViewParentItem = selectedTreeViewItem?.Parent as TreeViewItem;
            var selectedTechnologyItem = TechnologyComboBox?.SelectedItem as ComboBoxItem;
            if (selectedTreeViewItem == null || selectedTreeViewParentItem == null || selectedTechnologyItem == null)
                return;
            var component = selectedTreeViewParentItem.Tag as string;
            var feature = selectedTreeViewItem.Tag as string;
            var technology = selectedTechnologyItem.Tag as string;
            string url = "http://DlhSoft.com/GanttChartLightLibrary/Demos/Samples/" + technology + "/" + component + "/" + feature + ".zip";
            Process.Start(new ProcessStartInfo(url));
        }

        private Window containerWindow;

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (containerWindow != null)
                containerWindow.Close();
        }
    }
}
