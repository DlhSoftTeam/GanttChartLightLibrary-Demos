Imports DlhSoft.Windows.Controls

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim allocation11 As AllocationItem = LoadChartDataGrid.Items(0).GanttChartItems(0)
        allocation11.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        allocation11.Finish = Date.Today.Add(TimeSpan.Parse("16:00:00"))

        Dim allocation112 As AllocationItem = LoadChartDataGrid.Items(0).GanttChartItems(1)
        allocation112.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        allocation112.Finish = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"))
        allocation112.Units = 1.5

        Dim allocation12 As AllocationItem = LoadChartDataGrid.Items(0).GanttChartItems(2)
        allocation12.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"))
        allocation12.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        allocation12.Units = 0.5

        Dim allocation13 As AllocationItem = LoadChartDataGrid.Items(0).GanttChartItems(3)
        allocation13.Start = Date.Today.AddDays(4).Add(TimeSpan.Parse("08:00:00"))
        allocation13.Finish = Date.Today.AddDays(4).Add(TimeSpan.Parse("16:00:00"))

        Dim allocation22 As AllocationItem = LoadChartDataGrid.Items(1).GanttChartItems(0)
        allocation22.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        allocation22.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))

        For i As Integer = 3 To 16
            Dim item As LoadChartItem = New LoadChartItem With {.Content = "Resource " & i}
            For j As Integer = 1 To (i - 1) Mod 4 + 1
                item.GanttChartItems.Add(New AllocationItem With {.Content = "Task " & i & "." & j & (If((i + j) Mod 2 = 1, " [200%]", String.Empty)), .Start = Date.Today.AddDays(i + (i - 1) * (j - 1)), .Finish = Date.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1), .Units = 1 + (i + j) Mod 2})
            Next j
            LoadChartDataGrid.Items.Add(item)
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
        LoadChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub
End Class
