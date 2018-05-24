// <copyright file="RuleRepository.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.RuleValidation.RucmRules
{
    using System.Collections.Generic;

    /// <summary>
    /// The class containing all the RUCM rules used by this application.
    /// </summary>
    public static class RuleRepository
    {
        /// <summary>
        /// Initializes static members of the <see cref="RuleRepository"/> class.
        /// </summary>
        static RuleRepository()
        {
            Rules = new List<IRule>
            {
                new RucmRule_24_25()
            };
        }
        
        /// <summary>
        /// Gets or Sets the RUCM Rules
        /// </summary>
        public static List<IRule> Rules { get; private set; }
    }
}
