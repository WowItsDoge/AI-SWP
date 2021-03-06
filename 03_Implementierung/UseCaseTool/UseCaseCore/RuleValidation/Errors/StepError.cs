﻿// <copyright file="StepError.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.Errors
{
    /// <summary>
    /// The error class for errors that occurred in a step.
    /// </summary>
    public class StepError : IError
    {
        /// <summary>
        /// The number of the step that the error belongs to.
        /// </summary>
        private int stepReferenceNumber;

        /// <summary>
        /// A message containing some information about how to fix the error.
        /// </summary>
        private string resolveMessage;

        /// <summary>
        /// The reason for this error.
        /// </summary>
        private string errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="StepError"/> class.
        /// </summary>
        /// <param name="stepReferenceNumber">The number of the step that the error belongs to.</param>
        /// <param name="resolveMessage">A message containing some information about how to fix the error.</param>
        /// <param name="errorMessage">The reason for this error.</param>
        public StepError(int stepReferenceNumber, string resolveMessage, string errorMessage)
        {
            this.stepReferenceNumber = stepReferenceNumber;
            this.resolveMessage = resolveMessage;
            this.errorMessage = errorMessage;
        }

        /// <summary>
        /// Returns a string representing this instance
        /// </summary>
        /// <returns>this instance as a string</returns>
        public override string ToString()
        {
            return "Fehler in Step " + this.stepReferenceNumber + ": " + this.errorMessage + "\t" + this.resolveMessage + "\r\n";
        }
    }
}
