﻿// <copyright file="SpecificAlternativeFlow.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>
namespace UseCaseCore.XmlParser
{
    /// <summary>
    /// The specific alternative flow instance.
    /// </summary>
    public class SpecificAlternativeFlow : Flow
    {
        /// <summary>
        /// The flow id.
        /// </summary>
        private int id;

        //// private ReferenceStep referenceStep;

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
