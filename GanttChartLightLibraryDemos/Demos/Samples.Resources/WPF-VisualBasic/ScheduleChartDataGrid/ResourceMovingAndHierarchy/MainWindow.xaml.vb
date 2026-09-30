Imports System.Windows.Controls.Primitives
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Threading
Imports DlhSoft.Windows.Controls
Imports DlhSoft.Windows.Data
Imports Microsoft.Win32

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        Me.InitializeComponent()
        Dim applicationName = Me.GetType().Namespace

        Dim unassignedScheduleChartItem = Me.ScheduleChartDataGrid.Items(0)

        Dim item1 As New CustomGanttChartItem With {.Content = "Project delivery", .Start = Date.Today.AddDays(1), .Finish = Date.Today.AddDays(5)}
        unassignedScheduleChartItem.GanttChartItems.Add(item1)

        Dim item2 As New CustomGanttChartItem With {.Content = "Maintaince", .Start = Date.Today.AddDays(2), .Finish = Date.Today.AddDays(6)}
        unassignedScheduleChartItem.GanttChartItems.Add(item2)

        Dim item3 As New CustomGanttChartItem With {.Content = "Marketing", .Start = Date.Today.AddDays(3), .IsMilestone = True}
        unassignedScheduleChartItem.GanttChartItems.Add(item3)

        Dim item4 As New CustomGanttChartItem With {.Content = "Colors", .Start = Date.Today.AddDays(4), .Finish = Date.Today.AddDays(8)}
        unassignedScheduleChartItem.GanttChartItems.Add(item4)

        Dim item5 As New CustomGanttChartItem With {.Content = "Logo", .Start = Date.Today.AddDays(5), .Finish = Date.Today.AddDays(9)}
        unassignedScheduleChartItem.GanttChartItems.Add(item5)

        Dim item6 As New CustomGanttChartItem With {
            .Content = "Samples app",
            .Start = Date.Today.AddDays(6),
            .Finish = Date.Today.AddDays(10),
            .Icon = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Flag.png", applicationName), UriKind.Absolute))
        }
        unassignedScheduleChartItem.GanttChartItems.Add(item6)

        Dim item7 As New CustomGanttChartItem With {.Content = "Screenshots", .Start = Date.Today.AddDays(7), .Finish = Date.Today.AddDays(11)}
        unassignedScheduleChartItem.GanttChartItems.Add(item7)

        Dim item8 As New CustomGanttChartItem With {.Content = "Videos", .Start = Date.Today.AddDays(8), .Finish = Date.Today.AddDays(12)}
        unassignedScheduleChartItem.GanttChartItems.Add(item8)

        Dim task21 = TryCast(Me.ScheduleChartDataGrid.Items(2).GanttChartItems(0), CustomGanttChartItem)
        task21.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        task21.Finish = Date.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00"))
        task21.CompletedFinish = Date.Today.Add(TimeSpan.Parse("16:00:00"))

        Dim task22 = TryCast(Me.ScheduleChartDataGrid.Items(2).GanttChartItems(1), CustomGanttChartItem)
        task22.Start = Date.Today.AddDays(4).Add(TimeSpan.Parse("08:00:00"))
        task22.Finish = Date.Today.AddDays(8).Add(TimeSpan.Parse("16:00:00"))
        task22.CompletedFinish = Date.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"))
        task22.Icon = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Person.png", applicationName), UriKind.Absolute))
        task22.Note = "This task is very important."

        Dim pred21 As New PredecessorItem()
        pred21.Item = task21
        task22.Predecessors.Add(pred21)

        Dim task31 = TryCast(Me.ScheduleChartDataGrid.Items(3).GanttChartItems(0), CustomGanttChartItem)
        task31.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"))
        task31.Finish = Date.Today.AddDays(8).Add(TimeSpan.Parse("16:00:00"))
        task31.Icon = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Person.png", applicationName), UriKind.Absolute))
        task31.AssignmentsContent = "50%"

        Dim task51 = TryCast(Me.ScheduleChartDataGrid.Items(5).GanttChartItems(0), CustomGanttChartItem)
        task51.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        task51.Finish = Date.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00"))
        task51.CompletedFinish = Date.Today.Add(TimeSpan.Parse("16:00:00"))

        Dim task52 = TryCast(Me.ScheduleChartDataGrid.Items(5).GanttChartItems(1), CustomGanttChartItem)
        task52.Start = Date.Today.AddDays(5).Add(TimeSpan.Parse("12:00:00"))
        task52.Finish = Date.Today.AddDays(8).Add(TimeSpan.Parse("16:00:00"))

        Dim task53 = TryCast(Me.ScheduleChartDataGrid.Items(5).GanttChartItems(2), CustomGanttChartItem)
        task53.Start = Date.Today.AddDays(9).Add(TimeSpan.Parse("12:00:00"))
        task53.Finish = Date.Today.AddDays(12).Add(TimeSpan.Parse("16:00:00"))

        Dim pred51 As New PredecessorItem With {.Item = task51}
        task52.Predecessors.Add(pred51)

        Dim succ53 As New PredecessorItem With {.Item = task53, .DependencyType = DependencyType.FinishFinish}
        task52.Predecessors.Add(succ53)

        Dim task61 = TryCast(Me.ScheduleChartDataGrid.Items(6).GanttChartItems(0), CustomGanttChartItem)
        task61.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        task61.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        task61.CompletedFinish = Date.Today.Add(TimeSpan.Parse("16:00:00"))

        Dim task62 = TryCast(Me.ScheduleChartDataGrid.Items(6).GanttChartItems(1), CustomGanttChartItem)
        task62.Start = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))
        task62.Finish = Date.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"))
        task62.Icon = New BitmapImage(New Uri(String.Format("pack://application:,,,/{0};component/Images/Flag.png", applicationName), UriKind.Absolute))

        Dim task63 = TryCast(Me.ScheduleChartDataGrid.Items(6).GanttChartItems(2), CustomGanttChartItem)
        task63.Start = Date.Today.AddDays(7).Add(TimeSpan.Parse("12:00:00"))
        task63.Finish = Date.Today.AddDays(9).Add(TimeSpan.Parse("16:00:00"))

        Dim task71 = TryCast(Me.ScheduleChartDataGrid.Items(7).GanttChartItems(0), CustomGanttChartItem)
        task71.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        task71.Finish = Date.Today.AddDays(3).Add(TimeSpan.Parse("16:00:00"))
        task71.CompletedFinish = Date.Today.Add(TimeSpan.Parse("16:00:00"))

        Dim task81 = TryCast(Me.ScheduleChartDataGrid.Items(8).GanttChartItems(0), CustomGanttChartItem)
        task81.Start = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))
        task81.Finish = Date.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"))

        Dim task91 = TryCast(Me.ScheduleChartDataGrid.Items(9).GanttChartItems(0), CustomGanttChartItem)
        task91.Start = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))
        task91.Finish = Date.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"))

        Dim task111 = TryCast(Me.ScheduleChartDataGrid.Items(11).GanttChartItems(0), CustomGanttChartItem)
        task111.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        task111.Finish = Date.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"))
        task111.CompletedFinish = Date.Today.Add(TimeSpan.Parse("16:00:00"))

        Dim task112 = TryCast(Me.ScheduleChartDataGrid.Items(11).GanttChartItems(1), CustomGanttChartItem)
        task112.Start = Date.Today.AddDays(7).Add(TimeSpan.Parse("12:00:00"))
        task112.Finish = Date.Today.AddDays(12).Add(TimeSpan.Parse("16:00:00"))

        Me.ScheduleChartDataGrid.DependencyCreationValidator = Function(i1, i2) i1 IsNot i2
        Me.ScheduleChartDataGrid.AreHierarchyConstraintsEnabled = False

        Me.ScalesComboBox.SelectedIndex = 0
        Me.ShowWeekendsCheckBox.IsChecked = True
    End Sub

    Private theme As String = "Generic-bright"
    Public Sub New(theme As String)
        Me.New()
        Me.theme = theme
        Me.ApplyTemplate()
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
        Me.ScheduleChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub

    Public Shared ReadOnly IsSelectedProperty As DependencyProperty = DependencyProperty.RegisterAttached("IsSelected", GetType(Boolean), GetType(MainWindow), New PropertyMetadata(False))
    Public Shared Function GetIsSelected(obj As DependencyObject) As Boolean
        Return CBool(obj.GetValue(IsSelectedProperty))
    End Function
    Public Shared Sub SetIsSelected(obj As DependencyObject, value As Boolean)
        obj.SetValue(IsSelectedProperty, value)
    End Sub

    Private SelectedItem As GanttChartItem

    Private Sub ScheduleChartDataGrid_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        Dim controlPosition = e.GetPosition(Me.ScheduleChartDataGrid)
        If controlPosition.X < Me.ScheduleChartDataGrid.ActualWidth - Me.ScheduleChartDataGrid.GanttChartView.ActualWidth Then
            Return
        End If

        Dim item As GanttChartItem = Nothing
        Dim frameworkElement = TryCast(e.OriginalSource, FrameworkElement)
        If frameworkElement IsNot Nothing Then
            item = TryCast(frameworkElement.DataContext, GanttChartItem)
        End If

        If SelectedItem IsNot Nothing Then
            SetIsSelected(SelectedItem, False)
            SelectedItem = Nothing
        End If

        If item Is Nothing Then
            Return
        End If

        SelectedItem = item
        SetIsSelected(SelectedItem, True)
    End Sub

    Public Shared Shadows ReadOnly OpacityProperty As DependencyProperty = DependencyProperty.RegisterAttached("Opacity", GetType(Double), GetType(MainWindow), New PropertyMetadata(1.0))
    Public Shared Function GetOpacity(obj As DependencyObject) As Double
        Return CDbl(obj.GetValue(OpacityProperty))
    End Function
    Public Shared Sub SetOpacity(obj As DependencyObject, value As Double)
        obj.SetValue(OpacityProperty, value)
    End Sub

    Private Sub AddNewButton_Click(sender As Object, e As RoutedEventArgs)
        Dim item As New ScheduleChartItem With {.Content = "New Resource"}
        item.GanttChartItems.Add(New GanttChartItem With {.Content = "New Task", .Start = Date.Today, .Finish = Date.Today.AddDays(1)})
        Me.ScheduleChartDataGrid.Items.Add(item)
        Me.ScheduleChartDataGrid.SelectedItem = item
        Me.ScheduleChartDataGrid.ScrollTo(item.GanttChartItems(0))
        Me.ScheduleChartDataGrid.ScrollTo(item.GanttChartItems(0).Start)
    End Sub
    Private Sub InsertNewButton_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedItem = TryCast(Me.ScheduleChartDataGrid.SelectedItem, ScheduleChartItem)
        If selectedItem Is Nothing Then
            MessageBox.Show("Cannot insert a new item before selection as the selection is empty; you can either add a new item to the end of the list instead, or select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        Dim item As New ScheduleChartItem With {.Content = "New Resource"}
        item.GanttChartItems.Add(New GanttChartItem With {.Content = "New Task", .Start = Date.Today, .Finish = Date.Today.AddDays(1)})
        Me.ScheduleChartDataGrid.Items.Insert(selectedItem.Index, item)
        Me.ScheduleChartDataGrid.SelectedItem = item
        Me.ScheduleChartDataGrid.ScrollTo(item.GanttChartItems(0))
        Me.ScheduleChartDataGrid.ScrollTo(item.GanttChartItems(0).Start)
    End Sub
    Private Sub DeleteButton_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of ScheduleChartItem)()
        For Each item As ScheduleChartItem In Me.ScheduleChartDataGrid.GetSelectedItems()
            items.Add(item)
        Next
        If items.Count <= 0 Then
            MessageBox.Show("Cannot delete the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        items.Reverse()
        For Each item In items
            Me.ScheduleChartDataGrid.Items.Remove(item)
        Next
    End Sub
    Private Sub SetColorButton_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of ScheduleChartItem)()
        For Each item As ScheduleChartItem In Me.ScheduleChartDataGrid.GetSelectedItems()
            items.Add(item)
        Next
        If items.Count <= 0 Then
            MessageBox.Show("Cannot set a custom bar color to the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        For Each item In items
            For Each ganttChartItem As GanttChartItem In item.GanttChartItems
                GanttChartView.SetStandardBarFill(ganttChartItem, TryCast(Me.Resources("CustomStandardBarFill"), Brush))
                GanttChartView.SetStandardBarStroke(ganttChartItem, TryCast(Me.Resources("CustomStandardBarStroke"), Brush))
            Next
        Next
    End Sub
    Private Sub CopyButton_Click(sender As Object, e As RoutedEventArgs)
        If Me.ScheduleChartDataGrid.GetSelectedItemCount() <= 0 Then
            MessageBox.Show("Cannot copy selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        Me.ScheduleChartDataGrid.Copy()
    End Sub
    Private Sub PasteButton_Click(sender As Object, e As RoutedEventArgs)
        Me.ScheduleChartDataGrid.Paste()
    End Sub
    Private Sub UndoButton_Click(sender As Object, e As RoutedEventArgs)
        If Me.ScheduleChartDataGrid.CanUndo() Then
            Me.ScheduleChartDataGrid.Undo()
        Else
            MessageBox.Show("Currently there is no recorded action in the undo queue; perform an action first.", "Information", MessageBoxButton.OK)
        End If
    End Sub
    Private Sub RedoButton_Click(sender As Object, e As RoutedEventArgs)
        If Me.ScheduleChartDataGrid.CanRedo() Then
            Me.ScheduleChartDataGrid.Redo()
        Else
            MessageBox.Show("Currently there is no recorded action in the redo queue; perform an action and undo it first.", "Information", MessageBoxButton.OK)
        End If
    End Sub
    Private Sub ScaleTypeComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim selectedComboBoxItem = TryCast(Me.ScalesComboBox.SelectedItem, ComboBoxItem)
        Dim scalesResourceKey = TryCast(selectedComboBoxItem.Tag, String)
        Dim scales = TryCast(Me.Resources(scalesResourceKey), ScaleCollection)
        Me.ScheduleChartDataGrid.Scales = scales
    End Sub
    Private Sub ZoomCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        originalZoom = Me.ScheduleChartDataGrid.HourWidth
        Me.ScheduleChartDataGrid.HourWidth = originalZoom * 2
    End Sub
    Private Sub ZoomCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        Me.ScheduleChartDataGrid.HourWidth = originalZoom
    End Sub
    Private originalZoom As Double
    Private Sub IncreaseTimelinePageButton_Click(sender As Object, e As RoutedEventArgs)
        Me.ScheduleChartDataGrid.TimelinePageFinish += pageUpdateAmount
        Me.ScheduleChartDataGrid.TimelinePageStart += pageUpdateAmount
    End Sub
    Private Sub DecreaseTimelinePageButton_Click(sender As Object, e As RoutedEventArgs)
        Me.ScheduleChartDataGrid.TimelinePageFinish -= pageUpdateAmount
        Me.ScheduleChartDataGrid.TimelinePageStart -= pageUpdateAmount
    End Sub
    Private ReadOnly pageUpdateAmount As TimeSpan = TimeSpan.FromDays(7)
    Private Sub ShowWeekendsCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        Me.ScheduleChartDataGrid.VisibleWeekStart = DayOfWeek.Sunday
        Me.ScheduleChartDataGrid.VisibleWeekFinish = DayOfWeek.Saturday
        Me.WorkOnWeekendsCheckBox.IsEnabled = True
    End Sub
    Private Sub ShowWeekendsCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        Me.WorkOnWeekendsCheckBox.IsChecked = False
        Me.WorkOnWeekendsCheckBox.IsEnabled = False
        Me.ScheduleChartDataGrid.VisibleWeekStart = DayOfWeek.Monday
        Me.ScheduleChartDataGrid.VisibleWeekFinish = DayOfWeek.Friday
    End Sub
    Private Sub WorkOnWeekendsCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        Me.ScheduleChartDataGrid.WorkingWeekStart = DayOfWeek.Sunday
        Me.ScheduleChartDataGrid.WorkingWeekFinish = DayOfWeek.Saturday
    End Sub
    Private Sub WorkOnWeekendsCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        Me.ScheduleChartDataGrid.WorkingWeekStart = DayOfWeek.Monday
        Me.ScheduleChartDataGrid.WorkingWeekFinish = DayOfWeek.Friday
    End Sub
    Private Sub PrintButton_Click(sender As Object, e As RoutedEventArgs)
        Dim oldStart = Me.ScheduleChartDataGrid.TimelinePageStart
        Dim oldFinish = Me.ScheduleChartDataGrid.TimelinePageFinish

        Try
            Dim dialog As New System.Windows.Controls.PrintDialog()
            Dim timelinePageStart = Me.ScheduleChartDataGrid.GetProjectStart().AddDays(-1)
            Dim timelinePageFinish = Me.ScheduleChartDataGrid.GetProjectFinish().AddDays(1)

            If dialog.ShowDialog() = True Then
                Me.ScheduleChartDataGrid.SetTimelinePage(timelinePageStart, timelinePageFinish)
                Dim exportedSize = Me.ScheduleChartDataGrid.GetExportSize()

                If exportedSize.Width + 2 * 48 <= dialog.PrintableAreaWidth AndAlso exportedSize.Height + 2 * 32 <= dialog.PrintableAreaHeight Then
                    Me.ScheduleChartDataGrid.Export(Sub()
                                                     Dim exportedVisual = Me.ScheduleChartDataGrid.GetExportDrawingVisual()
                                                     exportedVisual.Transform = GetPageFittingTransform(dialog)
                                                     Dim container As New Border()
                                                     container.Padding = New Thickness(48, 32, 48, 32)
                                                     container.Child = New Rectangle With {.Fill = New VisualBrush(exportedVisual), .Width = exportedSize.Width, .Height = exportedSize.Height}
                                                     dialog.PrintVisual(container, "Schedule Chart Document")
                                                 End Sub)
                Else
                    Dim documentPaginator As New DlhSoft.Windows.Controls.GanttChartDataGrid.DocumentPaginator(Me.ScheduleChartDataGrid)
                    documentPaginator.PageSize = New Size(dialog.PrintableAreaWidth, dialog.PrintableAreaHeight)
                    dialog.PrintDocument(documentPaginator, "Schedule Chart Document")
                End If

                Close()
            End If
        Finally
            Dispatcher.BeginInvoke(Sub()
                                       Me.ScheduleChartDataGrid.SetTimelinePage(oldStart, oldFinish)
                                   End Sub)
        End Try
    End Sub
    Private Function GetPageFittingTransform(printDialog As System.Windows.Controls.PrintDialog) As TransformGroup
        Dim scale = GetPageFittingScaleRatio(printDialog)
        Dim transformGroup As New TransformGroup()
        transformGroup.Children.Add(New ScaleTransform(scale, scale))
        Return transformGroup
    End Function
    Private Function GetPageFittingScaleRatio(printDialog As System.Windows.Controls.PrintDialog) As Double
        Dim outputSize = Me.ScheduleChartDataGrid.GetExportSize()
        Dim scaleX = printDialog.PrintableAreaWidth / outputSize.Width
        Dim scaleY = printDialog.PrintableAreaHeight / outputSize.Height
        Return Math.Min(scaleX, scaleY)
    End Function
    Private Sub ExportImageButton_Click(sender As Object, e As RoutedEventArgs)
        Me.ScheduleChartDataGrid.Export(Sub()
                                         Dim saveFileDialog As New SaveFileDialog With {.Title = "Export Image To", .Filter = "PNG image files|*.png"}
                                         If saveFileDialog.ShowDialog() <> True Then
                                             Return
                                         End If
                                         Dim bitmapSource = Me.ScheduleChartDataGrid.GetExportBitmapSource(96 * 2)
                                         Using stream As Stream = saveFileDialog.OpenFile()
                                             Dim pngBitmapEncoder As New PngBitmapEncoder()
                                             pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource))
                                             pngBitmapEncoder.Save(stream)
                                         End Using
                                     End Sub)
    End Sub
    Private Sub AddNewTaskButton_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedItem = TryCast(Me.ScheduleChartDataGrid.SelectedItem, ScheduleChartItem)
        If selectedItem Is Nothing Then
            MessageBox.Show("Cannot add a new task if the selection of a resource is empty; you can either add a new resource to the end of the list instead, or select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If

        Dim task As New CustomGanttChartItem With {.Content = "New Task", .Start = Date.Today, .Finish = Date.Today.AddDays(1)}
        selectedItem.GanttChartItems.Insert(0, task)
        Me.ScheduleChartDataGrid.ScrollTo(task.Start)
    End Sub
    Private Shared Sub DeleteTask(itemToDelete As CustomGanttChartItem)
        Dim timer As New DispatcherTimer With {.Interval = TimeSpan.FromMilliseconds(5)}
        AddHandler timer.Tick, Sub(ts, te)
                                   Dim opacity = GetOpacity(itemToDelete)
                                   opacity -= 0.05
                                   If opacity <= 0 Then
                                       timer.Stop()
                                       Dim parent = itemToDelete.ScheduleChartItem
                                       parent.GanttChartItems.Remove(itemToDelete)
                                   End If
                                   SetOpacity(itemToDelete, opacity)
                               End Sub
        timer.Start()
    End Sub
    Private Sub DeleteTaskMenuItem_Click(sender As Object, e As RoutedEventArgs)
        Dim itemToDelete = TryCast(TryCast(sender, MenuItem).DataContext, CustomGanttChartItem)
        If itemToDelete Is Nothing Then Return
        If MessageBox.Show("Are you sure you want to delete this task?", "Question", MessageBoxButton.YesNo) <> MessageBoxResult.Yes Then
            Return
        End If
        DeleteTask(itemToDelete)
    End Sub
    Private Sub DeleteTaskButton_Click(sender As Object, e As RoutedEventArgs)
        If SelectedItem Is Nothing Then
            MessageBox.Show("Cannot delete a task if the selection is empty.", "Information", MessageBoxButton.OK)
            Return
        End If
        Dim itemToDelete = TryCast(SelectedItem, CustomGanttChartItem)
        If itemToDelete Is Nothing Then Return
        If MessageBox.Show("Are you sure you want to delete this task?", "Question", MessageBoxButton.YesNo) <> MessageBoxResult.Yes Then
            Return
        End If
        DeleteTask(itemToDelete)
    End Sub
    Private draggedElement As FrameworkElement
    Private hoveredElement As FrameworkElement
    Private draggedItem As CustomScheduleChartItem

    Private Const DraggingOpacity As Double = 0.35
    Private Const TargetOpacity As Double = 0.55

    Private Sub Thumb_DragStarted(sender As Object, e As DragStartedEventArgs)
        draggedElement = TryCast(sender, Thumb)
        draggedItem = If(draggedElement IsNot Nothing, TryCast(draggedElement.DataContext, CustomScheduleChartItem), Nothing)
        If draggedItem Is Nothing Then Return

        Dim includeChildren = draggedItem.Indentation = 0
        SetDragOpacity(draggedItem, DraggingOpacity, includeChildren)

        Dim row = FindAncestor(Of DataGridRow)(draggedElement)
        If row IsNot Nothing Then row.Opacity = 0.9
    End Sub

    Private Sub Thumb_DragDelta(sender As Object, e As DragDeltaEventArgs)
        If draggedItem Is Nothing Then Return

        Dim p = Mouse.GetPosition(ScheduleChartDataGrid.DataTreeGrid)
        AutoScrollOnDrag(p)

        Dim result = VisualTreeHelper.HitTest(ScheduleChartDataGrid.DataTreeGrid, p)
        Dim element = If(result IsNot Nothing, TryCast(result.VisualHit, FrameworkElement), Nothing)
        If element Is Nothing Then Return

        Dim newHovered As FrameworkElement = FindAncestor(Of DataGridRow)(element)
        If newHovered Is Nothing Then Return

        If Not ReferenceEquals(newHovered, hoveredElement) Then
            If hoveredElement IsNot Nothing Then SetHoverVisual(hoveredElement, False)

            Dim hoveredItem = TryCast(newHovered.DataContext, CustomScheduleChartItem)
            If hoveredItem Is Nothing Then
                hoveredElement = Nothing
                Return
            End If
            If Not IsValidDropTarget(draggedItem, hoveredItem) Then
                hoveredElement = Nothing
                Return
            End If

            hoveredElement = newHovered
            SetHoverVisual(hoveredElement, True)
        End If
    End Sub

    Private Sub Thumb_DragCompleted(sender As Object, e As DragCompletedEventArgs)
        If hoveredElement IsNot Nothing Then SetHoverVisual(hoveredElement, False)
        If draggedElement IsNot Nothing Then
            SetHoverVisual(draggedElement, False)
            Dim row = FindAncestor(Of DataGridRow)(draggedElement)
            If row IsNot Nothing Then row.Opacity = 1.0
        End If

        Dim targetItem = If(hoveredElement IsNot Nothing, TryCast(hoveredElement.DataContext, CustomScheduleChartItem), Nothing)
        If draggedItem Is Nothing OrElse targetItem Is Nothing Then Return

        Dim includeChildren = draggedItem.Indentation = 0
        SetDragOpacity(draggedItem, 1.0, includeChildren)

        If draggedItem.Indentation = 0 Then
            MoveParent(draggedItem, targetItem, e.VerticalChange)
            FocusAndScrollToItem(draggedItem)
            Return
        End If
        If draggedItem.Indentation = 1 Then
            MoveChild(draggedItem, targetItem, e.VerticalChange)
            FocusAndScrollToItem(draggedItem)
            Return
        End If
    End Sub

    Private Function IsValidDropTarget(dragged As CustomScheduleChartItem, hovered As CustomScheduleChartItem) As Boolean
        If dragged Is Nothing OrElse hovered Is Nothing Then Return False
        If ReferenceEquals(dragged, hovered) Then Return False

        ' Parent groups can only be dropped on other parent rows.
        If dragged.Indentation = 0 Then Return hovered.Indentation = 0

        ' Children can move between groups, except into the pinned first row.
        If dragged.Indentation = 1 Then
            If hovered.Indentation = 0 Then Return GetLiveIndex(hovered) <> 0
            If hovered.Indentation = 1 Then
                Dim hoveredParent = TryCast(hovered.Parent, CustomScheduleChartItem)
                If hoveredParent Is Nothing Then Return False
                Return GetLiveIndex(hoveredParent) <> 0
            End If
        End If
        Return False
    End Function

    ' Move the parent and its children together, preserving their order.
    Private Sub MoveParent(draggedParent As CustomScheduleChartItem, targetParent As CustomScheduleChartItem, verticalChange As Double)
        If draggedParent Is Nothing OrElse targetParent Is Nothing Then Return
        If draggedParent.Indentation <> 0 OrElse targetParent.Indentation <> 0 Then Return

        Dim startIndex, endIndex As Integer
        GetGroupRangeLive(draggedParent, startIndex, endIndex)
        If startIndex < 0 OrElse endIndex < startIndex Then Return

        Dim targetStart, targetEnd As Integer
        GetGroupRangeLive(targetParent, targetStart, targetEnd)
        If targetStart < 0 OrElse targetEnd < targetStart Then Return

        Dim movingDown = startIndex < targetStart
        If Not movingDown AndAlso targetStart = 0 Then Return

        MoveGroupBlockAtomic(startIndex, endIndex, targetStart, targetEnd, movingDown)
    End Sub

    ' Children stay at indentation 1 and cannot move into the pinned first row.
    Private Sub MoveChild(draggedChild As CustomScheduleChartItem, target As CustomScheduleChartItem, verticalChange As Double)
        If draggedChild Is Nothing OrElse target Is Nothing Then Return
        If draggedChild.Indentation <> 1 Then Return

        If target.Indentation = 0 Then
            If GetLiveIndex(target) = 0 Then Return
            Dim insertIndex = GetLiveIndex(target) + 1
            If insertIndex <= 0 Then insertIndex = 1
            ScheduleChartDataGrid.Move(draggedChild, insertIndex)
            Return
        End If

        If target.Indentation = 1 Then
            Dim targetParent = TryCast(target.Parent, CustomScheduleChartItem)
            If targetParent Is Nothing Then Return
            If GetLiveIndex(targetParent) = 0 Then Return

            Dim targetIndex = GetLiveIndex(target)
            Dim insertIndex = targetIndex + If(verticalChange > 0, 1, 0)
            If insertIndex <= 0 Then insertIndex = 1
            ScheduleChartDataGrid.Move(draggedChild, insertIndex)
        End If
    End Sub

    Private Sub MoveGroupBlockAtomic(startIndex As Integer, endIndex As Integer, targetStart As Integer, targetEnd As Integer, movingDown As Boolean)
        Dim blockCount = endIndex - startIndex + 1
        Dim block As New List(Of ScheduleChartItem)(blockCount)
        For i As Integer = startIndex To endIndex
            block.Add(ScheduleChartDataGrid.Items(i))
        Next
        For i As Integer = endIndex To startIndex Step -1
            ScheduleChartDataGrid.Items.RemoveAt(i)
        Next

        If targetStart > startIndex Then
            targetStart -= blockCount
            targetEnd -= blockCount
        End If

        Dim insertIndex = If(movingDown, targetEnd + 1, targetStart)
        If insertIndex <= 0 Then insertIndex = 1
        For i As Integer = 0 To block.Count - 1
            ScheduleChartDataGrid.Items.Insert(insertIndex + i, block(i))
        Next
    End Sub

    Private Sub GetGroupRangeLive(parent As CustomScheduleChartItem, ByRef startIndex As Integer, ByRef endIndex As Integer)
        startIndex = -1
        endIndex = -1
        If parent Is Nothing OrElse parent.Indentation <> 0 Then Return

        startIndex = ScheduleChartDataGrid.Items.IndexOf(parent)
        If startIndex < 0 Then Return
        endIndex = startIndex

        ' A group's children are contiguous, up to the next parent.
        For i As Integer = startIndex + 1 To ScheduleChartDataGrid.Items.Count - 1
            Dim item = TryCast(ScheduleChartDataGrid.Items(i), CustomScheduleChartItem)
            If item Is Nothing Then Continue For
            If item.Indentation = 0 Then Exit For
            endIndex = i
        Next
    End Sub

    Private Function GetLiveIndex(item As CustomScheduleChartItem) As Integer
        Return ScheduleChartDataGrid.Items.IndexOf(item)
    End Function

    Private Shared Function FindVisualChild(Of T As DependencyObject)(parent As DependencyObject) As T
        For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(parent) - 1
            Dim child = VisualTreeHelper.GetChild(parent, i)
            Dim found = TryCast(child, T)
            If found IsNot Nothing Then Return found

            Dim result = FindVisualChild(Of T)(child)
            If result IsNot Nothing Then Return result
        Next
        Return Nothing
    End Function

    Private Sub AutoScrollOnDrag(mousePos As Point)
        Dim scrollViewer = FindVisualChild(Of ScrollViewer)(ScheduleChartDataGrid)
        If scrollViewer Is Nothing Then Return
        Dim edgeThreshold As Double = 40
        Dim scrollSpeed As Double = 1

        If mousePos.Y < edgeThreshold Then
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - scrollSpeed)
        ElseIf mousePos.Y > ScheduleChartDataGrid.ActualHeight - edgeThreshold Then
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + scrollSpeed)
        End If
    End Sub

    Private Sub FocusAndScrollToItem(item As CustomScheduleChartItem)
        If item Is Nothing Then Return
        ScheduleChartDataGrid.SelectedItem = item
        ScheduleChartDataGrid.Dispatcher.BeginInvoke(New Action(Sub()
                                                                  ScheduleChartDataGrid.UpdateLayout()
                                                                  ScheduleChartDataGrid.ScrollIntoView(item)
                                                                  ScheduleChartDataGrid.Focus()
                                                              End Sub), DispatcherPriority.Background)
    End Sub

    Private Sub SetDragOpacity(item As CustomScheduleChartItem, opacity As Double, includeChildren As Boolean)
        If item Is Nothing Then Return
        item.DragOpacity = opacity
        If Not includeChildren Then Return

        For Each child In item.AllChildren
            Dim customChild = TryCast(child, CustomScheduleChartItem)
            If customChild IsNot Nothing Then customChild.DragOpacity = opacity
        Next
    End Sub

    Private Shared Function FindAncestor(Of T As DependencyObject)(element As DependencyObject) As T
        While element IsNot Nothing AndAlso Not TypeOf element Is T
            element = VisualTreeHelper.GetParent(element)
        End While
        Return DirectCast(element, T)
    End Function

    Private Sub SetHoverVisual(element As FrameworkElement, isHover As Boolean)
        If element Is Nothing Then Return
        element.Opacity = If(isHover, TargetOpacity, 1.0)
    End Sub
End Class