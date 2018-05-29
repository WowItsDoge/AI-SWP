// <copyright file="Scenario.cs" company="Team B">
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
    /// Scenario class
    /// </summary>
    public class Scenario
    {
        /// <summary>
        /// Identifies each Scenario
        /// </summary>
        private int scenarioID;

        /// <summary>
        /// Describes which steps get executed in what order
        /// </summary>
        private string description;

        /// <summary>
        /// Sorted List of all nodes traversed in this scenario
        /// </summary>
        private List<Node> nodes;

        /// <summary>
        /// Initializes a new instance of the Scenario class
        /// </summary>
        /// <param name="newID"> ID for the Scenario </param>
        public Scenario(int newID)
        {
            this.ScenarioID = newID;
            this.nodes = new List<Node>();
        }
        
        /// <summary>
        /// Gets or sets ScenarioID
        /// </summary>
        public int ScenarioID
        {
            get { return this.scenarioID; }
            set { this.scenarioID = value; }
        }

        /// <summary>
        /// Gets or sets Nodes
        /// </summary>
        public List<Node> Nodes
        {
            get { return this.nodes; }
            set { this.nodes = value; }
        }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }        
    }
}
