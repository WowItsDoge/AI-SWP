// <copyright file="RucmRuleValidatorTest.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation
{
    using NUnit.Framework;
    using UseCaseCore.RuleValidation;
    using UseCaseCore.RuleValidation.RucmRules;

    /// <summary>
    /// Test class for RUCMRuleValidator
    /// </summary>
    [TestFixture]
    public class RucmRuleValidatorTest
    {
        /// <summary>
        /// Validates one flow against all the RUCM-Rules. 
        /// </summary>
        [Test]
        public void ValidateTest()
        {            
            var rucmRuleValidator = new RucmRuleValidator(RuleRepository.Rules);
        }
    }
}
