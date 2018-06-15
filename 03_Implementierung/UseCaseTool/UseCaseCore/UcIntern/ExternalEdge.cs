// <copyright file="ExternalEdge.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    /// <summary>
    /// An instance of this class represents an edge between two nodes.
    /// <para/>
    /// This class is used by the <see cref="GraphBuilder"/>  to specify edges between two steps.
    /// Because the <see cref="GraphBuilder"/> creates the edges by analyzing small blocks of nodes it can happen that an edge must be created that points to a
    /// node that is not in the block that is analyzed. To specify such an edge this class is used. It holds the step number for the current block of nodes and the
    /// reference step to the step where it points to. Remember to increase the step number if the analyzed block was contained in a larger block of nodes.
    /// <para/>
    /// The edge always points from the source to the target.
    /// </summary>
    public struct ExternalEdge
    {
        /// <summary>
        /// The number of the step in the current block.
        /// </summary>
        public readonly int SourceStepNumber;

        /// <summary>
        /// The target of this edge.
        /// </summary>
        public readonly ReferenceStep TargetStep;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalEdge"/> struct.
        /// </summary>
        /// <param name="sourceStepNumber">The number of the source step.</param>
        /// <param name="targetStep">The target step.</param>
        public ExternalEdge(int sourceStepNumber, ReferenceStep targetStep)
        {
            this.SourceStepNumber = sourceStepNumber;
            this.TargetStep = targetStep;
        }

        /// <summary>
        /// Tests if <paramref name="x"/> is equal to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to be compared.</param>
        /// <param name="y">The second object to be compared.</param>
        /// <returns>If <paramref name="x"/> is equal to <paramref name="y"/>.</returns>
        public static bool operator ==(ExternalEdge x, ExternalEdge y)
        {
            return x.SourceStepNumber == y.SourceStepNumber
                && x.TargetStep == y.TargetStep;
        }

        /// <summary>
        /// Tests if <paramref name="x"/> is not equal to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to be compared.</param>
        /// <param name="y">The second object to be compared.</param>
        /// <returns>If <paramref name="x"/> is not equal to <paramref name="y"/>.</returns>
        public static bool operator !=(ExternalEdge x, ExternalEdge y)
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
            return obj is ExternalEdge && this == (ExternalEdge)obj;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return BitShifter.ShiftAndWrap(this.SourceStepNumber.GetHashCode(), 1)
                ^ this.TargetStep.GetHashCode();
        }

        /// <summary>
        /// Returns a new <see cref="ExternalEdge"/> instance with the source step number of the current one increased by <paramref name="incrementValue"/>.
        /// </summary>
        /// <param name="incrementValue">The value to add to the current source step number value.</param>
        /// <returns>A new <see cref="ExternalEdge"/> with the source step number of this instance increased by <paramref name="incrementValue"/>.</returns>
        public ExternalEdge NewWithIncreasedSourceStepNumber(int incrementValue)
        {
            return new ExternalEdge(this.SourceStepNumber + incrementValue, this.TargetStep);
        }
    }
}
