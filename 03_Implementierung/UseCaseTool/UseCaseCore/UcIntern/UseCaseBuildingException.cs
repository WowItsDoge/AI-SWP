// <copyright file="UseCaseBuildingException.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    using System;

    /// <summary>
    /// An exception that wraps an exception that occurred in the <see cref="GraphBuilder"/>. 
    /// </summary>
    public class UseCaseBuildingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UseCaseBuildingException"/> class.
        /// </summary>
        /// <param name="innerException">The exception that occurred while building the graph.</param>
        public UseCaseBuildingException(Exception innerException) : base("An internal exception occurred while building the graph.", innerException)
        {
        }
    }
}
