Imports System.Globalization

Public Class UnlimitedIntConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim intValue As Integer = CInt(Fix(value))
        Return If(intValue < Integer.MaxValue, intValue.ToString(), "Unlimited")
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Dim stringValue As String = CStr(value)
        If stringValue = String.Empty OrElse stringValue.ToLowerInvariant() = "unlimited" Then
            Return Integer.MaxValue
        End If
        stringValue = stringValue.ToLowerInvariant().Replace("unlimited", String.Empty)
        Dim intValue As Integer
        If Integer.TryParse(stringValue, intValue) Then
            Return intValue
        End If
        Return 0
    End Function
End Class
