﻿// <copyright file="Scenario.cs" company="Team B">
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
        /// Describes which steps get executed in what order
        /// </summary>
        private string description;

        /// <summary>
        /// Sorted List of all nodes traversed in this scenario
        /// </summary>
        private List<Node> nodes;

        /// <summary>
        /// Id of the scenario
        /// </summary>
        private int id;

        /// <summary>
        /// User defined comment to a scenario
        /// </summary>
        private string comment;

        /// <summary>
        /// Initializes a new instance of the Scenario class
        /// </summary>
        public Scenario()
        {
            this.description = string.Empty;
            this.nodes = new List<Node>();
        }

        /// <summary>
        /// Initializes a new instance of the Scenario class
        /// </summary>
        /// <param name="s"> scenario that gets copied </param>
        public Scenario(Scenario s)
        {
            this.description = s.Description;
            this.nodes = new List<Node>();
            s.nodes.ForEach(n => this.nodes.Add(n));
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
        
        /// <summary>
        /// Gets or sets the scenario id
        /// </summary>
        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets the comment
        /// </summary>
        public string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }        
    }
}