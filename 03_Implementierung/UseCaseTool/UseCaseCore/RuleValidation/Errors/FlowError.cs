// <copyright file="FlowError.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.Errors
{
    /// <summary>
    /// The error class for errors that occurred in a flow.
    /// </summary>
    public class FlowError : IError
    {
        /// <summary>
        /// The number of the flow that the error belongs to.
        /// </summary>
        private int flowReferenceNumber;

        /// <summary>
        /// A message containing some information about how to fix the error.
        /// </summary>
        private string resolveMessage;

        /// <summary>
        /// The reason for this error.
        /// </summary>
        private string errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlowError"/> class.
        /// </summary>
        /// <param name="flowReferenceNumber">The number of the flow that the error belongs to.</param>
        /// <param name="resolveMessage">A message containing some information about how to fix the error.</param>
        /// <param name="errorMessage">The reason for this error.</param>
        public FlowError(int flowReferenceNumber, string resolveMessage, string errorMessage)
        {
            this.flowReferenceNumber = flowReferenceNumber;
            this.resolveMessage = resolveMessage;
            this.errorMessage = errorMessage;
        }

        /// <summary>
        /// Produces a string, continuing all the information of the error.
        /// </summary>
        /// <returns>A string with the error message.</returns>
        public string GetErrorString()
        {
            return string.Empty;
        }
    }
}
