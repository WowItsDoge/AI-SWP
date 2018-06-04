﻿// <copyright file="ScenarioMatrixTest.cs" company="Team B">
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
        /// Tests if a scenario is contained in a list of scenarios
        /// </summary>
        /// <param name="scenarios"> List of scenarios to be searched </param>
        /// <param name="expectedScenario"> Scenario which is expected </param>
        /// <returns> returns true if expectedScenario is contained in the scenarios </returns>
        public bool ContainsScenario(List<Scenario> scenarios, List<Node> expectedScenario)
        {
            foreach (Scenario s in scenarios) 
            {
                if (s.Nodes.Count != expectedScenario.Count)
                {
                    continue;
                }

                bool nodesEqual = false;

                for (int i = 0; i < s.Nodes.Count; i++)
                {
                    if (s.Nodes[i].StepDescription != expectedScenario[i].StepDescription
                        || s.Nodes[i].Identifier != expectedScenario[i].Identifier)
                    {
                        nodesEqual = false;
                        break;
                    }
                    else
                    {
                        nodesEqual = true;
                    }
                }

                if (nodesEqual)
                {
                    return true;
                }
            }

            return false;
        }
        
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
        
        /// <summary>
        /// Testing Graph A with CycleDepth = 1
        /// </summary>
        [Test]
        public void ScenarioMatrix_GraphA_CycleDepth1()
        {
            bool[,] matrixA = new bool[,]                       //// 1 2 3 4 5 6
              { { false, true,  false, false, false, false } ,  // 1 O X O O O O
              { false, false, true,  false, false, true } ,     // 2 O O X O O X
              { false, false, false, true,  true, false } ,     // 3 O O O X X O
              { false, false, false, false, false, false } ,    // 4 O O O O O O
              { false, true,  false, false, false, false } ,    // 5 O X O O O O
              { false, false, false, false, false, false }      // 6 O O O O O O
              };

            int cycleDepth = 1;

            Matrix<bool> m = new Matrix<bool>(matrixA);
            
            Node n1 = new Node("Schritt 1", new FlowIdentifier(FlowType.Basic, 1));
            Node n2 = new Node("Schritt 2", new FlowIdentifier(FlowType.Basic, 1));
            Node n3 = new Node("Schritt 3", new FlowIdentifier(FlowType.Basic, 1));
            Node n4 = new Node("Schritt 4", new FlowIdentifier(FlowType.Basic, 1));
            Node n5 = new Node("Schritt 5", new FlowIdentifier(FlowType.Basic, 1));
            Node n6 = new Node("Schritt 6", new FlowIdentifier(FlowType.Basic, 1));

            List<Node> nodes = new List<Node>();
            nodes.Add(n1);
            nodes.Add(n2);
            nodes.Add(n3);
            nodes.Add(n4);
            nodes.Add(n5);
            nodes.Add(n6);

            List<Node> expectedScenario1 = new List<Node>();
            expectedScenario1.Add(n1);
            expectedScenario1.Add(n2);
            expectedScenario1.Add(n6);
            
            List<Node> expectedScenario2 = new List<Node>();
            expectedScenario2.Add(n1);
            expectedScenario2.Add(n2);
            expectedScenario2.Add(n3);
            expectedScenario2.Add(n4);
            
            UseCase uc = new UseCase();
            uc.EdgeMatrix = m;
            uc.Nodes = nodes;

            ScenarioMatrix sm = new ScenarioMatrix(uc, cycleDepth);
            sm.CreateScenarios();
            List<Scenario> foundScenarios = sm.GetScenarios();

            Assert.AreEqual(foundScenarios.Count, 2);
            Assert.IsTrue(this.ContainsScenario(foundScenarios, expectedScenario1));
            Assert.IsTrue(this.ContainsScenario(foundScenarios, expectedScenario2));
        }

        /// <summary>
        /// Testing Graph A with CycleDepth = 2
        /// </summary>
        [Test]
        public void ScenarioMatrix_GraphA_CycleDepth2()
        {
            bool[,] matrixA = new bool[,]                       //// 1 2 3 4 5 6
              { { false, true,  false, false, false, false } ,  // 1 O X O O O O
              { false, false, true,  false, false, true } ,     // 2 O O X O O X
              { false, false, false, true,  true, false } ,     // 3 O O O X X O
              { false, false, false, false, false, false } ,    // 4 O O O O O O
              { false, true,  false, false, false, false } ,    // 5 O X O O O O
              { false, false, false, false, false, false }      // 6 O O O O O O
              };

            int cycleDepth = 2;

            Matrix<bool> m = new Matrix<bool>(matrixA);
            
            Node n1 = new Node("Schritt 1", new FlowIdentifier(FlowType.Basic, 1));
            Node n2 = new Node("Schritt 2", new FlowIdentifier(FlowType.Basic, 1));
            Node n3 = new Node("Schritt 3", new FlowIdentifier(FlowType.Basic, 1));
            Node n4 = new Node("Schritt 4", new FlowIdentifier(FlowType.Basic, 1));
            Node n5 = new Node("Schritt 5", new FlowIdentifier(FlowType.Basic, 1));
            Node n6 = new Node("Schritt 6", new FlowIdentifier(FlowType.Basic, 1));

            List<Node> nodes = new List<Node>();
            nodes.Add(n1);
            nodes.Add(n2);
            nodes.Add(n3);
            nodes.Add(n4);
            nodes.Add(n5);
            nodes.Add(n6);

            List<Node> expectedScenario1 = new List<Node>();
            expectedScenario1.Add(n1);
            expectedScenario1.Add(n2);
            expectedScenario1.Add(n6);

            List<Node> expectedScenario2 = new List<Node>();
            expectedScenario2.Add(n1);
            expectedScenario2.Add(n2);
            expectedScenario2.Add(n3);
            expectedScenario2.Add(n4);

            List<Node> expectedScenario3 = new List<Node>();
            expectedScenario3.Add(n1);
            expectedScenario3.Add(n2);
            expectedScenario3.Add(n3);
            expectedScenario3.Add(n5);
            expectedScenario3.Add(n2);
            expectedScenario3.Add(n3);
            expectedScenario3.Add(n4);

            List<Node> expectedScenario4 = new List<Node>();
            expectedScenario4.Add(n1);
            expectedScenario4.Add(n2);
            expectedScenario4.Add(n3);
            expectedScenario4.Add(n5);
            expectedScenario4.Add(n2);
            expectedScenario4.Add(n6);
            
            UseCase uc = new UseCase();
            uc.EdgeMatrix = m;
            uc.Nodes = nodes;

            ScenarioMatrix sm = new ScenarioMatrix(uc, cycleDepth);
            sm.CreateScenarios();
            List<Scenario> foundScenarios = sm.GetScenarios();

            Assert.AreEqual(foundScenarios.Count, 4);
            Assert.IsTrue(this.ContainsScenario(foundScenarios, expectedScenario1));
            Assert.IsTrue(this.ContainsScenario(foundScenarios, expectedScenario2));
            Assert.IsTrue(this.ContainsScenario(foundScenarios, expectedScenario3));
            Assert.IsTrue(this.ContainsScenario(foundScenarios, expectedScenario4));
        }


        /// <summary>
        /// Testing Graph B with CycleDepth = 1
        /// </summary>
        [Test]
        public void ScenarioMatrix_GraphB_CycleDepth1()
        {
            bool[,] matrixB = new bool[,]              //   1 2 3 4 5
            { { false, true,  false, false, false } ,  // 1 O X O O O
              { false, false, true,  true,  false } ,  // 2 O O X X O
              { false, true, false, false, true } ,    // 3 O X O O X
              { false, false, false, false, false } ,  // 4 O O O O O
              { false, false, false, false, false }    // 5 O O O O O
            };

            int cycleDepth = 1;

            Matrix<bool> m = new Matrix<bool>(matrixB);
            
            Node n1 = new Node("Schritt 1", new FlowIdentifier(FlowType.Basic, 1));
            Node n2 = new Node("Schritt 2", new FlowIdentifier(FlowType.Basic, 1));
            Node n3 = new Node("Schritt 3", new FlowIdentifier(FlowType.Basic, 1));
            Node n4 = new Node("Schritt 4", new FlowIdentifier(FlowType.Basic, 1));
            Node n5 = new Node("Schritt 5", new FlowIdentifier(FlowType.Basic, 1));

            List<Node> nodes = new List<Node>();
            nodes.Add(n1);
            nodes.Add(n2);
            nodes.Add(n3);
            nodes.Add(n4);
            nodes.Add(n5);

            List<Node> expectedScenario1 = new List<Node>();
            expectedScenario1.Add(n1);
            expectedScenario1.Add(n2);
            expectedScenario1.Add(n4);

            List<Node> expectedScenario2 = new List<Node>();
            expectedScenario2.Add(n1);
            expectedScenario2.Add(n2);
            expectedScenario2.Add(n3);
            expectedScenario2.Add(n5);

            UseCase uc = new UseCase();
            uc.EdgeMatrix = m;
            uc.Nodes = nodes;

            ScenarioMatrix sm = new ScenarioMatrix(uc, cycleDepth);
            sm.CreateScenarios();
            List<Scenario> foundScenarios = sm.GetScenarios();

            Assert.AreEqual(foundScenarios.Count, 2);
            Assert.IsTrue(this.ContainsScenario(foundScenarios, expectedScenario1));
            Assert.IsTrue(this.ContainsScenario(foundScenarios, expectedScenario2));
        }
        
        /// <summary>
        /// Testing Graph B with CycleDepth = 3
        /// </summary>
        [Test]
        public void ScenarioMatrix_GraphB_CycleDepth3()
        {

            bool[,] matrixB = new bool[,]              //   1 2 3 4 5
            { { false, true,  false, false, false } ,  // 1 O X O O O
              { false, false, true,  true,  false } ,  // 2 O O X X O
              { false, true, false, false, true } ,    // 3 O X O O X
              { false, false, false, false, false } ,  // 4 O O O O O
              { false, false, false, false, false }    // 5 O O O O O
            };

            int cycleDepth = 3;

            Matrix<bool> m = new Matrix<bool>(matrixB);

            Node n1 = new Node("Schritt 1", new FlowIdentifier(FlowType.Basic, 1));
            Node n2 = new Node("Schritt 2", new FlowIdentifier(FlowType.Basic, 1));
            Node n3 = new Node("Schritt 3", new FlowIdentifier(FlowType.Basic, 1));
            Node n4 = new Node("Schritt 4", new FlowIdentifier(FlowType.Basic, 1));
            Node n5 = new Node("Schritt 5", new FlowIdentifier(FlowType.Basic, 1));

            List<Node> nodes = new List<Node>();
            nodes.Add(n1);
            nodes.Add(n2);
            nodes.Add(n3);
            nodes.Add(n4);
            nodes.Add(n5);

            List<Node> expectedScenario1 = new List<Node>();
            expectedScenario1.Add(n1);
            expectedScenario1.Add(n2);
            expectedScenario1.Add(n4);

            List<Node> expectedScenario2 = new List<Node>();
            expectedScenario2.Add(n1);
            expectedScenario2.Add(n2);
            expectedScenario2.Add(n3);
            expectedScenario2.Add(n5);

            List<Node> expectedScenario3 = new List<Node>();
            expectedScenario3.Add(n1);
            expectedScenario3.Add(n2);
            expectedScenario3.Add(n3);
            expectedScenario3.Add(n2);
            expectedScenario3.Add(n3);
            expectedScenario3.Add(n5);

            List<Node> expectedScenario4 = new List<Node>();
            expectedScenario4.Add(n1);
            expectedScenario4.Add(n2);
            expectedScenario4.Add(n3);
            expectedScenario4.Add(n2);
            expectedScenario4.Add(n3);
            expectedScenario4.Add(n2);
            expectedScenario4.Add(n3);
            expectedScenario4.Add(n5);

            List<Node> expectedScenario5 = new List<Node>();
            expectedScenario5.Add(n1);
            expectedScenario5.Add(n2);
            expectedScenario5.Add(n3);
            expectedScenario5.Add(n2);
            expectedScenario5.Add(n4);

            List<Node> expectedScenario6 = new List<Node>();
            expectedScenario6.Add(n1);
            expectedScenario6.Add(n2);
            expectedScenario6.Add(n3);
            expectedScenario6.Add(n2);
            expectedScenario6.Add(n3);
            expectedScenario6.Add(n2);
            expectedScenario6.Add(n4);

            UseCase uc = new UseCase();
            uc.EdgeMatrix = m;
            uc.Nodes = nodes;

            ScenarioMatrix sm = new ScenarioMatrix(uc, cycleDepth);
            sm.CreateScenarios();
            List<Scenario> foundScenarios = sm.GetScenarios();

            Assert.AreEqual(foundScenarios.Count, 6);
            Assert.IsTrue(this.ContainsScenario(foundScenarios, expectedScenario1));
            Assert.IsTrue(this.ContainsScenario(foundScenarios, expectedScenario2));
            Assert.IsTrue(this.ContainsScenario(foundScenarios, expectedScenario3));
            Assert.IsTrue(this.ContainsScenario(foundScenarios, expectedScenario4));
            Assert.IsTrue(this.ContainsScenario(foundScenarios, expectedScenario5));
            Assert.IsTrue(this.ContainsScenario(foundScenarios, expectedScenario6));
        }
    }
}
