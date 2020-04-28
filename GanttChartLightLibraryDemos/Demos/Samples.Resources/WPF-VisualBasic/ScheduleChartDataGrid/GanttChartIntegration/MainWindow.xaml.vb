Imports DlhSoft.Windows.Controls
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

        Dim item8 As GanttChartItem = GanttChartDataGrid.Items(8)
        item8.Start = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))
        item8.Finish = Date.Today.AddDays(6).Add(TimeSpan.Parse("14:00:00"))
        item8.AssignmentsContent = "Resource 1 [50%], Resource 2 [75%]"

        Dim item9 As GanttChartItem = GanttChartDataGrid.Items(9)
        item9.Start = Date.Today.AddDays(6).Add(TimeSpan.Parse("08:00:00"))
        item9.Finish = Date.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"))
        item9.AssignmentsContent = "Resource 1"

        Dim item10 As GanttChartItem = GanttChartDataGrid.Items(10)
        item10.Start = Date.Today.AddDays(7).Add(TimeSpan.Parse("08:00:00"))
        item10.Finish = Date.Today.AddDays(28).Add(TimeSpan.Parse("16:00:00"))
        item10.Predecessors.Add(New PredecessorItem With {.Item = item9})

        For i As Integer = 3 To 25
            GanttChartDataGrid.Items.Add(New GanttChartItem With {.Content = "Task " & i, .Indentation = If(i Mod 3 = 0, 0, 1), .Start = Date.Today.AddDays(If(i <= 8, (i - 4) * 2, i - 8)), .Finish = Date.Today.AddDays((If(i <= 8, (i - 4) * 2 + (If(i > 8, 6, 1)), i - 2)) + 2), .CompletedFinish = Date.Today.AddDays(If(i <= 8, (i - 4) * 2, i - 8)).AddDays(If(i Mod 6 = 4, 3, 0))})
        Next i
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

    Private Sub AddNewButton_Click(sender As Object, e As RoutedEventArgs)
        Dim item As GanttChartItem = New GanttChartItem With {.Content = "New Task", .Start = Date.Today, .Finish = Date.Today.AddDays(1)}
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
        Dim item As GanttChartItem = New GanttChartItem With {.Content = "New Task", .Indentation = selectedItem.Indentation, .Start = Date.Today, .Finish = Date.Today.AddDays(1)}
        GanttChartDataGrid.Items.Insert(selectedItem.Index, item)
        GanttChartDataGrid.SelectedItem = item
        GanttChartDataGrid.ScrollTo(item)
    End Sub
    Private Sub DeleteButton_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of GanttChartItem)()
        For Each item As GanttChartItem In GanttChartDataGrid.GetSelectedItems()
            items.Add(item)
        Next item
        If items.Count <= 0 Then
            MessageBox.Show("Cannot delete the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        items.Reverse()
        GanttChartDataGrid.BeginInit()
        For Each item As GanttChartItem In items
            If item.HasChildren Then
                MessageBox.Show(String.Format("Cannot delete {0} because it has child items; remove its child items first.", item), "Information", MessageBoxButton.OK)
                Continue For
            End If
            GanttChartDataGrid.Items.Remove(item)
        Next item
        GanttChartDataGrid.EndInit()
    End Sub

    ' Show a Schedule Chart in a dialog window based on Gantt Chart items.
    ' Upon closing the dialog, update Gantt Chart items according to the changes done in the Schedule Chart.
    Private Sub ScheduleChartButton_Click(sender As Object, e As RoutedEventArgs)
        Dim originalOpacity As Double = Opacity
        Opacity = 0.5
        GanttChartDataGrid.UnassignedScheduleChartItemContent = "(Unassigned)" ' Optional
        Dim scheduleChartItems As ObservableCollection(Of ScheduleChartItem) = GanttChartDataGrid.GetScheduleChartItems()
        Dim scheduleChartWindow As Window = New Window With {.Owner = Application.Current.MainWindow, .Title = "Schedule Chart", .WindowStartupLocation = WindowStartupLocation.CenterOwner, .Width = 1280, .Height = 480, .ResizeMode = ResizeMode.CanResize, .Content = New DlhSoft.Windows.Controls.ScheduleChartDataGrid With {.Items = scheduleChartItems, .DataGridWidth = New GridLength(0.2, GridUnitType.Star), .UseMultipleLinesPerRow = True, .AreIndividualItemAppearanceSettingsApplied = True, .IsAlternatingItemBackgroundInverted = True, .UnassignedScheduleChartItemContent = GanttChartDataGrid.UnassignedScheduleChartItemContent}}
        If themeResourceDictionary IsNot Nothing Then
            TryCast(scheduleChartWindow.Content, FrameworkElement).Resources.MergedDictionaries.Add(themeResourceDictionary)
        End If
        scheduleChartWindow.ShowDialog()
        GanttChartDataGrid.UpdateChangesFromScheduleChartItems(scheduleChartItems)
        GanttChartDataGrid.DisposeScheduleChartItems(scheduleChartItems)
        Opacity = originalOpacity
    End Sub
End Class
