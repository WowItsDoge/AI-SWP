// <copyright file="GraphBuilder.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    using System.Collections.Generic;

    /// <summary>
    /// This class transforms the use case description consisting of the different flow types into a graph representation.
    /// </summary>
    public class GraphBuilder
    {
        /// <summary>
        /// Builds the graph.
        /// </summary>
        /// <param name="basicFlow">The basic flow.</param>
        /// <param name="specificAlternativeFlows">The specific alternative flows.</param>
        /// <param name="globalAlternativeFlows">The global alternative flows.</param>
        /// <param name="boundedAlternativeFlows">The bounded alternative flows.</param>
        /// <param name="nodes">The nodes of all flows.</param>
        /// <param name="edgeMatrix">The edge matrix for the nodes.</param>
        /// <param name="conditionMatrix">The condition matrix for the flows.</param>
        public void BuildGraph(
            Flow basicFlow,
            IReadOnlyList<Flow> specificAlternativeFlows,
            IReadOnlyList<Flow> globalAlternativeFlows,
            IReadOnlyList<Flow> boundedAlternativeFlows,
            out IReadOnlyList<Node> nodes,
            out Matrix<bool> edgeMatrix,
            out Matrix<Condition?> conditionMatrix)
        {
            nodes = null;
            edgeMatrix = null;
            conditionMatrix = null;
        }

        /// <summary>
        /// Sets the edges in a block of nodes.
        /// </summary>
        /// <param name="nodes">The nodes to wire.</param>
        /// <param name="edgeMatrix">The matrix with the edges.</param>
        public void SetEdgesInNodeBlock(IReadOnlyList<Node> nodes, out Matrix<bool> edgeMatrix)
        {
            // out of external edges
            edgeMatrix = null;
        }
    }
}
