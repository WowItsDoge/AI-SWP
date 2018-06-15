// <copyright file="BitShifterTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
    using System;
    using NUnit.Framework;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A class collecting all tests for the <see cref="BitShifter"/> class.
    /// </summary>
    [TestFixture]
    public class BitShifterTests
    {
        /// <summary>
        /// Shift 1 one to the left. Result: 2
        /// </summary>
        [Test]
        public void ShiftOneOneLeft()
        {
            Assert.AreEqual(2, BitShifter.ShiftAndWrap(1, 1));
        }

        /// <summary>
        /// Shift 1 thirty two to the left. Result: 1
        /// </summary>
        [Test]
        public void ShiftOneThirtyTwoLeft()
        {
            Assert.AreEqual(1, BitShifter.ShiftAndWrap(1, 32));
        }

        /// <summary>
        /// Shift -1 one to the left. Result: -1
        /// </summary>
        [Test]
        public void ShiftMinusOneOneLeft()
        {
            Assert.AreEqual(-1, BitShifter.ShiftAndWrap(-1, 1));
        }

        /// <summary>
        /// Shift the minimum of a 32 bit int one to the left. Result: 1
        /// </summary>
        [Test]
        public void ShiftIntMinOneLeft()
        {
            Assert.AreEqual(1, BitShifter.ShiftAndWrap(Int32.MinValue, 1));
        }
    }
}
