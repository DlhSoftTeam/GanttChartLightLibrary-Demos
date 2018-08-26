Imports DlhSoft.Windows.Controls.Pert

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim item0 As CustomNetworkDiagramItem = TryCast(NetworkDiagramView.Items(0), CustomNetworkDiagramItem)
        item0.EarlyStart = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item0.EarlyFinish = item0.EarlyStart
        item0.LateStart = item0.EarlyStart
        item0.LateFinish = item0.LateStart
        item0.ActualStart = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item0.ActualFinish = item0.ActualStart
        item0.AssignmentsContent = "N/A"

        Dim item1 As CustomNetworkDiagramItem = TryCast(NetworkDiagramView.Items(1), CustomNetworkDiagramItem)
        item1.Effort = TimeSpan.Parse("08:00:00")
        item1.EarlyStart = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item1.EarlyFinish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        item1.LateStart = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item1.LateFinish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        item1.ActualStart = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item1.ActualFinish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        item1.Slack = TimeSpan.Zero
        item1.AssignmentsContent = "Resource 1"
        item1.Predecessors.Add(New NetworkPredecessorItem With {.Item = item0})

        Dim item2 As CustomNetworkDiagramItem = TryCast(NetworkDiagramView.Items(2), CustomNetworkDiagramItem)
        item2.Effort = TimeSpan.Parse("04:00:00")
        item2.EarlyStart = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item2.EarlyFinish = Date.Today.Add(TimeSpan.Parse("12:00:00"))
        item2.LateStart = Date.Today.Add(TimeSpan.Parse("12:00:00"))
        item2.LateFinish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        item2.ActualStart = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item2.ActualFinish = Date.Today.Add(TimeSpan.Parse("12:00:00"))
        item2.Slack = TimeSpan.Parse("04:00:00")
        item2.AssignmentsContent = "Resource 2"
        item2.Predecessors.Add(New NetworkPredecessorItem With {.Item = item0})

        Dim item3 As CustomNetworkDiagramItem = TryCast(NetworkDiagramView.Items(3), CustomNetworkDiagramItem)
        item3.Effort = TimeSpan.Parse("16:00:00")
        item3.EarlyStart = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item3.EarlyFinish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item3.LateStart = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item3.LateFinish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item3.ActualStart = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item3.ActualFinish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item3.Slack = TimeSpan.Zero
        item3.AssignmentsContent = "Resource 1, Resource 2"
        item3.Predecessors.Add(New NetworkPredecessorItem With {.Item = item1})
        item3.Predecessors.Add(New NetworkPredecessorItem With {.Item = item2})

        Dim item4 As CustomNetworkDiagramItem = TryCast(NetworkDiagramView.Items(4), CustomNetworkDiagramItem)
        item4.Effort = TimeSpan.Parse("04:00:00")
        item4.EarlyStart = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item4.EarlyFinish = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"))
        item4.LateStart = Date.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"))
        item4.LateFinish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item4.ActualStart = Date.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"))
        item4.ActualFinish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item4.Slack = TimeSpan.Parse("12:00:00")
        item4.AssignmentsContent = "Resource 2"
        item4.Predecessors.Add(New NetworkPredecessorItem With {.Item = item1})

        Dim item5 As CustomNetworkDiagramItem = TryCast(NetworkDiagramView.Items(5), CustomNetworkDiagramItem)
        item5.EarlyStart = Date.Today.AddDays(3).Add(TimeSpan.Parse("08:00:00"))
        item5.EarlyFinish = Date.Today.AddDays(3).Add(TimeSpan.Parse("08:00:00"))
        item5.LateStart = Date.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"))
        item5.LateFinish = Date.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"))
        item5.ActualStart = Date.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00"))
        item5.ActualFinish = Date.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00"))
        item5.Slack = TimeSpan.Parse("12:00:00")
        item5.AssignmentsContent = "Resource 2"
        item5.Predecessors.Add(New NetworkPredecessorItem With {.Item = item3})
        item5.Predecessors.Add(New NetworkPredecessorItem With {.Item = item4})

        Dim item6 As CustomNetworkDiagramItem = TryCast(NetworkDiagramView.Items(6), CustomNetworkDiagramItem)
        item6.Effort = TimeSpan.Parse("2.00:00:00")
        item6.EarlyStart = Date.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"))
        item6.EarlyFinish = Date.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"))
        item6.LateStart = Date.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"))
        item6.LateFinish = Date.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"))
        item6.ActualStart = Date.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"))
        item6.ActualFinish = Date.Today.AddDays(10).Add(TimeSpan.Parse("10:00:00"))
        item6.Slack = TimeSpan.Zero
        item6.AssignmentsContent = "Resource 1"
        item6.Predecessors.Add(New NetworkPredecessorItem With {.Item = item5})

        Dim item7 As CustomNetworkDiagramItem = TryCast(NetworkDiagramView.Items(7), CustomNetworkDiagramItem)
        item7.Effort = TimeSpan.Parse("20:00:00")
        item7.EarlyStart = Date.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"))
        item7.EarlyFinish = Date.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"))
        item7.LateStart = Date.Today.AddDays(8).Add(TimeSpan.Parse("08:00:00"))
        item7.LateFinish = Date.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"))
        item7.ActualStart = Date.Today.AddDays(7).Add(TimeSpan.Parse("12:00:00"))
        item7.ActualFinish = Date.Today.AddDays(9).Add(TimeSpan.Parse("16:00:00"))
        item7.Slack = TimeSpan.Parse("1.04:00:00")
        item7.AssignmentsContent = "Resource 2"
        item7.Predecessors.Add(New NetworkPredecessorItem With {.Item = item5})

        Dim item8 As CustomNetworkDiagramItem = TryCast(NetworkDiagramView.Items(8), CustomNetworkDiagramItem)
        item8.LateFinish = Date.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"))
        item8.LateStart = item8.LateFinish
        item8.EarlyFinish = item8.LateFinish
        item8.EarlyStart = item8.EarlyFinish
        item8.ActualFinish = Date.Today.AddDays(10).Add(TimeSpan.Parse("10:00:00"))
        item8.ActualStart = item8.ActualFinish
        item8.AssignmentsContent = "N/A"
        item8.Predecessors.Add(New NetworkPredecessorItem With {.Item = item6})
        item8.Predecessors.Add(New NetworkPredecessorItem With {.Item = item7})
    End Sub

    Private theme As String = "Generic-bright"
    Public Sub New(theme As String)
        Me.New()
        Me.theme = theme
        ApplyTemplate()
    End Sub
    Public Overrides Sub OnApplyTemplate()
        LoadTheme()
        MyBase.OnApplyTemplate()
    End Sub
    Private Sub LoadTheme()
        If theme Is Nothing OrElse theme = "Default" OrElse theme = "Aero" Then
            Return
        End If
        Dim themeResourceDictionary = New ResourceDictionary With {.Source = New Uri("/" & Me.GetType().Assembly.GetName().Name & ";component/Themes/" & theme & ".xaml", UriKind.Relative)}
        NetworkDiagramView.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub
End Class
