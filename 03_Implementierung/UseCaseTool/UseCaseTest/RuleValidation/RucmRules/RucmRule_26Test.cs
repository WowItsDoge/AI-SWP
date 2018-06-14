// <copyright file="RucmRule_26Test.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation
{
    using NUnit.Framework;

    using UseCaseCore.RuleValidation.RucmRules;
    using UseCaseCore.XmlParser;

    /// <summary>
    /// Test class for the rucm rule 26.
    /// </summary>
    [TestFixture]
    public class RucmRule_26Test
    {
        /// <summary>
        /// Tests the Check of the rules.
        /// </summary>
        [Test]
        public void Check26Test()
        {
            // Basic flow without postcondition
            var basicFlow = new BasicFlow();
            basicFlow.AddStep("Der erste Schritt");
            basicFlow.AddStep("Der zweite Schritt");
            basicFlow.AddStep("Der dritte Schritt");

            var rucmRule = new RucmRule_26();

            var checkResult = rucmRule.Check(basicFlow);
            Assert.IsTrue(checkResult.Count == 1);

            // Basic flow with postcondition
            basicFlow.SetPostcondition("Die Nachbedingung");

            checkResult = rucmRule.Check(basicFlow);
            Assert.IsTrue(checkResult.Count == 0);

            // BasicFlow with two set postconditions
            basicFlow.SetPostcondition("Die zweite Nachbedingung");

            checkResult = rucmRule.Check(basicFlow);
            Assert.IsTrue(checkResult.Count == 0);

            // global flow without post condition
            var globalFlow = new GlobalAlternativeFlow();
            globalFlow.AddStep("Der erste gf-Schritt");
            globalFlow.AddStep("Der zweite gf-Schritt");
            globalFlow.SetId(1);

            checkResult = rucmRule.Check(globalFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 1);

            // global flow with post condition
            globalFlow.SetPostcondition("Die gf-Nachbedingung");

            checkResult = rucmRule.Check(globalFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 0);

            // specific flow without post condition
            var specflow = new SpecificAlternativeFlow();
            specflow.AddStep("Der erste sf-Schritt");
            specflow.AddStep("Der zweite sf-Schritt");

            checkResult = rucmRule.Check(specflow, basicFlow);
            Assert.IsTrue(checkResult.Count == 1);

            // specific flow with post condition
            specflow.SetPostcondition("Die sf-Nachbedingung");

            checkResult = rucmRule.Check(specflow, basicFlow);
            Assert.IsTrue(checkResult.Count == 0);

            // bounded flow without post condition
            var boundedFlow = new BoundedAlternativeFlow();
            boundedFlow.AddStep("Der erste bf-Schritt");
            boundedFlow.AddStep("Der zweite bf-Schritt");

            checkResult = rucmRule.Check(boundedFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 1);

            // bounded flow with post condition
            boundedFlow.SetPostcondition("Die bf-Nachbedingung");

            checkResult = rucmRule.Check(boundedFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 0);
        }
    }
}