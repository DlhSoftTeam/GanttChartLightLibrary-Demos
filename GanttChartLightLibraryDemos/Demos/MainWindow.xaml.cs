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
                SelectComponent(selectedItem);
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
        private void SelectComponent(TreeViewItem selectedItem)
        {
            var firstChildItem = selectedItem.Items[0] as TreeViewItem;
            firstChildItem.IsSelected = true;
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
                                "Main" + (!isSilverlight ? "Window" : "Page") + ".xaml" + (!isVisualBasic ? ".cs" : ".vb")
                            };
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
