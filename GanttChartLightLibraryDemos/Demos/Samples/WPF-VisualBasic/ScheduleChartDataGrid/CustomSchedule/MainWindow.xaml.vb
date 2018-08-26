Imports DlhSoft.Windows.Controls
Imports DlhSoft.Windows.Data
Imports System.Collections.ObjectModel

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        ScheduleChartDataGrid.WorkingDayStart = TimeOfDay.Parse("10:00:00")
        ScheduleChartDataGrid.WorkingDayFinish = TimeOfDay.Parse("14:00:00")
        ScheduleChartDataGrid.NonworkingIntervals = New ObservableCollection(Of TimeInterval)(New TimeInterval() {New TimeInterval(Date.Today.AddDays(2), Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")))})
        ScheduleChartDataGrid.TimelinePageStart = Date.Today
        ScheduleChartDataGrid.TimelinePageFinish = Date.Today.AddMonths(2)
        ScheduleChartDataGrid.VisibleDayStart = TimeOfDay.Parse("09:00:00")
        ScheduleChartDataGrid.VisibleDayFinish = TimeOfDay.Parse("15:00:00")

        Dim task1 As GanttChartItem = ScheduleChartDataGrid.Items(0).GanttChartItems(0)
        task1.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        task1.Finish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        task1.CompletedFinish = Date.Today.Add(TimeSpan.Parse("12:00:00"))

        Dim task21 As GanttChartItem = ScheduleChartDataGrid.Items(0).GanttChartItems(1)
        task21.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"))
        task21.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        task21.AssignmentsContent = "50%"

        Dim task22 As GanttChartItem = ScheduleChartDataGrid.Items(1).GanttChartItems(0)
        task22.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"))
        task22.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))

        For i As Integer = 3 To 16
            Dim item As ScheduleChartItem = New ScheduleChartItem With {.Content = "Resource " & i}
            For j As Integer = 1 To (i - 1) Mod 4 + 1
                item.GanttChartItems.Add(New GanttChartItem With {.Content = "Task " & i & "." & j, .Start = Date.Today.AddDays((i - 1) * (j - 1)), .Finish = Date.Today.AddDays((i - 1) * (j - 1) + 1), .CompletedFinish = Date.Today.AddDays((i - 1) * (j - 1)).AddDays(If((i + j) Mod 5 = 2, 2, 0))})
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
