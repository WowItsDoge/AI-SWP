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
        /// Initializes a new instance of the MatrixControl class
        /// </summary>
        public MatrixControl()
        {
            this.InitializeComponent();
            this.scenarios = new List<Scenario>();
            this.matrixGrid.ItemsSource = this.Scenarios;
        }

        private List<Scenario> scenarios;
        
        public List<Scenario> Scenarios
        {
            get { return scenarios; }
            set { scenarios = value; }
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
        
    }
}
