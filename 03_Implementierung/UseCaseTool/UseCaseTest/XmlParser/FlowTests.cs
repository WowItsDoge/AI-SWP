// <copyright file="FlowTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.XmlParser
{
    using NUnit.Framework;
    using System.Collections.Generic;
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

        /// <summary>
        /// Creates a new basic flow instance.
        /// Test with specified test data.
        /// </summary>
        [Test]
        public void BasicFlowSpecifiedTestDataTest()
        {
            // Arrange
            List<string> testBasicSteps = new List<string>();
            testBasicSteps.Add("INCLUDE USE CASE Validate PIN.");
            testBasicSteps.Add("ATM customer selects Withdrawal through the system");
            testBasicSteps.Add("ATM customer enters the withdrawal amount through the system.");
            testBasicSteps.Add("ATM customer selects the account number through the system.");
            testBasicSteps.Add("The system VALIDATES THAT the account number is valid.");
            testBasicSteps.Add("The system VALIDATES THAT ATM customer has enough funds in the account.");
            testBasicSteps.Add("The system VALIDATES THAT the withdrawal amount does not exceed the daily limit of the account.");
            testBasicSteps.Add("The system VALIDATES THAT the ATM has enough funds.");
            testBasicSteps.Add("The system dispenses the cash amount.");
            testBasicSteps.Add("The system prints a receipt showing transaction number, transaction type, amount withdrawn, and account balance.");
            testBasicSteps.Add("The system ejects the ATM card.");
            testBasicSteps.Add("The system displays Welcome message.");
            string testPostcondition = "ATM customer funds have been withdrawn.";
            BasicFlow testBasicFlow = new BasicFlow();

            // Act
            for (int i = 0; i < testBasicSteps.Count; i++)
            {
                testBasicFlow.AddStep(testBasicSteps[i]);
            }
            testBasicFlow.SetPostcondition(testPostcondition);

            // Assert
            Assert.AreEqual(testBasicSteps, testBasicFlow.GetSteps());
            Assert.AreEqual(testPostcondition, testBasicFlow.GetPostcondition());
        }

        /// <summary>
        /// Creates a new bounded alternative flow instance.
        /// Test with specified test data.
        /// </summary>
        [Test]
        public void BoundedAlternativeFlowSpecifiedTestDataTest()
        {
            // Arrange
            int testBoundedAlternativeFlowsCount = 3;
            List<int> testReferenceStepIds = new List<int>();
            testReferenceStepIds.Add(5);
            testReferenceStepIds.Add(6);
            testReferenceStepIds.Add(7);
            List<string> testBoundedAlternativeFlowSteps = new List<string>();
            testBoundedAlternativeFlowSteps.Add("The system displays an apology message MEANWHILE the system ejects the ATM card.");
            testBoundedAlternativeFlowSteps.Add("The system shuts down.");
            testBoundedAlternativeFlowSteps.Add("ABORT.");
            string testPostcondition = "ATM customer funds have not been withdrawn. The system is shut down.";
            BoundedAlternativeFlow testBoundedAlternativeFlow = null;
            FlowIdentifier testFlowIdentifier = null;
            ReferenceStep testReferenceStep = null;
            List<BoundedAlternativeFlow> testBoundedAlternativeFlows = new List<BoundedAlternativeFlow>();
            List<ReferenceStep> testReferenceSteps = new List<ReferenceStep>();

            // Act
            for (int i = 0; i < testBoundedAlternativeFlowsCount; i++)
            {
                testBoundedAlternativeFlow = new BoundedAlternativeFlow();
                testFlowIdentifier = new FlowIdentifier(FlowType.BoundedAlternative, i + 1);
                testReferenceStep = new ReferenceStep(testFlowIdentifier, testReferenceStepIds[i]);
                for (int n = 0; n < testBoundedAlternativeFlowSteps.Count; n++)
                {
                    testBoundedAlternativeFlow.AddStep(testBoundedAlternativeFlowSteps[n]);
                }
                testBoundedAlternativeFlow.SetPostcondition(testPostcondition);
                testBoundedAlternativeFlow.SetId(i + 1);
                testBoundedAlternativeFlow.AddReferenceStep(testReferenceStep);
                testBoundedAlternativeFlows.Add(testBoundedAlternativeFlow);
                testReferenceSteps.Add(testReferenceStep);
            }

            // Assert
            for (int i = 0; i < testBoundedAlternativeFlowsCount; i++)
            {
                Assert.AreEqual(testBoundedAlternativeFlowSteps, testBoundedAlternativeFlows[i].GetSteps());
                Assert.AreEqual(testPostcondition, testBoundedAlternativeFlows[i].GetPostcondition());
                Assert.AreEqual(i + 1, testBoundedAlternativeFlows[i].GetId());
                Assert.AreEqual(testReferenceSteps[i], testBoundedAlternativeFlows[i].GetReferenceSteps()[0]);
            }
        }

        /// <summary>
        /// Creates a new global alternative flow instance.
        /// Test with specified test data.
        /// </summary>
        [Test]
        public void GlobalAlternativeFlowSpecifiedTestDataTest()
        {
            // Arrange
            int testGlobalAlternativeFlowsCount = 3;
            List<string> testGlobalAlternativeFlowSteps = new List<string>();
            testGlobalAlternativeFlowSteps.Add("IF ATM customer enters Cancel THEN");
            testGlobalAlternativeFlowSteps.Add("The system cancels the transaction MEANWHILE the system ejects the ATM card.");
            testGlobalAlternativeFlowSteps.Add("ABORT.");
            testGlobalAlternativeFlowSteps.Add("ENDIF");
            string testPostcondition = "ATM customer funds have not been withdrawn. The system is idle. The system is displaying a Welcome message.";
            GlobalAlternativeFlow testGlobalAlternativeFlow = null;
            List<GlobalAlternativeFlow> testGlobalAlternativeFlows = new List<GlobalAlternativeFlow>();

            // Act
            for (int i = 0; i < testGlobalAlternativeFlowsCount; i++)
            {
                testGlobalAlternativeFlow = new GlobalAlternativeFlow();
                for (int n = 0; n < testGlobalAlternativeFlowSteps.Count; n++)
                {
                    testGlobalAlternativeFlow.AddStep(testGlobalAlternativeFlowSteps[n]);
                }
                testGlobalAlternativeFlow.SetPostcondition(testPostcondition);
                testGlobalAlternativeFlow.SetId(i + 1);
                testGlobalAlternativeFlows.Add(testGlobalAlternativeFlow);
            }

            // Assert
            for (int i = 0; i < testGlobalAlternativeFlowsCount; i++)
            {
                Assert.AreEqual(testGlobalAlternativeFlowSteps, testGlobalAlternativeFlows[i].GetSteps());
                Assert.AreEqual(testPostcondition, testGlobalAlternativeFlows[i].GetPostcondition());
                Assert.AreEqual(i + 1, testGlobalAlternativeFlows[i].GetId());
            }
        }

        /// <summary>
        /// Creates a new specific alternative flow instance.
        /// Test with specified test data.
        /// </summary>
        [Test]
        public void SpecificAlternativeFlowSpecifiedTestDataTest()
        {
            // Arrange
            int testSpecificAlternativeFlowsCount = 3;
            List<int> testReferenceStepIds = new List<int>();
            testReferenceStepIds.Add(8);
            testReferenceStepIds.Add(9);
            testReferenceStepIds.Add(10);
            List<string> testSpecificAlternativeFlowSteps = new List<string>();
            testSpecificAlternativeFlowSteps.Add("The system displays an apology message MEANWHILE the system ejects the ATM card.");
            testSpecificAlternativeFlowSteps.Add("ABORT.");
            string testPostcondition = "ATM customer funds have not been withdrawn. The system is idle.The system is displaying a Welcome message.";
            SpecificAlternativeFlow testSpecificAlternativeFlow = null;
            FlowIdentifier testFlowIdentifier = null;
            ReferenceStep testReferenceStep = null;
            List<SpecificAlternativeFlow> testSpecificAlternativeFlows = new List<SpecificAlternativeFlow>();
            List<ReferenceStep> testReferenceSteps = new List<ReferenceStep>();

            // Act
            for (int i = 0; i < testSpecificAlternativeFlowsCount; i++)
            {
                testSpecificAlternativeFlow = new SpecificAlternativeFlow();
                testFlowIdentifier = new FlowIdentifier(FlowType.SpecificAlternative, i + 1);
                testReferenceStep = new ReferenceStep(testFlowIdentifier, testReferenceStepIds[i]);
                for (int n = 0; n < testSpecificAlternativeFlowSteps.Count; n++)
                {
                    testSpecificAlternativeFlow.AddStep(testSpecificAlternativeFlowSteps[n]);
                }
                testSpecificAlternativeFlow.SetPostcondition(testPostcondition);
                testSpecificAlternativeFlow.AddReferenceStep(testReferenceStep);
                testSpecificAlternativeFlows.Add(testSpecificAlternativeFlow);
                testReferenceSteps.Add(testReferenceStep);
            }

            // Assert
            for (int i = 0; i < testSpecificAlternativeFlowsCount; i++)
            {
                Assert.AreEqual(testSpecificAlternativeFlowSteps, testSpecificAlternativeFlows[i].GetSteps());
                Assert.AreEqual(testPostcondition, testSpecificAlternativeFlows[i].GetPostcondition());
                Assert.AreEqual(testReferenceSteps[i], testSpecificAlternativeFlows[i].GetReferenceStep());
            }
        }
    }
}
