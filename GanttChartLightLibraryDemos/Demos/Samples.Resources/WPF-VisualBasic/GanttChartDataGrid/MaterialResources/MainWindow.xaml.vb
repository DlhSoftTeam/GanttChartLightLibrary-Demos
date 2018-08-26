Imports DlhSoft.Windows.Controls
Imports System.Windows.Threading
Imports DlhSoft.Windows.Data
Imports System.Collections.ObjectModel

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim dateTime As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 8, 0, 0)

        For i As Integer = 1 To 16
            GanttChartDataGrid.Items.Add(New GanttChartItem With {.Content = "Print job #" & i, .Start = New Date(dateTime.Year, dateTime.Month, dateTime.Day, 8, 0, 0)})
        Next i

        ' Set up resources.
        GanttChartDataGrid.AssignableResources = New ObservableCollection(Of String) From {"Printer", "Paper", "Supervisor"}
        GanttChartDataGrid.ResourceQuantities = New Dictionary(Of String, Double) From {{"Printer", 5}, {"Paper", Double.PositiveInfinity}, {"Supervisor", 2}}
        ' Define printing cost for 100 sheets of paper (default quantity used for cost by design).
        GanttChartDataGrid.SpecificResourceHourCosts = New Dictionary(Of String, Double) From {{"Paper", 5}}

        ' Assign a printer, the number of pages to pront on each print job, and part of the time of a supervisor needed to overview the printing jobs.
        ' Update finish times of the task to based on their estimated durations, considering this ratio: 15 sheets of paper per minute.
        Dim sheetsOfPaperRequiredForPrintJobs = New Decimal() {50, 20, 30, 60, 25, 10, 30, 50, 60, 80, 100, 25, 30, 30, 120, 80}
        For i As Integer = 0 To GanttChartDataGrid.Items.Count - 1
            Dim requiredSheetsOfPaper = sheetsOfPaperRequiredForPrintJobs(i)
            GanttChartDataGrid.Items(i).AssignmentsContent = "Printer, Paper [" & requiredSheetsOfPaper & "%], Supervisor [50%]"
            GanttChartDataGrid.Items(i).Finish = New Date(dateTime.Year, dateTime.Month, dateTime.Day, 8, Convert.ToInt16(Math.Ceiling(requiredSheetsOfPaper / 15)), 0)
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

        ' Set timeline page start and displayed time.
        GanttChartDataGrid.SetTimelinePage(dateTime.AddHours(-1), dateTime.AddHours(2))
        GanttChartDataGrid.DisplayedTime = dateTime.AddMinutes(-dateTime.Minute)
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

    Private Sub GanttChartDataGrid_TimelinePageChanged(sender As Object, e As EventArgs)
        Dispatcher.BeginInvoke(CType(Sub()
                                         Dim hourQuarterScale As Scale = GanttChartDataGrid.Scales(0)
                                         hourQuarterScale.Intervals.Clear()
                                         Dim dateTime As Date = GanttChartDataGrid.TimelinePageStart
                                         Do While dateTime <= GanttChartDataGrid.TimelinePageFinish
                                             hourQuarterScale.Intervals.Add(New ScaleInterval(dateTime, dateTime.AddMinutes(15)) With {.HeaderContent = dateTime.ToString("g")})
                                             dateTime = dateTime.AddMinutes(15)
                                         Loop
                                         Dim minuteScale As Scale = GanttChartDataGrid.Scales(1)
                                         minuteScale.Intervals.Clear()
                                         dateTime = GanttChartDataGrid.TimelinePageStart
                                         Do While dateTime <= GanttChartDataGrid.TimelinePageFinish
                                             minuteScale.Intervals.Add(New ScaleInterval(dateTime, dateTime.AddMinutes(3)) With {.HeaderContent = dateTime.ToString("mm")})
                                             dateTime = dateTime.AddMinutes(3)
                                         Loop
                                     End Sub, Action), DispatcherPriority.Render)
    End Sub

    Private Sub LevelResourcesButton_Click(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.LevelResources(Date.Today.AddHours(8))
    End Sub
End Class
