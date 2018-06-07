// <copyright file="FlowIdentifierTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
    using NUnit.Framework;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A class collecting all tests for the <see cref="Flow"/> struct.
    /// </summary>
    [TestFixture]
    public class FlowIdentifierTests
    {
        /// <summary>
        /// Sets all fields of the struct and reads them.
        /// </summary>
        [Test]
        public void SetFields()
        {
            // Arrange
            FlowType expectedType = FlowType.BoundedAlternative;
            int expectedId = 3;
            FlowIdentifier fi = new FlowIdentifier(expectedType, expectedId);

            // Act

            // Assert
            Assert.AreEqual(expectedType, fi.Type);
            Assert.AreEqual(expectedId, fi.Id);
        }

        /// <summary>
        /// Compares a flow identifier with an object instance.
        /// </summary>
        [Test]
        public void CompareFlowIdentifierToObject()
        {
            // Arrange
            FlowIdentifier fi = new FlowIdentifier();

            // Act

            // Assert
            Assert.IsFalse(fi.Equals(new object()));
        }

        //// <summary>
        /// Compares two flow identifiers that are equal using the standard constructor.
        /// </summary>
        [Test]
        public void CompareEqualFlowIdentifiersStandardConstructor()
        {
            // Arrange
            FlowIdentifier fi1 = new FlowIdentifier(),
                fi2 = new FlowIdentifier();

            // Act

            // Assert
            Assert.IsTrue(fi1 == fi2);
            Assert.IsFalse(fi1 != fi2);
            Assert.IsTrue(fi1.Equals(fi2));
        }

        /// <summary>
        /// Compares two flow identifiers that are equal using the initializing constructor.
        /// </summary>
        [Test]
        public void CompareEqualFlowIdentifiersInitializingConstructor()
        {
            // Arrange
            FlowIdentifier fi1 = new FlowIdentifier(FlowType.Basic, 0),
                fi2 = new FlowIdentifier(FlowType.Basic, 0);

            // Act

            // Assert
            Assert.IsTrue(fi1 == fi2);
            Assert.IsFalse(fi1 != fi2);
            Assert.IsTrue(fi1.Equals(fi2));
        }

        /// <summary>
        /// Compares two flow identifiers with different type.
        /// </summary>
        [Test]
        public void CompareFlowIdentifiersDifferentType()
        {
            // Arrange
            FlowIdentifier fi1 = new FlowIdentifier(FlowType.Basic, 0),
                fi2 = new FlowIdentifier(FlowType.BoundedAlternative, 0);

            // Act

            // Assert
            Assert.IsFalse(fi1 == fi2);
            Assert.IsTrue(fi1 != fi2);
            Assert.IsFalse(fi1.Equals(fi2));
        }

        /// <summary>
        /// Compares two flow identifiers with different id.
        /// </summary>
        [Test]
        public void CompareFlowIdentifiersDifferentId()
        {
            // Arrange
            FlowIdentifier fi1 = new FlowIdentifier(FlowType.Basic, 0),
                fi2 = new FlowIdentifier(FlowType.Basic, 1);

            // Act

            // Assert
            Assert.IsFalse(fi1 == fi2);
            Assert.IsTrue(fi1 != fi2);
            Assert.IsFalse(fi1.Equals(fi2));
        }

        /// <summary>
        /// Returns the hash of two differing flow identifier objects.
        /// </summary>
        [Test]
        public void GetHashOfDifferentObjects()
        {
            // Arrange
            FlowIdentifier fi1 = new FlowIdentifier(),
                fi2 = new FlowIdentifier(FlowType.SpecificAlternative, 4);

            // Act

            // Assert
            Assert.AreNotEqual(fi1.GetHashCode(), fi2.GetHashCode());
        }
    }
}
