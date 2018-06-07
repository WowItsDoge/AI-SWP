// <copyright file="TestUseCase.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
    using System.Collections.Generic;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A class for tests where it is possible to set the normaly not setable properties.
    /// </summary>
    public class TestUseCase : UseCase
    {
        /// <summary>
        /// Gets this test use case as use case.
        /// </summary>
        public UseCase UseCase
        {
            get
            {
                return (UseCase)this;
            }
        }

        /// <summary>
        /// Sets the nodes.
        /// </summary>
        /// <param name="nodes">The nodes to set.</param>
        public void SetNodes(List<Node> nodes)
        {
            this.Nodes = nodes.AsReadOnly();
        }

        /// <summary>
        /// Sets the edge matrix.
        /// </summary>
        /// <param name="edgeMatrix">The edge matrix to set.</param>
        public void SetEdgeMatrix(Matrix<bool> edgeMatrix)
        {
            this.EdgeMatrix = edgeMatrix.AsReadonly();
        }

        /// <summary>
        /// Sets the condition matrix.
        /// </summary>
        /// <param name="conditionMatrix">The condition matrix to set.</param>
        public void SetConditionMatrix(Matrix<Condition?> conditionMatrix)
        {
            this.ConditionMatrix = conditionMatrix.AsReadonly();
        }

        /// <summary>
        /// Sets the basic flow.
        /// </summary>
        /// <param name="basicFlow">The basic flow to set.</param>
        public void SetBasicFlow(Flow basicFlow)
        {
            this.BasicFlow = basicFlow;
        }

        /// <summary>
        /// Sets the specific alternative flows.
        /// </summary>
        /// <param name="specificAlternativeFlows">The specific alternative flows to set.</param>
        public void SetSpecificAlternativeFlows(List<Flow> specificAlternativeFlows)
        {
            this.SpecificAlternativeFlows = specificAlternativeFlows.AsReadOnly();
        }

        /// <summary>
        /// Sets the global alternative flows.
        /// </summary>
        /// <param name="globalAlternativeFlows">The global alternative flows to set.</param>
        public void SetGlobalAlternativeFlows(List<Flow> globalAlternativeFlows)
        {
            this.GlobalAlternativeFlows = globalAlternativeFlows.AsReadOnly();
        }

        /// <summary>
        /// Sets the bounded alternative flows.
        /// </summary>
        /// <param name="boundedAlternativeFlows">The bounded alternative flows to set.</param>
        public void SetBoundedAlternativeFlows(List<Flow> boundedAlternativeFlows)
        {
            this.BoundedAlternativeFlows = boundedAlternativeFlows.AsReadOnly();
        }
    }
}
