// <copyright file="UseCase.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This class is used to create and retrieve the representation of the use-case as a graph. It also provides all the information contained in a use-case.
    /// The usage of the class involves two steps:
    /// <para/>
    /// The first step is to build the graph. For that the setter methods and the methods starting with ""Set"" and ""Add"" are used to insert the description of a graph.
    /// For that all ""Set"" methods must be used and all setters and ""Add"" methods can be used to describe a use-case.
    /// The setters (in this case setters are all methods starting with ""set_"" are not mandatory because they are simple string setters and therefor return null if no value was set.
    /// After inserting the whole description the method BuildGraph is called. After that no usage of setters, ""Set"" and ""Add"" methods is allowed anymore.
    /// <para/>
    /// The second step involves retrieval of the data. For that various getters are provided with most being simple string getters.
    /// The remaining getters are used to retrieve the graph in a representation with a node list and two edge matrices.
    /// </summary>
    public class UseCase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UseCase"/> class.
        /// For interaction with other modules a sample graph is returned.
        /// </summary>
        public UseCase()
        {
            this.SpecificAlternativeFlows = new List<Flow>();
            this.GlobalAlternativeFlows = new List<Flow>();
            this.BoundedAlternativeFlows = new List<Flow>();
        }

        /// <summary>
        /// Gets or sets the name of the use case.
        /// </summary>
        public string UseCaseName { get; set; }

        /// <summary>
        /// Gets or sets the brief description of the use case.
        /// </summary>
        public string BriefDescription { get; set; }

        /// <summary>
        /// Gets or sets the precondition text of the use case.
        /// </summary>
        public string Precondition { get; set; }

        /// <summary>
        /// Gets or sets the primary actor text of the use case.
        /// </summary>
        public string PrimaryActor { get; set; }
        
        /// <summary>
        /// Gets or sets the secondary actors text of the use case.
        /// </summary>
        public string SecondaryActors { get; set; }

        /// <summary>
        /// Gets or sets the dependency text of the use case.
        /// </summary>
        public string Dependency { get; set; }

        /// <summary>
        /// Gets or sets the generalization text of the use case.
        /// </summary>
        public string Generalization { get; set; }

        /// <summary>
        /// Gets or sets all nodes of the graph.
        /// </summary>
        public IReadOnlyList<Node> Nodes { get; protected set; }

        /// <summary>
        /// Gets or sets an edge matrix that describes the edges between all nodes of the graph. All edges are always directional.
        /// The row or column number of an entry corresponds to the node with the same number in the node list.
        /// An edge always points from the row node to the column node.This matrix only describes if an edge exists and its direction.
        /// </summary>
        public Matrix<bool> EdgeMatrix { get; protected set; }

        /// <summary>
        /// Gets or sets an edge matrix that describes the edges between all nodes of the graph which have a condition.
        /// It is build like the normal edge matrix but only has an entry for an edge if that edge has a condition that must be matched for the edge to become valid.
        /// For an edge to become valid all conditions(if the list consists of more than one) must be matched!
        /// All other entries are ""null"". ""null"" does not mean that no edge exists there but only that no edge with a condition exists there.
        /// </summary>
        public Matrix<Condition?> ConditionMatrix { get; protected set; }

        /// <summary>
        /// Gets or sets the basic flow.
        /// It can be altered by the graph creation!
        /// </summary>
        public Flow BasicFlow { get; set; }

        /// <summary>
        /// Gets or sets the specific alternative flow list.
        /// </summary>
        public IReadOnlyList<Flow> SpecificAlternativeFlows { get; set; }

        /// <summary>
        /// Gets or sets the global alternative flow list.
        /// </summary>
        public IReadOnlyList<Flow> GlobalAlternativeFlows { get; set; }

        /// <summary>
        /// Gets or sets the bounded alternative flow list.
        /// </summary>
        public IReadOnlyList<Flow> BoundedAlternativeFlows { get; set; }

        /// <summary>
        /// Signals the end of the description of the use-case and builds the internal graph-representation.
        /// Before calling BuildGraph only the setters and methods starting with ""Set"" and ""Add"" are allowed to be called.
        /// After using these methods to describe the use-case, BuildGraph is called and the given description is used to build a graph representation.
        /// After calling BuildGraph only the getters are allowed to be called.
        /// </summary>
        public void BuildGraph()
        {
            Flow basicFlow = this.BasicFlow;
            List<Node> nodes;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            try
            {
                GraphBuilder.BuildGraph(
                ref basicFlow,
                this.SpecificAlternativeFlows,
                this.GlobalAlternativeFlows,
                this.BoundedAlternativeFlows,
                out nodes,
                out edgeMatrix,
                out conditionMatrix);
            }
            catch (Exception e)
            {
                throw new UseCaseBuildingException(e);
            }

            this.BasicFlow = basicFlow;
            this.Nodes = nodes.AsReadOnly();
            this.EdgeMatrix = edgeMatrix.AsReadonly();
            this.ConditionMatrix = conditionMatrix.AsReadonly();
        }
    }
}
