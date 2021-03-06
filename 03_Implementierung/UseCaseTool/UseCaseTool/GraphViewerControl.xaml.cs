﻿// <copyright file="GraphViewerControl.xaml.cs" company="Team B">
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
        /// List of node title and node color elements
        /// </summary>
        private static List<Tuple<string, Microsoft.Msagl.Drawing.Color>> nodeColors;

        /// <summary>
        /// List of flow id, flow color palette, last color id used
        /// </summary>
        private static List<Tuple<int, string[], int>> flowIdPalette;

        /// <summary>
        /// reference to the viewer control
        /// </summary>
        private Microsoft.Msagl.GraphViewerGdi.GViewer viewer;

        /// <summary>
        /// reference to the visible graph
        /// </summary>
        private Microsoft.Msagl.Drawing.Graph graph;

        /// <summary>
        /// Reference to the use case object
        /// </summary>
        private UseCase useCase;

        /// <summary>
        /// If true, the graph is displayed with titles; if false, only the ids are displayed
        /// </summary>
        private bool displayGraphTitles;

        /// <summary>
        /// If true, the graph is displayed with conditions
        /// </summary>
        private bool displayGraphConditions;

        /// <summary>
        /// The graph transform matrix after the initialization
        /// </summary>
        private double[][] initialTransform;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphViewerControl"/> class.
        /// </summary>
        public GraphViewerControl()
        {
            this.InitializeComponent();

            GraphViewerControl.nodeColors = new List<Tuple<string, Microsoft.Msagl.Drawing.Color>>();

            GraphViewerControl.flowIdPalette = new List<Tuple<int, string[], int>>();

            this.displayGraphTitles = true;
            this.displayGraphConditions = true;

            // create a viewer object 
            this.viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();

            // create a graph object 
            this.graph = new Microsoft.Msagl.Drawing.Graph("graph");

            // bind the graph to the viewer 
            this.viewer.Graph = this.graph;
            this.viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewer.OutsideAreaBrush = System.Drawing.Brushes.White;
            this.viewer.ToolBarIsVisible = false;

            // display the windows form control in the wpf view
            this.GraphView.Child = this.viewer;

            // on graph view update
            this.viewer.Paint += this.Viewer_Invalidated;
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
        /// sets the graph position in the viewer control
        /// </summary>
        /// <param name="x">the horizontal position</param>
        /// <param name="y">the vertical position</param>
        public void SetPosition(double x, double y)
        {
            if (this.initialTransform == null)
            {
                return;
            }

            this.viewer.Transform = new Microsoft.Msagl.Core.Geometry.Curves.PlaneTransformation(
                this.initialTransform[0][0], this.initialTransform[0][1], this.initialTransform[0][2] + x,
                this.initialTransform[1][0], this.initialTransform[1][1], this.initialTransform[1][2] + y);

            this.viewer.DrawingPanel.Invalidate();
        }

        /// <summary>
        /// displays the graph
        /// </summary>
        /// <param name="useCase">the use case to display</param>
        /// <returns>true, if the graph could be displayed</returns>
        public bool UpdateGraphView(UseCase useCase)
        {
            this.useCase = useCase;
            this.graph = new Microsoft.Msagl.Drawing.Graph("graph");

            var layoutSettings = (Microsoft.Msagl.Layout.Layered.SugiyamaLayoutSettings)this.graph.LayoutAlgorithmSettings;
            layoutSettings.EdgeRoutingSettings.EdgeRoutingMode = Microsoft.Msagl.Core.Routing.EdgeRoutingMode.SplineBundling;

            // if no graph should be displayed
            if (this.useCase == null)
            {
                this.viewer.Graph = this.graph;

                return false;
            }

            for (int n1 = 0; n1 < useCase.Nodes.Count; n1++)
            {
                for (int n2 = 0; n2 < useCase.Nodes.Count; n2++)
                {
                    if (IsConnected(n1, n2, useCase))
                    {
                        this.GenerateNodes(n1, n2, useCase);
                    }
                }
            }

            var layout = new Microsoft.Msagl.Layout.MDS.MdsLayoutSettings();

            this.viewer.Graph = this.graph;
            this.initialTransform = this.GetTransformMatrix();

            return true;
        }

        /// <summary>
        /// Updates the graph view, with the current use case file
        /// </summary>
        /// <returns>true, if the graph could be displayed</returns>
        public bool UpdateGraphView()
        {
            return this.UpdateGraphView(this.useCase);
        }

        /// <summary>
        /// This method changes the colors of the graph
        /// </summary>
        public void ChangeGraphColors()
        {
            nodeColors.Clear();
            flowIdPalette.Clear();

            this.UpdateGraphView();
        }

        /// <summary>
        /// This method changes the graph titles
        /// </summary>
        /// <param name="displayGraphTitles">true, if the graph titles should be displayed</param>
        public void ChangeDisplayTitles(bool displayGraphTitles)
        {
            this.displayGraphTitles = displayGraphTitles;

            this.UpdateGraphView();
        }

        /// <summary>
        /// This method changes the graph conditions
        /// </summary>
        /// <param name="displayGraphConditions">true, if the graph conditions should be displayed</param>
        public void ChangeDisplayConditions(bool displayGraphConditions)
        {
            this.displayGraphConditions = displayGraphConditions;

            this.UpdateGraphView();
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
        /// Clears the graph view
        /// </summary>
        public void ClearGraph()
        {
            this.useCase = null;
        }

        /// <summary>
        /// Set the style of a graph node
        /// </summary>
        /// <param name="node">the microsoft drawing node</param>
        /// <param name="nodeTitle">the node title</param>
        /// <param name="flowId">the flow id</param>
        private static void SetNodeStyle(Microsoft.Msagl.Drawing.Node node, string nodeTitle, int flowId)
        {
            var backgroundColor = GetNodeColor(nodeTitle, flowId);
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
        /// Returns the color for a use case item
        /// </summary>
        /// <param name="nodeTitle">The node title</param>
        /// <param name="flowId">The flow id</param>
        /// <returns>The microsoft algorithm drawing color</returns>
        private static Microsoft.Msagl.Drawing.Color GetNodeColor(string nodeTitle, int flowId)
        {
            // if the node color exists
            for (int i = 0; i < nodeColors.Count; i++)
            {
                if (nodeColors[i].Item1.Equals(nodeTitle))
                {
                    // return it
                    return nodeColors[i].Item2;
                }
            }

            // if the node doens´t exists, generate a new color
            var colorHex = FlowPaletteGetNextColor(flowId);
            var color = MaterialDesignColors.MsaglColorFromHex(colorHex);

            var nodeColor = new Tuple<string, Microsoft.Msagl.Drawing.Color>(nodeTitle, color);
            nodeColors.Add(nodeColor);

            return nodeColor.Item2;
        }

        /// <summary>
        /// Returns the id of a flow palette for a flow id
        /// </summary>
        /// <param name="flowId">the flow id</param>
        /// <returns>the flow palette id</returns>
        private static int GetFlowPaletteId(int flowId)
        {
            for (int i = 0; i < flowIdPalette.Count; i++)
            {
                if (flowIdPalette[i].Item1 == flowId)
                {
                    return i;
                }
            }

            // if the palette doesn´t exist for the flow id, create it
            var flowPaletteId = flowIdPalette.Count;

            flowIdPalette.Add(new Tuple<int, string[], int>(flowId, MaterialDesignColors.GetRandomPalette(), 0));

            return flowPaletteId;
        }

        /// <summary>
        /// Gets the next flow palette color
        /// </summary>
        /// <param name="flowId">the flow id</param>
        /// <returns>the hex color value</returns>
        private static string FlowPaletteGetNextColor(int flowId)
        {
            // get the flow palette id for the flow id
            var id = GetFlowPaletteId(flowId);

            // get the color palette
            string[] colorPalette = flowIdPalette[id].Item2;

            // get the next color id
            int colorId = (flowIdPalette[id].Item3 + 1) % (colorPalette.Length - 1);

            // save the next color id
            flowIdPalette[id] = new Tuple<int, string[], int>(flowId, colorPalette, colorId);

            return colorPalette[colorId];
        }

        /// <summary>
        /// Displays the edge condition
        /// </summary>
        /// <param name="n1">id node 1</param>
        /// <param name="n2">id node 2</param>
        /// <param name="node1">node object 1</param>
        /// <param name="node2">node object 2</param>
        /// <param name="edge">edge object</param>
        /// <param name="useCase">use case object</param>
        private void DisplayEdgeCondition(int n1, int n2, Microsoft.Msagl.Drawing.Node node1, Microsoft.Msagl.Drawing.Node node2, Microsoft.Msagl.Drawing.Edge edge, UseCase useCase)
        {
            UseCaseCore.UcIntern.Condition condition = useCase.ConditionMatrix[n1, n2] ?? new UseCaseCore.UcIntern.Condition();
            if (this.displayGraphConditions && condition.ConditionText != null)
            {
                edge.LabelText = condition.ConditionText + " is " + condition.ConditionState;
                edge.Label.FontSize = node1.Label.FontSize / 2;
                edge.Label.FontColor = node1.Attr.FillColor;
            }
        }

        /// <summary>
        /// Adds a graph layout constraint
        /// </summary>
        /// <param name="n1">id node 1</param>
        /// <param name="n2">id node 2</param>
        /// <param name="node1">node object 1</param>
        /// <param name="node2">node object 2</param>
        /// <param name="graph">graph object</param>
        /// <param name="useCase">use case object</param>
        private void AddLayerConstraint(int n1, int n2, Microsoft.Msagl.Drawing.Node node1, Microsoft.Msagl.Drawing.Node node2, Microsoft.Msagl.Drawing.Graph graph, UseCase useCase)
        {
            if (useCase.Nodes[n1].Identifier.Id == useCase.Nodes[n2].Identifier.Id &&
                            useCase.Nodes[n1].Identifier.Type == FlowType.Basic &&
                            useCase.Nodes[n2].Identifier.Type == FlowType.Basic &&
                            n1 < n2)
            {
                graph.LayerConstraints.AddSequenceOfUpDownVerticalConstraint(node1, node2);
            }
        }

        /// <summary>
        /// Generates the nodes
        /// </summary>
        /// <param name="n1">id node 1</param>
        /// <param name="n2">id node 2</param>
        /// <param name="useCase">use case object</param>
        private void GenerateNodes(int n1, int n2, UseCase useCase)
        {
            string nodeTitle1 = this.GetNodeTitle(useCase.Nodes[n1], n1);
            string nodeTitle2 = this.GetNodeTitle(useCase.Nodes[n2], n2);

            var edge = this.graph.AddEdge(nodeTitle1, nodeTitle2);

            var node1 = this.graph.FindNode(nodeTitle1);
            SetNodeStyle(node1, nodeTitle1, useCase.Nodes[n1].Identifier.GetHashCode());

            var node2 = this.graph.FindNode(nodeTitle2);
            SetNodeStyle(node2, nodeTitle2, useCase.Nodes[n2].Identifier.GetHashCode());

            edge.Attr.Color = GetNodeColor(nodeTitle1, useCase.Nodes[n1].Identifier.GetHashCode());

            this.DisplayEdgeCondition(n1, n2, node1, node2, edge, useCase);
            this.AddLayerConstraint(n1, n2, node1, node2, this.graph, useCase);
        }

        /// <summary>
        /// Generates the title for a graph node
        /// </summary>
        /// <param name="node">the node</param>
        /// <param name="nodeId">the id</param>
        /// <returns>the graph node title</returns>
        private string GetNodeTitle(Node node, int nodeId)
        {
            if (!this.displayGraphTitles)
            {
                return (nodeId + 1).ToString();
            }

            return "id: " + (nodeId + 1) + " flow: " + node.Identifier.Id + " type: " + node.Identifier.Type + "\r\n" + this.InsertLines(node.StepDescription);
        }

        /// <summary>
        /// Inserts line breaks
        /// </summary>
        /// <param name="text">the text to display</param>
        /// <returns>the text with line breaks</returns>
        private string InsertLines(string text)
        {
            int myLimit = 40;
            string[] words = text.Split(' ');

            StringBuilder newSentence = new StringBuilder();

            string line = string.Empty;
            foreach (string word in words)
            {
                if ((line + word).Length > myLimit)
                {
                    newSentence.AppendLine(line);
                    line = string.Empty;
                }

                line += string.Format("{0} ", word);
            }

            if (line.Length > 0)
            {
                newSentence.AppendLine(line);
            }

            return newSentence.ToString();
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
        private void Viewer_Invalidated(object sender, EventArgs e)
        {
            if (this.GraphVisualisationChanged != null)
            {
                this.GraphVisualisationChanged(this, null);
            }
        }
    }
}
