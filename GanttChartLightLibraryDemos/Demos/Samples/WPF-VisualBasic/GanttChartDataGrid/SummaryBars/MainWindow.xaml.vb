Imports DlhSoft.Windows.Controls
Imports System.Collections.ObjectModel

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

        Dim item5 As GanttChartItem = GanttChartDataGrid.Items(5)

        Dim item6 As GanttChartItem = GanttChartDataGrid.Items(6)
        item6.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item6.Finish = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))

        Dim item7 As GanttChartItem = GanttChartDataGrid.Items(7)
        item7.Start = Date.Today.AddDays(4)
        item7.IsMilestone = True
        item7.Predecessors.Add(New PredecessorItem With {.Item = item4})
        item7.Predecessors.Add(New PredecessorItem With {.Item = item6})

        ' Turn off asynchronous presentation and apply template to ensure task hierarchy initialization and to be able to access the internal GanttChartView control.
        GanttChartDataGrid.IsAsyncPresentationEnabled = False

        ' Component ApplyTemplate is called in order to complete loading of the user interface, after the main ApplyTemplate that initializes the custom theme, and using an asynchronous action to allow further constructor initializations if they exist (such as setting up the theme name to load).
        Dispatcher.BeginInvoke(CType(Sub()
                                         ' Set up the internally managed leaf item clones to be displayed in the chart area (instead of GanttChartDataGrid.Items).
                                         ' Store children of each summary task for reference purposes.
                                         ' Store clones as item tags for reference purposes.
                                         ' Store parents of each cloned item for reference purposes.
                                         ' Initialize expansion notification handler on summary items in the DataGrid.
                                         ApplyTemplate()
                                         GanttChartDataGrid.ApplyTemplate()
                                         For Each item As GanttChartItem In GanttChartDataGrid.Items
                                             If item.HasChildren Then
                                                 item.Tag = GanttChartDataGrid.GetAllChildren(item)
                                                 Continue For
                                             End If
                                             Dim clone As New GanttChartItem()
                                             BindingOperations.SetBinding(clone, GanttChartItem.ContentProperty, New Binding("Content") With {.Source = item})
                                             BindingOperations.SetBinding(clone, GanttChartItem.StartProperty, New Binding("Start") With {.Source = item})
                                             BindingOperations.SetBinding(clone, GanttChartItem.FinishProperty, New Binding("Finish") With {.Source = item})
                                             BindingOperations.SetBinding(clone, GanttChartItem.CompletedFinishProperty, New Binding("CompletedFinish") With {.Source = item})
                                             BindingOperations.SetBinding(clone, GanttChartItem.IsMilestoneProperty, New Binding("IsMilestone") With {.Source = item})
                                             BindingOperations.SetBinding(clone, GanttChartItem.AssignmentsContentProperty, New Binding("AssignmentsContent") With {.Source = item})
                                             BindingOperations.SetBinding(clone, GanttChartItem.DisplayRowIndexProperty, New Binding("ActualDisplayRowIndex") With {.Source = item})
                                             BindingOperations.SetBinding(clone, GanttChartItem.IsVisibleProperty, New Binding("IsVisible") With {.Source = item})
                                             BindingOperations.SetBinding(item, GanttChartItem.StartProperty, New Binding("Start") With {.Source = clone})
                                             BindingOperations.SetBinding(item, GanttChartItem.FinishProperty, New Binding("Finish") With {.Source = clone})
                                             BindingOperations.SetBinding(item, GanttChartItem.CompletedFinishProperty, New Binding("CompletedFinish") With {.Source = clone})
                                             BindingOperations.SetBinding(item, GanttChartItem.IsMilestoneProperty, New Binding("IsMilestone") With {.Source = clone})
                                             item.Tag = clone
                                             clone.Tag = GanttChartDataGrid.GetAllParents(item)
                                             ganttChartItemClones.Add(clone)
                                         Next item
                                         GanttChartDataGrid.GanttChartView.Items = ganttChartItemClones
                                         For Each item As GanttChartItem In GanttChartDataGrid.Items
                                             If item.HasChildren Then
                                                 AddHandler item.ExpansionChanged, AddressOf Item_ExpansionChanged
                                             End If
                                         Next item
                                         item0.IsExpanded = False
                                         item5.IsExpanded = False
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

    ' Stores the clones of leaf Gantt Chart items.
    Private ganttChartItemClones As New ObservableCollection(Of GanttChartItem)()

    Private Sub Item_ExpansionChanged(sender As Object, e As EventArgs)
        Dim summaryItem As GanttChartItem = TryCast(sender, GanttChartItem)
        Dim childItems As IEnumerable(Of GanttChartItem) = TryCast(summaryItem.Tag, IEnumerable(Of GanttChartItem))
        If Not summaryItem.IsExpanded Then
            ' When a summary item gets collapsed, show child item clones in the chart area in the summary row.
            For Each childItem As GanttChartItem In childItems
                Dim clone As GanttChartItem = TryCast(childItem.Tag, GanttChartItem)
                If clone Is Nothing Then
                    Continue For
                End If
                BindingOperations.SetBinding(clone, GanttChartItem.DisplayRowIndexProperty, New Binding("ActualDisplayRowIndex") With {.Source = summaryItem})
                BindingOperations.SetBinding(clone, GanttChartItem.IsVisibleProperty, New Binding("IsVisible") With {.Source = summaryItem})
                clone.AssignmentsContent = Nothing
            Next childItem
        Else
            ' When a summary item gets expanded, show child item clones in the chart area in their own row (or in the row of their first visible parent).
            For Each childItem As GanttChartItem In childItems
                Dim clone As GanttChartItem = TryCast(childItem.Tag, GanttChartItem)
                If clone Is Nothing Then
                    Continue For
                End If
                If childItem.IsVisible Then
                    BindingOperations.SetBinding(clone, GanttChartItem.DisplayRowIndexProperty, New Binding("ActualDisplayRowIndex") With {.Source = childItem})
                    BindingOperations.SetBinding(clone, GanttChartItem.IsVisibleProperty, New Binding("IsVisible") With {.Source = childItem})
                    BindingOperations.SetBinding(clone, GanttChartItem.AssignmentsContentProperty, New Binding("AssignmentsContent") With {.Source = childItem})
                Else
                    Dim parentItems As IEnumerable(Of GanttChartItem) = TryCast(clone.Tag, IEnumerable(Of GanttChartItem))
                    For Each parentItem As GanttChartItem In parentItems
                        If parentItem.IsVisible Then
                            BindingOperations.SetBinding(clone, GanttChartItem.DisplayRowIndexProperty, New Binding("ActualDisplayRowIndex") With {.Source = parentItem})
                            BindingOperations.SetBinding(clone, GanttChartItem.IsVisibleProperty, New Binding("IsVisible") With {.Source = parentItem})
                            Exit For
                        End If
                    Next parentItem
                End If
            Next childItem
        End If
    End Sub
End Class
