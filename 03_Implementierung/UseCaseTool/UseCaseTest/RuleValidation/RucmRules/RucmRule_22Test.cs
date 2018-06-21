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
            // Check a normal basic Flow for 
            var flowId = new FlowIdentifier(FlowType.Basic, 1);
            var nodes = new List<Node>();
            nodes.Add(new Node("Der erste Schritt", flowId));
            nodes.Add(new Node("VALIDATES THAT the use case is valid", flowId));
            nodes.Add(new Node("VALIDATES THAT the use case is valid again", flowId));
            nodes.Add(new Node("Der zweite Schritt", flowId));
            nodes.Add(new Node("Der dritte Schritt", flowId));
            var rfStep = new List<ReferenceStep>();

            var basicFlow = new Flow(flowId, "Die Standard-Nachbedingung.", nodes, rfStep);

            var rucmRule = new RucmRule_22();
        }
    }
}