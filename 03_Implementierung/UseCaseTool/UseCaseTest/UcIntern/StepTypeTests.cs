// <copyright file="StepTypeTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using NUnit.Framework;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A class collecting all tests for the <see cref="StepType"/> class.
    /// </summary>
    [TestFixture]
    public class StepTypeTests
    {
        /// <summary>
        /// Gets the reference of every step type to test if no duplicated indices are set.
        /// </summary>
        [Test]
        public void TestStepTypeIndices()
        {
            List<int> usedIds = new List<int>();

            Type stepTypeType = typeof(StepType);

            FieldInfo[] fields = stepTypeType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (FieldInfo fieldInfo in fields)
            {
                StepType stepType = (StepType)fieldInfo.GetValue(null);
                
                if (usedIds.Contains(stepType.Id))
                {
                    Assert.Fail($"Duplicated step type id: {stepType.Id}");
                }

                usedIds.Add(stepType.Id);
            }
        }

        /// <summary>
        /// Tests the All method of step type.
        /// </summary>
        [Test]
        public void TestAllMethod()
        {
            List<StepType> stepTypes = new List<StepType>();

            Type stepTypeType = typeof(StepType);

            FieldInfo[] fields = stepTypeType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (FieldInfo fieldInfo in fields)
            {
                StepType stepType = (StepType)fieldInfo.GetValue(null);

                stepTypes.Add(stepType);
            }

            Assert.AreEqual(stepTypes, StepType.All);
        }
    }
}
