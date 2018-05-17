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
        private double oldWidth = 0;
        private string exportDirectory = string.Empty;

        private Controller controller = new Controller();

        public UcaWindow()
        {
            InitializeComponent();
        }

        private void OpenButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openXmlFileDialog = new OpenFileDialog();

            openXmlFileDialog.InitialDirectory = "c:\\";
            openXmlFileDialog.Filter = "XML files (*.xml)|*.xml";
            //openXmlFileDialog.FilterIndex = 2;
            openXmlFileDialog.RestoreDirectory = true;

            openXmlFileDialog.ShowDialog();

            if (openXmlFileDialog.FileName != null)
            {
                selectedFile.Text = openXmlFileDialog.FileName;
                controller.CurrentXmlFilePath(openXmlFileDialog.FileName);
            }
        }




        /// <summary>
        /// Button to hide the sidebar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SidebarHideClick(object sender, RoutedEventArgs e)
        {
            oldWidth = GridColumn0.ActualWidth;

            mainGrid.ColumnDefinitions[0].Width = new GridLength(0);

            sidebarHide.Visibility = Visibility.Collapsed;
            sidebarShow.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Button to show the sidebar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SidebarShowClick(object sender, RoutedEventArgs e)
        {
            mainGrid.ColumnDefinitions[0].Width = new GridLength(oldWidth);
            GridColumn0.Visibility = Visibility.Visible;

            sidebarShow.Visibility = Visibility.Collapsed;
            sidebarHide.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// Button to export the report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportReportClick(object sender, RoutedEventArgs e)
        {
            OpenSaveFileDialog();
            if (exportDirectory != null)
            {
                controller.ReportFilePath(exportDirectory);
            }
        }


        private void ExportMatrixClick(object sender, RoutedEventArgs e)
        {
            OpenSaveFileDialog();
            if (exportDirectory != null)
            {
                controller.MatrixFilePath(exportDirectory);
            }
        }

        private string OpenSaveFileDialog()
        {
            SaveFileDialog saveFileDialogReport = new SaveFileDialog();

            saveFileDialogReport.InitialDirectory = "c:\\";
            saveFileDialogReport.Filter = "PDF files (*.pdf)|*.pdf";
            //saveFileDialogReport.FilterIndex = 2;
            saveFileDialogReport.RestoreDirectory = true;

            saveFileDialogReport.ShowDialog();
            exportDirectory = saveFileDialogReport.FileName;
            return exportDirectory;
        }
    }
}
