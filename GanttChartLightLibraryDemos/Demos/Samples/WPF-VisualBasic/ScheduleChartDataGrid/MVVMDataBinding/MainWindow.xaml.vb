Imports System.Collections.ObjectModel

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim resourceItems = New ObservableCollection(Of CustomResourceItem) From {
                New CustomResourceItem With {.Name = "Resource 1", .Description = "Description of custom resource 1", .AssignedTasks = New ObservableCollection(Of CustomTaskItem) From {New CustomTaskItem With {.Name = "Task 1", .StartDate = Date.Today.Add(TimeSpan.Parse("08:00:00")), .FinishDate = Date.Today.Add(TimeSpan.Parse("16:00:00")), .CompletionCurrentDate = Date.Today.Add(TimeSpan.Parse("12:00:00"))}, New CustomTaskItem With {.Name = "Task 2", .StartDate = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00")), .FinishDate = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00")), .AssignmentsString = "50%"}}},
                New CustomResourceItem With {.Name = "Resource 2", .Description = "Description of custom resource 2", .AssignedTasks = New ObservableCollection(Of CustomTaskItem) From {New CustomTaskItem With {.Name = "Task 2", .StartDate = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00")), .FinishDate = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))}}}
            }

        For i As Integer = 3 To 16
            Dim item As CustomResourceItem = New CustomResourceItem With {.Name = "Resource " & i, .Description = "Description of custom resource " & i, .AssignedTasks = New ObservableCollection(Of CustomTaskItem)()}
            For j As Integer = 1 To (i - 1) Mod 4 + 1
                item.AssignedTasks.Add(New CustomTaskItem With {.Name = "Task " & i & "." & j, .StartDate = Date.Today.AddDays(i + (i - 1) * (j - 1)), .FinishDate = Date.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1), .CompletionCurrentDate = Date.Today.AddDays(i + (i - 1) * (j - 1)).AddDays(If((i + j) Mod 5 = 2, 2, 0))})
            Next j
            resourceItems.Add(item)
        Next i

        ScheduleChartDataGrid.DataContext = resourceItems
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
        ScheduleChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub
End Class
