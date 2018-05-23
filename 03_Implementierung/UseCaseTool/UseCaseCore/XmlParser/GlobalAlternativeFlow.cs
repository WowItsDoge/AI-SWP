// <copyright file="GlobalAlternativeFlow.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>
namespace UseCaseCore.XmlParser
{
    /// <summary>
    /// The global alternative flow instance.
    /// </summary>
    public class GlobalAlternativeFlow : Flow
    {
        /// <summary>
        /// The flow id.
        /// </summary>
        private int id;

        /// <summary>
        /// A method to set the flow id.
        /// </summary>
        /// <param name="id">Specifies the id to set.</param>
        private void SetId(int id)
        {
            this.id = id;
        }
    }
}
