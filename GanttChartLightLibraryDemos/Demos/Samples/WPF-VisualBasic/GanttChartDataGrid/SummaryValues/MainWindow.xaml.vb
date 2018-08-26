Imports DlhSoft.Windows.Controls

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim item0 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(0), CustomGanttChartItem)

        Dim item1 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(1), CustomGanttChartItem)
        item1.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item1.Finish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        item1.CompletedFinish = Date.Today.Add(TimeSpan.Parse("12:00:00"))
        item1.AssignmentsContent = "Resource 1"

        Dim item2 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(2), CustomGanttChartItem)
        item2.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item2.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item2.AssignmentsContent = "Resource 1, Resource 2"
        item2.Predecessors.Add(New PredecessorItem With {.Item = item1})

        Dim item3 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(3), CustomGanttChartItem)
        item3.Predecessors.Add(New PredecessorItem With {.Item = item0, .DependencyType = DependencyType.StartStart})

        Dim item4 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(4), CustomGanttChartItem)
        item4.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item4.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"))

        Dim item6 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(6), CustomGanttChartItem)
        item6.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item6.Finish = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))

        Dim item7 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(7), CustomGanttChartItem)
        item7.Start = Date.Today.AddDays(4)
        item7.IsMilestone = True
        item7.Predecessors.Add(New PredecessorItem With {.Item = item4})
        item7.Predecessors.Add(New PredecessorItem With {.Item = item6})

        ' Turn off asynchronous presentation and apply template before setting ExtraCosts property values, in order for items to be aware of their container (GanttChartView.Items collection) and therefore to be able to aggregate summary (parent item) values automatically at initialization time also.
        GanttChartDataGrid.IsAsyncPresentationEnabled = False

        ' Component ApplyTemplate is called in order to complete loading of the user interface, after the main ApplyTemplate that initializes the custom theme, and using an asynchronous action to allow further constructor initializations if they exist (such as setting up the theme name to load).
        Dispatcher.BeginInvoke(CType(Sub()
                                         ApplyTemplate()
                                         GanttChartDataGrid.ApplyTemplate()
                                         item1.ExtraCosts = 200
                                         item2.ExtraCosts = 300
                                         item6.ExtraCosts = 600
                                     End Sub, Action))
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
End Class
