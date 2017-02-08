using System;
using System.Globalization;
using System.Windows.Data;

namespace Demos.WPF.CSharp.GanttChartDataGrid.NumericDays
{
    class NumericDayStringConverter : IValueConverter
    {
        // Indicates the origin date of numeric day conversions.
        public static DateTime Origin = new DateTime(2001, 1, 1);

        // Converts a date and time value to an integral numeric day string value when the value is positive, or to an empty string otherwise.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isFinish = parameter as string == "Finish";
            DateTime dateTimeValue = (DateTime)value;
            if (isFinish && dateTimeValue.Date == dateTimeValue)
                dateTimeValue = dateTimeValue.AddDays(-1);
            return dateTimeValue.Date >= Origin ? string.Format("{0}", (int)(dateTimeValue.Date - Origin).TotalDays + 1) : string.Empty;
        }

        // Converts a integral numeric day string value to the appropriate date value when parsing is successful and the value is positive, or to the origin date value otherwise.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isFinish = parameter as string == "Finish";
            string stringValue = (string)value;
            int intValue;
            if (int.TryParse(stringValue, out intValue))
                return Origin.AddDays(Math.Max(0, intValue - (!isFinish ? 1 : 0)));
            return Origin;
        }
    }
}
