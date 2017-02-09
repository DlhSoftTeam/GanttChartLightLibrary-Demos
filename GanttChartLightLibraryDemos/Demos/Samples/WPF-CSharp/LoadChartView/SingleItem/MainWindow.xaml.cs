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
using DlhSoft.Windows.Controls;

namespace Demos.WPF.CSharp.LoadChartView.SingleItem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AllocationItem allocation1 = LoadChartView.Items[0].GanttChartItems[0];
            allocation1.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            allocation1.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));

            AllocationItem allocation12 = LoadChartView.Items[0].GanttChartItems[1];
            allocation12.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            allocation12.Finish = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            allocation12.Units = 1.5;

            AllocationItem allocation2 = LoadChartView.Items[0].GanttChartItems[2];
            allocation2.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("12:00:00"));
            allocation2.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            allocation2.Units = 0.5;

            AllocationItem allocation3 = LoadChartView.Items[0].GanttChartItems[3];
            allocation3.Start = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("08:00:00"));
            allocation3.Finish = DateTime.Today.AddDays(4).Add(TimeSpan.Parse("16:00:00"));
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
            LoadChartView.Resources.MergedDictionaries.Add(themeResourceDictionary);
        }
    }
}
