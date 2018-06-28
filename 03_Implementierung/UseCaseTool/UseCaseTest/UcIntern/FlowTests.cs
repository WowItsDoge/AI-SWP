// <copyright file="FlowTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A class collecting all tests for the <see cref="Flow"/> struct.
    /// </summary>
    [TestFixture]
    public class FlowTests
    {
        private FlowIdentifier flowIdentifier1, flowIdentifier2;

        private ReferenceStep referenceStep1, referenceStep2;

        private IReadOnlyList<Node> nodeList1, nodeList2;

        private IReadOnlyList<ReferenceStep> referenceStepList1, referenceStepList2;

        /// <summary>
        /// Setup method for private variables.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            flowIdentifier1 = new FlowIdentifier(FlowType.Basic, 0);
            flowIdentifier2 = new FlowIdentifier(FlowType.BoundedAlternative, 1);

            referenceStep1 = new ReferenceStep(flowIdentifier1, 0);
            referenceStep2 = new ReferenceStep(flowIdentifier2, 1);

            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("A node", flowIdentifier1));
            nodeList1 = nodes.AsReadOnly();

            nodes = new List<Node>();
            nodes.Add(new Node("Another node",flowIdentifier2));
            nodeList2 = nodes.AsReadOnly();

            List<ReferenceStep> referenceSteps = new List<ReferenceStep>();
            referenceSteps.Add(referenceStep1);
            referenceStepList1 = referenceSteps.AsReadOnly();

            referenceSteps = new List<ReferenceStep>();
            referenceSteps.Add(referenceStep2);
            referenceStepList2 = referenceSteps.AsReadOnly();
        }

        /// <summary>
        /// Sets all fields of the struct and reads them.
        /// </summary>
        [Test]
        public void SetFields()
        {
            // Arrange
            FlowIdentifier expectedFlowIdentifier = flowIdentifier1;
            string expectedPostcondition = "Test";
            IReadOnlyList<Node> expectedNodes = nodeList1;
            IReadOnlyList<ReferenceStep> expectedReferenceSteps = referenceStepList1;
            Flow f = new Flow(expectedFlowIdentifier, expectedPostcondition, expectedNodes, expectedReferenceSteps);

            // Act

            // Assert
            Assert.AreEqual(expectedFlowIdentifier, f.Identifier);
            Assert.AreEqual(expectedPostcondition, f.Postcondition);
            Assert.AreEqual(expectedNodes, f.Nodes);
            Assert.AreEqual(expectedReferenceSteps, f.ReferenceSteps);
        }

        /// <summary>
        /// Compares a flow with an object instance.
        /// </summary>
        [Test]
        public void CompareFlowToObject()
        {
            // Arrange
            Flow f = new Flow();

            // Act

            // Assert
            Assert.IsFalse(f.Equals(new object()));
        }

        /// <summary>
        /// Compares two flows that are equal using the standard constructor.
        /// </summary>
        [Test]
        public void CompareEqualFlowsStandardConstructor()
        {
            // Arrange
            Flow f1 = new Flow(),
                f2 = new Flow();

            // Act

            // Assert
            Assert.IsTrue(f1 == f2);
            Assert.IsFalse(f1 != f2);
            Assert.IsTrue(f1.Equals(f2));
        }

        /// <summary>
        /// Compares two flows that are equal using the initializing constructor.
        /// </summary>
        [Test]
        public void CompareEqualFlowsInitializingConstructor()
        {
            // Arrange
            Flow f1 = new Flow(flowIdentifier1, "Test", nodeList1, referenceStepList1),
                f2 = new Flow(flowIdentifier1, "Test", nodeList1, referenceStepList1);

            // Act

            // Assert
            Assert.IsTrue(f1 == f2);
            Assert.IsFalse(f1 != f2);
            Assert.IsTrue(f1.Equals(f2));
        }

        /// <summary>
        /// Compares two flows with different identifier.
        /// </summary>
        [Test]
        public void CompareConditionsDifferentIdentifier()
        {
            // Arrange
            Flow f1 = new Flow(flowIdentifier1, "Test", nodeList1, referenceStepList1),
                f2 = new Flow(flowIdentifier2, "Test", nodeList1, referenceStepList1);

            // Act

            // Assert
            Assert.IsFalse(f1 == f2);
            Assert.IsTrue(f1 != f2);
            Assert.IsFalse(f1.Equals(f2));
        }

        /// <summary>
        /// Compares two flows with different postcondition.
        /// </summary>
        [Test]
        public void CompareConditionsDifferentPostcondition()
        {
            // Arrange
            Flow f1 = new Flow(flowIdentifier1, "1", nodeList1, referenceStepList1),
                f2 = new Flow(flowIdentifier1, "2", nodeList1, referenceStepList1);

            // Act

            // Assert
            Assert.IsFalse(f1 == f2);
            Assert.IsTrue(f1 != f2);
            Assert.IsFalse(f1.Equals(f2));
        }

        /// <summary>
        /// Compares two flows with different nodes.
        /// </summary>
        [Test]
        public void CompareConditionsDifferentNodes()
        {
            // Arrange
            Flow f1 = new Flow(flowIdentifier1, "Test", nodeList1, referenceStepList1),
                f2 = new Flow(flowIdentifier1, "Test", nodeList2, referenceStepList1);

            // Act

            // Assert
            Assert.IsFalse(f1 == f2);
            Assert.IsTrue(f1 != f2);
            Assert.IsFalse(f1.Equals(f2));
        }

        /// <summary>
        /// Compares two flows with different reference steps.
        /// </summary>
        [Test]
        public void CompareConditionsDifferentReferenceSteps()
        {
            // Arrange
            Flow f1 = new Flow(flowIdentifier1, "Test", nodeList1, referenceStepList1),
                f2 = new Flow(flowIdentifier1, "Test", nodeList1, referenceStepList2);

            // Act

            // Assert
            Assert.IsFalse(f1 == f2);
            Assert.IsTrue(f1 != f2);
            Assert.IsFalse(f1.Equals(f2));
        }

        /// <summary>
        /// Tests the hash of two differing flow objects.
        /// </summary>
        [Test]
        public void GetHashOfDifferentObjects()
        {
            // Arrange
            Flow f1 = new Flow(flowIdentifier1, "Test", nodeList1, referenceStepList1),
                f2 = new Flow();

            // Act

            // Assert
            Assert.AreNotEqual(f1.GetHashCode(), f2.GetHashCode());
        }
    }
}
