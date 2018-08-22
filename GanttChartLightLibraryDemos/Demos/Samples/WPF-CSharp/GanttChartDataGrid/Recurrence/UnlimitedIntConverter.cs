using System;
using System.Windows.Data;
using System.Globalization;

namespace Demos.WPF.CSharp.GanttChartDataGrid.Recurrence
{
    public class UnlimitedIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue = (int)value;
            return intValue < int.MaxValue ? intValue.ToString() : "Unlimited";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = (string)value;
            if (stringValue == string.Empty || stringValue.ToLowerInvariant() == "unlimited")
                return int.MaxValue;
            stringValue = stringValue.ToLowerInvariant().Replace("unlimited", string.Empty);
            int intValue;
            if (int.TryParse(stringValue, out intValue))
                return intValue;
            return 0;
        }
    }
}
