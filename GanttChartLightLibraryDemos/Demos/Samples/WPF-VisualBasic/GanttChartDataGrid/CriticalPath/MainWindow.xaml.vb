Imports DlhSoft.Windows.Controls

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
        ' Important note: CompletedFinish value defaults to DateTime.Today, therefore you should always set it to a Start (or a value between Start and Finish) when you initialize a past task item! In this example we don't set it as the task is in the future.
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
        item6.BaselineStart = item6.Start
        item6.BaselineFinish = item6.Finish

        Dim item7 As GanttChartItem = GanttChartDataGrid.Items(7)
        item7.Start = Date.Today.AddDays(4)
        item7.IsMilestone = True
        item7.Predecessors.Add(New PredecessorItem With {.Item = item4})
        item7.Predecessors.Add(New PredecessorItem With {.Item = item6})
        item7.BaselineStart = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))

        Dim item8 As GanttChartItem = GanttChartDataGrid.Items(8)
        item8.Start = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))
        item8.Finish = Date.Today.AddDays(6).Add(TimeSpan.Parse("14:00:00"))
        item8.AssignmentsContent = "Resource 1 [50%], Resource 2 [75%]"
        item8.BaselineStart = Date.Today.Add(TimeSpan.Parse("12:00:00"))
        item8.BaselineFinish = Date.Today.AddDays(4).Add(TimeSpan.Parse("14:00:00"))

        Dim item9 As GanttChartItem = GanttChartDataGrid.Items(9)
        item9.Start = Date.Today.AddDays(6).Add(TimeSpan.Parse("08:00:00"))
        item9.Finish = Date.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"))
        item9.AssignmentsContent = "Resource 1"
        item9.BaselineStart = Date.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"))
        item9.BaselineFinish = Date.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"))

        Dim item10 As GanttChartItem = GanttChartDataGrid.Items(10)
        item10.Start = Date.Today.AddDays(7).Add(TimeSpan.Parse("08:00:00"))
        item10.Finish = Date.Today.AddDays(28).Add(TimeSpan.Parse("16:00:00"))
        item10.Predecessors.Add(New PredecessorItem With {.Item = item9})

        For i As Integer = 6 To 25
            Dim item = New GanttChartItem With {.Content = "Task " & i, .Indentation = If(i Mod 3 = 0, 0, 1), .Start = Date.Today.AddDays(If(i <= 8, (i - 4) * 3, i - 8)), .Finish = Date.Today.AddDays((If(i <= 8, (i - 4) * 3 + (If(i > 8, 6, 1)), i - 2)) + 1), .CompletedFinish = Date.Today.AddDays(If(i <= 8, (i - 4) * 3, i - 8)).AddDays(If(i Mod 6 = 1, 3, 0))}
            If i Mod 3 <> 1 Then
                Dim c = GanttChartDataGrid.Items.Count
                item.Predecessors.Add(New PredecessorItem With {.Item = If(i Mod 3 = 0, GanttChartDataGrid.Items(c - 3), GanttChartDataGrid.Items(c - 1))})
            End If
            GanttChartDataGrid.Items.Add(item)
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

    Private Sub CriticalPathCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        GanttChartDataGrid.IsReadOnly = True
        UsingPertMethodCheckBox.IsEnabled = False
        Dim items As IEnumerable(Of GanttChartItem)
        If UsingPertMethodCheckBox.IsChecked = True Then
            items = GanttChartDataGrid.GetPertCriticalItems()
        Else
            items = GanttChartDataGrid.GetCriticalItems(criticalDelay:=TimeSpan.FromHours(8))
        End If
        For Each item In items
            If item.HasChildren Then
                Continue For
            End If
            SetCriticalPathHighlighting(item, True)
        Next item
        MessageBox.Show("Gantt Chart items are temporarily read only while critical path is highlighted.", "Information", MessageBoxButton.OK)
        End Sub
    Private Sub CriticalPathCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        UsingPertMethodCheckBox.IsEnabled = True
        GanttChartDataGrid.IsReadOnly = False
        For Each item As GanttChartItem In GanttChartDataGrid.Items
            If item.HasChildren Then
                Continue For
            End If
            SetCriticalPathHighlighting(item, False)
        Next item
    End Sub

    Private Sub SetCriticalPathHighlighting(item As GanttChartItem, isHighlighted As Boolean)
        If Not item.IsMilestone Then
            GanttChartView.SetStandardBarFill(item, If(isHighlighted, TryCast(Resources("CustomStandardBarFill"), Brush), GanttChartDataGrid.StandardBarFill))
            GanttChartView.SetStandardBarStroke(item, If(isHighlighted, TryCast(Resources("CustomStandardBarStroke"), Brush), GanttChartDataGrid.StandardBarStroke))
            GanttChartView.SetStandardCompletedBarFill(item, If(isHighlighted, TryCast(Resources("CustomStandardCompletedBarFill"), Brush), GanttChartDataGrid.StandardCompletedBarFill))
            GanttChartView.SetStandardCompletedBarStroke(item, If(isHighlighted, TryCast(Resources("CustomStandardCompletedBarStroke"), Brush), GanttChartDataGrid.StandardCompletedBarFill))
        Else
            GanttChartView.SetMilestoneBarFill(item, If(isHighlighted, TryCast(Resources("CustomStandardBarFill"), Brush), GanttChartDataGrid.MilestoneBarFill))
            GanttChartView.SetMilestoneBarStroke(item, If(isHighlighted, TryCast(Resources("CustomStandardBarStroke"), Brush), GanttChartDataGrid.MilestoneBarStroke))
        End If
    End Sub
End Class
