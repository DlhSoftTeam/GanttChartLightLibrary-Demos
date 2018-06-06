using System;
using System.Windows.Media;
using DlhSoft.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;

namespace Demos.WPF.CSharp.GanttChartDataGrid.AssignmentsTemplate
{
    class CustomGanttChartItem : GanttChartItem
    {
        private ImageSource assignmentsIconSource;
        public ImageSource AssignmentsIconSource
        {
            get { return assignmentsIconSource; }
            set
            {
                assignmentsIconSource = value;
            }
        }
    }
}
