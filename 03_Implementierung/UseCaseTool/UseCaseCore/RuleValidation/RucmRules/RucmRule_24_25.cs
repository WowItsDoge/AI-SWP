// <copyright file="RucmRule_24_25.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.RucmRules
{
    using System.Collections.Generic;
    using System.Linq;
    using Errors;
    using UcIntern;

    /// <summary>
    /// Checks the RUCM rules 24 and 25.
    /// </summary>
    public class RucmRule_24_25 : RucmRule
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
            flowsToCheck.AddRange(globalAlternativeFlows);
            flowsToCheck.AddRange(specificAlternativeFlows);
            flowsToCheck.AddRange(boundedAlternativeFlows);

            foreach (var flow in flowsToCheck)
            {
                var stepsToCheck = (List<Node>)flow.Nodes;
                if (!this.CheckPathEnd(stepsToCheck, basicFlow))
                {
                    this.errors.Add(new FlowError(flow.Identifier.Id, "Flow endet ungültig! \nEin Flow muss immer mit ABORT oder einem gültigen RESUME STEP enden!", "Verletzung der Regel 24/25!"));
                }
            }

            return this.errors;
        }

        /// <summary>
        /// Checks if all paths of the specified block have an end. 
        /// </summary>
        /// <param name="blockToCheck">the block to check.</param>
        /// <param name="referencedBasicFlow">The referenced flow by the flow to check.</param>
        /// <returns>true if all paths have an end.</returns>
        private bool CheckPathEnd(List<Node> blockToCheck, Flow referencedBasicFlow)
        {
            var lastStep = blockToCheck.LastOrDefault();
            if (lastStep == null)
            {
                return false;
            }
            else
            {
                if (this.ContainsEndKeyword(lastStep.StepDescription))
                {
                    return this.CheckForCorrectUsage(lastStep.Identifier.Id, lastStep.StepDescription, referencedBasicFlow);
                }
                else
                {
                    var ifBlocks = new Dictionary<int, List<Node>>();
                    for (var i = 0; i < blockToCheck.Count; i++)
                    {
                        var step = blockToCheck[i];
                        var conditionCounter = 0;
                        if (this.ContainsConditionKeyword(step.StepDescription))
                        {
                            ifBlocks[i] = new List<Node>();
                            var j = i + 1;
                            conditionCounter++;
                            for (; j < blockToCheck.Count; j++)
                            {
                                if (blockToCheck[j].StepDescription.Split(' ').Contains(RucmRuleKeyWords.IfKeyWord))
                                {
                                    conditionCounter++;
                                    ifBlocks[i].Add(blockToCheck[j]);
                                }
                                else if (this.ContainsConditionEndKeyword(blockToCheck[j].StepDescription))
                                {
                                    conditionCounter--;
                                    if (conditionCounter == 0)
                                    {
                                        break;
                                    }

                                    ifBlocks[i].Add(blockToCheck[j]);
                                }
                                else
                                {
                                    ifBlocks[i].Add(blockToCheck[j]);
                                }
                            }

                            i = j - 1;
                        }
                    }

                    var result = ifBlocks.Count != 0;
                    foreach (var block in ifBlocks.Values)
                    {
                        result &= this.CheckPathEnd(block, referencedBasicFlow);
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// Check if the referenced step ending is correct
        /// </summary>
        /// <param name="referencedStepId">the id of the step to check.</param>
        /// <param name="step">the step to check.</param>
        /// <param name="referencedBasicFlow">The referenced basic flow</param>
        /// <returns>True if the keyword is used right.</returns>
        private bool CheckForCorrectUsage(int referencedStepId, string step, Flow referencedBasicFlow)
        {
            var result = false;
            if (step == RucmRuleKeyWords.AbortKeyWord)
            {
                result = true;
            }
            else
            {
                if (step.StartsWith(RucmRuleKeyWords.ResumeKeyWord))
                {
                    int numberToResume = 0;
                    if (int.TryParse(step.Substring(RucmRuleKeyWords.ResumeKeyWord.Length), out numberToResume))
                    {
                        if (referencedBasicFlow.Nodes.Count >= numberToResume && numberToResume != 0)
                        {
                            result = true;                            
                        }
                        else
                        {
                            this.errors.Add(new StepError(referencedStepId, string.Format("Der angegebene RESUME STEP konnte nicht gefunden werden!\nStellen Sie sicher, dass es einen Basic Step mit der Nummer {0} gibt!", numberToResume), "Verletzung der Regel 24 / 25!"));
                        }
                    }
                    else
                    {
                        this.errors.Add(new StepError(referencedStepId, "Der angegebene RESUME STEP konnte nicht gefunden werden!\nStellen Sie sicher, dass die Vorgabe \"RESUME STEP [+ Nummer]\" eingehalten wird.", "Verletzung der Regel 24/25!"));
                    }
                }
                else
                {
                    this.errors.Add(new StepError(referencedStepId, "Ungültige Zeile\n Stellen Sie sicher, dass bei der Verwendung der Schlüsselwörter ABORT bzw. RESUME STEP die vorgegebene Struktur eingehalten wird!", "Verletzung der Regel 24/25!"));
                }
            }

            return result;
        }
    }
}