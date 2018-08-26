Imports DlhSoft.Windows.Controls

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim applicationName As String = Me.GetType().Namespace

        Dim item0 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(0), CustomGanttChartItem)

        Dim item1 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(1), CustomGanttChartItem)
        item1.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item1.Finish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        item1.CompletedFinish = Date.Today.Add(TimeSpan.Parse("12:00:00"))
        item1.AssignmentsContent = "Resource 1"
        item1.Icon = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Check.png", applicationName), UriKind.Absolute))
        item1.EstimatedStart = Date.Today.AddDays(-1).Add(TimeSpan.Parse("08:00:00"))
        item1.EstimatedFinish = Date.Today.AddDays(-1).Add(TimeSpan.Parse("16:00:00"))

        Dim item2 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(2), CustomGanttChartItem)
        item2.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item2.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item2.AssignmentsContent = "Resource 1, Resource 2"
        item2.Predecessors.Add(New PredecessorItem With {.Item = item1})
        item2.Icon = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Person.png", applicationName), UriKind.Absolute))
        item2.Note = "This task is very important."
        item2.EstimatedStart = Date.Today.AddDays(1 - 1).Add(TimeSpan.Parse("08:00:00"))
        item2.EstimatedFinish = Date.Today.AddDays(2 + 1).Add(TimeSpan.Parse("16:00:00"))

        Dim item3 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(3), CustomGanttChartItem)
        item3.Predecessors.Add(New PredecessorItem With {.Item = item0, .DependencyType = DependencyType.StartStart})

        Dim item4 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(4), CustomGanttChartItem)
        item4.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item4.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"))
        item4.EstimatedStart = Date.Today.AddDays(+1).Add(TimeSpan.Parse("08:00:00"))
        item4.EstimatedFinish = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))
        item4.Markers.Add(New Marker With {.DateValue = Date.Today.AddDays(1).Add(TimeSpan.Parse("09:00:00")), .Icon = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Warning.png", applicationName), UriKind.Absolute)), .Note = "Validation required"})
        item4.Markers.Add(New Marker With {.DateValue = Date.Today.AddDays(1).Add(TimeSpan.Parse("14:00:00")), .Icon = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Error.png", applicationName), UriKind.Absolute)), .Note = "Impossible to finish the task"})

        Dim item6 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(6), CustomGanttChartItem)
        item6.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item6.Finish = Date.Today.AddDays(6).Add(TimeSpan.Parse("12:00:00"))
        item6.EstimatedStart = Date.Today.AddDays(+1).Add(TimeSpan.Parse("08:00:00"))
        item6.EstimatedFinish = Date.Today.AddDays(6 - 1).Add(TimeSpan.Parse("12:00:00"))
        item6.Interruptions.Add(New Interruption With {.Start = Date.Today.AddDays(2).Add(TimeSpan.Parse("14:00:00")), .Finish = Date.Today.AddDays(4).Add(TimeSpan.Parse("10:00:00"))})

        Dim item7 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(7), CustomGanttChartItem)
        item7.Start = Date.Today.AddDays(4)
        item7.IsMilestone = True
        item7.Predecessors.Add(New PredecessorItem With {.Item = item4})
        item7.Predecessors.Add(New PredecessorItem With {.Item = item6})

        For i As Integer = 3 To 25
            GanttChartDataGrid.Items.Add(New CustomGanttChartItem With {.Content = "Task " & i, .Indentation = If(i Mod 3 = 0, 0, 1), .Start = Date.Today.AddDays(If(i <= 8, (i - 4) * 3, i - 8)), .Finish = Date.Today.AddDays((If(i <= 8, (i - 4) * 3 + (If(i > 8, 6, 1)), i - 2)) + 1), .CompletedFinish = Date.Today.AddDays(If(i <= 8, (i - 4) * 3, i - 8)).AddDays(If(i Mod 6 = 1, 3, 0))})
        Next i
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
        GanttChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub
End Class
