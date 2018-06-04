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
        /// Initializes a new instance of the <see cref="GlobalAlternativeFlow" /> class.
        /// </summary>
        public GlobalAlternativeFlow() : base()
        {
            this.id = 0;
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
        /// A method to set the flow id.
        /// </summary>
        /// <param name="id">Specifies the id to set.</param>
        public void SetId(int id)
        {
            this.id = id;
        }
    }
}
