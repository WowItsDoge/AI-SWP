// <copyright file="Controller.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.Controller
{
    using UseCaseCore.UcIntern;
    using UseCaseCore.XmlParser;
    using UseCaseCore.RuleValidation;
    using UseCaseCore.ScenarioMatrix;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;
    using System.Windows;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// Controller class
    /// </summary>
    public class Controller : INotifyPropertyChanged
    {
        private string currentFilePath = "";

        private BackgroundWorker backgroundWorkerLoadFile;
        private BackgroundWorker backgroundWorkerValidFile;
        private BackgroundWorker backgroundWorkerGetErrorReport;
        private BackgroundWorker backgroundWorkerGenerateMatrix;
        private BackgroundWorker backgroundWorkerGenerateGraph;

        /// <summary>
        /// Creates an instance of the xml structure parser class
        /// </summary>
        private XmlStructureParser xmlParser = new XmlStructureParser();

        /// <summary>
        /// Creates an instance of the UseCase class
        /// </summary>
        private UseCase useCase = new UseCase();

        /// <summary>
        /// Creates an instance of the ErrorReport class
        /// </summary>
        private ErrorReport errorReport = new ErrorReport();

        /// <summary>
        /// Creates an instance of the ScenarioMatrix class
        /// </summary>
        private ScenarioMatrix matrix;

        /// <summary>
        /// Creates an instance of the BackgroundColor class with default color
        /// </summary>
        private Brush backgroundColor = new SolidColorBrush(Color.FromArgb(255, 65, 177, 255));

        private Visibility visibilityOk = Visibility.Hidden;
        private Visibility visibilityFail = Visibility.Hidden;

        /// <summary>
        /// Fires when new scenarios were created
        /// </summary>
        public event Action<List<Scenario>> ScenariosCreated;

        public Brush BackgroundColor1
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                OnPropertyChanged();
            }
        }

        public Brush BackgroundColor2
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                OnPropertyChanged();
            }
        }

        public Brush BackgroundColor3
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                OnPropertyChanged();
            }
        }

        public Brush BackgroundColor4
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                OnPropertyChanged();
            }
        }

        public Visibility VisibilityOk1
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

        public Visibility VisibilityOk2
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

        public Visibility VisibilityOk3
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

        public Visibility VisibilityOk4
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

        public Visibility VisibilityFail1
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

        public Visibility VisibilityFail2
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

        public Visibility VisibilityFail3
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

        public Visibility VisibilityFail4
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
            currentFilePath = filePath;

            this.backgroundWorkerLoadFile = new BackgroundWorker();
            this.backgroundWorkerLoadFile.DoWork += new DoWorkEventHandler(backgroundWorkerLoadFile_DoWork);
            this.backgroundWorkerLoadFile.WorkerSupportsCancellation = true;

            if (!this.backgroundWorkerLoadFile.IsBusy)
            {
                this.backgroundWorkerLoadFile.RunWorkerAsync();
            }
        }


        private void backgroundWorkerLoadFile_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.xmlParser.LoadXmlFile(currentFilePath))
            {
                this.BackgroundColor1 = Brushes.LimeGreen;
                this.VisibilityOk1 = Visibility.Visible;

                this.backgroundWorkerValidFile = new BackgroundWorker();
                this.backgroundWorkerValidFile.DoWork += new DoWorkEventHandler(backgroundWorkerValidFile_DoWork);
                this.backgroundWorkerValidFile.WorkerSupportsCancellation = true;

                if (!this.backgroundWorkerValidFile.IsBusy)
                {
                    this.backgroundWorkerValidFile.RunWorkerAsync();
                }
            }
            else
            {
                this.BackgroundColor1 = Brushes.Red;
                this.VisibilityFail1 = Visibility.Visible;
            }
        }

        private void backgroundWorkerValidFile_DoWork(object sender, DoWorkEventArgs e)
        {
            this.backgroundWorkerGetErrorReport = new BackgroundWorker();
            this.backgroundWorkerGetErrorReport.DoWork += new DoWorkEventHandler(backgroundWorkerGetErrorReport_DoWork);
            this.backgroundWorkerGetErrorReport.WorkerSupportsCancellation = true;

            if (this.xmlParser.ParseXmlFile(out this.useCase))
            {
                this.BackgroundColor2 = Brushes.LimeGreen;
                this.VisibilityOk2 = Visibility.Visible;

                this.backgroundWorkerGenerateGraph = new BackgroundWorker();
                this.backgroundWorkerGenerateGraph.DoWork += new DoWorkEventHandler(backgroundWorkerGenerateGraph_DoWork);
                this.backgroundWorkerGenerateGraph.WorkerSupportsCancellation = true;

                this.backgroundWorkerGenerateMatrix = new BackgroundWorker();
                this.backgroundWorkerGenerateMatrix.DoWork += new DoWorkEventHandler(backgroundWorkerGenerateMatrix_DoWork);
                this.backgroundWorkerGenerateMatrix.WorkerSupportsCancellation = true;

                if (!this.backgroundWorkerGenerateGraph.IsBusy)
                {
                    this.backgroundWorkerGenerateGraph.RunWorkerAsync();
                }

                if (!this.backgroundWorkerGenerateMatrix.IsBusy)
                {
                    this.backgroundWorkerGenerateMatrix.RunWorkerAsync();
                }
            }
            else
            {
                this.BackgroundColor2 = Brushes.Red;
                this.VisibilityFail1 = Visibility.Visible;
            }

            if (!this.backgroundWorkerGetErrorReport.IsBusy)
            {
                this.backgroundWorkerGetErrorReport.RunWorkerAsync();
            }
        }

        private void backgroundWorkerGetErrorReport_DoWork(object sender, DoWorkEventArgs e)
        {
            //errorReport.GetErrorList();
            //ToDo...

        }

        private void backgroundWorkerGenerateGraph_DoWork(object sender, DoWorkEventArgs e)
        {
            //ToDo...
        }

        private void backgroundWorkerGenerateMatrix_DoWork(object sender, DoWorkEventArgs e)
        {
            matrix = new ScenarioMatrix(useCase, 1);
            matrix.ScenariosCreated += Matrix_scenariosCreated;
        }

        /// <summary>
        /// When new Scenarios got created
        /// </summary>
        /// <param name="obj"> Scenarios to be drawn </param>
        private void Matrix_scenariosCreated(System.Collections.Generic.List<Scenario> obj)
        {
            if (this.ScenariosCreated != null) 
            {
                ScenariosCreated(obj);
            }
        }

        public void CancelProgress()
        {
            backgroundWorkerLoadFile.CancelAsync();
            backgroundWorkerValidFile.CancelAsync();
            backgroundWorkerGetErrorReport.CancelAsync();
            backgroundWorkerGenerateMatrix.CancelAsync();
            backgroundWorkerGenerateGraph.CancelAsync();
        }

        /// <summary>
        /// Tell the system path and file name for the export of the scenario matrix and to trigger the export. 
        /// The parameter string contains the path under which the new file should be stored.
        /// </summary>
        /// <param name="filePath">destination path for the matrix</param>
        public void MatrixFilePath(string filePath)
        {
            //ToDo...
        }

        /// <summary>
        /// Tell the system path and file name for the export of the defect report and to trigger this. 
        /// The parameter string contains the path under which the new file should be stored.
        /// </summary>
        /// <param name="filePath">destination path for the defect report</param>
        public void ReportFilePath(string filePath)
        {
            //ToDo...
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}