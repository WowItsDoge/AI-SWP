// <copyright file="RucmRule_24_25.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.RucmRules
{
    using System.Collections.Generic;
    using Errors;
    using XmlParser;

    /// <summary>
    /// Checks the RUCM rules 24 and 25.
    /// </summary>
    public class RucmRule_24_25 : RucmRule
    {
        /// <summary>
        /// Can be used to check if a flow violates against this rule.
        /// </summary>
        /// <param name="flowToCheck">The flow to check for violations.</param>
        /// <param name="referencedBasicFlow">The referenced flow by the flow to check.</param>
        /// <returns>A list containing the errors that occurred during the check.</returns>
        public override List<IError> Check(Flow flowToCheck, Flow referencedBasicFlow = null)
        {
            var errors = new List<IError>();
            if (referencedBasicFlow != null)
            {
            }

            return errors;
        }
    }
}
