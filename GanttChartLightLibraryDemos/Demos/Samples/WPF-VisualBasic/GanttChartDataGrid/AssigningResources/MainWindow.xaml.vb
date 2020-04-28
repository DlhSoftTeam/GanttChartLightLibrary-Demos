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
        item2.AssignmentsContent = "Resource 1 [200%], Resource 2"
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

        ' Define assignable resources.
        GanttChartDataGrid.AssignableResources = New ObservableCollection(Of String) From {"Resource 1", "Resource 2", "Resource 3", "Material 1", "Material 2"}

        ' Define the quantity values to consider when leveling resources, indicating maximum material amounts available for use at the same time.
        GanttChartDataGrid.ResourceQuantities = New Dictionary(Of String, Double) From {{"Material 1", 4}, {"Material 2", Double.PositiveInfinity}}
        item4.AssignmentsContent = "Material 1 [300%]"
        item6.AssignmentsContent = "Resource 2, Material 2"

        ' Define task and resource costs.
        GanttChartDataGrid.TaskInitiationCost = 5
        item4.ExecutionCost = 50
        GanttChartDataGrid.DefaultResourceUsageCost = 10
        GanttChartDataGrid.SpecificResourceUsageCosts = New Dictionary(Of String, Double) From {{"Resource 1", 2}, {"Material 1", 7}}
        GanttChartDataGrid.SpecificResourceHourCosts = New Dictionary(Of String, Double) From {{"Resource 2", 20}, {"Material 2", 0.5}}
    End Sub

    Private themeResourceDictionary As ResourceDictionary
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
        themeResourceDictionary = New ResourceDictionary With {.Source = New Uri("/" & Me.GetType().Assembly.GetName().Name & ";component/Themes/" & theme & ".xaml", UriKind.Relative)}
        GanttChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub

    Private Sub AddNewButton_Click(sender As Object, e As RoutedEventArgs)
        Dim item As GanttChartItem = New GanttChartItem With {.Content = "New Task", .Start = Date.Today, .Finish = Date.Today.AddDays(1)}
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
        Dim item As GanttChartItem = New GanttChartItem With {.Content = "New Task", .Indentation = selectedItem.Indentation, .Start = Date.Today, .Finish = Date.Today.AddDays(1)}
        GanttChartDataGrid.Items.Insert(selectedItem.Index, item)
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
    Private Sub EditResourcesButton_Click(sender As Object, e As RoutedEventArgs)
        Dim originalOpacity As Double = Opacity
        Opacity = 0.5
        Dim dockPanel = New DockPanel()
        Dim resourceItems = GanttChartDataGrid.AssignableResources
        Dim resourceListBox = New ListBox With {.ItemsSource = resourceItems, .SelectionMode = SelectionMode.Extended, .TabIndex = 2, .Margin = New Thickness(4)}
        Dim commandsDockPanel = New DockPanel With {.Margin = New Thickness(4, 0, 4, 4)}
        System.Windows.Controls.DockPanel.SetDock(commandsDockPanel, Dock.Bottom)
        dockPanel.Children.Add(commandsDockPanel)
        Dim newResourceTextBox = New TextBox With {.TabIndex = 0, .Margin = New Thickness(0, 0, 4, 0)}
        Dim addResourceButton = New Button With {.Content = "Add", .IsDefault = True, .TabIndex = 1, .Margin = New Thickness(0, 0, 4, 0)}
        System.Windows.Controls.DockPanel.SetDock(addResourceButton, Dock.Right)
        Dim deleteResourcesButton = New Button With {.Content = "Delete", .TabIndex = 3, .Margin = New Thickness(4, 0, 0, 0)}
        System.Windows.Controls.DockPanel.SetDock(deleteResourcesButton, Dock.Right)
        commandsDockPanel.Children.Add(deleteResourcesButton)
        commandsDockPanel.Children.Add(addResourceButton)
        commandsDockPanel.Children.Add(newResourceTextBox)
        dockPanel.Children.Add(resourceListBox)
        AddHandler addResourceButton.Click, Sub()
                                                Dim newResource = newResourceTextBox.Text
                                                If (Not String.IsNullOrEmpty(newResource)) AndAlso (Not resourceItems.Contains(newResource)) Then
                                                    resourceItems.Add(newResource)
                                                End If
                                                newResourceTextBox.Clear()
                                                resourceListBox.SelectedItem = newResource
                                            End Sub
        AddHandler deleteResourcesButton.Click, Sub()
                                                    Dim removedResources As New List(Of String)()
                                                    For Each resource As String In resourceListBox.SelectedItems
                                                        removedResources.Add(resource)
                                                    Next resource
                                                    For Each resource As String In removedResources
                                                        resourceItems.Remove(resource)
                                                    Next resource
                                                    newResourceTextBox.Clear()
                                                End Sub
        Dim resourceWindow As Window = New Window With {.Owner = Application.Current.MainWindow, .Title = "Resources", .WindowStartupLocation = WindowStartupLocation.CenterOwner, .Width = 640, .Height = 300, .ResizeMode = ResizeMode.CanMinimize, .Content = dockPanel}
        resourceWindow.ShowDialog()
        Opacity = originalOpacity
    End Sub
End Class