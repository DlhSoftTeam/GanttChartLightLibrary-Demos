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

        Dim task1 As GanttChartItem = ScheduleChartView.Items(0).GanttChartItems(0)
        task1.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        task1.Finish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        task1.CompletedFinish = Date.Today.Add(TimeSpan.Parse("12:00:00"))

        Dim task21 As GanttChartItem = ScheduleChartView.Items(0).GanttChartItems(1)
        task21.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"))
        task21.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        task21.AssignmentsContent = "50%"

        Dim task22 As GanttChartItem = ScheduleChartView.Items(1).GanttChartItems(0)
        task22.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"))
        task22.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))

        For i As Integer = 3 To 16
            Dim item As New ScheduleChartItem()
            For j As Integer = 1 To (i - 1) Mod 4 + 1
                item.GanttChartItems.Add(New GanttChartItem With {.Content = "Task " & i & "." & j & " (Resource " & i & ")", .Start = Date.Today.AddDays(i + (i - 1) * (j - 1)), .Finish = Date.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1), .CompletedFinish = Date.Today.AddDays(i + (i - 1) * (j - 1)).AddDays(If((i + j) Mod 5 = 2, 2, 0))})
            Next j
            ScheduleChartView.Items.Add(item)
        Next i

        ' You may uncomment the next lines of code to test the component performance:
        ' for (int i = 17; i <= 1024; i++)
        ' {
        '    ScheduleChartItem item = new ScheduleChartItem();
        '    for (int j = 1; j <= (i - 1) % 4 + 1; j++)
        '    {
        '        item.GanttChartItems.Add(
        '            new GanttChartItem
        '            {
        '                Content = "Task " + i + "." + j + " (Resource " + i + ")",
        '                Start = DateTime.Today.AddDays(i + (i - 1) * (j - 1)),
        '                Finish = DateTime.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1)
        '            });
        '    }
        '    ScheduleChartView.Items.Add(item);
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
        ScheduleChartView.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub

    ' Control area commands.
    Private Sub SetColorButton_Click(sender As Object, e As RoutedEventArgs)
        Dim item1 As GanttChartItem = ScheduleChartView.Items(0).GanttChartItems(1)
        GanttChartView.SetStandardBarFill(item1, TryCast(Resources("CustomStandardBarFill"), Brush))
        GanttChartView.SetStandardBarStroke(item1, TryCast(Resources("CustomStandardBarStroke"), Brush))
        Dim item2 As GanttChartItem = ScheduleChartView.Items(1).GanttChartItems(0)
        GanttChartView.SetStandardBarFill(item2, TryCast(Resources("CustomStandardBarFill"), Brush))
        GanttChartView.SetStandardBarStroke(item2, TryCast(Resources("CustomStandardBarStroke"), Brush))
    End Sub
    Private Sub ScaleTypeComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim selectedComboBoxItem As ComboBoxItem = TryCast(ScalesComboBox.SelectedItem, ComboBoxItem)
        Dim scalesResourceKey As String = TryCast(selectedComboBoxItem.Tag, String)
        Dim scales As ScaleCollection = TryCast(Resources(scalesResourceKey), ScaleCollection)
        ScheduleChartView.Scales = scales
    End Sub
    Private Sub ZoomCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        originalZoom = ScheduleChartView.HourWidth
        ScheduleChartView.HourWidth = originalZoom * 2
    End Sub
    Private Sub ZoomCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        ScheduleChartView.HourWidth = originalZoom
    End Sub
    Private originalZoom As Double
    Private Sub IncreaseTimelinePageButton_Click(sender As Object, e As RoutedEventArgs)
        ScheduleChartView.TimelinePageFinish += pageUpdateAmount
        ScheduleChartView.TimelinePageStart += pageUpdateAmount
    End Sub
    Private Sub DecreaseTimelinePageButton_Click(sender As Object, e As RoutedEventArgs)
        ScheduleChartView.TimelinePageFinish -= pageUpdateAmount
        ScheduleChartView.TimelinePageStart -= pageUpdateAmount
    End Sub
    Private ReadOnly pageUpdateAmount As TimeSpan = TimeSpan.FromDays(7)
    Private Sub ShowWeekendsCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        ScheduleChartView.VisibleWeekStart = DayOfWeek.Sunday
        ScheduleChartView.VisibleWeekFinish = DayOfWeek.Saturday
        WorkOnWeekendsCheckBox.IsEnabled = True
    End Sub
    Private Sub ShowWeekendsCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        WorkOnWeekendsCheckBox.IsChecked = False
        WorkOnWeekendsCheckBox.IsEnabled = False
        ScheduleChartView.VisibleWeekStart = DayOfWeek.Monday
        ScheduleChartView.VisibleWeekFinish = DayOfWeek.Friday
    End Sub
    Private Sub WorkOnWeekendsCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        ScheduleChartView.WorkingWeekStart = DayOfWeek.Sunday
        ScheduleChartView.WorkingWeekFinish = DayOfWeek.Saturday
    End Sub
    Private Sub WorkOnWeekendsCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        ScheduleChartView.WorkingWeekStart = DayOfWeek.Monday
        ScheduleChartView.WorkingWeekFinish = DayOfWeek.Friday
    End Sub
    Private Sub PrintButton_Click(sender As Object, e As RoutedEventArgs)
        ScheduleChartView.Print("ScheduleChartView Sample Document")
    End Sub
    Private Sub ExportImageButton_Click(sender As Object, e As RoutedEventArgs)
        ScheduleChartView.Export(CType(Sub()
                                           Dim saveFileDialog As SaveFileDialog = New SaveFileDialog With {.Title = "Export Image To", .Filter = "PNG image files|*.png"}
                                           If Not saveFileDialog.ShowDialog().Equals(True) Then
                                               Return
                                           End If
                                           Dim bitmapSource As BitmapSource = ScheduleChartView.GetExportBitmapSource(96 * 2)
                                           Using stream As Stream = saveFileDialog.OpenFile()
                                               Dim pngBitmapEncoder As New PngBitmapEncoder()
                                               pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource))
                                               pngBitmapEncoder.Save(stream)
                                           End Using
                                       End Sub, Action))
    End Sub
End Class
