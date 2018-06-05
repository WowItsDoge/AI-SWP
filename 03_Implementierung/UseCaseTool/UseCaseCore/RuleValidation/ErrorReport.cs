// <copyright file="ErrorReport.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Errors;

    /// <summary>
    /// The class that contains the information of the errorReport
    /// </summary>
    public class ErrorReport
    {
        /// <summary>
        /// The constant string at the beginning of every exported file.
        /// </summary>
        public const string ExportHeader = "Mängelbericht: \n";

        /// <summary>
        /// The constant string for an exported file without any errors.
        /// </summary>
        public const string EmptyErrorExportMessage = "Es wurden keine Verstöße festgestellt!\n";

        /// <summary>
        /// The header before all general errors.
        /// </summary>
        public const string GeneralErrorHeader = "Generelle Fehler: \n";

        /// <summary>
        /// The header before all flow specific errors.
        /// </summary>
        public const string FlowErrorHeader = "Flow-bezogene Fehler: \n";

        /// <summary>
        /// The header before all step specific errors.
        /// </summary>
        public const string StepErrorHeader = "Step-bezogene Fehler: \n";

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
            var exportResult = false;

            try
            {
                File.WriteAllText(path, ExportHeader);
                if (this.errorList.Count == 0)
                {
                    File.AppendAllText(path, EmptyErrorExportMessage);
                }
                else
                {
                    var generalErrors = this.errorList.Where(x => x.GetType() == typeof(GeneralError));
                    if (generalErrors.Count() != 0)
                    {
                        File.AppendAllText(path, GeneralErrorHeader);
                        foreach (var error in generalErrors)
                        {
                            File.AppendAllText(path, error.GetErrorString());
                        }
                    }

                    var flowErrors = this.errorList.Where(x => x.GetType() == typeof(FlowError));
                    if (flowErrors.Count() != 0)
                    {
                        File.AppendAllText(path, FlowErrorHeader);
                        foreach (var error in flowErrors)
                        {
                            File.AppendAllText(path, error.GetErrorString());
                        }
                    }

                    var stepErrors = this.errorList.Where(x => x.GetType() == typeof(StepError));
                    if (stepErrors.Count() != 0)
                    {
                        File.AppendAllText(path, StepErrorHeader);
                        foreach (var error in stepErrors)
                        {
                            File.AppendAllText(path, error.GetErrorString());
                        }
                    }
                }

                exportResult = true;                                                
            }
            catch
            {
                exportResult = false;
            }

            return exportResult;
        }
    }
}
