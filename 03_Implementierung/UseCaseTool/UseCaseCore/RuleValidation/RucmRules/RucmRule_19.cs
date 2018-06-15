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
        /// <param name="flowToCheck">The flow to check for violations.</param>
        /// <param name="referencedBasicFlow">The referenced flow by the flow to check.</param>
        /// <returns>A list containing the errors that occurred during the check.</returns>
        //// public override List<IError> Check(Flow flowToCheck, Flow referencedBasicFlow = null)
        public override List<IError> Check(Flow flowToCheck, Flow referencedBasicFlow = new Flow())
        {
            this.errors = new List<IError>();
            if (flowToCheck.Identifier.Type == FlowType.SpecificAlternative)
            {
                var flow = flowToCheck;
                var rfs = flow.ReferenceSteps;
                if (rfs.Count == 0)
                {
                    this.errors.Add(new FlowError(0, "Geben Sie im Feld RFS genau einen Referenzschritt an!", "Es wurde kein RFS gefunden."));
                }
                else
                {
                    if (rfs[0].Step > referencedBasicFlow.Nodes.Count || rfs[0].Step == 0)
                    {
                        this.errors.Add(new FlowError(0, "Bitte überprüfen Sie die Nummer des Referenzschrittes!", string.Format("Es wurde kein zum RFS {0} passender Step gefunden.", rfs[0].Step)));
                    }
                }
            }
            else if (flowToCheck.Identifier.Type == FlowType.BoundedAlternative)
            {
                var flow = flowToCheck;
                var referenceSteps = flow.ReferenceSteps;
                if (referenceSteps.Count == 0)
                {
                    this.errors.Add(new FlowError(0, "Geben Sie im Feld RFS mindestens einen Referenzschritt an!", "Es wurde kein RFS gefunden."));
                }
                else
                {
                    foreach (var rfs in referenceSteps)
                    {
                        if (rfs.Step > referencedBasicFlow.Nodes.Count || rfs.Step == 0)
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