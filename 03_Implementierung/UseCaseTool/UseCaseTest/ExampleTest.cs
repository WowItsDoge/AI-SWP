// <copyright file="ExampleTest.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest
{
    using NUnit.Framework;

    using UseCaseCore;

    /// <summary>
    /// The unit test class for the example class
    /// </summary>
    [TestFixture]
    public class ExampleTest
    {
        /// <summary>
        /// An example test method 
        /// </summary>
        [Test]
        public void TestMethod1()
        {
            string message = Example.GetMessage();

            Assert.IsNotNull(message);
        }
    }
}