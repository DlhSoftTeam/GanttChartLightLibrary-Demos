Imports System.Net
Imports System.Windows.Ink
Imports System.Windows.Media.Animation
Imports System.Globalization
Imports System.Collections.ObjectModel
Imports DlhSoft.Windows.Controls
Imports System.ComponentModel
Imports System.Collections.Specialized

Public Class TaskItemsConverter
    Implements IValueConverter

    ' Retrieve a collection of GanttChartItem based on Task data context.
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim tasks As IEnumerable(Of Task) = (TryCast(value, IEnumerable(Of Task))).OrderBy(Function(t) t.Index)
        Dim items As New ObservableCollection(Of GanttChartItem)()
        For Each t As Task In tasks
            Dim item As GanttChartItem = New GanttChartItem With {.Tag = t}
            SetGanttChartItem(item)
            items.Add(item)
        Next t
        For Each item As GanttChartItem In items
            SetGanttChartItemPredecessors(item, items)
        Next item
        AddHandler items.CollectionChanged, AddressOf Items_CollectionChanged
        Return items
    End Function

    ' When the GanttChartItem collection changes, update data context accordingly.
    Private Sub Items_CollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
        Select Case e.Action
            Case NotifyCollectionChangedAction.Add
                Dim items As IEnumerable(Of GanttChartItem) = TryCast(sender, IEnumerable(Of GanttChartItem))
                For Each item As GanttChartItem In e.NewItems
                    Dim t As Task = New Task With {.Name = If(TryCast(item.Content, String), "New Task"), .Start = item.Start, .Finish = item.Finish, .Completion = item.Start, .Assignments = String.Empty, .Index = GetNextTaskIndex(items)}
                    item.Tag = t
                    SetGanttChartItem(item)
                    SetGanttChartItemPredecessors(item, items)
                    Context.Tasks.AddObject(t)
                Next item
            Case NotifyCollectionChangedAction.Remove
                For Each item As GanttChartItem In e.OldItems
                    Dim t As Task = TryCast(item.Tag, Task)
                    Context.Tasks.DeleteObject(t)
                Next item
        End Select
    End Sub

    Private Sub SetGanttChartItem(item As GanttChartItem)
        Dim t As Task = TryCast(item.Tag, Task)
        BindingOperations.SetBinding(item, GanttChartItem.IndentationProperty, New Binding("Indentation") With {.Source = t, .Mode = BindingMode.TwoWay})
        BindingOperations.SetBinding(item, GanttChartItem.ContentProperty, New Binding("Name") With {.Source = t, .Mode = BindingMode.TwoWay})
        BindingOperations.SetBinding(item, GanttChartItem.StartProperty, New Binding("Start") With {.Source = t, .Mode = BindingMode.TwoWay})
        BindingOperations.SetBinding(item, GanttChartItem.FinishProperty, New Binding("Finish") With {.Source = t, .Mode = BindingMode.TwoWay})
        BindingOperations.SetBinding(item, GanttChartItem.CompletedFinishProperty, New Binding("Completion") With {.Source = t, .Mode = BindingMode.TwoWay})
        BindingOperations.SetBinding(item, GanttChartItem.IsMilestoneProperty, New Binding("IsMilestone") With {.Source = t, .Mode = BindingMode.TwoWay})
        BindingOperations.SetBinding(item, GanttChartItem.AssignmentsContentProperty, New Binding("Assignments") With {.Source = t, .Mode = BindingMode.TwoWay})
    End Sub

    Private Sub SetGanttChartItemPredecessors(item As GanttChartItem, items As IEnumerable(Of GanttChartItem))
        Dim t As Task = TryCast(item.Tag, Task)
        BindingOperations.SetBinding(item, GanttChartItem.PredecessorsProperty, New Binding("Predecessors") With {.Source = t, .Converter = PredecessorItemsConverter.GetInstance(t, items)})
    End Sub

    Private Function GetNextTaskIndex(items As IEnumerable(Of GanttChartItem)) As Integer
        items = items.Where(Function(i) i.Tag IsNot Nothing)
        If Not items.Any() Then
            Return 0
        End If
        Return (
            From i In items
            Let t = TryCast(i.Tag, Task)
            Select t.Index).Max() + 1
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotSupportedException()
    End Function

    Private privateContext As DatabaseEntities
    Public Property Context() As DatabaseEntities
        Get
            Return privateContext
        End Get
        Private Set(ByVal value As DatabaseEntities)
            privateContext = value
        End Set
    End Property

    Public Shared Function GetInstance(context As DatabaseEntities) As TaskItemsConverter
        Return New TaskItemsConverter With {.Context = context}
    End Function
End Class
