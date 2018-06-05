// <copyright file="Node.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    /// <summary>
    /// A node describes a flow with a description (the text of the step) and a flow identifier telling the flow that the node is assigned to.
    /// </summary>
    public struct Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> struct.
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

        /// <summary>
        /// Tests if <paramref name="x"/> is equal to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to be compared.</param>
        /// <param name="y">The second object to be compared.</param>
        /// <returns>If <paramref name="x"/> is equal to <paramref name="y"/>.</returns>
        public static bool operator ==(Node x, Node y)
        {
            return x.StepDescription == y.StepDescription
                && x.Identifier == y.Identifier;
        }

        /// <summary>
        /// Tests if <paramref name="x"/> is not equal to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to be compared.</param>
        /// <param name="y">The second object to be compared.</param>
        /// <returns>If <paramref name="x"/> is not equal to <paramref name="y"/>.</returns>
        public static bool operator !=(Node x, Node y)
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
            return obj is Node && this == (Node)obj;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return BitShifter.ShiftAndWrap(this.StepDescription.GetHashCode(), 1)
                ^ this.Identifier.GetHashCode();
        }
    }
}
