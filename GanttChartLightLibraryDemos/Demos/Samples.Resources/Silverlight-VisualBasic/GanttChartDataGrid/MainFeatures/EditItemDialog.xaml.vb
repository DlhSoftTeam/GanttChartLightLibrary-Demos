Public Class EditItemDialog
    Inherits ChildWindow

    Public Sub New()
        ' Copy the assignable resource collection reference from the main page before the InitializeComponent call.
        Resources.Add("AssignableResources", (TryCast(Application.Current.RootVisual, FrameworkElement)).Resources("AssignableResources"))

        InitializeComponent()
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

End Class
