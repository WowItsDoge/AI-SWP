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
        /// Returns a string representing this instance
        /// </summary>
        /// <returns>this instance as a string</returns>
        public override string ToString()
        { 
            return this.errorMessage + "\n";
        }
    }
}
