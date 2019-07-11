Imports DlhSoft.Windows.Controls
Imports DlhSoft.Windows.Data
Imports System.Windows.Threading

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        ' Record the current date and time at minute level.
        Dim now As Date = Date.Now
        now = now.Date.AddHours(now.Hour).AddMinutes(now.Minute)

        Dim item0 As GanttChartItem = GanttChartDataGrid.Items(0)

        Dim item1 As GanttChartItem = GanttChartDataGrid.Items(1)
        item1.Start = now.Add(TimeSpan.Parse("00:08:00"))
        item1.Finish = now.Add(TimeSpan.Parse("00:16:00"))
        item1.CompletedFinish = now.Add(TimeSpan.Parse("00:12:00"))
        item1.AssignmentsContent = "Resource 1"

        Dim item2 As GanttChartItem = GanttChartDataGrid.Items(2)
        item2.Start = now.Add(TimeSpan.Parse("00:16:00"))
        item2.Finish = now.Add(TimeSpan.Parse("00:32:00"))
        item2.AssignmentsContent = "Resource 1, Resource 2"
        item2.Predecessors.Add(New PredecessorItem With {.Item = item1})

        Dim item3 As GanttChartItem = GanttChartDataGrid.Items(3)
        item3.Predecessors.Add(New PredecessorItem With {.Item = item0, .DependencyType = DependencyType.StartStart})

        Dim item4 As GanttChartItem = GanttChartDataGrid.Items(4)
        item4.Start = now.Add(TimeSpan.Parse("00:08:00"))
        item4.Finish = now.Add(TimeSpan.Parse("00:28:00"))

        Dim item6 As GanttChartItem = GanttChartDataGrid.Items(6)
        item6.Start = now.Add(TimeSpan.Parse("00:08:00"))
        item6.Finish = now.Add(TimeSpan.Parse("00:36:00"))

        Dim item7 As GanttChartItem = GanttChartDataGrid.Items(7)
        item7.Start = now.Add(TimeSpan.Parse("00:36:00"))
        item7.IsMilestone = True
        item7.Predecessors.Add(New PredecessorItem With {.Item = item4})
        item7.Predecessors.Add(New PredecessorItem With {.Item = item6})

        For i As Integer = 3 To 25
            GanttChartDataGrid.Items.Add(New GanttChartItem With {.Content = "Task " & i, .Indentation = If(i Mod 3 = 0, 0, 1), .Start = now.AddMinutes(If(i <= 8, (i - 4) * 3, i - 8)), .Finish = now.AddMinutes((If(i <= 8, (i - 4) * 3 + (If(i > 8, 6, 1)), i - 2)) + 1), .CompletedFinish = now.AddMinutes(If(i <= 8, (i - 4) * 3, i - 8)).AddMinutes(If(i Mod 6 = 1, 3, 0))})
        Next i

        ' Set working and visible time to 24 hours/day and 7 day/week, and nonworking time as not highlighted (as there is only working time).
        GanttChartDataGrid.VisibleDayStart = TimeOfDay.MinValue
        GanttChartDataGrid.WorkingDayStart = GanttChartDataGrid.VisibleDayStart
        GanttChartDataGrid.VisibleDayFinish = TimeOfDay.MaxValue
        GanttChartDataGrid.WorkingDayFinish = GanttChartDataGrid.VisibleDayFinish
        GanttChartDataGrid.VisibleWeekStart = DayOfWeek.Sunday
        GanttChartDataGrid.WorkingWeekStart = GanttChartDataGrid.VisibleWeekStart
        GanttChartDataGrid.VisibleWeekFinish = DayOfWeek.Saturday
        GanttChartDataGrid.WorkingWeekFinish = GanttChartDataGrid.VisibleWeekFinish
        GanttChartDataGrid.IsNonworkingTimeHighlighted = False

        ' Configure the minute scale.
        GanttChartDataGrid.GetScale(1).IntervalProvider = Function(start, finish)
                                                              Dim intervals = New List(Of ScaleInterval)
                                                              Dim d = start
                                                              While d < finish
                                                                  intervals.Add(New ScaleInterval(d, d.AddMinutes(1)))
                                                                  d = d.AddMinutes(1)
                                                              End While
                                                              Return intervals
                                                          End Function
        GanttChartDataGrid.GetScale(1).HeaderContentProvider = Function(start, finish) start.ToString("mm")

        ' Set timeline page start and displayed time to the numeric day origin.
        GanttChartDataGrid.SetTimelinePage(now.AddMinutes(-10), now.AddMinutes(60))
        GanttChartDataGrid.DisplayedTime = now.AddMinutes(-1)
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
