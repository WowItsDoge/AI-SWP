// <copyright file="RucmRule_24_25Test.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation
{
    using NUnit.Framework;

    using UseCaseCore.RuleValidation.RucmRules;
    using UseCaseCore.XmlParser;

    /// <summary>
    /// Test class for the rucm rules 24 and 25.
    /// </summary>
    [TestFixture]
    public class RucmRule_24_25Test
    {
        /// <summary>
        /// Tests the Check of the rules.
        /// </summary>
        [Test]
        public void CheckTest()
        {
            ////var basicFlow = new BasicFlow();
            ////basicFlow.AddStep("The system starts.");
            ////basicFlow.AddStep("The system ends.");

            ////var goodRefFlow = new SpecificAlternativeFlow();
            ////goodRefFlow.AddStep("Bei los gehts los");
            ////goodRefFlow.AddStep("ABORT.");

            ////var badRefFlow = new SpecificAlternativeFlow();
            ////badRefFlow.AddStep("Bei los gehts los");
            ////badRefFlow.AddStep("Bei weiter gehts weiter.");

            ////var globalAlternative = new GlobalAlternativeFlow();
            ////globalAlternative.AddStep("IF ATM customer enters Cancel THEN");
            ////globalAlternative.AddStep("The system cancels the transaction MEANWHILE the system ejects the ATM card.");
            ////globalAlternative.AddStep("ABORT.");

            ////var rule = new RucmRule_24_25();
            ////Assert.IsTrue(rule.Check(basicFlow).Count == 0);
            ////Assert.IsTrue(rule.Check(goodRefFlow).Count == 0);
            ////Assert.IsTrue(rule.Check(badRefFlow).Count != 0);
            ////Assert.IsTrue(rule.Check(globalAlternative).Count == 0);
        }
    }
}
