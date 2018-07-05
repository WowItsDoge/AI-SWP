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
        public static readonly string[] AbortKeyWord = { "ABORT", "ABBRECHEN" };

        /// <summary>
        /// The keyword for resuming to a step
        /// </summary>
        public static readonly string[] ResumeKeyWord = { "RESUME STEP", "FORTSETZEN SCHRITT" };

        /// <summary>
        /// The keyword for a IF condition
        /// </summary>
        public static readonly string[] IfKeyWord = { "IF", "WENN" };

        /// <summary>
        /// The keyword for a then statement
        /// </summary>
        public static readonly string[] ThenKeyWord = { "THEN", "DANN" };

        /// <summary>
        /// The keyword for a else statement
        /// </summary>
        public static readonly string[] ElseKeyWord = { "ELSE", "SONST" };

        /// <summary>
        /// The keyword for a else-if statement
        /// </summary>
        public static readonly string[] ElseifKeyWord = { "ELSEIF", "SONST WENN" };

        /// <summary>
        /// The keyword for ending an if statement
        /// </summary>
        public static readonly string[] EndifKeyWord = { "ENDIF", "ENDE WENN" };

        /// <summary>
        /// The starting keyword for a loop
        /// </summary>
        public static readonly string[] DoKeyWord = { "DO", "TUE" };

        /// <summary>
        /// The ending keyword for a loop
        /// </summary>
        public static readonly string[] UntilKeyWord = { "UNTIL", "SOLANGE" };

        /// <summary>
        /// The keyword for a validation
        /// </summary>
        public static readonly string[] ValidateKeyWord = { "VALIDATES THAT", "VALIDIERT, DASS" };
    }
}
