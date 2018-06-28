// <copyright file="RucmRule_20.cs" company="Team B">
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
    public class RucmRule_20 : RucmRule
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
                var stepsToCheck = (List<Node>)flow.Nodes;
                if (!this.CheckIf(stepsToCheck))
                {
                    this.errors.Add(new FlowError(flow.Identifier.Id, "Flow enthält eine ungültige IF - Struktur! \r\nBitte achten Sie auf eine geschlossene IF-Struktur!", "Verletzung der Regel 20!"));
                }
            }

            return this.errors;
        }

        /// <summary>
        /// Checks if the specified block has a valid if. 
        /// </summary>
        /// <param name="blockToCheck">the block to check.</param>
        /// <returns>true if the block has a valid if structure.</returns>
        private bool CheckIf(List<Node> blockToCheck)
        {
            var result = true;
            var ifBlocks = new Dictionary<int, List<Node>>();
            for (var i = 0; i < blockToCheck.Count; i++)
            {
                var step = blockToCheck[i];
                var conditionCounter = 0;
                if (this.ContainsConditionKeyword(step.StepDescription) && this.CheckForCorrectUsage(step))
                {
                    ifBlocks[i] = new List<Node>();
                    var j = i + 1;
                    conditionCounter++;
                    for (; j < blockToCheck.Count; j++)
                    {
                        if (this.ContainsConditionKeyword(blockToCheck[j].StepDescription))
                        {
                            conditionCounter++;
                        }

                        if (this.ContainsConditionEndKeyword(blockToCheck[j].StepDescription))
                        {
                            conditionCounter--;
                            if (!this.CheckForCorrectUsage(step))
                            {
                                result = false;
                            }

                            if (conditionCounter == 0)
                            {
                                ifBlocks[i].Add(blockToCheck[j]);
                                break;
                            }
                        }

                        ifBlocks[i].Add(blockToCheck[j]);
                    }

                    if (j == blockToCheck.Count)
                    {
                        this.errors.Add(new StepError(step.Identifier.Id, "Es wurde eine ungültige Verwendung eines Schlüsselwortes erkannt!\r\nStellen Sie sicher, dass die Schlüsselwörter IF, THEN, ELSE, ELSEIF und ENDIF richtig verwendet werden.", "Verletzung der Regel 20!"));
                        result = false;
                    }

                    i = j - 1;
                }
            }

            foreach (var block in ifBlocks.Values)
            {
                result &= this.CheckIf(block);
            }

            return result;
        }

        /// <summary>
        /// Check if the string is used correct
        /// </summary>
        /// <param name="stepToCheck">the step to check.</param>
        /// <returns>True if the keyword is used right.</returns>
        private bool CheckForCorrectUsage(Node stepToCheck)
        {
            var stringToCheck = stepToCheck.StepDescription;
            var result = false;
            if (stringToCheck == RucmRuleKeyWords.EndifKeyWord)
            {
                result = true;
            }
            else if (stringToCheck.StartsWith(RucmRuleKeyWords.ElseifKeyWord))
            {
                if (!stringToCheck.EndsWith(RucmRuleKeyWords.ThenKeyWord))
                {
                    this.errors.Add(new StepError(stepToCheck.Identifier.Id, "Ungültige Verwendung von ELSEIF!\r\nStellen Sie sicher, dass der Aufbau \"ELSEIF condition THEN\" eingehalten wird.", "Verletzung der Regel 20!"));
                }
                else if (string.IsNullOrWhiteSpace(stringToCheck.Replace(RucmRuleKeyWords.ElseifKeyWord, string.Empty).Replace(RucmRuleKeyWords.ThenKeyWord, string.Empty)))
                {
                    this.errors.Add(new StepError(stepToCheck.Identifier.Id, "Ungültige Verwendung von ELSEIF!\r\nStellen Sie sicher, dass der Aufbau \"ELSEIF condition THEN\" eingehalten wird.", "Verletzung der Regel 20!"));
                }
                else
                {
                    result = true;
                }
            }
            else if (stringToCheck == RucmRuleKeyWords.ElseKeyWord)
            {
                result = true;
            }
            else if (stringToCheck.StartsWith(RucmRuleKeyWords.IfKeyWord))
            {
                if (!stringToCheck.EndsWith(RucmRuleKeyWords.ThenKeyWord))
                {
                    this.errors.Add(new StepError(stepToCheck.Identifier.Id, "Ungültige Verwendung von IF!\r\nStellen Sie sicher, dass der Aufbau \"IF condition THEN\" eingehalten wird.", "Verletzung der Regel 20!"));
                }
                else if (string.IsNullOrWhiteSpace(stringToCheck.Replace(RucmRuleKeyWords.IfKeyWord, string.Empty).Replace(RucmRuleKeyWords.ThenKeyWord, string.Empty)))
                {
                    this.errors.Add(new StepError(stepToCheck.Identifier.Id, "Ungültige Verwendung von IF!\r\nStellen Sie sicher, dass der Aufbau \"IF condition THEN\" eingehalten wird.", "Verletzung der Regel 20!"));
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                this.errors.Add(new StepError(stepToCheck.Identifier.Id, "Es wurde eine ungültige Verwendung eines Schlüsselwortes erkannt!\r\nStellen Sie sicher, dass die Schlüsselwörter IF, THEN, ELSE, ELSEIF und ENDIF richtig verwendet werden.", "Verletzung der Regel 20!"));
            }

            return result;
        }
    }
}