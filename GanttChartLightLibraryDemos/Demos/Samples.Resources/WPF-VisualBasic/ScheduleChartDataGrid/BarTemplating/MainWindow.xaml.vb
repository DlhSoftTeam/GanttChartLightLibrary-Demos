Imports DlhSoft.Windows.Controls

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim applicationName As String = Me.GetType().Namespace

        Dim task1 As CustomGanttChartItem = TryCast(ScheduleChartDataGrid.Items(0).GanttChartItems(0), CustomGanttChartItem)
        task1.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        task1.Finish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        task1.CompletedFinish = Date.Today.Add(TimeSpan.Parse("12:00:00"))
        task1.Icon = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Check.png", applicationName), UriKind.Absolute))
        task1.EstimatedStart = Date.Today.AddDays(-1).Add(TimeSpan.Parse("08:00:00"))
        task1.EstimatedFinish = Date.Today.AddDays(-1).Add(TimeSpan.Parse("16:00:00"))

        Dim task21 As CustomGanttChartItem = TryCast(ScheduleChartDataGrid.Items(0).GanttChartItems(1), CustomGanttChartItem)
        task21.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"))
        task21.Finish = Date.Today.AddDays(5).Add(TimeSpan.Parse("16:00:00"))
        task21.AssignmentsContent = "50%"
        task21.Icon = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Person.png", applicationName), UriKind.Absolute))
        task21.Markers.Add(New Marker With {.DateTime = Date.Today.AddDays(1).Add(TimeSpan.Parse("09:00:00")), .Icon = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Warning.png", applicationName), UriKind.Absolute)), .Note = "Validation required for Task 2"})
        task21.Markers.Add(New Marker With {.DateTime = Date.Today.AddDays(3).Add(TimeSpan.Parse("14:00:00")), .Icon = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Error.png", applicationName), UriKind.Absolute)), .Note = "Impossible to finish Task 2"})

        Dim task22 As CustomGanttChartItem = TryCast(ScheduleChartDataGrid.Items(1).GanttChartItems(0), CustomGanttChartItem)
        task22.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"))
        task22.Finish = Date.Today.AddDays(4).Add(TimeSpan.Parse("16:00:00"))
        task22.Note = "This assignment is very important."
        task22.EstimatedStart = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        task22.EstimatedFinish = Date.Today.AddDays(4 - 2).Add(TimeSpan.Parse("12:00:00"))
        task22.Interruptions.Add(New Interruption With {.Start = Date.Today.AddDays(4).Add(TimeSpan.Parse("10:00:00")), .Finish = Date.Today.AddDays(4).Add(TimeSpan.Parse("13:00:00"))})

        For i As Integer = 3 To 16
            Dim item As ScheduleChartItem = New ScheduleChartItem With {.Content = "Resource " & i}
            For j As Integer = 1 To (i - 1) Mod 4 + 1
                item.GanttChartItems.Add(New CustomGanttChartItem With {.Content = "Task " & i & "." & j, .Start = Date.Today.AddDays(i + (i - 1) * (j - 1)), .Finish = Date.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1), .CompletedFinish = Date.Today.AddDays(i + (i - 1) * (j - 1)).AddDays(If((i + j) Mod 5 = 2, 2, 0))})
            Next j
            ScheduleChartDataGrid.Items.Add(item)
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
        ScheduleChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub
End Class
