// <copyright file="IError.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.Errors
{
    /// <summary>
    /// The interface for all possible errors.
    /// </summary>
    public interface IError
    {
        /// <summary>
        /// Produces a string, continuing all the information of the error.
        /// </summary>
        /// <returns>A string with the error message.</returns>
        string GetErrorString();
    }
}
