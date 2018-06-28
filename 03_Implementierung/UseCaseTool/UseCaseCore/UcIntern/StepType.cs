// <copyright file="StepType.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// This class describes different kinds of steps.
    /// </summary>
    public class StepType
    {
        /// <summary>
        /// A step without a pattern. It can not be matched.
        /// </summary>
        public static readonly StepType Unmatched = new StepType(0, new string[0]);

        /// <summary>
        /// The starting step of an if-statement.
        /// </summary>
        public static readonly StepType If = new StepType(1, new[] { @"(.*)\bIF\b(.*)\bTHEN\b" });

        /// <summary>
        /// The else step of an if-statement.
        /// </summary>
        public static readonly StepType Else = new StepType(2, new[] { @"\bELSE\b" });

        /// <summary>
        /// An else if step of an if-statement.
        /// </summary>
        public static readonly StepType ElseIf = new StepType(3, new[] { @"\bELSEIF\b(.*)\bTHEN\b" });

        /// <summary>
        /// An endif step of an if-statement.
        /// </summary>
        public static readonly StepType EndIf = new StepType(4, new[] { @"\bENDIF\b" });

        /// <summary>
        /// The do step of an do-until-statement.
        /// </summary>
        public static readonly StepType Do = new StepType(5, new[] { @"\bDO\b" });

        /// <summary>
        /// The until step of an do-until-statement.
        /// </summary>
        public static readonly StepType Until = new StepType(6, new[] { @"(.*)\bUNTIL\b(.*)" });

        /// <summary>
        /// A resume step.
        /// <para/>
        /// One group holding the number of the basic step to resume to.
        /// Don't forget that C# puts the full match into an additional group at index 0!
        /// </summary>
        public static readonly StepType Resume = new StepType(7, new[] { @"\bRESUME STEP\b ([0-9]+)", @"\bRESUME\b ([0-9]+)" });

        /// <summary>
        /// An abort step.
        /// </summary>
        public static readonly StepType Abort = new StepType(8, new[] { @"\bABORT\b" });

        /// <summary>
        /// An abort step.
        /// </summary>
        public static readonly StepType ValidatesThat = new StepType(9, new[] { @"(.*)\bVALIDATES THAT\b(.*)" });

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
