Imports System.ComponentModel
Imports System.Collections.ObjectModel

Public Class CustomResourceItem
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

    Private assignedTasksValue As ObservableCollection(Of CustomTaskItem)
    Public Property AssignedTasks() As ObservableCollection(Of CustomTaskItem)
        Get
            Return assignedTasksValue
        End Get
        Set(value As ObservableCollection(Of CustomTaskItem))
            If value IsNot assignedTasksValue Then
                assignedTasksValue = value
            End If
            OnPropertyChanged("AssignedTasks")
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

#Region "Changes"

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Overridable Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

#End Region
End Class
