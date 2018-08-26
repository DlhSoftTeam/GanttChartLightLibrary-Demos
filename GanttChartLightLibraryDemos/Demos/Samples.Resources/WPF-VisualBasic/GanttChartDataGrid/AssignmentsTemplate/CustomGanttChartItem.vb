Imports DlhSoft.Windows.Controls
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Collections.Specialized

Friend Class CustomGanttChartItem
    Inherits GanttChartItem

    Private assignmentsIconSourceValue As ImageSource
    Public Property AssignmentsIconSource() As ImageSource
        Get
            Return assignmentsIconSourceValue
        End Get
        Set(value As ImageSource)
            assignmentsIconSourceValue = value
        End Set
    End Property
End Class
