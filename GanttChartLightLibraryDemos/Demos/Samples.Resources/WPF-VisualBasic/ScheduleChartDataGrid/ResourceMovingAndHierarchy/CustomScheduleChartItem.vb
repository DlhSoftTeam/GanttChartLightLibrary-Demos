Imports System.ComponentModel
Imports System.Windows.Media
Imports DlhSoft.Windows.Controls

Friend Class CustomScheduleChartItem
    Inherits ScheduleChartItem
    Implements INotifyPropertyChanged

    Private _image As ImageSource
    Public Property Image As ImageSource
        Get
            Return _image
        End Get
        Set(value As ImageSource)
            _image = value
            OnPropertyChanged(NameOf(Image))
        End Set
    End Property

    Private _dragOpacity As Double = 1.0
    Public Property DragOpacity As Double
        Get
            Return _dragOpacity
        End Get
        Set(value As Double)
            _dragOpacity = value
            OnPropertyChanged(NameOf(DragOpacity))
        End Set
    End Property

    Protected Overrides Sub OnPropertyChanged(propertyName As String)
        MyBase.OnPropertyChanged(propertyName)
    End Sub
End Class
