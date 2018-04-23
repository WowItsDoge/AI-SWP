using System;
using UseCaseCore;
using NUnit.Framework;

namespace UseCaseTest
{
    [TestFixture]
    public class ExampleTest
    {
        [Test]
        public void TestMethod1()
        {
            string message = Example.GetMessage();

            Assert.IsNotNull(message);
        }
    }
}