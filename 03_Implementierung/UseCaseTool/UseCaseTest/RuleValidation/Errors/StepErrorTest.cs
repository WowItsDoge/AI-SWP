// <copyright file="StepErrorTest.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation.Errors
{
    using NUnit.Framework;
    using UseCaseCore.RuleValidation.Errors;

    /// <summary>
    /// Test class for the StepError
    /// </summary>
    [TestFixture]
    public class StepErrorTest
    {
        /// <summary>
        /// Produces a string, continuing all the information of the error.
        /// </summary>
        [Test]
        public void GetStepErrorStringTest()
        {
            var errorMessage = "Das ist ein Fehler!";
            var resolveMessage = "Zur Lösung bitte das machen...";
            var referenceStep = 1;
            var stepError = new StepError(referenceStep, resolveMessage, errorMessage);

            var expectedResult = "Fehler in Step " + referenceStep + ": " + errorMessage + "\t" + resolveMessage + "\r\n";

            Assert.IsTrue(stepError.ToString() == expectedResult);
        }
    }
}