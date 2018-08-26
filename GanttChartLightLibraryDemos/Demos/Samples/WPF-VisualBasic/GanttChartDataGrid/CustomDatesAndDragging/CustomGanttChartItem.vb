Imports DlhSoft.Windows.Controls
Imports System.ComponentModel
Imports System.Text

Friend Class CustomGanttChartItem
    Inherits GanttChartItem
    Implements INotifyPropertyChanged

    Private customStartValue As Date = Date.Today
    Public Property CustomStart() As Date
        Get
            Return customStartValue
        End Get
        Set(value As Date)
            If value > CustomFinish Then
                CustomFinish = value
            End If
            customStartValue = value
            OnPropertyChanged(NameOf(CustomStart))
        End Set
    End Property
    Public ReadOnly Property ComputedCustomBarLeft() As Double
        Get
            Return GanttChartView.GetPosition(CustomStart) - GanttChartView.GetPosition(Start)
        End Get
    End Property

    Private customFinishValue As Date = Date.Today
    Public Property CustomFinish() As Date
        Get
            Return customFinishValue
        End Get
        Set(value As Date)
            If value < CustomStart Then
                CustomStart = value
            End If
            customFinishValue = value
            OnPropertyChanged(NameOf(CustomFinish))
        End Set
    End Property
    Public ReadOnly Property ComputedCustomBarWidth() As Double
        Get
            Return GanttChartView.GetPosition(CustomFinish) - GanttChartView.GetPosition(CustomStart)
        End Get
    End Property

    Protected Overrides Sub OnPropertyChanged(propertyName As String)
        MyBase.OnPropertyChanged(propertyName)
        Select Case propertyName
            Case NameOf(Start), NameOf(CustomStart)
                OnPropertyChanged(NameOf(ComputedCustomBarLeft))
        End Select
        Select Case propertyName
            Case NameOf(CustomStart), NameOf(CustomFinish)
                OnPropertyChanged(NameOf(ComputedCustomBarWidth))
        End Select
    End Sub

    ' Alternatively (required, if you have mouse wheel zooming enabled), refresh the user interface from a central handler.
    Protected Overrides Sub OnBarChanged()
        OnPropertyChanged(NameOf(ComputedCustomBarLeft))
        OnPropertyChanged(NameOf(ComputedCustomBarWidth))
        MyBase.OnBarChanged()
    End Sub
End Class
