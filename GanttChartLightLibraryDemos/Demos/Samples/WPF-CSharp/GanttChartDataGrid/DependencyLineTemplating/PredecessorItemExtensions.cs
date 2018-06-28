using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Demos.WPF.CSharp.GanttChartDataGrid.DependencyLineTemplating
{
    public static class PredecessorItemExtensions
    {
        public static readonly DependencyProperty IsImportantProperty = DependencyProperty.RegisterAttached("IsImportant", typeof(bool), typeof(PredecessorItemExtensions));
        public static bool GetIsImportant(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsImportantProperty);
        }
        public static void SetIsImportant(DependencyObject obj, bool value)
        {
            obj.SetValue(IsImportantProperty, value);
        }
    }
}
