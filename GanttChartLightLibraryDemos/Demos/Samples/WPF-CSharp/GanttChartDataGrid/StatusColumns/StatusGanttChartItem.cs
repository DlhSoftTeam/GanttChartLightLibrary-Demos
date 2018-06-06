using DlhSoft.Windows.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Demos.WPF.CSharp.GanttChartDataGrid.StatusColumns
{
    class StatusGanttChartItem : GanttChartItem
    {
        public string Status
        {
            get
            {
                if (HasChildren || IsMilestone)
                    return string.Empty;
                if (CompletedFinish >= Finish)
                    return "Completed";
                var now = DateTime.Now;
                if (CompletedFinish < now)
                    return "Behind schedule";
                if (CompletedFinish > Start)
                    return "In progress";
                return "To Do";
            }
        }

        public SolidColorBrush StatusColor
        {
            get
            {
                switch (Status)
                {
                    case "Completed":
                        return Brushes.Green;
                    case "To Do":
                        return Brushes.Gray;
                    case "Behind schedule":
                        return Brushes.Red;
                    case "In progress":
                        return Brushes.Orange;
                    default:
                        return Brushes.Transparent;
                }
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            var statusAffectingPropertyNames = new[] { nameof(Start), nameof(CompletedFinish), nameof(Finish), nameof(IsMilestone), nameof(HasChildren) };
            if (statusAffectingPropertyNames.Contains(propertyName))
            {
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(StatusColor));
            }
        }
    }
}
