// <copyright file="InternalEdgeTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
    using NUnit.Framework;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A class collecting all tests for the <see cref="InternalEdge"/> struct.
    /// </summary>
    [TestFixture]
    public class InternalEdgeTests
    {
        /// <summary>
        /// Sets all fields of the struct and reads them.
        /// </summary>
        [Test]
        public void SetFields()
        {
            // Arrange
            int expectedSourceStep = 15;
            int expectedTargetStep = 17;
            InternalEdge ie = new InternalEdge(expectedSourceStep, expectedTargetStep);

            // Act

            // Assert
            Assert.AreEqual(expectedSourceStep, ie.SourceStep);
            Assert.AreEqual(expectedTargetStep, ie.TargetStep);
        }

        /// <summary>
        /// Compares an internal edge with an object instance.
        /// </summary>
        [Test]
        public void CompareInternalEdgeToObject()
        {
            // Arrange
            InternalEdge ie = new InternalEdge();

            // Act

            // Assert
            Assert.IsFalse(ie.Equals(new object()));
        }

        /// <summary>
        /// Compares two internal edges that are equal using the standard constructor.
        /// </summary>
        [Test]
        public void CompareInternalEdgesStandardConstructor()
        {
            // Arrange
            InternalEdge ie1 = new InternalEdge(),
                ie2 = new InternalEdge();

            // Act

            // Assert
            Assert.IsTrue(ie1 == ie2);
            Assert.IsFalse(ie1 != ie2);
            Assert.IsTrue(ie1.Equals(ie2));
        }

        /// <summary>
        /// Compares two internal edges that are equal using the initializing constructor.
        /// </summary>
        [Test]
        public void CompareEqualInternalEdgesInitializingConstructor()
        {
            // Arrange
            InternalEdge ie1 = new InternalEdge(6, 3),
                ie2 = new InternalEdge(6, 3);

            // Act

            // Assert
            Assert.IsTrue(ie1 == ie2);
            Assert.IsFalse(ie1 != ie2);
            Assert.IsTrue(ie1.Equals(ie2));
        }

        /// <summary>
        /// Compares two internal edges with different source step.
        /// </summary>
        [Test]
        public void CompareInternalEdgesDifferentSourceStep()
        {
            // Arrange
            InternalEdge ie1 = new InternalEdge(6, 2),
                ie2 = new InternalEdge(9, 2);

            // Act

            // Assert
            Assert.IsFalse(ie1 == ie2);
            Assert.IsTrue(ie1 != ie2);
            Assert.IsFalse(ie1.Equals(ie2));
        }

        /// <summary>
        /// Compares two internal edges with different target step.
        /// </summary>
        [Test]
        public void CompareInternalEdgesDifferentTargetStep()
        {
            // Arrange
            InternalEdge ie1 = new InternalEdge(6, 2),
                ie2 = new InternalEdge(6, 9);

            // Act

            // Assert
            Assert.IsFalse(ie1 == ie2);
            Assert.IsTrue(ie1 != ie2);
            Assert.IsFalse(ie1.Equals(ie2));
        }

        /// <summary>
        /// Tests the hash of two differing condition objects.
        /// </summary>
        [Test]
        public void GetHashOfDifferentObjects()
        {
            // Arrange
            InternalEdge ie1 = new InternalEdge(),
                ie2 = new InternalEdge(3, 8);

            // Act

            // Assert
            Assert.AreNotEqual(ie1.GetHashCode(), ie2.GetHashCode());
        }

        /// <summary>
        /// Creates an internal edge and tests if the method for creating a new instance with incremented value works.
        /// </summary>
        [Test]
        public void GetNewInternalEdgeWithIncrementedSourceTargetStep()
        {
            // Arrange
            int oldSourceValue = -1,
                oldTargetValue = 9,
                incrementValue = 9,
                expectedSourceValue = oldSourceValue + incrementValue,
                expectedTargetValue = oldTargetValue + incrementValue;
            InternalEdge ie = new InternalEdge(oldSourceValue, oldTargetValue);

            // Act
            InternalEdge newIntenalEdge = ie.NewWithIncreasedSourceTargetStep(incrementValue);

            // Assert
            Assert.AreEqual(expectedSourceValue, newIntenalEdge.SourceStep);
            Assert.AreEqual(expectedTargetValue, newIntenalEdge.TargetStep);
        }
    }
}
