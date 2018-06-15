


// <copyright file="RucmRule_24_25Test.cs" company="Team B">
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
    public class RucmRule_24_25Test
    {
        /// <summary>
        /// Tests the Check of the rules.
        /// </summary>
        [Test]
        public void Check2425Test()
        {

            /*
            
            var basicFlow = new BasicFlow();
            basicFlow.AddStep("Der erste Schritt");
            basicFlow.AddStep("Der zweite Schritt");
            basicFlow.AddStep("Der dritte Schritt");

            var globalFlow = new GlobalAlternativeFlow();

            // Basic flow does not have to end with ABORT or RESUME.
            var rucmRule = new RucmRule_24_25();
            var checkResult = rucmRule.Check(basicFlow);
            Assert.IsTrue(checkResult.Count == 0);

            // Check if alternative flow ends with ABORT
            checkResult = rucmRule.Check(globalFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 1);

            globalFlow.AddStep("ABORT");
            checkResult = rucmRule.Check(globalFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 0);

            // Check if the form of the abort is correct
            globalFlow = new GlobalAlternativeFlow();
            globalFlow.AddStep("Hier Endet der UC ABORT");

            checkResult = rucmRule.Check(globalFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 2);

            globalFlow = new GlobalAlternativeFlow();
            globalFlow.AddStep("Hier Endet der UC");
            globalFlow.AddStep("RESUME STEP 5");

            checkResult = rucmRule.Check(globalFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 2);

            globalFlow = new GlobalAlternativeFlow();
            globalFlow.AddStep("Hier Endet der UC");
            globalFlow.AddStep("RESUME STEP %");

            checkResult = rucmRule.Check(globalFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 2);

            globalFlow = new GlobalAlternativeFlow();
            globalFlow.AddStep("Hier Endet der UC");
            globalFlow.AddStep("deswegen RESUME STEP Der erste Schritt");

            checkResult = rucmRule.Check(globalFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 2);

            // Check if specific alternative flow ends correctly
            var specificFlow = new SpecificAlternativeFlow();
            specificFlow.AddReferenceStep(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 2));
            specificFlow.SetPostcondition("The status is crisis");
            specificFlow.AddStep("IF blablabla THEN");
            specificFlow.AddStep("RESUME STEP 1");
            specificFlow.AddStep("ENDIF");
            specificFlow.AddStep("IF blablabla THEN");
            specificFlow.AddStep("RESUME STEP 2");
            specificFlow.AddStep("ENDIF");
            specificFlow.AddStep("IF blablabla THEN");
            specificFlow.AddStep("do some stuff");
            specificFlow.AddStep("DO");
            specificFlow.AddStep("IF blablabla THEN");
            specificFlow.AddStep("RESUME STEP 1");
            specificFlow.AddStep("ENDIF");
            specificFlow.AddStep("IF blablabla THEN");
            specificFlow.AddStep("RESUME STEP 1");
            specificFlow.AddStep("ENDIF");
            specificFlow.AddStep("IF blablabla THEN");
            specificFlow.AddStep("ABORT");
            specificFlow.AddStep("ENDIF");
            specificFlow.AddStep("UNTIL blablabla");
            specificFlow.AddStep("ENDIF");

            checkResult = rucmRule.Check(specificFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 0);

            specificFlow = new SpecificAlternativeFlow();
            specificFlow.AddReferenceStep(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 2));
            specificFlow.SetPostcondition("The status is crisis");
            specificFlow.AddStep("IF blablabla THEN");
            specificFlow.AddStep("do some stuff");
            specificFlow.AddStep("DO");
            specificFlow.AddStep("do some stuff");
            specificFlow.AddStep("UNTIL blablabla");
            specificFlow.AddStep("ENDIF");

            checkResult = rucmRule.Check(specificFlow, basicFlow);
            Assert.IsTrue(checkResult.Count == 1);

            */
        }
    }
}