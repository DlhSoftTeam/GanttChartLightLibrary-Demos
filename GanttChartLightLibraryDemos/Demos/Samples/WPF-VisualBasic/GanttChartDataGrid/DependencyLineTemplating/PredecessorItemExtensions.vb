Imports System.Text

Public NotInheritable Class PredecessorItemExtensions

    Private Sub New()
    End Sub

    Public Shared ReadOnly IsImportantProperty As DependencyProperty = DependencyProperty.RegisterAttached("IsImportant", GetType(Boolean), GetType(PredecessorItemExtensions))
    Public Shared Function GetIsImportant(obj As DependencyObject) As Boolean
        Return CBool(obj.GetValue(IsImportantProperty))
    End Function
    Public Shared Sub SetIsImportant(obj As DependencyObject, value As Boolean)
        obj.SetValue(IsImportantProperty, value)
    End Sub
End Class
