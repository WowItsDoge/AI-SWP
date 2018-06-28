// <copyright file="RucmRule_23Test.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using UseCaseCore.RuleValidation.RucmRules;
    using UseCaseCore.UcIntern;

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
            var flowId = new FlowIdentifier(FlowType.Basic, 1);
            var nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("DO", flowId));
            nodes.Add(new Node("Der erste Schritt im Do", flowId));
            nodes.Add(new Node("Der zweite Schritt im Do", flowId));
            nodes.Add(new Node("UNTIL Stop", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));
            var rfStep = new List<ReferenceStep>();

            var basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);
            var globalFlows = new List<Flow>();
            var specificFlows = new List<Flow>();
            var boundedFlows = new List<Flow>();

            var rucmRule = new RucmRule_23();

            var result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 0);

            // Check no DO-UNTIL
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);

            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 0);

            // Check for a nested DO-UNTIL
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("DO", flowId));
            nodes.Add(new Node("Der erste Schritt im Do", flowId));
            nodes.Add(new Node("DO", flowId));
            nodes.Add(new Node("Der erste Schritt im Do-Do", flowId));
            nodes.Add(new Node("UNTIL Stop", flowId));
            nodes.Add(new Node("UNTIL Stop-Stop", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);

            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 0);

            // Check for two seperate DOs
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("DO", flowId));
            nodes.Add(new Node("Der erste Schritt im Do", flowId));
            nodes.Add(new Node("UNTIL Stop", flowId));
            nodes.Add(new Node("DO", flowId));
            nodes.Add(new Node("Der erste Schritt im Do-Do", flowId));
            nodes.Add(new Node("UNTIL Stop-Stop", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);

            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 0);

            // Check for a DO without UNTIL
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("DO", flowId));
            nodes.Add(new Node("Der erste Schritt im Do", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);

            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 2);

            // Check for a UNTIL without DO
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("Der erste Schritt im Do", flowId));
            nodes.Add(new Node("UNTIL Stop", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);

            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 2);

            // Check for a UNTIL on the end
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("DO", flowId));
            nodes.Add(new Node("Der erste Schritt im Do", flowId));
            nodes.Add(new Node("Stop UNTIL", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);

            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 2);

            // Check for a DO-UNTIL without a Stopcondition
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("DO", flowId));
            nodes.Add(new Node("Der erste Schritt im Do", flowId));
            nodes.Add(new Node("UNTIL", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);

            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 2);

            // Check for a DO-UNTIL without the stuff TODO in the DO
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("DO Das alles was hier steht", flowId));
            nodes.Add(new Node("UNTIL Stop gesetzt wird", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);

            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 2);
        }
    }
}