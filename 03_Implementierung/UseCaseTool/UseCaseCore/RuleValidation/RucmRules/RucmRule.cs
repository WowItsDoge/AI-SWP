// <copyright file="RucmRule.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.RucmRules
{
    using System.Collections.Generic;
    using Errors;
    using UcIntern;

    /// <summary>
    /// The abstract base class for all the RUCM rules
    /// </summary>
    public abstract class RucmRule : IRule
    {
        /// <summary>
        /// Can be used to check if a flow violates against this rule.
        /// </summary>
        /// <param name="basicFlow">The basic flow that has to be checked.</param>
        /// <param name="globalAlternativeFlows">The global alternative flows that have to be checked.</param>
        /// <param name="specificAlternativeFlows">The specific alternative flows that have to be checked.</param>
        /// <param name="boundedAlternativeFlows">The bounded alternative flows that have to be checked.</param>
        /// <returns>A list containing the errors that occurred during the check.</returns>
        public abstract List<IError> Check(Flow basicFlow, List<Flow> globalAlternativeFlows, List<Flow> specificAlternativeFlows, List<Flow> boundedAlternativeFlows);

        /// <summary>
        /// Checks if a string contains an ending keyword.
        /// </summary>
        /// <param name="stepToCheck">A string to check for the keywords.</param>
        /// <returns>True if the string contains one of the keywords.</returns>
        protected bool ContainsEndKeyword(string stepToCheck)
        {
            if (stepToCheck.Contains(RucmRuleKeyWords.AbortKeyWord) || stepToCheck.Contains(RucmRuleKeyWords.ResumeKeyWord))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a string contains an condition keyword.
        /// </summary>
        /// <param name="stepToCheck">A string to check for the keywords.</param>
        /// <returns>True if the string contains one of the keywords.</returns>
        protected bool ContainsConditionKeyword(string stepToCheck)
        {
            if (stepToCheck.Contains(RucmRuleKeyWords.IfKeyWord + " ") || stepToCheck.Contains(RucmRuleKeyWords.ElseifKeyWord) || stepToCheck.Contains(RucmRuleKeyWords.ElseKeyWord))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a string contains an condition ending keyword.
        /// </summary>
        /// <param name="stepToCheck">A string to check for the keywords.</param>
        /// <returns>True if the string contains one of the keywords.</returns>
        protected bool ContainsConditionEndKeyword(string stepToCheck)
        {
            if (stepToCheck.Contains(RucmRuleKeyWords.ElseifKeyWord) || stepToCheck.Contains(RucmRuleKeyWords.EndifKeyWord) || stepToCheck.Contains(RucmRuleKeyWords.ElseKeyWord))
            {
                return true;
            }

            return false;
        }
    }
}
