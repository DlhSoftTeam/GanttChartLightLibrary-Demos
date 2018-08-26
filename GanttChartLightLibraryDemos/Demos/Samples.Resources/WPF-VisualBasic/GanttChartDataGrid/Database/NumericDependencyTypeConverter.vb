Imports System.Net
Imports System.Windows.Ink
Imports System.Windows.Media.Animation
Imports System.Globalization
Imports System.Collections.ObjectModel
Imports DlhSoft.Windows.Controls
Imports System.ComponentModel

Public Class NumericDependencyTypeConverter
    Implements IValueConverter

    ' Converts a DependencyType value from/to an int value.
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim intValue As Integer = CInt(Fix(value))
        Return CType(intValue, DependencyType)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Dim dependencyTypeValue As DependencyType = CType(value, DependencyType)
        Return CInt(Fix(dependencyTypeValue))
    End Function

    'INSTANT VB NOTE: The variable instance was renamed since Visual Basic does not allow variables and other class members to have the same name:
    Private Shared instance_Renamed As NumericDependencyTypeConverter
    Public Shared ReadOnly Property Instance() As NumericDependencyTypeConverter
        Get
            If instance_Renamed Is Nothing Then
                instance_Renamed = New NumericDependencyTypeConverter()
            End If
            Return instance_Renamed
        End Get
    End Property
End Class
