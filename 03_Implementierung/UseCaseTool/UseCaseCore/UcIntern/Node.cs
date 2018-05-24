// <copyright file="Node.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    /// <summary>
    /// A node describes a flow with a description (the text of the step) and a flow identifier telling the flow that the node is assigned to.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="stepDescription">The description text of the step.</param>
        /// <param name="identifier">The flow the node belongs to.</param>
        public Node(string stepDescription, FlowIdentifier identifier)
        {
            this.StepDescription = stepDescription;
            this.Identifier = identifier;
        }

        /// <summary>
        /// Gets the description text of the step.
        /// </summary>
        public string StepDescription { get; }

        /// <summary>
        /// Gets the identifier of the flow the node belongs to.
        /// </summary>
        public FlowIdentifier Identifier { get; }
    }
}
