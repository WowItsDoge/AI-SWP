// <copyright file="RucmRuleRepository.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.RucmRules
{
    using System.Collections.Generic;

    /// <summary>
    /// The class containing all the RUCM rules used by this application.
    /// </summary>
    public static class RucmRuleRepository
    {
        /// <summary>
        /// Initializes static members of the <see cref="RucmRuleRepository"/> class.
        /// </summary>
        static RucmRuleRepository()
        {
            Rules = new List<IRule>
            {
                new RucmRule_19(),
                new RucmRule_20(),
                new RucmRule_22(),
                new RucmRule_23(),
                new RucmRule_24_25(),
                new RucmRule_26()                
            };
        }
        
        /// <summary>
        /// Gets or Sets the RUCM Rules
        /// </summary>
        public static List<IRule> Rules { get; private set; }
    }
}
