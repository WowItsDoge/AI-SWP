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

namespace UseCaseTool
{
    /// <summary>
    /// Interaktionslogik für MatrixControl.xaml
    /// </summary>
    public partial class MatrixControl : UserControl
    {
        public MatrixControl()
        {
            InitializeComponent();

            //Beispieldaten
            List<Scenario> scenarios = new List<Scenario> { new Scenario(1, "K1;K2;K3"), new Scenario(2, "K1;K2;K4"), new Scenario(3, "K1;K2;K5") };
            Draw(scenarios);
        }
        
        public void Draw(List<Scenario> scenarios)
        {
            foreach (Scenario s in scenarios)
                AddTableRow(s);
        }


        private void AddTableRow(Scenario s)
        {
            
            var rowGroup = MatrixTable.RowGroups.Last();

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
