// <copyright file="XmlStructureParserTest.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.XmlParser
{
    using DocumentFormat.OpenXml.Packaging;
    using NUnit.Framework;
    using System.Collections.Generic;
    using UseCaseCore.RuleValidation;
    using UseCaseCore.RuleValidation.RucmRules;
    using UseCaseCore.UcIntern;
    using UseCaseCore.XmlParser;
    using System;

    /// <summary>
    /// A class collecting all tests for the <see cref="Flow"/> class and its derived classes.
    /// </summary>
    [TestFixture]
    public class XmlStructureParserTest
    {
        //// Die auskommentieren Testfälle werden nichtmehr benötigt,
        //// da nun zugunsten der Programmuniversalität und -teilweiterverwendung
        //// statt den eigenen Flow-Klassen, die Flow-Klassen von "UcIntern" mitverwendet werden
        //// und in "UcIntern-Test" bereits Testfälle existieren

        /*
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
            ReferenceStep standardReferenceStep = new ReferenceStep();
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
            FlowIdentifier testFlowIdentifier = new FlowIdentifier();
            ReferenceStep testReferenceStep = new ReferenceStep();
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
            FlowIdentifier testFlowIdentifier = new FlowIdentifier();
            ReferenceStep testReferenceStep = new ReferenceStep();
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
        */

        /// <summary>
        /// Testfuction for GetError function
        /// </summary>
        [Test]
        public void GetErrorTest()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string result = string.Empty;

            // Act
            result = testxmlStructureParser.GetError();

            // Assert
            Assert.IsEmpty(result);
        }

        /// <summary>
        /// Testfuction for TryToFixMalformedXml function
        /// </summary>
        [Test]
        public void TryToFixMalformedXmlTest()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            bool result = false;

            // Act
            result = testxmlStructureParser.TryToFixMalformedXml();

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Testfuction for LoadXmlFile function
        /// Test with correct sample file
        /// </summary>
        [Test]
        public void LoadXmlFileTestWithCorrectXmlSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - Keine Fehler.docm";
            bool result = false;

            // Act
            result = testxmlStructureParser.LoadXmlFile(filePath);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Testfuction for ParseXmlFile function
        /// Test with correct sample file
        /// </summary>
        [Test]
        public void ParseXmlFileTestWithCorrectXmlSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - Keine Fehler - 2.docm";
            UseCase testUseCase = new UseCase();
            bool resultLoadXmlFile = false;
            bool resultParseXmlFile = false;

            // Act
            resultLoadXmlFile = testxmlStructureParser.LoadXmlFile(filePath);
            resultParseXmlFile = testxmlStructureParser.ParseXmlFile(out testUseCase);

            // Assert
            Assert.IsTrue(resultLoadXmlFile);
            Assert.IsTrue(resultParseXmlFile);
            Assert.IsNotNull(testUseCase);
        }

        /// <summary>
        /// Testfuction for ParseXmlFile function
        /// Test with correct sample file but with formatting changes
        /// </summary>
        [Test]
        public void ParseXmlFileTestWithCorrectXmlSampleFileButWithFormattingChanges()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - Keine Fehler aber mit Formatierungsänderungen.docm";
            UseCase testUseCase = new UseCase();
            bool resultLoadXmlFile = false;
            bool resultParseXmlFile = false;

            // Act
            resultLoadXmlFile = testxmlStructureParser.LoadXmlFile(filePath);
            resultParseXmlFile = testxmlStructureParser.ParseXmlFile(out testUseCase);

            // Assert
            Assert.IsTrue(resultLoadXmlFile);
            Assert.IsTrue(resultParseXmlFile);
            Assert.IsNotNull(testUseCase);
        }

        /// <summary>
        /// Testfuction for LoadXmlFile function
        /// Test with broken sample file
        /// </summary>
        [Test]
        public void LoadXmlFileTestWithBrokenXmlSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - XML-Struktur defekt.docm";
            bool result = false;

            // Act
            result = testxmlStructureParser.LoadXmlFile(filePath);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Testfuction for ParseXmlFile function
        /// Test with no basic flow sample file
        /// </summary>
        [Test]
        public void ParseXmlFileTestWithNoBasicFlowSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - Kein Basic Flow.docm";
            UseCase testUseCase = new UseCase();
            bool resultLoadXmlFile = false;
            bool resultParseXmlFile = false;

            // Act
            resultLoadXmlFile = testxmlStructureParser.LoadXmlFile(filePath);
            resultParseXmlFile = testxmlStructureParser.ParseXmlFile(out testUseCase);

            // Assert
            Assert.IsTrue(resultLoadXmlFile);
            Assert.IsFalse(resultParseXmlFile);
            Assert.IsNull(testUseCase);
        }

        /// <summary>
        /// Testfuction for ParseXmlFile function
        /// Test with two basic flow sample file
        /// </summary>
        [Test]
        public void ParseXmlFileTestWithTwoBasicFlowSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - Zwei Basic Flows.docm";
            UseCase testUseCase = new UseCase();
            bool resultLoadXmlFile = false;
            bool resultParseXmlFile = false;

            // Act
            resultLoadXmlFile = testxmlStructureParser.LoadXmlFile(filePath);
            resultParseXmlFile = testxmlStructureParser.ParseXmlFile(out testUseCase);

            // Assert
            Assert.IsTrue(resultLoadXmlFile);
            Assert.IsFalse(resultParseXmlFile);
            Assert.IsNull(testUseCase);
        }

        /// <summary>
        /// Testfuction for ParseXmlFile function
        /// Test with no use case name sample file
        /// </summary>
        [Test]
        public void ParseXmlFileTestWithNoUseCaseNameSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - Kein UseCase Name.docm";
            UseCase testUseCase = new UseCase();
            bool resultLoadXmlFile = false;
            bool resultParseXmlFile = false;

            // Act
            resultLoadXmlFile = testxmlStructureParser.LoadXmlFile(filePath);
            resultParseXmlFile = testxmlStructureParser.ParseXmlFile(out testUseCase);

            // Assert
            Assert.IsTrue(resultLoadXmlFile);
            Assert.IsFalse(resultParseXmlFile);
            Assert.IsNull(testUseCase);
        }

        /// <summary>
        /// Testfuction for ParseXmlFile function
        /// Test with two use case name sample file
        /// </summary>
        [Test]
        public void ParseXmlFileTestWithTwoUseCaseNameSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - Zwei UseCase Namen.docm";
            UseCase testUseCase = new UseCase();
            bool resultLoadXmlFile = false;
            bool resultParseXmlFile = false;

            // Act
            resultLoadXmlFile = testxmlStructureParser.LoadXmlFile(filePath);
            resultParseXmlFile = testxmlStructureParser.ParseXmlFile(out testUseCase);

            // Assert
            Assert.IsTrue(resultLoadXmlFile);
            Assert.IsFalse(resultParseXmlFile);
            Assert.IsNull(testUseCase);
        }

        /// <summary>
        /// Testfuction for ParseXmlFile function
        /// Test with one RFS BoundedAlternativeFlow sample file
        /// </summary>
        [Test]
        public void ParseXmlFileTestWithOneRfsBoundedAlternativeFlowSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - BoundedAlternativeFlow mit einem RFS.docm";
            UseCase testUseCase = new UseCase();
            bool resultLoadXmlFile = false;
            bool resultParseXmlFile = false;

            // Act
            resultLoadXmlFile = testxmlStructureParser.LoadXmlFile(filePath);
            resultParseXmlFile = testxmlStructureParser.ParseXmlFile(out testUseCase);

            // Assert
            Assert.IsTrue(resultLoadXmlFile);
            Assert.IsTrue(resultParseXmlFile);
            Assert.IsNotNull(testUseCase);
        }

        /// <summary>
        /// Testfuction for ParseXmlFile function
        /// Test with no flows sample file
        /// </summary>
        [Test]
        public void ParseXmlFileTestWithNoFlowsSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - Keine Flows.docm";
            UseCase testUseCase = new UseCase();
            bool resultLoadXmlFile = false;
            bool resultParseXmlFile = false;

            // Act
            resultLoadXmlFile = testxmlStructureParser.LoadXmlFile(filePath);
            resultParseXmlFile = testxmlStructureParser.ParseXmlFile(out testUseCase);

            // Assert
            Assert.IsTrue(resultLoadXmlFile);
            Assert.IsFalse(resultParseXmlFile);
            Assert.IsNull(testUseCase);
        }

        /// <summary>
        /// Testfuction for ParseXmlFile function
        /// Test with correct "worst case" sample file
        /// </summary>
        [Test]
        public void ParseXmlFileTestWithWorstCaseSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - Worst-Case Szenario.docm";
            UseCase testUseCase = new UseCase();
            bool resultLoadXmlFile = false;
            bool resultParseXmlFile = false;

            // Act
            resultLoadXmlFile = testxmlStructureParser.LoadXmlFile(filePath);
            resultParseXmlFile = testxmlStructureParser.ParseXmlFile(out testUseCase);

            // Assert
            Assert.IsTrue(resultLoadXmlFile);
            Assert.IsTrue(resultParseXmlFile);
            Assert.IsNotNull(testUseCase);
        }

        /// <summary>
        /// Testfuction for ParseXmlFile function
        /// Test with RUCM validation rule 19 failed sample file
        /// </summary>
        [Test]
        public void ParseXmlFileTestWithRucmValidationRule19FailedSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - Regel 19 verletzt.docm";
            UseCase testUseCase = new UseCase();
            bool resultLoadXmlFile = false;
            bool resultParseXmlFile = false;

            // Act
            resultLoadXmlFile = testxmlStructureParser.LoadXmlFile(filePath);
            resultParseXmlFile = testxmlStructureParser.ParseXmlFile(out testUseCase);

            // Assert
            Assert.IsTrue(resultLoadXmlFile);
            Assert.IsFalse(resultParseXmlFile);
            Assert.IsNotNull(testUseCase);
        }

        /// <summary>
        /// Testfuction for ParseXmlFile function
        /// Test with RUCM validation rule 23 failed sample file
        /// </summary>
        [Test]
        public void ParseXmlFileTestWithRucmValidationRule23FailedSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - Regel 23 verletzt.docm";
            UseCase testUseCase = new UseCase();
            bool resultLoadXmlFile = false;
            bool resultParseXmlFile = false;

            // Act
            resultLoadXmlFile = testxmlStructureParser.LoadXmlFile(filePath);
            resultParseXmlFile = testxmlStructureParser.ParseXmlFile(out testUseCase);

            // Assert
            Assert.IsTrue(resultLoadXmlFile);
            Assert.IsFalse(resultParseXmlFile);
            Assert.IsNotNull(testUseCase);
        }

        /// <summary>
        /// Testfuction for ParseXmlFile function
        /// Test with RUCM validation rule 26 failed sample file
        /// </summary>
        [Test]
        public void ParseXmlFileTestWithRucmValidationRule26FailedSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - Regel 26 verletzt.docm";
            UseCase testUseCase = new UseCase();
            bool resultLoadXmlFile = false;
            bool resultParseXmlFile = false;

            // Act
            resultLoadXmlFile = testxmlStructureParser.LoadXmlFile(filePath);
            resultParseXmlFile = testxmlStructureParser.ParseXmlFile(out testUseCase);

            // Assert
            Assert.IsTrue(resultLoadXmlFile);
            Assert.IsFalse(resultParseXmlFile);
            Assert.IsNotNull(testUseCase);
        }

        /// <summary>
        /// Testfuction for ParseXmlFile function
        /// Test with RUCM validation rule 19 and 26 failed sample file
        /// </summary>
        [Test]
        public void ParseXmlFileTestWithRucmValidationRule19_26FailedSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - Regel 19 und 26 verletzt.docm";
            UseCase testUseCase = new UseCase();
            bool resultLoadXmlFile = false;
            bool resultParseXmlFile = false;

            // Act
            resultLoadXmlFile = testxmlStructureParser.LoadXmlFile(filePath);
            resultParseXmlFile = testxmlStructureParser.ParseXmlFile(out testUseCase);

            // Assert
            Assert.IsTrue(resultLoadXmlFile);
            Assert.IsFalse(resultParseXmlFile);
            Assert.IsNotNull(testUseCase);
        }

        /// <summary>
        /// Testfuction for ParseXmlFile function
        /// Test with RUCM validation rule 24 and 25 failed sample file
        /// </summary>
        [Test]
        public void ParseXmlFileTestWithRucmValidationRule24_25FailedSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - Regel 24 und 25 verletzt.docm";
            UseCase testUseCase = new UseCase();
            bool resultLoadXmlFile = false;
            bool resultParseXmlFile = false;

            // Act
            resultLoadXmlFile = testxmlStructureParser.LoadXmlFile(filePath);
            resultParseXmlFile = testxmlStructureParser.ParseXmlFile(out testUseCase);

            // Assert
            Assert.IsTrue(resultLoadXmlFile);
            Assert.IsFalse(resultParseXmlFile);
            Assert.IsNotNull(testUseCase);
        }

        /// <summary>
        /// Testfuction for ParseXmlFile function
        /// Test with RUCM validation rule 19, 23, 24, 25 and 26 failed sample file
        /// </summary>
        [Test]
        public void ParseXmlFileTestWithRucmValidationRule19_23_24_25_26FailedSampleFile()
        {
            // Arrange
            IRucmRuleValidator testrucmRuleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            XmlStructureParser testxmlStructureParser = new XmlStructureParser(testrucmRuleValidator);
            string filePath = "UseCaseTest\\XmlParser\\Testdateien\\UseCaseBeispiel - Regel 19, 23, 24, 25 und 26 verletzt.docm";
            UseCase testUseCase = new UseCase();
            bool resultLoadXmlFile = false;
            bool resultParseXmlFile = false;

            // Act
            resultLoadXmlFile = testxmlStructureParser.LoadXmlFile(filePath);
            resultParseXmlFile = testxmlStructureParser.ParseXmlFile(out testUseCase);

            // Assert
            Assert.IsTrue(resultLoadXmlFile);
            Assert.IsFalse(resultParseXmlFile);
            Assert.IsNotNull(testUseCase);
        }

    }
}
