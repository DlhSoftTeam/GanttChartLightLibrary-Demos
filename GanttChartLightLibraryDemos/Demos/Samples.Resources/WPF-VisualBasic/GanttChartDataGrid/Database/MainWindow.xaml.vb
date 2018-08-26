Imports System.Text
Imports DlhSoft.Windows.Controls
Imports System.Windows.Threading
Imports System.Collections.ObjectModel

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        ' Initialize data.
        LoadData()
    End Sub

    ' Hosts the data context used for data binding and change recording.
    Private context As DatabaseEntities

    Private Sub LoadMenuItem_Click(sender As Object, e As RoutedEventArgs)
        ' Clear and reinitialize data.
        ClearData()
        LoadData()
    End Sub
    Private Sub ClearData()
        ' Clear the data context.
        GanttChartDataGrid.ClearValue(DlhSoft.Windows.Controls.GanttChartDataGrid.ItemsProperty)
        GanttChartDataGrid.DataContext = Nothing

        If context IsNot Nothing Then
            ' Clear the existing database context.
            context.Dispose()
            context = Nothing
        End If
    End Sub
    Private Sub LoadData()
        ' Create a new database context.
        context = New DatabaseEntities()

        ' Set up the GanttChartDataGrid.DataContext for reference purposes.
        GanttChartDataGrid.DataContext = context

        ' Bind GanttChartDataGrid.Items to Tasks using TaskItemsConverter instance (see TaskItemsConverter source code).
        GanttChartDataGrid.SetBinding(DlhSoft.Windows.Controls.GanttChartDataGrid.ItemsProperty, New Binding("Tasks") With {.Converter = TaskItemsConverter.GetInstance(context)})

        ' Apply template to ensure we are able to compute overall project values and use scrolling features.
        GanttChartDataGrid.ApplyTemplate()

        ' Scroll the view to properly display the project.
        Dim overallStart As Date = GanttChartDataGrid.GetProjectStart()
        If overallStart >= Date.Today Then
            overallStart = Date.Today
        End If
        GanttChartDataGrid.SetTimelinePage(overallStart.AddDays(-1), overallStart.AddDays(60 + 2))
        GanttChartDataGrid.ScrollTo(Date.Today)
    End Sub
    Private Sub SaveMenuItem_Click(sender As Object, e As RoutedEventArgs)
        ' Save the data changes to the database.
        SaveData()
    End Sub
    Private Sub SaveData()
        ' Save changes on the database context.
        context.SaveChanges()
    End Sub

    ' Edit commands.
    Private Sub AddNewMenuItem_Click(sender As Object, e As RoutedEventArgs)
        Dim item As GanttChartItem = New GanttChartItem With {.Content = "New Task", .Start = Date.Today, .Finish = Date.Today.AddDays(1)}
        GanttChartDataGrid.Items.Add(item)
        GanttChartDataGrid.SelectedItem = item
    End Sub
    Private Sub DeleteMenuItem_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of GanttChartItem)()
        For Each item As GanttChartItem In GanttChartDataGrid.SelectedItems
            items.Add(item)
        Next item
        If items.Count <= 0 Then
            MessageBox.Show("Cannot delete the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        items.Reverse()
        For Each item As GanttChartItem In items
            If item.HasChildren Then
                MessageBox.Show(String.Format("Cannot delete {0} because it has child items; remove its child items first.", item), "Information", MessageBoxButton.OK)
                Continue For
            End If
            GanttChartDataGrid.Items.Remove(item)
        Next item
    End Sub
    Private Sub IncreaseIndentationMenuItem_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of GanttChartItem)()
        For Each item As GanttChartItem In GanttChartDataGrid.SelectedItems
            items.Add(item)
        Next item
        If items.Count <= 0 Then
            MessageBox.Show("Cannot increase indentation for the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        For Each item As GanttChartItem In items
            Dim index As Integer = GanttChartDataGrid.IndexOf(item)
            If index > 0 Then
                Dim previousItem As GanttChartItem = GanttChartDataGrid(index - 1)
                If item.Indentation <= previousItem.Indentation Then
                    ClearDependencies(item)
                    item.Indentation += 1
                Else
                    MessageBox.Show(String.Format("Cannot increase indentation for {0} because the previous item is its parent item; increase the indentation of its parent item first.", item), "Information", MessageBoxButton.OK)
                End If
            Else
                MessageBox.Show(String.Format("Cannot increase indentation for {0} because it is the first item; insert an item before this one first.", item), "Information", MessageBoxButton.OK)
            End If
        Next item
    End Sub
    Private Sub DecreaseIndentationMenuItem_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of GanttChartItem)()
        For Each item As GanttChartItem In GanttChartDataGrid.SelectedItems
            items.Add(item)
        Next item
        If items.Count <= 0 Then
            MessageBox.Show("Cannot decrease indentation for the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        items.Reverse()
        For Each item As GanttChartItem In items
            If item.Indentation > 0 Then
                Dim index As Integer = GanttChartDataGrid.IndexOf(item)
                Dim nextItem As GanttChartItem = If(index < GanttChartDataGrid.Items.Count - 1, GanttChartDataGrid(index + 1), Nothing)
                If nextItem Is Nothing OrElse item.Indentation >= nextItem.Indentation Then
                    ClearDependencies(item)
                    item.Indentation -= 1
                Else
                    MessageBox.Show(String.Format("Cannot increase indentation for {0} because the next item is one of its child items; decrease the indentation of its child items first.", item), "Information", MessageBoxButton.OK)
                End If
            Else
                MessageBox.Show(String.Format("Cannot decrease indentation for {0} because it is on the first indentation level, being a root item.", item), "Information", MessageBoxButton.OK)
            End If
        Next item
    End Sub
    Private Sub ClearDependencies(item As GanttChartItem)
        Dim hadDependencies As Boolean = False
        If item.Predecessors.Any() Then
            hadDependencies = True
        End If
        For Each predecessorItem As PredecessorItem In item.Predecessors.ToArray().Reverse()
            item.Predecessors.Remove(predecessorItem)
        Next predecessorItem
        Dim successorPredecessorItems As New List(Of PredecessorItem)(GanttChartDataGrid.GetSuccessorPredecessorItems(item))
        For Each successorPredecessorItem As PredecessorItem In successorPredecessorItems
            hadDependencies = True
            successorPredecessorItem.DependentItem.Predecessors.Remove(successorPredecessorItem)
        Next successorPredecessorItem
        If hadDependencies Then
            MessageBox.Show(String.Format("Dependencies are removed from {0}, in order to preserve hierarchical data consistency.", item), "Information", MessageBoxButton.OK)
        End If
    End Sub
End Class
