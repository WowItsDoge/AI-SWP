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
        /// Describes which steps get exectuted in what order
        /// </summary>
        private string description;

        /// <summary>
        /// User Comment to this scenario
        /// </summary>
        private string comment;

        /// <summary>
        /// Get/Set ScenarioID
        /// </summary>
        public int ScenarioID
        {
            get { return this.scenarioID; }
            set { this.scenarioID = value; }
        }


        /// <summary>
        /// Get/Set Description
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }


        /// <summary>
        /// Get/Set Comment
        /// </summary>
        public string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="newID"></param>
        public Scenario(int newID)
        {
            this.ScenarioID = newID;
        }
        





    }
}
