// <copyright file="RucmRule_23Test.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation
{
    using NUnit.Framework;

    using UseCaseCore.RuleValidation.RucmRules;
    using UseCaseCore.XmlParser;

    /// <summary>
    /// Test class for the rucm rule 23.
    /// </summary>
    [TestFixture]
    public class RucmRule_23Test
    {
        /// <summary>
        /// Tests the Check of the rules.
        /// </summary>
        [Test]
        public void Check23Test()
        {
            // Check a normal basic Flow for DO-UNTIL
            var basicFlow = new BasicFlow();
            basicFlow.AddStep("Der erste Schritt");
            basicFlow.AddStep("DO");
            basicFlow.AddStep("Der erste Schritt im Do");
            basicFlow.AddStep("Der zweite Schritt im Do");
            basicFlow.AddStep("UNTIL Stop");
            basicFlow.AddStep("Der zweite Schritt");
            basicFlow.AddStep("Der dritte Schritt");

            var rucmRule = new RucmRule_23();

            var result = rucmRule.Check(basicFlow);
            Assert.IsTrue(result.Count == 0);

            // Check no DO-UNTIL
            basicFlow = new BasicFlow();
            basicFlow.AddStep("Der erste Schritt");
            basicFlow.AddStep("Der zweite Schritt");

            result = rucmRule.Check(basicFlow);
            Assert.IsTrue(result.Count == 0);

            // Check for a nested DO-UNTIL
            basicFlow = new BasicFlow();
            basicFlow.AddStep("Der erste Schritt");
            basicFlow.AddStep("DO");
            basicFlow.AddStep("Der erste Schritt im Do");
            basicFlow.AddStep("DO");
            basicFlow.AddStep("Der erste Schritt im Do-Do");
            basicFlow.AddStep("UNTIL Stop");
            basicFlow.AddStep("UNTIL Stop-Stop");
            basicFlow.AddStep("Der zweite Schritt");

            result = rucmRule.Check(basicFlow);
            Assert.IsTrue(result.Count == 0);

            // Check for two seperate DOs
            basicFlow = new BasicFlow();
            basicFlow.AddStep("Der erste Schritt");
            basicFlow.AddStep("DO");
            basicFlow.AddStep("Der erste Schritt im Do");
            basicFlow.AddStep("UNTIL Stop");
            basicFlow.AddStep("DO");
            basicFlow.AddStep("Der erste Schritt im Do-Do");
            basicFlow.AddStep("UNTIL Stop-Stop");
            basicFlow.AddStep("Der zweite Schritt");

            result = rucmRule.Check(basicFlow);
            Assert.IsTrue(result.Count == 0);

            // Check for a DO without UNTIL
            basicFlow = new BasicFlow();
            basicFlow.AddStep("Der erste Schritt");
            basicFlow.AddStep("DO");
            basicFlow.AddStep("Der erste Schritt im Do");
            basicFlow.AddStep("Der zweite Schritt");

            result = rucmRule.Check(basicFlow);
            Assert.IsTrue(result.Count == 2);

            // Check for a UNTIL without DO
            basicFlow = new BasicFlow();
            basicFlow.AddStep("Der erste Schritt");
            basicFlow.AddStep("Der erste Schritt im Do");
            basicFlow.AddStep("UNTIL Stop");
            basicFlow.AddStep("Der zweite Schritt");

            result = rucmRule.Check(basicFlow);
            Assert.IsTrue(result.Count == 2);

            // Check for a UNTIL without DO
            basicFlow = new BasicFlow();
            basicFlow.AddStep("Der erste Schritt");
            basicFlow.AddStep("Der erste Schritt im Do");
            basicFlow.AddStep("Stop UNTIL");
            basicFlow.AddStep("Der zweite Schritt");

            result = rucmRule.Check(basicFlow);
            Assert.IsTrue(result.Count == 2);

            // Check for a DO-UNTIL without a Stopcondition
            basicFlow = new BasicFlow();
            basicFlow.AddStep("Der erste Schritt");
            basicFlow.AddStep("DO");
            basicFlow.AddStep("Der erste Schritt im Do");
            basicFlow.AddStep("UNTIL");
            basicFlow.AddStep("Der zweite Schritt");

            result = rucmRule.Check(basicFlow);
            Assert.IsTrue(result.Count == 2);

            // Check for a DO-UNTIL without the stuff TODO in the DO
            basicFlow = new BasicFlow();
            basicFlow.AddStep("Der erste Schritt");
            basicFlow.AddStep("DO Das alles was hier steht");
            basicFlow.AddStep("UNTIL Stop gesetzt wird");
            basicFlow.AddStep("Der zweite Schritt");

            result = rucmRule.Check(basicFlow);
            Assert.IsTrue(result.Count == 2);
        }
    }
}