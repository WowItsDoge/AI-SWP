﻿// <copyright file="RucmRuleRespositoryTest.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation
{
    using NUnit.Framework;
    using System.Linq;
    using UseCaseCore.RuleValidation.RucmRules;

    /// <summary>
    /// Test class for the rucm rules repositories
    /// </summary>
    [TestFixture]
    public class RucmRuleRespositoryTest
    {
        /// <summary>
        /// Tests the creation of the rules.
        /// </summary>
        [Test]
        public void RucmRulesRepositoryTest()
        {
            Assert.IsTrue(RucmRuleRepository.Rules.Count == 6);
            Assert.IsTrue(RucmRuleRepository.Rules.Any(x => x.GetType() == typeof(RucmRule_24_25)));
            Assert.IsTrue(RucmRuleRepository.Rules.Any(x => x.GetType() == typeof(RucmRule_19)));
            Assert.IsTrue(RucmRuleRepository.Rules.Any(x => x.GetType() == typeof(RucmRule_20)));
            Assert.IsTrue(RucmRuleRepository.Rules.Any(x => x.GetType() == typeof(RucmRule_22)));
            Assert.IsTrue(RucmRuleRepository.Rules.Any(x => x.GetType() == typeof(RucmRule_26)));
            Assert.IsTrue(RucmRuleRepository.Rules.Any(x => x.GetType() == typeof(RucmRule_23)));
        }
    }
}