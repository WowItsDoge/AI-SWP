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
    }
}
