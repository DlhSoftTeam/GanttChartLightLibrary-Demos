Imports DlhSoft.Windows.Controls
Imports DlhSoft.Windows.Data
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Windows.Threading
Imports Microsoft.Win32

Class MainWindow

    Public Sub New()
        InitializeComponent()

        Dim item0 As GanttChartItem = GanttChartDataGrid.Items(0)

        Dim item1 As GanttChartItem = GanttChartDataGrid.Items(1)
        item1.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"))
        item1.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"))
        item1.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("12:00:00"))
        item1.AssignmentsContent = "Resource 1"

        Dim item2 As GanttChartItem = GanttChartDataGrid.Items(2)
        item2.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item2.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        ' Important note: CompletedFinish value defaults to DateTime.Today, therefore you should always set it to a Start (or a value between Start and Finish) when you initialize a past task item! In this example we don't set it as the task is in the future.
        item2.AssignmentsContent = "Resource 1, Resource 2"
        item2.Predecessors.Add(New PredecessorItem() With {.Item = item1})

        Dim item3 As GanttChartItem = GanttChartDataGrid.Items(3)
        item3.Predecessors.Add(New PredecessorItem() With {.Item = item0, .DependencyType = DependencyType.StartStart})

        Dim item4 As GanttChartItem = GanttChartDataGrid.Items(4)
        item4.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"))
        item4.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"))

        Dim item6 As GanttChartItem = GanttChartDataGrid.Items(6)
        item6.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"))
        item6.Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))
        item6.BaselineStart = item6.Start
        item6.BaselineFinish = item6.Finish

        Dim item7 As GanttChartItem = GanttChartDataGrid.Items(7)
        item7.Start = DateTime.Today.AddDays(4)
        item7.IsMilestone = True
        item7.Predecessors.Add(New PredecessorItem() With {.Item = item4})
        item7.Predecessors.Add(New PredecessorItem() With {.Item = item6})
        item7.BaselineStart = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))

        Dim item8 As GanttChartItem = GanttChartDataGrid.Items(8)
        item8.Start = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))
        item8.Finish = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("14:00:00"))
        item8.AssignmentsContent = "Resource 1 [50%], Resource 2 [75%]"
        item8.BaselineStart = DateTime.Today.Add(TimeSpan.Parse("12:00:00"))
        item8.BaselineFinish = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("14:00:00"))

        Dim item9 As GanttChartItem = GanttChartDataGrid.Items(9)
        item9.Start = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("08:00:00"))
        item9.Finish = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"))
        item9.AssignmentsContent = "Resource 1"
        item9.BaselineStart = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"))
        item9.BaselineFinish = DateTime.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"))

        ' You may uncomment the next lines of code to set default schedule and timeline visibility settings for the Gantt Chart.
        '' Working week: between Tuesday and Saturday.
        'GanttChartDataGrid.WorkingWeekStart = DayOfWeek.Tuesday
        'GanttChartDataGrid.WorkingWeekFinish = DayOfWeek.Saturday
        '' Working day: between 9 AM and 5 PM.
        'GanttChartDataGrid.VisibleDayStart = TimeOfDay.Parse("09:00:00")
        'GanttChartDataGrid.WorkingDayStart = TimeOfDay.Parse("09:00:00")
        'GanttChartDataGrid.VisibleDayFinish = TimeOfDay.Parse("17:00:00")
        'GanttChartDataGrid.WorkingDayFinish = TimeOfDay.Parse("17:00:00")
        '' Optionally, generic nonworking intervals.
        'GanttChartDataGrid.NonworkingIntervals = New ObservableCollection(Of TimeInterval)(New TimeInterval() {New TimeInterval(DateTime.Today.AddDays(1), DateTime.Today.AddDays(1).Add(TimeOfDay.MaxValue)), New TimeInterval(DateTime.Today.AddDays(3), DateTime.Today.AddDays(5).Add(TimeSpan.Parse("12:00:00")))})
        '' Optionally, specific nonworking intervals based on date parameter: recurrent breaks and holidays accepted.
        'GanttChartDataGrid.NonworkingDayIntervalProvider = Function([date])
        '                                                       If [date].Day Mod 3 = 0 Then
        '                                                           Return New DayTimeInterval() {New DayTimeInterval(TimeOfDay.MinValue, TimeOfDay.Parse("12:00:00")), New DayTimeInterval(TimeOfDay.Parse("12:30:00"), TimeOfDay.Parse("13:00:00"))}
        '                                                       ElseIf [date].DayOfWeek <> DayOfWeek.Wednesday Then
        '                                                           Return New DayTimeInterval() {New DayTimeInterval(TimeOfDay.Parse("12:00:00"), TimeOfDay.Parse("13:00:00"))}
        '                                                       End If
        '                                                       Return Nothing
        '                                                   End Function
        '' Alternatively, add working day time breaks using AddWorkingDayTimeBreak or AddWorkingDayTimeBreaks methods.
        ' GanttChartDataGrid.AddWorkingDayTimeBreak(TimeOfDay.Parse("11:30"), TimeOfDay.Parse("12:30"))

        ' You may uncomment the next lines of code to set specific schedule settings for Task 3.
        'item8.Schedule = New Schedule(DayOfWeek.Sunday, DayOfWeek.Thursday, TimeSpan.Parse("10:00:00"), TimeSpan.Parse("14:00:00"), _
        '                              New TimeInterval() {New TimeInterval(DateTime.Today.AddDays(4), DateTime.Today.AddDays(4).Add(TimeOfDay.MaxValue)), New TimeInterval(DateTime.Today.AddDays(8), DateTime.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00")))}, Function([date])
        '                                                                                                                                                                                                                                                                If [date].Day Mod 10 = 0 Then
        '                                                                                                                                                                                                                                                                    Return New DayTimeInterval() {New DayTimeInterval(TimeOfDay.MinValue, TimeOfDay.Parse("12:00:00")), New DayTimeInterval(TimeOfDay.Parse("12:30:00"), TimeOfDay.Parse("13:00:00"))}
        '                                                                                                                                                                                                                                                                ElseIf [date].DayOfWeek <> DayOfWeek.Monday Then
        '                                                                                                                                                                                                                                                                    Return New DayTimeInterval() {New DayTimeInterval(TimeOfDay.Parse("12:00:00"), TimeOfDay.Parse("13:00:00"))}
        '                                                                                                                                                                                                                                                                End If
        '                                                                                                                                                                                                                                                                Return Nothing
        '                                                                                                                                                                                                                                                            End Function)
        ' GanttChartDataGrid.IsIndividualItemNonworkingTimeHighlighted = True

        ' You may uncomment the next lines of code to test the component performance:
        ' For i As Integer = 5 To 4096
        '    GanttChartDataGrid.Items.Add( _
        '        New GanttChartItem With _
        '        { _
        '            .Content = "Task " + i.ToString(), _
        '            .Start = DateTime.Today.AddDays(5 + i / 20), _
        '            .Finish = DateTime.Today.AddDays(5 + i / 10 + 1) _
        '        })
        ' Next

        ' Optionally, define assignable resources.
        GanttChartDataGrid.AssignableResources = New ObservableCollection(Of String) From {"Resource 1", "Resource 2", "Resource 3",
                                                                                           "Material 1", "Material 2"}

        ' Optionally, define the quantity values to consider when leveling resources, indicating maximum material amounts available for use at the same time.
        GanttChartDataGrid.ResourceQuantities = New Dictionary(Of String, Double) From {{"Material 1", 4}, {"Material 2", Double.PositiveInfinity}}
        item4.AssignmentsContent = "Material 1 [300%]"
        item6.AssignmentsContent = "Material 1 [250%], Material 2"

        ' Optionally, define task and resource costs.
        ' GanttChartDataGrid.TaskInitiationCost = 5
        item4.ExecutionCost = 50
        ' GanttChartDataGrid.DefaultResourceUsageCost = 1
        ' GanttChartDataGrid.SpecificResourceUsageCosts = New Dictionary(Of String, Double) From {{"Resource 1", 2}, {"Material 1", 7}}
        GanttChartDataGrid.DefaultResourceHourCost = 10
        GanttChartDataGrid.SpecificResourceHourCosts = New Dictionary(Of String, Double) From {{"Resource 1", 20}, {"Material 2", 0.5}}

        ' Optionally, set AreHierarchyConstraintsEnabled to false to increase performance when you perform hierarchy validation in your application logic.
        GanttChartDataGrid.AreHierarchyConstraintsEnabled = False

        ' Initialize the control area.
        ScalesComboBox.SelectedIndex = 0
        ShowWeekendsCheckBox.IsChecked = True
        BaselineCheckBox.IsChecked = True
        EnableDependencyConstraintsCheckBox.IsChecked = True
    End Sub

    Public Sub New(theme As String)
        Me.New()
        If theme Is Nothing Or theme = "Default" Or theme = "Aero" Then Return
        themeResourceDictionary = New ResourceDictionary With {.Source = New Uri("/" + Me.GetType().Assembly.GetName().Name + ";component/Themes/" + theme + ".xaml", UriKind.Relative)}
        GanttChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub

    Private themeResourceDictionary As ResourceDictionary

    ' Control area commands.
    Private Sub EditButton_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedItem As GanttChartItem = TryCast(GanttChartDataGrid.SelectedItem, GanttChartItem)
        If selectedItem Is Nothing Then
            MessageBox.Show("Cannot edit as the selection is empty; you should select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        Dim editItemDialog As New EditItemDialog With {.Owner = Application.Current.MainWindow, .DataContext = selectedItem}
        editItemDialog.ShowDialog()
    End Sub
    Private Sub AddNewButton_Click(sender As Object, e As RoutedEventArgs)
        Dim item As New GanttChartItem() With { _
         .Content = "New Task", _
         .Start = DateTime.Today, _
         .Finish = DateTime.Today.AddDays(1) _
        }
        GanttChartDataGrid.Items.Add(item)
        GanttChartDataGrid.SelectedItem = item
        GanttChartDataGrid.ScrollTo(item)
    End Sub
    Private Sub InsertNewButton_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedItem As GanttChartItem = TryCast(GanttChartDataGrid.SelectedItem, GanttChartItem)
        If selectedItem Is Nothing Then
            MessageBox.Show("Cannot insert a new item before selection as the selection is empty; you can either add a new item to the end of the list instead, or select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        Dim item As New GanttChartItem() With { _
         .Content = "New Task", _
         .Indentation = selectedItem.Indentation, _
         .Start = DateTime.Today, _
         .Finish = DateTime.Today.AddDays(1) _
        }
        GanttChartDataGrid.Items.Insert(GanttChartDataGrid.SelectedIndex, item)
        GanttChartDataGrid.SelectedItem = item
        GanttChartDataGrid.ScrollTo(item)
    End Sub
    Private Sub DeleteButton_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of GanttChartItem)()
        For Each item As GanttChartItem In GanttChartDataGrid.GetSelectedItems()
            items.Add(item)
        Next
        If items.Count <= 0 Then
            MessageBox.Show("Cannot delete the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        items.Reverse()
        ' If you have many items, you may use BeginInit and EndInit to avoid intermediate user interface updates.
        ' GanttChartDataGrid.BeginInit()
        For Each item As GanttChartItem In items
            If item.HasChildren Then
                MessageBox.Show(String.Format("Cannot delete {0} because it has child items; remove its child items first.", item), "Information", MessageBoxButton.OK)
                Continue For
            End If
            GanttChartDataGrid.Items.Remove(item)
        Next
        ' GanttChartDataGrid.EndInit()
    End Sub
    Private Sub IncreaseIndentationButton_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of GanttChartItem)()
        For Each item As GanttChartItem In GanttChartDataGrid.GetSelectedItems()
            items.Add(item)
        Next
        If items.Count <= 0 Then
            MessageBox.Show("Cannot increase indentation for the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        ' GanttChartDataGrid.BeginInit()
        For Each item As GanttChartItem In items
            Dim index As Integer = GanttChartDataGrid.IndexOf(item)
            If index > 0 Then
                Dim previousItem As GanttChartItem = GanttChartDataGrid(index - 1)
                If item.Indentation <= previousItem.Indentation Then
                    item.Indentation += 1
                Else
                    MessageBox.Show(String.Format("Cannot increase indentation for {0} because the previous item is its parent item; increase the indentation of its parent item first.", item), "Information", MessageBoxButton.OK)
                End If
            Else
                MessageBox.Show(String.Format("Cannot increase indentation for {0} because it is the first item; insert an item before this one first.", item), "Information", MessageBoxButton.OK)
            End If
        Next
        ' GanttChartDataGrid.EndInit()
    End Sub
    Private Sub DecreaseIndentationButton_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of GanttChartItem)()
        For Each item As GanttChartItem In GanttChartDataGrid.GetSelectedItems()
            items.Add(item)
        Next
        If items.Count <= 0 Then
            MessageBox.Show("Cannot decrease indentation for the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        items.Reverse()
        ' GanttChartDataGrid.BeginInit()
        For Each item As GanttChartItem In items
            If item.Indentation > 0 Then
                Dim index As Integer = GanttChartDataGrid.IndexOf(item)
                Dim nextItem As GanttChartItem = If(index < GanttChartDataGrid.Items.Count - 1, GanttChartDataGrid(index + 1), Nothing)
                If nextItem Is Nothing OrElse item.Indentation >= nextItem.Indentation Then
                    item.Indentation -= 1
                Else
                    MessageBox.Show(String.Format("Cannot increase indentation for {0} because the next item is one of its child items; decrease the indentation of its child items first.", item), "Information", MessageBoxButton.OK)
                End If
            Else
                MessageBox.Show(String.Format("Cannot decrease indentation for {0} because it is on the first indentation level, being a root item.", item), "Information", MessageBoxButton.OK)
            End If
        Next
        ' GanttChartDataGrid.EndInit()
    End Sub
    Private Sub SetColorButton_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of GanttChartItem)()
        For Each item As GanttChartItem In GanttChartDataGrid.GetSelectedItems()
            items.Add(item)
        Next
        If items.Count <= 0 Then
            MessageBox.Show("Cannot set a custom bar color to the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        For Each item As GanttChartItem In items
            GanttChartView.SetStandardBarFill(item, TryCast(Resources("CustomStandardBarFill"), Brush))
            GanttChartView.SetStandardBarStroke(item, TryCast(Resources("CustomStandardBarStroke"), Brush))
            If item.HasChildren OrElse item.IsMilestone Then
                MessageBox.Show(String.Format("The custom bar color was set to {0}, but its appearance won't change until it becomes a standard task (currently it is a {1} task).", item, If(item.HasChildren, "summary", "milestone")), "Information", MessageBoxButton.OK)
                Continue For
            End If
        Next
    End Sub
    Private Sub CopyButton_Click(sender As Object, e As RoutedEventArgs)
        If (GanttChartDataGrid.GetSelectedItemCount() <= 0) Then
            MessageBox.Show("Cannot copy selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
        End If
        GanttChartDataGrid.Copy()
    End Sub
    Private Sub PasteButton_Click(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.Paste()
    End Sub
    Private Sub MoveUpButton_Click(sender As Object, e As RoutedEventArgs)
        If GanttChartDataGrid.GetSelectedItemCount() <= 0 Then
            MessageBox.Show("Cannot move selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        Dim selectedItems = GanttChartDataGrid.GetSelectedItems().ToArray()
        If selectedItems.Select(Function(i) i.Parent).Distinct().Count() > 1 Then
            MessageBox.Show("Cannot move selected item(s) as the selection includes items that have different parents; select only root items or only items having the same parent.", "Information", MessageBoxButton.OK)
            Return
        End If
        ' GanttChartDataGrid.BeginInit()
        For Each item As GanttChartItem In selectedItems
            GanttChartDataGrid.MoveUp(item, True, True)
        Next
        ' GanttChartDataGrid.EndInit()
        GanttChartDataGrid.SelectedItems.Clear()
        For Each item As GanttChartItem In selectedItems
            GanttChartDataGrid.SelectedItems.Add(item)
        Next
    End Sub
    Private Sub MoveDownButton_Click(sender As Object, e As RoutedEventArgs)
        If GanttChartDataGrid.GetSelectedItemCount() <= 0 Then
            MessageBox.Show("Cannot move selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        Dim selectedItems = GanttChartDataGrid.GetSelectedItems().Reverse().ToArray()
        If selectedItems.Select(Function(i) i.Parent).Distinct().Count() > 1 Then
            MessageBox.Show("Cannot move selected item(s) as the selection includes items that have different parents; select only root items or only items having the same parent.", "Information", MessageBoxButton.OK)
            Return
        End If
        ' GanttChartDataGrid.BeginInit()
        For Each item As GanttChartItem In selectedItems
            GanttChartDataGrid.MoveDown(item, True, True)
        Next
        ' GanttChartDataGrid.EndInit()
        GanttChartDataGrid.SelectedItems.Clear()
        For Each item As GanttChartItem In selectedItems
            GanttChartDataGrid.SelectedItems.Add(item)
        Next
    End Sub
    Private Sub UndoButton_Click(sender As Object, e As RoutedEventArgs)
        If GanttChartDataGrid.CanUndo() Then
            GanttChartDataGrid.Undo()
        Else
            MessageBox.Show("Currently there is no recorded action in the undo queue; perform an action first.", "Information", MessageBoxButton.OK)
        End If
    End Sub
    Private Sub RedoButton_Click(sender As Object, e As RoutedEventArgs)
        If GanttChartDataGrid.CanRedo() Then
            GanttChartDataGrid.Redo()
        Else
            MessageBox.Show("Currently there is no recorded action in the redo queue; perform an action and undo it first.", "Information", MessageBoxButton.OK)
        End If
    End Sub
    Private Sub ScaleTypeComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim selectedComboBoxItem As ComboBoxItem = TryCast(ScalesComboBox.SelectedItem, ComboBoxItem)
        Dim scalesResourceKey As String = TryCast(selectedComboBoxItem.Tag, String)
        Dim scales As ScaleCollection = TryCast(Resources(scalesResourceKey), ScaleCollection)
        GanttChartDataGrid.Scales = scales
    End Sub
    Private Sub ZoomCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        originalZoom = GanttChartDataGrid.HourWidth
        GanttChartDataGrid.HourWidth = originalZoom * 2
    End Sub
    Private Sub ZoomCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.HourWidth = originalZoom
    End Sub
    Private originalZoom As Double
    Private Sub IncreaseTimelinePageButton_Click(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.TimelinePageFinish += pageUpdateAmount
        GanttChartDataGrid.TimelinePageStart += pageUpdateAmount
    End Sub
    Private Sub DecreaseTimelinePageButton_Click(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.TimelinePageFinish -= pageUpdateAmount
        GanttChartDataGrid.TimelinePageStart -= pageUpdateAmount
    End Sub
    Private Sub FitToTimelinePageCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        ZoomCheckBox.IsChecked = False
        ZoomCheckBox.IsEnabled = False
        GanttChartDataGrid.IsFittingToTimelinePageEnabled = True
    End Sub
    Private Sub FitToTimelinePageCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.IsFittingToTimelinePageEnabled = False
        ZoomCheckBox.IsEnabled = True
    End Sub
    Private ReadOnly pageUpdateAmount As TimeSpan = TimeSpan.FromDays(7)
    Private Sub ShowWeekendsCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.VisibleWeekStart = DayOfWeek.Sunday
        GanttChartDataGrid.VisibleWeekFinish = DayOfWeek.Saturday
        WorkOnWeekendsCheckBox.IsEnabled = True
    End Sub
    Private Sub ShowWeekendsCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        WorkOnWeekendsCheckBox.IsChecked = False
        WorkOnWeekendsCheckBox.IsEnabled = False
        GanttChartDataGrid.VisibleWeekStart = DayOfWeek.Monday
        GanttChartDataGrid.VisibleWeekFinish = DayOfWeek.Friday
    End Sub
    Private Sub WorkOnWeekendsCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.WorkingWeekStart = DayOfWeek.Sunday
        GanttChartDataGrid.WorkingWeekFinish = DayOfWeek.Saturday
    End Sub
    Private Sub WorkOnWeekendsCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.WorkingWeekStart = DayOfWeek.Monday
        GanttChartDataGrid.WorkingWeekFinish = DayOfWeek.Friday
    End Sub
    Private Sub BaselineCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.IsBaselineVisible = True
    End Sub
    Private Sub BaselineCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.IsBaselineVisible = False
    End Sub
    Private Sub CriticalPathCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        EditButton.IsEnabled = False
        AddNewButton.IsEnabled = False
        InsertNewButton.IsEnabled = False
        IncreaseIndentationButton.IsEnabled = False
        DecreaseIndentationButton.IsEnabled = False
        DeleteButton.IsEnabled = False
        PasteButton.IsEnabled = False
        UndoButton.IsEnabled = False
        RedoButton.IsEnabled = False
        SetColorButton.IsEnabled = False
        SplitRemainingWorkButton.IsEnabled = False
        LevelResourcesButton.IsEnabled = False
        EnableDependencyConstraintsCheckBox.IsEnabled = False
        ScheduleChartButton.IsEnabled = False
        LoadProjectXmlButton.IsEnabled = False
        GanttChartDataGrid.IsReadOnly = True
        Dim criticalDelay As TimeSpan = TimeSpan.FromHours(8)
        For Each item As GanttChartItem In GanttChartDataGrid.Items
            If item.HasChildren Then Continue For
            SetCriticalPathHighlighting(item, GanttChartDataGrid.IsCritical(item, criticalDelay))
        Next
    End Sub
    Private Sub CriticalPathCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        For Each item As GanttChartItem In GanttChartDataGrid.Items
            If item.HasChildren Then Continue For
            SetCriticalPathHighlighting(item, False)
        Next
        GanttChartDataGrid.IsReadOnly = False
        EditButton.IsEnabled = True
        AddNewButton.IsEnabled = True
        InsertNewButton.IsEnabled = True
        IncreaseIndentationButton.IsEnabled = True
        DecreaseIndentationButton.IsEnabled = True
        DeleteButton.IsEnabled = True
        PasteButton.IsEnabled = True
        UndoButton.IsEnabled = True
        RedoButton.IsEnabled = True
        SetColorButton.IsEnabled = True
        SplitRemainingWorkButton.IsEnabled = True
        LevelResourcesButton.IsEnabled = True
        EnableDependencyConstraintsCheckBox.IsEnabled = True
        ScheduleChartButton.IsEnabled = True
        LoadProjectXmlButton.IsEnabled = True
    End Sub
    Private Sub SetCriticalPathHighlighting(item As GanttChartItem, isHighlighted As Boolean)
        If Not item.IsMilestone Then
            GanttChartView.SetStandardBarFill(item, If(isHighlighted, TryCast(Resources("CustomStandardBarFill"), Brush), GanttChartDataGrid.StandardBarFill))
            GanttChartView.SetStandardBarStroke(item, If(isHighlighted, TryCast(Resources("CustomStandardBarStroke"), Brush), GanttChartDataGrid.StandardBarStroke))
        Else
            GanttChartView.SetMilestoneBarFill(item, If(isHighlighted, TryCast(Resources("CustomStandardBarFill"), Brush), GanttChartDataGrid.MilestoneBarFill))
            GanttChartView.SetMilestoneBarStroke(item, If(isHighlighted, TryCast(Resources("CustomStandardBarStroke"), Brush), GanttChartDataGrid.MilestoneBarStroke))
        End If
    End Sub
    Private Sub SplitRemainingWorkButton_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedItem As GanttChartItem = GanttChartDataGrid.SelectedItem
        If selectedItem Is Nothing Or selectedItem.HasChildren Or selectedItem.IsMilestone Or Not selectedItem.HasStarted Or selectedItem.IsCompleted Then
            MessageBox.Show("Cannot split work as the selection is empty or the selected item does not represent a standard task in progress; you should select an appropriate item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        GanttChartDataGrid.SplitRemainingWork(selectedItem, " (rem. work)", " (compl. work)")
    End Sub
    Private Sub ScheduleChartButton_Click(sender As Object, e As RoutedEventArgs)
        Dim originalOpacity As Double = Opacity
        Opacity = 0.5
        GanttChartDataGrid.UnassignedScheduleChartItemContent = "(Unassigned)" ' Optional
        Dim scheduleChartItems As ObservableCollection(Of ScheduleChartItem) = GanttChartDataGrid.GetScheduleChartItems()
        Dim scheduleChartWindow As New Window() With { _
         .Owner = Application.Current.MainWindow, .Title = "Schedule Chart", _
         .Width = 640, .Height = 480,
         .WindowStartupLocation = WindowStartupLocation.CenterOwner, .ResizeMode = ResizeMode.CanResize, _
         .Content = New ScheduleChartDataGrid() With { _
          .Items = scheduleChartItems, .DataGridWidth = New GridLength(0.2, GridUnitType.Star), _
          .UseMultipleLinesPerRow = True, .AreIndividualItemAppearanceSettingsApplied = True, .IsAlternatingItemBackgroundInverted = True, .UnassignedScheduleChartItemContent = GanttChartDataGrid.UnassignedScheduleChartItemContent _
         } _
        }
        If Not themeResourceDictionary Is Nothing Then CType(scheduleChartWindow.Content, FrameworkElement).Resources.MergedDictionaries.Add(themeResourceDictionary)
        scheduleChartWindow.ShowDialog()
        GanttChartDataGrid.UpdateChangesFromScheduleChartItems(scheduleChartItems)
        GanttChartDataGrid.DisposeScheduleChartItems(scheduleChartItems)
        Opacity = originalOpacity
    End Sub
    Private Sub LoadChartButton_Click(sender As Object, e As RoutedEventArgs)
        Dim originalOpacity As Double = Opacity
        Opacity = 0.5
        Dim loadChartItems As ObservableCollection(Of LoadChartItem) = GanttChartDataGrid.GetLoadChartItems()
        selectedLoadChartItemContainer = New ObservableCollection(Of LoadChartItem)
        loadChartResourceComboBox = New ComboBox With {.ItemsSource = loadChartItems, .DisplayMemberPath = "Content", .Margin = New Thickness(4)}
        AddHandler loadChartResourceComboBox.SelectionChanged, AddressOf LoadChartResourceComboBoxSelectionChanged
        If loadChartResourceComboBox.Items.Count > 0 Then loadChartResourceComboBox.SelectedIndex = 0
        Dim dockPanel As New DockPanel()
        dockPanel.Children.Add(loadChartResourceComboBox)
        dockPanel.SetDock(loadChartResourceComboBox, Dock.Top)
        dockPanel.Children.Add(New LoadChartView() With {
          .Items = selectedLoadChartItemContainer,
          .ItemHeight = 176, .BarHeight = 172, .Height = 230, .Margin = New Thickness(4, 0, 4, 4), .VerticalAlignment = VerticalAlignment.Top
        })
        Dim loadChartWindow As New Window() With {
         .Owner = Application.Current.MainWindow, .Title = "Load Chart",
         .Width = 640, .Height = 300,
         .WindowStartupLocation = WindowStartupLocation.CenterOwner, .ResizeMode = ResizeMode.CanMinimize,
         .Content = dockPanel
        }
        If Not themeResourceDictionary Is Nothing Then
            Dim loadChartView = CType(dockPanel.Children(dockPanel.Children.Count - 1), LoadChartView)
            loadChartView.Resources.MergedDictionaries.Add(themeResourceDictionary)
            loadChartView.BarHeight = loadChartView.BarHeight - 21
            loadChartView.ItemHeight = loadChartView.ItemHeight - 21
        End If
        loadChartWindow.ShowDialog()
        GanttChartDataGrid.DisposeLoadChartItems(loadChartItems)
        Opacity = originalOpacity
    End Sub
    Private loadChartResourceComboBox As ComboBox
    Private selectedLoadChartItemContainer As ObservableCollection(Of LoadChartItem)
    Private Sub LoadChartResourceComboBoxSelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        selectedLoadChartItemContainer.Clear()
        selectedLoadChartItemContainer.Add(TryCast(loadChartResourceComboBox.SelectedItem, LoadChartItem))
    End Sub
    Private Sub EditResourcesButton_Click(sender As Object, e As RoutedEventArgs)
        Dim originalOpacity As Double = Opacity
        Opacity = 0.5
        Dim resourceItems = GanttChartDataGrid.AssignableResources
        Dim dockPanel = New DockPanel()
        Dim resourceListBox = New ListBox() With {.ItemsSource = resourceItems, .SelectionMode = SelectionMode.Extended, .TabIndex = 2, .Margin = New Thickness(4)}
        Dim commandsDockPanel = New DockPanel() With {.Margin = New Thickness(4, 0, 4, 4)}
        DockPanel.SetDock(commandsDockPanel, Dock.Bottom)
        dockPanel.Children.Add(commandsDockPanel)
        Dim newResourceTextBox = New TextBox() With {.TabIndex = 0, .Margin = New Thickness(0, 0, 4, 0)}
        Dim addResourceButton = New Button() With {.Content = "Add", .IsDefault = True, .TabIndex = 1, .Margin = New Thickness(0, 0, 4, 0)}
        DockPanel.SetDock(addResourceButton, Dock.Right)
        Dim deleteResourcesButton = New Button() With {.Content = "Delete", .TabIndex = 3, .Margin = New Thickness(4, 0, 0, 0)}
        DockPanel.SetDock(deleteResourcesButton, Dock.Right)
        commandsDockPanel.Children.Add(deleteResourcesButton)
        commandsDockPanel.Children.Add(addResourceButton)
        commandsDockPanel.Children.Add(newResourceTextBox)
        dockPanel.Children.Add(resourceListBox)
        AddHandler addResourceButton.Click,
            Sub()
                Dim newResource = newResourceTextBox.Text
                If Not String.IsNullOrEmpty(newResource) AndAlso Not resourceItems.Contains(newResource) Then
                    resourceItems.Add(newResource)
                End If
                newResourceTextBox.Clear()
                resourceListBox.SelectedItem = newResource
            End Sub
        AddHandler deleteResourcesButton.Click,
            Sub()
                Dim removedResources As New List(Of String)()
                For Each resource As String In resourceListBox.SelectedItems
                    removedResources.Add(resource)
                Next
                For Each resource As String In removedResources
                    resourceItems.Remove(resource)
                Next
                newResourceTextBox.Clear()
            End Sub
        Dim resourceWindow As New Window() With {
         .Owner = Application.Current.MainWindow, .Title = "Resources",
         .Width = 640, .Height = 300,
         .WindowStartupLocation = WindowStartupLocation.CenterOwner, .ResizeMode = ResizeMode.CanMinimize,
         .Content = dockPanel
        }
        resourceWindow.ShowDialog()
        Opacity = originalOpacity
    End Sub
    Private Sub PertChartButton_Click(sender As Object, e As RoutedEventArgs)
        Dim originalOpacity As Double = Opacity
        Opacity = 0.5
        ' Optionally, specify a maximum indentation level to consider when generating PERT items as a parameter to the GetPertChartItems method call.
        Dim pertChartItems As ObservableCollection(Of DlhSoft.Windows.Controls.Pert.PertChartItem) = GanttChartDataGrid.GetPertChartItems()
        pertChartView = New Pert.PertChartView() With { _
          .Items = pertChartItems, _
          .PredecessorToolTipTemplate = TryCast(Resources("PertChartPredecessorToolTipTemplate"), DataTemplate) _
        }
        Dim pertChartWindow As New Window() With {
         .Owner = Application.Current.MainWindow, .Title = "PERT Chart",
         .Width = 640, .Height = 480,
         .WindowStartupLocation = WindowStartupLocation.CenterOwner, .ResizeMode = ResizeMode.CanResize,
         .Content = pertChartView
        }
        ' Optionally, highlight the critical path.
        AddHandler pertChartView.AsyncPresentationCompleted, TryCast(AddressOf PertChartViewHighlightCriticalPathInternal, EventHandler)
        If Not themeResourceDictionary Is Nothing Then CType(pertChartWindow.Content, FrameworkElement).Resources.MergedDictionaries.Add(themeResourceDictionary)
        pertChartWindow.ShowDialog()
        GanttChartDataGrid.DisposePertChartItems(pertChartItems)
        Opacity = originalOpacity
    End Sub
    Private Sub PertChartViewHighlightCriticalPathInternal(sender As Object, e As EventArgs)
        For Each item In pertChartView.GetCriticalItems()
            Pert.PertChartView.SetShapeStroke(item, Brushes.Red)
            Pert.PertChartView.SetTextForeground(item, Brushes.Red)
        Next
        For Each predecessorItem In pertChartView.GetCriticalDependencies()
            Pert.PertChartView.SetDependencyLineStroke(predecessorItem, Brushes.Red)
            Pert.PertChartView.SetDependencyTextForeground(predecessorItem, Brushes.Red)
        Next
    End Sub
    Private pertChartView As Pert.PertChartView
    Private Sub NetworkDiagramButton_Click(sender As Object, e As RoutedEventArgs)
        Dim originalOpacity As Double = Opacity
        Opacity = 0.5
        ' Optionally, specify a maximum indentation level to consider when generating network items as a parameter to the GetNetworkDiagramItems method call.
        Dim networkDiagramItems As ObservableCollection(Of DlhSoft.Windows.Controls.Pert.NetworkDiagramItem) = GanttChartDataGrid.GetNetworkDiagramItems()
        networkDiagramView = New Pert.NetworkDiagramView() With { _
          .Items = networkDiagramItems _
        }
        Dim networkDiagramWindow As New Window() With {
         .Owner = Application.Current.MainWindow, .Title = "Network Diagram",
         .Width = 960, .Height = 600,
         .WindowStartupLocation = WindowStartupLocation.CenterOwner, .ResizeMode = ResizeMode.CanResize,
         .Content = networkDiagramView
        }
        ' Optionally, reposition start and finish milestones between the first and second rows of the view.
        AddHandler networkDiagramView.AsyncPresentationCompleted, TryCast(AddressOf NetworkDiagramViewRepositionEndsInternal, EventHandler)
        ' Optionally, highlight the critical path.
        AddHandler networkDiagramView.AsyncPresentationCompleted, TryCast(AddressOf NetworkDiagramViewHighlightCriticalPathInternal, EventHandler)
        If Not themeResourceDictionary Is Nothing Then CType(networkDiagramWindow.Content, FrameworkElement).Resources.MergedDictionaries.Add(themeResourceDictionary)
        networkDiagramWindow.ShowDialog()
        GanttChartDataGrid.DisposeNetworkDiagramItems(networkDiagramItems)
        Opacity = originalOpacity
    End Sub
    Private Sub NetworkDiagramViewRepositionEndsInternal(sender As Object, e As EventArgs)
        networkDiagramView.RepositionEnds()
    End Sub
    Private Sub NetworkDiagramViewHighlightCriticalPathInternal(sender As Object, e As EventArgs)
        For Each item In networkDiagramView.GetCriticalItems()
            Pert.NetworkDiagramView.SetShapeStroke(item, Brushes.Red)
        Next
        For Each predecessorItem In networkDiagramView.GetCriticalDependencies()
            Pert.NetworkDiagramView.SetDependencyLineStroke(predecessorItem, Brushes.Red)
        Next
    End Sub
    Private networkDiagramView As Pert.NetworkDiagramView
    Private Sub LevelResourcesButton_Click(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.LevelResources()
    End Sub
    Private Sub ProjectStatisticsButton_Click(sender As Object, e As RoutedEventArgs)
        Dim statistics = String.Format("Start:" & vbTab & "{0:d}" & vbCrLf & "Finish:" & vbTab & "{1:d}" & vbCrLf & "Effort:" & vbTab & "{2:0.##}h" & vbCrLf & "Compl.:" & vbTab & "{3:0.##%}" & vbCrLf & "Cost:" & vbTab & "${4:0.##}", _
                                       GanttChartDataGrid.GetProjectStart(), GanttChartDataGrid.GetProjectFinish(), _
                                       GanttChartDataGrid.GetProjectEffort().TotalHours, GanttChartDataGrid.GetProjectCompletion(), _
                                       GanttChartDataGrid.GetProjectCost())
        MessageBox.Show(statistics, "Project statistics", MessageBoxButton.OK, MessageBoxImage.Information)
    End Sub
    Private Sub EnableDependencyConstraintsCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.AreTaskDependencyConstraintsEnabled = True
    End Sub
    Private Sub EnableDependencyConstraintsCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.AreTaskDependencyConstraintsEnabled = False
    End Sub
    Private Sub SaveProjectXmlButton_Click(sender As Object, e As RoutedEventArgs)
        ' Select a Project XML file to save data to.
        Dim saveFileDialog As New SaveFileDialog With {.Title = "Save Project XML", .Filter = "Project XML files|*.xml", .DefaultExt = ".xml"}
        If saveFileDialog.ShowDialog() <> True Then Return
        Dim assignableResources = GanttChartDataGrid.AssignableResources
        Using stream As Stream = saveFileDialog.OpenFile()
            GanttChartDataGrid.SaveProjectXml(stream)
        End Using
    End Sub
    Private Sub LoadProjectXmlButton_Click(sender As Object, e As RoutedEventArgs)
        ' Select a Project XML file to load data from.
        Dim openFileDialog As New OpenFileDialog With {.Title = "Load Project XML", .Filter = "Project XML files|*.xml", .Multiselect = False}
        If openFileDialog.ShowDialog() <> True Then Return
        Dim assignableResources = GanttChartDataGrid.AssignableResources
        Using stream As Stream = openFileDialog.OpenFile()
            GanttChartDataGrid.LoadProjectXml(stream)
        End Using
    End Sub
    Private Sub PrintButton_Click(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.Print("GanttChartDataGrid Sample Document")
    End Sub
    Private Sub ExportImageButton_Click(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.Export(TryCast(AddressOf ExportImageInternal, Action))
    End Sub
    Private Sub ExportImageInternal()
        Dim saveFileDialog As New Microsoft.Win32.SaveFileDialog() With { _
         .Title = "Export Image To", _
         .Filter = "PNG image files|*.png" _
        }
        If saveFileDialog.ShowDialog() <> True Then Return
        Dim bitmapSource As BitmapSource = GanttChartDataGrid.GetExportBitmapSource(96 * 2)
        Using stream As IO.Stream = saveFileDialog.OpenFile()
            Dim pngBitmapEncoder As New PngBitmapEncoder()
            pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource))
            pngBitmapEncoder.Save(stream)
        End Using
    End Sub
End Class
