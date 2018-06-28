// <copyright file="FlowType.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    /// <summary>
    /// Specifies the type of a flow.
    /// </summary>
    public enum FlowType
    {
        /// <summary>
        /// The basic flow.
        /// </summary>
        Basic,

        /// <summary>
        /// The specific alternative flow.
        /// </summary>
        SpecificAlternative,

        /// <summary>
        /// The global alternative flow.
        /// </summary>
        GlobalAlternative,

        /// <summary>
        /// The bounded alternative flow.
        /// </summary>
        BoundedAlternative
    }
}
