Imports DlhSoft.Windows.Controls
Imports System.Collections.ObjectModel

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()
        Dim item1 As RecurrentGanttChartItem = TryCast(GanttChartDataGrid.Items(1), RecurrentGanttChartItem)
        item1.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item1.Finish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        ' Set OccurrenceCount to indicate the number of occurrences that should be generated for the task.
        item1.OccurrenceCount = 4
        Dim item2 As RecurrentGanttChartItem = TryCast(GanttChartDataGrid.Items(2), RecurrentGanttChartItem)
        item2.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item2.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item2.OccurrenceCount = 3
        Dim item4 As RecurrentGanttChartItem = TryCast(GanttChartDataGrid.Items(4), RecurrentGanttChartItem)
        item4.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item4.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"))
        item4.OccurrenceCount = Integer.MaxValue
        Dim item6 As RecurrentGanttChartItem = TryCast(GanttChartDataGrid.Items(6), RecurrentGanttChartItem)
        item6.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item6.Finish = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))
        ' You may set OccurrenceCount to MaxValue to indicate that virtually unlimited occurrences should be generated.
        item6.OccurrenceCount = Integer.MaxValue
        Dim item7 As RecurrentGanttChartItem = TryCast(GanttChartDataGrid.Items(7), RecurrentGanttChartItem)
        item7.Start = Date.Today.AddDays(4).Add(TimeSpan.Parse("08:00:00"))
        item7.Finish = Date.Today.AddDays(4).Add(TimeSpan.Parse("16:00:00"))
        ' Set RecurrenceType to indicate the type of recurrence to use when generating occurrences for the task.
        item7.RecurrenceType = RecurrenceType.Daily
        item7.OccurrenceCount = 2

        ' Component ApplyTemplate is called in order to complete loading of the user interface, after the main ApplyTemplate that initializes the custom theme, and using an asynchronous action to allow further constructor initializations if they exist (such as setting up the theme name to load).
        Dispatcher.BeginInvoke(CType(Sub()
                                         ' Apply template to be able to access the internal GanttChartView control.
                                         ' Set up the internally managed occurrence item collection to be displayed in the chart area (instead of GanttChartDataGrid.Items).
                                         ApplyTemplate()
                                         GanttChartDataGrid.ApplyTemplate()
                                         GanttChartDataGrid.GanttChartView.Items = ganttChartItemOccurrences
                                         UpdateOccurrences()
                                     End Sub, Action))
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

    ' Stores the internally generated task occurrences displayed in the GanttChartView control.
    Private ganttChartItemOccurrences As New ObservableCollection(Of GanttChartItem)()

    Private Sub GanttChartDataGrid_TimelinePageChanged(sender As Object, e As EventArgs)
        UpdateOccurrences()
    End Sub

    Private Sub GanttChartDataGrid_ItemPropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs)
        If e.PropertyName <> "RecurrenceType" And e.PropertyName <> "OccurrenceCount" Then
            Return
        End If
        UpdateOccurrences()
    End Sub

    ' Update the managed occurrence item collection for all DataGrid items, according to the current timeline page displayed in the chart area, based on their recurrence settings.
    Private Sub UpdateOccurrences()
        For Each item As GanttChartItem In GanttChartDataGrid.Items
            UpdateOccurrences(TryCast(item, RecurrentGanttChartItem), False)
        Next item
    End Sub

    ' Update the managed occurrence item collection for the specified recurrent item, according to the current timeline page displayed in the chart area, based on their recurrence settings.
    Private Sub UpdateOccurrences(item As RecurrentGanttChartItem, preserveExistingOccurrences As Boolean)
        If item Is Nothing Then
            Return
        End If

        If Not preserveExistingOccurrences Then
            Dim occurrences As List(Of GanttChartItem) = (
                From i In ganttChartItemOccurrences
                Where i.Tag Is item
                Select i).ToList()
            For Each occurrence As GanttChartItem In occurrences
                ganttChartItemOccurrences.Remove(occurrence)
            Next occurrence
        End If

        Dim existingOccurrenceCount As Integer = ganttChartItemOccurrences.Where(Function(i) i.Tag Is item).Count()
        If item.OccurrenceCount >= existingOccurrenceCount Then
            For i As Integer = existingOccurrenceCount To item.OccurrenceCount - 1
                Dim start As Date = item.Start, finish As Date = item.Finish
                Select Case item.RecurrenceType
                    Case RecurrenceType.Daily
                        start = start.AddDays(i)
                        finish = finish.AddDays(i)
                    Case RecurrenceType.Weekly
                        start = start.AddDays(7 * i)
                        finish = finish.AddDays(7 * i)
                    Case RecurrenceType.Monthly
                        start = start.AddMonths(i)
                        finish = finish.AddMonths(i)
                    Case RecurrenceType.Yearly
                        start = start.AddYears(i)
                        finish = finish.AddYears(i)
                End Select

                ' Avoid creating occurrences that would fall after the current timeline page (i.e. in the far future).
                If start >= GanttChartDataGrid.TimelinePageFinish Then
                    Exit For
                End If

                ' Create and link a new Gantt Chart occurrence to its DataGrid item using Tag and Content properties.
                Dim occurrence As GanttChartItem = New GanttChartItem With {.Start = start, .Finish = finish, .CompletedFinish = start, .Tag = item}
                BindingOperations.SetBinding(occurrence, GanttChartItem.ContentProperty, New Binding("Content") With {.Source = item})
                ganttChartItemOccurrences.Add(occurrence)

                ' After having the new occurrence initialized as an element within GanttChartView, bind its visibility and position properties to the linked item values, accordingly (use a delegate to postpone code for the rendering phase).
                Dispatcher.BeginInvoke(CType(Sub()
                                                 BindingOperations.SetBinding(occurrence, GanttChartItem.IsVisibleProperty, New Binding("IsVisible") With {.Source = item, .Mode = BindingMode.OneWay})
                                                 BindingOperations.SetBinding(occurrence, GanttChartItem.DisplayRowIndexProperty, New Binding("ActualDisplayRowIndex") With {.Source = item})
                                             End Sub, Action))
            Next i
        Else
            Dim occurrences As List(Of GanttChartItem) = (
                From i In ganttChartItemOccurrences
                Where i.Tag Is item
                Select i).Skip(item.OccurrenceCount).ToList()
            For Each occurrence As GanttChartItem In occurrences
                ganttChartItemOccurrences.Remove(occurrence)
            Next occurrence
        End If
    End Sub
End Class
