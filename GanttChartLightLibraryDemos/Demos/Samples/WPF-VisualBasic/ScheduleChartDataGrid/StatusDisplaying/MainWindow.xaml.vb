Imports DlhSoft.Windows.Controls

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim [date] As Date = Date.Today
        Dim year As Integer = [date].Year, month As Integer = [date].Month

        Dim startedStatusBar As Brush = Brushes.YellowGreen
        Dim issuesStatusBar As Brush = Brushes.Red
        Dim maintenanceStatusBar As Brush = Brushes.Orange

        Dim resource1 As ScheduleChartItem = New ScheduleChartItem With {.Content = "Resource 1"}

        Dim r1S1 As GanttChartItem = New GanttChartItem With {.Content = "Resource 1: started", .Start = New Date(year, month, 2, 8, 0, 0), .Finish = New Date(year, month, 8, 16, 0, 0)}
        GanttChartView.SetStandardBarFill(r1S1, startedStatusBar)
        resource1.GanttChartItems.Add(r1S1)

        Dim r1M1 As GanttChartItem = New GanttChartItem With {.Content = "Resource 1: maintenance", .Start = New Date(year, month, 9, 8, 0, 0), .Finish = New Date(year, month, 9, 16, 0, 0)}
        GanttChartView.SetStandardBarFill(r1M1, maintenanceStatusBar)
        resource1.GanttChartItems.Add(r1M1)

        Dim r1S2 As GanttChartItem = New GanttChartItem With {.Content = "Resource 1: started", .Start = New Date(year, month, 10, 8, 0, 0), .Finish = New Date(year, month, 13, 16, 0, 0)}
        GanttChartView.SetStandardBarFill(r1S2, startedStatusBar)
        resource1.GanttChartItems.Add(r1S2)

        Dim r1I1 As GanttChartItem = New GanttChartItem With {.Content = "Resource 1: issues", .Start = New Date(year, month, 14, 8, 0, 0), .Finish = New Date(year, month, 14, 16, 0, 0)}
        GanttChartView.SetStandardBarFill(r1I1, issuesStatusBar)
        resource1.GanttChartItems.Add(r1I1)

        Dim r1M2 As GanttChartItem = New GanttChartItem With {.Content = "Resource 1: maintenance", .Start = New Date(year, month, 15, 8, 0, 0), .Finish = New Date(year, month, 16, 16, 0, 0)}
        GanttChartView.SetStandardBarFill(r1M2, maintenanceStatusBar)
        resource1.GanttChartItems.Add(r1M2)

        Dim r1S3 As GanttChartItem = New GanttChartItem With {.Content = "Resource 1: started", .Start = New Date(year, month, 16, 8, 0, 0), .Finish = New Date(year, month, 22, 16, 0, 0)}
        GanttChartView.SetStandardBarFill(r1S3, startedStatusBar)
        resource1.GanttChartItems.Add(r1S3)

        ScheduleChartDataGrid.Items.Add(resource1)

        Dim resource2 As ScheduleChartItem = New ScheduleChartItem With {.Content = "Resource 2"}

        Dim r2S1 As GanttChartItem = New GanttChartItem With {.Content = "Resource 2: started", .Start = New Date(year, month, 2, 8, 0, 0), .Finish = New Date(year, month, 8, 16, 0, 0)}
        GanttChartView.SetStandardBarFill(r2S1, startedStatusBar)
        resource2.GanttChartItems.Add(r2S1)

        Dim r2I1 As GanttChartItem = New GanttChartItem With {.Content = "Resource 2: issues", .Start = New Date(year, month, 9, 8, 0, 0), .Finish = New Date(year, month, 12, 16, 0, 0)}
        GanttChartView.SetStandardBarFill(r2I1, issuesStatusBar)
        resource2.GanttChartItems.Add(r2I1)

        Dim r2M1 As GanttChartItem = New GanttChartItem With {.Content = "Resource 2: maintenance", .Start = New Date(year, month, 13, 8, 0, 0), .Finish = New Date(year, month, 14, 16, 0, 0)}
        GanttChartView.SetStandardBarFill(r2M1, maintenanceStatusBar)
        resource2.GanttChartItems.Add(r2M1)

        Dim r2S2 As GanttChartItem = New GanttChartItem With {.Content = "Resource 2: started", .Start = New Date(year, month, 15, 8, 0, 0), .Finish = New Date(year, month, 22, 16, 0, 0)}
        GanttChartView.SetStandardBarFill(r2S2, startedStatusBar)
        resource2.GanttChartItems.Add(r2S2)

        ScheduleChartDataGrid.Items.Add(resource2)

        Dim resource3 As ScheduleChartItem = New ScheduleChartItem With {.Content = "Resource 3"}
        Dim r3S1 As GanttChartItem = New GanttChartItem With {.Content = "Resource 3: started", .Start = New Date(year, month, 2, 8, 0, 0), .Finish = New Date(year, month, 22, 16, 0, 0)}
        GanttChartView.SetStandardBarFill(r3S1, startedStatusBar)
        resource3.GanttChartItems.Add(r3S1)

        ScheduleChartDataGrid.Items.Add(resource3)

        For i As Integer = 4 To 16
            Dim item As ScheduleChartItem = New ScheduleChartItem With {.Content = "Resource " & i}

            Dim ts1 As GanttChartItem = New GanttChartItem With {.Content = "Resource " & i & ": started", .Start = New Date(year, month, 2, 8, 0, 0), .Finish = New Date(year, month, 5, 16, 0, 0)}
            GanttChartView.SetStandardBarFill(ts1, startedStatusBar)
            item.GanttChartItems.Add(ts1)

            Dim ti1 As GanttChartItem = New GanttChartItem With {.Content = "Resource " & i & ": issues", .Start = New Date(year, month, 6, 8, 0, 0), .Finish = New Date(year, month, 6, 16, 0, 0)}
            GanttChartView.SetStandardBarFill(ti1, issuesStatusBar)
            item.GanttChartItems.Add(ti1)

            Dim tm1 As GanttChartItem = New GanttChartItem With {.Content = "Resource " & i & ": maintenance", .Start = New Date(year, month, 7, 8, 0, 0), .Finish = New Date(year, month, 7, 16, 0, 0)}
            GanttChartView.SetStandardBarFill(tm1, maintenanceStatusBar)
            item.GanttChartItems.Add(tm1)

            Dim ts2 As GanttChartItem = New GanttChartItem With {.Content = "Resource " & i & ": started", .Start = New Date(year, month, 8, 8, 0, 0), .Finish = New Date(year, month, 15, 16, 0, 0)}
            GanttChartView.SetStandardBarFill(ts2, startedStatusBar)
            item.GanttChartItems.Add(ts2)

            Dim ti2 As GanttChartItem = New GanttChartItem With {.Content = "Resource " & i & ": issues", .Start = New Date(year, month, 16, 8, 0, 0), .Finish = New Date(year, month, 16, 16, 0, 0)}
            GanttChartView.SetStandardBarFill(ti2, issuesStatusBar)
            item.GanttChartItems.Add(ti2)

            Dim ts3 As GanttChartItem = New GanttChartItem With {.Content = "Resource " & i & ": started", .Start = New Date(year, month, 17, 8, 0, 0), .Finish = New Date(year, month, 22, 16, 0, 0)}
            GanttChartView.SetStandardBarFill(ts3, startedStatusBar)
            item.GanttChartItems.Add(ts3)

            ScheduleChartDataGrid.Items.Add(item)
        Next i

        ScheduleChartDataGrid.DisplayedTime = New Date(year, month, 1)
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
