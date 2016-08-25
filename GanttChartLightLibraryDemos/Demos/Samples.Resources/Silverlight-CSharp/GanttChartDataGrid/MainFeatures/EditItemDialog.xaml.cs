using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GanttChartDataGridSample
{
    public partial class EditItemDialog : ChildWindow
    {
        public EditItemDialog()
        {
            // Copy the assignable resource collection reference from the main page before the InitializeComponent call.
            Resources.Add("AssignableResources", (Application.Current.RootVisual as FrameworkElement).Resources["AssignableResources"]);

            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

