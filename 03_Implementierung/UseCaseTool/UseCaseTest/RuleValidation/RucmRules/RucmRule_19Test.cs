// <copyright file="RucmRule_19Test.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using UseCaseCore.RuleValidation.RucmRules;
    using UseCaseCore.UcIntern;
    using UseCaseCore.XmlParser;

    /// <summary>
    /// Test class for the rucm rule 19
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

            var rucmRule = new RucmRule_19();

            var checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 0);

            // Also global flows have no rfs
            flowId = new FlowIdentifier(FlowType.GlobalAlternative, 1);
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste gf-Schritt", flowId));
            nodes.Add(new Node("Der zweite gf-Schritt", flowId));
            rfStep = new List<ReferenceStep>();

            var globalFlow = new Flow(flowId, "Die gf-Nachbedingung", nodes, rfStep);
            globalFlows.Add(globalFlow);
            
            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 0);

            // Specific alternative flow should have exactly one RFS
            // 0 RFS
            flowId = new FlowIdentifier(FlowType.SpecificAlternative, 2);
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste sf-Schritt", flowId));
            nodes.Add(new Node("Der zweite sf-Schritt", flowId));
            rfStep = new List<ReferenceStep>();

            var specificFlow = new Flow(flowId, "Die sf-Nachbedingung", nodes, rfStep);
            globalFlows.Clear();
            specificFlows.Add(specificFlow);

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 1);

            // 1 RFS
            rfStep.Add(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 2));
            specificFlow = new Flow(flowId, "Die sf-Nachbedingung", nodes, rfStep);
            specificFlows = new List<Flow>
            {
                specificFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 0);

            // 2 RFS
            rfStep.Add(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 1));
            specificFlow = new Flow(flowId, "Die sf-Nachbedingung", nodes, rfStep);
            specificFlows = new List<Flow>
            {
                specificFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 1);

            // 1 invalid RFS
            rfStep = new List<ReferenceStep>();
            rfStep.Add(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 6));
            specificFlow = new Flow(flowId, "Die sf-Nachbedingung", nodes, rfStep);
            specificFlows = new List<Flow>
            {
                specificFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 1);

            // bounded alternative flow should one or more RFS
            // 0 RFS
            flowId = new FlowIdentifier(FlowType.BoundedAlternative, 3);
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste bf-Schritt", flowId));
            nodes.Add(new Node("Der zweite bf-Schritt", flowId));
            rfStep = new List<ReferenceStep>();

            var boundedFlow = new Flow(flowId, "Die bf-Nachbedingung", nodes, rfStep);
            specificFlows.Clear();
            boundedFlows = new List<Flow>
            {
                boundedFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 1);

            // 1 RFS
            rfStep.Add(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 2));
            boundedFlow = new Flow(flowId, "Die bf-Nachbedingung", nodes, rfStep);
            boundedFlows = new List<Flow>
            {
                boundedFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 0);

            // 2 RFS
            rfStep.Add(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 1));
            boundedFlow = new Flow(flowId, "Die bf-Nachbedingung", nodes, rfStep);
            boundedFlows = new List<Flow>
            {
                boundedFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 0);
            
            // 1 invalid RFS
            rfStep.Add(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 6));
            boundedFlow = new Flow(flowId, "Die bf-Nachbedingung", nodes, rfStep);
            boundedFlows = new List<Flow>
            {
                boundedFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 1);

            // 2 invalid of 3 RFS
            rfStep = new List<ReferenceStep>();
            rfStep.Add(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 2));
            rfStep.Add(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 6));
            rfStep.Add(new ReferenceStep(new FlowIdentifier(FlowType.Basic, 1), 4));
            boundedFlow = new Flow(flowId, "Die bf-Nachbedingung", nodes, rfStep);
            boundedFlows = new List<Flow>
            {
                boundedFlow
            };

            checkResult = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(checkResult.Count == 2);
        }
    }
}