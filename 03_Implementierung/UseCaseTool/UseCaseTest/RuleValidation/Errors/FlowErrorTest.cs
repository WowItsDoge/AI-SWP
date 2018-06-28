// <copyright file="FlowErrorTest.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation.Errors
{
    using NUnit.Framework;
    using UseCaseCore.RuleValidation.Errors;

    /// <summary>
    /// Test class for the FlowError
    /// </summary>
    [TestFixture]
    public class FlowErrorTest
    {
        /// <summary>
        /// Produces a string, continuing all the information of the error.
        /// </summary>
        [Test]
        public void GetFlowErrorStringTest()
        {
            var errorMessage = "Das ist ein Fehler!";
            var resolveMessage = "Zur Lösung bitte das machen...";
            var referenceflow = 0;
            var flowError = new FlowError(referenceflow, resolveMessage, errorMessage);

            var expectedResult = "Fehler in Flow " + (referenceflow + 1) + ": " + errorMessage + "\t" + resolveMessage + "\r\n";

            Assert.IsTrue(flowError.ToString() == expectedResult);
        }
    }
}
