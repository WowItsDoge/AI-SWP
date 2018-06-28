// <copyright file="NodeTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
    using NUnit.Framework;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A class collecting all tests for the <see cref="Node"/> struct.
    /// </summary>
    [TestFixture]
    public class NodeTests
    {
        private FlowIdentifier flowIdentifier1, flowIdentifier2;

        /// <summary>
        /// Setup method for private variables.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            flowIdentifier1 = new FlowIdentifier(FlowType.Basic, 0);
            flowIdentifier2 = new FlowIdentifier(FlowType.SpecificAlternative, 1);
        }

        /// <summary>
        /// Sets all fields of the struct and reads them.
        /// </summary>
        [Test]
        public void SetFields()
        {
            // Arrange
            string expectedStepDescription = "Test";
            FlowIdentifier expectedIdentifier = this.flowIdentifier1;
            Node n = new Node(expectedStepDescription, expectedIdentifier);

            // Act

            // Assert
            Assert.AreEqual(expectedStepDescription, n.StepDescription);
            Assert.AreEqual(expectedIdentifier, n.Identifier);
        }

        /// <summary>
        /// Compares a node with an object instance.
        /// </summary>
        [Test]
        public void CompareNodeToObject()
        {
            // Arrange
            Node n = new Node();

            // Act

            // Assert
            Assert.IsFalse(n.Equals(new object()));
        }

        //// <summary>
        /// Compares two nodes that are equal using the standard constructor.
        /// </summary>
        [Test]
        public void CompareEqualNodesStandardConstructor()
        {
            // Arrange
            Node n1 = new Node(),
                n2 = new Node();

            // Act

            // Assert
            Assert.IsTrue(n1 == n2);
            Assert.IsFalse(n1 != n2);
            Assert.IsTrue(n1.Equals(n2));
        }

        /// <summary>
        /// Compares two nodes that are equal using the initializing constructor.
        /// </summary>
        [Test]
        public void CompareEqualNodesInitializingConstructor()
        {
            // Arrange
            Node n1 = new Node("0", this.flowIdentifier1),
                n2 = new Node("0", this.flowIdentifier1);

            // Act

            // Assert
            Assert.IsTrue(n1 == n2);
            Assert.IsFalse(n1 != n2);
            Assert.IsTrue(n1.Equals(n2));
        }

        /// <summary>
        /// Compares two nodes with different step description.
        /// </summary>
        [Test]
        public void CompareNodesDifferentType()
        {
            // Arrange
            Node n1 = new Node("1", this.flowIdentifier1),
                n2 = new Node("2", this.flowIdentifier1);

            // Act

            // Assert
            Assert.IsFalse(n1 == n2);
            Assert.IsTrue(n1 != n2);
            Assert.IsFalse(n1.Equals(n2));
        }

        /// <summary>
        /// Compares two nodes with different identifier.
        /// </summary>
        [Test]
        public void CompareNodesDifferentIdentifier()
        {
            // Arrange
            Node n1 = new Node("0", this.flowIdentifier1),
                n2 = new Node("0", this.flowIdentifier2);

            // Act

            // Assert
            Assert.IsFalse(n1 == n2);
            Assert.IsTrue(n1 != n2);
            Assert.IsFalse(n1.Equals(n2));
        }

        /// <summary>
        /// Tests the hash of two differing node objects.
        /// </summary>
        [Test]
        public void GetHashOfDifferentObjects()
        {
            // Arrange
            Node n1 = new Node(),
                n2 = new Node("Test", this.flowIdentifier1);

            // Act

            // Assert
            Assert.AreNotEqual(n1.GetHashCode(), n2.GetHashCode());
        }
    }
}
