// <copyright file="ScenarioMatrixTest.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.ScenarioMatrix
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using UcIntern;
    using UseCaseCore.ScenarioMatrix;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// The unit test class for the ScenarioMatrix class
    /// </summary>
    [TestFixture]
    public class ScenarioMatrixTest
    {
        /// <summary>
        /// Testing a simple linear graph
        /// </summary>
        [Test]
        public void ScenarioMatrix_LinearGraph_ContainedEdges()
        {
            Scenario s = new Scenario(1);

            Node n1 = new Node("Schritt 1", new FlowIdentifier(FlowType.Basic, 1));
            Node n2 = new Node("Schritt 2", new FlowIdentifier(FlowType.Basic, 1));
            Node n3 = new Node("Schritt 3", new FlowIdentifier(FlowType.Basic, 1));
            Node n4 = new Node("Schritt 4", new FlowIdentifier(FlowType.Basic, 1));
            Node n5 = new Node("Schritt 5", new FlowIdentifier(FlowType.Basic, 1));

            s.Nodes.Add(n1);
            s.Nodes.Add(n2);
            s.Nodes.Add(n3);
            s.Nodes.Add(n4);
            s.Nodes.Add(n5);
            
            Type type = typeof(ScenarioMatrix);
            MethodInfo methodInfo = type.GetMethod("ContainedEdges", BindingFlags.Static | BindingFlags.NonPublic);
            int amount = (int)methodInfo.Invoke(null, new object[] { n1, n2, s });

            Assert.AreEqual(1, amount);
        }
        
        /// <summary>
        /// Testing a simple cyclic graph
        /// </summary>
        [Test]
        public void ScenarioMatrix_CyclicGraph_ContainedEdges()
        {
            Scenario s = new Scenario(1);

            Node n1 = new Node("Schritt 1", new FlowIdentifier(FlowType.Basic, 1));
            Node n2 = new Node("Schritt 2", new FlowIdentifier(FlowType.Basic, 1));
            Node n3 = new Node("Schritt 3", new FlowIdentifier(FlowType.Basic, 1));
            Node n4 = new Node("Schritt 4", new FlowIdentifier(FlowType.Basic, 1));

            s.Nodes.Add(n1);
            s.Nodes.Add(n2);
            s.Nodes.Add(n3);
            s.Nodes.Add(n1);
            s.Nodes.Add(n2);
            s.Nodes.Add(n3);
            s.Nodes.Add(n4);

            Type type = typeof(ScenarioMatrix);
            MethodInfo methodInfo = type.GetMethod("ContainedEdges", BindingFlags.Static | BindingFlags.NonPublic);
            int amount = (int)methodInfo.Invoke(null, new object[] { n1, n2, s });

            Assert.AreEqual(2, amount);
        }
        
        /// <summary>
        /// Testing a cyclic graph with 1 element
        /// </summary>
        [Test]
        public void ScenarioMatrix_CyclicSingularGraph_ContainedEdges()
        {
            Scenario s = new Scenario(1);

            Node n1 = new Node("Schritt 1", new FlowIdentifier(FlowType.Basic, 1));

            s.Nodes.Add(n1);
            s.Nodes.Add(n1);
            s.Nodes.Add(n1);

            Type type = typeof(ScenarioMatrix);
            MethodInfo methodInfo = type.GetMethod("ContainedEdges", BindingFlags.Static | BindingFlags.NonPublic);
            int amount = (int)methodInfo.Invoke(null, new object[] { n1, n1, s });

            Assert.AreEqual(2, amount);
        }
    }
}
