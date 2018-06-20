// <copyright file="MatrixControl.xaml.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTool
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using UseCaseCore.ScenarioMatrix;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// Interaction logic for MatrixControl.xaml
    /// </summary>
    public partial class MatrixControl : UserControl
    {        
        /// <summary>
        /// List of scenarios to be displayed
        /// </summary>
        private List<Scenario> scenarios;
        
        /// <summary>
        /// Initializes a new instance of the MatrixControl class
        /// </summary>
        public MatrixControl()
        {
            this.InitializeComponent();
            this.scenarios = new List<Scenario>();
            this.matrixGrid.ItemsSource = this.Scenarios;
            this.matrixGrid.RowEditEnding += this.MatrixGrid_RowEditEnding;
        }

        /// <summary>
        /// Event that fires when the Scenario Matrix gets changed by the user
        /// </summary>
        public event Action<Scenario> MatrixChangedByUser;

        /// <summary>
        /// Gets or sets the scenarios
        /// </summary>
        public List<Scenario> Scenarios
        {
            get { return this.scenarios; }
            set { this.scenarios = value; }
        }
                
        /// <summary>
        /// Draws the scenario matrix
        /// </summary>
        /// <param name="scenarios"> scenarios to be drawn in the matrix </param>
        public void Draw(List<Scenario> scenarios)
        {
            this.Scenarios.Clear();
            this.Scenarios.AddRange(scenarios);
            Dispatcher.Invoke(() => { matrixGrid.Items.Refresh(); });
        }
        
        /// <summary>
        /// Method that gets fires the MatrixChangeByUser event, when a row of the matrixGrid gets changed
        /// </summary>
        /// <param name="sender"> sender object </param>
        /// <param name="e"> event arguments</param>
        private void MatrixGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (this.MatrixChangedByUser != null)
            {
                this.MatrixChangedByUser((Scenario)e.Row.Item);
            }
        }
    }
}
