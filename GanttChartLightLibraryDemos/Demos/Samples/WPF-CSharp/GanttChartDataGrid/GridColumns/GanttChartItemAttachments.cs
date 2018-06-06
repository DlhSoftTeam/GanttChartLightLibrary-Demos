using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Demos.WPF.CSharp.GanttChartDataGrid.GridColumns
{
    static class GanttChartItemAttachments
    {
        public static readonly DependencyProperty MyValue5Property =
            DependencyProperty.RegisterAttached("MyValue5", typeof(string), typeof(GanttChartItemAttachments));
        public static string GetMyValue5(DependencyObject obj)
        {
            return (string)obj.GetValue(MyValue5Property);
        }
        public static void SetMyValue5(DependencyObject obj, string value)
        {
            obj.SetValue(MyValue5Property, value);
        }

        public static readonly DependencyProperty MyValue6Property =
            DependencyProperty.RegisterAttached("MyValue6", typeof(string), typeof(GanttChartItemAttachments));
        public static string GetMyValue6(DependencyObject obj)
        {
            return (string)obj.GetValue(MyValue6Property);
        }
        public static void SetMyValue6(DependencyObject obj, string value)
        {
            obj.SetValue(MyValue6Property, value);
        }
    }
}
