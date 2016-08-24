Public Class EditItemDialog

    Public Sub New()
        InitializeComponent()
    End Sub

    Private assignableResourcesValue As IList(Of String)
    Public Property AssignableResources() As IList(Of String)
        Get
            Return assignableResourcesValue
        End Get
        Friend Set(ByVal value As IList(Of String))
            assignableResourcesValue = value
        End Set
    End Property

    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

End Class
