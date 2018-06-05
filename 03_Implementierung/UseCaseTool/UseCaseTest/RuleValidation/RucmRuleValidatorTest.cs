// <copyright file="RucmRuleValidatorTest.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation
{
    using NUnit.Framework;
    using RucmRules;
    using System.Collections.Generic;
    using UseCaseCore.RuleValidation;
    using UseCaseCore.RuleValidation.RucmRules;
    using UseCaseCore.XmlParser;

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
            var dummyBasicFlow = new BasicFlow();

            var dummy1 = new DummyRule(0);
            var dummy2 = new DummyRule(1);
            var dummy3 = new DummyRule(2);

            // Test validation with 3 rules and 3 errors
            var ruleList = new List<IRule> { dummy1, dummy2, dummy3 };
            var rucmRuleValidator = new RucmRuleValidator(ruleList);
            var result = rucmRuleValidator.Validate(dummyBasicFlow);

            Assert.IsFalse(result);
            Assert.IsTrue(rucmRuleValidator.GetErrorReport().GetErrorList.Count == 3);

            // Test validation with 2 rules and 2 errors
            ruleList = new List<IRule> { dummy1, dummy3 };
            rucmRuleValidator = new RucmRuleValidator(ruleList);
            result = rucmRuleValidator.Validate(dummyBasicFlow);
            Assert.IsFalse(result);
            Assert.IsTrue(rucmRuleValidator.GetErrorReport().GetErrorList.Count == 2);

            // Test validation with 1 rules and 0 errors
            ruleList = new List<IRule> { dummy1 };
            rucmRuleValidator = new RucmRuleValidator(ruleList);
            result = rucmRuleValidator.Validate(dummyBasicFlow);
            Assert.IsTrue(result);
            Assert.IsTrue(rucmRuleValidator.GetErrorReport().GetErrorList.Count == 0);
        }
    }
}
