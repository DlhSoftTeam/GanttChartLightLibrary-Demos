Imports DlhSoft.Windows.Controls
Imports System.ComponentModel

Friend Class CustomGanttChartItem
    Inherits GanttChartItem
    Implements INotifyPropertyChanged

    Private extraCostsValue As Decimal
    Public Property ExtraCosts() As Decimal
        Get
            Return extraCostsValue
        End Get
        Set(value As Decimal)
            If value < 0 Then
                value = 0
            End If

            extraCostsValue = value
            OnPropertyChanged("ExtraCosts")

            If GanttChartView Is Nothing Then
                Return
            End If
            Dim parent As CustomGanttChartItem = TryCast(GanttChartView.GetParent(Me), CustomGanttChartItem)
            If parent Is Nothing Then
                Return
            End If
            parent.ExtraCosts = GanttChartView.GetChildren(parent).Sum(Function(item) (TryCast(item, CustomGanttChartItem)).ExtraCosts)
        End Set
    End Property
End Class
