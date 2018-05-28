// <copyright file="Flow.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>
namespace UseCaseCore.XmlParser
{
    using System.Collections.Generic;

    /// <summary>
    /// The abstract class for the flows.
    /// </summary>
    public abstract class Flow
    {
        /// <summary>
        /// A list of steps the flow has.
        /// </summary>
        private List<string> steps;

        /// <summary>
        /// The postcondition the flow has.
        /// </summary>
        private string postcondition;

        /// <summary>
        /// Initializes a new instance of the <see cref="Flow" /> class.
        /// </summary>
        public Flow()
        {
            this.steps = new List<string>();
            this.postcondition = string.Empty;
        }

        /// <summary>
        /// A method to add a step to the flow.
        /// </summary>
        /// <param name="stepToAdd">Specifies the step to add.</param>
        public void AddStep(string stepToAdd)
        {
            this.steps.Add(stepToAdd);
        }

        /// <summary>
        /// A method to add a step to the flow.
        /// </summary>
        /// <returns> The list of the steps.</returns>
        public List<string> GetSteps()
        {
            return this.steps;
        }

        /// <summary>
        /// A method to set the postcondition of the flow.
        /// </summary>
        /// <param name="postconditionToSet">Specifies the postcondition to set.</param>
        public void SetPostcondition(string postconditionToSet)
        {
            this.postcondition = postconditionToSet;
        }

        /// <summary>
        /// A method to get the postcondition of the flow.
        /// </summary>
        /// <returns> The postcondition of the flow.</returns>
        public string GetPostcondition()
        {
            return this.postcondition;
        }
    }
}
