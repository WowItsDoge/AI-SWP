// <copyright file="RucmRuleValidatorTest.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation
{
    using NUnit.Framework;
    using RucmRules;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UseCaseCore.RuleValidation;
    using UseCaseCore.RuleValidation.RucmRules;
    using UseCaseCore.XmlParser;

    /// <summary>
    /// Test class for RUCMRuleValidator
    /// </summary>
    [TestFixture]
    public class RucmRuleValidatorTest
    {
        /// <summary>
        /// Validates one flow against all the RUCM-Rules. 
        /// </summary>
        [Test]
        public void ValidateTest()
        {
            var dummyBasicFlow = new BasicFlow();

            var dummy1 = new DummyRule(0);
            var dummy2 = new DummyRule(1);
            var dummy3 = new DummyRule(2);

            // Test validation with 3 rules and 3 errors
            var ruleList = new List<IRule> { dummy1, dummy2, dummy3 };
            var rucmRuleValidator = new RucmRuleValidator(ruleList);
            var result = rucmRuleValidator.Validate(dummyBasicFlow);

            Assert.IsFalse(result);
            Assert.IsTrue(rucmRuleValidator.GetErrorReport().GetErrorList.Count == 3);

            // Test validation with 2 rules and 2 errors
            ruleList = new List<IRule> { dummy1, dummy3 };
            rucmRuleValidator = new RucmRuleValidator(ruleList);
            result = rucmRuleValidator.Validate(dummyBasicFlow);
            Assert.IsFalse(result);
            Assert.IsTrue(rucmRuleValidator.GetErrorReport().GetErrorList.Count == 2);

            // Test validation with 1 rules and 0 errors
            ruleList = new List<IRule> { dummy1 };
            rucmRuleValidator = new RucmRuleValidator(ruleList);
            result = rucmRuleValidator.Validate(dummyBasicFlow);
            Assert.IsTrue(result);
            Assert.IsTrue(rucmRuleValidator.GetErrorReport().GetErrorList.Count == 0);
        }

        /// <summary>
        /// Tests the export funcion
        /// </summary>
        [Test]
        public void ExportTest()
        {
            // Erfolgreicher Export ohne Mängel
            var tempPath = this.GetTempFile();
            var ruleList = new List<IRule>();
            var rucmRuleValidator = new RucmRuleValidator(ruleList);
            var exportResult = rucmRuleValidator.Export(tempPath);

            try
            {
                Assert.IsTrue(exportResult);
                Assert.IsTrue(File.Exists(tempPath));
                Assert.IsTrue(File.ReadAllText(tempPath) == ErrorReport.ExportHeader + ErrorReport.EmptyErrorExportMessage);
            }
            finally
            {
                File.Delete(tempPath);
            }

            // Erfolgreicher Export mit externen Mängeln
            tempPath = this.GetTempFile();
            var fehlermeldung = "Das ist ein Fehler";
            rucmRuleValidator.AddExternalError(fehlermeldung);
            exportResult = rucmRuleValidator.Export(tempPath);

            try
            {
                Assert.IsTrue(exportResult);
                Assert.IsTrue(File.Exists(tempPath));
                Assert.IsTrue(File.ReadAllText(tempPath) == ErrorReport.ExportHeader + ErrorReport.GeneralErrorHeader + fehlermeldung + "\n");
            }
            finally
            {
                File.Delete(tempPath);
            }

            // Erfolgreicher Export mit Validierungsmängel
            tempPath = this.GetTempFile();
            var dummy = new DummyRule(2, 2, 2);
            var dummyBasicFlow = new BasicFlow();

            ruleList = new List<IRule> { dummy };
            rucmRuleValidator = new RucmRuleValidator(ruleList);
            rucmRuleValidator.Validate(dummyBasicFlow);
            exportResult = rucmRuleValidator.Export(tempPath);

            try
            {
                Assert.IsTrue(exportResult);
                Assert.IsTrue(File.Exists(tempPath));
                var expectedText = ErrorReport.ExportHeader + ErrorReport.GeneralErrorHeader + "Error #1\nError #2\n" + 
                    ErrorReport.FlowErrorHeader + "Error in Flow 1: Error #1\tLösung zu: 1\n" + "Error in Flow 2: Error #2\tLösung zu: 2\n" +
                    ErrorReport.StepErrorHeader + "Error in Step 1: Error #1\tLösung zu: 1\n" + "Error in Step 2: Error #2\tLösung zu: 2\n";
                Assert.IsTrue(File.ReadAllText(tempPath) == expectedText);
            }
            finally
            {
                File.Delete(tempPath);
            }

            // Erfolgreicher Export mit Dateiersetzung
            tempPath = this.GetTempFile();
            dummy = new DummyRule(2);
            ruleList = new List<IRule> { dummy };
            rucmRuleValidator = new RucmRuleValidator(ruleList);
            rucmRuleValidator.Validate(dummyBasicFlow);
            File.WriteAllText(tempPath, "Test");
            exportResult = rucmRuleValidator.Export(tempPath);

            try
            {
                Assert.IsTrue(exportResult);
                Assert.IsTrue(File.Exists(tempPath));
                Assert.IsTrue(File.ReadAllText(tempPath) == ErrorReport.ExportHeader + ErrorReport.GeneralErrorHeader + "Error #1\nError #2\n");
            }
            finally
            {
                File.Delete(tempPath);
            }

            // Fehlerhafter Export
            var invalidPath = "ABC:\\Invalid\\test.txt";
            exportResult = rucmRuleValidator.Export(invalidPath);

            Assert.IsFalse(exportResult);
        }

        /// <summary>
        /// Generates a temporary filename
        /// </summary>
        /// <returns>a temp file name</returns>
        private string GetTempFile()
        {
            return Path.Combine(Path.GetTempPath(), string.Format("BerichtExport_{0:HH_mm_ss_fff}.txt", DateTime.Now));
        }
    }
}
