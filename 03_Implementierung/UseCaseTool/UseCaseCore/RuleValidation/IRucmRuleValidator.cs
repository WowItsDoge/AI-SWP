// <copyright file="IRucmRuleValidator.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation
{
    using System.Collections.Generic;
    using UcIntern;

    /// <summary>
    /// The interface for the Rule Validator.
    /// </summary>
    public interface IRucmRuleValidator
    {
        /// <summary>
        /// Validates one flow against all the RUCM-Rules. 
        /// For each violation an IError is added to the ErrorReport. 
        /// </summary>
        /// <param name="basicFlow">The basic flow that has to be checked.</param>
        /// <param name="globalAlternativeFlows">The global alternative flows that have to be checked.</param>
        /// <param name="specificAlternativeFlows">The specific alternative flows that have to be checked.</param>
        /// <param name="boundedAlternativeFlows">The bounded alternative flows that have to be checked.</param>
        /// <returns>Returns true if there was no violation found, otherwise false.</returns>
        bool Validate(Flow basicFlow, List<Flow> globalAlternativeFlows, List<Flow> specificAlternativeFlows, List<Flow> boundedAlternativeFlows);

        /// <summary>
        /// Method can be called to add an external GeneralError to the ErrorReport, e.g. if the XML file could not be read.
        /// </summary>
        /// <param name="errorToAdd">Contains the message that should be added.</param>
        void AddExternalError(string errorToAdd);

        /// <summary>
        /// Can be used to get the actual ErrorReport. 
        /// </summary>
        /// <returns>Returns the existing ErrorReport instance.</returns>
        ErrorReport GetErrorReport();

        /// <summary>
        /// Can be called to export the ErrorReport in a CSV-file.        
        /// </summary>
        /// <param name="path">Specifies the path where to put the file.</param>
        /// <returns>Returns true if the ErrorReport was exported without any problems, otherwise false.</returns>
        bool Export(string path);

        /// <summary>
        /// Resets the RUCM rule validator.
        /// </summary>
        void Reset();
    }
}
