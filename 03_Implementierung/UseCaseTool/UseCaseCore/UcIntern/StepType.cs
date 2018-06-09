// <copyright file="StepType.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// This class descripes different kinds of steps.
    /// </summary>
    public class StepType
    {
        /// <summary>
        /// A normal step
        /// </summary>
        public static readonly StepType Normal = new StepType(0, new string[0]);

        /// <summary>
        /// The starting step of an if-statement.
        /// </summary>
        public static readonly StepType If = new StepType(1, new[] { @"\b\b" });

        /// <summary>
        /// The else step of an if-statement.
        /// </summary>
        public static readonly StepType Else = new StepType(2, new[] { @"\b\b" });

        /// <summary>
        /// An else if step of an if-statment.
        /// </summary>
        public static readonly StepType ElseIf = new StepType(3, new[] { @"\b\b" });

        /// <summary>
        /// The do step of an do-while-statement.
        /// </summary>
        public static readonly StepType Do = new StepType(4, new[] { @"\b\b" });

        /// <summary>
        /// The while step of an do-while-statement.
        /// </summary>
        public static readonly StepType While = new StepType(5, new[] { @"\b\b" });

        /// <summary>
        /// A resume step.
        /// </summary>
        public static readonly StepType Resume = new StepType(6, new[] { @"\b\b" });

        /// <summary>
        /// An abort step.
        /// </summary>
        public static readonly StepType Abort = new StepType(7, new[] { @"\b\b" });

        /// <summary>
        /// An abort step.
        /// </summary>
        public static readonly StepType ValidatesThat = new StepType(8, new[] { @"\b\b" });

        /// <summary>
        /// The regex patterns that match this type.
        /// </summary>
        public readonly string[] Patterns;

        /// <summary>
        /// The id of this type.
        /// </summary>
        public readonly int Id;

        /// <summary>
        /// Initializes a new instance of the <see cref="StepType"/> class.
        /// </summary>
        /// <param name="id">The id of the type. Must be unique for all types.</param>
        /// <param name="patterns">The regex patterns that match this type.</param>
        private StepType(int id, string[] patterns)
        {
            this.Id = id;
            this.Patterns = patterns;
        }

        /// <summary>
        /// Gets an array containing all step types.
        /// </summary>
        /// <returns>All step types.</returns>
        public static StepType[] All
        {
            get
            {
                List<StepType> stepTypes = new List<StepType>();

                Type stepTypeType = typeof(StepType);

                FieldInfo[] fields = stepTypeType.GetFields(BindingFlags.Public | BindingFlags.Static);

                foreach (FieldInfo fieldInfo in fields)
                {
                    StepType stepType = (StepType)fieldInfo.GetValue(null);

                    stepTypes.Add(stepType);
                }

                return stepTypes.ToArray();
            }
        }
    }
}
