// <copyright file="GeneralErrorTest.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.RuleValidation.Errors
{
    using NUnit.Framework;
    using UseCaseCore.RuleValidation.Errors;

    /// <summary>
    /// Test class for the GeneralError
    /// </summary>
    [TestFixture]
    public class GeneralErrorTest
    {
        /// <summary>
        /// Produces a string, continuing all the information of the error.
        /// </summary>
        [Test]
        public void GetGeneralErrorStringTest()
        {
            var errorMessage = "Das ist ein Fehler!";
            var generalError = new GeneralError(errorMessage);
            Assert.IsTrue(generalError.GetErrorString() == errorMessage + "\n");
        }
    }
}