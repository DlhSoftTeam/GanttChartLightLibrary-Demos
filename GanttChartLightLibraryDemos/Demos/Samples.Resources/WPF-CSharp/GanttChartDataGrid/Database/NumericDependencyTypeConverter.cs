using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DlhSoft.Windows.Controls;
using System.ComponentModel;
using System.Linq;

namespace GanttChartDataGridDatabaseSample
{
    public class NumericDependencyTypeConverter : IValueConverter
    {
        // Converts a DependencyType value from/to an int value.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue = (int)value;
            return (DependencyType)intValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DependencyType dependencyTypeValue = (DependencyType)value;
            return (int)dependencyTypeValue;
        }

        private static NumericDependencyTypeConverter instance;
        public static NumericDependencyTypeConverter Instance
        {
            get
            {
                if (instance == null)
                    instance = new NumericDependencyTypeConverter();
                return instance;
            }
        }
    }
}
