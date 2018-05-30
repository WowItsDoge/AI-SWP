// <copyright file="FlowTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.XmlParser
{
    using NUnit.Framework;
    using UseCaseCore.UcIntern;
    using UseCaseCore.XmlParser;

    /// <summary>
    /// A class collecting all tests for the <see cref="Flow"/> class and its derived classes.
    /// </summary>
    [TestFixture]
    public class FlowTests
    {
        /// <summary>
        /// Creates a new basic flow instance.
        /// Tests the constructor.
        /// </summary>
        [Test]
        public void BasicFlowConstructorTest()
        {
            // Arrange
            string standardPostcondition = string.Empty;
            int emptyListCount = 0;
            BasicFlow testBasicFlow = new BasicFlow();

            // Act

            // Assert
            Assert.AreEqual(standardPostcondition, testBasicFlow.GetPostcondition());
            Assert.AreEqual(emptyListCount, testBasicFlow.GetSteps().Count);
        }

        /// <summary>
        /// Creates a new bounded alternative flow instance.
        /// Tests the constructor.
        /// </summary>
        [Test]
        public void BoundedAlternativeFlowConstructorTest()
        {
            // Arrange
            string standardPostcondition = string.Empty;
            int standardId = 0;
            int emptyStepListCount = 0;
            int emptyReferenceStepListCount = 0;
            BoundedAlternativeFlow testBoundedAlternativeFlow = new BoundedAlternativeFlow();

            // Act

            // Assert
            Assert.AreEqual(standardPostcondition, testBoundedAlternativeFlow.GetPostcondition());
            Assert.AreEqual(standardId, testBoundedAlternativeFlow.GetId());
            Assert.AreEqual(emptyStepListCount, testBoundedAlternativeFlow.GetSteps().Count);
            Assert.AreEqual(emptyReferenceStepListCount, testBoundedAlternativeFlow.GetReferenceSteps().Count);
        }

        /// <summary>
        /// Creates a new global alternative flow instance.
        /// Tests the constructor.
        /// </summary>
        [Test]
        public void GlobalAlternativeFlowConstructorTest()
        {
            // Arrange
            string standardPostcondition = string.Empty;
            int standardId = 0;
            int emptyStepListCount = 0;
            GlobalAlternativeFlow testGlobalAlternativeFlow = new GlobalAlternativeFlow();

            // Act

            // Assert
            Assert.AreEqual(standardPostcondition, testGlobalAlternativeFlow.GetPostcondition());
            Assert.AreEqual(standardId, testGlobalAlternativeFlow.GetId());
            Assert.AreEqual(emptyStepListCount, testGlobalAlternativeFlow.GetSteps().Count);
        }

        /// <summary>
        /// Creates a new specific alternative flow instance.
        /// Tests the constructor.
        /// </summary>
        [Test]
        public void SpecificAlternativeFlowConstructorTest()
        {
            // Arrange
            string standardPostcondition = string.Empty;
            int emptyListCount = 0;
            ReferenceStep standardReferenceStep = null;
            SpecificAlternativeFlow testSpecificAlternativeFlow = new SpecificAlternativeFlow();

            // Act

            // Assert
            Assert.AreEqual(standardPostcondition, testSpecificAlternativeFlow.GetPostcondition());
            Assert.AreEqual(emptyListCount, testSpecificAlternativeFlow.GetSteps().Count);
            Assert.AreEqual(standardReferenceStep, testSpecificAlternativeFlow.GetReferenceStep());
        }
    }
}
