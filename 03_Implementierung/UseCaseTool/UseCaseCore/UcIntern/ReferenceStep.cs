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
        /// Gets the identifier of the flow the reference step points to.
        /// </summary>
        public readonly FlowIdentifier Identifier;

        /// <summary>
        /// Gets the step number of the flow that is referenced.
        /// </summary>
        public readonly int Step;

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
        /// Tests if <paramref name="x"/> is equal to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to be compared.</param>
        /// <param name="y">The second object to be compared.</param>
        /// <returns>If <paramref name="x"/> is equal to <paramref name="y"/>.</returns>
        public static bool operator ==(ReferenceStep x, ReferenceStep y)
        {
            return x.Identifier == y.Identifier
                && x.Step == y.Step;
        }

        /// <summary>
        /// Tests if <paramref name="x"/> is not equal to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to be compared.</param>
        /// <param name="y">The second object to be compared.</param>
        /// <returns>If <paramref name="x"/> is not equal to <paramref name="y"/>.</returns>
        public static bool operator !=(ReferenceStep x, ReferenceStep y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is ReferenceStep && this == (ReferenceStep)obj;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return BitShifter.ShiftAndWrap(this.Identifier.GetHashCode(), 1)
                ^ this.Step.GetHashCode();
        }
    }
}
