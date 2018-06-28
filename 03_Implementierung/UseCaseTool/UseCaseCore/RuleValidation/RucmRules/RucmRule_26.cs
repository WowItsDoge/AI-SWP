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
        /// <param name="basicFlow">The basic flow that has to be checked.</param>
        /// <param name="globalAlternativeFlows">The global alternative flows that have to be checked.</param>
        /// <param name="specificAlternativeFlows">The specific alternative flows that have to be checked.</param>
        /// <param name="boundedAlternativeFlows">The bounded alternative flows that have to be checked.</param>
        /// <returns>A list containing the errors that occurred during the check.</returns>
        public override List<IError> Check(Flow basicFlow, List<Flow> globalAlternativeFlows, List<Flow> specificAlternativeFlows, List<Flow> boundedAlternativeFlows)
        {
            this.errors = new List<IError>();
            var flowsToCheck = new List<Flow>();
            flowsToCheck.Add(basicFlow);
            flowsToCheck.AddRange(globalAlternativeFlows);
            flowsToCheck.AddRange(specificAlternativeFlows);
            flowsToCheck.AddRange(boundedAlternativeFlows);

            foreach (var flow in flowsToCheck)
            {
                var postCondition = flow.Postcondition;
                if (string.IsNullOrWhiteSpace(postCondition))
                {
                    this.errors.Add(new FlowError(flow.Identifier.Id, "Dieser Flow enthält keine Nachbedingung!\r\nBitte geben Sie für jeden Flow eine eigene Nachbedingung an.", "Verletzung der Regel 26!"));
                }
            }

            return this.errors;
        }
    }
}
