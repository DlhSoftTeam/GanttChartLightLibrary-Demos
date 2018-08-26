Imports DlhSoft.Windows.Controls

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim unassignedScheduleChartItem = New ScheduleChartItem With {.Content = "(Unassigned)"}
        For i As Integer = 1 To 12
            unassignedScheduleChartItem.GanttChartItems.Add(New GanttChartItem With {.Content = "Task " & i, .Start = Date.Today.AddDays(i), .Finish = Date.Today.AddDays(i + 4)})
        Next i
        ScheduleChartDataGrid.Items.Add(unassignedScheduleChartItem)

        For i As Integer = 1 To 8
            Dim item As ScheduleChartItem = New ScheduleChartItem With {.Content = "Resource " & i}
            For j As Integer = 1 To (i - 1) Mod 4 + 1
                item.GanttChartItems.Add(New GanttChartItem With {.Content = "Task " & i & "." & j, .Start = Date.Today.AddDays(i), .Finish = Date.Today.AddDays(i + j + 2), .CompletedFinish = Date.Today.AddDays(i)})
            Next j
            ScheduleChartDataGrid.Items.Add(item)
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
        ScheduleChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub
End Class
