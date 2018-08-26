Imports System.Globalization

Friend Class NumericDayStringConverter
    Implements IValueConverter

    ' Indicates the origin date of numeric day conversions.
    Public Shared Origin As New Date(2001, 1, 1)

    ' Converts a date and time value to an integral numeric day string value when the value is positive, or to an empty string otherwise.
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim isFinish As Boolean = TryCast(parameter, String) = "Finish"
        Dim dateTimeValue As Date = CDate(value)
        If isFinish AndAlso dateTimeValue.Date = dateTimeValue Then
            dateTimeValue = dateTimeValue.AddDays(-1)
        End If
        Return If(dateTimeValue.Date >= Origin, String.Format("{0}", CInt(Fix((dateTimeValue.Date - Origin).TotalDays)) + 1), String.Empty)
    End Function

    ' Converts a integral numeric day string value to the appropriate date value when parsing is successful and the value is positive, or to the origin date value otherwise.
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Dim isFinish As Boolean = TryCast(parameter, String) = "Finish"
        Dim stringValue As String = CStr(value)
        Dim intValue As Integer
        If Integer.TryParse(stringValue, intValue) Then
            Return Origin.AddDays(Math.Max(0, intValue - (If((Not isFinish), 1, 0))))
        End If
        Return Origin
    End Function
End Class
