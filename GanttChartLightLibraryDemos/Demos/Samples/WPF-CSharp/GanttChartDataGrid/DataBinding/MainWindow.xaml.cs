using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using DlhSoft.Windows.Controls;

namespace Demos.WPF.CSharp.GanttChartDataGrid.DataBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var taskItems = new ObservableCollection<CustomTaskItem>
            {
                new CustomTaskItem { Name = "Task 1", Description = "Description of custom task 1" },
                new CustomTaskItem { Name = "Task 1.1", IndentLevel = 1, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.Add(TimeSpan.Parse("16:00:00")), CompletionCurrentDate = DateTime.Today.Add(TimeSpan.Parse("12:00:00")), AssignmentsString = "Resource 1", Description = "Description of custom task 1.1" },
                new CustomTaskItem { Name = "Task 1.2", IndentLevel = 1, StartDate = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00")), AssignmentsString = "Resource 1, Resource 2", Description = "Description of custom task 1.2" },
                new CustomTaskItem { Name = "Task 2", Description = "Description of custom task 2" },
                new CustomTaskItem { Name = "Task 2.1", IndentLevel = 1, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.1" },
                new CustomTaskItem { Name = "Task 2.2", IndentLevel = 1, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" },
                new CustomTaskItem { Name = "Task 3", IndentLevel = 0, StartDate = DateTime.Today.AddDays(5), Milestone = true, Description = "Description of custom task 3" }
            };
            taskItems[2].Predecessors = new ObservableCollection<CustomPredecessorItem> { new CustomPredecessorItem { Reference = taskItems[1] } };
            taskItems[3].Predecessors = new ObservableCollection<CustomPredecessorItem> { new CustomPredecessorItem { Reference = taskItems[0], Type = (int)DependencyType.StartStart } };
            taskItems[6].Predecessors = new ObservableCollection<CustomPredecessorItem> { new CustomPredecessorItem { Reference = taskItems[0] }, new CustomPredecessorItem { Reference = taskItems[3] } };

            for (int i = 4; i <= 25; i++)
            {
                taskItems.Add(
                    new CustomTaskItem
                    {
                        Name = "Task " + i,
                        IndentLevel = i % 3 == 1 ? 0 : 1,
                        StartDate = DateTime.Today.AddDays(i <= 8 ? (i - 4) * 3 : i - 8),
                        FinishDate = DateTime.Today.AddDays((i <= 8 ? (i - 4) * 3 + (i > 8 ? 6 : 1) : i - 2) + 1),
                        CompletionCurrentDate = DateTime.Today.AddDays(i <= 8 ? (i - 4) * 3 : i - 8).AddDays(i % 6 == 0 ? 3 : 0)
                    });
            }

            GanttChartDataGrid.DataContext = taskItems;
        }

        private string theme = "Generic-bright";
        public MainWindow(string theme) : this()
        {
            this.theme = theme;
            ApplyTemplate();
        }
        public override void OnApplyTemplate()
        {
            LoadTheme();
            base.OnApplyTemplate();
        }
        private void LoadTheme()
        {
            if (theme == null || theme == "Default" || theme == "Aero")
                return;
            var themeResourceDictionary = new ResourceDictionary { Source = new Uri("/" + GetType().Assembly.GetName().Name + ";component/Themes/" + theme + ".xaml", UriKind.Relative) };
            GanttChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }
    }
}
