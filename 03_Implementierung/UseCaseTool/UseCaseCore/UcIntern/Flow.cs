// <copyright file="Flow.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An object representing a flow with its unique identifier and its postcondition as a string.
    /// </summary>
    public struct Flow
    {
        /// <summary>
        /// Gets the identifier of the flow.
        /// </summary>
        public readonly FlowIdentifier Identifier;

        /// <summary>
        /// Gets the postcondition of the flow.
        /// </summary>
        public readonly string Postcondition;

        /// <summary>
        /// Gets the nodes of the flow.
        /// </summary>
        public readonly IReadOnlyList<Node> Nodes;

        /// <summary>
        /// Gets the reference steps of the flow.
        /// </summary>
        public readonly IReadOnlyList<ReferenceStep> ReferenceSteps;

        /// <summary>
        /// Initializes a new instance of the <see cref="Flow"/> struct. 
        /// </summary>
        /// <param name="identifier">The identifier of the flow.</param>
        /// <param name="postcondition">The postcondition of the flow.</param>
        /// <param name="nodes">The nodes of the flow</param>
        /// <param name="referenceSteps">The reference steps of the flow.</param>
        public Flow(FlowIdentifier identifier, string postcondition, IReadOnlyList<Node> nodes, IReadOnlyList<ReferenceStep> referenceSteps)
        {
            this.Identifier = identifier;
            this.Postcondition = postcondition;
            this.Nodes = nodes;
            this.ReferenceSteps = referenceSteps;
        }

        /// <summary>
        /// Tests if <paramref name="x"/> is equal to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to be compared.</param>
        /// <param name="y">The second object to be compared.</param>
        /// <returns>If <paramref name="x"/> is equal to <paramref name="y"/>.</returns>
        public static bool operator ==(Flow x, Flow y)
        {
            return x.Identifier == y.Identifier
                && x.Postcondition == y.Postcondition
                && ((x.Nodes == null && y.Nodes == null) || x.Nodes.SequenceEqual(y.Nodes))
                && ((x.ReferenceSteps == null && y.ReferenceSteps == null) || x.ReferenceSteps.SequenceEqual(y.ReferenceSteps));
        }

        /// <summary>
        /// Tests if <paramref name="x"/> is not equal to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to be compared.</param>
        /// <param name="y">The second object to be compared.</param>
        /// <returns>If <paramref name="x"/> is not equal to <paramref name="y"/>.</returns>
        public static bool operator !=(Flow x, Flow y)
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
            return obj is Flow && this == (Flow)obj;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return BitShifter.ShiftAndWrap(this.Identifier.GetHashCode(), 3)
                ^ BitShifter.ShiftAndWrap(this.Postcondition?.GetHashCode() ?? 0, 2)
                ^ BitShifter.ShiftAndWrap(this.Nodes?.GetHashCode() ?? 0, 1)
                ^ this.ReferenceSteps?.GetHashCode() ?? 0;
        }
    }
}
