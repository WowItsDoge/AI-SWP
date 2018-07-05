// <copyright file="RucmRule_22.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.RucmRules
{
    using System.Collections.Generic;
    using System.Linq;
    using Errors;
    using UcIntern;

    /// <summary>
    /// Checks the RUCM rule 22.
    /// </summary>
    public class RucmRule_22 : RucmRule
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
            var referencedStepNumbers = new List<int>();
            foreach (var specflow in specificAlternativeFlows)
            {
                foreach (var rfs in specflow.ReferenceSteps)
                {
                    referencedStepNumbers.Add(rfs.Step);
                }
            }

            foreach (var boundedFlow in boundedAlternativeFlows)
            {
                foreach (var rfs in boundedFlow.ReferenceSteps)
                {
                    referencedStepNumbers.Add(rfs.Step);
                }
            }
            
            for (int i = 0; i < basicFlow.Nodes.Count; i++)
            {
                var step = basicFlow.Nodes[i];
                if (RucmRuleKeyWords.ValidateKeyWord.Any(x => step.StepDescription.Contains(x)))
                {
                    if (!referencedStepNumbers.Any(x => x == (i + 1)))
                    {
                        this.errors.Add(new StepError(i + 1, "Kein \"alternativer Pfad\" gefunden! \r\n Einem Step mit VALIDATES THAT muss immer mindestens ein RFS zugeordnet sein.", "Verletzung der Regel 22!"));
                    }
                }
            }

            return this.errors;
        }
    }
}
