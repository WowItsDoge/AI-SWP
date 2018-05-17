// <copyright file="RucmRuleValidator.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation
{
    using System.Collections.Generic;

    using RucmRules;
    using XmlParser;

    /// <summary>
    /// The rule validator instance.
    /// </summary>
    public class RucmRuleValidator : IRucmRuleValidator
    {
        /// <summary>
        /// The error report for the use case.
        /// </summary>
        private ErrorReport errorReport;

        /// <summary>
        /// A list containing all rules to check.s
        /// </summary>
        private List<IRule> ruleList;

        /// <summary>
        /// Initializes a new instance of the <see cref="RucmRuleValidator"/> class.
        /// </summary>
        public RucmRuleValidator()
        {
            this.errorReport = new ErrorReport();
            this.ruleList = new List<IRule>();
        }

        /// <summary>
        /// Method can be called to add an external GeneralError to the ErrorReport, e.g. if the XML file could not be read.
        /// </summary>
        /// <param name="errorToAdd">Contains the message that should be added.</param>
        public void AddExternalError(string errorToAdd)
        {
        }

        /// <summary>
        /// Can be called to export the ErrorReport in a CSV-file.        
        /// </summary>
        /// <param name="path">Specifies the path where to put the file.</param>
        /// <returns>Returns true if the ErrorReport was exported without any problems, otherwise false.</returns>
        public bool Export(string path)
        {
            return false;
        }

        /// <summary>
        /// Can be used to get the actual ErrorReport. 
        /// </summary>
        /// <returns>Returns the existing ErrorReport instance.</returns>
        public ErrorReport GetErrorReport()
        {
            return null;
        }

        /// <summary>
        /// Validates one flow against all the RUCM-Rules. 
        /// For each violation an IError is added to the ErrorReport. 
        /// </summary>
        /// <param name="flowToCheck">The flow that has to be checked.</param>
        /// <param name="referencedBasicFlow">If the flowToCheck is the „Basic Flow“ no „Reference Flow“ has to be passed,
        /// if the flowToCheck is an „Alternative Flow“, the referencedBasicFlow has to be passed in order to validate it properly.</param>
        /// <returns>Returns true if there was no violation found, otherwise false.</returns>
        public bool Validate(Flow flowToCheck, Flow referencedBasicFlow = null)
        {
            return false;
        }
    }
}
