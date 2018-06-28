// <copyright file="ScenarioMatrix.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.ScenarioMatrix
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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
        ///  Sets the cycle depth that defines how many times an alternative flow may be repeated
        /// </summary>
        private uint cycleDepth;

        /// <summary>
        /// Sets the Use Case from which the scenarios get created
        /// </summary>
        private UseCase uc;
            
        /// <summary>
        /// Initializes a new instance of the ScenarioMatrix class
        /// </summary>
        /// <param name="uc"> UseCase from which the scenarios get created </param>
        /// <param name="cycleDepth"> CycleDepth defined by the GUI </param>
        public ScenarioMatrix(UseCase uc, uint cycleDepth = 1)
        {
            this.uc = uc;
            this.scenarios = new List<Scenario>();
            this.cycleDepth = cycleDepth;            
        }

        /// <summary>
        /// Event that fires when new Scenarios were created
        /// </summary>
        public event Action<List<Scenario>> ScenariosCreated;

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
        public uint CycleDepth
        {
            get
            {
                return this.cycleDepth;
            }

            set
            {
                this.cycleDepth = value;

                // Refresh scenarios
                this.CreateScenarios();
            }
        }
        
        /// <summary>
        /// Exports the ScenarioMatrix to a given path
        /// </summary>
        /// <param name="path"> path to the location, where the exported file should be saved </param>
        /// <returns> Returns true if successful </returns>
        public bool Export(string path)
        {
            if (path == null || this.uc == null) 
            {
                return false;
            }

            bool exportResult = false;
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    sw.WriteLine("Scenario Matrix:");
                    sw.WriteLine(string.Empty);
                    int i = 1;
                    foreach (Scenario s in this.scenarios)
                    {
                        if (s.Comment != null || s.Comment == string.Empty) 
                        {
                            sw.WriteLine("// " + s.Comment);                            
                        }

                        sw.WriteLine("Scenario " + i.ToString() + ": " + s.Description);
                        sw.WriteLine(string.Empty);
                        i++;
                    }

                    sw.Close();
                }

                exportResult = true;
            }
            catch
            {
                exportResult = false;
            }
                           
            return exportResult;
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
        /// Creates the Scenarios to the current UseCase
        /// </summary>
        /// <returns> Returns true if more than 0 scenarios were found </returns>
        public bool CreateScenarios()
        {
            if (this.uc == null || this.uc.Nodes.Count == 0)
            {
                return false;
            }

            this.scenarios = new List<Scenario>(); // Clear all old scenarios to create new ones

            Scenario s = new Scenario();            
            s.Nodes.Add(this.uc.Nodes[0]); // add start node
            s.Description += "Step 1, ";

            this.TraverseGraphRec(this.uc.EdgeMatrix, 0, s, this.CycleDepth);
            this.EnmuerateScenarioIds();

            this.CreateMatrix();
            return true;
        }
        
        /// <summary>
        /// Updates a Scenarios Comment
        /// </summary>
        /// <param name="newScenario"> new scenario to replace the old one </param>
        public void UpdateScenarioComment(Scenario newScenario)
        {
            foreach (Scenario s in this.scenarios) 
            {
                if (s.ID == newScenario.ID) 
                {
                    s.Comment = newScenario.Comment;
                }
            }
        }

        /// <summary>
        /// Clears the Matrix and all Scenarios
        /// </summary>
        public void ClearMatrix()
        {
            this.scenarios = new List<Scenario>();
            this.CreateMatrix();
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
        /// Counts amount of child nodes from a starting node (row in a matrix)
        /// </summary>
        /// <param name="matrix"> edge matrix of this use case </param>
        /// <param name="row"> row representing the current node </param>
        /// <returns> returns the amount of child nodes / edges </returns>
        private static int CountEdgesPerRow(Matrix<bool> matrix, int row)
        {
            int amount = 0;
            for (int i = 0; i < matrix.ColumnCount; i++)
            {
                if (matrix[row, i] == true)
                {
                    amount++;
                }
            }

            return amount;
        }

        /// <summary>
        /// Returns whether a Scenario is the Use Cases Basic Flow
        /// </summary>
        /// <param name="s">Scenario to check</param>
        /// <returns>Returns true if the scenario is the basic flow</returns>
        private bool IsScenarioBasicFlow(Scenario s)
        {
            if (s.Nodes.Count != this.uc.BasicFlow.Nodes.Count)
            {
                return false;
            }

            int i = 0;
            foreach (Node n in s.Nodes)
            {
                if (n.StepDescription != this.uc.BasicFlow.Nodes[i].StepDescription
                    || n.Identifier != this.uc.BasicFlow.Nodes[i].Identifier) 
                {
                    return false;
                }

                i++;
            }

            return true;
        }

        /// <summary>
        /// Gives every Scenario an ID > 1
        /// </summary>
        private void EnmuerateScenarioIds()
        {
            int i = 1;
            foreach (Scenario s in this.scenarios)
            {
                s.ID = i;
                i++;
            }
        }

        /// <summary>
        /// Creates the ScenarioMatrix
        /// </summary>
        private void CreateMatrix()
        {
            if (this.ScenariosCreated != null) 
            {
                this.ScenariosCreated(this.scenarios);
            }
        }

        /// <summary>
        /// Traverses the graph recursively, while finding all possible paths through the graph
        /// </summary>
        /// <param name="matrix"> Edge matrix representing the graph </param>
        /// <param name="startnode"> Node from which the traversing starts </param>
        /// <param name="s"> scenario up to the current node </param>
        /// <param name="cycleDepth"> maximum cycle depth for the scenarios </param>
        private void TraverseGraphRec(Matrix<bool> matrix, int startnode, Scenario s, uint cycleDepth)
        { 
            int stepsFound = CountEdgesPerRow(matrix, startnode);

            // If the node is the last node of the basic flow add a scenario for the basic flow and continue traversing
            if (this.uc.Nodes[startnode].StepDescription == this.uc.BasicFlow.Nodes.Last().StepDescription)
            {
                Scenario s1 = new Scenario(s);
                s1.Description = Regex.Replace(s1.Description, "(.*),", "$1"); // Remove last comma

                if (this.IsScenarioBasicFlow(s))
                {
                    s1.Comment = "Basic Flow";
                }

                this.scenarios.Add(s1);

                // If the basic flows last node doesnt have any other edges, return so this scenario doesnt get included twice
                if (stepsFound == 0) 
                {
                    return;
                }
            }

            // If no next steps were found for the current steps, the scenario is finished and can be added to the list
            if (stepsFound == 0)
            {
                s.Description = Regex.Replace(s.Description, "(.*),", "$1"); // Remove last comma
                this.scenarios.Add(s);
                return;
            }

            Scenario savedScenario = new Scenario(s);

            for (int i = 0; i < matrix.ColumnCount; i++)
            {
                if (matrix[startnode, i] == true) 
                {
                    if (ContainedEdges(this.uc.Nodes[startnode], this.uc.Nodes[i], s) >= cycleDepth)
                    {
                        return; // If this edge was repeated to often already
                    }

                    // Add current node to the scenario and increase of found steps from the previous node
                    s.Nodes.Add(this.uc.Nodes[i]);
                    s.Description += "Step" + (i + 1).ToString() + ", ";

                    this.TraverseGraphRec(matrix, i, s, cycleDepth);

                    // If the current node doesnt have at least 2 child nodes, go back to the last node that did
                    if (stepsFound < 2)
                    {
                        return;
                    }

                    // Continue with old scenario 
                    s = new Scenario(savedScenario);
                }
            }
        }
    }
}
