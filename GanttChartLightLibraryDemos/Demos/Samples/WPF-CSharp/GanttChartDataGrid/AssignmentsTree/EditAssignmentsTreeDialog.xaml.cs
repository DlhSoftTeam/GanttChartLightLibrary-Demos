using DlhSoft.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Demos.WPF.CSharp.GanttChartDataGrid.AssignmentsTree
{
    /// <summary>
    /// Interaction logic for EditAssignmentsTreeDialog.xaml
    /// </summary>
    public partial class EditAssignmentsTreeDialog : Window
    {
        public EditAssignmentsTreeDialog()
        {
            InitializeComponent();

            AssignmentsDataTreeGrid.Items = new ObservableCollection<DataTreeGridItem> {
                new Resource { Content = "Department A", Allocation = 100 },
                new Resource { Content = "Resource 1", Allocation = 100, Role = "Role 1", Indentation = 1 },
                new Resource { Content = "Resource 2", Allocation = 100, Role = "Role 2", Indentation = 1 },
                new Resource { Content = "Department B", Allocation = 100 },
                new Resource { Content = "Resource 3", Allocation = 100, Role = "Role 1", Indentation = 1 },
                new Resource { Content = "Department B1", Allocation = 100, Indentation = 1 },
                new Resource { Content = "Resource 4", Allocation = 100, Role = "Role 3", Indentation = 2 },
                new Resource { Content = "Resource 5", Allocation = 100, Role = "Role 1", Indentation = 2 } };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var item = DataContext as GanttChartItem;
            SelectAssignments(item.AssignmentsContent as string);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var item = DataContext as GanttChartItem;
            item.AssignmentsContent = GetSelectedAssignmentsString();
            Close();
        }

        private void SelectAssignments(string assignmentsString)
        {
            if (assignmentsString == null || assignmentsString.Trim() == string.Empty)
                return;

            foreach (string assignment in assignmentsString.Split(','))
            {
                string resourceContent = assignment.Trim();
                if (resourceContent.StartsWith("{") && resourceContent.EndsWith("}"))
                    resourceContent = resourceContent.Substring(1, resourceContent.Length - 2);

                int index = resourceContent.IndexOf("[");
                string allocationContent = string.Empty;
                if (index > 0)
                {
                    int percentIndex = resourceContent.IndexOf("%");
                    if (percentIndex < 0)
                        percentIndex = resourceContent.IndexOf("]");
                    if (percentIndex < 0)
                        percentIndex = resourceContent.Length;
                    int length = percentIndex - index - 1;
                    allocationContent = resourceContent.Substring(index + 1, length);
                    resourceContent = resourceContent.Substring(0, index).Trim();
                }

                Resource resource = AssignmentsDataTreeGrid.Items
                    .Where(r => r.Content as string == resourceContent)
                    .FirstOrDefault() as Resource;

                if (resource == null)
                {
                    resource = new Resource() { Content = resourceContent };
                    AssignmentsDataTreeGrid.Items.Add(resource);
                }

                double allocation;
                if (!double.TryParse(allocationContent, out allocation))
                    allocation = 100;
                resource.Allocation = allocation;

                if (!AssignmentsDataTreeGrid.SelectedItems.Contains(resource))
                    AssignmentsDataTreeGrid.SelectedItems.Add(resource);
            }
        }

        private string GetSelectedAssignmentsString()
        {
            string assignments = "";
            for (int i = 0; i < AssignmentsDataTreeGrid.GetSelectedItems().Count(); i++)
            {
                var selectedAssignment = AssignmentsDataTreeGrid.GetSelectedItems().ToArray()[i] as Resource;

                if (AssignmentsDataTreeGrid.SelectedItems.Contains(selectedAssignment.Parent))
                    continue;

                if (assignments.Length > 0)
                    assignments += ", ";
                if (selectedAssignment.HasChildren)
                {
                    // Determine whether all children are selected for the group.
                    var areAllChildrenSelected = true;
                    foreach (Resource child in selectedAssignment.AllChildren)
                    {
                        if (!AssignmentsDataTreeGrid.GetSelectedItems().Contains(child))
                        {
                            areAllChildrenSelected = false;
                            break;
                        }
                    }
                    if (areAllChildrenSelected)
                    {
                        assignments += "{" + selectedAssignment.Content + "}";
                        foreach (Resource child in selectedAssignment.AllChildren)
                        {
                            AssignmentsDataTreeGrid.SelectedItems.Remove(child);
                        }
                    }
                }
                else
                {
                    // Handle individual resources.
                    if (selectedAssignment.Content.ToString().Trim().Length > 0)
                        assignments += selectedAssignment.Content;
                }

                // Append allocation percent.
                if (selectedAssignment.Allocation != 100)
                    assignments += " [" + selectedAssignment.Allocation + "%]";
            }
            return assignments;
        }

        private void AssignmentsDataTreeGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // We use a flag to avoid recurrent event handler calls.
            if (isDuringInternalAssignmentsDataTreeGridSelection)
                return;
            isDuringInternalAssignmentsDataTreeGridSelection = true;

            var selectedItems = AssignmentsDataTreeGrid.SelectedItems;
            var addedToSelection = e.AddedItems.Cast<Resource>().ToList();
            var removedFromSelection = e.RemovedItems.Cast<Resource>().ToList();

            // Wait for the selection changes to actually occur before we update it, if needed.
            Dispatcher.BeginInvoke((Action)delegate
            {
                // When items are selected, also select their children.
                if (addedToSelection != null)
                {
                    foreach (var selected in addedToSelection)
                    {
                        if (selected.HasChildren)
                        {
                            foreach (var child in selected.AllChildren)
                            {
                                if (!selectedItems.Contains(child))
                                    selectedItems.Add(child);
                            }
                        }
                    }
                }

                // When items are unselected, also unselect their children, and ensure parents are no longer selected either.
                if (removedFromSelection != null)
                {
                    // Note that when clicking on any item (without holding Control key), items may get unselected automatically,
                    // due to the way DataGrid behaves. We need to make sure we do not propagate the changes to children,
                    // and also add the inadvertently unselected items back, as appropriate for a hierarchy.
                    var remainingSelectedItems = selectedItems.Cast<Resource>().ToArray();
                    var remainingSelectedChildren = remainingSelectedItems.SelectMany(i => i.AllChildren.Cast<Resource>()).ToArray();
                    var remainingSelectedItemsWithChildren = remainingSelectedItems.Union(remainingSelectedChildren).ToArray();
                    removedFromSelection.RemoveAll(i => remainingSelectedChildren.Contains(i));

                    foreach (var selected in removedFromSelection)
                    {
                        if (selected.HasChildren)
                        {
                            foreach (var child in selected.AllChildren)
                            {
                                if (selectedItems.Contains(child))
                                    selectedItems.Remove(child);
                            }
                        }
                        foreach (var parent in selected.AllParents)
                        {
                            if (selectedItems.Contains(parent))
                                selectedItems.Remove(parent);
                        }
                    }

                    foreach (var selected in remainingSelectedItemsWithChildren)
                    {
                        if (!selectedItems.Contains(selected))
                            selectedItems.Add(selected);
                    }
                }

                isDuringInternalAssignmentsDataTreeGridSelection = false;
            });
        }
        private bool isDuringInternalAssignmentsDataTreeGridSelection;
    }
}
