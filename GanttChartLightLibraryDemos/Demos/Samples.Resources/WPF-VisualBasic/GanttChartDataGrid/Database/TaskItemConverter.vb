Imports System.Net
Imports System.Windows.Ink
Imports System.Windows.Media.Animation
Imports System.Globalization
Imports System.Collections.ObjectModel
Imports DlhSoft.Windows.Controls
Imports System.ComponentModel

Public Class TaskItemConverter
    Implements IValueConverter

    ' Retrieve a GanttChartItem based on Task data context.
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim t As Task = TryCast(value, Task)
        Return (
            From i In GanttChartItems
            Where i.Tag Is t
            Select i).SingleOrDefault()
    End Function

    ' Retrieve a Task data item based on a GanttChartItem.
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Dim item As GanttChartItem = TryCast(value, GanttChartItem)
        Return TryCast(item.Tag, Task)
    End Function

    Private privateGanttChartItems As IEnumerable(Of GanttChartItem)
    Public Property GanttChartItems() As IEnumerable(Of GanttChartItem)
        Get
            Return privateGanttChartItems
        End Get
        Private Set(ByVal value As IEnumerable(Of GanttChartItem))
            privateGanttChartItems = value
        End Set
    End Property

    Public Shared Function GetInstance(ganttChartItems As IEnumerable(Of GanttChartItem)) As TaskItemConverter
        Return New TaskItemConverter With {.GanttChartItems = ganttChartItems}
    End Function
End Class
