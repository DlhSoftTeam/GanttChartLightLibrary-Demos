using System;
using System.Windows;
using DlhSoft.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;
using System.Xml.Linq;
using System.ComponentModel;

namespace Demos.WPF.CSharp.GanttChartDataGrid.ImportingExportingXML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CustomGanttChartItem item0 = GanttChartDataGrid.Items[0] as CustomGanttChartItem;

            CustomGanttChartItem item1 = GanttChartDataGrid.Items[1] as CustomGanttChartItem;
            item1.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item1.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            item1.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));
            item1.AssignmentsContent = "Resource 1";

            CustomGanttChartItem item2 = GanttChartDataGrid.Items[2] as CustomGanttChartItem;
            item2.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item2.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item2.BaselineStart = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));
            item2.BaselineFinish = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("16:00:00"));
            item2.AssignmentsContent = "Resource 1, Resource 2";
            item2.Predecessors.Add(new PredecessorItem { Item = item1 });

            CustomGanttChartItem item3 = GanttChartDataGrid.Items[3] as CustomGanttChartItem;
            item3.Predecessors.Add(new PredecessorItem { Item = item0, DependencyType = DependencyType.StartStart });

            CustomGanttChartItem item4 = GanttChartDataGrid.Items[4] as CustomGanttChartItem    ;
            item4.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item4.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"));
            item4.BaselineStart = DateTime.Today.AddDays(-1).Add(TimeSpan.Parse("12:00:00"));
            item4.BaselineFinish = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem item6 = GanttChartDataGrid.Items[6] as CustomGanttChartItem;
            item6.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item6.Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));

            CustomGanttChartItem item7 = GanttChartDataGrid.Items[7] as CustomGanttChartItem;
            item7.Start = DateTime.Today.AddDays(4);
            item7.IsMilestone = true;
            item7.BaselineStart = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));
            item7.Predecessors.Add(new PredecessorItem { Item = item4 });
            item7.Predecessors.Add(new PredecessorItem { Item = item6 });

            for (int i = 3; i <= 25; i++)
            {
                GanttChartDataGrid.Items.Add(
                    new CustomGanttChartItem
                    {
                        Content = "Task " + i,
                        Indentation = i % 3 == 0 ? 0 : 1,
                        Start = DateTime.Today.AddDays(i <= 8 ? (i - 4) * 2 : i - 8),
                        Finish = DateTime.Today.AddDays((i <= 8 ? (i - 4) * 2 + (i > 8 ? 6 : 1) : i - 2) + 2),
                        CompletedFinish = DateTime.Today.AddDays(i <= 8 ? (i - 4) * 2 : i - 8).AddDays(i % 6 == 4 ? 3 : 0),
                        Description = "Task " + i + " Description"
                    });
            }
        }

        private ResourceDictionary themeResourceDictionary;
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
            themeResourceDictionary = new ResourceDictionary { Source = new Uri("/" + GetType().Assembly.GetName().Name + ";component/Themes/" + theme + ".xaml", UriKind.Relative) };
            GanttChartDataGrid.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }

        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(MainWindow), new FrameworkPropertyMetadata());
        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        private void SaveProjectXmlButton_Click(object sender, RoutedEventArgs e)
        {
            // Select a Project XML file to save data to.
            SaveFileDialog saveFileDialog = new SaveFileDialog { Title = "Save Project XML", Filter = "Project XML files|*.xml", DefaultExt = ".xml" };
            if (saveFileDialog.ShowDialog() != true)
                return;
            var assignableResources = GanttChartDataGrid.AssignableResources;

            FileInfo fileInfo = new FileInfo(saveFileDialog.FileName);
            using (Stream stream = fileInfo.Create())
            {
                SerializerFile(stream, assignableResources);
            }
        }

        private void SerializerFile(Stream stream, ObservableCollection<string> assignableResources)
        {
            var serializer = new DlhSoft.Windows.Controls.GanttChartDataGrid.ProjectXmlSerializer(GanttChartDataGrid, assignableResources);
            serializer.GanttChartItemSaving += Serializer_GanttChartItemSaving;
            serializer.Save(stream);
        }

        private const string xmlns = "http://schemas.microsoft.com/project";

        private void Serializer_GanttChartItemSaving(object sender, GanttChartView.ProjectXmlSerializer.GanttChartItemSavingEventArgs e)
        {
            XElement taskElement = XElement.Parse(e.OutputXml);
            CustomGanttChartItem task = e.GanttChartItem as CustomGanttChartItem;

            if (task.BaselineStart != null || task.BaselineFinish != null)
            {
                XElement baseline = taskElement.Element(XName.Get("Baseline", xmlns));
                if (baseline == null)
                {
                    baseline = new XElement(XName.Get("Baseline"));
                    taskElement.Add(baseline);
                }

                XElement start = new XElement(XName.Get("Start"), task.BaselineStart);
                baseline.Add(start);
                XElement finish = new XElement(XName.Get("Finish"), task.BaselineFinish);
                baseline.Add(finish);
            }

            if (!string.IsNullOrEmpty(task.Description)) 
            {
                XElement description = new XElement(XName.Get("Description"), task.Description);
                taskElement.Add(description);
            }

            e.OutputXml = taskElement.ToString();
        }

        private void LoadProjectXmlButton_Click(object sender, RoutedEventArgs e)
        {
            // Select a Project XML file to load data from.
            OpenFileDialog openFileDialog = new OpenFileDialog { Title = "Load Project XML", Filter = "Project XML files|*.xml", Multiselect = false };
            if (openFileDialog.ShowDialog() != true)
                return;
            var assignableResources = GanttChartDataGrid.AssignableResources;

            using (Stream stream = openFileDialog.OpenFile())
            {
                FileName = openFileDialog.FileName;
                DeserializerFile(stream, assignableResources);
            }
        }

        private void DeserializerFile(Stream stream, ObservableCollection<string> assignableResources)
        {
            var serializer = new DlhSoft.Windows.Controls.GanttChartDataGrid.ProjectXmlSerializer(GanttChartDataGrid, assignableResources);
            serializer.GanttChartItemLoading += Serializer_GanttChartItemLoading;
            serializer.Load(stream);
        }

        private void Serializer_GanttChartItemLoading(object sender, GanttChartView.ProjectXmlSerializer.GanttChartItemLoadingEventArgs e)
        {
            var taskElement = e.SourceElement;
            CustomGanttChartItem task = new CustomGanttChartItem
            {
                Content = e.GanttChartItem.Content,
                Indentation = e.GanttChartItem.Indentation,
                Start = e.GanttChartItem.Start,
                Finish = e.GanttChartItem.Finish,
                CompletedFinish = e.GanttChartItem.CompletedFinish,
                IsMilestone = e.GanttChartItem.IsMilestone,
                AssignmentsContent = e.GanttChartItem.AssignmentsContent,
                Predecessors = e.GanttChartItem.Predecessors                
            };
            e.GanttChartItem = task;

            XElement baselineElement = taskElement.Element(XName.Get("Baseline", xmlns));
            if (baselineElement != null)
            {
                if (!string.IsNullOrEmpty(baselineElement.Element(XName.Get("Start", xmlns))?.Value))
                    task.BaselineStart = DateTime.Parse(baselineElement.Element(XName.Get("Start", xmlns))?.Value);
                if (!string.IsNullOrEmpty(baselineElement.Element(XName.Get("Finish", xmlns))?.Value))
                    task.BaselineFinish = DateTime.Parse(baselineElement.Element(XName.Get("Finish", xmlns))?.Value);
            }

            task.Description = taskElement.Element(XName.Get("Description", xmlns))?.Value;
        }
    }

    public class CustomGanttChartItem : GanttChartItem, INotifyPropertyChanged
    {
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
