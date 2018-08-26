Imports DlhSoft.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports DlhSoft.Windows.Data

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim item0 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(0), CustomGanttChartItem)

        Dim item1 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(1), CustomGanttChartItem)
        item1.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item1.Finish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        item1.CustomStart = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item1.CustomFinish = Date.Today.AddDays(1).Add(TimeSpan.Parse("16:00:00"))

        Dim item2 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(2), CustomGanttChartItem)
        item2.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item2.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item2.Predecessors.Add(New PredecessorItem With {.Item = item1})
        item2.CustomStart = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item2.CustomFinish = Date.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00"))

        Dim item3 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(3), CustomGanttChartItem)
        item3.Predecessors.Add(New PredecessorItem With {.Item = item0, .DependencyType = DependencyType.StartStart})
        item3.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item3.Finish = Date.Today.AddDays(1).Add(TimeSpan.Parse("16:00:00"))
        item3.CustomStart = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item3.CustomFinish = Date.Today.AddDays(1).Add(TimeSpan.Parse("16:00:00"))

        Dim item4 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(4), CustomGanttChartItem)
        item4.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item4.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"))
        item4.CustomStart = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item4.CustomFinish = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))

        Dim item6 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(6), CustomGanttChartItem)
        item6.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item6.Finish = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))
        item6.CustomStart = Date.Today.AddDays(+1).Add(TimeSpan.Parse("08:00:00"))
        item6.CustomFinish = Date.Today.AddDays(5).Add(TimeSpan.Parse("12:00:00"))

        Dim item7 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(7), CustomGanttChartItem)
        item7.Start = Date.Today.AddDays(4)
        item7.IsMilestone = True
        item7.Predecessors.Add(New PredecessorItem With {.Item = item4})
        item7.Predecessors.Add(New PredecessorItem With {.Item = item6})

        For i As Integer = 3 To 25
            GanttChartDataGrid.Items.Add(New CustomGanttChartItem With {.Content = "Task " & i, .Indentation = If(i Mod 3 = 0, 0, 1), .Start = Date.Today.AddDays(If(i <= 8, (i - 4) * 2, i - 8)), .Finish = Date.Today.AddDays((If(i <= 8, (i - 4) * 2 + (If(i > 8, 6, 1)), i - 2)) + 2), .CustomStart = Date.Today.AddDays(If(i <= 8, (i - 4) * 2, i - 8)), .CustomFinish = Date.Today.AddDays((If(i <= 8, (i - 4) * 2 + (If(i > 8, 6, 1)), i - 2)) + 2)})
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

    Private Sub CustomBarThumb_DragDelta(sender As Object, e As DragDeltaEventArgs)
        Dim thumb = TryCast(sender, Thumb)
        Dim item = TryCast(thumb.DataContext, CustomGanttChartItem)
        Dim customStart = item.CustomStart
        Dim customStartPosition = GanttChartDataGrid.GetPosition(customStart)
        Dim visibilitySchedule = GanttChartDataGrid.GetVisibilitySchedule()
        Dim customDuration = GanttChartDataGrid.GetEffort(item.CustomStart, item.CustomFinish, visibilitySchedule)
        customStartPosition += e.HorizontalChange
        customStart = GanttChartDataGrid.GetDateTime(customStartPosition)
        item.CustomStart = customStart
        item.CustomFinish = GanttChartDataGrid.GetFinish(customStart, customDuration, visibilitySchedule)

        Dim scrollViewer = GanttChartDataGrid.GanttChartView.ScrollViewer
        Dim mousePosition = Mouse.GetPosition(scrollViewer)
        Const autoScrollMargin As Double = 10
        If mousePosition.X < autoScrollMargin Then
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - autoScrollMargin)
        ElseIf mousePosition.X > scrollViewer.ActualWidth - autoScrollMargin Then
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + autoScrollMargin)
        End If
    End Sub
End Class
