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
            this.Nodes = new List<Node>();

            List<Node> basicNodes = new List<Node>();
            FlowIdentifier basicId = new FlowIdentifier(FlowType.Basic, 0);
            basicNodes.Add(new Node("Step 1", basicId));
            basicNodes.Add(new Node("Step 2", basicId));
            basicNodes.Add(new Node("Step 3", basicId));
            foreach (Node node in basicNodes)
            {
                this.Nodes.Add(node);
            }

            this.BasicFlow = new Flow(basicId, "Postcondition Basic", basicNodes, new List<ReferenceStep>());

            List<Node> specificNodes = new List<Node>();
            FlowIdentifier specificId = new FlowIdentifier(FlowType.Basic, 0);
            specificNodes.Add(new Node("Specific Step 1", specificId));
            foreach (Node node in specificNodes)
            {
                this.Nodes.Add(node);
            }

            this.SpecificAlternativeFlows = new List<Flow>();
            this.SpecificAlternativeFlows.Add(new Flow(specificId, "Postcondition Specific", specificNodes, new List<ReferenceStep>() { new ReferenceStep(basicId, 1) }));

            this.GlobalAlternativeFlows = new List<Flow>();
            this.BoundedAlternativeFlows = new List<Flow>();

            this.EdgeMatrix = new Matrix<bool>(4, false);
            this.EdgeMatrix[0, 1] = true; // 0 -> 1
            this.EdgeMatrix[1, 2] = true; // 1 -> 2
            this.EdgeMatrix[0, 3] = true; // 0 -> Specific 0
            this.EdgeMatrix[3, 2] = true; // Specific 0 -> 2

            this.ConditionMatrix = new Matrix<Condition>(4, null);
            this.ConditionMatrix[0, 3] = new Condition("Wahr", true);
            this.ConditionMatrix[0, 1] = new Condition("Wahr", false);
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
        /// <para/>
        /// Do not set the nodes outside of the module UC-Intern!
        /// </summary>
        public List<Node> Nodes { get; set; }

        /// <summary>
        /// Gets or sets an edge matrix that describes the edges between all nodes of the graph. All edges are always directional.
        /// The row or column number of an entry corresponds to the node with the same number in the node list.
        /// An edge always points from the row node to the column node.This matrix only describes if an edge exists and its direction.
        /// <para/>
        /// Do not set the nodes outside of the module UC-Intern.
        /// </summary>
        public Matrix<bool> EdgeMatrix { get; set; }

        /// <summary>
        /// Gets or sets an edge matrix that describes the edges between all nodes of the graph which have a condition.
        /// It is build like the normal edge matrix but only has an entry for an edge if that edge has a condition that must be matched for the edge to become valid.
        /// For an edge to become valid all conditions(if the list consists of more than one) must be matched!
        /// All other entries are ""null"". ""null"" does not mean that no edge exists there but only that no edge with a condition exists there.
        /// <para/>
        /// Do not set the condition matrix outside of the module UC-Intern.
        /// </summary>
        public Matrix<Condition> ConditionMatrix { get; set; }

        /// <summary>
        /// Gets or sets the basic flow.
        /// <para/>
        /// Do not set the basic flow outside of the module UC-Intern.
        /// </summary>
        public Flow BasicFlow { get; set; }

        /// <summary>
        /// Gets or sets the specific alternative flow list.
        /// <para/>
        /// Do not set or alter the specific alternative flow list outside the module UC-Intern.
        /// </summary>
        public List<Flow> SpecificAlternativeFlows { get; set; }

        /// <summary>
        /// Gets or sets the global alternative flow list.
        /// <para/>
        /// Do not set or alter the global alternative flow list outside the module UC-Intern.
        /// </summary>
        public List<Flow> GlobalAlternativeFlows { get; set; }

        /// <summary>
        /// Gets or sets the bounded alternative flow list.
        /// <para/>
        /// Do not set or alter the bounded alternative flow list outside the module UC-Intern.
        /// </summary>
        public List<Flow> BoundedAlternativeFlows { get; set; }

        /// <summary>
        /// Sets a basic flow for the use-case. It consists of a list of steps numbered by their position in the list and a postcondition as string.
        /// </summary>
        /// <param name="steps">The steps of the flow.</param>
        /// <param name="postcondition">The postcondition text of the flow.</param>
        public void SetBasicFlow(List<string> steps, string postcondition)
        {
        }

        /// <summary>
        /// Adds a specific alternative flow with an unique id (unique for all specific alternative flows added to this use-case), a list of steps numbered by their position in the list,
        /// a postcondition and a reference step that describes the step from which this alternative flow may start.
        /// </summary>
        /// <param name="id">The unique id of the flow.</param>
        /// <param name="steps">The steps of the flow.</param>
        /// <param name="postcondition">The postcondition text of the flow.</param>
        /// <param name="referenceStep">The reference step of the specific flow.</param>
        public void AddSpecificAlternativeFlow(int id, List<string> steps, string postcondition, ReferenceStep referenceStep)
        {
        }

        /// <summary>
        /// Adds a global alternative flow with an unique id (unique for all global alternative flows added to this use-case),
        /// a list of steps numbered by their position in the list and a postcondition.
        /// </summary>
        /// <param name="id">The unique id of the flow.</param>
        /// <param name="steps">The steps of the flow.</param>
        /// <param name="postcondition">The postcondition text of the flow.</param>
        public void AddGlobalAlternativeFlow(int id, List<string> steps, string postcondition)
        {
        }

        /// <summary>
        /// Adds a bounded alternative flow with an unique id (unique for all bounded alternative flows added to this use-case), a list of steps numbered by their position in the list,
        /// a postcondition and a list of reference steps that describe the steps from which this alternative flow may start.
        /// </summary>
        /// <param name="id">The unique id of the flow.</param>
        /// <param name="steps">The steps of the flow.</param>
        /// <param name="postcondition">The postcondition text of the flow.</param>
        /// <param name="referenceSteps">The reference steps of the bounded flow.</param>
        public void AddBoundedAlternativeFlow(int id, List<string> steps, string postcondition, List<ReferenceStep> referenceSteps)
        {
        }

        /// <summary>
        /// Signals the end of the description of the use-case and builds the internal graph-representation.
        /// Before calling BuildGraph only the setters and methods starting with ""Set"" and ""Add"" are allowed to be called.
        /// After using these methods to describe the use-case, BuildGraph is called and the given description is used to build a graph representation.
        /// After calling BuildGraph only the getters are allowed to be called.
        /// </summary>
        public void BuildGraph()
        {
        }
    }
}
