Imports System.Net
Imports System.Windows.Ink
Imports System.Windows.Media.Animation
Imports System.ComponentModel
Imports System.Collections.ObjectModel

Public Class CustomTaskItem
    Implements INotifyPropertyChanged

    Private nameValue As String
    Public Property Name() As String
        Get
            Return nameValue
        End Get
        Set(value As String)
            If value <> nameValue Then
                nameValue = value
            End If
            OnPropertyChanged("Name")
        End Set
    End Property

    Private indentLevelValue As Integer
    Public Property IndentLevel() As Integer
        Get
            Return indentLevelValue
        End Get
        Set(value As Integer)
            If value <> indentLevelValue Then
                indentLevelValue = value
            End If
            OnPropertyChanged("IndentLevel")
        End Set
    End Property

    Public startDateValue As Date
    Public Property StartDate() As Date
        Get
            Return startDateValue
        End Get
        Set(value As Date)
            If value <> startDateValue Then
                startDateValue = value
            End If
            OnPropertyChanged("StartDate")
        End Set
    End Property

    Public finishDateValue As Date
    Public Property FinishDate() As Date
        Get
            Return finishDateValue
        End Get
        Set(value As Date)
            If value <> finishDateValue Then
                finishDateValue = value
            End If
            OnPropertyChanged("FinishDate")
        End Set
    End Property

    Public completionCurrentDateValue As Date
    Public Property CompletionCurrentDate() As Date
        Get
            Return completionCurrentDateValue
        End Get
        Set(value As Date)
            If value <> completionCurrentDateValue Then
                completionCurrentDateValue = value
            End If
            OnPropertyChanged("CompletionCurrentDate")
        End Set
    End Property

    Public milestoneValue As Boolean
    Public Property Milestone() As Boolean
        Get
            Return milestoneValue
        End Get
        Set(value As Boolean)
            If value <> milestoneValue Then
                milestoneValue = value
            End If
            OnPropertyChanged("Milestone")
        End Set
    End Property

    Private assignmentsStringValue As String
    Public Property AssignmentsString() As String
        Get
            Return assignmentsStringValue
        End Get
        Set(value As String)
            If value <> assignmentsStringValue Then
                assignmentsStringValue = value
            End If
            OnPropertyChanged("AssignmentsString")
        End Set
    End Property

    Private predecessorsValue As ObservableCollection(Of CustomPredecessorItem)
    Public Property Predecessors() As ObservableCollection(Of CustomPredecessorItem)
        Get
            Return predecessorsValue
        End Get
        Set(value As ObservableCollection(Of CustomPredecessorItem))
            If value IsNot predecessorsValue Then
                predecessorsValue = value
            End If
            OnPropertyChanged("Predecessors")
        End Set
    End Property

    Private descriptionValue As String
    Public Property Description() As String
        Get
            Return descriptionValue
        End Get
        Set(value As String)
            If value <> descriptionValue Then
                descriptionValue = value
            End If
            OnPropertyChanged("Description")
        End Set
    End Property

#Region "Changes"

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Overridable Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

#End Region
End Class

Public Class CustomPredecessorItem
    Implements INotifyPropertyChanged

    Private referenceValue As CustomTaskItem
    Public Property Reference() As CustomTaskItem
        Get
            Return referenceValue
        End Get
        Set(value As CustomTaskItem)
            If value IsNot referenceValue Then
                referenceValue = value
            End If
            OnPropertyChanged("Reference")
        End Set
    End Property

    Private typeValue As Integer
    Public Property Type() As Integer
        Get
            Return typeValue
        End Get
        Set(value As Integer)
            If value <> typeValue Then
                typeValue = value
            End If
            OnPropertyChanged("Type")
        End Set
    End Property

#Region "Changes"

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Overridable Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

#End Region
End Class
