// <copyright file="Controller.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTool
{
    using UseCaseCore.UcIntern;
    using UseCaseCore.XmlParser;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;
    using System.Windows;

    /// <summary>
    /// Controller class
    /// </summary>
    public class Controller : INotifyPropertyChanged
    {
        /// <summary>
        /// Creates an instance of the xml structure parser class
        /// </summary>
        private XmlStructureParser xmlParser = new XmlStructureParser();

        /// <summary>
        /// Creates an instance of the UseCase class
        /// </summary>
        private UseCase useCase = new UseCase();

        private Brush foregroundColor = new SolidColorBrush(Color.FromArgb(255, 65, 177, 255)); 
        private Visibility visibilityOk = Visibility.Hidden;
        private Visibility visibilityFail = Visibility.Hidden;

        public Brush ForegroundColor
        {
            get { return foregroundColor; }
            set
            {
                foregroundColor = value;
                OnPropertyChanged();
            }
        }

        public Visibility VisibilityOk
        {
            get
            {
                return visibilityOk;
            }
            set
            {
                visibilityOk = value;
                OnPropertyChanged();
            }
        }

        public Visibility VisibilityFail
        {
            get
            {
                return visibilityFail;
            }
            set
            {
                visibilityFail = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Tell the system path and file name of the XML UseCase file. The string para-meter contains the path to the new file.
        /// </summary>
        /// <param name="filePath">The currently selected path</param>
        public void CurrentXmlFilePath(string filePath)
        {
            if (this.xmlParser.LoadXmlFile(filePath))
            {
                this.ForegroundColor = Brushes.LimeGreen;
                this.VisibilityOk = Visibility.Visible;

                this.xmlParser.ParseXmlFile(out this.useCase);
                string test = string.Empty;
            }
            else
            {
                this.ForegroundColor = Brushes.Red;
                this.VisibilityFail = Visibility.Visible;
            }
        }

        /// <summary>
        /// Tell the system path and file name for the export of the scenario matrix and to trigger the export. 
        /// The parameter string contains the path under which the new file should be stored.
        /// </summary>
        /// <param name="filePath">destination path for the matrix</param>
        public void MatrixFilePath(string filePath)
        {
            ////  ...
        }

        /// <summary>
        /// Tell the system path and file name for the export of the defect report and to trigger this. 
        /// The parameter string contains the path under which the new file should be stored.
        /// </summary>
        /// <param name="filePath">destination path for the defect report</param>
        public void ReportFilePath(string filePath)
        {
            ////  ...
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}