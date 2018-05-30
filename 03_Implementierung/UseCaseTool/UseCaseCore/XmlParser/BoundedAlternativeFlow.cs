// <copyright file="BoundedAlternativeFlow.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>
namespace UseCaseCore.XmlParser
{
    using System.Collections.Generic;
    using UcIntern;

    /// <summary>
    /// The bounded alternative flow instance.
    /// </summary>
    public class BoundedAlternativeFlow : Flow
    {
        /// <summary>
        /// The flow id.
        /// </summary>
        private int id;

        /// <summary>
        /// The list of reference steps.
        /// </summary>
        private List<ReferenceStep> referenceSteps;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedAlternativeFlow" /> class.
        /// </summary>
        public BoundedAlternativeFlow() : base()
        {
            this.id = 0;
            this.referenceSteps = new List<ReferenceStep>();
        }

        /// <summary>
        /// The method to set the flow id.
        /// </summary>
        /// <param name="id">Specifies the id to set.</param>
        public void SetId(int id)
        {
            this.id = id;
        }

        /// <summary>
        /// The method to get the flow id.
        /// </summary>
        /// <returns>The flow id.</returns>
        public int GetId()
        {
            return this.id;
        }

        /// <summary>
        /// The method to add a reference step.
        /// </summary>
        /// <param name="referenceStepToAdd">Specifies the reference step to add.</param>
        public void AddReferenceStep(ReferenceStep referenceStepToAdd)
        {
            this.referenceSteps.Add(referenceStepToAdd);
        }

        /// <summary>
        /// The method to get the reference steps.
        /// </summary>
        /// <returns> The list of reference steps.</returns>
        public List<ReferenceStep> GetReferenceSteps()
        {
            return this.referenceSteps;
        }
    }
}
