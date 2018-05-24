// <copyright file="DummyRule.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation.RucmRules
{
    using System;
    using System.Collections.Generic;
    using UseCaseCore.RuleValidation.Errors;
    using UseCaseCore.RuleValidation.RucmRules;
    using UseCaseCore.XmlParser;

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
                this.errorList.Add(new GeneralError("Error #" + i));
            }
        }

        /// <summary>
        /// Can be used to check if a flow violates against this rule.
        /// </summary>
        /// <param name="flowToCheck">The flow to check for violations.</param>
        /// <param name="referencedBasicFlow">The referenced flow by the flow to check.</param>
        /// <returns>A list containing the errors that occurred during the check.</returns>
        public override List<IError> Check(Flow flowToCheck, Flow referencedBasicFlow = null)
        {
            return this.errorList;
        }
    }
}
