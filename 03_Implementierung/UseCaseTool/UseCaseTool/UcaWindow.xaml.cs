// <copyright file="UcaWindow.xaml.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTool
{
    using System.Windows;
    using MahApps.Metro.Controls;
    using Microsoft.Win32;

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
            this.InitializeComponent();
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
        /// Button to close the window
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Programm wirklich schließen?", "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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

            saveFileDialogReport.InitialDirectory = "c:\\";
            saveFileDialogReport.Filter = "PDF files (*.pdf)|*.pdf";
            ////saveFileDialogReport.FilterIndex = 2;
            saveFileDialogReport.RestoreDirectory = true;

            saveFileDialogReport.ShowDialog();
            this.exportDirectory = saveFileDialogReport.FileName;
            return this.exportDirectory;
        }
    }
}
