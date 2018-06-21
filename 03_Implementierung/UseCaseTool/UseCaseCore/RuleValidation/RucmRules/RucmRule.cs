﻿// <copyright file="RucmRule.cs" company="Team B">
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
        /// <param name="flowToCheck">The flow to check for violations.</param>
        /// <param name="referencedBasicFlow">The referenced flow by the flow to check.</param>
        /// <returns>A list containing the errors that occurred during the check.</returns>
        //// public abstract List<IError> Check(Flow flowToCheck, Flow referencedBasicFlow = null);
        public abstract List<IError> Check(Flow flowToCheck, Flow referencedBasicFlow = new Flow());

        /// <summary>
        /// Can be used to get temporary errors. If a rule generates errors that could be removed again during the validation process, the errors can be received by this method.
        /// </summary>
        /// <returns>A list containing the temporary errors</returns>
        public virtual List<IError> GetTemporaryErrors()
        {
            return new List<IError>();
        }

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
