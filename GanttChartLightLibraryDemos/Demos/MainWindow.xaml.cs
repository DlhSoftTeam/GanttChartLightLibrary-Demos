using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            var firstItem = TreeView.Items[0] as TreeViewItem;
            firstItem.IsSelected = true;
        }
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = e.NewValue as TreeViewItem;
            if (selectedItem.HasItems)
            {
                var firstChildItem = selectedItem.Items[0] as TreeViewItem;
                firstChildItem.IsSelected = true;
                selectedItem.IsExpanded = true;
            }
            else
            {
                if (previousSelectedParentItem != null)
                    previousSelectedParentItem.Style = previousSelectedParentItemStyle;
                var parentItem = selectedItem.Parent as TreeViewItem;
                previousSelectedParentItem = parentItem;
                previousSelectedParentItemStyle = parentItem.Style;
                parentItem.Style = Resources["SelectedTreeViewItemParentStyle"] as Style;
            }
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

        private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedFileItem = FilesListBox.SelectedItem as ListBoxItem;
            if (selectedFileItem == null)
            {
                FilesListBox.SelectedIndex = 0;
                return;
            }
            Dispatcher.BeginInvoke((Action)LoadContent);
        }

        private void LoadContent()
        {
            var selectedFileItem = FilesListBox.SelectedItem as ListBoxItem;
            if (selectedFileItem.Tag == null)
            {
                if (containerWindow != null)
                    containerWindow.Close();
                var selectedThemeItem = ThemeComboBox.SelectedItem as ComboBoxItem;
                var theme = selectedThemeItem?.Tag as string;
                containerWindow = new WPF.CSharp.GanttChartDataGrid.MainFeatures.MainWindow(theme);
                ContentPresenter.Content = containerWindow.Content;
                ContentPresenter.Visibility = Visibility.Visible;
                ContentTextBox.Visibility = Visibility.Hidden;
                ContentTextBox.Text = null;
                ContentBrowser.Visibility = Visibility.Hidden;
                ContentBrowser.Source = null;
            }
            else
            {
                var resourceStreamInfo = Application.GetResourceStream(new Uri("/Samples.Resources/WPF-CSharp/GanttChartDataGrid/MainFeatures/" + selectedFileItem.Tag, UriKind.Relative));
                using (var resourceStreamReader = new StreamReader(resourceStreamInfo.Stream))
                {
                    ContentTextBox.Text = resourceStreamReader.ReadToEnd();
                }
                ContentTextBox.Visibility = Visibility.Visible;
                ContentPresenter.Visibility = Visibility.Hidden;
                ContentPresenter.Content = null;
                ContentBrowser.Visibility = Visibility.Hidden;
                ContentBrowser.Source = null;
            }
        }

        private Window containerWindow;

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (containerWindow != null)
                containerWindow.Close();
        }
    }
}
