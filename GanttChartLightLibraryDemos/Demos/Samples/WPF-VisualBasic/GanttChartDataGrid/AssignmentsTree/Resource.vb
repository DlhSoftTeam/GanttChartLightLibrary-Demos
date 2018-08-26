Imports DlhSoft.Windows.Controls
Imports System.Text

Public Class Resource
    Inherits DataTreeGridItem

    Private allocationValue As Double
    Public Property Allocation() As Double
        Get
            Return allocationValue
        End Get
        Set(value As Double)
            allocationValue = value
            OnPropertyChanged(NameOf(Allocation))
        End Set
    End Property

    'INSTANT VB NOTE: The variable role was renamed since Visual Basic does not allow variables and other class members to have the same name:
    Private roleValue As String
    Public Property Role() As String
        Get
            Return roleValue
        End Get
        Set(value As String)
            roleValue = value
        End Set
    End Property
End Class
