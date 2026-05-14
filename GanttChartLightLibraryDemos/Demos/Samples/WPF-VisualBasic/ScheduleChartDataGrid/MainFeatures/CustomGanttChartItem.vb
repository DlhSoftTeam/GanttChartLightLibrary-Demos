Imports System.ComponentModel
Imports System.Windows.Media
Imports DlhSoft.Windows.Controls

Friend Class CustomGanttChartItem
    Inherits GanttChartItem
    Implements INotifyPropertyChanged

    Private _icon As ImageSource
    Public Property Icon As ImageSource
        Get
            Return _icon
        End Get
        Set(value As ImageSource)
            _icon = value
            OnPropertyChanged("Icon")
        End Set
    End Property

    Private _note As String
    Public Property Note As String
        Get
            Return _note
        End Get
        Set(value As String)
            _note = value
            OnPropertyChanged("Note")
        End Set
    End Property

    Protected Overrides Sub OnPropertyChanged(propertyName As String)
        MyBase.OnPropertyChanged(propertyName)
    End Sub
End Class
