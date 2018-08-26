Imports System.Text
Imports DlhSoft.Windows.Controls.Pert
Imports Microsoft.Win32
Imports System.IO
Imports System.Windows.Threading

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()

        Dim item0 As NetworkDiagramItem = NetworkDiagramView.Items(0)
        item0.EarlyStart = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item0.EarlyFinish = item0.EarlyStart
        item0.LateStart = item0.EarlyStart
        item0.LateFinish = item0.LateStart
        item0.AssignmentsContent = "N/A"

        Dim item1 As NetworkDiagramItem = NetworkDiagramView.Items(1)
        item1.Effort = TimeSpan.Parse("08:00:00")
        item1.EarlyStart = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item1.EarlyFinish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        item1.LateStart = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item1.LateFinish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        item1.Slack = TimeSpan.Zero
        item1.AssignmentsContent = "Resource 1"
        item1.Predecessors.Add(New NetworkPredecessorItem With {.Item = item0})

        Dim item2 As NetworkDiagramItem = NetworkDiagramView.Items(2)
        item2.Effort = TimeSpan.Parse("04:00:00")
        item2.EarlyStart = Date.Today.Add(TimeSpan.Parse("08:00:00"))
        item2.EarlyFinish = Date.Today.Add(TimeSpan.Parse("12:00:00"))
        item2.LateStart = Date.Today.Add(TimeSpan.Parse("12:00:00"))
        item2.LateFinish = Date.Today.Add(TimeSpan.Parse("16:00:00"))
        item2.Slack = TimeSpan.Parse("04:00:00")
        item2.AssignmentsContent = "Resource 2"
        item2.Predecessors.Add(New NetworkPredecessorItem With {.Item = item0})

        Dim item3 As NetworkDiagramItem = NetworkDiagramView.Items(3)
        item3.Effort = TimeSpan.Parse("16:00:00")
        item3.EarlyStart = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item3.EarlyFinish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item3.LateStart = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item3.LateFinish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item3.Slack = TimeSpan.Zero
        item3.AssignmentsContent = "Resource 1, Resource 2"
        item3.Predecessors.Add(New NetworkPredecessorItem With {.Item = item1})
        item3.Predecessors.Add(New NetworkPredecessorItem With {.Item = item2})

        Dim item4 As NetworkDiagramItem = NetworkDiagramView.Items(4)
        item4.Effort = TimeSpan.Parse("04:00:00")
        item4.EarlyStart = Date.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"))
        item4.EarlyFinish = Date.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"))
        item4.LateStart = Date.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"))
        item4.LateFinish = Date.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"))
        item4.Slack = TimeSpan.Parse("12:00:00")
        item4.AssignmentsContent = "Resource 2"
        item4.Predecessors.Add(New NetworkPredecessorItem With {.Item = item1})

        Dim item5 As NetworkDiagramItem = NetworkDiagramView.Items(5)
        item5.Effort = TimeSpan.Parse("12:00:00")
        item5.EarlyStart = Date.Today.AddDays(3).Add(TimeSpan.Parse("08:00:00"))
        item5.EarlyFinish = Date.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"))
        item5.LateStart = Date.Today.AddDays(3).Add(TimeSpan.Parse("08:00:00"))
        item5.LateFinish = Date.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"))
        item5.Slack = TimeSpan.Zero
        item5.AssignmentsContent = "Resource 2"
        item5.Predecessors.Add(New NetworkPredecessorItem With {.Item = item3})
        item5.Predecessors.Add(New NetworkPredecessorItem With {.Item = item4})

        Dim item6 As NetworkDiagramItem = NetworkDiagramView.Items(6)
        item6.Effort = TimeSpan.Parse("2.00:00:00")
        item6.EarlyStart = Date.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"))
        item6.EarlyFinish = Date.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"))
        item6.LateStart = Date.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"))
        item6.LateFinish = Date.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"))
        item6.Slack = TimeSpan.Zero
        item6.AssignmentsContent = "Resource 1"
        item6.Predecessors.Add(New NetworkPredecessorItem With {.Item = item5})

        Dim item7 As NetworkDiagramItem = NetworkDiagramView.Items(7)
        item7.Effort = TimeSpan.Parse("20:00:00")
        item7.EarlyStart = Date.Today.AddDays(4).Add(TimeSpan.Parse("12:00:00"))
        item7.EarlyFinish = Date.Today.AddDays(6).Add(TimeSpan.Parse("16:00:00"))
        item7.LateStart = Date.Today.AddDays(8).Add(TimeSpan.Parse("08:00:00"))
        item7.LateFinish = Date.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"))
        item7.Slack = TimeSpan.Parse("1.04:00:00")
        item7.AssignmentsContent = "Resource 2"
        item7.Predecessors.Add(New NetworkPredecessorItem With {.Item = item5})

        Dim item8 As NetworkDiagramItem = NetworkDiagramView.Items(8)
        item8.LateFinish = Date.Today.AddDays(10).Add(TimeSpan.Parse("12:00:00"))
        item8.LateStart = item8.LateFinish
        item8.EarlyFinish = item8.LateFinish
        item8.EarlyStart = item8.EarlyFinish
        item8.AssignmentsContent = "N/A"
        item8.Predecessors.Add(New NetworkPredecessorItem With {.Item = item6})
        item8.Predecessors.Add(New NetworkPredecessorItem With {.Item = item7})

        ' Optionally, reposition start and finish milestones between the first and second rows of the view.
        ' Dispatcher.BeginInvoke((Action)delegate { NetworkDiagramView.RepositionEnds(); }, DispatcherPriority.Background);
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
        NetworkDiagramView.Resources.MergedDictionaries.Add(themeResourceDictionary)
    End Sub

    ' Control area commands.
    Private Sub SetColorButton_Click(sender As Object, e As RoutedEventArgs)
        Dim item As NetworkDiagramItem = NetworkDiagramView.Items(2)
        DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetShapeFill(item, TryCast(Resources("CustomShapeFill"), Brush))
        DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetShapeStroke(item, TryCast(Resources("CustomShapeStroke"), Brush))
    End Sub
    Private Sub SetDependencyColorButton_Click(sender As Object, e As RoutedEventArgs)
        Dim predecessorItem As NetworkPredecessorItem = NetworkDiagramView.Items(2).Predecessors(0)
        DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetDependencyLineStroke(predecessorItem, TryCast(Resources("CustomDependencyLineStroke"), Brush))
    End Sub
    Private Sub CriticalPathCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        SetColorButton.IsEnabled = False
        For Each item As NetworkDiagramItem In NetworkDiagramView.ManagedItems
            SetCriticalPathHighlighting(item, False)
            If item.Predecessors IsNot Nothing Then
                For Each predecessorItem As NetworkPredecessorItem In item.Predecessors
                    SetCriticalPathHighlighting(predecessorItem, False)
                Next predecessorItem
            End If
        Next item
        For Each item As NetworkDiagramItem In NetworkDiagramView.GetCriticalItems()
            SetCriticalPathHighlighting(item, True)
        Next item
        For Each predecessorItem As NetworkPredecessorItem In NetworkDiagramView.GetCriticalDependencies()
            SetCriticalPathHighlighting(predecessorItem, True)
        Next predecessorItem
    End Sub
    Private Sub CriticalPathCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        For Each predecessorItem As NetworkPredecessorItem In NetworkDiagramView.GetCriticalDependencies()
            SetCriticalPathHighlighting(predecessorItem, False)
        Next predecessorItem
        For Each item As NetworkDiagramItem In NetworkDiagramView.GetCriticalItems()
            SetCriticalPathHighlighting(item, False)
        Next item
        SetColorButton.IsEnabled = True
    End Sub
    Private Sub SetCriticalPathHighlighting(item As NetworkDiagramItem, isHighlighted As Boolean)
        DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetShapeFill(item, If(isHighlighted, TryCast(Resources("CustomShapeFill"), Brush), NetworkDiagramView.ShapeFill))
        DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetShapeStroke(item, If(isHighlighted, TryCast(Resources("CustomShapeStroke"), Brush), NetworkDiagramView.ShapeStroke))
    End Sub
    Private Sub SetCriticalPathHighlighting(predecessorItem As NetworkPredecessorItem, isHighlighted As Boolean)
        DlhSoft.Windows.Controls.Pert.NetworkDiagramView.SetDependencyLineStroke(predecessorItem, If(isHighlighted, TryCast(Resources("CustomDependencyLineStroke"), Brush), NetworkDiagramView.DependencyLineStroke))
    End Sub
    Private Sub PrintButton_Click(sender As Object, e As RoutedEventArgs)
        NetworkDiagramView.Print("NetworkDiagramView Sample Document")
    End Sub
    Private Sub ExportImageButton_Click(sender As Object, e As RoutedEventArgs)
        NetworkDiagramView.Export(CType(Sub()
                                            Dim saveFileDialog As SaveFileDialog = New SaveFileDialog With {.Title = "Export Image To", .Filter = "PNG image files|*.png"}
                                            If Not saveFileDialog.ShowDialog().Equals(True) Then
                                                Return
                                            End If
                                            Dim bitmapSource As BitmapSource = NetworkDiagramView.GetExportBitmapSource(96 * 2)
                                            Using stream As Stream = saveFileDialog.OpenFile()
                                                Dim pngBitmapEncoder As New PngBitmapEncoder()
                                                pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource))
                                                pngBitmapEncoder.Save(stream)
                                            End Using
                                        End Sub, Action))
    End Sub
End Class
