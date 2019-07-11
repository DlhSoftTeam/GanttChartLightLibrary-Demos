Imports DlhSoft.Windows.Controls
Imports System.Collections.ObjectModel
Imports DlhSoft.Windows.Data
Imports System.Windows.Threading

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim engineerCount As Integer = 8, managerCount As Integer = 3
        For i = 1 To engineerCount
            ScheduleChartDataGrid.Items.Add(New ScheduleChartItem With {.Content = "Engineer #" & i})
        Next i
        For i = 1 To managerCount
            ScheduleChartDataGrid.Items.Add(New ScheduleChartItem With {.Content = "Manager #" & i})
        Next i

        Dim [date] As Date = Date.Today
        Dim year As Integer = Date.Today.Year, month As Integer = Date.Today.Month

        Dim timelinePageStart As New Date(year, month, 1)
        Dim timelinePageFinish As Date = timelinePageStart.AddDays(7)
        ScheduleChartDataGrid.SetTimelinePage(timelinePageStart, timelinePageFinish)
        ScheduleChartDataGrid.DisplayedTime = timelinePageStart

        ' Setup hour scale.
        Dim hoursScale As Scale = ScheduleChartDataGrid.GetScale(2)
        hoursScale.Intervals.Clear()
        Dim dateTime As Date = ScheduleChartDataGrid.TimelinePageStart.Date.AddDays(-1).AddHours(23)
        Do While dateTime <= ScheduleChartDataGrid.TimelinePageFinish
            Dim start = dateTime
            If start < ScheduleChartDataGrid.TimelinePageStart Then
                start = ScheduleChartDataGrid.TimelinePageStart
            End If
            hoursScale.Intervals.Add(New ScaleInterval(start, dateTime.AddHours(8)) With {.HeaderContent = dateTime.ToString("HH")})
            dateTime = dateTime.AddHours(8)
        Loop

        ' Set up the actual shifts for engineers and managers (resource assignments).
        Dim engineerMorning As Brush = New SolidColorBrush(Color.FromArgb(128, 104, 168, 96))
        Dim engineerAfternoon As Brush = New SolidColorBrush(Color.FromArgb(128, 239, 156, 80))
        Dim engineerNight As Brush = New SolidColorBrush(Color.FromArgb(128, 80, 108, 164))
        Dim managerMorning As Brush = New SolidColorBrush(Color.FromArgb(32, 128, 128, 128))
        Dim managerAfternoon As Brush = New SolidColorBrush(Color.FromArgb(128, 128, 128, 128))
        Dim managerNight As Brush = New SolidColorBrush(Color.FromArgb(255, 128, 128, 128))

        For i As Integer = 0 To engineerCount - 1
            Dim scheduleChartItem As ScheduleChartItem = ScheduleChartDataGrid.Items(i)
            Dim d As Date = timelinePageStart.Date.AddDays(-1).AddHours(23).AddHours((i Mod 4) * 8)
            Do While d < timelinePageFinish
                Dim shiftType As String = If(d.Hour <= 8, "morning", (If(d.Hour <= 16, "afternoon", "night")))
                Dim task As GanttChartItem = New GanttChartItem With {.Content = "Engineering " & shiftType & " shift", .Start = d, .Finish = d.AddHours(8)}
                Select Case shiftType
                    Case "morning"
                        GanttChartView.SetStandardBarFill(task, engineerMorning)
                    Case "afternoon"
                        GanttChartView.SetStandardBarFill(task, engineerAfternoon)
                    Case "night"
                        GanttChartView.SetStandardBarFill(task, engineerNight)
                End Select
                scheduleChartItem.GanttChartItems.Add(task)
                d = d.AddDays(1).AddHours(8)
            Loop
        Next i

        For i As Integer = 0 To managerCount - 1
            Dim scheduleChartItem = ScheduleChartDataGrid.Items(engineerCount + i)

            Dim d As Date = timelinePageStart.Date.AddDays(-1).AddHours(23).AddHours((i Mod 4) * 8)
            Do While d < timelinePageFinish
                Dim shiftType As String = If(d.Hour <= 8, "morning", (If(d.Hour <= 16, "afternoon", "night")))
                Dim task As GanttChartItem = New GanttChartItem With {.Content = "Management " & shiftType & " shift", .Start = d, .Finish = d.AddHours(8)}
                Select Case shiftType
                    Case "morning"
                        GanttChartView.SetStandardBarFill(task, managerMorning)
                    Case "afternoon"
                        GanttChartView.SetStandardBarFill(task, managerAfternoon)
                    Case "night"
                        GanttChartView.SetStandardBarFill(task, managerNight)
                End Select
                scheduleChartItem.GanttChartItems.Add(task)
                d = d.AddDays(1).AddHours(8)
            Loop
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

        ' Enlarge HeaderHeight to for 3 scales to fit (by default it is initialized for 2 scales).
        ScheduleChartDataGrid.HeaderHeight *= 3.0F / 2.0F
    End Sub
    Private Sub LoadTheme()
        If theme Is Nothing OrElse theme = "Default" OrElse theme = "Aero" Then
            Return
        End If
        Dim themeResourceDictionary = New ResourceDictionary With {.Source = New Uri("/" & Me.GetType().Assembly.GetName().Name & ";component/Themes/" & theme & ".xaml", UriKind.Relative)}
        ScheduleChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub
End Class
