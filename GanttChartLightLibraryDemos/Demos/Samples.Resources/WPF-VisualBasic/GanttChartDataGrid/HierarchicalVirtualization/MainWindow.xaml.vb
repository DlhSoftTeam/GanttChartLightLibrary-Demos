Imports DlhSoft.Windows.Controls
Imports System.Collections.ObjectModel

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Private year As Integer = Date.Now.Year, month As Integer = Date.Now.Month

    Public Sub New()
        InitializeComponent()

        ' Load root items only, and ensure IsExpanded is set to false on all of them.
        ' For each root item that is going to be a summary node, load a dummy child item as well sharing the 
        ' (pre-cached) aggregated values of the child items, to ensure root items are displayed as summary items. 
        ' We will remove the dummy items once the summary items are expanded (see below).
        GanttChartDataGrid.Items = New ObservableCollection(Of GanttChartItem)() From {
                New GanttChartItem() With {.Content = "Task 1", .IsExpanded = False},
                New GanttChartItem() With {.Content = "Task 1 hierarchy placeholder", .IsHidden = True, .Indentation = 1, .Start = New Date(year, month, 2, 8, 0, 0), .Finish = New Date(year, month, 5, 12, 0, 0)},
                New GanttChartItem() With {.Content = "Task 2", .IsExpanded = False},
                New GanttChartItem() With {.Content = "Task 2 hierarchy placeholder", .IsHidden = True, .Indentation = 1, .Start = New Date(year, month, 2, 8, 0, 0), .Finish = New Date(year, month, 14, 16, 0, 0)},
                New GanttChartItem() With {.Content = "Task 3", .Start = New Date(year, month, 15, 16, 0, 0), .IsMilestone = True}
            }

        GanttChartDataGrid.TimelinePageStart = New Date(year, month, 1)
        GanttChartDataGrid.DisplayedTime = New Date(year, month, 1)
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

    ' Detect expansion changes and load child items (summary child items will be loaded in collapsed state, 
    ' with dummy nodes as well, to make this work recursively).
    Private Sub GanttChartDataGrid_ItemPropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs)
        Dim item As GanttChartItem = TryCast(sender, GanttChartItem)
        If e.PropertyName = "IsExpanded" AndAlso item.IsExpanded Then
            Dim index As Integer = GanttChartDataGrid.Items.IndexOf(item) + 1
            Dim nextItem As GanttChartItem = If(index < GanttChartDataGrid.Items.Count, GanttChartDataGrid.Items(index), Nothing)

            ' Ensure we do not try loading hierarchy for the last node nor reload hierarchies for previously expanded nodes.
            If nextItem Is Nothing OrElse (Not nextItem.IsHidden) Then
                Return
            End If

            ' Determine the actual sub-items of the expanded summary item.
            Dim subItems = New List(Of GanttChartItem)()
            Select Case TryCast(item.Content, String)
                Case "Task 1"
                    subItems.Add(New GanttChartItem With {.Content = "Task 1.1", .Start = New Date(year, month, 2, 8, 0, 0), .Finish = New Date(year, month, 4, 16, 0, 0)})
                    subItems.Add(New GanttChartItem With {.Content = "Task 1.2", .Start = New Date(year, month, 3, 8, 0, 0), .Finish = New Date(year, month, 5, 12, 0, 0)})
                Case "Task 2"
                    subItems.Add(New GanttChartItem With {.Content = "Task 2.1", .Start = New Date(year, month, 2, 8, 0, 0), .Finish = New Date(year, month, 8, 16, 0, 0), .CompletedFinish = New Date(year, month, 5, 16, 0, 0), .AssignmentsContent = "Resource 1, Resource 2 [50%]"})
                    subItems.Add(New GanttChartItem With {.Content = "Task 2.2"})
                    ' When needed, add subsquent levels of hierarchy placeholders.
                    subItems.Add(New GanttChartItem With {.Content = "Task 2.2 hierarchy placeholder", .IsHidden = True, .Start = New Date(year, month, 11, 8, 0, 0), .Finish = New Date(year, month, 14, 16, 0, 0)})
                Case "Task 2.2"
                    subItems.Add(New GanttChartItem With {.Content = "Task 2.2.1", .Start = New Date(year, month, 11, 8, 0, 0), .Finish = New Date(year, month, 14, 16, 0, 0), .CompletedFinish = New Date(year, month, 14, 16, 0, 0), .AssignmentsContent = "Resource 2"})
                    subItems.Add(New GanttChartItem With {.Content = "Task 2.2.2", .Start = New Date(year, month, 12, 12, 0, 0), .Finish = New Date(year, month, 14, 16, 0, 0), .AssignmentsContent = "Resource 2"})
            End Select

            ' Replace the original hierarchy placeholder with the actual sub-items and prepare subsquent hierarchy levels.
            For Each subItem In subItems
                subItem.Indentation = nextItem.Indentation
                If subItem.IsHidden Then
                    subItem.Indentation += 1
                End If
                subItem.IsExpanded = False
                GanttChartDataGrid.Items.Insert(index, subItem)
                index += 1
            Next subItem
            GanttChartDataGrid.Items.Remove(nextItem)
        End If
    End Sub
End Class
