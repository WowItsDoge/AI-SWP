// <copyright file="MatrixControl.xaml.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTool
{
    using System;
    using System.Collections.Generic;
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

    /// <summary>
    /// Interaktionslogik für MatrixControl.xaml
    /// </summary>
    public partial class MatrixControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the MatrixControl class
        /// </summary>
        public MatrixControl()
        {
            this.InitializeComponent();

            // Beispieldaten
            List<Scenario> scenarios = new List<Scenario> { new Scenario(1, "K1;K2;K3"), new Scenario(2, "K1;K2;K4"), new Scenario(3, "K1;K2;K5") };
            this.Draw(scenarios);
        }
        
        /// <summary>
        /// Draws the scenario matrix
        /// </summary>
        /// <param name="scenarios"> scenarios to be drawn in the matrix </param>
        public void Draw(List<Scenario> scenarios)
        {
            foreach (Scenario s in scenarios)
            {
                this.AddTableRow(s);
            }
        }

        /// <summary>
        /// Adds a table row with a given scenario to the scenario matrix
        /// </summary>
        /// <param name="s"> scenario to be included in the matrix table </param>
        private void AddTableRow(Scenario s)
        {            
            var rowGroup = this.MatrixTable.RowGroups.Last();

            if (rowGroup != null)
            {
                TableRow row = new TableRow();

                TableCell cell = new TableCell();
                cell.BorderThickness = new Thickness(1);
                cell.BorderBrush = Brushes.Black;

                cell.Blocks.Add(new Paragraph(new Run("Szenario " + s.ScenarioID.ToString())));
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.BorderThickness = new Thickness(1);
                cell.BorderBrush = Brushes.Black;
                cell.Blocks.Add(new Paragraph(new Run(s.Description.ToString())));
                row.Cells.Add(cell);
                
                rowGroup.Rows.Add(row);
            }
        }
    }
}
