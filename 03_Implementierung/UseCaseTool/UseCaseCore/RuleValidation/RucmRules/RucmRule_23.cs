﻿// <copyright file="RucmRule_23.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.RucmRules
{
    using System.Collections.Generic;
    using Errors;
    using UcIntern;

    /// <summary>
    /// Checks the RUCM rule 23.
    /// </summary>
    public class RucmRule_23 : RucmRule
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
            if (!this.CheckStepsForCompleteLoop((List<Node>)flowToCheck.Nodes))
            {
                this.errors.Add(new FlowError(flowToCheck.Identifier.Id, "Flow enthält ungültige Schleife! \nEin Flow muss eine immer geschlossene DO-UNTIL Schleife enthalten.", "Verletzung der Regel 23!"));
            }            

            return this.errors;
        }

        /// <summary>
        /// A recursive method to check if the steps contain a complete DO-UNTIL loop
        /// </summary>
        /// <param name="stepsToCheck">the steps to check for the loop</param>
        /// <returns>True if the steps contain a complete loop.</returns>
        private bool CheckStepsForCompleteLoop(List<Node> stepsToCheck)
        {
            var result = true;

            var doList = new Dictionary<int, List<Node>>();
            for (int i = 0; i < stepsToCheck.Count; i++)
            {
                var step = stepsToCheck[i];
                if (step.StepDescription.Contains(RucmRuleKeyWords.DoKeyWord))
                {
                    if (step.StepDescription != RucmRuleKeyWords.DoKeyWord)
                    {
                        this.errors.Add(new StepError(step.Identifier.Id, "Ungültige Verwendung von DO. \nBitte verwenden Sie für das DO einen eigenen Schritt!", "Verletzung der Regel 23!"));
                        result = false;
                        break;
                    }

                    doList[i] = new List<Node>();
                    var j = i + 1;
                    var doCounter = 1;
                    for (; j < stepsToCheck.Count; j++)
                    {
                        if (stepsToCheck[j].StepDescription.Contains(RucmRuleKeyWords.DoKeyWord))
                        {
                            doCounter++;
                        }
                        else if (stepsToCheck[j].StepDescription.Contains(RucmRuleKeyWords.UntilKeyWord))
                        {
                            doCounter--;
                            if (doCounter == 0)
                            {
                                if (!stepsToCheck[j].StepDescription.StartsWith(RucmRuleKeyWords.UntilKeyWord) || 
                                    string.IsNullOrWhiteSpace(stepsToCheck[j].StepDescription.Replace(RucmRuleKeyWords.UntilKeyWord, string.Empty)))
                                {
                                    this.errors.Add(new StepError(step.Identifier.Id, "Ungültige Verwendung von UNTIL.\nBitte verwenden Sie für UNTIL die Syntax \"UNTIL condition\"!", "Verletzung der Regel 23!"));
                                    result = false;
                                }

                                break;
                            }
                        }

                        doList[i].Add(stepsToCheck[j]);
                    }

                    if (doCounter != 0 && j == stepsToCheck.Count)
                    {
                        this.errors.Add(new StepError(step.Identifier.Id, "DO ohne zugehöriges UNTIL gefunden! \nBitte achten Sie auf eine geschlossene DO-UNTIL-Schleifenstruktur", "Verletzung der Regel 23!"));
                        result = false;
                        break;
                    }

                    i = j;
                }
                else if (step.StepDescription.Contains(RucmRuleKeyWords.UntilKeyWord))
                {
                    this.errors.Add(new StepError(step.Identifier.Id, "UNTIL ohne zugehöriges DO gefunden! \nBitte achten Sie auf eine geschlossene DO-UNTIL-Schleifenstruktur", "Verletzung der Regel 23!"));
                    result = false;
                    break;
                }
            }

            foreach (var nestedLoop in doList.Values)
            {
                result &= this.CheckStepsForCompleteLoop(nestedLoop);
            }
            
            return result;
        }
    }
}
