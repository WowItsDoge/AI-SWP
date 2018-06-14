// <copyright file="GraphViewerControl.xaml.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
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
    using UseCaseCore.UcIntern;

    /// <summary>
    /// Interaction logic for GraphViewerControl
    /// </summary>
    public partial class GraphViewerControl : UserControl
    {
        /// <summary>
        /// reference to the viewer control
        /// </summary>
        private Microsoft.Msagl.GraphViewerGdi.GViewer viewer;

        /// <summary>
        /// reference to the visible graph
        /// </summary>
        private Microsoft.Msagl.Drawing.Graph graph;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphViewerControl"/> class.
        /// </summary>
        public GraphViewerControl()
        {
            this.InitializeComponent();

            // create a viewer object 
            this.viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();

            // create a graph object 
            this.graph = new Microsoft.Msagl.Drawing.Graph("graph");

            /*
            // example graph
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            graph.AddEdge("A", "C").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
            graph.FindNode("A").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
            graph.FindNode("B").Attr.FillColor = Microsoft.Msagl.Drawing.Color.MistyRose;
            Microsoft.Msagl.Drawing.Node c = graph.FindNode("C");
            c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleGreen;
            */

            // bind the graph to the viewer 
            this.viewer.Graph = this.graph;
            this.viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewer.OutsideAreaBrush = System.Drawing.Brushes.White;
            this.viewer.ToolBarIsVisible = false;

            // display the windows form control in the wpf view
            this.GraphView.Child = this.viewer;

            // on graph view update
            this.viewer.Invalidated += this.Viewer_Invalidated;
        }

        /// <summary>
        /// Event handler when the graph visualization was changed
        /// </summary>
        public event EventHandler GraphVisualisationChanged;

        /// <summary>
        /// zooms the graph in the viewer control
        /// </summary>
        /// <param name="delta">the zoom value</param>
        public void Zoom(double delta)
        {
            this.viewer.ZoomF *= delta;
        }

        /// <summary>
        /// moves the graph in the viewer control
        /// </summary>
        /// <param name="x">the horizontal position delta</param>
        /// <param name="y">the vertical position delta</param>
        public void Move(double x, double y)
        {
            double[][] currentTransform = this.GetTransformMatrix();

            this.viewer.Transform = new Microsoft.Msagl.Core.Geometry.Curves.PlaneTransformation(
                currentTransform[0][0], currentTransform[0][1], currentTransform[0][2] + x,
                currentTransform[1][0], currentTransform[1][1], currentTransform[1][2] + y);

            this.viewer.DrawingPanel.Invalidate();
        }

        /// <summary>
        /// displays the graph
        /// </summary>
        /// <param name="useCase">the use case to display</param>
        public void UpdateGraphView(UseCase useCase)
        {
            this.graph = new Microsoft.Msagl.Drawing.Graph("graph");

            for (int n1 = 0; n1 < useCase.Nodes.Count; n1++)
            {
                for (int n2 = 0; n2 < useCase.Nodes.Count; n2++)
                {
                    if (IsConnected(n1, n2, useCase))
                    {
                        string nodeTitle1 = GetNodeTitle(useCase.Nodes[n1], n1);
                        string nodeTitle2 = GetNodeTitle(useCase.Nodes[n2], n2);

                        var edge = this.graph.AddEdge(nodeTitle1, nodeTitle2);
                        edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Gray;

                        var node1 = this.graph.FindNode(nodeTitle1);
                        SetNodeStyle(node1);

                        var node2 = this.graph.FindNode(nodeTitle2);
                        SetNodeStyle(node2);
                    }
                }
            }

            this.viewer.Graph = this.graph;
        }

        /// <summary>
        /// Get the horizontal graph range
        /// </summary>
        /// <returns>the minimum and maximum x value</returns>
        public Tuple<double, double> GetGraphRangeX()
        {
            return new Tuple<double, double>(-1000, 1000);
        }

        /// <summary>
        /// Get the vertical graph range
        /// </summary>
        /// <returns>the minimum and maximum y value</returns>
        public Tuple<double, double> GetGraphRangeY()
        {
            return new Tuple<double, double>(-1000, 1000);
        }

        /// <summary>
        /// Get the current graph x position
        /// </summary>
        /// <returns>graph x position</returns>
        public double GetGraphX()
        {
            var transformMatrix = this.GetTransformMatrix();

            return transformMatrix[0][2];
        }

        /// <summary>
        /// Get the current graph y position
        /// </summary>
        /// <returns>graph y position</returns>
        public double GetGraphY()
        {
            var transformMatrix = this.GetTransformMatrix();

            return transformMatrix[1][2];
        }

        /// <summary>
        /// Set the style of a graph node
        /// </summary>
        /// <param name="node">the microsoft drawing node</param>
        private static void SetNodeStyle(Microsoft.Msagl.Drawing.Node node)
        {
            var backgroundColor = MaterialDesignColors.RandomMsaglColor();
            var backgroundHex = MaterialDesignColors.MsaglColorToHex(backgroundColor);
            var foregroundHex = MaterialDesignColors.GetForegroundColor(backgroundHex);
            var foregroundColor = MaterialDesignColors.MsaglColorFromHex(foregroundHex);

            node.Attr.FillColor = backgroundColor;
            node.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Box;
            node.Attr.Color = MaterialDesignColors.MsaglColorFromHex(MaterialDesignColors.White);
            node.Label.FontColor = foregroundColor;
            node.Label.FontSize = 10;
        }

        /// <summary>
        /// Generates the title for a graph node
        /// </summary>
        /// <param name="node">the node</param>
        /// <param name="nodeId">the id</param>
        /// <returns>the graph node title</returns>
        private static string GetNodeTitle(Node node, int nodeId)
        {
            return nodeId + ": " + node.StepDescription;
        }

        /// <summary>
        /// Returns true if both nodes are connected
        /// </summary>
        /// <param name="node1">the first node to compare</param>
        /// <param name="node2">the second node to compare</param>
        /// <param name="useCase">the use case</param>
        /// <returns>true, if both nodes are connected</returns>
        private static bool IsConnected(Node node1, Node node2, UseCase useCase)
        {
            int id1 = GetNodeId(node1, useCase);
            int id2 = GetNodeId(node2, useCase);

            return IsConnected(id1, id2, useCase);
        }

        /// <summary>
        /// returns true if both nodes are connected
        /// </summary>
        /// <param name="nodeId1">the id of node 1</param>
        /// <param name="nodeId2">the id of node 2</param>
        /// <param name="useCase">the use case object</param>
        /// <returns>true, if both nodes are connected</returns>
        private static bool IsConnected(int nodeId1, int nodeId2, UseCase useCase)
        {
            return useCase.EdgeMatrix[nodeId1, nodeId2];
        }

        /// <summary>
        /// returns the node id
        /// </summary>
        /// <param name="node">the node object</param>
        /// <param name="useCase">the use case object</param>
        /// <returns>the node id</returns>
        private static int GetNodeId(Node node, UseCase useCase)
        {
            for (int i = 0; i < useCase.Nodes.Count; i++)
            {
                if (node.Equals(useCase.Nodes[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns the transform matrix of the graph viewer.
        /// </summary>
        /// <returns>the transform matrix</returns>
        private double[][] GetTransformMatrix()
        {
            return (double[][])typeof(Microsoft.Msagl.Core.Geometry.Curves.PlaneTransformation)
                .GetField("elements", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this.viewer.Transform);
        }

        /// <summary>
        /// The graph invalidation
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the event args</param>
        private void Viewer_Invalidated(object sender, System.Windows.Forms.InvalidateEventArgs e)
        {
            if (this.GraphVisualisationChanged != null)
            {
                this.GraphVisualisationChanged(this, null);
            }
        }
    }
}
