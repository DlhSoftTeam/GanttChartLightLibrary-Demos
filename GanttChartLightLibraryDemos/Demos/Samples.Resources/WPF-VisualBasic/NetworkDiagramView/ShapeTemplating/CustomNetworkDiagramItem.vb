Imports DlhSoft.Windows.Controls.Pert
Imports System.ComponentModel

Friend Class CustomNetworkDiagramItem
    Inherits NetworkDiagramItem
    Implements INotifyPropertyChanged

    Private actualStartValue As Date = Date.Today
    Public Property ActualStart() As Date
        Get
            Return actualStartValue
        End Get
        Set(value As Date)
            If value > ActualFinish Then
                ActualFinish = value
            End If
            actualStartValue = value
            OnPropertyChanged("ActualStart")
        End Set
    End Property

    Private actualFinishValue As Date = Date.Today
    Public Property ActualFinish() As Date
        Get
            Return actualFinishValue
        End Get
        Set(value As Date)
            If value < ActualStart Then
                ActualStart = value
            End If
            actualFinishValue = value
            OnPropertyChanged("ActualFinish")
        End Set
    End Property
End Class
