using DlhSoft.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demos.WPF.CSharp.GanttChartDataGrid.AssignmentsTree
{
    public class Resource : DataTreeGridItem
    {
        private double allocation;
        public double Allocation { get { return allocation; } set { allocation = value; OnPropertyChanged(nameof(Allocation)); } }

        private string role;
        public string Role { get { return role; } set { role = value; } }
    }
}
