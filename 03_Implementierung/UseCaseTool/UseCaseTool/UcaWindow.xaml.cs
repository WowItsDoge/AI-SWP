// <copyright file="UcaWindow.xaml.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace UseCaseTool
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class UcaWindow : MetroWindow
    {
        double oldWidth = 0;
        string exportDirectory = "";


        Controller ucController = new Controller();

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
                ucController.currentXmlFilePath(openXmlFileDialog.FileName);
            }
        }


        private void SidebarHideClick(object sender, RoutedEventArgs e)
        {
            oldWidth = GridColumn0.ActualWidth;

            mainGrid.ColumnDefinitions[0].Width = new GridLength(0);

            sidebarHide.Visibility = Visibility.Collapsed;
            sidebarShow.Visibility = Visibility.Visible;
        }

        private void SidebarShowClick(object sender, RoutedEventArgs e)
        {
            mainGrid.ColumnDefinitions[0].Width = new GridLength(oldWidth);
            GridColumn0.Visibility = Visibility.Visible;

            sidebarShow.Visibility = Visibility.Collapsed;
            sidebarHide.Visibility = Visibility.Visible;
        }



        private void ExportReportClick(object sender, RoutedEventArgs e)
        {
            OpenSaveFileDialog();
            if (exportDirectory != null)
            {
                ucController.reportFilePath(exportDirectory);
            }
        }


        private void ExportMatrixClick(object sender, RoutedEventArgs e)
        {
            OpenSaveFileDialog();
            if (exportDirectory != null)
            {
                ucController.matrixFilePath(exportDirectory);
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
