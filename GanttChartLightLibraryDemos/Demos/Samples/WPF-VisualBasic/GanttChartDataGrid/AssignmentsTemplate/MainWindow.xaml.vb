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
        item1.AssignmentsIconSource = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Person 1.png", applicationName), UriKind.Absolute))
        item1.AssignmentsContent = "Resource 1"

        Dim item2 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(2), CustomGanttChartItem)
        item2.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item2.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item2.AssignmentsIconSource = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Persons.png", applicationName), UriKind.Absolute))
        item2.AssignmentsContent = "Resource 1, Resource 2"
        item2.Predecessors.Add(New PredecessorItem With {.Item = item1})

        Dim item3 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(3), CustomGanttChartItem)
        item3.Predecessors.Add(New PredecessorItem With {.Item = item0, .DependencyType = DependencyType.StartStart})

        Dim item4 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(4), CustomGanttChartItem)
        item4.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item4.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"))
        item4.AssignmentsIconSource = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Person 2.png", applicationName), UriKind.Absolute))
        item4.AssignmentsContent = "Resource 2"

        Dim item6 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(6), CustomGanttChartItem)
        item6.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item6.Finish = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))
        item6.AssignmentsIconSource = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Person 1.png", applicationName), UriKind.Absolute))
        item6.AssignmentsContent = "Resource 1"

        Dim item7 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(7), CustomGanttChartItem)
        item7.Start = Date.Today.AddDays(4)
        item7.IsMilestone = True
        item7.AssignmentsIconSource = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Person 2.png", applicationName), UriKind.Absolute))
        item7.AssignmentsContent = "Resource 2"
        item7.Predecessors.Add(New PredecessorItem With {.Item = item4})
        item7.Predecessors.Add(New PredecessorItem With {.Item = item6})

        For i As Integer = 3 To 25
            GanttChartDataGrid.Items.Add(New CustomGanttChartItem With {.Content = "Task " & i, .Indentation = If(i Mod 3 = 0, 0, 1), .Start = Date.Today.AddDays(If(i <= 8, (i - 4) * 2, i - 8)), .Finish = Date.Today.AddDays((If(i <= 8, (i - 4) * 2 + (If(i > 8, 6, 1)), i - 2)) + 2), .CompletedFinish = Date.Today.AddDays(If(i <= 8, (i - 4) * 2, i - 8)).AddDays(If(i Mod 6 = 4, 3, 0)), .AssignmentsIconSource = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Person {1}.png", applicationName, 1 + i Mod 2), UriKind.Absolute))})
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

