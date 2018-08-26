Imports DlhSoft.Windows.Controls
Imports System.Collections.ObjectModel

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        For i As Integer = 1 To 32
            Dim gridItem As GanttChartItem = New GanttChartItem With {.Content = String.Format("Item {0}", i)}
            gridItems.Add(gridItem)
            For j As Integer = 1 To 3
                Dim chartItem As GanttChartItem = New GanttChartItem With {.Content = String.Format("{0}.{1}", i, j), .Start = Date.Today.AddDays(j * (i + 4)), .Finish = Date.Today.AddDays(4 + j * (i + 4)), .DisplayRowIndex = i - 1}
                chartItems.Add(chartItem)
            Next j
        Next i

        ' Component ApplyTemplate is called in order to complete loading of the user interface, after the main ApplyTemplate that initializes the custom theme, and using an asynchronous action to allow further constructor initializations if they exist (such as setting up the theme name to load).
        Dispatcher.BeginInvoke(CType(Sub()
                                         ' Apply template to be able to access the internal GanttChartView control.
                                         ' Set up grid items and separately the internally computed bars to be displayed in the chart area (instead of GanttChartDataGrid.Items).
                                         ' For optimization, also set DisplayRowCount on the inner GanttChartView component.
                                         ApplyTemplate()
                                         GanttChartDataGrid.ApplyTemplate()
                                         GanttChartDataGrid.Items = gridItems
                                         GanttChartDataGrid.GanttChartView.DisplayRowCount = gridItems.Count
                                         GanttChartDataGrid.GanttChartView.Items = chartItems
                                     End Sub, Action))
    End Sub

    Private gridItems As New ObservableCollection(Of GanttChartItem)()
    Private chartItems As New ObservableCollection(Of GanttChartItem)()

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
