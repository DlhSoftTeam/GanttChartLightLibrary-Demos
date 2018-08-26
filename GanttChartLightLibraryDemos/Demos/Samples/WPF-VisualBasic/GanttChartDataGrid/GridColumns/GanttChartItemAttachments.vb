Imports System.Text

Friend NotInheritable Class GanttChartItemAttachments

    Private Sub New()
    End Sub

    Public Shared ReadOnly MyValue5Property As DependencyProperty = DependencyProperty.RegisterAttached("MyValue5", GetType(String), GetType(GanttChartItemAttachments))
    Public Shared Function GetMyValue5(obj As DependencyObject) As String
        Return CStr(obj.GetValue(MyValue5Property))
    End Function
    Public Shared Sub SetMyValue5(obj As DependencyObject, value As String)
        obj.SetValue(MyValue5Property, value)
    End Sub

    Public Shared ReadOnly MyValue6Property As DependencyProperty = DependencyProperty.RegisterAttached("MyValue6", GetType(String), GetType(GanttChartItemAttachments))
    Public Shared Function GetMyValue6(obj As DependencyObject) As String
        Return CStr(obj.GetValue(MyValue6Property))
    End Function
    Public Shared Sub SetMyValue6(obj As DependencyObject, value As String)
        obj.SetValue(MyValue6Property, value)
    End Sub
End Class
