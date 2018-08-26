Imports DlhSoft.Windows.Controls
Imports System.Collections.Specialized
Imports System.Collections.ObjectModel

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

        GanttChartDataGrid.AssignableResources = New ObservableCollection(Of String) From {"Resource 1", "Resource 2", "Resource 3"}
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

    Private Sub ReadOnlyCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.IsReadOnly = (ReadOnlyCheckBox.IsChecked = True)
    End Sub

    Private Sub GridReadOnlyCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.DataTreeGrid.IsReadOnly = (GridReadOnlyCheckBox.IsChecked = True)
    End Sub

    Private Sub ChartReadOnlyCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.GanttChartView.IsReadOnly = (ChartReadOnlyCheckBox.IsChecked = True)
    End Sub

    Private Sub SetReadOnlyStateForColumns(columnNames() As String, isReadOnly As Boolean)
        For Each column In GanttChartDataGrid.Columns.Where(Function(c) columnNames.Contains(TryCast(c.Header, String)))
            column.IsReadOnly = isReadOnly
        Next column
    End Sub

    Private Sub TaskNameColumnReadOnlyCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        Dim isReadOnly As Boolean = (TaskNameColumnReadOnlyCheckBox.IsChecked = True)
        SetReadOnlyStateForColumns({"Task"}, isReadOnly)
    End Sub

    Private Sub SchedulingColumnsReadOnlyCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        Dim isReadOnly As Boolean = (SchedulingColumnsReadOnlyCheckBox.IsChecked = True)
        SetReadOnlyStateForColumns({"Start", "Finish", "Milestone", "Compl."}, isReadOnly)
    End Sub

    Private Sub AssignmentsColumnReadOnlyCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        Dim isReadOnly As Boolean = (AssignmentsColumnReadOnlyCheckBox.IsChecked = True)
        SetReadOnlyStateForColumns({"Assignments"}, isReadOnly)
    End Sub

    Private Sub EffortIsPreservedWhenChangingStartFromGridCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        '..In chart effort is not preserved
        Dim columnStart = GanttChartDataGrid.Columns.Single(Function(c) TryCast(c.Header, String) = "Start")
        Dim columnStartPreservingEffort = GanttChartDataGrid.Columns.Single(Function(c) TryCast(c.Header, String) = "Start Preserving Effort")

        If EffortIsPreservedWhenChangingStartFromGridCheckBox.IsChecked = True Then
            columnStart.Visibility = Visibility.Collapsed
            columnStartPreservingEffort.Visibility = Visibility.Visible
        Else
            columnStart.Visibility = Visibility.Visible
            columnStartPreservingEffort.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Sub StartReadOnlyInChartCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.IsTaskStartReadOnly = (StartReadOnlyInChartCheckBox.IsChecked = True)
    End Sub

    Private Sub CompletionReadOnlyInChartCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.IsTaskCompletionReadOnly = (CompletionReadOnlyInChartCheckBox.IsChecked = True)
    End Sub

    Private Sub DurationReadOnlyInChartCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.IsTaskFinishReadOnly = (DurationReadOnlyInChartCheckBox.IsChecked = True)
        If GanttChartDataGrid.IsTaskFinishReadOnly Then
            DisableUpdatingDurationByStartDraggingCheckBox.IsChecked = True
        End If
    End Sub

    Private Sub DisableUpdatingDurationByStartDraggingCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.IsDraggingTaskStartEndsEnabled = (DisableUpdatingDurationByStartDraggingCheckBox.IsChecked = False)
        If GanttChartDataGrid.IsDraggingTaskStartEndsEnabled Then
            DurationReadOnlyInChartCheckBox.IsChecked = False
        End If
    End Sub

    Private Sub HideDependenciesCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.AreTaskDependenciesVisible = (HideDependenciesCheckBox.IsChecked = False)
    End Sub

    Private Sub DisableCreatingStartDependenciesCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.AllowCreatingStartDependencies = (DisableCreatingStartDependenciesCheckBox.IsChecked = False)
    End Sub

    Private Sub DisableCreatingToFinishDependenciesCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.AllowCreatingToFinishDependencies = (DisableCreatingToFinishDependenciesCheckBox.IsChecked = False)
    End Sub

    Private Sub DisableChartScrollingOnGridRowClickingCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.IsRowClickTimeScrollingEnabled = (DisableChartScrollingOnGridRowClickingCheckBox.IsChecked = False)
    End Sub

    Private Sub SetChartBarItemAsReadOnly_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of GanttChartItem)()
        For Each item As GanttChartItem In GanttChartDataGrid.GetSelectedItems()
            items.Add(item)
        Next item
        If items.Count <= 0 Then
            MessageBox.Show("Please select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        For Each item As GanttChartItem In items
            item.IsBarReadOnly = True
            GanttChartView.SetStandardBarFill(item, Brushes.Green)
            GanttChartView.SetStandardBarStroke(item, Brushes.Green)
            If item.IsMilestone Then
                GanttChartView.SetMilestoneBarFill(item, Brushes.YellowGreen)
            End If
            If item.IsSummaryEnabled Then
                GanttChartView.SetSummaryBarFill(item, Brushes.DarkBlue)
                GanttChartView.SetSummaryBarStroke(item, Brushes.DarkBlue)
            End If
        Next item
    End Sub
End Class
