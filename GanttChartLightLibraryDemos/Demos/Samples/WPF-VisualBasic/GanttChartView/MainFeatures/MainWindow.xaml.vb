Imports System.Text
Imports DlhSoft.Windows.Controls
Imports Microsoft.Win32
Imports System.IO

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim item0 As GanttChartItem = GanttChartView.Items(0)

        Dim item1 As GanttChartItem = GanttChartView.Items(1)
        item1.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item1.Finish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        item1.CompletedFinish = Date.Today.Add(TimeSpan.Parse("12:00:00"))
        item1.AssignmentsContent = "Resource 1"

        Dim item2 As GanttChartItem = GanttChartView.Items(2)
        item2.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item2.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item2.AssignmentsContent = "Resource 1, Resource 2"
        item2.Predecessors.Add(New PredecessorItem With {.Item = item1})

        Dim item3 As GanttChartItem = GanttChartView.Items(3)
        item3.Predecessors.Add(New PredecessorItem With {.Item = item0, .DependencyType = DependencyType.StartStart})

        Dim item4 As GanttChartItem = GanttChartView.Items(4)
        item4.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item4.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"))

        Dim item6 As GanttChartItem = GanttChartView.Items(6)
        item6.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item6.Finish = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))

        Dim item7 As GanttChartItem = GanttChartView.Items(7)
        item7.Start = Date.Today.AddDays(4)
        item7.IsMilestone = True
        item7.Predecessors.Add(New PredecessorItem With {.Item = item4})
        item7.Predecessors.Add(New PredecessorItem With {.Item = item6})

        For i As Integer = 3 To 23
            GanttChartView.Items.Add(New GanttChartItem With {.Content = "Task " & i, .Indentation = If(i Mod 3 = 0, 0, 1), .Start = Date.Today.AddDays(If(i <= 8, (i - 4) * 3, i - 8)), .Finish = Date.Today.AddDays((If(i <= 8, (i - 4) * 3 + (If(i > 8, 6, 1)), i - 2)) + 1), .CompletedFinish = Date.Today.AddDays(If(i <= 8, (i - 4) * 3, i - 8)).AddDays(If(i Mod 6 = 4, 3, 0))})
        Next i

        ' You may uncomment the next lines of code to test the component performance:
        ' for (int i = 24; i <= 4096; i++)
        ' {
        '    GanttChartView.Items.Add(
        '        new GanttChartItem
        '        {
        '            Content = "Task " + i,
        '            Indentation = i % 5 == 0 ? 0 : 1,
        '            IsExpanded = i % 10 == 0 ? false : true,
        '            Start = DateTime.Today.AddDays(i),
        '            Finish = DateTime.Today.AddDays(i * 1.2 + 1)
        '        });
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
        GanttChartView.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub

    ' Control area commands.
    Private Sub SetColorButton_Click(sender As Object, e As RoutedEventArgs)
        Dim item As GanttChartItem = GanttChartView.Items(4)
        DlhSoft.Windows.Controls.GanttChartView.SetStandardBarFill(item, TryCast(Resources("CustomStandardBarFill"), Brush))
        DlhSoft.Windows.Controls.GanttChartView.SetStandardBarStroke(item, TryCast(Resources("CustomStandardBarStroke"), Brush))
    End Sub
    Private Sub ScaleTypeComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim selectedComboBoxItem As ComboBoxItem = TryCast(ScalesComboBox.SelectedItem, ComboBoxItem)
        Dim scalesResourceKey As String = TryCast(selectedComboBoxItem.Tag, String)
        Dim scales As ScaleCollection = TryCast(Resources(scalesResourceKey), ScaleCollection)
        GanttChartView.Scales = scales
    End Sub
    Private Sub ZoomCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        originalZoom = GanttChartView.HourWidth
        GanttChartView.HourWidth = originalZoom * 2
    End Sub
    Private Sub ZoomCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        GanttChartView.HourWidth = originalZoom
    End Sub
    Private originalZoom As Double
    Private Sub IncreaseTimelinePageButton_Click(sender As Object, e As RoutedEventArgs)
        GanttChartView.TimelinePageFinish += pageUpdateAmount
        GanttChartView.TimelinePageStart += pageUpdateAmount
    End Sub
    Private Sub DecreaseTimelinePageButton_Click(sender As Object, e As RoutedEventArgs)
        GanttChartView.TimelinePageFinish -= pageUpdateAmount
        GanttChartView.TimelinePageStart -= pageUpdateAmount
    End Sub
    Private ReadOnly pageUpdateAmount As TimeSpan = TimeSpan.FromDays(7)
    Private Sub ShowWeekendsCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartView.VisibleWeekStart = DayOfWeek.Sunday
        GanttChartView.VisibleWeekFinish = DayOfWeek.Saturday
        WorkOnWeekendsCheckBox.IsEnabled = True
    End Sub
    Private Sub ShowWeekendsCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        WorkOnWeekendsCheckBox.IsChecked = False
        WorkOnWeekendsCheckBox.IsEnabled = False
        GanttChartView.VisibleWeekStart = DayOfWeek.Monday
        GanttChartView.VisibleWeekFinish = DayOfWeek.Friday
    End Sub
    Private Sub WorkOnWeekendsCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartView.WorkingWeekStart = DayOfWeek.Sunday
        GanttChartView.WorkingWeekFinish = DayOfWeek.Saturday
    End Sub
    Private Sub WorkOnWeekendsCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        GanttChartView.WorkingWeekStart = DayOfWeek.Monday
        GanttChartView.WorkingWeekFinish = DayOfWeek.Friday
    End Sub
    Private Sub PrintButton_Click(sender As Object, e As RoutedEventArgs)
        GanttChartView.Print("GanttChartView Sample Document")
    End Sub
    Private Sub ExportImageButton_Click(sender As Object, e As RoutedEventArgs)
        GanttChartView.Export(CType(Sub()
                                        Dim saveFileDialog As SaveFileDialog = New SaveFileDialog With {.Title = "Export Image To", .Filter = "PNG image files|*.png"}
                                        If Not saveFileDialog.ShowDialog().Equals(True) Then
                                            Return
                                        End If
                                        Dim bitmapSource As BitmapSource = GanttChartView.GetExportBitmapSource(96 * 2)
                                        Using stream As Stream = saveFileDialog.OpenFile()
                                            Dim pngBitmapEncoder As New PngBitmapEncoder()
                                            pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource))
                                            pngBitmapEncoder.Save(stream)
                                        End Using
                                    End Sub, Action))
    End Sub
End Class
