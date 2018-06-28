// <copyright file="RucmRule_22Test.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using UseCaseCore.RuleValidation.RucmRules;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// Test class for the rucm rule 22.
    /// </summary>
    [TestFixture]
    public class RucmRule_22Test
    {
        /// <summary>
        /// Tests the Check of the rules.
        /// </summary>
        [Test]
        public void Check22Test()
        {
            // Check a normal basic Flow for a RFS for every VALIDATES THAT with specific flows
            var flowId = new FlowIdentifier(FlowType.Basic, 1);
            var nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("VALIDATES THAT the use case is valid", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("VALIDATES THAT the use case is valid again", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));
            var rfStep = new List<ReferenceStep>();

            var basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);
            var globalFlows = new List<Flow>();
            var specificFlows = new List<Flow>();
            var boundedFlows = new List<Flow>();

            var specFlowId1 = new FlowIdentifier(FlowType.SpecificAlternative, 2);
            var specNodes = new List<Node>();
            specNodes.Add(new Node("Der erste Schritt", flowId));
            specNodes.Add(new Node("Der zweite Schritt", flowId));
            specNodes.Add(new Node("Der dritte Schritt", flowId));
            var specRfStep1 = new List<ReferenceStep>();
            specRfStep1.Add(new ReferenceStep(flowId, 2));

            specificFlows.Add(new Flow(specFlowId1, "Test", specNodes, specRfStep1));

            var specFlowId2 = new FlowIdentifier(FlowType.SpecificAlternative, 3);
            var specRfStep2 = new List<ReferenceStep>();
            specRfStep2.Add(new ReferenceStep(flowId, 4));

            specificFlows.Add(new Flow(specFlowId2, "Test", specNodes, specRfStep2));
            
            var rucmRule = new RucmRule_22();
            
            var result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 0);

            // Check a normal basic Flow for a RFS for every VALIDATES THAT with bounded flows
            var boundedFlowId1 = new FlowIdentifier(FlowType.BoundedAlternative, 2);
            var boundedNodes = new List<Node>();
            boundedNodes.Add(new Node("Der erste Schritt", flowId));
            boundedNodes.Add(new Node("Der zweite Schritt", flowId));
            boundedNodes.Add(new Node("Der dritte Schritt", flowId));
            var boundedRfStep1 = new List<ReferenceStep>();
            boundedRfStep1.Add(new ReferenceStep(flowId, 2));
            boundedRfStep1.Add(new ReferenceStep(flowId, 4));

            specificFlows = new List<Flow>();
            boundedFlows.Add(new Flow(boundedFlowId1, "Test", boundedNodes, boundedRfStep1));

            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 0);

            // Check a normal basic Flow for a RFS for every VALIDATES THAT with specific flows
            var specFlowId = new FlowIdentifier(FlowType.SpecificAlternative, 2);
            var specRfStep = new List<ReferenceStep>();
            specRfStep.Add(new ReferenceStep(flowId, 4));

            specificFlows = new List<Flow>();
            boundedFlows = new List<Flow>();
            specificFlows.Add(new Flow(specFlowId, "Test", specNodes, specRfStep));

            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 1);
            
            specRfStep = new List<ReferenceStep>();
            specRfStep.Add(new ReferenceStep(flowId, 3));

            specificFlows = new List<Flow>();
            specificFlows.Add(new Flow(specFlowId, "Test", specNodes, specRfStep));

            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 2);
        }
    }
}