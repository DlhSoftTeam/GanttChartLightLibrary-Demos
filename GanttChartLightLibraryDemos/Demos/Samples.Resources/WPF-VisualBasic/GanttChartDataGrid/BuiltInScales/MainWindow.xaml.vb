Imports DlhSoft.Windows.Controls
Imports System.Globalization

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

    Private Sub Root_Loaded(sender As Object, e As RoutedEventArgs)
        MajorScaleTypeComboBox.SelectedItem = ScaleType.Weeks
        MinorScaleTypeComboBox.SelectedItem = ScaleType.Days

        MajorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.ShortDate
        MinorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.DayOfWeekInitial

        ZoomLevelTextBox.Text = "5"

        UpdateScaleComboBox.SelectedIndex = 1 ' 15min
    End Sub

    Private Sub MajorScaleTypeComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        SetMajorScale()
    End Sub
    Private Sub SetMajorScale()
        If GanttChartDataGrid Is Nothing Then
            Return
        End If
        Dim scaleType As ScaleType = CType(MajorScaleTypeComboBox.SelectedItem, ScaleType)
        GanttChartDataGrid.GetScale(0).ScaleType = GetActualScaleType(scaleType)
        UpdateFromSelectedMajorScaleType(scaleType)
    End Sub
    Private Sub UpdateFromSelectedMajorScaleType(scaleType As ScaleType)
        Select Case scaleType
            Case ScaleType.Years
                MajorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Year
                MinorScaleTypeComboBox.SelectedItem = ScaleType.Months
            Case ScaleType.Quarters
                MajorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.YearMonth
                MinorScaleTypeComboBox.SelectedItem = ScaleType.Months
            Case ScaleType.Months
                MajorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Month
                MinorScaleTypeComboBox.SelectedItem = ScaleType.Weeks
            Case ScaleType.Weeks
                MajorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.ShortDate
                MinorScaleTypeComboBox.SelectedItem = ScaleType.Days
            Case ScaleType.Days
                MajorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Day
                MinorScaleTypeComboBox.SelectedIndex = 5
            Case ScaleType.Hours
                MajorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Hour
                MinorScaleTypeComboBox.SelectedItem = ScaleType.Days
        End Select
    End Sub

    Private Sub MinorScaleTypeComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        SetMinorScale()
    End Sub
    Private Sub SetMinorScale()
        If GanttChartDataGrid Is Nothing OrElse MinorScaleTypeComboBox.SelectedItem Is Nothing Then
            Return
        End If
        Dim scaleType = CType(MinorScaleTypeComboBox.SelectedItem, ScaleType)
        GanttChartDataGrid.GetScale(1).ScaleType = GetActualScaleType(scaleType)
        UpdateFromSelectedMinorScaleType(scaleType)
    End Sub
    Private Sub UpdateFromSelectedMinorScaleType(scaleType As ScaleType)
        Select Case scaleType
            Case ScaleType.Years
                MinorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Year
                ZoomLevelTextBox.Text = "0.5"
            Case ScaleType.Months
                MinorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Month
                ZoomLevelTextBox.Text = "1"
            Case ScaleType.Weeks
                MinorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Day
                ZoomLevelTextBox.Text = "2"
            Case ScaleType.Days
                MinorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.DayOfWeekInitial
                ZoomLevelTextBox.Text = "5"
            Case ScaleType.Hours
                MinorScaleHeaderFormatComboBox.SelectedItem = TimeScaleTextFormat.Hour
                ZoomLevelTextBox.Text = "25"
        End Select
    End Sub

    Private Function GetActualScaleType(value As ScaleType) As ScaleType
        Dim scale As ScaleType
        If value <> ScaleType.Weeks OrElse MondayBasedCheckBox.IsChecked <> True Then
            scale = value
        Else
            scale = ScaleType.WeeksStartingMonday
        End If
        Return scale
    End Function

    Private Sub MajorScaleHeaderFormatComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If GanttChartDataGrid Is Nothing Then
            Return
        End If
        Dim headerFormat As TimeScaleTextFormat = CType(MajorScaleHeaderFormatComboBox.SelectedItem, TimeScaleTextFormat)
        GanttChartDataGrid.GetScale(0).HeaderContentFormat = headerFormat
    End Sub
    Private Sub MinorScaleHeaderFormatComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If GanttChartDataGrid Is Nothing Then
            Return
        End If
        Dim headerFormat As TimeScaleTextFormat = CType(MinorScaleHeaderFormatComboBox.SelectedItem, TimeScaleTextFormat)
        GanttChartDataGrid.GetScale(1).HeaderContentFormat = headerFormat
    End Sub

    Private Sub MajorScaleSeparatorCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        If GanttChartDataGrid Is Nothing Then
            Return
        End If
        If MajorScaleSeparatorCheckBox.IsChecked = True Then
            GanttChartDataGrid.GetScale(0).BorderThickness = New Thickness(0, 0, 1, 0)
        Else
            GanttChartDataGrid.GetScale(0).BorderThickness = New Thickness(0)
        End If
    End Sub
    Private Sub MinorScaleSeparatorCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        If GanttChartDataGrid Is Nothing Then
            Return
        End If
        If MinorScaleSeparatorCheckBox.IsChecked = True Then
            GanttChartDataGrid.GetScale(1).BorderThickness = New Thickness(0, 0, 1, 0)
            GanttChartDataGrid.GetScale(1).BorderBrush = GanttChartDataGrid.GetScale(0).BorderBrush
        Else
            GanttChartDataGrid.GetScale(1).BorderThickness = New Thickness(0)
        End If
    End Sub

    Private Sub MondayBasedCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        SetMajorScale()
        SetMinorScale()
    End Sub

    Private Sub NonworkingDaysCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        If GanttChartDataGrid Is Nothing Then
            Return
        End If
        If NonworkingDaysCheckBox.IsChecked = True Then
            GanttChartDataGrid.IsNonworkingTimeHighlighted = True
        Else
            GanttChartDataGrid.IsNonworkingTimeHighlighted = False
        End If

    End Sub
    Private Sub CurrentTimeVisibleCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        If GanttChartDataGrid Is Nothing Then
            Return
        End If
        If CurrentTimeVisibleCheckBox.IsChecked = True Then
            GanttChartDataGrid.IsCurrentTimeLineVisible = True
        Else
            GanttChartDataGrid.IsCurrentTimeLineVisible = False
        End If
    End Sub

    Private Sub ZoomLevelTextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        If GanttChartDataGrid Is Nothing Then
            Return
        End If
        Dim hourWidth As Double
        If Double.TryParse(ZoomLevelTextBox.Text.Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, hourWidth) Then
            If hourWidth > 0 Then
                GanttChartDataGrid.HourWidth = hourWidth
            End If
        End If
    End Sub

    Private Sub UpdateScaleComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If GanttChartDataGrid Is Nothing Then
            Return
        End If
        Dim selectedItem = TryCast(UpdateScaleComboBox.SelectedItem, ComboBoxItem)
        Select Case TryCast(selectedItem.Content, String)
            Case "Free"
                GanttChartDataGrid.UpdateScaleInterval = TimeSpan.Zero
            Case "15 min"
                GanttChartDataGrid.UpdateScaleInterval = TimeSpan.Parse("00:15:00")
            Case "Hour"
                GanttChartDataGrid.UpdateScaleInterval = TimeSpan.Parse("01:00:00")
            Case "Day"
                GanttChartDataGrid.UpdateScaleInterval = TimeSpan.Parse("1.00:00:00")
        End Select
    End Sub
End Class
