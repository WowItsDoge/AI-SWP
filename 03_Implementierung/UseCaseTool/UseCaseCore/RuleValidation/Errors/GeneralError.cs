// <copyright file="GeneralError.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.Errors
{
    /// <summary>
    /// The error class for general errors that occurred.
    /// </summary>
    public class GeneralError : IError
    {
        /// <summary>
        /// The reason for this error.
        /// </summary>
        private string errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralError"/> class.
        /// </summary>
        /// <param name="errorMessage">The reason for this error.</param>
        public GeneralError(string errorMessage)
        {
            this.errorMessage = errorMessage;
        }

        /// <summary>
        /// Produces a string, continuing all the information of the error.
        /// </summary>
        /// <returns>A string with the error message.</returns>
        public string GetErrorString()
        {
            return this.errorMessage + "\n";
        }
    }
}
