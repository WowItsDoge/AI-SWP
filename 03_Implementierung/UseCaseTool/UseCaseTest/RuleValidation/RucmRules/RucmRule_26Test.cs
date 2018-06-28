// <copyright file="RucmRule_26Test.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using UseCaseCore.RuleValidation.RucmRules;
    using UseCaseCore.UcIntern;

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
            var flowId = new FlowIdentifier(FlowType.Basic, 1);
            var nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));
            var rfStep = new List<ReferenceStep>();

            var basicFlow = new Flow(flowId, string.Empty, nodes, rfStep);
            var globalFlows = new List<Flow>();
            var specificFlows = new List<Flow>();
            var boundedFlows = new List<Flow>();

            var rucmRule = new RucmRule_26();

            var checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 1);

            // Basic flow with postcondition
            basicFlow = new Flow(flowId, "Die Nachbedingung", nodes, rfStep);

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 0);

            // BasicFlow with null
            basicFlow = new Flow(flowId, null, nodes, rfStep);

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 1);

            // global flow without post condition
            flowId = new FlowIdentifier(FlowType.GlobalAlternative, 2);
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste gf-Schritt", flowId));
            nodes.Add(new Node("Der zweite gf-Schritt", flowId));
            rfStep = new List<ReferenceStep>();

            var globalFlow = new Flow(flowId, string.Empty, nodes, rfStep);
            basicFlow = new Flow(flowId, "Die Nachbedingung", nodes, rfStep);
            globalFlows = new List<Flow>
            {
                globalFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 1);

            // global flow with post condition
            globalFlow = new Flow(flowId, "Die gf-Nachbedingung", nodes, rfStep);
            globalFlows = new List<Flow>
            {
                globalFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 0);

            // specific flow without post condition
            flowId = new FlowIdentifier(FlowType.SpecificAlternative, 3);
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste sf-Schritt", flowId));
            nodes.Add(new Node("Der zweite sf-Schritt", flowId));
            rfStep = new List<ReferenceStep>();

            var specflow = new Flow(flowId, string.Empty, nodes, rfStep);
            globalFlows.Clear();
            specificFlows = new List<Flow>
            {
                specflow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 1);

            // specific flow with post condition
            specflow = new Flow(flowId, "Die sf-Nachbedingung", nodes, rfStep);
            specificFlows = new List<Flow>
            {
                specflow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 0);

            // bounded flow without post condition
            flowId = new FlowIdentifier(FlowType.BoundedAlternative, 4);
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste bf-Schritt", flowId));
            nodes.Add(new Node("Der zweite bf-Schritt", flowId));
            rfStep = new List<ReferenceStep>();

            var boundedFlow = new Flow(flowId, string.Empty, nodes, rfStep);
            specificFlows.Clear();
            boundedFlows = new List<Flow>
            {
                boundedFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 1);

            // bounded flow with post condition
            boundedFlow = new Flow(flowId, "Die bf-Nachbedingung", nodes, rfStep);
            boundedFlows = new List<Flow>
            {
                boundedFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 0);
        }
    }
}