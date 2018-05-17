// <copyright file="RucmRule.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.RucmRules
{
    using System.Collections.Generic;
    using Errors;
    using XmlParser;

    /// <summary>
    /// The abstract base class for all the RUCM rules
    /// </summary>
    public abstract class RucmRule : IRule
    {
        /// <summary>
        /// Can be used to check if a flow violates against this rule.
        /// </summary>
        /// <param name="flowToCheck">The flow to check for violations.</param>
        /// <param name="referencedBasicFlow">The referenced flow by the flow to check.</param>
        /// <returns>A list containing the errors that occurred during the check.</returns>
        public abstract List<IError> Check(Flow flowToCheck, Flow referencedBasicFlow = null);
    }
}
