Imports System.Text
Imports DlhSoft.Windows.Controls
Imports Microsoft.Win32
Imports System.IO
Imports System.Collections.ObjectModel

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim allocation11 As AllocationItem = LoadChartDataGrid.Items(0).GanttChartItems(0)
        allocation11.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        allocation11.Finish = Date.Today.Add(TimeSpan.Parse("16:00:00"))

        Dim allocation112 As AllocationItem = LoadChartDataGrid.Items(0).GanttChartItems(1)
        allocation112.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        allocation112.Finish = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"))
        allocation112.Units = 1.5

        Dim allocation12 As AllocationItem = LoadChartDataGrid.Items(0).GanttChartItems(2)
        allocation12.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"))
        allocation12.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        allocation12.Units = 0.5

        Dim allocation13 As AllocationItem = LoadChartDataGrid.Items(0).GanttChartItems(3)
        allocation13.Start = Date.Today.AddDays(4).Add(TimeSpan.Parse("08:00:00"))
        allocation13.Finish = Date.Today.AddDays(4).Add(TimeSpan.Parse("16:00:00"))

        Dim allocation22 As AllocationItem = LoadChartDataGrid.Items(1).GanttChartItems(0)
        allocation22.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        allocation22.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))

        For i As Integer = 3 To 16
            Dim item As LoadChartItem = New LoadChartItem With {.Content = "Resource " & i}
            For j As Integer = 1 To (i - 1) Mod 4 + 1
                item.GanttChartItems.Add(New AllocationItem With {.Content = "Task " & i & "." & j & (If((i + j) Mod 2 = 1, " [200%]", String.Empty)), .Start = Date.Today.AddDays(i + (i - 1) * (j - 1)), .Finish = Date.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1), .Units = 1 + (i + j) Mod 2})
            Next j
            LoadChartDataGrid.Items.Add(item)
        Next i

        ' You may uncomment the next lines of code to test the component performance:
        ' for (int i = 17; i <= 1024; i++)
        ' {
        '    LoadChartItem item = new LoadChartItem { Content = "Resource " + i };
        '    for (int j = 1; j <= (i - 1) % 4 + 1; j++)
        '    {
        '        item.GanttChartItems.Add(
        '            new AllocationItem
        '            {
        '                Content = "Task " + i + "." + j + ((i + j) % 2 == 1 ? " [200%]" : string.Empty),
        '                Start = DateTime.Today.AddDays(i + (i - 1) * (j - 1)),
        '                Finish = DateTime.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1),
        '                Units = 1 + (i + j) % 2
        '            });
        '    }
        '    LoadChartDataGrid.Items.Add(item);
        ' }

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
        LoadChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub

    ' Control area commands.
    Private Sub AddNewButton_Click(sender As Object, e As RoutedEventArgs)
        Dim item As LoadChartItem = New LoadChartItem With {.Content = "New Resource"}
        item.GanttChartItems.Add(New AllocationItem With {.Content = "New Task", .Start = Date.Today, .Finish = Date.Today.AddDays(1)})
        LoadChartDataGrid.Items.Add(item)
        LoadChartDataGrid.SelectedItem = item
        LoadChartDataGrid.ScrollTo(item.GanttChartItems(0))
    End Sub
    Private Sub InsertNewButton_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedItem As LoadChartItem = TryCast(LoadChartDataGrid.SelectedItem, LoadChartItem)
        If selectedItem Is Nothing Then
            MessageBox.Show("Cannot insert a new item before selection as the selection is empty; you can either add a new item to the end of the list instead, or select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        Dim item As LoadChartItem = New LoadChartItem With {.Content = "New Resource"}
        item.GanttChartItems.Add(New AllocationItem With {.Content = "New Task", .Start = Date.Today, .Finish = Date.Today.AddDays(1)})
        LoadChartDataGrid.Items.Insert(selectedItem.Index, item)
        LoadChartDataGrid.SelectedItem = item
    End Sub
    Private Sub DeleteButton_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of LoadChartItem)()
        For Each item As LoadChartItem In LoadChartDataGrid.GetSelectedItems()
            items.Add(item)
        Next item
        If items.Count <= 0 Then
            MessageBox.Show("Cannot delete the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        items.Reverse()
        ' LoadChartDataGrid.BeginInit();
        For Each item As LoadChartItem In items
            LoadChartDataGrid.Items.Remove(item)
        Next item
        ' LoadChartDataGrid.EndInit();
    End Sub
    Private Sub ScaleTypeComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim selectedComboBoxItem As ComboBoxItem = TryCast(ScalesComboBox.SelectedItem, ComboBoxItem)
        Dim scalesResourceKey As String = TryCast(selectedComboBoxItem.Tag, String)
        Dim scales As ScaleCollection = TryCast(Resources(scalesResourceKey), ScaleCollection)
        LoadChartDataGrid.Scales = scales
    End Sub
    Private Sub ZoomCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        originalZoom = LoadChartDataGrid.HourWidth
        LoadChartDataGrid.HourWidth = originalZoom * 2
    End Sub
    Private Sub ZoomCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        LoadChartDataGrid.HourWidth = originalZoom
    End Sub
    Private originalZoom As Double
    Private Sub IncreaseTimelinePageButton_Click(sender As Object, e As RoutedEventArgs)
        LoadChartDataGrid.TimelinePageFinish += pageUpdateAmount
        LoadChartDataGrid.TimelinePageStart += pageUpdateAmount
    End Sub
    Private Sub DecreaseTimelinePageButton_Click(sender As Object, e As RoutedEventArgs)
        LoadChartDataGrid.TimelinePageFinish -= pageUpdateAmount
        LoadChartDataGrid.TimelinePageStart -= pageUpdateAmount
    End Sub
    Private ReadOnly pageUpdateAmount As TimeSpan = TimeSpan.FromDays(7)
    Private Sub ShowWeekendsCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        LoadChartDataGrid.VisibleWeekStart = DayOfWeek.Sunday
        LoadChartDataGrid.VisibleWeekFinish = DayOfWeek.Saturday
    End Sub
    Private Sub ShowWeekendsCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        LoadChartDataGrid.VisibleWeekStart = DayOfWeek.Monday
        LoadChartDataGrid.VisibleWeekFinish = DayOfWeek.Friday
    End Sub
    Private Sub PrintButton_Click(sender As Object, e As RoutedEventArgs)
        LoadChartDataGrid.Print("LoadChartDataGrid Sample Document")
    End Sub
    Private Sub ExportImageButton_Click(sender As Object, e As RoutedEventArgs)
        LoadChartDataGrid.Export(CType(Sub()
                                           Dim saveFileDialog As SaveFileDialog = New SaveFileDialog With {.Title = "Export Image To", .Filter = "PNG image files|*.png"}
                                           If Not saveFileDialog.ShowDialog().Equals(True) Then
                                               Return
                                           End If
                                           Dim bitmapSource As BitmapSource = LoadChartDataGrid.GetExportBitmapSource(96 * 2)
                                           Using stream As Stream = saveFileDialog.OpenFile()
                                               Dim pngBitmapEncoder As New PngBitmapEncoder()
                                               pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource))
                                               pngBitmapEncoder.Save(stream)
                                           End Using
                                       End Sub, Action))
    End Sub
End Class
