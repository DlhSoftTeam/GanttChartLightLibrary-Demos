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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Demos.WPF.CSharp.GanttChartDataGrid.DataBinding
{
    public class CustomTaskItem : INotifyPropertyChanged
    {
        private string name;
        public string Name { get { return name; } set { if (value != name) name = value; OnPropertyChanged("Name"); } }

        private int indentLevel;
        public int IndentLevel { get { return indentLevel; } set { if (value != indentLevel) indentLevel = value; OnPropertyChanged("IndentLevel"); } }

        public DateTime startDate;
        public DateTime StartDate { get { return startDate; } set { if (value != startDate) startDate = value; OnPropertyChanged("StartDate"); } }

        public DateTime finishDate;
        public DateTime FinishDate { get { return finishDate; } set { if (value != finishDate) finishDate = value; OnPropertyChanged("FinishDate"); } }

        public DateTime completionCurrentDate;
        public DateTime CompletionCurrentDate { get { return completionCurrentDate; } set { if (value != completionCurrentDate) completionCurrentDate = value; OnPropertyChanged("CompletionCurrentDate"); } }

        public bool milestone;
        public bool Milestone { get { return milestone; } set { if (value != milestone) milestone = value; OnPropertyChanged("Milestone"); } }

        private string assignmentsString;
        public string AssignmentsString { get { return assignmentsString; } set { if (value != assignmentsString) assignmentsString = value; OnPropertyChanged("AssignmentsString"); } }

        private ObservableCollection<CustomPredecessorItem> predecessors;
        public ObservableCollection<CustomPredecessorItem> Predecessors { get { return predecessors; } set { if (value != predecessors) predecessors = value; OnPropertyChanged("Predecessors"); } }

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

    public class CustomPredecessorItem : INotifyPropertyChanged
    {
        private CustomTaskItem reference;
        public CustomTaskItem Reference { get { return reference; } set { if (value != reference) reference = value; OnPropertyChanged("Reference"); } }

        private int type;
        public int Type { get { return type; } set { if (value != type) type = value; OnPropertyChanged("Type"); } }

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
