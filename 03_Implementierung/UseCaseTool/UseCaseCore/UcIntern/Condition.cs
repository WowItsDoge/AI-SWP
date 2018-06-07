// <copyright file="Condition.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    /// <summary>
    /// A condition describes the condition which must be matched for a edge in the graph to be valid. For most edges there is no condition meaning they are always valid (if the exists).
    /// But at every point where from a step more than one edge leaves they need a condition to determine which
    /// edge is the one to follow.For that the condition is formulated with a string that can either be true or false.
    /// The condition is valid if the answer to the formulated condition equates to the condition state.
    /// This means that for every condition string there are two condition objects.
    /// One describing the edge that is valid if the condition is true and one to describe if the condition is false.
    /// Sadly there can be multiple valid conditions for the edges leaving a node.Then the right edge must be determined in the context.
    /// </summary>
    public struct Condition
    {
        /// <summary>
        /// Gets the condition text.
        /// </summary>
        public readonly string ConditionText;

        /// <summary>
        /// Gets a value indicating whether the condition text is true.
        /// </summary>
        public readonly bool ConditionState;

        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> struct.
        /// </summary>
        /// <param name="conditionText">The condition text.</param>
        /// <param name="conditionState">The condition state.</param>
        public Condition(string conditionText, bool conditionState)
        {
            this.ConditionText = conditionText;
            this.ConditionState = conditionState;
        }

        /// <summary>
        /// Tests if <paramref name="x"/> is equal to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to be compared.</param>
        /// <param name="y">The second object to be compared.</param>
        /// <returns>If <paramref name="x"/> is equal to <paramref name="y"/>.</returns>
        public static bool operator ==(Condition x, Condition y)
        {
            return x.ConditionText == y.ConditionText
                && x.ConditionState == y.ConditionState;
        }

        /// <summary>
        /// Tests if <paramref name="x"/> is not equal to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to be compared.</param>
        /// <param name="y">The second object to be compared.</param>
        /// <returns>If <paramref name="x"/> is not equal to <paramref name="y"/>.</returns>
        public static bool operator !=(Condition x, Condition y)
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
            return obj is Condition && this == (Condition)obj;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return BitShifter.ShiftAndWrap(this.ConditionText?.GetHashCode() ?? 0, 1)
                ^ this.ConditionState.GetHashCode();
        }
    }
}
