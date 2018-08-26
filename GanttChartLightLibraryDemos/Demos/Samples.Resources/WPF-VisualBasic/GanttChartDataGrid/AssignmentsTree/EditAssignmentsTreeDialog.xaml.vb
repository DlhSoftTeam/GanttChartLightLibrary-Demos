Imports DlhSoft.Windows.Controls
Imports System.Collections.ObjectModel
Imports System.Text

''' <summary>
''' Interaction logic for EditAssignmentsTreeDialog.xaml
''' </summary>
Partial Public Class EditAssignmentsTreeDialog
    Inherits Window

    Public Sub New()
        InitializeComponent()

        AssignmentsDataTreeGrid.Items = New ObservableCollection(Of DataTreeGridItem) From {
                New Resource With {.Content = "Department A", .Allocation = 100},
                New Resource With {.Content = "Resource 1", .Allocation = 100, .Role = "Role 1", .Indentation = 1},
                New Resource With {.Content = "Resource 2", .Allocation = 100, .Role = "Role 2", .Indentation = 1},
                New Resource With {.Content = "Department B", .Allocation = 100},
                New Resource With {.Content = "Resource 3", .Allocation = 100, .Role = "Role 1", .Indentation = 1},
                New Resource With {.Content = "Department B1", .Allocation = 100, .Indentation = 1},
                New Resource With {.Content = "Resource 4", .Allocation = 100, .Role = "Role 3", .Indentation = 2},
                New Resource With {.Content = "Resource 5", .Allocation = 100, .Role = "Role 1", .Indentation = 2}
            }
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Dim item = TryCast(DataContext, GanttChartItem)
        SelectAssignments(TryCast(item.AssignmentsContent, String))
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs)
        Dim item = TryCast(DataContext, GanttChartItem)
        item.AssignmentsContent = GetSelectedAssignmentsString()
        Close()
    End Sub

    Private Sub SelectAssignments(assignmentsString As String)
        If assignmentsString Is Nothing OrElse assignmentsString.Trim() = String.Empty Then
            Return
        End If

        For Each assignment As String In assignmentsString.Split(","c)
            Dim resourceContent As String = assignment.Trim()
            If resourceContent.StartsWith("{") AndAlso resourceContent.EndsWith("}") Then
                resourceContent = resourceContent.Substring(1, resourceContent.Length - 2)
            End If

            Dim index As Integer = resourceContent.IndexOf("[")
            Dim allocationContent As String = String.Empty
            If index > 0 Then
                Dim percentIndex As Integer = resourceContent.IndexOf("%")
                If percentIndex < 0 Then
                    percentIndex = resourceContent.IndexOf("]")
                End If
                If percentIndex < 0 Then
                    percentIndex = resourceContent.Length
                End If
                Dim length As Integer = percentIndex - index - 1
                allocationContent = resourceContent.Substring(index + 1, length)
                resourceContent = resourceContent.Substring(0, index).Trim()
            End If

            Dim resource As Resource = TryCast(AssignmentsDataTreeGrid.Items.Where(Function(r) TryCast(r.Content, String) = resourceContent).FirstOrDefault(), Resource)

            If resource Is Nothing Then
                resource = New Resource() With {.Content = resourceContent}
                AssignmentsDataTreeGrid.Items.Add(resource)
            End If

            Dim allocation As Double
            If Not Double.TryParse(allocationContent, allocation) Then
                allocation = 100
            End If
            resource.Allocation = allocation

            If Not AssignmentsDataTreeGrid.SelectedItems.Contains(resource) Then
                AssignmentsDataTreeGrid.SelectedItems.Add(resource)
            End If
        Next assignment
    End Sub

    Private Function GetSelectedAssignmentsString() As String
        Dim assignments As String = ""
        Dim i As Integer = 0
        While i < AssignmentsDataTreeGrid.GetSelectedItems().Count()
            Dim selectedAssignment = TryCast(AssignmentsDataTreeGrid.GetSelectedItems().ToArray()(i), Resource)

            If AssignmentsDataTreeGrid.SelectedItems.Contains(selectedAssignment.Parent) Then
                i = i + 1
                Continue While
            End If

            If assignments.Length > 0 Then
                assignments &= ", "
            End If
            If selectedAssignment.HasChildren Then
                ' Determine whether all children are selected for the group.
                Dim areAllChildrenSelected = True
                For Each child As Resource In selectedAssignment.AllChildren
                    If Not AssignmentsDataTreeGrid.GetSelectedItems().Contains(child) Then
                        areAllChildrenSelected = False
                        Exit For
                    End If
                Next child
                If areAllChildrenSelected Then
                    assignments &= "{" & selectedAssignment.Content.ToString() & "}"
                    For Each child As Resource In selectedAssignment.AllChildren
                        AssignmentsDataTreeGrid.SelectedItems.Remove(child)
                    Next child
                End If
            Else
                ' Handle individual resources.
                If selectedAssignment.Content.ToString().Trim().Length > 0 Then
                    assignments &= selectedAssignment.Content.ToString()
                End If
            End If

            ' Append allocation percent.
            If selectedAssignment.Allocation <> 100 Then
                assignments &= " [" & selectedAssignment.Allocation & "%]"
            End If
            i = i + 1
        End While

        Return assignments
    End Function

    Private Sub AssignmentsDataTreeGrid_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        ' We use a flag to avoid recurrent event handler calls.
        If isDuringInternalAssignmentsDataTreeGridSelection Then
            Return
        End If
        isDuringInternalAssignmentsDataTreeGridSelection = True

        Dim selectedItems = AssignmentsDataTreeGrid.SelectedItems
        Dim addedToSelection = e.AddedItems.Cast(Of Resource)().ToList()
        Dim removedFromSelection = e.RemovedItems.Cast(Of Resource)().ToList()

        ' Wait for the selection changes to actually occur before we update it, if needed.
        Dispatcher.BeginInvoke(CType(Sub()
                                         ' When items are selected, also select their children.
                                         ' When items are unselected, also unselect their children, and ensure parents are no longer selected either.
                                         ' Note that when clicking on any item (without holding Control key), items may get unselected automatically,
                                         ' due to the way DataGrid behaves. We need to make sure we do not propagate the changes to children,
                                         ' and also add the inadvertently unselected items back, as appropriate for a hierarchy.
                                         If addedToSelection IsNot Nothing Then
                                             For Each selected In addedToSelection
                                                 If selected.HasChildren Then
                                                     For Each child In selected.AllChildren
                                                         If Not selectedItems.Contains(child) Then
                                                             selectedItems.Add(child)
                                                         End If
                                                     Next child
                                                 End If
                                             Next selected
                                         End If
                                         If removedFromSelection IsNot Nothing Then
                                             Dim remainingSelectedItems = selectedItems.Cast(Of Resource)().ToArray()
                                             Dim remainingSelectedChildren = remainingSelectedItems.SelectMany(Function(i) i.AllChildren.Cast(Of Resource)()).ToArray()
                                             Dim remainingSelectedItemsWithChildren = remainingSelectedItems.Union(remainingSelectedChildren).ToArray()
                                             removedFromSelection.RemoveAll(Function(i) remainingSelectedChildren.Contains(i))
                                             For Each selected In removedFromSelection
                                                 If selected.HasChildren Then
                                                     For Each child In selected.AllChildren
                                                         If selectedItems.Contains(child) Then
                                                             selectedItems.Remove(child)
                                                         End If
                                                     Next child
                                                 End If
                                                 For Each parent_Renamed In selected.AllParents
                                                     If selectedItems.Contains(parent_Renamed) Then
                                                         selectedItems.Remove(parent_Renamed)
                                                     End If
                                                 Next parent_Renamed
                                             Next selected
                                             For Each selected In remainingSelectedItemsWithChildren
                                                 If Not selectedItems.Contains(selected) Then
                                                     selectedItems.Add(selected)
                                                 End If
                                             Next selected
                                         End If
                                         isDuringInternalAssignmentsDataTreeGridSelection = False
                                     End Sub, Action))
    End Sub
    Private isDuringInternalAssignmentsDataTreeGridSelection As Boolean
End Class
