// <copyright file="UseCase.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
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
        /// </summary>
        public Flow BasicFlow { get; protected set; }

        /// <summary>
        /// Gets or sets the specific alternative flow list.
        /// </summary>
        public IReadOnlyList<Flow> SpecificAlternativeFlows { get; protected set; }

        /// <summary>
        /// Gets or sets the global alternative flow list.
        /// </summary>
        public IReadOnlyList<Flow> GlobalAlternativeFlows { get; protected set; }

        /// <summary>
        /// Gets or sets the bounded alternative flow list.
        /// </summary>
        public IReadOnlyList<Flow> BoundedAlternativeFlows { get; protected set; }

        /// <summary>
        /// Sets a basic flow for the use-case. It consists of a list of steps numbered by their position in the list and a postcondition as string.
        /// </summary>
        /// <param name="steps">The steps of the flow.</param>
        /// <param name="postcondition">The postcondition text of the flow.</param>
        //// public void SetBasicFlow(List<string> steps, string postcondition)
        public void SetBasicFlow(Flow basicFlow)
        {
            /*List<Node> basicSteps = new List<Node>();
            FlowIdentifier basicIdentifier = new FlowIdentifier(FlowType.Basic, 0);

            foreach (string step in steps)
            {
                basicSteps.Add(new Node(step, basicIdentifier));
            }

            this.BasicFlow = new Flow(basicIdentifier, postcondition, basicSteps, new List<ReferenceStep>());*/
            this.BasicFlow = basicFlow;
        }

        /// <summary>
        /// Adds a specific alternative flow with an unique id (unique for all specific alternative flows added to this use-case), a list of steps numbered by their position in the list,
        /// a postcondition and a reference step that describes the step from which this alternative flow may start.
        /// </summary>
        /// <param name="id">The unique id of the flow.</param>
        /// <param name="steps">The steps of the flow.</param>
        /// <param name="postcondition">The postcondition text of the flow.</param>
        /// <param name="referenceStep">The reference step of the specific flow.</param>
        //// public void AddSpecificAlternativeFlow(int id, List<string> steps, string postcondition, ReferenceStep referenceStep)
        public void AddSpecificAlternativeFlow(List<Flow> specificAlternativeFlows)
        {
            /*List<Node> specificSteps = new List<Node>();
            FlowIdentifier specificIdentifier = new FlowIdentifier(FlowType.SpecificAlternative, id);

            foreach (string step in steps)
            {
                specificSteps.Add(new Node(step, specificIdentifier));
            }

            List<Flow> tempList = new List<Flow>();
            tempList.AddRange(this.SpecificAlternativeFlows);
            tempList.Add(new Flow(specificIdentifier, postcondition, specificSteps, new List<ReferenceStep>() { referenceStep }));

            this.SpecificAlternativeFlows = tempList;*/
            this.SpecificAlternativeFlows = specificAlternativeFlows;
        }

        /// <summary>
        /// Adds a global alternative flow with an unique id (unique for all global alternative flows added to this use-case),
        /// a list of steps numbered by their position in the list and a postcondition.
        /// </summary>
        /// <param name="id">The unique id of the flow.</param>
        /// <param name="steps">The steps of the flow.</param>
        /// <param name="postcondition">The postcondition text of the flow.</param>
        //// public void AddGlobalAlternativeFlow(int id, List<string> steps, string postcondition)
        public void AddGlobalAlternativeFlow(List<Flow> globalAlternativeFlows)
        {
            /*List<Node> globalSteps = new List<Node>();
            FlowIdentifier globalIdentifier = new FlowIdentifier(FlowType.GlobalAlternative, id);

            foreach (string step in steps)
            {
                globalSteps.Add(new Node(step, globalIdentifier));
            }

            List<Flow> tempList = new List<Flow>();
            tempList.AddRange(this.GlobalAlternativeFlows);
            tempList.Add(new Flow(globalIdentifier, postcondition, globalSteps, new List<ReferenceStep>()));

            this.GlobalAlternativeFlows = tempList;*/
            this.GlobalAlternativeFlows = globalAlternativeFlows;
        }

        /// <summary>
        /// Adds a bounded alternative flow with an unique id (unique for all bounded alternative flows added to this use-case), a list of steps numbered by their position in the list,
        /// a postcondition and a list of reference steps that describe the steps from which this alternative flow may start.
        /// </summary>
        /// <param name="id">The unique id of the flow.</param>
        /// <param name="steps">The steps of the flow.</param>
        /// <param name="postcondition">The postcondition text of the flow.</param>
        /// <param name="referenceSteps">The reference steps of the bounded flow.</param>
        //// public void AddBoundedAlternativeFlow(int id, List<string> steps, string postcondition, List<ReferenceStep> referenceSteps)
        public void AddBoundedAlternativeFlow(List<Flow> boundedAlternativeFlow)
        {
            /*List<Node> boundedSteps = new List<Node>();
            FlowIdentifier boundedIdentifier = new FlowIdentifier(FlowType.BoundedAlternative, id);

            foreach (string step in steps)
            {
                boundedSteps.Add(new Node(step, boundedIdentifier));
            }

            List<Flow> tempList = new List<Flow>();
            tempList.AddRange(this.BoundedAlternativeFlows);
            tempList.Add(new Flow(boundedIdentifier, postcondition, boundedSteps, referenceSteps));

            this.BoundedAlternativeFlows = tempList;*/
            this.BoundedAlternativeFlows = boundedAlternativeFlow;
        }

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

            GraphBuilder.BuildGraph(
                ref basicFlow,
                this.SpecificAlternativeFlows,
                this.GlobalAlternativeFlows,
                this.BoundedAlternativeFlows,
                out nodes,
                out edgeMatrix,
                out conditionMatrix);

            this.BasicFlow = basicFlow;
            this.Nodes = nodes;
            this.EdgeMatrix = edgeMatrix;
            this.ConditionMatrix = conditionMatrix;
        }

        /// <summary>
        /// Prints the edges of the matrix into the console, that are equal to the match object.
        /// Output: {row} -> {column}
        /// </summary>
        /// <typeparam name="T">The matrix contents type.</typeparam>
        /// <param name="matrix">The matrix.</param>
        /// <param name="matchObject">The object whose edges from the matrix will be printed.</param>
        public void PrintEdges<T>(Matrix<T> matrix, T matchObject)
        {
            for (int row = 0; row < matrix.RowCount; row++)
            {
                for (int column = 0; column < matrix.ColumnCount; column++)
                {
                    if (matrix[row, column].Equals(matchObject))
                    {
                        System.Console.WriteLine($"{row} -> {column}");
                    }
                }
            }
        }

        /// <summary>
        /// Prints the edges of the matrix into the console, that are not equal to the match object.
        /// Output: {row} -> {column}
        /// </summary>
        /// <typeparam name="T">The matrix contents type.</typeparam>
        /// <param name="matrix">The matrix.</param>
        /// <param name="matchObject">The object whose edges from the matrix will not be printed.</param>
        public void PrintEdgesInverse<T>(Matrix<T> matrix, T matchObject)
        {
            for (int row = 0; row < matrix.RowCount; row++)
            {
                for (int column = 0; column < matrix.ColumnCount; column++)
                {
                    if (!matrix[row, column].Equals(matchObject))
                    {
                        System.Console.WriteLine($"{row} -> {column}");
                    }
                }
            }
        }
    }
}
