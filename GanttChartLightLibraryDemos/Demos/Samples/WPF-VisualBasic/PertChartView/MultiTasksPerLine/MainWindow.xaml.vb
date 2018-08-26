Imports DlhSoft.Windows.Controls
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Windows.Threading

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
		Inherits Window

		Public Sub New()
			InitializeComponent()

			DataContext = Me
			Tasks = New ObservableCollection(Of GanttChartItem) From {
				New GanttChartItem With {.Content = "Task 1", .Start = Date.Today.Add(TimeSpan.Parse("08:00:00")), .Finish = Date.Today.Add(TimeSpan.Parse("16:00:00"))},
				New GanttChartItem With {.Content = "Task 2", .Start = Date.Today.Add(TimeSpan.Parse("08:00:00")), .Finish = Date.Today.Add(TimeSpan.Parse("16:00:00"))},
				New GanttChartItem With {.Content = "Milestone", .Start = Date.Today.Add(TimeSpan.Parse("16:00:00")), .IsMilestone = True},
				New GanttChartItem With {.Content = "Task 3", .Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00")), .Finish = Date.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00"))},
				New GanttChartItem With {.Content = "Task 4", .Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00")), .Finish = Date.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00"))}
			}
			Tasks(2).Predecessors.Add(New PredecessorItem With {.Item = Tasks(0)})
			Tasks(2).Predecessors.Add(New PredecessorItem With {.Item = Tasks(1)})
			Tasks(3).Predecessors.Add(New PredecessorItem With {.Item = Tasks(2)})
			Tasks(4).Predecessors.Add(New PredecessorItem With {.Item = Tasks(2)})

			Dispatcher.BeginInvoke(CType(Sub() TabControl.SelectedItem = PertChartTabItem, Action), DispatcherPriority.Background)
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
			PertChartView.Resources.MergedDictionaries.Add(themeResourceDictionary)
		End Sub

		Public Property Tasks() As ObservableCollection(Of GanttChartItem)

		Private Sub TabControl_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If TabControl.SelectedItem Is PertChartTabItem Then
            ' Get PERT Chart items from Gantt Chart. The collection may contain generic links, i.e. virtual effort tasks.
            Dim taskEvents = GanttChartDataGrid.GetPertChartItems()
            OptimizeTasks(taskEvents) ' Comment this line to see default behavior of DlhSoft Gantt Chart Light Library components.
            PertChartView.Items = taskEvents
        End If
    End Sub

		' Optimize tasks between task events, by removing generic links and replacing them by multiple dependencies between the same two task events as appropriate.
		Private Shared Sub OptimizeTasks(taskEvents As ObservableCollection(Of DlhSoft.Windows.Controls.Pert.PertChartItem))
			For Each taskEvent In taskEvents.Where(Function(te) te.Predecessors IsNot Nothing).ToArray()
            Dim tasksValue = taskEvent.Predecessors

            ' When a task event has only virtual effort links to other events, link the previous events directly to the current event.
            If tasksValue.Any() AndAlso tasksValue.All(Function(t) t.IsEffortVirtual) Then
                Dim previousTaskEvents = tasksValue.Select(Function(t) t.Item).ToArray()
                Dim previousTasks = previousTaskEvents.SelectMany(Function(pte) pte.Predecessors).ToArray()
                For Each pte In previousTaskEvents
                    taskEvents.Remove(pte)
                Next pte
                taskEvent.Predecessors.Clear()

                For i = 0 To previousTasks.Length - 1
                    Dim pt = previousTasks(i)
                    taskEvent.Predecessors.Add(pt)

                    ' Set line index values to dependency lines sharing the same start and end events to be able to compute points to be used when displaying polygonal dependency lines accordingly.
                    TaskEventExtensions.SetLineIndex(pt, i)

                    ' Whenever dependency line points are computed being required in the UI, we'll update them accordingly, inserting intermediary points to respect line indexes using vertical positioning.
                    Dim computedDependencyLinePointsPropertyDescriptor As DependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(DlhSoft.Windows.Controls.Pert.PredecessorItem.ComputedDependencyLinePointsProperty, GetType(DlhSoft.Windows.Controls.Pert.PredecessorItem))
                    computedDependencyLinePointsPropertyDescriptor.AddValueChanged(pt, Sub(sender, e)
                                                                                           Dim task = TryCast(sender, DlhSoft.Windows.Controls.Pert.PredecessorItem)
                                                                                           Dim points = task.ComputedDependencyLinePoints
                                                                                           If points.Count < 2 Then
                                                                                               Return
                                                                                           End If
                                                                                           Dim fp As Point = points.First(), lp As Point = points.Last()
                                                                                           Dim widthValue As Double = lp.X - fp.X
                                                                                           points.Insert(1, New Point(fp.X + widthValue * DistanceRateToIntermediaryPoints, fp.Y + TaskEventExtensions.GetLineIndex(pt) * DistanceBetweenLines))
                                                                                           points.Insert(points.Count - 1, New Point(lp.X - widthValue * DistanceRateToIntermediaryPoints, fp.Y + TaskEventExtensions.GetLineIndex(pt) * DistanceBetweenLines))
                                                                                       End Sub)
                Next i
            End If
        Next taskEvent
		End Sub

		Public Const DistanceBetweenLines As Double = 30
		Public Const DistanceRateToIntermediaryPoints As Double = 0.06
	End Class

' Allows storing line index values for predecessor items (dependency lines).
Public NotInheritable Class TaskEventExtensions

    Private Sub New()
    End Sub

    Public Shared Function GetLineIndex(obj As DependencyObject) As Integer
        Return CInt(Fix(obj.GetValue(LineIndexProperty)))
    End Function

    Public Shared Sub SetLineIndex(obj As DependencyObject, value As Integer)
        obj.SetValue(LineIndexProperty, value)
    End Sub

    Public Shared ReadOnly LineIndexProperty As DependencyProperty = DependencyProperty.RegisterAttached("LineIndex", GetType(Integer), GetType(TaskEventExtensions), New PropertyMetadata(0))
End Class
