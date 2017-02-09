using DlhSoft.Windows.Controls;
using System.ComponentModel;
using System.Linq;

namespace Demos.WPF.CSharp.GanttChartDataGrid.SummaryValues
{
    class CustomGanttChartItem : GanttChartItem, INotifyPropertyChanged
    {
        private decimal extraCosts;
        public decimal ExtraCosts
        {
            get { return extraCosts; }
            set
            {
                if (value < 0)
                    value = 0;

                extraCosts = value;
                OnPropertyChanged("ExtraCosts");

                if (GanttChartView == null)
                    return;
                CustomGanttChartItem parent = GanttChartView.GetParent(this) as CustomGanttChartItem;
                if (parent == null)
                    return;
                parent.ExtraCosts = GanttChartView.GetChildren(parent).Sum(item => (item as CustomGanttChartItem).ExtraCosts);
            }
        }
    }
}
