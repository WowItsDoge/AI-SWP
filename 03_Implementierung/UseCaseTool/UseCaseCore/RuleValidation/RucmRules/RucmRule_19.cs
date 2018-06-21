// <copyright file="RucmRule_19.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.RucmRules
{
    using System.Collections.Generic;
    using Errors;
    using UcIntern;

    /// <summary>
    /// Checks the RUCM rules 24 and 25.
    /// </summary>
    public class RucmRule_19 : RucmRule
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
            foreach (var specificFlow in specificAlternativeFlows)
            {
                var rfs = specificFlow.ReferenceSteps;
                if (rfs.Count != 1)
                {
                    this.errors.Add(new FlowError(specificFlow.Identifier.Id, "Geben Sie im Feld RFS genau einen Referenzschritt an!", "Verletzung der Regel 19!"));
                }
                else
                {
                    if (rfs[0].Step > basicFlow.Nodes.Count || rfs[0].Step == 0)
                    {
                        this.errors.Add(new FlowError(specificFlow.Identifier.Id, string.Format("Bitte überprüfen Sie die Nummer des Referenzschrittes!\nEs wurde kein zum RFS {0} passender Step gefunden.", rfs[0].Step), "Verletzung der Regel 19!"));
                    }
                }
            }

            foreach (var boundedFlow in boundedAlternativeFlows)
            {
                var referenceSteps = boundedFlow.ReferenceSteps;
                if (referenceSteps.Count == 0)
                {
                    this.errors.Add(new FlowError(boundedFlow.Identifier.Id, "Geben Sie im Feld RFS mindestens einen Referenzschritt an!", "Verletzung der Regel 19!"));
                }
                else
                {
                    foreach (var rfs in referenceSteps)
                    {
                        if (rfs.Step > basicFlow.Nodes.Count || rfs.Step == 0)
                        {
                            this.errors.Add(new FlowError(boundedFlow.Identifier.Id, string.Format("Bitte überprüfen Sie die Nummer des Referenzschrittes!\nEs wurde kein zum RFS {0} passender Step gefunden.", rfs.Step), "Verletzung der Regel 19!"));
                        }
                    }
                }
            }

            return this.errors;
        }
    }
}