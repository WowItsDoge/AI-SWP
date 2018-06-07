﻿// <copyright file="RucmRule_24_25.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.RucmRules
{
    using System.Collections.Generic;
    using System.Linq;
    using Errors;
    using XmlParser;

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
        /// <param name="flowToCheck">The flow to check for violations.</param>
        /// <param name="referencedBasicFlow">The referenced flow by the flow to check.</param>
        /// <returns>A list containing the errors that occurred during the check.</returns>
        public override List<IError> Check(Flow flowToCheck, Flow referencedBasicFlow = null)
        {
            this.errors = new List<IError>();

            // Check only if it is an alternative flow.
            if (referencedBasicFlow != null)
            {
                var stepsToCheck = flowToCheck.GetSteps();
                if (!this.CheckPathEnd(stepsToCheck, referencedBasicFlow))
                {
                    this.errors.Add(new FlowError(0, "Ein Flow muss immer mit ABORT oder einem gültigen RESUME STEP enden!", "Flow endet ungültig!"));
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
        private bool CheckPathEnd(List<string> blockToCheck, Flow referencedBasicFlow)
        {
            var lastStep = blockToCheck.LastOrDefault();
            if (lastStep == null)
            {
                return false;
            }
            else
            {
                if (this.ContainsEndKeyword(lastStep))
                {
                    return this.CheckForCorrectUsage(blockToCheck.Count, lastStep, referencedBasicFlow);
                }
                else
                {
                    var ifBlocks = new Dictionary<int, List<string>>();
                    for (var i = 0; i < blockToCheck.Count; i++)
                    {
                        var step = blockToCheck[i];
                        var conditionCounter = 0;
                        if (this.ContainsConditionKeyword(step))
                        {
                            ifBlocks[i] = new List<string>();
                            var j = i + 1;
                            conditionCounter++;
                            for (; j < blockToCheck.Count; j++)
                            {
                                if (blockToCheck[j].Split(' ').Contains(RucmRuleKeyWords.IfKeyWord))
                                {
                                    conditionCounter++;
                                    ifBlocks[i].Add(blockToCheck[j]);
                                }
                                else if (this.ContainsConditionEndKeyword(blockToCheck[j]))
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
        /// <return>True if the keyword is used right.</return>
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
                        if (referencedBasicFlow.GetSteps().Count >= numberToResume && numberToResume != 0)
                        {
                            result = true;                            
                        }
                        else
                        {
                            this.errors.Add(new StepError(referencedStepId, string.Format("Stellen Sie sicher, dass es einen Basic Step mit der Nummer {0} gibt!", numberToResume), "Der angegebene RESUME STEP konnte nicht gefunden werden!"));
                        }
                    }
                    else
                    {
                        this.errors.Add(new StepError(referencedStepId, "Stellen Sie sicher, dass die Vorgabe \"RESUME STEP [+ Nummer]\" eingehalten wird.", "Der angegebene RESUME STEP konnte nicht gefunden werden!"));
                    }
                }
                else
                {
                    this.errors.Add(new StepError(referencedStepId, "Stellen Sie sicher, dass bei der Verwendung der Schlüsselwörter ABORT bzw. RESUME STEP die vorgegebene Struktur eingehalten wird!", "Ungültige Zeile"));
                }
            }

            return result;
        }
    }
}