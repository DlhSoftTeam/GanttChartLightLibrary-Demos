Imports DlhSoft.Windows.Controls
Imports System.ComponentModel
Imports System.Text

Friend Class StatusGanttChartItem
    Inherits GanttChartItem

    Public ReadOnly Property Status() As String
        Get
            If HasChildren OrElse IsMilestone Then
                Return String.Empty
            End If
            If CompletedFinish >= Finish Then
                Return "Completed"
            End If
            Dim now = Date.Now
            If CompletedFinish < now Then
                Return "Behind schedule"
            End If
            If CompletedFinish > Start Then
                Return "In progress"
            End If
            Return "To Do"
        End Get
    End Property

    Public ReadOnly Property StatusColor() As SolidColorBrush
        Get
            Select Case Status
                Case "Completed"
                    Return Brushes.Green
                Case "To Do"
                    Return Brushes.Gray
                Case "Behind schedule"
                    Return Brushes.Red
                Case "In progress"
                    Return Brushes.Orange
                Case Else
                    Return Brushes.Transparent
            End Select
        End Get
    End Property

    Protected Overrides Sub OnPropertyChanged(propertyName As String)
        MyBase.OnPropertyChanged(propertyName)

        Dim statusAffectingPropertyNames = {NameOf(Start), NameOf(CompletedFinish), NameOf(Finish), NameOf(IsMilestone), NameOf(HasChildren)}
        If statusAffectingPropertyNames.Contains(propertyName) Then
            OnPropertyChanged(NameOf(Status))
            OnPropertyChanged(NameOf(StatusColor))
        End If
    End Sub
End Class