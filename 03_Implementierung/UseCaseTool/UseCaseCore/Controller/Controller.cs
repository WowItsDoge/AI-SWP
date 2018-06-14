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

        private BackgroundWorker backgroundWorkerLoadFile = new BackgroundWorker();
        private BackgroundWorker backgroundWorkerValidFile = new BackgroundWorker();
        private BackgroundWorker backgroundWorkerGetErrorReport = new BackgroundWorker();
        private BackgroundWorker backgroundWorkerGenerateMatrix = new BackgroundWorker();
        private BackgroundWorker backgroundWorkerGenerateGraph = new BackgroundWorker();

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

        private uint currentCycleDepth = 1;

        private Visibility visibilityOk = Visibility.Hidden;
        private Visibility visibilityFail = Visibility.Hidden;

        /// <summary>
        /// Fires when new scenarios were created
        /// </summary>
        public event Action<List<Scenario>> ScenariosCreated;

        /// <summary>
        /// Fires when new scenarios were created
        /// </summary>
        public event Action<UseCase> GraphCreated;

        private bool cancelButtonEnabled = false;
        public bool ButtonEnabled
        {
            get
            {
                return cancelButtonEnabled;
            }
            set
            {
                cancelButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool matrixCycleDepthEnabled = false;
        public bool MatrixCycleDepthEnabled
        {
            get
            {
                return matrixCycleDepthEnabled;
            }
            set
            {
                matrixCycleDepthEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool graphButtonsEnabled = false;
        public bool GraphButtonsEnabled
        {
            get
            {
                return graphButtonsEnabled;
            }
            set
            {
                graphButtonsEnabled = value;
                OnPropertyChanged();
            }
        }

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


        private void ResetPreviousContent()
        {
            this.BackgroundColor1 = Brushes.Gray;
            //ToDo
            //...

        }

        /// <summary>
        /// Tell the system path and file name of the XML UseCase file. The string parameter contains the path to the new file.
        /// </summary>
        /// <param name="filePath">The currently selected path</param>
        public void CurrentXmlFilePath(string filePath)
        {
            if (currentFilePath != "")
            {
                this.ResetPreviousContent();
            }           
            
            currentFilePath = filePath;

            this.backgroundWorkerLoadFile.DoWork += new DoWorkEventHandler(backgroundWorkerLoadFile_DoWork);
            this.backgroundWorkerLoadFile.WorkerSupportsCancellation = true;

            if (!this.backgroundWorkerLoadFile.IsBusy)
            {
                this.backgroundWorkerLoadFile.RunWorkerAsync();
            }
        }


        private void backgroundWorkerLoadFile_DoWork(object sender, DoWorkEventArgs e)
        {
            // bool test = this.xmlParser.LoadXmlFile(currentFilePath);

            //// Anmerkung Christopher: LoadXmlFile()-Funktion wird 2x hintereinander aufgerufen,
            //// wenn bei bereits geöffneten Programm nochmal eine UseCase-Datei eingelesen werden soll.
            //// Sieht man ganz schön im "Ausgabefenster" wenn das Programm läuft (XMLParser gibt jedesmal Meldung aus, wenn LoadXmlFile()-Funktion aufgerufen wird).
            //// Sofern das mit dem 2 maligen Aufrufen der LoadXmlFile()-Funktion gefixt ist, sollte dann auch der erneute Einlesevorgang (beim 2., 3., 4., ... mal) funktionieren (aktuell wird ja noch eine Exception im "XMLParser" geworfen)

            if (this.xmlParser.LoadXmlFile(currentFilePath))
            {
                this.BackgroundColor1 = Brushes.LimeGreen;
                this.VisibilityOk1 = Visibility.Visible;
                this.ButtonEnabled = true;

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
            //delay(10000);
            if (this.xmlParser.ParseXmlFile(out this.useCase))
            {
                this.BackgroundColor2 = Brushes.LimeGreen;
                this.VisibilityOk2 = Visibility.Visible;

                this.backgroundWorkerGetErrorReport.DoWork += new DoWorkEventHandler(backgroundWorkerGetErrorReport_DoWork);
                this.backgroundWorkerGetErrorReport.WorkerSupportsCancellation = true;

                this.backgroundWorkerGenerateGraph.DoWork += new DoWorkEventHandler(backgroundWorkerGenerateGraph_DoWork);
                this.backgroundWorkerGenerateGraph.WorkerSupportsCancellation = true;

                this.backgroundWorkerGenerateMatrix.DoWork += new DoWorkEventHandler(backgroundWorkerGenerateMatrix_DoWork);
                this.backgroundWorkerGenerateMatrix.WorkerSupportsCancellation = true;

                if (!this.backgroundWorkerGetErrorReport.IsBusy)
                {
                    this.backgroundWorkerGetErrorReport.RunWorkerAsync();
                }

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
                this.VisibilityFail2 = Visibility.Visible;
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
            this.GraphCreated(this.useCase);
            this.GraphButtonsEnabled = true;
        }

        private void backgroundWorkerGenerateMatrix_DoWork(object sender, DoWorkEventArgs e)
        {
            matrix = new ScenarioMatrix(useCase, currentCycleDepth);
            matrix.ScenariosCreated += Matrix_scenariosCreated;
            if (matrix.CreateScenarios())
            {
                this.BackgroundColor3 = Brushes.LimeGreen;
                this.VisibilityOk3 = Visibility.Visible;
                this.MatrixCycleDepthEnabled = true;
            }
            else
            {
                this.BackgroundColor3 = Brushes.Red;
                this.VisibilityFail3 = Visibility.Visible;
            }
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

        /// <summary>
        /// Cancel process
        /// </summary>
        public void CancelProcess()
        {
            if(backgroundWorkerLoadFile.IsBusy)
            {
                backgroundWorkerLoadFile.CancelAsync();
            }
            if (backgroundWorkerValidFile.IsBusy)
            {
                backgroundWorkerValidFile.CancelAsync();
            }
            if (backgroundWorkerGetErrorReport.IsBusy)
            {
                backgroundWorkerGetErrorReport.CancelAsync();
            }
            if (backgroundWorkerGenerateMatrix.IsBusy)
            {
                backgroundWorkerGenerateMatrix.CancelAsync();
            }
            if (backgroundWorkerGenerateGraph.IsBusy)
            {
                backgroundWorkerGenerateGraph.CancelAsync();
            }
        }

        /// <summary>
        /// Tell the system path and file name for the export of the scenario matrix and to trigger the export. 
        /// The parameter string contains the path under which the new file should be stored.
        /// </summary>
        /// <param name="filePath">destination path for the matrix</param>
        public void MatrixFilePath(string filePath)
        {
            this.matrix.Export(filePath);
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

        public void ChangeCycleDepth(uint depth)
        {
            if(depth != currentCycleDepth & depth >= 0)
            {
                currentCycleDepth = depth;
                matrix.CycleDepth = currentCycleDepth;
                //this.backgroundWorkerGenerateMatrix = new BackgroundWorker();
                //this.backgroundWorkerGenerateMatrix.DoWork += new DoWorkEventHandler(backgroundWorkerGenerateMatrix_DoWork);
                //this.backgroundWorkerGenerateMatrix.WorkerSupportsCancellation = true;

                if (!this.backgroundWorkerGenerateMatrix.IsBusy)
                {
                    this.backgroundWorkerGenerateMatrix.RunWorkerAsync();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}



//ToDo...
// - Graph anzeigen
// - Exeption wenn weitere Datei eingelesen wird!!!!!!!!!!!!!!!!!!!!!!
// - Reset der Anzeige wenn weitere Datei eingelesen wird...
// - ... 
