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
    using System.Reflection;
    using UseCaseCore.UcIntern;

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
            double[][] currentTransform = (double[][])typeof(Microsoft.Msagl.Core.Geometry.Curves.PlaneTransformation)
                .GetField("elements", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(viewer.Transform);

            viewer.Transform = new Microsoft.Msagl.Core.Geometry.Curves.PlaneTransformation(
                currentTransform[0][0], currentTransform[0][1], currentTransform[0][2] + x,
                currentTransform[1][0], currentTransform[1][1], currentTransform[1][2] + y);

            viewer.DrawingPanel.Invalidate();
        }

        public void UpdateGraphView(UseCase useCase)
        {
            graph = new Microsoft.Msagl.Drawing.Graph("graph");
            viewer.Graph = graph;

            for (int n1 = 0; n1 < useCase.Nodes.Count; n1++)
            {
                for (int n2 = 0; n2 < useCase.Nodes.Count; n2++)
                {
                    if (IsConnected(n1, n2, useCase))
                    {
                        string nodeTitle1 = GetNodeTitle(useCase.Nodes[n1], n1);
                        string nodeTitle2 = GetNodeTitle(useCase.Nodes[n2], n2);

                        var edge = graph.AddEdge(nodeTitle1, nodeTitle2);
                        edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Green;

                        var node1 = graph.FindNode(nodeTitle1);
                        SetNodeStyle(node1);

                        var node2 = graph.FindNode(nodeTitle2);
                        SetNodeStyle(node2);
                    }
                }
            }
        }

        private static void SetNodeStyle(Microsoft.Msagl.Drawing.Node node)
        {
            node.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
            node.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Ellipse;
        }

        private static string GetNodeTitle(Node node, int nodeId)
        {
            return nodeId + ": " + node.StepDescription;
        }

        private static bool IsConnected(Node node1, Node node2, UseCase useCase)
        {
            int id1 = GetNodeId(node1, useCase);
            int id2 = GetNodeId(node2, useCase);

            return IsConnected(id1, id2, useCase);
        }

        private static bool IsConnected(int nodeId1, int nodeId2, UseCase useCase)
        {
            return useCase.EdgeMatrix[nodeId1, nodeId2];
        }

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
    }
}
