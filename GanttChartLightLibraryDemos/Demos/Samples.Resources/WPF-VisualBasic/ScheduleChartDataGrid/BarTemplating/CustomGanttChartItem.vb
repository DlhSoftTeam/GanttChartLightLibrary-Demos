Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports DlhSoft.Windows.Controls
Imports DlhSoft.Windows.Data
Imports System.Windows.Controls

Friend Class CustomGanttChartItem
    Inherits GanttChartItem
    Implements INotifyPropertyChanged

    Public Sub New()
        AddHandler Markers.CollectionChanged, AddressOf Markers_CollectionChanged
        AddHandler Interruptions.CollectionChanged, AddressOf Interruptions_CollectionChanged
    End Sub

    Private iconValue As ImageSource
    Public Property Icon() As ImageSource
        Get
            Return iconValue
        End Get
        Set(value As ImageSource)
            iconValue = value
            OnPropertyChanged("Icon")
        End Set
    End Property

    Private noteValue As String
    Public Property Note() As String
        Get
            Return noteValue
        End Get
        Set(value As String)
            noteValue = value
            OnPropertyChanged("Note")
        End Set
    End Property

    Private estimatedStartValue As Date = Date.Today
    Public Property EstimatedStart() As Date
        Get
            Return estimatedStartValue
        End Get
        Set(value As Date)
            If value > EstimatedFinish Then
                EstimatedFinish = value
            End If
            estimatedStartValue = value
            OnPropertyChanged("EstimatedStart")
        End Set
    End Property
    Public ReadOnly Property ComputedEstimatedBarLeft() As Double
        Get
            Return GanttChartView.GetPosition(EstimatedStart) - GanttChartView.GetPosition(Start)
        End Get
    End Property

    Private estimatedFinishValue As Date = Date.Today
    Public Property EstimatedFinish() As Date
        Get
            Return estimatedFinishValue
        End Get
        Set(value As Date)
            If value < EstimatedStart Then
                EstimatedStart = value
            End If
            estimatedFinishValue = value
            OnPropertyChanged("EstimatedFinish")
        End Set
    End Property
    Public ReadOnly Property ComputedEstimatedBarWidth() As Double
        Get
            Return GanttChartView.GetPosition(EstimatedFinish) - GanttChartView.GetPosition(EstimatedStart)
        End Get
    End Property

    Private markersValue As New ObservableCollection(Of Marker)()
    Public ReadOnly Property Markers() As ObservableCollection(Of Marker)
        Get
            Return markersValue
        End Get
    End Property
    Private Sub Markers_CollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
        Select Case e.Action
            Case NotifyCollectionChangedAction.Add
                For Each marker As Marker In e.NewItems
                    marker.Item = Me
                Next marker
        End Select
    End Sub

    Private interruptionsValue As New ObservableCollection(Of Interruption)()
    Public ReadOnly Property Interruptions() As ObservableCollection(Of Interruption)
        Get
            Return interruptionsValue
        End Get
    End Property
    Private Sub Interruptions_CollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
        Select Case e.Action
            Case NotifyCollectionChangedAction.Add
                For Each interruption As Interruption In e.NewItems
                    interruption.Item = Me
                    AddHandler interruption.PropertyChanged, AddressOf Interruption_PropertyChanged
                Next interruption
                Exit Select
            Case NotifyCollectionChangedAction.Remove
                For Each interruption As Interruption In e.OldItems
                    RemoveHandler interruption.PropertyChanged, AddressOf Interruption_PropertyChanged
                Next interruption
                Exit Select
        End Select
        OnPropertyChanged("ComputedInterruptedBars")
    End Sub
    Private Sub Interruption_PropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        OnPropertyChanged("ComputedInterruptedBars")
    End Sub
    Public ReadOnly Iterator Property ComputedInterruptedBars() As IEnumerable(Of InterruptedBar)
        Get
            Dim interruptionsValue = From i In Interruptions
                                     Where (i.ComputedLeft > 0 OrElse i.ComputedLeft + i.ComputedWidth > 0) AndAlso
                                           (i.ComputedLeft < ComputedBarWidth OrElse i.ComputedLeft + i.ComputedWidth < ComputedBarWidth)
                                     Order By i.ComputedLeft
                                     Select i
            Dim previousRight As Double = 0
            For Each interruption As Interruption In interruptionsValue
                If interruption.ComputedLeft > previousRight Then
                    Yield New InterruptedBar With {.Item = Me, .Left = previousRight, .Width = interruption.ComputedLeft - previousRight}
                End If
                previousRight = interruption.ComputedLeft + interruption.ComputedWidth
            Next interruption
            If ComputedBarWidth > previousRight Then
                Yield New InterruptedBar With {.Item = Me, .Left = previousRight, .Width = ComputedBarWidth - previousRight}
            End If
        End Get
    End Property
    Public Class InterruptedBar
        Private privateItem As CustomGanttChartItem
        Public Property Item() As CustomGanttChartItem
            Get
                Return privateItem
            End Get
            Friend Set(ByVal value As CustomGanttChartItem)
                privateItem = value
            End Set
        End Property
        Public ReadOnly Property GanttChartView() As IGanttChartView
            Get
                Return If(Item IsNot Nothing, Item.GanttChartView, Nothing)
            End Get
        End Property
        Public Property Left() As Double
        Public Property Width() As Double
    End Class

    Protected Overrides Sub OnPropertyChanged(propertyName As String)
        MyBase.OnPropertyChanged(propertyName)
        Select Case propertyName
            Case "Start", "EstimatedStart"
                OnPropertyChanged("ComputedEstimatedBarLeft")
        End Select
        Select Case propertyName
            Case "EstimatedStart", "EstimatedFinish"
                OnPropertyChanged("ComputedEstimatedBarWidth")
        End Select
        Select Case propertyName
            Case "Start"
                For Each marker As Marker In Markers
                    marker.OnPropertyChanged("ComputedLeft")
                Next marker
                For Each interruption As Interruption In Interruptions
                    interruption.OnPropertyChanged("ComputedLeft")
                    interruption.OnPropertyChanged("ComputedWidth")
                Next interruption
        End Select
        Select Case propertyName
            Case "Start", "Finish"
                Dispatcher.BeginInvoke(CType(Sub() OnPropertyChanged("ComputedInterruptedBars"), Action))
        End Select
    End Sub

    ' Alternatively (required, if you have mouse wheel zooming enabled), refresh the user interface from a central handler.
    ' protected override void OnBarChanged()
    ' {
    '     OnPropertyChanged("ComputedEstimatedBarLeft");
    '     OnPropertyChanged("ComputedEstimatedBarWidth");
    '     OnPropertyChanged("ComputedInterruptedBars");
    '     base.OnBarChanged();
    ' }
End Class
