// <copyright file="RucmRuleTest.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation
{
    using NUnit.Framework;
    using RucmRules;

    /// <summary>
    /// Test class for the rucm rules basic class
    /// </summary>
    [TestFixture]
    public class RucmRuleTest
    {
        /// <summary>
        /// Tests the containment checks
        /// </summary>
        [Test]
        public void EndKeywordTest()
        {
            var dummyRule = new DummyRule(0);
            var testStr1 = "ABORT";
            var testStr2 = "RESUME STEP 1";
            var testStr3 = "Nichts con beiden";

            var result = dummyRule.ContainsEndKeywordTest(testStr1);

            Assert.IsTrue(result);

            result = dummyRule.ContainsEndKeywordTest(testStr2);

            Assert.IsTrue(result);

            result = dummyRule.ContainsEndKeywordTest(testStr3);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests the condition checks
        /// </summary>
        [Test]
        public void ConditionKeywordTest()
        {
            var dummyRule = new DummyRule(0);
            var testStr1 = "IF";
            var testStr2 = "ELSE";
            var testStr3 = "THEN";
            var testStr4 = "ELSEIF";
            var testStr5 = "ENDIF";
            var testStr6 = "Keins der Wörter";
            var testStr7 = "Keins der Wörter oder doch mit IF";
            var testStr8 = "Keins der Wörter oder doch mit kleinem if";
            var testStr9 = "IF blabla THEN";

            var result = dummyRule.ContainsConditionKeywordTest(testStr1);

            Assert.IsFalse(result);

            result = dummyRule.ContainsConditionKeywordTest(testStr2);

            Assert.IsTrue(result);

            result = dummyRule.ContainsConditionKeywordTest(testStr3);

            Assert.IsFalse(result);

            result = dummyRule.ContainsConditionKeywordTest(testStr4);

            Assert.IsTrue(result);

            result = dummyRule.ContainsConditionKeywordTest(testStr5);

            Assert.IsFalse(result);

            result = dummyRule.ContainsConditionKeywordTest(testStr6);

            Assert.IsFalse(result);

            result = dummyRule.ContainsConditionKeywordTest(testStr7);

            Assert.IsFalse(result);

            result = dummyRule.ContainsConditionKeywordTest(testStr8);

            Assert.IsFalse(result);

            result = dummyRule.ContainsConditionKeywordTest(testStr9);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests the end condition checks
        /// </summary>
        [Test]
        public void ConditionEndKeywordTest()
        {
            var dummyRule = new DummyRule(0);
            var testStr1 = "IF";
            var testStr2 = "ELSE";
            var testStr3 = "THEN";
            var testStr4 = "ELSEIF";
            var testStr5 = "ENDIF";
            var testStr6 = "Keins der Wörter";
            var testStr7 = "Keins der Wörter oder doch mit ELSE";
            var testStr8 = "Keins der Wörter oder doch mit kleinem else";
            var testStr9 = "IF blabla THEN";

            var result = dummyRule.ContainsConditionEndKeywordTest(testStr1);

            Assert.IsFalse(result);

            result = dummyRule.ContainsConditionEndKeywordTest(testStr2);

            Assert.IsTrue(result);

            result = dummyRule.ContainsConditionEndKeywordTest(testStr3);

            Assert.IsFalse(result);

            result = dummyRule.ContainsConditionEndKeywordTest(testStr4);

            Assert.IsTrue(result);

            result = dummyRule.ContainsConditionEndKeywordTest(testStr5);

            Assert.IsTrue(result);

            result = dummyRule.ContainsConditionEndKeywordTest(testStr6);

            Assert.IsFalse(result);

            result = dummyRule.ContainsConditionEndKeywordTest(testStr7);

            Assert.IsTrue(result);

            result = dummyRule.ContainsConditionEndKeywordTest(testStr8);

            Assert.IsFalse(result);

            result = dummyRule.ContainsConditionEndKeywordTest(testStr9);

            Assert.IsFalse(result);
        }
    }
}