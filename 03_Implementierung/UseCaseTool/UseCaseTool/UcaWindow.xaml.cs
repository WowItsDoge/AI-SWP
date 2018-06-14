// <copyright file="UcaWindow.xaml.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTool
{
    using System.Windows;
    using MahApps.Metro.Controls;
    using Microsoft.Win32;
    using System.Windows.Input;
    using UseCaseCore.Controller;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class UcaWindow : MetroWindow
    {
        /// <summary>
        /// That's storage for the originally broad sidebar.
        /// </summary>
        private double oldWidth = 0;

        /// <summary>
        /// Save the export directory
        /// </summary>
        private string importDirectory = string.Empty;
        
        /// <summary>
        /// /// Save the export directory
        /// </summary>
        private string exportDirectory = string.Empty;

        /// <summary>
        /// Creates an element of the controller class
        /// </summary>
        private Controller controller = new Controller();

        /// <summary>
        /// Initializes a new instance of the <see cref="UcaWindow"/> class.
        /// </summary>
        public UcaWindow()
        {
            this.DataContext = this.controller;
            this.controller.ScenariosCreated += Controller_ScenariosCreated;
            this.controller.GraphCreated += Controller_GraphCreated;
            this.InitializeComponent();
        }

        /// <summary>
        /// When new scenarios were created, draw them with the Matrix Control
        /// </summary>
        /// <param name="obj"></param>
        private void Controller_ScenariosCreated(System.Collections.Generic.List<UseCaseCore.ScenarioMatrix.Scenario> obj)
        {
            this.MatrixControl.Draw(obj);
        }

        /// <summary>
        /// When new Graph was created, draw it with Graph Control
        /// </summary>
        /// <param name="obj"></param>
        private void Controller_GraphCreated(UseCaseCore.UcIntern.UseCase useCase)
        {
            this.GraphControl.UpdateGraphView(useCase);
        }

        /// <summary>
        /// Button to open the dialog with which you can select the file
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void OpenButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openXmlFileDialog = new OpenFileDialog();

            openXmlFileDialog.Filter = "Docm files (*.docm)|*.docm";
            openXmlFileDialog.RestoreDirectory = true;

            openXmlFileDialog.ShowDialog();

            if (openXmlFileDialog.FileName != string.Empty)
            {
                selectedFile.Text = openXmlFileDialog.FileName;
                this.controller.CurrentXmlFilePath(openXmlFileDialog.FileName);
            }
        }

        /// <summary>
        /// Button to cancel the process
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            controller.CancelProcess();
        }

        /// <summary>
        /// Button to close the window
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Wollen Sie das Programm wirklich schließen?", "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Button to hide the sidebar
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void SidebarHideClick(object sender, RoutedEventArgs e)
        {
            this.oldWidth = GridColumn0.ActualWidth;

            mainGrid.ColumnDefinitions[0].Width = new GridLength(0);

            sidebarHide.Visibility = Visibility.Collapsed;
            sidebarShow.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Button to show the sidebar
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void SidebarShowClick(object sender, RoutedEventArgs e)
        {
            mainGrid.ColumnDefinitions[0].Width = new GridLength(this.oldWidth);
            GridColumn0.Visibility = Visibility.Visible;

            sidebarShow.Visibility = Visibility.Collapsed;
            sidebarHide.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Button to export the report
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void ExportReportClick(object sender, RoutedEventArgs e)
        {
            this.OpenSaveFileDialog();
            if (this.exportDirectory != null)
            {
                this.controller.ReportFilePath(this.exportDirectory);
            }
        }

        /// <summary>
        /// Button to export the matrix
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void ExportMatrixClick(object sender, RoutedEventArgs e)
        {
            this.OpenSaveFileDialog();
            if (this.exportDirectory != null)
            {
                this.controller.MatrixFilePath(this.exportDirectory);
            }
        }

        /// <summary>
        /// Open file dialog where the report is exported
        /// </summary>
        /// <returns>Return the path to where the report should be exported</returns>
        private string OpenSaveFileDialog()
        {
            SaveFileDialog saveFileDialogReport = new SaveFileDialog();

            saveFileDialogReport.Filter = "Textdatei (*.txt)|*.txt";
            saveFileDialogReport.RestoreDirectory = true;

            saveFileDialogReport.ShowDialog();
            this.exportDirectory = saveFileDialogReport.FileName;
            return this.exportDirectory;
        }

        /// <summary>
        /// Button to show infos
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void InfoClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Bla bla...", "Info Über Use-Case Visualizer", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        /// <summary>
        /// Button to change language
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void ChangeLanguageClick(object sender, RoutedEventArgs e)
        {
            ////
        }

        /// <summary>
        /// Button to change zoom level
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            GraphControl.Zoom(1.2);
        }

        /// <summary>
        /// Button to change zoom level
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            GraphControl.Zoom(0.8);
        }

        /// <summary>
        /// Chance view with key up
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void TabItem_KeyUp(object sender, KeyEventArgs e)
        {
            GraphControl.Move(10, 0);
        }


        /// <summary>
        /// Chance cycle depth value
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void NumericUpDownValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            controller.ChangeCycleDepth((uint)CycleDepth.Value);

        }

        //ToDo...
        // - Cancel Button
        // - Anzeige Mängelbericht
        // - ... 

    }
}
