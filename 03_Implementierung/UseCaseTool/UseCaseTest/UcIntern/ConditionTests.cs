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
        public void SetContent()
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
    }
}
