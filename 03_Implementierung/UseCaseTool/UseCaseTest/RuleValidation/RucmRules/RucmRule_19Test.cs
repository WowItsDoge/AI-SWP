// <copyright file="RucmRule_19Test.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation
{
    using NUnit.Framework;

    using UseCaseCore.RuleValidation.RucmRules;
    using UseCaseCore.UcIntern;
    using UseCaseCore.XmlParser;

    /// <summary>
    /// Test class for the rucm rules 24 and 25.
    /// </summary>
    [TestFixture]
    public class RucmRule_19Test
    {
        /// <summary>
        /// Tests the Check of the rules.
        /// </summary>
        [Test]
        public void Check19Test()
        {
            // Basic flow has nor RFS
            var basicFlow = new BasicFlow();
            basicFlow.SetPostcondition("Die Standard-Nachbedingung.");
            basicFlow.AddStep("Der erste Schritt");
            basicFlow.AddStep("Der zweite Schritt");
            basicFlow.AddStep("Der dritte Schritt");

            var rucmRule = new RucmRule_19();

            var checkResult = rucmRule.Check(basicFlow);
            Assert.IsTrue(checkResult.Count == 0);

            // Also global flows have no rfs
            var globalFlow = new GlobalAlternativeFlow();
            globalFlow.AddStep("Der erste gf-Schritt");
            globalFlow.AddStep("Der zweite gf-Schritt");
            globalFlow.SetId(1);
            globalFlow.SetPostcondition("Die gf-Nachbedingung");

            checkResult = rucmRule.Check(globalFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 0);

            // Specific alternative flow should have exactly one RFS
            // 0 RFS
            var specificFlow = new SpecificAlternativeFlow();
            specificFlow.AddStep("Der erste gf-Schritt");
            specificFlow.AddStep("Der zweite gf-Schritt");
            specificFlow.SetPostcondition("Die gf-Nachbedingung");

            checkResult = rucmRule.Check(specificFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 1);

            // 1 RFS
            specificFlow.AddReferenceStep(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 2));
            checkResult = rucmRule.Check(specificFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 0);

            // 2 RFS
            specificFlow = new SpecificAlternativeFlow();
            specificFlow.AddReferenceStep(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 1));
            specificFlow.AddReferenceStep(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 2));
            checkResult = rucmRule.Check(specificFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 0);

            // 1 invalid RFS
            specificFlow = new SpecificAlternativeFlow();
            specificFlow.AddReferenceStep(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 6));
            checkResult = rucmRule.Check(specificFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 1);
            
            // bounded alternative flow should one or more RFS
            // 0 RFS
            var boundedFlow = new BoundedAlternativeFlow();
            boundedFlow.AddStep("Der erste gf-Schritt");
            boundedFlow.AddStep("Der zweite gf-Schritt");
            boundedFlow.SetPostcondition("Die gf-Nachbedingung");

            checkResult = rucmRule.Check(boundedFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 1);

            // 1 RFS
            boundedFlow.AddReferenceStep(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 2));
            checkResult = rucmRule.Check(boundedFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 0);

            // 2 RFS
            boundedFlow = new BoundedAlternativeFlow();
            boundedFlow.AddReferenceStep(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 1));
            boundedFlow.AddReferenceStep(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 2));
            checkResult = rucmRule.Check(boundedFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 0);

            // 1 invalid RFS
            boundedFlow = new BoundedAlternativeFlow();
            boundedFlow.AddReferenceStep(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 6));
            checkResult = rucmRule.Check(boundedFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 1);

            // 2 invalid of 3 RFS
            boundedFlow = new BoundedAlternativeFlow();
            boundedFlow.AddReferenceStep(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 1));
            boundedFlow.AddReferenceStep(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 6));
            boundedFlow.AddReferenceStep(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 4));
            checkResult = rucmRule.Check(boundedFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 2);
        }
    }
}