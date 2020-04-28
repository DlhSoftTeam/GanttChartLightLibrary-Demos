Imports System.Text
Imports DlhSoft.Windows.Controls
Imports Microsoft.Win32
Imports System.IO
Imports DlhSoft.Windows.Data
Imports System.Collections.ObjectModel
Imports System.Windows.Threading

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

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
                item.GanttChartItems.Add(New GanttChartItem With {.Content = "Task " & i & "." & j, .Start = Date.Today.AddDays(i + (i - 1) * (j - 1)), .Finish = Date.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1), .CompletedFinish = Date.Today.AddDays(i + (i - 1) * (j - 1)).AddDays(If((i + j) Mod 5 = 2, 2, 0))})
            Next j
            ScheduleChartDataGrid.Items.Add(item)
        Next i

        ' You may uncomment the next lines of code to test the component performance:
        ' for (int i = 17; i <= 1024; i++)
        ' {
        '    ScheduleChartItem item = new ScheduleChartItem { Content = "Resource " + i };
        '    for (int j = 1; j <= (i - 1) % 4 + 1; j++)
        '    {
        '        item.GanttChartItems.Add(
        '            new GanttChartItem
        '            {
        '                Content = "Task " + i + "." + j,
        '                Start = DateTime.Today.AddDays(i + (i - 1) * (j - 1)),
        '                Finish = DateTime.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1)
        '            });
        '    }
        '    ScheduleChartDataGrid.Items.Add(item);
        ' }

        ' Optionally, define custom schedules for resources, used when scheduling items assigned to those resources.
        ' ScheduleChartDataGrid.Items[1].Schedule = new Schedule(
        '     DayOfWeek.Tuesday, DayOfWeek.Saturday, // Working week: between Tuesday and Saturday.
        '     TimeSpan.Parse("07:00:00"), TimeSpan.Parse("15:00:00"), // Working day: between 7 AM and 3 PM.
        '     new TimeInterval[] { // Optionally, generic nonworking intervals.
        '         new TimeInterval(DateTime.Today.AddDays(14), DateTime.Today.AddDays(14).Add(TimeOfDay.MaxValue)), // Holiday: full day.
        '         new TimeInterval(DateTime.Today.AddDays(18), DateTime.Today.AddDays(20).Add(TimeSpan.Parse("12:00:00"))) // Custom time interval off: full and partial day accepted.
        '     },
        '     (date) => { // Optionally, specific nonworking intervals based on date parameter: recurrent breaks and holidays accepted.
        '         if (date.Day % 15 == 0) // First recurrence expression: on decade end days.
        '             return new DayTimeInterval[] { 
        '                 new DayTimeInterval(TimeOfDay.MinValue, TimeOfDay.Parse("12:00:00")), // Large interval off: first part of day.
        '                 new DayTimeInterval(TimeOfDay.Parse("12:00:00"), TimeOfDay.Parse("12:30:00")) // Short break: fast lunch time.
        '             };
        '         else if (date.DayOfWeek != DayOfWeek.Monday) // Second recurrence expression: every day except Mondays.
        '             return new DayTimeInterval[] { 
        '                 new DayTimeInterval(TimeOfDay.Parse("11:30:00"), TimeOfDay.Parse("12:30:00")) // Break: regular lunch time.
        '             };
        '         return null; // Otherwise use regular timing only.
        '     });

        ' Optionally, set AreHierarchyConstraintsEnabled to false to increase performance when you perform hierarchy validation in your application.
        ScheduleChartDataGrid.AreHierarchyConstraintsEnabled = False

        ' Initialize the control area.
        ScalesComboBox.SelectedIndex = 0
        ShowWeekendsCheckBox.IsChecked = True
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

    ' Control area commands.
    Private Sub AddNewButton_Click(sender As Object, e As RoutedEventArgs)
        Dim item As ScheduleChartItem = New ScheduleChartItem With {.Content = "New Resource"}
        item.GanttChartItems.Add(New GanttChartItem With {.Content = "New Task", .Start = Date.Today, .Finish = Date.Today.AddDays(1)})
        ScheduleChartDataGrid.Items.Add(item)
        ScheduleChartDataGrid.SelectedItem = item
        ScheduleChartDataGrid.ScrollTo(item.GanttChartItems(0))
        ScheduleChartDataGrid.ScrollTo(item.GanttChartItems(0).Start)
    End Sub
    Private Sub InsertNewButton_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedItem As ScheduleChartItem = TryCast(ScheduleChartDataGrid.SelectedItem, ScheduleChartItem)
        If selectedItem Is Nothing Then
            MessageBox.Show("Cannot insert a new item before selection as the selection is empty; you can either add a new item to the end of the list instead, or select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        Dim item As ScheduleChartItem = New ScheduleChartItem With {.Content = "New Resource"}
        item.GanttChartItems.Add(New GanttChartItem With {.Content = "New Task", .Start = Date.Today, .Finish = Date.Today.AddDays(1)})
        ScheduleChartDataGrid.Items.Insert(selectedItem.Index, item)
        ScheduleChartDataGrid.SelectedItem = item
        ScheduleChartDataGrid.ScrollTo(item.GanttChartItems(0))
        ScheduleChartDataGrid.ScrollTo(item.GanttChartItems(0).Start)
    End Sub
    Private Sub DeleteButton_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of ScheduleChartItem)()
        For Each item As ScheduleChartItem In ScheduleChartDataGrid.GetSelectedItems()
            items.Add(item)
        Next item
        If items.Count <= 0 Then
            MessageBox.Show("Cannot delete the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        items.Reverse()
        ' ScheduleChartDataGrid.BeginInit();
        For Each item As ScheduleChartItem In items
            ScheduleChartDataGrid.Items.Remove(item)
        Next item
        ' ScheduleChartDataGrid.EndInit();
    End Sub
    Private Sub SetColorButton_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of ScheduleChartItem)()
        For Each item As ScheduleChartItem In ScheduleChartDataGrid.GetSelectedItems()
            items.Add(item)
        Next item
        If items.Count <= 0 Then
            MessageBox.Show("Cannot set a custom bar color to the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        For Each item As ScheduleChartItem In items
            For Each ganttChartItem As GanttChartItem In item.GanttChartItems
                GanttChartView.SetStandardBarFill(ganttChartItem, TryCast(Resources("CustomStandardBarFill"), Brush))
                GanttChartView.SetStandardBarStroke(ganttChartItem, TryCast(Resources("CustomStandardBarStroke"), Brush))
            Next ganttChartItem
        Next item
    End Sub
    Private Sub CopyButton_Click(sender As Object, e As RoutedEventArgs)
        If ScheduleChartDataGrid.GetSelectedItemCount() <= 0 Then
            MessageBox.Show("Cannot copy selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        ScheduleChartDataGrid.Copy()
    End Sub
    Private Sub PasteButton_Click(sender As Object, e As RoutedEventArgs)
        ScheduleChartDataGrid.Paste()
    End Sub
    Private Sub UndoButton_Click(sender As Object, e As RoutedEventArgs)
        If ScheduleChartDataGrid.CanUndo() Then
            ScheduleChartDataGrid.Undo()
        Else
            MessageBox.Show("Currently there is no recorded action in the undo queue; perform an action first.", "Information", MessageBoxButton.OK)
        End If
    End Sub
    Private Sub RedoButton_Click(sender As Object, e As RoutedEventArgs)
        If ScheduleChartDataGrid.CanRedo() Then
            ScheduleChartDataGrid.Redo()
        Else
            MessageBox.Show("Currently there is no recorded action in the redo queue; perform an action and undo it first.", "Information", MessageBoxButton.OK)
        End If
    End Sub
    Private Sub ScaleTypeComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim selectedComboBoxItem As ComboBoxItem = TryCast(ScalesComboBox.SelectedItem, ComboBoxItem)
        Dim scalesResourceKey As String = TryCast(selectedComboBoxItem.Tag, String)
        Dim scales As ScaleCollection = TryCast(Resources(scalesResourceKey), ScaleCollection)
        ScheduleChartDataGrid.Scales = scales
    End Sub
    Private Sub ZoomCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        originalZoom = ScheduleChartDataGrid.HourWidth
        ScheduleChartDataGrid.HourWidth = originalZoom * 2
    End Sub
    Private Sub ZoomCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        ScheduleChartDataGrid.HourWidth = originalZoom
    End Sub
    Private originalZoom As Double
    Private Sub IncreaseTimelinePageButton_Click(sender As Object, e As RoutedEventArgs)
        ScheduleChartDataGrid.TimelinePageFinish += pageUpdateAmount
        ScheduleChartDataGrid.TimelinePageStart += pageUpdateAmount
    End Sub
    Private Sub DecreaseTimelinePageButton_Click(sender As Object, e As RoutedEventArgs)
        ScheduleChartDataGrid.TimelinePageFinish -= pageUpdateAmount
        ScheduleChartDataGrid.TimelinePageStart -= pageUpdateAmount
    End Sub
    Private ReadOnly pageUpdateAmount As TimeSpan = TimeSpan.FromDays(7)
    Private Sub ShowWeekendsCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        ScheduleChartDataGrid.VisibleWeekStart = DayOfWeek.Sunday
        ScheduleChartDataGrid.VisibleWeekFinish = DayOfWeek.Saturday
        WorkOnWeekendsCheckBox.IsEnabled = True
    End Sub
    Private Sub ShowWeekendsCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        WorkOnWeekendsCheckBox.IsChecked = False
        WorkOnWeekendsCheckBox.IsEnabled = False
        ScheduleChartDataGrid.VisibleWeekStart = DayOfWeek.Monday
        ScheduleChartDataGrid.VisibleWeekFinish = DayOfWeek.Friday
    End Sub
    Private Sub WorkOnWeekendsCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        ScheduleChartDataGrid.WorkingWeekStart = DayOfWeek.Sunday
        ScheduleChartDataGrid.WorkingWeekFinish = DayOfWeek.Saturday
    End Sub
    Private Sub WorkOnWeekendsCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        ScheduleChartDataGrid.WorkingWeekStart = DayOfWeek.Monday
        ScheduleChartDataGrid.WorkingWeekFinish = DayOfWeek.Friday
    End Sub
    Private Sub PrintButton_Click(sender As Object, e As RoutedEventArgs)
        ScheduleChartDataGrid.Print("ScheduleChartDataGrid Sample Document")
    End Sub
    Private Sub ExportImageButton_Click(sender As Object, e As RoutedEventArgs)
        ScheduleChartDataGrid.Export(CType(Sub()
                                               Dim saveFileDialog As SaveFileDialog = New SaveFileDialog With {.Title = "Export Image To", .Filter = "PNG image files|*.png"}
                                               If Not saveFileDialog.ShowDialog().Equals(True) Then
                                                   Return
                                               End If
                                               Dim bitmapSource As BitmapSource = ScheduleChartDataGrid.GetExportBitmapSource(96 * 2)
                                               Using stream As Stream = saveFileDialog.OpenFile()
                                                   Dim pngBitmapEncoder As New PngBitmapEncoder()
                                                   pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource))
                                                   pngBitmapEncoder.Save(stream)
                                               End Using
                                           End Sub, Action))
    End Sub
End Class
