Imports DlhSoft.Windows.Controls
Imports System.Windows.Threading

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()
        Dim item0 As GanttChartItem = GanttChartDataGrid.Items(0)

        Dim item1 As GanttChartItem = GanttChartDataGrid.Items(1)
        ' Use the numeric day origin (defined as a static value) for date and time values of Gantt Chart items.
        item1.Start = NumericDayOrigin
        item1.Finish = NumericDayOrigin.AddDays(1)
        item1.CompletedFinish = NumericDayOrigin.AddDays(1)
        item1.AssignmentsContent = "Resource 1"

        Dim item2 As GanttChartItem = GanttChartDataGrid.Items(2)
        item2.Start = NumericDayOrigin.AddDays(1)
        item2.Finish = NumericDayOrigin.AddDays(3)
        item2.CompletedFinish = NumericDayOrigin.AddDays(1)
        item2.AssignmentsContent = "Resource 1, Resource 2"
        item2.Predecessors.Add(New PredecessorItem With {.Item = item1})

        Dim item3 As GanttChartItem = GanttChartDataGrid.Items(3)
        item3.Predecessors.Add(New PredecessorItem With {.Item = item0, .DependencyType = DependencyType.StartStart})

        Dim item4 As GanttChartItem = GanttChartDataGrid.Items(4)
        item4.Start = NumericDayOrigin
        item4.Finish = NumericDayOrigin.AddDays(3)
        item4.CompletedFinish = NumericDayOrigin

        Dim item6 As GanttChartItem = GanttChartDataGrid.Items(6)
        item6.Start = NumericDayOrigin
        item6.Finish = NumericDayOrigin.AddDays(3)
        item6.CompletedFinish = NumericDayOrigin

        Dim item7 As GanttChartItem = GanttChartDataGrid.Items(7)
        item7.Start = NumericDayOrigin.AddDays(4)
        item7.IsMilestone = True
        item7.Predecessors.Add(New PredecessorItem With {.Item = item4})
        item7.Predecessors.Add(New PredecessorItem With {.Item = item6})

        Dim item8 As GanttChartItem = GanttChartDataGrid.Items(6)
        item8.Start = NumericDayOrigin
        item8.Finish = NumericDayOrigin.AddDays(3)
        item8.CompletedFinish = NumericDayOrigin

        Dim item9 As GanttChartItem = GanttChartDataGrid.Items(6)
        item9.Start = NumericDayOrigin
        item9.Finish = NumericDayOrigin.AddDays(3)
        item9.CompletedFinish = NumericDayOrigin

        Dim item10 As GanttChartItem = GanttChartDataGrid.Items(6)
        item10.Start = NumericDayOrigin
        item10.Finish = NumericDayOrigin.AddDays(3)
        item10.CompletedFinish = NumericDayOrigin

        For i As Integer = 3 To 25
            GanttChartDataGrid.Items.Add(New GanttChartItem With {.Content = "Task " & i, .Indentation = If(i Mod 3 = 0, 0, 1), .Start = NumericDayOrigin.AddDays(If(i <= 8, (i - 4) * 3, i - 8)), .Finish = NumericDayOrigin.AddDays((If(i <= 8, (i - 4) * 3 + (If(i > 8, 6, 1)), i - 2)) + 3), .CompletedFinish = NumericDayOrigin.AddDays(If(i <= 8, (i - 4) * 3, i - 8)).AddDays(If(i Mod 6 = 1, 3, 0))})
        Next i

        ' Set timeline page start and displayed time to the numeric day origin.
        GanttChartDataGrid.SetTimelinePage(NumericDayOrigin, NumericDayOrigin.AddDays(45))
        GanttChartDataGrid.DisplayedTime = NumericDayOrigin
    End Sub

    Private themeResourceDictionary As ResourceDictionary
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
        themeResourceDictionary = New ResourceDictionary With {.Source = New Uri("/" & Me.GetType().Assembly.GetName().Name & ";component/Themes/" & theme & ".xaml", UriKind.Relative)}
        GanttChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub

    Private Shared ReadOnly Property NumericDayOrigin() As Date
        Get
            Return NumericDayStringConverter.Origin
        End Get
    End Property

    Private Sub GanttChartDataGrid_TimelinePageChanged(sender As Object, e As EventArgs)
        ' Use Dispatcher.BeginInvoke in order to ensure that scale objects and their interval header items are properly created before setting their HeaderContent values.
        ' Use DispatcherPriority.Render to apply the changes when rendering the view.
        Dispatcher.BeginInvoke(CType(Sub()
                                         ' Scales use one based indexes because a special scale (non working highlighting) is inserted at position zero during control initialization (behind the scenes).
                                         If GanttChartDataGrid.Scales.Count <= 2 Then
                                             Return
                                         End If
                                         Dim weekScale As Scale = GanttChartDataGrid.Scales(1)
                                         For Each i As ScaleInterval In weekScale.Intervals
                                             i.HeaderContent = If(i.TimeInterval.Start.Date >= NumericDayOrigin, String.Format("Week {0}", CInt(Fix((i.TimeInterval.Start.Date - NumericDayOrigin).TotalDays)) \ 7 + 1), String.Empty)
                                         Next i
                                         Dim dayScale As Scale = GanttChartDataGrid.Scales(2)
                                         For Each i As ScaleInterval In dayScale.Intervals
                                             i.HeaderContent = If(i.TimeInterval.Start.Date >= NumericDayOrigin, String.Format("{0:00}", (CInt(Fix((i.TimeInterval.Start.Date - NumericDayOrigin).TotalDays)) + 1) Mod 100), String.Empty)
                                         Next i
                                     End Sub, Action), DispatcherPriority.Render)
    End Sub
End Class
