// <copyright file="FlowIdentifier.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    /// <summary>
    /// A flow identifier identifies one flow with a type and a number. As there is only one basic flow the number should not be accounted for the basic flow type.
    /// </summary>
    public class FlowIdentifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlowIdentifier"/> class. 
        /// </summary>
        /// <param name="type">The flow type.</param>
        /// <param name="id">The flow id.</param>
        public FlowIdentifier(FlowType type, int id)
        {
            this.Type = type;
            this.Id = id;
        }

        /// <summary>
        /// Gets the flow type.
        /// </summary>
        public FlowType Type { get; }

        /// <summary>
        /// Gets the flow id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Tests if <paramref name="x"/> is equal to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to be compared.</param>
        /// <param name="y">The second object to be compared.</param>
        /// <returns>If <paramref name="x"/> is equal to <paramref name="y"/>.</returns>
        public static bool operator ==(FlowIdentifier x, FlowIdentifier y)
        {
            return x.Type == y.Type
                && x.Id == y.Id;
        }

        /// <summary>
        /// Tests if <paramref name="x"/> is not equal to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to be compared.</param>
        /// <param name="y">The second object to be compared.</param>
        /// <returns>If <paramref name="x"/> is not equal to <paramref name="y"/>.</returns>
        public static bool operator !=(FlowIdentifier x, FlowIdentifier y)
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
            return obj is FlowIdentifier && this == (FlowIdentifier)obj;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return BitShifter.ShiftAndWrap(this.Type.GetHashCode(), 1)
                ^ this.Id.GetHashCode();
        }
    }
}
