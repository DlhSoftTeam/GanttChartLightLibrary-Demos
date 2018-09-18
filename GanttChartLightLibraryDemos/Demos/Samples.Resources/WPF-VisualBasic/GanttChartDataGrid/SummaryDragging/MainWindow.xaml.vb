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

        Dim item0 As GanttChartItem = GanttChartDataGrid.Items(0)

        Dim item1 As GanttChartItem = GanttChartDataGrid.Items(1)
        item1.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item1.Finish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        item1.CompletedFinish = Date.Today.Add(TimeSpan.Parse("12:00:00"))
        item1.AssignmentsContent = "Resource 1"

        Dim item2 As GanttChartItem = GanttChartDataGrid.Items(2)
        item2.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item2.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item2.AssignmentsContent = "Resource 1, Resource 2"
        item2.Predecessors.Add(New PredecessorItem With {.Item = item1})

        Dim item3 As GanttChartItem = GanttChartDataGrid.Items(3)
        item3.Predecessors.Add(New PredecessorItem With {.Item = item0, .DependencyType = DependencyType.StartStart})

        Dim item4 As GanttChartItem = GanttChartDataGrid.Items(4)
        item4.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item4.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"))

        Dim item6 As GanttChartItem = GanttChartDataGrid.Items(6)
        item6.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item6.Finish = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))

        Dim item7 As GanttChartItem = GanttChartDataGrid.Items(7)
        item7.Start = Date.Today.AddDays(4)
        item7.IsMilestone = True
        item7.Predecessors.Add(New PredecessorItem With {.Item = item4})
        item7.Predecessors.Add(New PredecessorItem With {.Item = item6})

        For i As Integer = 3 To 25
            GanttChartDataGrid.Items.Add(New GanttChartItem With {.Content = "Task " & i, .Indentation = If(i Mod 3 = 0, 0, 1), .Start = Date.Today.AddDays(If(i <= 8, (i - 4) * 2, i - 8)), .Finish = Date.Today.AddDays((If(i <= 8, (i - 4) * 2 + (If(i > 8, 6, 1)), i - 2)) + 2), .CompletedFinish = Date.Today.AddDays(If(i <= 8, (i - 4) * 2, i - 8)).AddDays(If(i Mod 6 = 4, 3, 0))})
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

    ' Used to record information/state while summary item dragging operations occur.
    ' DraggedSummaryItem indicates the summary item being dragged, and draggedChildItemOffsets indicate the leaf child items and their effort offsets compared to the dragged summary item.
    Private draggedSummaryItem As GanttChartItem
    Private draggedChildItemOffsets As New Dictionary(Of GanttChartItem, TimeSpan)()

    Private Sub Thumb_DragStarted(sender As Object, e As DragStartedEventArgs)
        Dim element = TryCast(sender, Thumb)
        ' Record the summary item that the end user started to drag, and store the original offsets for the start date-time values
        ' of its leaf child items (of all levels) - to be used upon DragDelta event handler.
        If element Is Nothing Then Return
        draggedSummaryItem = TryCast(element.DataContext, GanttChartItem)
        If draggedSummaryItem Is Nothing Then Return
        draggedChildItemOffsets = draggedSummaryItem.AllChildren.Cast(Of GanttChartItem)().Where(Function(i) (Not i.HasChildren)).ToDictionary(Function(i) i, Function(i) GanttChartDataGrid.GetEffort(draggedSummaryItem.Start, i.Start))
    End Sub

    Private Sub Thumb_DragDelta(sender As Object, e As DragDeltaEventArgs)
        If draggedSummaryItem Is Nothing Then
            Return
        End If
        ' Determine the updated summary start date-time based on the visibility schedule (visible days and hours of the timeline)
        ' and considering the horizontally dragged distance (e.HorizontalChange) reported by the thumb.
        Dim draggedDurationInHours = e.HorizontalChange / GanttChartDataGrid.HourWidth
        Dim draggedDuration = TimeSpan.FromHours(draggedDurationInHours)
        Dim visibilitySchedule = GanttChartDataGrid.GetVisibilitySchedule()
        Dim updatedSummaryStart = GanttChartDataGrid.GetFinish(draggedSummaryItem.Start, draggedDuration, visibilitySchedule)
        ' Then, update all leaf child item start date-times according to the updated start value of the summary and 
        ' considering their original effort offsets.
        For Each item As GanttChartItem In draggedChildItemOffsets.Keys
            Dim updatedStart = GanttChartDataGrid.GetFinish(updatedSummaryStart, draggedChildItemOffsets(item))
            item.RescheduleToStart(updatedStart)
        Next item
    End Sub
End Class
