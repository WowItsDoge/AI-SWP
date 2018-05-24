// <copyright file="ScenarioMatrix.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.ScenarioMatrix
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// ScenarioMatrix Class
    /// </summary>
    public class ScenarioMatrix
    {
        /// <summary>
        /// List of all found scenarios
        /// </summary>
        private List<Scenario> scenarios;

        /// <summary>
        ///  Sets the Cycledepth that defines how many times an alternative flow may be repeated
        /// </summary>
        private int cycleDepth;
        
        /// <summary>
        /// Initializes a new instance of the ScenarioMatrix class
        /// </summary>
        public ScenarioMatrix()
        {
            this.scenarios = new List<Scenario>();
            this.CycleDepth = 1;
        }

        /// <summary>
        /// Gets or sets CycleDepth
        /// </summary>
        private int CycleDepth
        {
            get { return this.cycleDepth; }
            set { this.cycleDepth = value; }
        }

        /// <summary>
        /// Changes the CycleDepth with a given value. Negative values get ignored. If the value changed, scenarios get recalculated
        /// </summary>
        /// <param name="newCycleDepth"> new value for cycleDepth</param>
        public void ChangeCycleDepth(int newCycleDepth)
        {
            // If negative cycledepth or same value return
            if (newCycleDepth < 0 || newCycleDepth == this.CycleDepth)
            {
                return;
            }

            this.CycleDepth = newCycleDepth;
            this.CreateScenarios();
        }
        
        // public bool Initialize()

        /// <summary>
        /// Exports the ScenarioMatrix to a given path
        /// </summary>
        /// <param name="path"> path to the location, where the exported file should be saved </param>
        /// <returns> Returns true if successful </returns>
        public bool Export(string path)
        {
            return true;
        }

        /// <summary>
        /// Returns all found scenarios
        /// </summary>
        /// <returns>list of scenarios</returns>
        public List<Scenario> GetScenarios()
        {
            return this.scenarios;
        }

        /// <summary>
        /// Calculates all scenarios
        /// </summary>
        private void CreateScenarios()
        {
            // TODO
        }

        /// <summary>
        /// Creates the ScenarioMatrix
        /// </summary>
        private void CreateMatrix()
        {
            // TODO
        }
    }
}
