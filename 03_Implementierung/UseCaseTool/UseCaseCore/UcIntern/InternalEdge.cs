// <copyright file="InternalEdge.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    /// <summary>
    /// An instance of this class represents an edge between two nodes.
    /// <para/>
    /// This class is used by the <see cref="GraphBuilder"/>  to specify edges between two steps.
    /// This class is used to identify an edge between two nodes from the same node list using their index.
    /// <para/>
    /// The edge always points from the source to the target.
    /// </summary>
    public struct InternalEdge
    {
        /// <summary>
        /// The source node.
        /// </summary>
        public readonly int SourceStep;

        /// <summary>
        /// The target node.
        /// </summary>
        public readonly int TargetStep;

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalEdge"/> struct.
        /// </summary>
        /// <param name="sourceStep">The number of the source step.</param>
        /// <param name="targetStep">The number of the target step.</param>
        public InternalEdge(int sourceStep, int targetStep)
        {
            this.SourceStep = sourceStep;
            this.TargetStep = targetStep;
        }

        /// <summary>
        /// Tests if <paramref name="x"/> is equal to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to be compared.</param>
        /// <param name="y">The second object to be compared.</param>
        /// <returns>If <paramref name="x"/> is equal to <paramref name="y"/>.</returns>
        public static bool operator ==(InternalEdge x, InternalEdge y)
        {
            return x.SourceStep == y.SourceStep
                && x.TargetStep == y.TargetStep;
        }

        /// <summary>
        /// Tests if <paramref name="x"/> is not equal to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to be compared.</param>
        /// <param name="y">The second object to be compared.</param>
        /// <returns>If <paramref name="x"/> is not equal to <paramref name="y"/>.</returns>
        public static bool operator !=(InternalEdge x, InternalEdge y)
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
            return obj is InternalEdge && this == (InternalEdge)obj;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return BitShifter.ShiftAndWrap(this.SourceStep.GetHashCode(), 3)
                ^ this.TargetStep.GetHashCode();
        }

        /// <summary>
        /// Returns a new <see cref="InternalEdge"/> instance with the source and target step of the current one increased by <paramref name="incrementValue"/>.
        /// </summary>
        /// <param name="incrementValue">The value to add to the current source and target step value.</param>
        /// <returns>A new <see cref="InternalEdge"/> with the source and target step of this instance increased by <paramref name="incrementValue"/>.</returns>
        public InternalEdge NewWithIncreasedSourceTargetStep(int incrementValue)
        {
            return new InternalEdge(this.SourceStep + incrementValue, this.TargetStep + incrementValue);
        }
    }
}
