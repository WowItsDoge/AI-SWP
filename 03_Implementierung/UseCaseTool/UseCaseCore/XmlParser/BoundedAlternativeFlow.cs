// <copyright file="BoundedAlternativeFlow.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>
namespace UseCaseCore.XmlParser
{
    /// <summary>
    /// The bounded alternative flow instance.
    /// </summary>
    public class BoundedAlternativeFlow : Flow
    {
        /// <summary>
        /// The flow id.
        /// </summary>
        private int id;
        ////private List<ReferenceStep> referenceSteps;

        /// <summary>
        /// The method to set the flow id.
        /// </summary>
        /// <param name="id">Specifies the id to set.</param>
        public void SetId(int id)
        {
            this.id = id;
        }

        /*public void addReferenceStep(ReferenceStep referenceStepToAdd)
        {

        }*/
    }
}
