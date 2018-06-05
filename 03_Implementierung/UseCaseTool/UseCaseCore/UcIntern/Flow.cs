// <copyright file="Flow.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    using System.Collections.Generic;

    /// <summary>
    /// An object representing a flow with its unique identifier and its postcondition as a string.
    /// </summary>
    public struct Flow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flow"/> struct. 
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
                && ReferenceEquals(x.Nodes, y.Nodes)
                && ReferenceEquals(x.ReferenceSteps, y.ReferenceSteps);
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
                ^ BitShifter.ShiftAndWrap(this.Postcondition.GetHashCode(), 2)
                ^ BitShifter.ShiftAndWrap(this.Nodes.GetHashCode(), 1)
                ^ this.ReferenceSteps.GetHashCode();
        }
    }
}
