// <copyright file="Flow.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    using System.Collections.Generic;

    /// <summary>
    /// An object representing a flow with its unique identifier and its postcondition as a string.
    /// </summary>
    public class Flow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flow"/> class. 
        /// </summary>
        /// <param name="identifier">The identifier of the flow.</param>
        /// <param name="postcondition">The postcondition of the flow.</param>
        /// <param name="nodes">The nodes of the flow</param>
        /// <param name="referenceSteps">The reference steps of the flow.</param>
        public Flow(FlowIdentifier identifier, string postcondition, List<Node> nodes, List<ReferenceStep> referenceSteps)
        {
            this.Identifier = identifier;
            this.Postcondition = postcondition;
            this.Nodes = nodes.AsReadOnly();
            this.ReferenceSteps = referenceSteps.AsReadOnly();
        }

        /// <summary>
        /// Gets the identifier of the flow.
        /// </summary>
        public FlowIdentifier Identifier { get; }

        /// <summary>
        /// Gets the postcondition of the flow.
        /// </summary>
        public string Postcondition { get; }

        /// <summary>
        /// Gets the nodes of the flow.
        /// </summary>
        public IList<Node> Nodes { get; }

        /// <summary>
        /// Gets the reference steps of the flow.
        /// </summary>
        public IList<ReferenceStep> ReferenceSteps { get; }
    }
}
