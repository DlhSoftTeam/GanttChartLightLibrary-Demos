Imports DlhSoft.Windows.Controls
Imports System.ComponentModel
Imports System.Text

Friend Class CustomGanttChartItem
    Inherits GanttChartItem
    Implements INotifyPropertyChanged

    Public Sub New()
        ' Alternatively, attached properties (e.g. MyValue3 and MyValue4) can be provided (e.g. by GanttChartItemAttachments).
        ' Optionally, also associate a tag object for defaults of other properties that may usually come from a different system (e.g. MyValue5, and MyValue6).
        Tag = New CustomDataObject()
    End Sub

    Private myValue1Value As Integer
    Public Property MyValue1() As Integer
        Get
            Return myValue1Value
        End Get
        Set(value As Integer)
            If value < 0 Then
                value = 0
            End If
            myValue1Value = value
            OnPropertyChanged("MyValue1")
        End Set
    End Property

    Private myValue2Value As String
    Public Property MyValue2() As String
        Get
            Return myValue2Value
        End Get
        Set(value As String)
            myValue2Value = value
            OnPropertyChanged("MyValue2")
        End Set
    End Property
End Class
