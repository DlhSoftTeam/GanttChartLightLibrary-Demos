Imports System.Text
Imports DlhSoft.Windows.Controls.Pert
Imports Microsoft.Win32
Imports System.IO

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim item0 As PertChartItem = PertChartView.Items(0)

        Dim item1 As PertChartItem = PertChartView.Items(1)
        item1.Predecessors.Add(New PredecessorItem With {.Item = item0, .DisplayedText = "A", .Content = "Task A", .Effort = TimeSpan.Parse("4")})

        Dim item2 As PertChartItem = PertChartView.Items(2)
        item2.Predecessors.Add(New PredecessorItem With {.Item = item0, .DisplayedText = "B", .Content = "Task B", .Effort = TimeSpan.Parse("2")})

        Dim item3 As PertChartItem = PertChartView.Items(3)
        item3.Predecessors.Add(New PredecessorItem With {.Item = item2, .DisplayedText = "C", .Content = "Task C", .Effort = TimeSpan.Parse("1")})

        Dim item4 As PertChartItem = PertChartView.Items(4)
        item4.Predecessors.Add(New PredecessorItem With {.Item = item1, .DisplayedText = "D", .Content = "Task D", .Effort = TimeSpan.Parse("5")})
        item4.Predecessors.Add(New PredecessorItem With {.Item = item2, .DisplayedText = "E", .Content = "Task E", .Effort = TimeSpan.Parse("3")})
        item4.Predecessors.Add(New PredecessorItem With {.Item = item3, .DisplayedText = "F", .Content = "Task F", .Effort = TimeSpan.Parse("2")})
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
        PertChartView.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub

    ' Control area commands.
    Private Sub SetColorButton_Click(sender As Object, e As RoutedEventArgs)
        Dim item As PertChartItem = PertChartView.Items(2)
        DlhSoft.Windows.Controls.Pert.PertChartView.SetShapeFill(item, TryCast(Resources("CustomShapeFill"), Brush))
        DlhSoft.Windows.Controls.Pert.PertChartView.SetShapeStroke(item, TryCast(Resources("CustomShapeStroke"), Brush))
        DlhSoft.Windows.Controls.Pert.PertChartView.SetTextForeground(item, TryCast(Resources("CustomShapeStroke"), Brush))
    End Sub
    Private Sub SetDependencyColorButton_Click(sender As Object, e As RoutedEventArgs)
        Dim predecessorItem As PredecessorItem = PertChartView.Items(2).Predecessors(0)
        DlhSoft.Windows.Controls.Pert.PertChartView.SetDependencyLineStroke(predecessorItem, TryCast(Resources("CustomDependencyLineStroke"), Brush))
        DlhSoft.Windows.Controls.Pert.PertChartView.SetDependencyTextForeground(predecessorItem, TryCast(Resources("CustomDependencyLineStroke"), Brush))
    End Sub
    Private Sub CriticalPathCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        SetColorButton.IsEnabled = False
        SetDependencyColorButton.IsEnabled = False
        For Each item As PertChartItem In PertChartView.ManagedItems
            SetCriticalPathHighlighting(item, False)
            If item.Predecessors IsNot Nothing Then
                For Each predecessorItem As PredecessorItem In item.Predecessors
                    SetCriticalPathHighlighting(predecessorItem, False)
                Next predecessorItem
            End If
        Next item
        For Each item As PertChartItem In PertChartView.GetCriticalItems()
            SetCriticalPathHighlighting(item, True)
        Next item
        For Each predecessorItem As PredecessorItem In PertChartView.GetCriticalDependencies()
            SetCriticalPathHighlighting(predecessorItem, True)
        Next predecessorItem
    End Sub
    Private Sub CriticalPathCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        For Each predecessorItem As PredecessorItem In PertChartView.GetCriticalDependencies()
            SetCriticalPathHighlighting(predecessorItem, False)
        Next predecessorItem
        For Each item As PertChartItem In PertChartView.GetCriticalItems()
            SetCriticalPathHighlighting(item, False)
        Next item
        SetDependencyColorButton.IsEnabled = True
        SetColorButton.IsEnabled = True
    End Sub
    Private Sub SetCriticalPathHighlighting(item As PertChartItem, isHighlighted As Boolean)
        DlhSoft.Windows.Controls.Pert.PertChartView.SetShapeFill(item, If(isHighlighted, TryCast(Resources("CustomShapeFill"), Brush), PertChartView.ShapeFill))
        DlhSoft.Windows.Controls.Pert.PertChartView.SetShapeStroke(item, If(isHighlighted, TryCast(Resources("CustomShapeStroke"), Brush), PertChartView.ShapeStroke))
        DlhSoft.Windows.Controls.Pert.PertChartView.SetTextForeground(item, If(isHighlighted, TryCast(Resources("CustomShapeStroke"), Brush), PertChartView.TextForeground))
    End Sub
    Private Sub SetCriticalPathHighlighting(predecessorItem As PredecessorItem, isHighlighted As Boolean)
        DlhSoft.Windows.Controls.Pert.PertChartView.SetDependencyLineStroke(predecessorItem, If(isHighlighted, TryCast(Resources("CustomDependencyLineStroke"), Brush), PertChartView.DependencyLineStroke))
        DlhSoft.Windows.Controls.Pert.PertChartView.SetDependencyTextForeground(predecessorItem, If(isHighlighted, TryCast(Resources("CustomDependencyLineStroke"), Brush), PertChartView.DependencyTextForeground))
    End Sub
    Private Sub PrintButton_Click(sender As Object, e As RoutedEventArgs)
        PertChartView.Print("PertChartView Sample Document")
    End Sub
    Private Sub ExportImageButton_Click(sender As Object, e As RoutedEventArgs)
        PertChartView.Export(CType(Sub()
                                       Dim saveFileDialog As SaveFileDialog = New SaveFileDialog With {.Title = "Export Image To", .Filter = "PNG image files|*.png"}
                                       If Not saveFileDialog.ShowDialog().Equals(True) Then
                                           Return
                                       End If
                                       Dim bitmapSource As BitmapSource = PertChartView.GetExportBitmapSource(96 * 2)
                                       Using stream As Stream = saveFileDialog.OpenFile()
                                           Dim pngBitmapEncoder As New PngBitmapEncoder()
                                           pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource))
                                           pngBitmapEncoder.Save(stream)
                                       End Using
                                   End Sub, Action))
    End Sub
End Class
