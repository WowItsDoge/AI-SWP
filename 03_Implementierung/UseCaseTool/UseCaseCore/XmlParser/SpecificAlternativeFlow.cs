// <copyright file="SpecificAlternativeFlow.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>
namespace UseCaseCore.XmlParser
{
    using System.Collections.Generic;
    using UcIntern;

    /// <summary>
    /// The specific alternative flow instance.
    /// </summary>
    public class SpecificAlternativeFlow : Flow
    {
        /// <summary>
        /// The reference step.
        /// </summary>
        private ReferenceStep referenceStep;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificAlternativeFlow" /> class.
        /// </summary>
        public SpecificAlternativeFlow() : base()
        {

        }

        /// <summary>
        /// The method to add a reference step.
        /// </summary>
        /// <param name="referenceStepToAdd">Specifies the reference step to add.</param>
        public void AddReferenceStep(ReferenceStep referenceStepToAdd)
        {
            this.referenceStep = referenceStepToAdd;
        }

        /// <summary>
        /// The method to get a reference step.
        /// </summary>
        /// <returns> The reference step.</returns>
        public ReferenceStep GetReferenceStep()
        {
            return this.referenceStep;
        }
    }
}
