﻿// <copyright file="RucmRuleValidator.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation
{
    using System.Collections.Generic;

    using Errors;
    using RucmRules;
    using UcIntern;

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
        /// <param name="rules">A list containing all rules that should be used.</param>
        public RucmRuleValidator(List<IRule> rules)
        {
            this.errorReport = new ErrorReport();
            this.ruleList = rules;
        }

        /// <summary>
        /// Method can be called to add an external GeneralError to the ErrorReport, e.g. if the XML file could not be read.
        /// </summary>
        /// <param name="errorToAdd">Contains the message that should be added.</param>
        public void AddExternalError(string errorToAdd)
        {
            this.errorReport.AddError(new GeneralError(errorToAdd));
        }

        /// <summary>
        /// Can be called to export the ErrorReport in a CSV-file.        
        /// </summary>
        /// <param name="path">Specifies the path where to put the file.</param>
        /// <returns>Returns true if the ErrorReport was exported without any problems, otherwise false.</returns>
        public bool Export(string path)
        {
            return this.errorReport.Export(path);
        }

        /// <summary>
        /// Can be used to get the actual ErrorReport. 
        /// </summary>
        /// <returns>Returns the existing ErrorReport instance.</returns>
        public ErrorReport GetErrorReport()
        {
            return this.errorReport;
        }

        /// <summary>
        /// Validates one flow against all the RUCM-Rules. 
        /// For each violation an IError is added to the ErrorReport. 
        /// </summary>
        /// <param name="basicFlow">The basic flow that has to be checked.</param>
        /// <param name="globalAlternativeFlows">The global alternative flows that have to be checked.</param>
        /// <param name="specificAlternativeFlows">The specific alternative flows that have to be checked.</param>
        /// <param name="boundedAlternativeFlows">The bounded alternative flows that have to be checked.</param>
        /// <returns>Returns true if there was no violation found, otherwise false.</returns>
        public bool Validate(Flow basicFlow, List<Flow> globalAlternativeFlows, List<Flow> specificAlternativeFlows, List<Flow> boundedAlternativeFlows)
        {
            var result = true;
            foreach (var rule in this.ruleList)
            {
                var errors = rule.Check(basicFlow, globalAlternativeFlows, specificAlternativeFlows, boundedAlternativeFlows);
                foreach (var error in errors)
                {
                    this.errorReport.AddError(error);
                    result = false;
                }               
            }

            return result;
        }

        /// <summary>
        /// Resets the RUCM rule validator.
        /// </summary>
        public void Reset()
        {
            this.errorReport = new ErrorReport();
        }    
    }
}