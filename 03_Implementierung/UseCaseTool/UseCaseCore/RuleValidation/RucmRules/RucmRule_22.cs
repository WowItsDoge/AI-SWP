// <copyright file="RucmRule_22.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.RucmRules
{
    using System.Collections.Generic;
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
        /// The dictionary containing the number of the basic flow step that contains VALIDATES THAT and a value indicating whether a RFS already occurred. 
        /// </summary>
        private Dictionary<int, bool> validatesOccurred = new Dictionary<int, bool>();

        /// <summary>
        /// Can be used to check if a flow violates against this rule.
        /// </summary>
        /// <param name="flowToCheck">The flow to check for violations.</param>
        /// <param name="referencedBasicFlow">The referenced flow by the flow to check.</param>
        /// <returns>A list containing the errors that occurred during the check.</returns>
        public override List<IError> Check(Flow flowToCheck, Flow referencedBasicFlow = new Flow())
        {
            this.errors = new List<IError>();

            // reset temp-error dictionary on new basic Flow
            if (flowToCheck.Identifier.Type == FlowType.Basic)
            {
                this.validatesOccurred = new Dictionary<int, bool>();
            }

            return this.errors;
        }

        /// <summary>
        /// Can be used to get temporary errors. If a rule generates errors that could be removed again during the validation process, the errors can be received by this method.
        /// </summary>
        /// <returns>A list containing the temporary errors</returns>
        public override List<IError> GetTemporaryErrors()
        {
            return new List<IError>();
        }
    }
}
