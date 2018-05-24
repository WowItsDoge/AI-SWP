// <copyright file="GraphViewerControl.xaml.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

    /// <summary>
    /// Interaction logic for GraphViewerControl
    /// </summary>
    public partial class GraphViewerControl : UserControl
    {
        private Microsoft.Msagl.GraphViewerGdi.GViewer viewer;

        private Microsoft.Msagl.Drawing.Graph graph;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphViewerControl"/> class.
        /// </summary>
        public GraphViewerControl()
        {
            this.InitializeComponent();

            // create a viewer object 
            viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();

            // create a graph object 
            graph = new Microsoft.Msagl.Drawing.Graph("graph");

            // create the graph content 
            UpdateGraphView();

            // bind the graph to the viewer 
            viewer.Graph = graph;
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            viewer.OutsideAreaBrush = System.Drawing.Brushes.White;
            viewer.ToolBarIsVisible = false;

            // display the windows form control in the wpf view
            GraphView.Child = viewer;
        }

        public void Zoom(double delta)
        {
            viewer.ZoomF *= delta;
        }

        public void Move(double x, double y)
        {
            viewer.Transform = new Microsoft.Msagl.Core.Geometry.Curves.PlaneTransformation(1, 0, x, 0, 1, -y);
            viewer.DrawingPanel.Invalidate();
        }

        private void UpdateGraphView()
        {
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            graph.AddEdge("A", "C").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
            graph.FindNode("A").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
            graph.FindNode("B").Attr.FillColor = Microsoft.Msagl.Drawing.Color.MistyRose;
            Microsoft.Msagl.Drawing.Node c = graph.FindNode("C");
            c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleGreen;
            c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
        }
    }
}
