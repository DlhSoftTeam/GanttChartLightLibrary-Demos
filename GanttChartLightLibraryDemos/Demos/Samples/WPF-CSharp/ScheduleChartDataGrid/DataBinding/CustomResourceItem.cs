using System;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Demos.WPF.CSharp.ScheduleChartDataGrid.DataBinding
{
    public class CustomResourceItem : INotifyPropertyChanged
    {
        private string name;
        public string Name { get { return name; } set { if (value != name) name = value; OnPropertyChanged("Name"); } }

        private ObservableCollection<CustomTaskItem> assignedTasks;
        public ObservableCollection<CustomTaskItem> AssignedTasks { get { return assignedTasks; } set { if (value != assignedTasks) assignedTasks = value; OnPropertyChanged("AssignedTasks"); } }

        private string description;
        public string Description { get { return description; } set { if (value != description) description = value; OnPropertyChanged("Description"); } }

        #region Changes

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class CustomTaskItem : INotifyPropertyChanged
    {
        private string name;
        public string Name { get { return name; } set { if (value != name) name = value; OnPropertyChanged("Name"); } }

        public DateTime startDate;
        public DateTime StartDate { get { return startDate; } set { if (value != startDate) startDate = value; OnPropertyChanged("StartDate"); } }

        public DateTime finishDate;
        public DateTime FinishDate { get { return finishDate; } set { if (value != finishDate) finishDate = value; OnPropertyChanged("FinishDate"); } }

        public DateTime completionCurrentDate;
        public DateTime CompletionCurrentDate { get { return completionCurrentDate; } set { if (value != completionCurrentDate) completionCurrentDate = value; OnPropertyChanged("CompletionCurrentDate"); } }

        private string assignmentsString;
        public string AssignmentsString { get { return assignmentsString; } set { if (value != assignmentsString) assignmentsString = value; OnPropertyChanged("AssignmentsString"); } }

        #region Changes

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
