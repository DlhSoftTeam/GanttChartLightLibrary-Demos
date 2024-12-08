﻿using System;
using System.Windows;
using DlhSoft.Windows.Controls;

namespace Demos.WPF.CSharp.ScheduleChartDataGrid.AssigningTasks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var unassignedScheduleChartItem = new ScheduleChartItem { Content = "(Unassigned)" };
            for (int i = 1; i <= 12; i++)
            {
                unassignedScheduleChartItem.GanttChartItems.Add(
                    new GanttChartItem
                    {
                        Content = "Task " + i,
                        Start = DateTime.Today.AddDays(i),
                        Finish = DateTime.Today.AddDays(i + 4)
                    });
            }
            ScheduleChartDataGrid.Items.Add(unassignedScheduleChartItem);

            for (int i = 1; i <= 8; i++)
            {
                ScheduleChartItem item = new ScheduleChartItem { Content = "Resource " + i };
                for (int j = 1; j <= (i - 1) % 4 + 1; j++)
                {
                    item.GanttChartItems.Add(
                        new GanttChartItem
                        {
                            Content = "Task " + i + "." + j,
                            Start = DateTime.Today.AddDays(i),
                            Finish = DateTime.Today.AddDays(i + j + 2),
                            CompletedFinish = DateTime.Today.AddDays(i)
                        });
                }
                ScheduleChartDataGrid.Items.Add(item);
            }
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

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        public static bool GetIsSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectedProperty);
        }

        public static void SetIsSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSelectedProperty, value);
        }

        private GanttChartItem SelectedItem;

        private void ScheduleChartDataGrid_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point controlPosition = e.GetPosition(ScheduleChartDataGrid);
            if (controlPosition.X < ScheduleChartDataGrid.ActualWidth - ScheduleChartDataGrid.GanttChartView.ActualWidth)
                return;

            GanttChartItem item = null;
            FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
            if (frameworkElement != null)
                item = frameworkElement.DataContext as GanttChartItem;

            if (SelectedItem != null)
            {
                SetIsSelected(SelectedItem, false);
                SelectedItem = null;
            }

            if (item == null)
                return;

            SelectedItem = item;
            SetIsSelected(SelectedItem, true);
        }
    }
}
