// <copyright file="ExternalEdgeTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
    using NUnit.Framework;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A class collecting all tests for the <see cref="ExternalEdge"/> struct.
    /// </summary>
    [TestFixture]
    public class ExternalEdgeTests
    {
        private FlowIdentifier flowIdentifier1, flowIdentifier2;

        private ReferenceStep referenceStep1, referenceStep2;

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
        }

        /// <summary>
        /// Sets all fields of the struct and reads them.
        /// </summary>
        [Test]
        public void SetFields()
        {
            // Arrange
            int expectedSourceStepNumber = 15;
            ReferenceStep expectedReferenceStep = this.referenceStep1;
            ExternalEdge ee = new ExternalEdge(expectedSourceStepNumber, expectedReferenceStep);

            // Act

            // Assert
            Assert.AreEqual(expectedSourceStepNumber, ee.SourceStepNumber);
            Assert.AreEqual(expectedReferenceStep, ee.TargetStep);
        }

        /// <summary>
        /// Compares an external edge with an object instance.
        /// </summary>
        [Test]
        public void CompareExternalEdgeToObject()
        {
            // Arrange
            ExternalEdge ee = new ExternalEdge();

            // Act

            // Assert
            Assert.IsFalse(ee.Equals(new object()));
        }

        /// <summary>
        /// Compares two external edges that are equal using the standard constructor.
        /// </summary>
        [Test]
        public void CompareExternalEdgesStandardConstructor()
        {
            // Arrange
            ExternalEdge ee1 = new ExternalEdge(),
                ee2 = new ExternalEdge();

            // Act

            // Assert
            Assert.IsTrue(ee1 == ee2);
            Assert.IsFalse(ee1 != ee2);
            Assert.IsTrue(ee1.Equals(ee2));
        }

        /// <summary>
        /// Compares two external edges that are equal using the initializing constructor.
        /// </summary>
        [Test]
        public void CompareEqualExternalEdgesInitializingConstructor()
        {
            // Arrange
            ExternalEdge ee1 = new ExternalEdge(6, this.referenceStep1),
                ee2 = new ExternalEdge(6, this.referenceStep1);

            // Act

            // Assert
            Assert.IsTrue(ee1 == ee2);
            Assert.IsFalse(ee1 != ee2);
            Assert.IsTrue(ee1.Equals(ee2));
        }

        /// <summary>
        /// Compares two external edges with different source step number.
        /// </summary>
        [Test]
        public void CompareExternalEdgesDifferentSourceStepNumber()
        {
            // Arrange
            ExternalEdge ee1 = new ExternalEdge(6, this.referenceStep1),
                ee2 = new ExternalEdge(9, this.referenceStep1);

            // Act

            // Assert
            Assert.IsFalse(ee1 == ee2);
            Assert.IsTrue(ee1 != ee2);
            Assert.IsFalse(ee1.Equals(ee2));
        }

        /// <summary>
        /// Compares two external edges with different target step.
        /// </summary>
        [Test]
        public void CompareExternalEdgesDifferentTargetStep()
        {
            // Arrange
            ExternalEdge ee1 = new ExternalEdge(6, this.referenceStep1),
                ee2 = new ExternalEdge(6, this.referenceStep2);

            // Act

            // Assert
            Assert.IsFalse(ee1 == ee2);
            Assert.IsTrue(ee1 != ee2);
            Assert.IsFalse(ee1.Equals(ee2));
        }

        /// <summary>
        /// Tests the hash of two differing condition objects.
        /// </summary>
        [Test]
        public void GetHashOfDifferentObjects()
        {
            // Arrange
            ExternalEdge ee1 = new ExternalEdge(),
                ee2 = new ExternalEdge(3, this.referenceStep1);

            // Act

            // Assert
            Assert.AreNotEqual(ee1.GetHashCode(), ee2.GetHashCode());
        }

        /// <summary>
        /// Creates an external edge and tests if the method for creating a new instance with incremented value works.
        /// </summary>
        [Test]
        public void GetNewExternalEdgeWithIncrementedSourceStepNumber()
        {
            // Arrange
            int oldValue = 3,
                incrementValue = 53,
                expectedValue = oldValue + incrementValue;
            ExternalEdge ee = new ExternalEdge(oldValue, this.referenceStep1);

            // Act
            ExternalEdge newExtenalEdge = ee.NewWithIncreasedSourceStepNumber(incrementValue);

            // Assert
            Assert.AreEqual(expectedValue, newExtenalEdge.SourceStepNumber);
        }
    }
}
