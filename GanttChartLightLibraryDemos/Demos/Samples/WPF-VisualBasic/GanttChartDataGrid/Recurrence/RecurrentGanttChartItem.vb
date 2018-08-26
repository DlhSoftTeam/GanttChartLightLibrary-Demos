Imports DlhSoft.Windows.Controls

Public Class RecurrentGanttChartItem
    Inherits GanttChartItem

    Private recurrenceTypeValue As RecurrenceType = RecurrenceType.Weekly
    Public Property RecurrenceType() As RecurrenceType
        Get
            Return recurrenceTypeValue
        End Get
        Set(value As RecurrenceType)
            recurrenceTypeValue = value
            OnPropertyChanged("RecurrenceType")
        End Set
    End Property

    Private occurrenceCountValue As Integer = 1
    Public Property OccurrenceCount() As Integer
        Get
            Return occurrenceCountValue
        End Get
        Set(value As Integer)
            occurrenceCountValue = Math.Max(1, value)
            OnPropertyChanged("OccurrenceCount")
        End Set
    End Property
End Class
