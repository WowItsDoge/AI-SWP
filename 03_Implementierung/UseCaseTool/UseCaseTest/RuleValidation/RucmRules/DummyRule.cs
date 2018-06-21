// <copyright file="DummyRule.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation.RucmRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UseCaseCore.RuleValidation.Errors;
    using UseCaseCore.RuleValidation.RucmRules;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A dummy rule for testing only
    /// </summary>
    public class DummyRule : RucmRule
    {
        /// <summary>
        /// The internal error list
        /// </summary>
        private readonly List<IError> errorList;

        /// <summary>
        /// Initializes a new instance of the <see cref="DummyRule"/> class.
        /// </summary>
        /// <param name="errorCount">The number of errors that should be produced in the check method.</param>
        public DummyRule(int errorCount)
        {
            this.errorList = new List<IError>();
            for (int i = 0; i < errorCount; i++)
            {
                this.errorList.Add(new GeneralError("Error #" + (i+1)));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DummyRule"/> class.
        /// </summary>
        /// <param name="generalCount">The number of general errors that should be produced in the check method.</param>
        /// <param name="flowCount">The number of flow errors that should be produced in the check method.</param>
        /// <param name="stepCount">The number of step errors that should be produced in the check method.</param>
        public DummyRule(int generalCount, int flowCount, int stepCount)
        {
            this.errorList = new List<IError>();
            for (int i = 0; i < generalCount;)
            {
                i++;
                this.errorList.Add(new GeneralError("Error #" + i ));
            }

            for (int i = 0; i < flowCount;)
            {
                i++;
                this.errorList.Add(new FlowError(i, "Lösung zu: " + i, "Error #" + i));
            }

            for (int i = 0; i < stepCount;)
            {
                i++;
                this.errorList.Add(new StepError(i, "Lösung zu: " + i, "Error #" + i));
            }
        }

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
            return this.errorList;
        }

        public bool ContainsEndKeywordTest(string stepToCheck)
        {
            return this.ContainsEndKeyword(stepToCheck);
        }

        public bool ContainsConditionKeywordTest(string stepToCheck)
        {
            return this.ContainsConditionKeyword(stepToCheck);
        }

        public bool ContainsConditionEndKeywordTest(string stepToCheck)
        {
            return this.ContainsConditionEndKeyword(stepToCheck);
        }

    }
}
