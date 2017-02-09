using System;
using System.Windows;
using System.Collections.ObjectModel;

namespace Demos.WPF.CSharp.ScheduleChartDataGrid.DataBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var resourceItems = new ObservableCollection<CustomResourceItem>
            {
                new CustomResourceItem
                {
                    Name = "Resource 1", Description = "Description of custom resource 1",
                    AssignedTasks = new ObservableCollection<CustomTaskItem>
                    {
                        new CustomTaskItem { Name = "Task 1", StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.Add(TimeSpan.Parse("16:00:00")), CompletionCurrentDate = DateTime.Today.Add(TimeSpan.Parse("12:00:00")) },
                        new CustomTaskItem { Name = "Task 2", StartDate = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00")), FinishDate = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00")), AssignmentsString = "50%" }
                    }
                },
                new CustomResourceItem
                {
                    Name = "Resource 2", Description = "Description of custom resource 2",
                    AssignedTasks = new ObservableCollection<CustomTaskItem>
                    {
                        new CustomTaskItem { Name = "Task 2", StartDate = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00")), FinishDate = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00")) }
                    }
                }
            };

            for (int i = 3; i <= 16; i++)
            {
                CustomResourceItem item = new CustomResourceItem { Name = "Resource " + i, AssignedTasks = new ObservableCollection<CustomTaskItem>() };
                for (int j = 1; j <= (i - 1) % 4 + 1; j++)
                {
                    item.AssignedTasks.Add(
                        new CustomTaskItem
                        {
                            Name = "Task " + i + "." + j,
                            StartDate = DateTime.Today.AddDays(i + (i - 1) * (j - 1)),
                            FinishDate = DateTime.Today.AddDays(i * 1.2 + (i - 1) * (j - 1) + 1),
                            CompletionCurrentDate = DateTime.Today.AddDays(i + (i - 1) * (j - 1)).AddDays((i + j) % 5 == 2 ? 2 : 0)
                        });
                }
                resourceItems.Add(item);
            }

            ScheduleChartDataGrid.DataContext = resourceItems;
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
            ScheduleChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }
    }
}
