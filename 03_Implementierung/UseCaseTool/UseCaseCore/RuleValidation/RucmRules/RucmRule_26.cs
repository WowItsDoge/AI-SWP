// <copyright file="RucmRule_26.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.RucmRules
{
    using System.Collections.Generic;
    using Errors;
    using UcIntern;

    /// <summary>
    /// Checks the RUCM rule 26.
    /// </summary>
    public class RucmRule_26 : RucmRule
    {
        /// <summary>
        /// The error list from this rule
        /// </summary>
        private List<IError> errors;

        /// <summary>
        /// Can be used to check if a flow violates against this rule.
        /// </summary>
        /// <param name="flowToCheck">The flow to check for violations.</param>
        /// <param name="referencedBasicFlow">The referenced flow by the flow to check.</param>
        /// <returns>A list containing the errors that occurred during the check.</returns>
        public override List<IError> Check(Flow flowToCheck, Flow referencedBasicFlow = new Flow())
        {
            this.errors = new List<IError>();
            var postCondition = flowToCheck.Postcondition;
            if (string.IsNullOrWhiteSpace(postCondition))
            {
                this.errors.Add(new FlowError(flowToCheck.Identifier.Id, "Dieser Flow enthält keine Nachbedingung!\nBitte geben Sie für jeden Flow eine eigene Nachbedingung an.", "Verletzung der Regel 26!"));
            }

            return this.errors;
        }
    }
}
