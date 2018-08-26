Imports System.Text
Imports DlhSoft.Windows.Controls

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim allocation1 As AllocationItem = LoadChartView.Items(0).GanttChartItems(0)
        allocation1.Start = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        allocation1.Finish = Date.Today.Add(TimeSpan.Parse("16:00:00"))

        Dim allocation12 As AllocationItem = LoadChartView.Items(0).GanttChartItems(1)
        allocation12.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        allocation12.Finish = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"))
        allocation12.Units = 1.5

        Dim allocation2 As AllocationItem = LoadChartView.Items(0).GanttChartItems(2)
        allocation2.Start = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"))
        allocation2.Finish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        allocation2.Units = 0.5

        Dim allocation3 As AllocationItem = LoadChartView.Items(0).GanttChartItems(3)
        allocation3.Start = Date.Today.AddDays(4).Add(TimeSpan.Parse("08:00:00"))
        allocation3.Finish = Date.Today.AddDays(4).Add(TimeSpan.Parse("16:00:00"))
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
        LoadChartView.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub
End Class
