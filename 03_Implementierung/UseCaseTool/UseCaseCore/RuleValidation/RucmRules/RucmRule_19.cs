// <copyright file="RucmRule_19.cs" company="Team B">
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
    public class RucmRule_19 : RucmRule
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
        public override List<IError> Check(Flow flowToCheck, Flow referencedBasicFlow = null)
        {
            this.errors = new List<IError>();
            if (flowToCheck.GetType() == typeof(SpecificAlternativeFlow))
            {
                var flow = (SpecificAlternativeFlow)flowToCheck;
                var rfs = flow.GetReferenceStep();
                if (rfs == null)
                {
                    this.errors.Add(new FlowError(0, "Geben Sie im Feld RFS genau einen Referenzschritt an!", "Es wurde kein RFS gefunden."));
                }
                else
                {
                    if (rfs.Step > referencedBasicFlow.GetSteps().Count || rfs.Step == 0)
                    {
                        this.errors.Add(new FlowError(0, "Bitte überprüfen Sie die Nummer des Referenzschrittes!", string.Format("Es wurde kein zum RFS {0} passender Step gefunden.", rfs.Step)));
                    }
                }
            }
            else if (flowToCheck.GetType() == typeof(BoundedAlternativeFlow))
            {
                var flow = (BoundedAlternativeFlow)flowToCheck;
                var referenceSteps = flow.GetReferenceSteps();
                if (referenceSteps.Count == 0)
                {
                    this.errors.Add(new FlowError(0, "Geben Sie im Feld RFS mindestens einen Referenzschritt an!", "Es wurde kein RFS gefunden."));
                }
                else
                {
                    foreach (var rfs in referenceSteps)
                    {
                        if (rfs.Step > referencedBasicFlow.GetSteps().Count || rfs.Step == 0)
                        {
                            this.errors.Add(new FlowError(0, "Bitte überprüfen Sie die Nummer des Referenzschrittes!", string.Format("Es wurde kein zum RFS {0} passender Step gefunden.", rfs.Step)));
                        }
                    }
                }
            }

            return this.errors;
        }
    }
}