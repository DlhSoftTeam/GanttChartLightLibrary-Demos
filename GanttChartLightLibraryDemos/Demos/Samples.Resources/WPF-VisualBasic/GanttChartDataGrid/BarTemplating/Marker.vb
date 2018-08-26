Imports System.ComponentModel
Imports DlhSoft.Windows.Controls

Friend Class Marker
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

    Private dateTimeValue As Date
    Public Property DateValue() As Date
        Get
            Return dateTimeValue
        End Get
        Set(value As Date)
            dateTimeValue = value
            OnPropertyChanged("DateTime")
        End Set
    End Property
    Public ReadOnly Property ComputedLeft() As Double
        Get
            Return GanttChartView.GetPosition(DateValue) - GanttChartView.GetPosition(Item.Start)
        End Get
    End Property

    Private iconValue As ImageSource
    Public Property Icon() As ImageSource
        Get
            Return iconValue
        End Get
        Set(value As ImageSource)
            iconValue = value
            OnPropertyChanged("Icon")
        End Set
    End Property

    'INSTANT VB NOTE: The variable note was renamed since Visual Basic does not allow variables and other class members to have the same name:
    Private noteValue As String
    Public Property Note() As String
        Get
            Return noteValue
        End Get
        Set(value As String)
            noteValue = value
            OnPropertyChanged("Note")
        End Set
    End Property

    Protected Friend Overridable Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        Select Case propertyName
            Case "DateTime"
                OnPropertyChanged("ComputedLeft")
        End Select
    End Sub
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
