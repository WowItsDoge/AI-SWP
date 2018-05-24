// <copyright file="ReferenceStep.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    /// <summary>
    /// A reference step describes one specific step across all flows and therefor has a flow identifier and a step number.
    /// </summary>
    public class ReferenceStep
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceStep"/> class.
        /// </summary>
        /// <param name="identifier">The identifier of the flow the reference step points to.</param>
        /// <param name="step">The step number of the flow that is referenced.</param>
        public ReferenceStep(FlowIdentifier identifier, int step)
        {
            this.Identifier = identifier;
            this.Step = step;
        }

        /// <summary>
        /// Gets the identifier of the flow the reference step points to.
        /// </summary>
        public FlowIdentifier Identifier { get; }

        /// <summary>
        /// Gets the step number of the flow that is referenced.
        /// </summary>
        public int Step { get; }
    }
}
