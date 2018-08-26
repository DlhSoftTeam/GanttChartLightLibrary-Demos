Imports System.ComponentModel
Imports DlhSoft.Windows.Controls

Friend Class Interruption
    Implements INotifyPropertyChanged

    Private privateItem As CustomGanttChartItem
    Public Property Item() As CustomGanttChartItem
        Get
            Return privateItem
        End Get
        Friend Set(ByVal value As CustomGanttChartItem)
            privateItem = value
        End Set
    End Property
    Public ReadOnly Property GanttChartView() As IGanttChartView
        Get
            Return If(Item IsNot Nothing, Item.GanttChartView, Nothing)
        End Get
    End Property

    Private startValue As Date
    Public Property Start() As Date
        Get
            Return startValue
        End Get
        Set(value As Date)
            startValue = value
            OnPropertyChanged("Start")
        End Set
    End Property
    Public ReadOnly Property ComputedLeft() As Double
        Get
            Return GanttChartView.GetPosition(Start) - GanttChartView.GetPosition(Item.Start)
        End Get
    End Property

    Private finishValue As Date
    Public Property Finish() As Date
        Get
            Return finishValue
        End Get
        Set(value As Date)
            finishValue = value
            OnPropertyChanged("Finish")
        End Set
    End Property
    Public ReadOnly Property ComputedWidth() As Double
        Get
            Return GanttChartView.GetPosition(Finish) - GanttChartView.GetPosition(Item.Start) - ComputedLeft
        End Get
    End Property

    Protected Friend Overridable Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        Select Case propertyName
            Case "Start"
                OnPropertyChanged("ComputedLeft")
        End Select
        Select Case propertyName
            Case "Start", "Finish"
                OnPropertyChanged("ComputedWidth")
        End Select
    End Sub
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
