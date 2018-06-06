// <copyright file="ConditionTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
    using NUnit.Framework;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A class collecting all tests for the <see cref="Condition"/> struct.
    /// </summary>
    [TestFixture]
    public class ConditionTests
    {
        /// <summary>
        /// Sets all fields of the struct and reads them.
        /// </summary>
        [Test]
        public void SetFields()
        {
            // Arrange
            string expectedConditionText = "Test";
            bool expectedConditionState = true;
            Condition c = new Condition(expectedConditionText, expectedConditionState);

            // Act

            // Assert
            Assert.AreEqual(expectedConditionText, c.ConditionText);
            Assert.AreEqual(expectedConditionState, c.ConditionState);
        }

        /// <summary>
        /// Compares a condition with an object instance.
        /// </summary>
        [Test]
        public void CompareConditionToObject()
        {
            // Arrange
            Condition c = new Condition();

            // Act

            // Assert
            Assert.IsFalse(c.Equals(new object()));
        }

        /// <summary>
        /// Compares two conditions that are equal using the standard constructor.
        /// </summary>
        [Test]
        public void CompareEqualConditionsStandardConstructor()
        {
            // Arrange
            Condition c1 = new Condition(),
                c2 = new Condition();

            // Act

            // Assert
            Assert.IsTrue(c1 == c2);
            Assert.IsFalse(c1 != c2);
            Assert.IsTrue(c1.Equals(c2));
        }

        /// <summary>
        /// Compares two conditions that are equal using the initializing constructor.
        /// </summary>
        [Test]
        public void CompareEqualConditionsInitializingConstructor()
        {
            // Arrange
            Condition c1 = new Condition("0", true),
                c2 = new Condition("0", true);

            // Act

            // Assert
            Assert.IsTrue(c1 == c2);
            Assert.IsFalse(c1 != c2);
            Assert.IsTrue(c1.Equals(c2));
        }

        /// <summary>
        /// Compares two conditions with different condition text.
        /// </summary>
        [Test]
        public void CompareConditionsDifferentConditionText()
        {
            // Arrange
            Condition c1 = new Condition("1", true),
                c2 = new Condition("2", true);

            // Act

            // Assert
            Assert.IsFalse(c1 == c2);
            Assert.IsTrue(c1 != c2);
            Assert.IsFalse(c1.Equals(c2));
        }

        /// <summary>
        /// Compares two conditions with different condition text.
        /// </summary>
        [Test]
        public void CompareConditionsDifferentConditionState()
        {
            // Arrange
            Condition c1 = new Condition("0", false),
                c2 = new Condition("0", true);

            // Act

            // Assert
            Assert.IsFalse(c1 == c2);
            Assert.IsTrue(c1 != c2);
            Assert.IsFalse(c1.Equals(c2));
        }

        /// <summary>
        /// Returns the hash of two differing condition objects.
        /// </summary>
        [Test]
        public void GetHashOfDifferentObjects()
        {
            // Arrange
            Condition c1 = new Condition(),
                c2 = new Condition("2", true);

            // Act

            // Assert
            Assert.AreNotEqual(c1.GetHashCode(), c2.GetHashCode());
        }
    }
}
