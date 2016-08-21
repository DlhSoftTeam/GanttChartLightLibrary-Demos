using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Demos
{
    public class FileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringParameter = parameter as string;
            if (stringParameter == null)
                return null;
            var index = stringParameter.IndexOf('*');
            var stringValue = value as string ?? string.Empty;
            if (index >= 0)
                return stringParameter.Substring(0, index) + stringValue + stringParameter.Substring(index + 1);
            return stringValue + stringParameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
