Imports DlhSoft.Windows.Controls
Imports System.Windows.Controls.Primitives

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

        Dim item7 As GanttChartItem = GanttChartDataGrid.Items(7)
        item7.Start = Date.Today.AddDays(4)
        item7.IsMilestone = True
        item7.Predecessors.Add(New PredecessorItem With {.Item = item4})
        item7.Predecessors.Add(New PredecessorItem With {.Item = item6})

        For i As Integer = 3 To 25
            GanttChartDataGrid.Items.Add(New GanttChartItem With {.Content = "Task " & i, .Indentation = If(i Mod 3 = 0, 0, 1), .Start = Date.Today.AddDays(If(i <= 8, (i - 4) * 3, i - 8)), .Finish = Date.Today.AddDays((If(i <= 8, (i - 4) * 3 + (If(i > 8, 6, 1)), i - 2)) + 1), .CompletedFinish = Date.Today.AddDays(If(i <= 8, (i - 4) * 3, i - 8)).AddDays(If(i Mod 6 = 1, 3, 0))})
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

    Private Sub SortingToggleButton_CheckedChanged(sender As Object, e As RoutedEventArgs)
        Dim toggleButton As ToggleButton = TryCast(e.OriginalSource, ToggleButton)
        If toggleButton Is Nothing Then
            Return
        End If
        Dim columnHeader As String = TryCast(toggleButton.DataContext, String)
        If columnHeader Is Nothing Then
            Return
        End If
        GanttChartDataGrid.Sort(Function(item1 As GanttChartItem, item2 As GanttChartItem) Compare(item1, item2, columnHeader), toggleButton.IsChecked = True)
    End Sub

    ' Compare two items and return -1 if the items are specified in ascending order, 0 if the items are similar, or +1 if the items are in specified descending order.
    Private Shared Function Compare(item1 As GanttChartItem, item2 As GanttChartItem, columnHeader As String) As Integer
        ' The current implementation compares the content (returned by ToString method) of the two items.
        ' Optionally, you may modify the code below to apply a custom sort implementation based on column header and specific requirements.
        Return String.Compare(item1.ToString(), item2.ToString())
    End Function
End Class
