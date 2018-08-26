Imports System.Net
Imports System.Windows.Ink
Imports System.Windows.Media.Animation
Imports System.Globalization
Imports System.Collections.ObjectModel
Imports DlhSoft.Windows.Controls
Imports System.ComponentModel
Imports System.Collections.Specialized

Public Class PredecessorItemsConverter
    Implements IValueConverter

    ' Retrieve a PredecessorItemCollection based on Predecessor data context.
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim predecessors As IEnumerable(Of Predecessor) = (TryCast(value, IEnumerable(Of Predecessor))).OrderBy(Function(p) p.Task.Index)
        Dim items As New PredecessorItemCollection()
        For Each pred As Predecessor In predecessors
            Dim item As PredecessorItem = New PredecessorItem With {.Tag = pred}
            SetPredecessorItem(item)
            items.Add(item)
        Next pred
        AddHandler items.CollectionChanged, AddressOf Items_CollectionChanged
        Return items
    End Function

    ' When the PredecessorItem collection changes, update data context accordingly.
    Private Sub Items_CollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
        Select Case e.Action
            Case NotifyCollectionChangedAction.Add
                Dim items As IEnumerable(Of PredecessorItem) = TryCast(sender, IEnumerable(Of PredecessorItem))
                For Each item As PredecessorItem In e.NewItems
                    Dim t As Task = TryCast(item.Item.Tag, Task)
                    Dim pred As Predecessor = New Predecessor With {.Task = t, .DependencyType = CInt(Fix(DependencyType.FinishStart))}
                    item.Tag = pred
                    SetPredecessorItem(item)
                    ContextTask.Predecessors.Add(pred)
                Next item
            Case NotifyCollectionChangedAction.Remove
                For Each item As PredecessorItem In e.OldItems
                    Dim pred As Predecessor = TryCast(item.Tag, Predecessor)
                    ContextTask.Predecessors.Remove(pred)
                Next item
        End Select
    End Sub

    Private Sub SetPredecessorItem(item As PredecessorItem)
        Dim pred As Predecessor = TryCast(item.Tag, Predecessor)
        BindingOperations.SetBinding(item, PredecessorItem.ItemProperty, New Binding("Task") With {.Source = pred, .Mode = BindingMode.TwoWay, .Converter = TaskItemConverter.GetInstance(GanttChartItems)})
        BindingOperations.SetBinding(item, PredecessorItem.DependencyTypeProperty, New Binding("DependencyType") With {.Source = pred, .Mode = BindingMode.TwoWay, .Converter = NumericDependencyTypeConverter.Instance})
    End Sub


    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotSupportedException()
    End Function

    Private privateContextTask As Task
    Public Property ContextTask() As Task
        Get
            Return privateContextTask
        End Get
        Private Set(ByVal value As Task)
            privateContextTask = value
        End Set
    End Property
    Private privateGanttChartItems As IEnumerable(Of GanttChartItem)
    Public Property GanttChartItems() As IEnumerable(Of GanttChartItem)
        Get
            Return privateGanttChartItems
        End Get
        Private Set(ByVal value As IEnumerable(Of GanttChartItem))
            privateGanttChartItems = value
        End Set
    End Property

    Public Shared Function GetInstance(contextTask As Task, ganttChartItems As IEnumerable(Of GanttChartItem)) As PredecessorItemsConverter
        Return New PredecessorItemsConverter With {.ContextTask = contextTask, .GanttChartItems = ganttChartItems}
    End Function
End Class
