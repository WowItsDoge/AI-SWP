// <copyright file="ScenarioTest.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.ScenarioMatrix
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using UcIntern;
    using UseCaseCore.ScenarioMatrix;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// The unit test class for the Scenario class
    /// </summary>
    [TestFixture]
    public class ScenarioTest
    {
        /// <summary>
        /// Testing a the copy constructor
        /// </summary>
        [Test]
        public void CopyConstructor_ListReferences_NotEqual()
        {
            Scenario s1 = new Scenario();
            Node n1 = new Node("Schritt 1", new FlowIdentifier(FlowType.Basic, 1));
            Node n2 = new Node("Schritt 2", new FlowIdentifier(FlowType.Basic, 1));
            Node n3 = new Node("Schritt 3", new FlowIdentifier(FlowType.Basic, 1));
            Node n4 = new Node("Schritt 4", new FlowIdentifier(FlowType.Basic, 1));
            Node n5 = new Node("Schritt 5", new FlowIdentifier(FlowType.Basic, 1));

            s1.Nodes.Add(n1);
            s1.Nodes.Add(n2);
            s1.Nodes.Add(n3);

            Scenario s2 = new Scenario(s1);

            s1.Nodes.Add(n4);
            s1.Nodes.Add(n5);

            Assert.AreNotEqual(s1.Nodes.Count(), s2.Nodes.Count());
        }

        /// <summary>
        /// Set Scenario nodes to another list of nodes
        /// </summary>
        [Test]
        public void Set_Nodes_Successful()
        {
            Node n1 = new Node("Schritt 1", new FlowIdentifier(FlowType.Basic, 1));
            Node n2 = new Node("Schritt 2", new FlowIdentifier(FlowType.Basic, 1));

            List<Node> nodes = new List<Node>();
            nodes.Add(n1);
            nodes.Add(n2);

            Scenario s = new Scenario();
            s.Nodes.Add(n1);

            Assert.AreEqual(s.Nodes.Count, 1);

            s.Nodes = nodes;
            Assert.AreEqual(s.Nodes.Count, 2);

        }
    }
}
