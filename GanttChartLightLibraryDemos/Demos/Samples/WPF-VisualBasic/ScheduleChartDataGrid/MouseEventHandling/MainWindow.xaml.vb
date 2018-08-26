Imports DlhSoft.Windows.Controls

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

    Private Sub ScheduleChartDataGrid_PreviewMouseLeftButtonUp(sender As Object, e As System.Windows.Input.MouseButtonEventArgs)
        Dim controlPosition As Point = e.GetPosition(ScheduleChartDataGrid)
        Dim contentPosition As Point = e.GetPosition(ScheduleChartDataGrid.ChartContentElement)

        Dim dateTime As Date = ScheduleChartDataGrid.GetDateTime(contentPosition.X)
        Dim itemRow As ScheduleChartItem = ScheduleChartDataGrid.GetItemAt(contentPosition.Y)

        Dim item As GanttChartItem = Nothing
        Dim frameworkElement As FrameworkElement = TryCast(e.OriginalSource, FrameworkElement)
        If frameworkElement IsNot Nothing Then
            item = TryCast(frameworkElement.DataContext, GanttChartItem)
        End If

        If controlPosition.X < ScheduleChartDataGrid.ActualWidth - ScheduleChartDataGrid.GanttChartView.ActualWidth Then
            Return
        End If
        Dim message As String = String.Empty
        If controlPosition.Y < ScheduleChartDataGrid.HeaderHeight Then
            message = String.Format("You have clicked the chart scale header at date and time {0:g}.", dateTime)
        ElseIf item IsNot Nothing Then
            message = String.Format("You have clicked the task item '{0}' assigned to resource item '{1}' at date and time {2:g}.", item, itemRow, If(dateTime > item.Finish, item.Finish, dateTime))
        ElseIf itemRow IsNot Nothing Then
            message = String.Format("You have clicked at date and time {0:g} within the row of item '{1}'.", dateTime, itemRow)
        Else
            message = String.Format("You have clicked at date and time {0:g} within an empty area of the chart.", dateTime)
        End If

        NotificationsTextBox.AppendText(String.Format("{0}{1}", If(NotificationsTextBox.Text.Length > 0, vbLf, String.Empty), message))
        NotificationsTextBox.ScrollToEnd()
    End Sub
End Class
