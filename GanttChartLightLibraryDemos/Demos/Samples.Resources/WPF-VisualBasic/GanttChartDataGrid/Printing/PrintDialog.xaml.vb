Imports DlhSoft.Windows.Controls


''' <summary>
''' Interaction logic for PrintDialog.xaml
''' </summary>
Partial Public Class PrintDialog
	Inherits Window

	Public Sub New()
		InitializeComponent()
	End Sub

	Public ReadOnly Property MainWindow As MainWindow
		Get
			Return TryCast(Owner, MainWindow)
		End Get
	End Property
	Public ReadOnly Property GanttChartDataGrid As DlhSoft.Windows.Controls.GanttChartDataGrid
		Get
			Return MainWindow.GanttChartDataGrid
		End Get
	End Property
	Public Property GridColumns() As List(Of ColumnSelector) = New List(Of ColumnSelector)()
	Public Property TimelinePageStart As Date
	Public Property TimelinePageFinish As Date

	Private Const PrintingThreshold As Double = 12000000

	Public Sub Load()
		TimelinePageStart = If(GanttChartDataGrid.Items.Any(), GanttChartDataGrid.GetProjectStart().AddDays(-1), GanttChartDataGrid.TimelinePageStart)
		TimelinePageFinish = If(GanttChartDataGrid.Items.Any(), GanttChartDataGrid.GetProjectFinish().AddDays(7), GanttChartDataGrid.TimelinePageFinish)

		For Each column In GanttChartDataGrid.Columns
			If column.Header Is Nothing Then
				Continue For
			End If
			GridColumns.Add(New ColumnSelector With {.Header = If(column.Header.ToString() = "", "Index", column.Header.ToString()), .IsSelected = True, .Column = column})
		Next column
	End Sub

	Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs)
		Close()
	End Sub

	Private Sub PrintButton_Click(sender As Object, e As RoutedEventArgs)
		If TimelinePageStart >= TimelinePageFinish Then
			Windows.MessageBox.Show("The selected dates are incorrect. Please choose a valid timeline.", "Information", MessageBoxButton.OK)
			Return
		End If

		Dim oldStart = GanttChartDataGrid.TimelinePageStart
		Dim oldFinish = GanttChartDataGrid.TimelinePageFinish

		Dim itemsForHiding As IEnumerable(Of GanttChartItem) = Nothing
		Dim visibleColumns As IEnumerable(Of DataGridColumn) = Nothing
		Try
			Dim dialog = New System.Windows.Controls.PrintDialog()
			'Optionally: dialog.PrintTicket.PageOrientation = PageOrientation.Landscape

			visibleColumns = GridColumns.Where(Function(c) c.IsSelected).Select(Function(c) c.Column)
			itemsForHiding = GanttChartDataGrid.Items.Where(Function(i) i.Finish < TimelinePageStart OrElse i.Start > TimelinePageFinish).ToArray()

			Dim itemsCount = GanttChartDataGrid.Items.Count - itemsForHiding.Count()
			Dim timelineHours = GanttChartDataGrid.GetEffort(TimelinePageStart, TimelinePageFinish, GanttChartDataGrid.GetVisibilitySchedule()).TotalHours
			Dim gridWidth = visibleColumns.Sum(Function(c) c.ActualWidth)

			If itemsCount * GanttChartDataGrid.ItemHeight * (gridWidth + timelineHours * GanttChartDataGrid.HourWidth) > PrintingThreshold Then
				Windows.MessageBox.Show("The printed output would be too big. Please select a shorter timeline and/or fewer columns.", "Information", MessageBoxButton.OK)
				Return
			End If

			If dialog.ShowDialog() = True Then
				GanttChartDataGrid.SetTimelinePage(TimelinePageStart, TimelinePageFinish)

				For Each column In GridColumns
					If Not column.IsSelected Then
						column.Column.Visibility = Visibility.Collapsed
					End If
				Next column

				For Each item In itemsForHiding
					item.IsHidden = True
				Next item

				Dim exportedSize = GanttChartDataGrid.GetExportSize()

				'Printing on a single page, if the content fits (considering the margins defined in PrintingTemplate as well).
				If exportedSize.Width + 2 * 48 <= dialog.PrintableAreaWidth AndAlso exportedSize.Height + 2 * 32 <= dialog.PrintableAreaHeight Then
					GanttChartDataGrid.Export(CType(Sub()
														' Get a DrawingVisual representing the Gantt Chart content.
														' Apply necessary transforms for the content to fit into the output page.
														' Actually print the visual.
														Dim exportedVisual = GanttChartDataGrid.GetExportDrawingVisual()
														exportedVisual.Transform = GetPageFittingTransform(dialog)
														Dim container = New Border()
														container.Padding = New Thickness(48, 32, 48, 32)
														container.Child = New Shapes.Rectangle With {.Fill = New VisualBrush(exportedVisual), .Width = exportedSize.Width, .Height = exportedSize.Height}
														dialog.PrintVisual(container, "Gantt Chart Document")
													End Sub, Action))
				Else
					Dim documentPaginator = New DlhSoft.Windows.Controls.GanttChartDataGrid.DocumentPaginator(GanttChartDataGrid)
					documentPaginator.PageSize = New Windows.Size(dialog.PrintableAreaWidth, dialog.PrintableAreaHeight)
					dialog.PrintDocument(documentPaginator, "Gantt Chart Document")
				End If

				Close()
			End If
		Finally
			Dispatcher.BeginInvoke(CType(Sub()
											 For Each columnSelector In GridColumns
												 If Not columnSelector.IsSelected Then
													 columnSelector.Column.Visibility = Visibility
												 End If
											 Next columnSelector
											 GanttChartDataGrid.SetTimelinePage(oldStart, oldFinish)
											 For Each item In itemsForHiding
												 item.IsHidden = False
											 Next item
										 End Sub, Action))
		End Try
	End Sub

	Private Function GetPageFittingTransform(printDialog As System.Windows.Controls.PrintDialog) As TransformGroup
		' Determine scale to apply for page fitting.
		Dim scale = GetPageFittingScaleRatio(printDialog)
		' Set up a transform group in order to allow multiple transforms, if needed.
		Dim transformGroup = New TransformGroup()
		transformGroup.Children.Add(New ScaleTransform(scale, scale))
		' Optionally, add other transforms, such as supplemental translation, scale, or rotation as you need for the output presentation.
		Return transformGroup
	End Function

	Private Function GetPageFittingScaleRatio(printDialog As System.Windows.Controls.PrintDialog) As Double
		' Determine the appropriate scale to apply based on export size and printable area size.
		Dim outputSize = GanttChartDataGrid.GetExportSize()
		Dim scaleX = printDialog.PrintableAreaWidth / outputSize.Width
		Dim scaleY = printDialog.PrintableAreaHeight / outputSize.Height
		Dim scale = Math.Min(scaleX, scaleY)
		Return scale
	End Function
End Class

Public Class ColumnSelector
	Public Property Header As String
	Public Property IsSelected As Boolean
	Public Property Column As DataGridColumn
End Class
