// <copyright file="ScenarioMatrix.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.ScenarioMatrix
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using UcIntern;

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
        /// Sets the Use Case from which the scenarios get created
        /// </summary>
        private UseCase uc;
            
        /// <summary>
        /// Initializes a new instance of the ScenarioMatrix class
        /// </summary>
        /// <param name="uc"> UseCase from which the scenarios get created </param>
        /// <param name="cycleDepth"> CycleDepth defined by the GUI </param>
        public ScenarioMatrix(UseCase uc, int cycleDepth = 1)
        {
            this.uc = uc;
            this.scenarios = new List<Scenario>();
            this.CycleDepth = cycleDepth;
        }

        /// <summary>
        /// Gets or sets the Use Case from which the scenarios get created
        /// </summary>
        public UseCase UC
        {
            get { return this.uc; }
            set { this.uc = value; }
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
        
        /// <summary>
        /// Initializes the ScenarioMatrix
        /// </summary>
        /// <param name="uc"> Reference to the UseCase from which the scenarios should be created </param>
        /// <returns> Returns true if successful Initialization</returns>
        public bool Initialize(UseCase uc)
        {
            this.uc = uc; 

            // this.CreateScenarios();
            return true; // TOCHANGE
        }

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
        public void CreateScenarios()
        {
            if (this.uc == null)
            {
                return;
            }

            this.scenarios = new List<Scenario>(); // Clear all old scenarios to create new ones

            Scenario s = new Scenario(this.scenarios.Count);            
            s.Nodes.Add(this.uc.Nodes[0]); // Startknoten hinzufügen

            this.TraverseGraphRec(this.uc.EdgeMatrix, 0, s, this.CycleDepth);
        }

        /// <summary>
        /// Returns the amount of Edges from node1 to node2 in a scenario
        /// </summary>
        /// <param name="node1"> start node of the edge </param>
        /// <param name="node2">  end node of the edge </param>
        /// <param name="s"> Scenario to be searched </param>
        /// <returns> Returns amount of found edges </returns>
        private static int ContainedEdges(Node node1, Node node2, Scenario s)
        {
            if (s.Nodes.Count < 2)
            {
                return 0; // A Scenario with only 1 node doesnt contain any edges
            }
            
            return s.Nodes
                .Select((n, i) => new { n, i })
                .Where(x => x.i > 0)
                .Where(x => x.n.Equals(node2) && s.Nodes[x.i - 1].Equals(node1)).Count();            
        }
        
        /// <summary>
        /// Creates the ScenarioMatrix
        /// </summary>
        private void CreateMatrix()
        {
            // TODO
        }

        /// <summary>
        /// Traverses the graph recursively, while finding all possible paths through the graph
        /// </summary>
        /// <param name="matrix"> Edgematrix representing the graph </param>
        /// <param name="startnode"> Node from which the traversing starts </param>
        /// <param name="s"> scenario up to the current node </param>
        /// <param name="cycleDepth"> maximum cycledepth for the scenarios </param>
        private void TraverseGraphRec(Matrix<bool> matrix, int startnode, Scenario s, int cycleDepth)
        { 
            int stepsFound = 0;
            Scenario savedScenario = new Scenario(s);
            for (int i = 0; i < matrix.ColumnCount; i++)
            {
                if (matrix[startnode][i] == true)
                {
                    if (ContainedEdges(this.uc.Nodes[startnode], this.uc.Nodes[i], s) >= cycleDepth)
                    {
                        return; // If this edge was repeated to often already
                    }

                    // Add current node to the scenario and increase of found steps from the previous node
                    s.Nodes.Add(this.uc.Nodes[i]); 
                    stepsFound++;

                    this.TraverseGraphRec(matrix, i, s, cycleDepth);

                    // Continue with old scenario 
                    s = savedScenario;
                }
            }

            // If no next steps were found for the current steps, the scenario is finished and can be added to the list
            if (stepsFound == 0)
            {
                this.scenarios.Add(s);
            }
        }
    }
}
