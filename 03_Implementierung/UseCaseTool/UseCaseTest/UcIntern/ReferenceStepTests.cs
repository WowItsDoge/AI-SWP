// <copyright file="ReferenceStepTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
    using NUnit.Framework;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A class collecting all tests for the <see cref="ReferenceStep"/> struct.
    /// </summary>
    [TestFixture]
    public class ReferenceStepTests
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
            FlowIdentifier expectedIdentifier = this.flowIdentifier1;
            int expectedStep = 6;
            ReferenceStep rs = new ReferenceStep(expectedIdentifier, expectedStep);

            // Act

            // Assert
            Assert.AreEqual(expectedIdentifier, rs.Identifier);
            Assert.AreEqual(expectedStep, rs.Step);
        }

        /// <summary>
        /// Compares a reference step with an object instance.
        /// </summary>
        [Test]
        public void CompareReferenceStepToObject()
        {
            // Arrange
            ReferenceStep rs = new ReferenceStep();

            // Act

            // Assert
            Assert.IsFalse(rs.Equals(new object()));
        }

        //// <summary>
        /// Compares two reference steps that are equal using the standard constructor.
        /// </summary>
        [Test]
        public void CompareEqualNodesStandardConstructor()
        {
            // Arrange
            ReferenceStep rs1 = new ReferenceStep(),
                rs2 = new ReferenceStep();

            // Act

            // Assert
            Assert.IsTrue(rs1 == rs2);
            Assert.IsFalse(rs1 != rs2);
            Assert.IsTrue(rs1.Equals(rs2));
        }

        /// <summary>
        /// Compares two reference steps that are equal using the initializing constructor.
        /// </summary>
        [Test]
        public void CompareEqualReferenceStepsInitializingConstructor()
        {
            // Arrange
            ReferenceStep rs1 = new ReferenceStep(this.flowIdentifier1, 8),
                rs2 = new ReferenceStep(this.flowIdentifier1, 8);

            // Act

            // Assert
            Assert.IsTrue(rs1 == rs2);
            Assert.IsFalse(rs1 != rs2);
            Assert.IsTrue(rs1.Equals(rs2));
        }

        /// <summary>
        /// Compares two reference steps with different identifier.
        /// </summary>
        [Test]
        public void CompareReferenceStepsDifferentIdentifier()
        {
            // Arrange
            ReferenceStep rs1 = new ReferenceStep(this.flowIdentifier1, 8),
                rs2 = new ReferenceStep(this.flowIdentifier2, 8);

            // Act

            // Assert
            Assert.IsFalse(rs1 == rs2);
            Assert.IsTrue(rs1 != rs2);
            Assert.IsFalse(rs1.Equals(rs2));
        }

        /// <summary>
        /// Compares two reference steps with different step.
        /// </summary>
        [Test]
        public void CompareReferenceStepsDifferentStep()
        {
            // Arrange
            ReferenceStep rs1 = new ReferenceStep(this.flowIdentifier1, 8),
                rs2 = new ReferenceStep(this.flowIdentifier1, 4);

            // Act

            // Assert
            Assert.IsFalse(rs1 == rs2);
            Assert.IsTrue(rs1 != rs2);
            Assert.IsFalse(rs1.Equals(rs2));
        }

        /// <summary>
        /// Tests the hash of two differing node objects.
        /// </summary>
        [Test]
        public void GetHashOfDifferentObjects()
        {
            // Arrange
            ReferenceStep rs1 = new ReferenceStep(),
                rs2 = new ReferenceStep(this.flowIdentifier1, 4);

            // Act

            // Assert
            Assert.AreNotEqual(rs1.GetHashCode(), rs2.GetHashCode());
        }
    }
}
