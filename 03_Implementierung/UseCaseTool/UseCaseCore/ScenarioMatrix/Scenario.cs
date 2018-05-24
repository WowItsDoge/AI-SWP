// <copyright file="Scenario.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>
namespace UseCaseCore.ScenarioMatrix
{
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
        /// User Comment to this scenario
        /// </summary>
        private string comment;
        
        /// <summary>
        /// Initializes a new instance of the Scenario class
        /// </summary>
        /// <param name="newID"> ID for the Scenario </param>
        public Scenario(int newID)
        {
            this.ScenarioID = newID;
        }

        /// <summary>
        /// Initializes a new instance of the Scenario class
        /// </summary>
        /// <param name="newID"> ID for the Scenario </param>
        /// <param name="description"> Description for the scenario </param>
        public Scenario(int newID, string description)
        {
            this.ScenarioID = newID;
            this.Description = description;
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
        /// Gets or sets Description
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        /// <summary>
        /// Gets or sets Comment
        /// </summary>
        public string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }        
    }
}
