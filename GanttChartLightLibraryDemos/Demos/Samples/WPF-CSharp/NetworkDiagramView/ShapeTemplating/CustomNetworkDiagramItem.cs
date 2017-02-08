using System;
using DlhSoft.Windows.Controls.Pert;
using System.ComponentModel;

namespace Demos.WPF.CSharp.NetworkDiagramView.ShapeTemplating
{
    class CustomNetworkDiagramItem : NetworkDiagramItem, INotifyPropertyChanged
    {
        private DateTime actualStart = DateTime.Today;
        public DateTime ActualStart
        {
            get { return actualStart; }
            set
            {
                if (value > ActualFinish)
                    ActualFinish = value;
                actualStart = value;
                OnPropertyChanged("ActualStart");
            }
        }

        private DateTime actualFinish = DateTime.Today;
        public DateTime ActualFinish
        {
            get { return actualFinish; }
            set
            {
                if (value < ActualStart)
                    ActualStart = value;
                actualFinish = value;
                OnPropertyChanged("ActualFinish");
            }
        }
    }
}
