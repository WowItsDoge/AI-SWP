using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCaseCore.ScenarioMatrix
{
    public class ScenarioMatrix
    {
        /// <summary>
        /// List of all found scenarios
        /// </summary>
        private List<Scenario> scenarios;

        /// <summary>
        /// Cycledepth that defines how many times an alternative flow may be repeated
        /// </summary>
        private int cycleDepth;

        /// <summary>
        /// Get/Set CycleDepth
        /// </summary>
        private int CycleDepth
        {
            get { return this.cycleDepth; }
            set { this.cycleDepth = value; }
        }       

        /// <summary>
        /// Changes the CycleDepth with a given value. Negative values get ignored. If the value changed, scenarios get recalculated
        /// </summary>
        /// <param name="newCycleDepth"></param>
        public void ChangeCycleDepth(int newCycleDepth)
        {
            //If negative cycledepth or same value return
            if (newCycleDepth < 0 || newCycleDepth == CycleDepth) return;

            CycleDepth = newCycleDepth;
            CreateScenarios();
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public ScenarioMatrix()
        {
            this.scenarios = new List<Scenario>();
            CycleDepth = 1;
        }

        //public bool Initialize()

        /// <summary>
        /// Exports the ScenarioMatrix to a given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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
            return scenarios;
        }

        /// <summary>
        /// Calculates all scenarios
        /// </summary>
        private void CreateScenarios()
        {
            //TODO
        }

        /// <summary>
        /// Creates the ScenarioMatrix
        /// </summary>
        private void CreateMatrix()
        {
            //TODO
        }

    }
}
