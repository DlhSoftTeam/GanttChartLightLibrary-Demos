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
        item1.MyValue1 = 1 ' Property of inherinting object.
        item1.MyValue2 = "Item 1 Value 2"
        item1.Tag = New CustomDataObject With {.MyValue3 = "Item 1 Value 3", .MyValue4 = "Item 1 Value 4"} ' Could be a database object.
        GanttChartItemAttachments.SetMyValue5(item1, "Item 1 Value 5") ' Attached property.
        item1.SetValue(GanttChartItemAttachments.MyValue6Property, "Item 1 Value 6") ' Another way to set an attached property.

        Dim item2 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(2), CustomGanttChartItem)
        item2.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item2.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item2.AssignmentsContent = "Resource 1, Resource 2"
        item2.Predecessors.Add(New PredecessorItem With {.Item = item1})
        item2.MyValue1 = 2 ' Property of inherinting object.
        item2.MyValue2 = "Item 2 Value 2"
        item2.Tag = New CustomDataObject With {.MyValue3 = "Item 2 Value 3", .MyValue4 = "Item 2 Value 4"} ' Could be a database object.
        GanttChartItemAttachments.SetMyValue5(item2, "Item 2 Value 5") ' Attached property.
        item2.SetValue(GanttChartItemAttachments.MyValue6Property, "Item 2 Value 6") ' Another way to set an attached property.

        Dim item3 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(3), CustomGanttChartItem)
        item3.Predecessors.Add(New PredecessorItem With {.Item = item0, .DependencyType = DependencyType.StartStart})

        Dim item4 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(4), CustomGanttChartItem)
        item4.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item4.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"))

        Dim item6 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(6), CustomGanttChartItem)
        item6.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item6.Finish = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"))
        item6.MyValue1 = 6

        Dim item7 As CustomGanttChartItem = TryCast(GanttChartDataGrid.Items(7), CustomGanttChartItem)
        item7.Start = Date.Today.AddDays(4)
        item7.IsMilestone = True
        item7.Predecessors.Add(New PredecessorItem With {.Item = item4})
        item7.Predecessors.Add(New PredecessorItem With {.Item = item6})

        For i As Integer = 3 To 25
            GanttChartDataGrid.Items.Add(New CustomGanttChartItem With {.Content = "Task " & i, .Indentation = If(i Mod 3 = 0, 0, 1), .Start = Date.Today.AddDays(If(i <= 8, (i - 4) * 2, i - 8)), .Finish = Date.Today.AddDays((If(i <= 8, (i - 4) * 2 + (If(i > 8, 6, 1)), i - 2)) + 2), .CompletedFinish = Date.Today.AddDays(If(i <= 8, (i - 4) * 2, i - 8)).AddDays(If(i Mod 6 = 4, 3, 0))})
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

    Private Sub AddNewButton_Click(sender As Object, e As RoutedEventArgs)
        Dim item As GanttChartItem = New CustomGanttChartItem With {.Content = "New Task", .Start = Date.Today, .Finish = Date.Today.AddDays(1)}
        GanttChartDataGrid.Items.Add(item)
        GanttChartDataGrid.SelectedItem = item
        GanttChartDataGrid.ScrollTo(item)
    End Sub
    Private Sub InsertNewButton_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedItem As GanttChartItem = TryCast(GanttChartDataGrid.SelectedItem, GanttChartItem)
        If selectedItem Is Nothing Then
            MessageBox.Show("Cannot insert a new item before selection as the selection is empty; you can either add a new item to the end of the list instead, or select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        Dim item As GanttChartItem = New CustomGanttChartItem With {.Content = "New Task", .Indentation = selectedItem.Indentation, .Start = Date.Today, .Finish = Date.Today.AddDays(1)}
        GanttChartDataGrid.Items.Insert(GanttChartDataGrid.SelectedIndex, item)
        GanttChartDataGrid.SelectedItem = item
        GanttChartDataGrid.ScrollTo(item)
    End Sub
    Private Sub DeleteButton_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of GanttChartItem)()
        For Each item As GanttChartItem In GanttChartDataGrid.GetSelectedItems()
            items.Add(item)
        Next item
        If items.Count <= 0 Then
            MessageBox.Show("Cannot delete the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        items.Reverse()
        For Each item As GanttChartItem In items
            If item.HasChildren Then
                MessageBox.Show(String.Format("Cannot delete {0} because it has child items; remove its child items first.", item), "Information", MessageBoxButton.OK)
                Continue For
            End If
            GanttChartDataGrid.Items.Remove(item)
        Next item
    End Sub
    Private Sub IncreaseIndentationButton_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of GanttChartItem)()
        For Each item As GanttChartItem In GanttChartDataGrid.GetSelectedItems()
            items.Add(item)
        Next item
        If items.Count <= 0 Then
            MessageBox.Show("Cannot increase indentation for the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        For Each item As GanttChartItem In items
            Dim index As Integer = GanttChartDataGrid.IndexOf(item)
            If index > 0 Then
                Dim previousItem As GanttChartItem = GanttChartDataGrid(index - 1)
                If item.Indentation <= previousItem.Indentation Then
                    item.Indentation += 1
                Else
                    MessageBox.Show(String.Format("Cannot increase indentation for {0} because the previous item is its parent item; increase the indentation of its parent item first.", item), "Information", MessageBoxButton.OK)
                End If
            Else
                MessageBox.Show(String.Format("Cannot increase indentation for {0} because it is the first item; insert an item before this one first.", item), "Information", MessageBoxButton.OK)
            End If
        Next item
    End Sub
    Private Sub DecreaseIndentationButton_Click(sender As Object, e As RoutedEventArgs)
        Dim items As New List(Of GanttChartItem)()
        For Each item As GanttChartItem In GanttChartDataGrid.GetSelectedItems()
            items.Add(item)
        Next item
        If items.Count <= 0 Then
            MessageBox.Show("Cannot decrease indentation for the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK)
            Return
        End If
        items.Reverse()
        For Each item As GanttChartItem In items
            If item.Indentation > 0 Then
                Dim index As Integer = GanttChartDataGrid.IndexOf(item)
                Dim nextItem As GanttChartItem = If(index < GanttChartDataGrid.Items.Count - 1, GanttChartDataGrid(index + 1), Nothing)
                If nextItem Is Nothing OrElse item.Indentation >= nextItem.Indentation Then
                    item.Indentation -= 1
                Else
                    MessageBox.Show(String.Format("Cannot increase indentation for {0} because the next item is one of its child items; decrease the indentation of its child items first.", item), "Information", MessageBoxButton.OK)
                End If
            Else
                MessageBox.Show(String.Format("Cannot decrease indentation for {0} because it is on the first indentation level, being a root item.", item), "Information", MessageBoxButton.OK)
            End If
        Next item
    End Sub
End Class
