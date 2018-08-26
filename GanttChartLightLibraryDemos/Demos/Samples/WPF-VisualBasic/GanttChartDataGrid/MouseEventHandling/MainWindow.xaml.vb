Imports DlhSoft.Windows.Controls
Imports DlhSoft.Windows.Shapes

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

    Private Sub GanttChartDataGrid_PreviewMouseLeftButtonUp(sender As Object, e As System.Windows.Input.MouseButtonEventArgs)
        Dim controlPosition As Point = e.GetPosition(GanttChartDataGrid)
        Dim contentPosition As Point = e.GetPosition(GanttChartDataGrid.ChartContentElement)

        Dim dateTime As Date = GanttChartDataGrid.GetDateTime(contentPosition.X)
        Dim itemRow As GanttChartItem = GanttChartDataGrid.GetItemAt(contentPosition.Y)

        Dim item As GanttChartItem = Nothing
        Dim predecessorItem As PredecessorItem = Nothing
        Dim frameworkElement As FrameworkElement = TryCast(e.OriginalSource, FrameworkElement)
        If frameworkElement IsNot Nothing Then
            item = TryCast(frameworkElement.DataContext, GanttChartItem)
            Dim dependencyLine As DependencyArrowLine.LineSegment = TryCast(frameworkElement.DataContext, DependencyArrowLine.LineSegment)
            If dependencyLine IsNot Nothing Then
                predecessorItem = TryCast(dependencyLine.Parent.DataContext, PredecessorItem)
            End If
        End If

        If controlPosition.X < GanttChartDataGrid.ActualWidth - GanttChartDataGrid.GanttChartView.ActualWidth Then
            Return
        End If
        Dim message As String = String.Empty
        If controlPosition.Y < GanttChartDataGrid.HeaderHeight Then
            message = String.Format("You have clicked the chart scale header at date and time {0:g}.", dateTime)
        ElseIf item IsNot Nothing AndAlso item.HasChildren Then
            message = String.Format("You have clicked the summary task item '{0}' at date and time {1:g}.", item, If(dateTime < item.Start, item.Start, (If(dateTime > item.Finish, item.Finish, dateTime))))
        ElseIf item IsNot Nothing AndAlso item.IsMilestone Then
            message = String.Format("You have clicked the milestone task item '{0}' at date and time {1:g}.", item, item.Start)
        ElseIf item IsNot Nothing Then
            message = String.Format("You have clicked the standard task item '{0}' at date and time {1:g}.", item, If(dateTime > item.Finish, item.Finish, dateTime))
        ElseIf predecessorItem IsNot Nothing Then
            message = String.Format("You have clicked the task dependency line between '{0}' and '{1}'.", predecessorItem.DependentItem, predecessorItem.Item)
        ElseIf itemRow IsNot Nothing Then
            message = String.Format("You have clicked at date and time {0:g} within the row of item '{1}'.", dateTime, itemRow)
        Else
            message = String.Format("You have clicked at date and time {0:g} within an empty area of the chart.", dateTime)
        End If

        NotificationsTextBox.AppendText(String.Format("{0}{1}", If(NotificationsTextBox.Text.Length > 0, vbLf, String.Empty), message))
        NotificationsTextBox.ScrollToEnd()
    End Sub
End Class
