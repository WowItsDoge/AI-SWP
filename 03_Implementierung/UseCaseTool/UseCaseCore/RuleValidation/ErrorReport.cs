// <copyright file="ErrorReport.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation
{
    using System.Collections.Generic;
    using Errors;

    /// <summary>
    /// The class that contains the information of the errorReport
    /// </summary>
    public class ErrorReport
    {
        /// <summary>
        /// The error list containing all errors.
        /// </summary>
        private List<IError> errorList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorReport"/> class.
        /// </summary>
        public ErrorReport()
        {
            this.errorList = new List<IError>();
        }

        /// <summary>
        /// Gets the error list.
        /// </summary>
        public List<IError> GetErrorList
        {
            get
            {
                return this.errorList;
            }
        }

        /// <summary>
        /// Adds the specified error to the error report.
        /// </summary>
        /// <param name="errorToAdd">An error object containing the information about the error.</param>
        public void AddError(IError errorToAdd)
        {
            this.errorList.Add(errorToAdd);
        }

        /// <summary>
        /// Creates a file containing all errors and exports it.
        /// </summary>
        /// <param name="path">The path where to export the file.</param>
        /// <returns>True if exportation was successfully, otherwise false.</returns>
        public bool Export(string path)
        {
            return false;
        }
    }
}
