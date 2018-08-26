Imports System.Text
Imports System.Collections.ObjectModel
Imports DlhSoft.Windows.Controls

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim taskItems = New ObservableCollection(Of CustomTaskItem) From {
                New CustomTaskItem With {.Name = "Task 1", .Description = "Description of custom task 1"},
                New CustomTaskItem With {.Name = "Task 1.1", .IndentLevel = 1, .StartDate = Date.Today.Add(TimeSpan.Parse("08:00:00")), .FinishDate = Date.Today.Add(TimeSpan.Parse("16:00:00")), .CompletionCurrentDate = Date.Today.Add(TimeSpan.Parse("12:00:00")), .AssignmentsString = "Resource 1", .Description = "Description of custom task 1.1"},
                New CustomTaskItem With {.Name = "Task 1.2", .IndentLevel = 1, .StartDate = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00")), .FinishDate = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00")), .AssignmentsString = "Resource 1, Resource 2", .Description = "Description of custom task 1.2"},
                New CustomTaskItem With {.Name = "Task 2", .Description = "Description of custom task 2"},
                New CustomTaskItem With {.Name = "Task 2.1", .IndentLevel = 1, .StartDate = Date.Today.Add(TimeSpan.Parse("08:00:00")), .FinishDate = Date.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00")), .Description = "Description of custom task 2.1"},
                New CustomTaskItem With {.Name = "Task 2.2", .IndentLevel = 1, .StartDate = Date.Today.Add(TimeSpan.Parse("08:00:00")), .FinishDate = Date.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), .Description = "Description of custom task 2.2"},
                New CustomTaskItem With {.Name = "Task 3", .IndentLevel = 0, .StartDate = Date.Today.AddDays(5), .Milestone = True, .Description = "Description of custom task 3"}
            }
        taskItems(2).Predecessors = New ObservableCollection(Of CustomPredecessorItem) From {New CustomPredecessorItem With {.Reference = taskItems(1)}}
        taskItems(3).Predecessors = New ObservableCollection(Of CustomPredecessorItem) From {New CustomPredecessorItem With {.Reference = taskItems(0), .Type = CInt(Fix(DependencyType.StartStart))}}
        taskItems(6).Predecessors = New ObservableCollection(Of CustomPredecessorItem) From {
                New CustomPredecessorItem With {.Reference = taskItems(0)},
                New CustomPredecessorItem With {.Reference = taskItems(3)}
            }

        For i As Integer = 4 To 25
            taskItems.Add(New CustomTaskItem With {.Name = "Task " & i, .IndentLevel = If(i Mod 3 = 1, 0, 1), .StartDate = Date.Today.AddDays(If(i <= 8, (i - 4) * 3, i - 8)), .FinishDate = Date.Today.AddDays((If(i <= 8, (i - 4) * 3 + (If(i > 8, 6, 1)), i - 2)) + 1), .CompletionCurrentDate = Date.Today.AddDays(If(i <= 8, (i - 4) * 3, i - 8)).AddDays(If(i Mod 6 = 0, 3, 0))})
        Next i

        GanttChartDataGrid.DataContext = taskItems
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
