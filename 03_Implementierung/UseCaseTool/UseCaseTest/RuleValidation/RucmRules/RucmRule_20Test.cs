// <copyright file="RucmRule_20Test.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using UseCaseCore.RuleValidation.RucmRules;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// Test class for the rucm rule 20.
    /// </summary>
    [TestFixture]
    public class RucmRule_20Test
    {
        /// <summary>
        /// Tests the Check of the rules.
        /// </summary>
        [Test]
        public void Check20Test()
        {
            // Check a normal basic Flow with if
            var flowId = new FlowIdentifier(FlowType.Basic, 1);
            var nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("IF blablabla THEN", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));
            var rfStep = new List<ReferenceStep>();

            var basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);
            var globalFlows = new List<Flow>();
            var specificFlows = new List<Flow>();
            var boundedFlows = new List<Flow>();

            var rucmRule = new RucmRule_20();

            var result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 0);

            // Check a normal basic Flow with if and else
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("IF blablabla THEN", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("ELSE", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("Der vierte Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);
            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 0);

            // Check a normal basic Flow with if and elseif
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("IF blablabla THEN", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("ELSEIF blabla THEN", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("Der vierte Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);
            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 0);

            // Check a normal basic Flow with if no endif
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("IF blablabla THEN", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);
            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 2);

            // Check a normal basic Flow with if, else and no endif
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("IF blablabla THEN", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("ELSE", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));
            nodes.Add(new Node("Der vierte Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);
            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 2);

            // Check a normal basic Flow with if, elseif and no endif
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("IF blablabla THEN", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("ELSEIF blabla THEN", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));
            nodes.Add(new Node("Der vierte Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);
            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 2);

            // Check a normal basic Flow with else without if
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("ELSE", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);
            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 0);

            // Check a normal basic Flow with elseif without if
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("ELSEIF blabla THEN", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);
            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 0);

            // Check a normal basic Flow with invalid usage of if
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("test IF true THEN", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("IF THEN", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("IF true", flowId));
            nodes.Add(new Node("Der vierte Schritt", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("Der fünfte Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);
            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 3);

            // Check a normal basic Flow with invalid usage of elseif
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("test ELSEIF true THEN", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("ELSEIF THEN", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("ELSEIF true", flowId));
            nodes.Add(new Node("Der vierte Schritt", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("Der fünfte Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);
            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 3);

            // Check a normal basic Flow with nested if
            nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("IF blablabla THEN", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("IF blabla THEN", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("Der vierte Schritt", flowId));
            nodes.Add(new Node("ENDIF", flowId));
            nodes.Add(new Node("Der fünfte Schritt", flowId));

            basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);
            result = rucmRule.Check(basicFlow, globalFlows, specificFlows, boundedFlows);
            Assert.IsTrue(result.Count == 0);
        }
    }
}