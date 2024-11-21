using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Demos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                var queryString = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;
            }
            catch (DeploymentException) { }
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void TechnologyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)LoadTabControl);
            Dispatcher.BeginInvoke((Action)LoadFiles);
            Dispatcher.BeginInvoke((Action)LoadContent);
        }
        private void LoadTabControl()
        {
            TabControl.Items.Clear();
            var selectedTechnologyItem = TechnologyComboBox?.SelectedItem as ComboBoxItem;
            if (selectedTechnologyItem == null)
                return;
            var technology = selectedTechnologyItem.Tag as string;
            var technologySeparatorIndex = technology.IndexOf('-');
            var platform = technology.Substring(0, technologySeparatorIndex);
            var programmingLanguage = technology.Substring(technologySeparatorIndex + 1);
            ComponentInfo[] components = null;
            switch (platform)
            {
                case "WPF":
                    switch (programmingLanguage)
                    {
                        case "CSharp":
                            components = new[]
                            {
                                new ComponentInfo
                                {
                                    Name = "GanttChartDataGrid",
                                    Header = "GanttChart\nDataGrid",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "BasicUsage", Title = "Basic usage", Description = "Shows how to load the component with minimum configuration" },
                                        new SampleInfo { Tag = "AssigningResources", Title = "Assigning resources", Description = "Shows how to add resources to tasks" },
                                        new SampleInfo { Tag = "Baseline", Title = "Baseline (estimation time bars vs. actual task bars)", Description = "Shows how you can define and display estimation bars for tasks (i.e. project baseline)" },
                                        new SampleInfo { Tag = "AutomaticScheduling", Title = "Automatic scheduling", Description = "Shows how task dependency constraints can be enabled to automatically scheduling tasks upon all changes" },
                                        new SampleInfo { Tag = "ReadOnlyVisibilityBehavior", Title = "Read only and visibility behavior", Description = "Shows how you can set up read only and visibility settings on the component and on specific items" },
                                        new SampleInfo { Tag = "DateAndTimeFormats", Title = "Date and time formats", Description = "Shows how you can set up custom formatting for dates and times" },
                                        new SampleInfo { Tag = "DataBinding", Title = "Data binding", Description = "Shows how you can data bind the component to a custom task item collection" },
                                        new SampleInfo { Tag = "GridColumns", Title = "Grid columns (built-in and custom)", Description = "Shows how to add built-in and custom grid columns including a column presenting task icon thumbs that offer vertical drag and drop support" },
                                        new SampleInfo { Tag = "CustomAppearance", Title = "Custom appearance", Description = "Shows how you can set colors for all or for individual task items" },
                                        new SampleInfo { Tag = "BarTemplating", Title = "Bar templates (interruptions, summary background)", Description = "Shows how you can define XAML templates for task bars displayed in the chart view (such as to display labels and interruptions for task bars, and/or summary background colors, and more)" },
                                        new SampleInfo { Tag = "DependencyLineTemplating", Title = "Dependency line colors and templating", Description = "Shows how you can define colors for dependency lines and/or an XAML template for displaying them in the chart view (such as to display dashed arrow lines and more)" },
                                        new SampleInfo { Tag = "AssignmentsTemplate", Title = "Assignments template (resource icons)", Description = "Shows how you can customize assignments template and show resource icons in the chart area" },
                                        new SampleInfo { Tag = "ZoomLevel", Title = "Zoom level (and disabling mouse wheel zooming)", Description = "Shows how you can set up zoom level settings for the chart area" },
                                        new SampleInfo { Tag = "BuiltInScales", Title = "Built-in scales (from years to hours)", Description = "Shows how you can combine and use built-in scale types and text header formats" },
                                        new SampleInfo { Tag = "CustomScaleHeaders", Title = "Custom scale (header texts)", Description = "Shows how to define a custom chart scale with text headers" },
                                        new SampleInfo { Tag = "SpecialDays", Title = "Special days (vertically highlight time intervals)", Description = "Shows how you can highlight special time intervals in the chart area (highlighted vertical bars)" },
                                        new SampleInfo { Tag = "ContinuousSchedule", Title = "Continuous schedule (non-stop working time)", Description = "Shows how you can define continuous working time for tasks (24/7)" },
                                        new SampleInfo { Tag = "CustomSchedule", Title = "Custom schedule and scales", Description = "Shows how you can define working and nonworking times for scheduling task items and custom scales and headers for the chart view" },
                                        new SampleInfo { Tag = "TimeConstraints", Title = "Time constraints (min-max start/finish)", Description = "Shows how you can defined limits for the start and finish dates and times of individual task items" },
                                        new SampleInfo { Tag = "SelectionMode", Title = "Selection mode", Description = "Shows how you can set selection mode to single or multiple items (extended)" },
                                        new SampleInfo { Tag = "Sorting", Title = "Sorting", Description = "Shows how you can hierarchically sort task items in the grid and chart view" },
                                        new SampleInfo { Tag = "Filtering", Title = "Filtering", Description = "Shows how you can hide specific task items for filtering purposes" },
                                        new SampleInfo { Tag = "ChangeNotifications", Title = "Change notifications", Description = "Shows how you can detect changes and perform custom actions when they occur" },
                                        new SampleInfo { Tag = "MouseEventHandling", Title = "Mouse event handling", Description = "Shows how you can determine the item and other elements bound to the cursor position for handling mouse events" },
                                        new SampleInfo { Tag = "MoveUpDown", Title = "Move up-down (hierarchical moving)", Description = "Shows how you can allow the end user to move up and down items without breaking the hierarchy" },
                                        new SampleInfo { Tag = "UndoRedo", Title = "Undo-redo", Description = "Shows how you can easily support undo and redo operations for item changes" },
                                        new SampleInfo { Tag = "WBSPath", Title = "WBS path", Description = "Shows how you can easily insert a WBS column in the grid" },
                                        new SampleInfo { Tag = "StatusColumns", Title = "Status columns (including color indicator)", Description = "Shows how to add supplemental custom columns for showing task statuses, such as To Do, In progress, Behind schedule, and Completed" },
                                        new SampleInfo { Tag = "CriticalPath", Title = "Critical path (simple algorithm or PERT method)", Description = "Shows how you can determine and highlight critical tasks in your project using either a simple algorithm (determining tasks that affect project finish, optionally with an acceptable delay) or the classic PERT method" },
                                        new SampleInfo { Tag = "WorkOptimizations", Title = "Work optimizations (reschedule, level resources)", Description = "Shows how you can optimize project timeline and avoid resource over-allocation" },
                                        new SampleInfo { Tag = "MaterialResources", Title = "Material resources (quantities and costs)", Description = "Shows how you can assign material resources having limited or unlimited available quantities and compute task costs based on the allocations" },
                                        new SampleInfo { Tag = "ImportingExportingXML", Title = "Importing and exporting Microsoft® Project XML", Description = "Shows how you can import and export Microsoft® Project XML schema based files, providing maximum compatibility with other applications" },
                                        new SampleInfo { Tag = "Printing", Title = "Printing", Description = "Shows how you can print the current Gantt Chart (with its associated data grid)" },
                                        new SampleInfo { Tag = "ExportImage", Title = "Export image", Description = "Shows how you can generate and save PNG images from the current Gantt Chart (and its associated data grid)" },
                                        new SampleInfo { Tag = "Performance", Title = "Performance (large data set)", Description = "Shows app responsiveness and other runtime performance features when loading large sets of hierarchical data" },
                                        new SampleInfo { Tag = "Styling", Title = "Styling", Description = "Shows how you can apply a custom style defining specific property values for the component element" },
                                        new SampleInfo { Tag = "Templating", Title = "Default templates", Description = "Source code providing expanded default XAML templates for specific component elements" },
                                        new SampleInfo { Tag = "CustomDatesAndDragging", Title = "Custom dates and dragging", Description = "Shows how you can define custom date and time item properties and display draggable secondary bars bound to them in the chart area" },
                                        new SampleInfo { Tag = "MultipleBarsPerLine", Title = "Multiple bars per line (display row index)", Description = "Shows how you can define and display multiple bars for a single task item in the same row of the chart area (i.e. item parts)" },
                                        new SampleInfo { Tag = "MinuteScale", Title = "Minute scale", Description = "Shows how you can customize scales and display task bars bound to hours and minutes" },
                                        new SampleInfo { Tag = "NumericDays", Title = "Numeric days", Description = "Shows how you can customize scales to display project week and day numbers instead of dates" },
                                        new SampleInfo { Tag = "Recurrence", Title = "Recurrence", Description = "Shows how you can define custom code to generate and display recurrent task items and chart bars" },
                                        new SampleInfo { Tag = "SummaryDragging", Title = "Summary dragging", Description = "Shows how you can add a thumb to allow the end user to drag summary bars and its children, having their start and end date-times automatically updated" },
                                        new SampleInfo { Tag = "SummaryBars", Title = "Summary bars", Description = "Shows how you can display child bars instead of summary bars when node items are collapsed" },
                                        new SampleInfo { Tag = "SummaryValues", Title = "Summary values", Description = "Shows how you can hierarchically summarize custom values such as custom task costs" },
                                        new SampleInfo { Tag = "HierarchicalVirtualization", Title = "Hierarchical virtualization", Description = "Shows how to develop summary task virtualization and lazy load child tasks only upon parent node expansion" },
                                        new SampleInfo { Tag = "AssignmentsTree", Title = "Assignments tree (using DataTreeGrid)", Description = "Shows how to show a custom popup allowing the end user to select assigned resources (or departments) from an organizational hierarchy – using DataTreeGrid control from DlhSoft Hierarchical Data Light Library, licensed separately" },
                                        new SampleInfo { Tag = "Database", IsLink = true, Title = "SQL Server® database integration", Description = "Load tasks from and save changes to a SQL Server® database" },
                                        new SampleInfo { Tag = "NetCore", IsLink = true, Title = ".NET Core support", Description = "Using the WPF components within a .NET Core desktop application" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "GanttChartView",
                                    Header = "GanttChart\nView",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "ScheduleChartDataGrid",
                                    Header = "ScheduleChart\nDataGrid",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "BasicUsage", Title = "Basic usage", Description = "Shows how to load the component with minimum configuration" },
                                        new SampleInfo { Tag = "AssigningTasks", Title = "Assigning tasks (drag unassigned items)", Description = "Shows how you can define a Schedule Chart view with draggable unassigned tasks" },
                                        new SampleInfo { Tag = "DataBinding", Title = "Data binding", Description = "Shows how you can data bind the component to a custom resource item collection" },
                                        new SampleInfo { Tag = "GanttChartIntegration", Title = "Gantt Chart integration", Description = "Shows how to generate a Schedule Chart view from Gantt Chart data" },
                                        new SampleInfo { Tag = "CustomAppearance", Title = "Custom appearance", Description = "Shows how you can set colors for all or for individual task items" },
                                        new SampleInfo { Tag = "BarTemplating", Title = "Bar templates", Description = "Shows how you can define XAML templates for task bars displayed in the chart view" },
                                        new SampleInfo { Tag = "CustomSchedule", Title = "Custom schedule and scales", Description = "Shows how you can define working and nonworking times for scheduling task items and custom scales and headers for the chart view" },
                                        new SampleInfo { Tag = "Hierarchy", Title = "Hierarchy", Description = "Shows how you can hierarchically display resource groups in the grid" },
                                        new SampleInfo { Tag = "MultipleLinesPerRow", Title = "Multiple lines per row", Description = "Shows how you can configure the component to display chart task bars using multiple lines per resource row automatically enlarging individual grid row height values" },
                                        new SampleInfo { Tag = "MouseEventHandling", Title = "Mouse event handling", Description = "Shows how you can determine the item and other elements bound to the cursor position for handling mouse events" },
                                        new SampleInfo { Tag = "StatusDisplaying", Title = "Status displaying (resource timeline)", Description = "Shows how you can easily display multiple resources and their status at different times using chart bars of different colors" },
                                        new SampleInfo { Tag = "ShiftScheduling", Title = "Shift scheduling (assigning employees on time shifts) ", Description = "Shows how you can define shifts as resource assignments so that the end user can drag and drop them vertically to change shifts as needed" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "ScheduleChartView",
                                    Header = "ScheduleChart\nView",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "LoadChartDataGrid",
                                    Header = "LoadChart\nDataGrid",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "CustomAppearance", Title = "Custom appearance", Description = "Shows how you can set colors for different types of allocation items" },
                                        new SampleInfo { Tag = "CustomSchedule", Title = "Custom schedule and scales", Description = "Shows how you can define working and nonworking times and custom scales and headers for the chart view" },
                                        new SampleInfo { Tag = "MouseEventHandling", Title = "Mouse event handling", Description = "Shows how you can determine the item and other elements bound to the cursor position for handling mouse events" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "LoadChartView",
                                    Header = "LoadChart\nView",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "SingleItem", Title = "Single item", Description = "Shows how you can set up a single displayed item in the chart view" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "PertChartView",
                                    Header = "PertChart\nView",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "MultiTasksPerLine", Title = "Multiple tasks per line", Description = "Shows how you can extend task lines into multiple parallel items displayed between the same task event shapes, especially useful to avoid diagram complexity when generating items from a Gantt Chart source" },
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "NetworkDiagramView",
                                    Header = "NetworkDiagram\nView",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "ShapeTemplating", Title = "Shape templating", Description = "Shows how you can define XAML templates for task shapes displayed in the view, optionally enabling item property editing as needed" }
                                    }
                                }
                            };
                            break;
                        case "VisualBasic":
                            components = new[]
                            {
                                new ComponentInfo
                                {
                                    Name = "GanttChartDataGrid",
                                    Header = "GanttChartDataGrid",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "BasicUsage", Title = "Basic usage", Description = "Shows how to load the component with minimum configuration" },
                                        new SampleInfo { Tag = "AssigningResources", Title = "Assigning resources", Description = "Shows how to add resources to tasks" },
                                        new SampleInfo { Tag = "Baseline", Title = "Baseline (estimation time bars vs. actual task bars)", Description = "Shows how you can define and display estimation bars for tasks (i.e. project baseline)" },
                                        new SampleInfo { Tag = "AutomaticScheduling", Title = "Automatic scheduling", Description = "Shows how task dependency constraints can be enabled to automatically scheduling tasks upon all changes" },
                                        new SampleInfo { Tag = "ReadOnlyVisibilityBehavior", Title = "Read only and visibility behavior", Description = "Shows how you can set up read only and visibility settings on the component and on specific items" },
                                        new SampleInfo { Tag = "DateAndTimeFormats", Title = "Date and time formats", Description = "Shows how you can set up custom formatting for dates and times" },
                                        new SampleInfo { Tag = "DataBinding", Title = "Data binding", Description = "Shows how you can data bind the component to a custom task item collection" },
                                        new SampleInfo { Tag = "GridColumns", Title = "Grid columns (built-in and custom)", Description = "Shows how to add built-in and custom grid columns including a column presenting task icon thumbs that offer vertical drag and drop support" },
                                        new SampleInfo { Tag = "CustomAppearance", Title = "Custom appearance", Description = "Shows how you can set colors for all or for individual task items" },
                                        new SampleInfo { Tag = "BarTemplating", Title = "Bar templates (interruptions, summary background)", Description = "Shows how you can define XAML templates for task bars displayed in the chart view (such as to display labels and interruptions for task bars, and/or summary background colors, and more)" },
                                        new SampleInfo { Tag = "DependencyLineTemplating", Title = "Dependency line colors and templating", Description = "Shows how you can define colors for dependency lines and/or an XAML template for displaying them in the chart view (such as to display dashed arrow lines and more)" },
                                        new SampleInfo { Tag = "AssignmentsTemplate", Title = "Assignments template (resource icons)", Description = "Shows how you can customize assignments template and show resource icons in the chart area" },
                                        new SampleInfo { Tag = "ZoomLevel", Title = "Zoom level (and disabling mouse wheel zooming)", Description = "Shows how you can set up zoom level settings for the chart area" },
                                        new SampleInfo { Tag = "BuiltInScales", Title = "Built-in scales (from years to hours)", Description = "Shows how you can combine and use built-in scale types and text header formats" },
                                        new SampleInfo { Tag = "CustomScaleHeaders", Title = "Custom scale (header texts)", Description = "Shows how to define a custom chart scale with text headers" },
                                        new SampleInfo { Tag = "SpecialDays", Title = "Special days (vertically highlight time intervals)", Description = "Shows how you can highlight special time intervals in the chart area (highlighted vertical bars)" },
                                        new SampleInfo { Tag = "ContinuousSchedule", Title = "Continuous schedule (non-stop working time)", Description = "Shows how you can define continuous working time for tasks (24/7)" },
                                        new SampleInfo { Tag = "CustomSchedule", Title = "Custom schedule and scales", Description = "Shows how you can define working and nonworking times for scheduling task items and custom scales and headers for the chart view" },
                                        new SampleInfo { Tag = "TimeConstraints", Title = "Time constraints (min-max start/finish)", Description = "Shows how you can defined limits for the start and finish dates and times of individual task items" },
                                        new SampleInfo { Tag = "SelectionMode", Title = "Selection mode", Description = "Shows how you can set selection mode to single or multiple items (extended)" },
                                        new SampleInfo { Tag = "Sorting", Title = "Sorting", Description = "Shows how you can hierarchically sort task items in the grid and chart view" },
                                        new SampleInfo { Tag = "Filtering", Title = "Filtering", Description = "Shows how you can hide specific task items for filtering purposes" },
                                        new SampleInfo { Tag = "ChangeNotifications", Title = "Change notifications", Description = "Shows how you can detect changes and perform custom actions when they occur" },
                                        new SampleInfo { Tag = "MouseEventHandling", Title = "Mouse event handling", Description = "Shows how you can determine the item and other elements bound to the cursor position for handling mouse events" },
                                        new SampleInfo { Tag = "MoveUpDown", Title = "Move up-down (hierarchical moving)", Description = "Shows how you can allow the end user to move up and down items without breaking the hierarchy" },
                                        new SampleInfo { Tag = "UndoRedo", Title = "Undo-redo", Description = "Shows how you can easily support undo and redo operations for item changes" },
                                        new SampleInfo { Tag = "WBSPath", Title = "WBS path", Description = "Shows how you can easily insert a WBS column in the grid" },
                                        new SampleInfo { Tag = "StatusColumns", Title = "Status columns (including color indicator)", Description = "Shows how to add supplemental custom columns for showing task statuses, such as To Do, In progress, Behind schedule, and Completed" },
                                        new SampleInfo { Tag = "CriticalPath", Title = "Critical path (simple algorithm or PERT method)", Description = "Shows how you can determine and highlight critical tasks in your project using either a simple algorithm (determining tasks that affect project finish, optionally with an acceptable delay) or the classic PERT method" },
                                        new SampleInfo { Tag = "WorkOptimizations", Title = "Work optimizations (reschedule, level resources)", Description = "Shows how you can optimize project timeline and avoid resource over-allocation" },
                                        new SampleInfo { Tag = "MaterialResources", Title = "Material resources (quantities and costs)", Description = "Shows how you can assign material resources having limited or unlimited available quantities and compute task costs based on the allocations" },
                                        new SampleInfo { Tag = "ImportingExportingXML", Title = "Importing and exporting Microsoft® Project XML", Description = "Shows how you can import and export Microsoft® Project XML schema based files, providing maximum compatibility with other applications" },
                                        new SampleInfo { Tag = "Printing", Title = "Printing", Description = "Shows how you can print the current Gantt Chart (with its associated data grid)" },
                                        new SampleInfo { Tag = "ExportImage", Title = "Export image", Description = "Shows how you can generate and save PNG images from the current Gantt Chart (and its associated data grid)" },
                                        new SampleInfo { Tag = "Performance", Title = "Performance (large data set)", Description = "Shows app responsiveness and other runtime performance features when loading large sets of hierarchical data" },
                                        new SampleInfo { Tag = "Styling", Title = "Styling", Description = "Shows how you can apply a custom style defining specific property values for the component element" },
                                        new SampleInfo { Tag = "Templating", Title = "Default templates", Description = "Source code providing expanded default XAML templates for specific component elements" },
                                        new SampleInfo { Tag = "CustomDatesAndDragging", Title = "Custom dates and dragging", Description = "Shows how you can define custom date and time item properties and display draggable secondary bars bound to them in the chart area" },
                                        new SampleInfo { Tag = "MultipleBarsPerLine", Title = "Multiple bars per line (display row index)", Description = "Shows how you can define and display multiple bars for a single task item in the same row of the chart area (i.e. item parts)" },
                                        new SampleInfo { Tag = "MinuteScale", Title = "Minute scale", Description = "Shows how you can customize scales and display task bars bound to hours and minutes" },
                                        new SampleInfo { Tag = "NumericDays", Title = "Numeric days", Description = "Shows how you can customize scales to display project week and day numbers instead of dates" },
                                        new SampleInfo { Tag = "Recurrence", Title = "Recurrence", Description = "Shows how you can define custom code to generate and display recurrent task items and chart bars" },
                                        new SampleInfo { Tag = "SummaryDragging", Title = "Summary dragging", Description = "Shows how you can add a thumb to allow the end user to drag summary bars and its children, having their start and end date-times automatically updated" },
                                        new SampleInfo { Tag = "SummaryBars", Title = "Summary bars", Description = "Shows how you can display child bars instead of summary bars when node items are collapsed" },
                                        new SampleInfo { Tag = "SummaryValues", Title = "Summary values", Description = "Shows how you can hierarchically summarize custom values such as custom task costs" },
                                        new SampleInfo { Tag = "HierarchicalVirtualization", Title = "Hierarchical virtualization", Description = "Shows how to develop summary task virtualization and lazy load child tasks only upon parent node expansion" },
                                        new SampleInfo { Tag = "AssignmentsTree", Title = "Assignments tree (using DataTreeGrid)", Description = "Shows how to show a custom popup allowing the end user to select assigned resources (or departments) from an organizational hierarchy – using DataTreeGrid control from DlhSoft Hierarchical Data Light Library, licensed separately" },
                                        new SampleInfo { Tag = "Database", IsLink = true, Title = "SQL Server® database integration", Description = "Load tasks from and save changes to a SQL Server® database" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "GanttChartView",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "ScheduleChartDataGrid",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "BasicUsage", Title = "Basic usage", Description = "Shows how to load the component with minimum configuration" },
                                        new SampleInfo { Tag = "AssigningTasks", Title = "Assigning tasks (drag unassigned items)", Description = "Shows how you can define a Schedule Chart view with draggable unassigned tasks" },
                                        new SampleInfo { Tag = "DataBinding", Title = "Data binding", Description = "Shows how you can data bind the component to a custom resource item collection" },
                                        new SampleInfo { Tag = "GanttChartIntegration", Title = "Gantt Chart integration", Description = "Shows how to generate a Schedule Chart view from Gantt Chart data" },
                                        new SampleInfo { Tag = "CustomAppearance", Title = "Custom appearance", Description = "Shows how you can set colors for all or for individual task items" },
                                        new SampleInfo { Tag = "BarTemplating", Title = "Bar templates", Description = "Shows how you can define XAML templates for task bars displayed in the chart view" },
                                        new SampleInfo { Tag = "CustomSchedule", Title = "Custom schedule and scales", Description = "Shows how you can define working and nonworking times for scheduling task items and custom scales and headers for the chart view" },
                                        new SampleInfo { Tag = "Hierarchy", Title = "Hierarchy", Description = "Shows how you can hierarchically display resource groups in the grid" },
                                        new SampleInfo { Tag = "MultipleLinesPerRow", Title = "Multiple lines per row", Description = "Shows how you can configure the component to display chart task bars using multiple lines per resource row automatically enlarging individual grid row height values" },
                                        new SampleInfo { Tag = "MouseEventHandling", Title = "Mouse event handling", Description = "Shows how you can determine the item and other elements bound to the cursor position for handling mouse events" },
                                        new SampleInfo { Tag = "StatusDisplaying", Title = "Status displaying (resource timeline)", Description = "Shows how you can easily display multiple resources and their status at different times using chart bars of different colors" },
                                        new SampleInfo { Tag = "ShiftScheduling", Title = "Shift scheduling (assigning employees on time shifts) ", Description = "Shows how you can define shifts as resource assignments so that the end user can drag and drop them vertically to change shifts as needed" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "ScheduleChartView",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "LoadChartDataGrid",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "CustomAppearance", Title = "Custom appearance", Description = "Shows how you can set colors for different types of allocation items" },
                                        new SampleInfo { Tag = "CustomSchedule", Title = "Custom schedule and scales", Description = "Shows how you can define working and nonworking times and custom scales and headers for the chart view" },
                                        new SampleInfo { Tag = "MouseEventHandling", Title = "Mouse event handling", Description = "Shows how you can determine the item and other elements bound to the cursor position for handling mouse events" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "LoadChartView",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "SingleItem", Title = "Single item", Description = "Shows how you can set up a single displayed item in the chart view" }
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "PertChartView",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "MultiTasksPerLine", Title = "Multiple tasks per line", Description = "Shows how you can extend task lines into multiple parallel items displayed between the same task event shapes, especially useful to avoid diagram complexity when generating items from a Gantt Chart source" },
                                    }
                                },
                                new ComponentInfo
                                {
                                    Name = "NetworkDiagramView",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" },
                                        new SampleInfo { Tag = "ShapeTemplating", Title = "Shape templating", Description = "Shows how you can define XAML templates for task shapes displayed in the view, optionally enabling item property editing as needed" }
                                    }
                                }
                            };
                            break;
                    }
                    break;
                case "Silverlight":
                    switch (programmingLanguage)
                    {
                        case "CSharp":
                            components = new[]
                            {
                                new ComponentInfo
                                {
                                    Name = "GanttChartDataGrid",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" }
                                    }
                                }
                            };
                            break;
                        case "VisualBasic":
                            components = new[]
                            {
                                new ComponentInfo
                                {
                                    Name = "GanttChartDataGrid",
                                    Features = new[]
                                    {
                                        new SampleInfo { Tag = "MainFeatures", Title = "Main features", Description = "Shows the main features of the component" }
                                    }
                                }
                            };
                            break;
                    }
                    break;
            }
            if (components == null)
                return;
            bool isFirst = true;
            foreach (var component in components.Where(c => c.Features != null))
            {
                var componentItem = new TabItem { Header = String.IsNullOrEmpty(component.Header) ? component.Name : component.Header, Tag = component.Name, IsSelected = isFirst };
                var listBox = new ListBox();
                listBox.SelectionMode = SelectionMode.Single;
                listBox.Tag = component.Name;
                foreach (var feature in component.Features)
                {
                    listBox.Items.Add(new ListBoxItem { Content = feature.Title, Tag = feature.Tag, ToolTip = feature.Description, IsSelected = isFirst, Opacity = feature.IsLink ? 0.65 : 1 });
                    isFirst = false;
                }
                componentItem.Content = listBox;
                listBox.SelectionChanged += ListBox_SelectionChanged;
                TabControl.Items.Add(componentItem);
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTabItem = (sender as TabControl).SelectedItem as TabItem;
            if (selectedTabItem == null)
                return;
            var listBox = selectedTabItem?.Content as ListBox;
            if (listBox != null)
            {
                ListBoxItem listBoxItem = (listBox.SelectedItems.Count > 0 ? listBox.SelectedItems[0] as ListBoxItem : null) ?? listBox.Items[0] as ListBoxItem;
                listBoxItem.IsSelected = true;
            }
            Dispatcher.BeginInvoke((Action)LoadFiles);
            Dispatcher.BeginInvoke((Action)LoadContent);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((e.AddedItems?.Count ?? 0) > 0)
            {
                Dispatcher.BeginInvoke((Action)LoadFiles);
                Dispatcher.BeginInvoke((Action)LoadContent);
            }
        }

        internal class ComponentInfo
        {
            public string Name { get; set; }
            public string Header { get; set; }
            public SampleInfo[] Features { get; set; }
        }
        internal class SampleInfo
        {
            public string Tag { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public bool IsLink { get; set; }
        }

        private void LoadFiles()
        {
            var fileItemCount = FilesListBox.Items.Count;
            var remainingItems = new[] { FilesListBox.Items[0], FilesListBox.Items[fileItemCount - 2], FilesListBox.Items[fileItemCount - 1] };
            var removingItems = new List<ListBoxItem>();
            foreach (ListBoxItem item in FilesListBox.Items)
            {
                if (!remainingItems.Contains(item))
                    removingItems.Add(item);
            }
            foreach (var item in removingItems)
                FilesListBox.Items.Remove(item);
            var selectedTabItem = TabControl.SelectedItem as TabItem;
            var selectedListBoxItem = (selectedTabItem?.Content as ListBox).SelectedItem as ListBoxItem;
            var selectedTechnologyItem = TechnologyComboBox?.SelectedItem as ComboBoxItem;
            if (selectedTabItem == null || selectedListBoxItem == null || selectedTechnologyItem == null)
                return;
            var component = selectedTabItem.Tag as string;
            var feature = selectedListBoxItem.Tag as string;
            var technology = selectedTechnologyItem.Tag as string;
            var isSilverlight = technology.StartsWith("Silverlight");
            var isVisualBasic = technology.EndsWith("VisualBasic");
            string[] fileItems = null;
            switch (component)
            {
                case "GanttChartDataGrid":
                    switch (feature)
                    {
                        case "MainFeatures":
                            fileItems = new[] {
                                "Main" + (!isSilverlight ? "Window" : "Page") + ".xaml",
                                "Main" + (!isSilverlight ? "Window" : "Page") + ".xaml" + (!isVisualBasic ? ".cs" : ".vb"),
                                "EditItemDialog.xaml",
                                "EditItemDialog.xaml" + (!isVisualBasic ? ".cs" : ".vb")
                            };
                            break;
                        case "DataBinding":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml" + (!isVisualBasic ? ".cs" : ".vb"), "CustomTaskItem" + (!isVisualBasic ? ".cs" : ".vb") };
                            break;
                        case "BarTemplating":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml" + (!isVisualBasic ? ".cs" : ".vb"), "CustomGanttChartItem" + (!isVisualBasic ? ".cs" : ".vb"), "Interruption" + (!isVisualBasic ? ".cs" : ".vb"), "Marker" + (!isVisualBasic ? ".cs" : ".vb") };
                            break;
                        case "Printing":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml" + (!isVisualBasic ? ".cs" : ".vb"), "PrintDialog.xaml", "PrintDialog.xaml" + (!isVisualBasic ? ".cs" : ".vb") };
                            break;
                        case "NumericDays":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml" + (!isVisualBasic ? ".cs" : ".vb"), "NumericDayStringConverter" + (!isVisualBasic ? ".cs" : ".vb") };
                            break;
                        case "Recurrence":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml" + (!isVisualBasic ? ".cs" : ".vb"), "RecurrentGanttChartItem" + (!isVisualBasic ? ".cs" : ".vb"), "RecurrenceType" + (!isVisualBasic ? ".cs" : ".vb"), "UnlimitedIntConverter" + (!isVisualBasic ? ".cs" : ".vb") };
                            break;
                        case "SummaryValues":
                        case "CustomDatesAndDragging":
                        case "AssignmentsTemplate":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml" + (!isVisualBasic ? ".cs" : ".vb"), "CustomGanttChartItem" + (!isVisualBasic ? ".cs" : ".vb") };
                            break;
                        case "GridColumns":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml" + (!isVisualBasic ? ".cs" : ".vb"), "CustomGanttChartItem" + (!isVisualBasic ? ".cs" : ".vb"), "CustomDataObject" + (!isVisualBasic ? ".cs" : ".vb"), "GanttChartItemAttachments" + (!isVisualBasic ? ".cs" : ".vb") };
                            break;
                        case "StatusColumns":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml" + (!isVisualBasic ? ".cs" : ".vb"), "StatusGanttChartItem" + (!isVisualBasic ? ".cs" : ".vb") };
                            break;
                        case "AssignmentsTree":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml" + (!isVisualBasic ? ".cs" : ".vb"), "EditAssignmentsTreeDialog.xaml", "EditAssignmentsTreeDialog.xaml" + (!isVisualBasic ? ".cs" : ".vb") };
                            break;
                    }
                    break;
                case "ScheduleChartDataGrid":
                    switch (feature)
                    {
                        case "BarTemplating":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml" + (!isVisualBasic ? ".cs" : ".vb"), "CustomGanttChartItem" + (!isVisualBasic ? ".cs" : ".vb"), "Interruption" + (!isVisualBasic ? ".cs" : ".vb"), "Marker" + (!isVisualBasic ? ".cs" : ".vb") };
                            break;
                        case "DataBinding":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml" + (!isVisualBasic ? ".cs" : ".vb"), "CustomResourceItem" + (!isVisualBasic ? ".cs" : ".vb") };
                            break;
                    }
                    break;
                case "NetworkDiagramView":
                    switch (feature)
                    {
                        case "ShapeTemplating":
                            fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml" + (!isVisualBasic ? ".cs" : ".vb"), "CustomNetworkDiagramItem" + (!isVisualBasic ? ".cs" : ".vb") };
                            break;
                    }
                    break;
            }
            if (fileItems == null)
                fileItems = new[] { "MainWindow.xaml", "MainWindow.xaml" + (!isVisualBasic ? ".cs" : ".vb") };
            int index = 1;
            foreach (var fileItem in fileItems)
                FilesListBox.Items.Insert(index++, new ListBoxItem { Content = fileItem, Tag = fileItem });
            if (!isSilverlight && selectedListBoxItem.Opacity == 1)
                FilesListBox.Items.Insert(index++, new ListBoxItem { Content = "AppResources.xaml", Tag = "AppResources.xaml" });
            if (FilesListBox.SelectedIndex > 0)
                FilesListBox.SelectedIndex = 0;
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)LoadContent);
        }

        private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)LoadContent);
        }

        private void LoadContent()
        {
            var selectedTabItem = TabControl.SelectedItem as TabItem;
            if (selectedTabItem == null) return;
            var selectedlistBoxItem = (selectedTabItem?.Content as ListBox).SelectedItem as ListBoxItem;
            var selectedTechnologyItem = TechnologyComboBox?.SelectedItem as ComboBoxItem;
            if (selectedTabItem == null || selectedlistBoxItem == null || selectedTechnologyItem == null)
                return;

            var feature = selectedlistBoxItem.Tag as string;
            var component = selectedTabItem.Tag as string;
            var technology = selectedTechnologyItem.Tag as string;
            var technologySeparatorIndex = technology.IndexOf('-');
            var platform = technology.Substring(0, technologySeparatorIndex);
            var programmingLanguage = technology.Substring(technologySeparatorIndex + 1);
            var selectedFileItem = FilesListBox.SelectedItem as ListBoxItem;
            if (selectedFileItem == null || selectedFileItem.Visibility != Visibility.Visible)
            {
                var runListBoxItem = FilesListBox.Items[0] as ListBoxItem;
                FilesListBox.SelectedIndex = runListBoxItem.Visibility == Visibility.Visible ? 0 : 1;
                return;
            }
            var selectedFileUrl = selectedFileItem.Tag as string;
            if (selectedFileUrl == null)
            {
                if (containerWindow != null)
                    containerWindow.Close();
                var selectedThemeItem = ThemeComboBox.SelectedItem as ComboBoxItem;
                var theme = selectedThemeItem?.Tag as string;
                var path = "Demos." + platform + "." + programmingLanguage + "." + component + "." + feature;
                containerWindow = Activator.CreateInstance(path, path + ".MainWindow", false, BindingFlags.Default, null, new[] { theme }, null, null).Unwrap() as Window;
                ContentPresenter.Content = containerWindow.Content;
                ContentPresenter.Visibility = Visibility.Visible;
                ContentTextBox.Visibility = Visibility.Hidden;
                ContentTextBox.Text = null;
            }
            else
            {
                var file = selectedFileItem.Tag as string;
                try
                {
                    var resourceStreamInfo = Application.GetResourceStream(new Uri("/Samples.Resources/" + technology + "/" + (file.StartsWith("Themes/") ? selectedFileUrl : (component + "/" + feature + "/" + selectedFileUrl)), UriKind.Relative));
                    using (var resourceStreamReader = new StreamReader(resourceStreamInfo.Stream))
                    {
                        ContentTextBox.Text = resourceStreamReader.ReadToEnd();
                    }
                }
                catch (IOException) { }
                ContentTextBox.Visibility = Visibility.Visible;
                ContentPresenter.Visibility = Visibility.Hidden;
                ContentPresenter.Content = null;
            }
        }

        private void GetZipButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTabItem = TabControl.SelectedItem as TabItem;
            var selectedListBoxItem = (selectedTabItem?.Content as ListBox).SelectedItem as ListBoxItem;
            var selectedTechnologyItem = TechnologyComboBox?.SelectedItem as ComboBoxItem;
            if (selectedTabItem == null || selectedListBoxItem == null || selectedTechnologyItem == null)
                return;
            var component = selectedTabItem.Tag as string;
            var feature = selectedListBoxItem.Tag as string;
            var technology = selectedTechnologyItem.Tag as string;
            string url = "https://DlhSoft.com/GanttChartLibrary.wpf/Demos/Samples/" + technology + "/" + component + "/" + feature + ".zip";
            Process.Start(new ProcessStartInfo(url));
        }

        private void TreeViewSearchTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TreeViewSearchTextBox.Foreground = Brushes.Black;
            TreeViewSearchTextBox.Text = string.Empty;
        }

        private void TreeViewSearchTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TreeViewSearchTextBox.Foreground = Brushes.Gray;
            TreeViewSearchTextBox.Text = "Search...";
        }

        private void TreeViewSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TabControl == null || string.IsNullOrEmpty(TreeViewSearchTextBox.Text) || TreeViewSearchTextBox.Foreground != Brushes.Black)
                return;

            if (timer == null)
            {
                timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.35) };
                timer.Tick += (ts, te) => 
                {
                    timer.Stop();
                    if (TreeViewSearchTextBox.Foreground != Brushes.Black)
                        return;
                    FindListBoxItem(TreeViewSearchTextBox.Text.ToLowerInvariant());
                };
            }
            timer.Stop();
            timer.Start();
        }

        private DispatcherTimer timer;

        private void FindListBoxItem(string text)
        {
            foreach (TabItem tabItem in TabControl.Items)
            {
                foreach (ListBoxItem item in (tabItem.Content as ListBox).Items)
                {
                    if ((item.Content as string ?? string.Empty).ToLowerInvariant().Contains(text))
                    {
                        tabItem.IsSelected = true;
                        item.IsSelected = true;
                        return;
                    }
                }
            }
        }

        private Window containerWindow;

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (containerWindow != null)
                containerWindow.Close();
        }
    }
}
