// <copyright file="RucmRule_24_25Test.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using UseCaseCore.RuleValidation.RucmRules;
    using UseCaseCore.UcIntern;

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
            var flowId = new FlowIdentifier(FlowType.Basic, 1);
            var nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));
            var rfStep = new List<ReferenceStep>();

            var basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);
            var globalFlows = new List<Flow>();
            var specificFlows = new List<Flow>();
            var boundedFlows = new List<Flow>();

            flowId = new FlowIdentifier(FlowType.GlobalAlternative, 2);
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste gf-Schritt", flowId));
            nodes.Add(new Node("Der zweite gf-Schritt", flowId));
            rfStep = new List<ReferenceStep>();

            var globalFlow = new Flow(flowId, "Die gf-Nachbedingung", nodes, rfStep);

            // Basic flow does not have to end with ABORT or RESUME.
            var rucmRule = new RucmRule_24_25();
            var checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 0);

            // Check if alternative flow ends with ABORT
            globalFlows.Add(globalFlow);
            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 1);

            nodes.Add(new Node("ABORT", flowId));
            globalFlow = new Flow(flowId, "Die gf-Nachbedingung", nodes, rfStep);
            globalFlows = new List<Flow>
            {
                globalFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 0);

            // Check if the form of the abort is correct
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste gf-Schritt", flowId));
            nodes.Add(new Node("Hier Endet der UC ABORT", flowId));
            globalFlow = new Flow(flowId, "Die gf-Nachbedingung", nodes, rfStep);
            globalFlows = new List<Flow>
            {
                globalFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 2);

            nodes = new List<Node>();
            nodes.Add(new Node("Hier Endet der UC", flowId));
            nodes.Add(new Node("RESUME STEP 5", flowId));
            globalFlow = new Flow(flowId, "Die gf-Nachbedingung", nodes, rfStep);
            globalFlows = new List<Flow>
            {
                globalFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 2);

            nodes = new List<Node>();
            nodes.Add(new Node("Hier Endet der UC", flowId));
            nodes.Add(new Node("RESUME STEP %", flowId));
            globalFlow = new Flow(flowId, "Die gf-Nachbedingung", nodes, rfStep);
            globalFlows = new List<Flow>
            {
                globalFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 2);

            nodes = new List<Node>();
            nodes.Add(new Node("Hier Endet der UC", flowId));
            nodes.Add(new Node("deswegen RESUME STEP Der erste Schritt", flowId));
            globalFlow = new Flow(flowId, "Die gf-Nachbedingung", nodes, rfStep);
            globalFlows = new List<Flow>
            {
                globalFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 2);

            // Check if specific alternative flow ends correctly
            flowId = new FlowIdentifier(FlowType.SpecificAlternative, 3);
            nodes = new List<Node>();
            nodes.Add(new Node("IF blablabla THEN", flowId));
            nodes.Add(new Node("RESUME STEP 1", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("IF blablabla THEN", flowId));
            nodes.Add(new Node("RESUME STEP 2", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("IF blablabla THEN", flowId));
            nodes.Add(new Node("do some stuff", flowId));
            nodes.Add(new Node("DO", flowId));
            nodes.Add(new Node("IF blablabla THEN", flowId));
            nodes.Add(new Node("RESUME STEP 1", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("IF blablabla THEN", flowId));
            nodes.Add(new Node("RESUME STEP 1", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("IF blablabla THEN", flowId));
            nodes.Add(new Node("ABORT", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("UNTIL blablabla", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            rfStep = new List<ReferenceStep>();
            rfStep.Add(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 2));

            var specificFlow = new Flow(flowId, "The status is crisis", nodes, rfStep);
            globalFlows.Clear();
            specificFlows = new List<Flow>
            {
                specificFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 0);

            nodes = new List<Node>();
            nodes.Add(new Node("IF blablabla THEN", flowId));
            nodes.Add(new Node("do some stuff", flowId));
            nodes.Add(new Node("DO", flowId));
            nodes.Add(new Node("do some stuff", flowId));
            nodes.Add(new Node("UNTIL blablabla", flowId));
            nodes.Add(new Node("ENDIF", flowId));

            specificFlow = new Flow(flowId, "The status is crisis", nodes, rfStep);
            specificFlows = new List<Flow>
            {
                specificFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 1);
        }
    }
}