// <copyright file="RucmRuleKeyWords.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.RucmRules
{
    /// <summary>
    /// A class containing all RUCM keywords
    /// </summary>
    public static class RucmRuleKeyWords
    {
        /// <summary>
        /// The keyword for aborting a UC
        /// </summary>
        public const string AbortKeyWord = "ABORT";

        /// <summary>
        /// The keyword for resuming to a step
        /// </summary>
        public const string ResumeKeyWord = "RESUME STEP";

        /// <summary>
        /// The keyword for a IF condition
        /// </summary>
        public const string IfKeyWord = "IF";

        /// <summary>
        /// The keyword for a then statement
        /// </summary>
        public const string ThenKeyWord = "THEN";

        /// <summary>
        /// The keyword for a else statement
        /// </summary>
        public const string ElseKeyWord = "ELSE";

        /// <summary>
        /// The keyword for a else-if statement
        /// </summary>
        public const string ElseifKeyWord = "ELSEIF";

        /// <summary>
        /// The keyword for ending an if statement
        /// </summary>
        public const string EndifKeyWord = "ENDIF";

        /// <summary>
        /// The starting keyword for a loop
        /// </summary>
        public const string DoKeyWord = "DO";

        /// <summary>
        /// The ending keyword for a loop
        /// </summary>
        public const string UntilKeyWord = "UNTIL";

        /// <summary>
        /// The keyword for a validation
        /// </summary>
        public const string ValidateKeyWord = "VALIDATES THAT";
    }
}
