// <copyright file="Controller.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Media;
    using RuleValidation.Errors;
    using RuleValidation.RucmRules;
    using UseCaseCore.RuleValidation;
    using UseCaseCore.ScenarioMatrix;
    using UseCaseCore.UcIntern;
    using UseCaseCore.XmlParser;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// Controller class
    /// </summary>
    public class Controller : INotifyPropertyChanged
    {
        /// <summary>
        /// The current file path
        /// </summary>
        private string currentFilePath = string.Empty;

        /// <summary>
        /// BackgroundWorker to load the file
        /// </summary>
        private BackgroundWorker backgroundWorkerLoadFile = new BackgroundWorker();

        /// <summary>
        /// BackgroundWorker to validate the file
        /// </summary>
        private BackgroundWorker backgroundWorkerValidFile = new BackgroundWorker();

        /// <summary>
        /// BackgroundWorker to generate the error report
        /// </summary>
        private BackgroundWorker backgroundWorkerGetErrorReport = new BackgroundWorker();

        /// <summary>
        /// BackgroundWorker to generate the matrix
        /// </summary>
        private BackgroundWorker backgroundWorkerGenerateMatrix = new BackgroundWorker();

        /// <summary>
        /// BackgroundWorker to generate the graph
        /// </summary>
        private BackgroundWorker backgroundWorkerGenerateGraph = new BackgroundWorker();

        /// <summary>
        /// Creates an instance of the ErrorReport class
        /// </summary>
        private IRucmRuleValidator ruleValidator;

        /// <summary>
        /// Creates an instance of the xml structure parser class
        /// </summary>
        private XmlStructureParser xmlParser;

        /// <summary>
        /// Creates an instance of the UseCase class
        /// </summary>
        private UseCase useCase = new UseCase();

        /// <summary>
        /// Creates an instance of the ScenarioMatrix class
        /// </summary>
        private ScenarioMatrix matrix;

        /// <summary>
        /// Creates an instance of the BackgroundColor class with default color
        /// </summary>
        private Brush backgroundColor1 = new SolidColorBrush(Color.FromArgb(255, 65, 177, 255));

        /// <summary>
        /// Creates an instance of the BackgroundColor class with default color
        /// </summary>
        private Brush backgroundColor2 = new SolidColorBrush(Color.FromArgb(255, 65, 177, 255));

        /// <summary>
        /// Creates an instance of the BackgroundColor class with default color
        /// </summary>
        private Brush backgroundColor3 = new SolidColorBrush(Color.FromArgb(255, 65, 177, 255));

        /// <summary>
        /// Creates an instance of the BackgroundColor class with default color
        /// </summary>
        private Brush backgroundColor4 = new SolidColorBrush(Color.FromArgb(255, 65, 177, 255));

        /// <summary>
        /// Creates an instance of the BackgroundColor class with default color
        /// </summary>
        private Brush backgroundColor5 = new SolidColorBrush(Color.FromArgb(255, 65, 177, 255));

        /// <summary>
        /// The current cycle depth
        /// </summary>
        private uint currentCycleDepth = 1;

        private Visibility visibilityOk = Visibility.Hidden;
        private Visibility visibilityFail = Visibility.Hidden;

        /// <summary>
        /// If no graph is drawn, the cancel button is ineffective
        /// </summary>
        private bool cancelButtonEnabled = false;

        /// <summary>
        /// If no graph is drawn, the cycle depth cannot be changed
        /// </summary>
        private bool matrixCycleDepthEnabled = false;

        /// <summary>
        /// If no graph is drawn, the graph button is ineffective
        /// </summary>
        private bool graphButtonsEnabled = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Controller"/> class.
        /// </summary>
        public Controller()
        {
            ruleValidator = new RucmRuleValidator(RucmRuleRepository.Rules);
            xmlParser = new XmlStructureParser(ruleValidator);
        }

        /// <summary>
        /// Fires when new scenarios were created
        /// </summary>
        public event Action<List<Scenario>> ScenariosCreated;

        /// <summary>
        /// Fires when writing an error report
        /// </summary>
        public event Action<List<IError>> WriteErrorReport;

        /// <summary>
        /// Fires when new scenarios were created
        /// </summary>
        public event Func<UseCase, bool> GraphCreated;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CancelButtonEnabled
        {
            get
            {
                return this.cancelButtonEnabled;
            }

            set
            {
                this.cancelButtonEnabled = value;
                this.OnPropertyChanged();
            }
        }

        public bool MatrixCycleDepthEnabled
        {
            get
            {
                return this.matrixCycleDepthEnabled;
            }

            set
            {
                this.matrixCycleDepthEnabled = value;
                this.OnPropertyChanged();
            }
        }

        public bool GraphButtonsEnabled
        {
            get
            {
                return this.graphButtonsEnabled;
            }

            set
            {
                this.graphButtonsEnabled = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the background color of the labelImportProcess
        /// </summary>
        public Brush BackgroundColor1
        {
            get
            {
                return this.backgroundColor1;
            }

            set
            {
                this.backgroundColor1 = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the background color of the labelValiProcess
        /// </summary>
        public Brush BackgroundColor2
        {
            get
            {
                return this.backgroundColor2;
            }

            set
            {
                this.backgroundColor2 = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the background color of the labelMatrixProcess
        /// </summary>
        public Brush BackgroundColor3
        {
            get
            {
                return this.backgroundColor3;
            }

            set
            {
                this.backgroundColor3 = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the background color of the labelGraphProcess
        /// </summary>
        public Brush BackgroundColor4
        {
            get
            {
                return this.backgroundColor4;
            }

            set
            {
                this.backgroundColor4 = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the background color of the labelReportProcess
        /// </summary>
        public Brush BackgroundColor5
        {
            get
            {
                return this.backgroundColor5;
            }

            set
            {
                this.backgroundColor5 = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the ok image1
        /// </summary>
        public Visibility VisibilityOk1
        {
            get
            {
                return this.visibilityOk;
            }

            set
            {
                this.visibilityOk = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the ok image2
        /// </summary>
        public Visibility VisibilityOk2
        {
            get
            {
                return this.visibilityOk;
            }

            set
            {
                this.visibilityOk = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the ok image3
        /// </summary>
        public Visibility VisibilityOk3
        {
            get
            {
                return this.visibilityOk;
            }

            set
            {
                this.visibilityOk = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the ok image4
        /// </summary>
        public Visibility VisibilityOk4
        {
            get
            {
                return this.visibilityOk;
            }

            set
            {
                this.visibilityOk = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the ok image5
        /// </summary>
        public Visibility VisibilityOk5
        {
            get
            {
                return this.visibilityOk;
            }

            set
            {
                this.visibilityOk = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the fail image1
        /// </summary>
        public Visibility VisibilityFail1
        {
            get
            {
                return this.visibilityFail;
            }

            set
            {
                this.visibilityFail = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the fail image2
        /// </summary>
        public Visibility VisibilityFail2
        {
            get
            {
                return this.visibilityFail;
            }

            set
            {
                this.visibilityFail = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the fail image3
        /// </summary>
        public Visibility VisibilityFail3
        {
            get
            {
                return this.visibilityFail;
            }

            set
            {
                this.visibilityFail = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the fail image4
        /// </summary>
        public Visibility VisibilityFail4
        {
            get
            {
                return this.visibilityFail;
            }

            set
            {
                this.visibilityFail = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the fail image5
        /// </summary>
        public Visibility VisibilityFail5
        {
            get
            {
                return this.visibilityFail;
            }

            set
            {
                this.visibilityFail = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Tell the system path and file name of the XML UseCase file. The string para-meter contains the path to the new file.
        /// </summary>
        /// <param name="filePath">The currently selected path</param>
        public void CurrentXmlFilePath(string filePath)
        {
            if (this.currentFilePath != string.Empty)
            {
                ResetPreviousContent();
            }

            currentFilePath = filePath;
            this.CancelButtonEnabled = true;

            if (!backgroundWorkerValidFile.CancellationPending)
            {
                this.backgroundWorkerLoadFile.WorkerSupportsCancellation = true;
                this.backgroundWorkerLoadFile.DoWork += new DoWorkEventHandler(BackgroundWorkerLoadFile_DoWork);
            }

            //this.CancelButtonEnabled = true;

            if (!this.backgroundWorkerLoadFile.IsBusy)
            {
                this.backgroundWorkerLoadFile.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Cancel all operation
        /// </summary>
        public void CancelOperation()
        {
            if (this.backgroundWorkerLoadFile.IsBusy)
            {
                this.backgroundWorkerLoadFile.CancelAsync();
            }

            if (this.backgroundWorkerValidFile.IsBusy)
            {
                this.backgroundWorkerValidFile.CancelAsync();
            }

            if (this.backgroundWorkerGetErrorReport.IsBusy)
            {
                this.backgroundWorkerGetErrorReport.CancelAsync();
            }

            if (this.backgroundWorkerGenerateMatrix.IsBusy)
            {
                this.backgroundWorkerGenerateMatrix.CancelAsync();
            }

            if (this.backgroundWorkerGenerateGraph.IsBusy)
            {
                this.backgroundWorkerGenerateGraph.CancelAsync();
            }
        }

        /// <summary>
        /// Tell the system path and file name for the export of the scenario matrix and to trigger the export. 
        /// The parameter string contains the path under which the new file should be stored.
        /// </summary>
        /// <param name="filePath">destination path for the matrix</param>
        public void MatrixFilePath(string filePath)
        {
            // Usecase da????
            if (filePath != string.Empty)
            {
                this.matrix.Export(filePath);
            }
            else
            {
                MessageBox.Show("Keine Datei eingelesen.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Tell the system path and file name for the export of the defect report and to trigger this. 
        /// The parameter string contains the path under which the new file should be stored.
        /// </summary>
        /// <param name="filePath">destination path for the defect report</param>
        public void ReportFilePath(string filePath)
        {
            ////ToDo...
        }

        /// <summary>
        /// Change cycle depth process
        /// </summary>
        /// <param name="depth">Cycle depth</param>
        public void ChangeCycleDepth(uint depth)
        {
            if (depth != this.currentCycleDepth & depth >= 0)
            {
                this.currentCycleDepth = depth;
                this.matrix.CycleDepth = this.currentCycleDepth;
                ////this.backgroundWorkerGenerateMatrix = new BackgroundWorker();
                ////this.backgroundWorkerGenerateMatrix.DoWork += new DoWorkEventHandler(backgroundWorkerGenerateMatrix_DoWork);
                ////this.backgroundWorkerGenerateMatrix.WorkerSupportsCancellation = true;

                if (!this.backgroundWorkerGenerateMatrix.IsBusy)
                {
                    this.backgroundWorkerGenerateMatrix.RunWorkerAsync();
                }
            }
        }

        /// <summary>
        /// Update a Scenario with the hnew value from the GUI
        /// </summary>
        /// <param name="s"></param>
        public void UpdateScenario(Scenario s)
        {
            this.matrix.UpdateScenarioComment(s);
        }

        /// <summary>
        /// process to reset previous content
        /// </summary>
        void ResetPreviousContent()
        {
            this.BackgroundColor1 = new SolidColorBrush(Color.FromArgb(255, 65, 177, 255));
            this.BackgroundColor2 = new SolidColorBrush(Color.FromArgb(255, 65, 177, 255));
            this.BackgroundColor3 = new SolidColorBrush(Color.FromArgb(255, 65, 177, 255));
            this.BackgroundColor4 = new SolidColorBrush(Color.FromArgb(255, 65, 177, 255));
            this.BackgroundColor5 = new SolidColorBrush(Color.FromArgb(255, 65, 177, 255));

            //ToDo: reset Fehlerbericht
            this.ruleValidator.Reset();
            // neu zeichnen??!?!?!

            this.backgroundWorkerLoadFile.DoWork -= new DoWorkEventHandler(BackgroundWorkerLoadFile_DoWork);
            this.backgroundWorkerValidFile.DoWork -= new DoWorkEventHandler(BackgroundWorkerValidFile_DoWork);
            this.backgroundWorkerGetErrorReport.DoWork -= new DoWorkEventHandler(BackgroundWorkerGetErrorReport_DoWork);
            this.backgroundWorkerGenerateGraph.DoWork -= new DoWorkEventHandler(BackgroundWorkerGenerateGraph_DoWork);
            this.backgroundWorkerGenerateMatrix.DoWork -= new DoWorkEventHandler(BackgroundWorkerGenerateMatrix_DoWork);
            //...
        }

        /// <summary>
        ///  BackgroundWorker to load the file
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void BackgroundWorkerLoadFile_DoWork(object sender, DoWorkEventArgs e)
        {
            //Task.Delay(2000).Wait();
            if ((!backgroundWorkerLoadFile.CancellationPending && !backgroundWorkerValidFile.CancellationPending))
            {
                if (this.xmlParser.LoadXmlFile(currentFilePath))
                {
                    this.BackgroundColor1 = Brushes.LimeGreen;
                    this.VisibilityOk1 = Visibility.Visible;

                    this.backgroundWorkerValidFile.WorkerSupportsCancellation = true;
                    this.backgroundWorkerValidFile.DoWork += new DoWorkEventHandler(BackgroundWorkerValidFile_DoWork);

                    if (!this.backgroundWorkerValidFile.IsBusy)
                    {
                        this.backgroundWorkerValidFile.RunWorkerAsync();
                    }
                }
                else
                {
                    this.BackgroundColor1 = Brushes.Red;
                    this.VisibilityFail1 = Visibility.Visible;
                    MessageBox.Show("Fehler beim Einlesen der Datei.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vorgang wurde abgebrochen.", "Abbruch", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// BackgroundWorker to validate the file
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void BackgroundWorkerValidFile_DoWork(object sender, DoWorkEventArgs e)
        {
            //Task.Delay(2000).Wait();
            if ((!backgroundWorkerValidFile.CancellationPending) && (!backgroundWorkerLoadFile.CancellationPending))
            {

                if (this.xmlParser.ParseXmlFile(out this.useCase))
                {
                    this.BackgroundColor2 = Brushes.LimeGreen;
                    this.VisibilityOk2 = Visibility.Visible;
                }
                else
                {
                    this.BackgroundColor2 = Brushes.Red;
                    this.VisibilityFail2 = Visibility.Visible;
                    //MessageBox.Show("Fehler beim Validieren der Datei.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                this.backgroundWorkerGetErrorReport.DoWork += new DoWorkEventHandler(this.BackgroundWorkerGetErrorReport_DoWork);
                this.backgroundWorkerGetErrorReport.WorkerSupportsCancellation = true;

                if (!this.backgroundWorkerGetErrorReport.IsBusy)
                {
                    this.backgroundWorkerGetErrorReport.RunWorkerAsync();
                }
            }
            else
            {
                MessageBox.Show("Vorgang wurde abgebrochen.", "Abbruch", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// BackgroundWorker to generate the error report
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void BackgroundWorkerGetErrorReport_DoWork(object sender, DoWorkEventArgs e)
        {
            //Task.Delay(2000).Wait();
            if ((!backgroundWorkerGetErrorReport.CancellationPending) && (!backgroundWorkerValidFile.CancellationPending) && (!backgroundWorkerLoadFile.CancellationPending))
            {
                //this.ruleValidator.AddExternalError("Beispiel Fehler");
                ErrorReport errorReport = this.ruleValidator.GetErrorReport();
                List<IError> errorList = errorReport.GetErrorList;
                if (errorList.Count > 0)
                {
                    this.BackgroundColor5 = Brushes.Red;
                    this.VisibilityFail5 = Visibility.Visible;
                    MessageBox.Show("Fehler in UseCase aufgetreten. Mängelbericht für weitere Infos abrufen.", "Fehler" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    this.BackgroundColor5 = Brushes.LimeGreen;
                    this.VisibilityOk5 = Visibility.Visible;
                    this.MatrixCycleDepthEnabled = true;

                    this.backgroundWorkerGenerateGraph.DoWork += new DoWorkEventHandler(this.BackgroundWorkerGenerateGraph_DoWork);
                    this.backgroundWorkerGenerateGraph.WorkerSupportsCancellation = true;

                    this.backgroundWorkerGenerateMatrix.DoWork += new DoWorkEventHandler(this.BackgroundWorkerGenerateMatrix_DoWork);
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

                if (this.WriteErrorReport != null)
                {
                    this.WriteErrorReport(errorList);
                }
            }
            else
            {
                MessageBox.Show("Vorgang wurde abgebrochen.", "Abbruch", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// BackgroundWorker to generate the graph
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void BackgroundWorkerGenerateGraph_DoWork(object sender, DoWorkEventArgs e)
        {
            //Task.Delay(2000).Wait();
            if ((!backgroundWorkerGenerateGraph.CancellationPending) && (!backgroundWorkerValidFile.CancellationPending) && (!backgroundWorkerLoadFile.CancellationPending))
            {
                if(this.GraphCreated(this.useCase))
                {
                    this.BackgroundColor4 = Brushes.LimeGreen;
                    this.VisibilityOk4 = Visibility.Visible;
                    this.GraphButtonsEnabled = true;
                }
                else
                {
                    this.BackgroundColor4 = Brushes.Red;
                    this.VisibilityFail4 = Visibility.Visible;
                    MessageBox.Show("Fehler beim Erstellen des Graphen.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vorgang wurde abgebrochen.", "Abbruch", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// BackgroundWorker to generate the matrix
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void BackgroundWorkerGenerateMatrix_DoWork(object sender, DoWorkEventArgs e)
        {
            //Task.Delay(2000).Wait();
            if ((!backgroundWorkerGenerateMatrix.CancellationPending) && (!backgroundWorkerValidFile.CancellationPending) && (!backgroundWorkerLoadFile.CancellationPending))
            {
                this.matrix = new ScenarioMatrix(this.useCase, this.currentCycleDepth);
                this.matrix.ScenariosCreated += this.Matrix_scenariosCreated;
                if (this.matrix.CreateScenarios())
                {
                    this.BackgroundColor3 = Brushes.LimeGreen;
                    this.VisibilityOk3 = Visibility.Visible;
                    this.MatrixCycleDepthEnabled = true;
                }
                else
                {
                    this.BackgroundColor3 = Brushes.Red;
                    this.VisibilityFail3 = Visibility.Visible;
                    MessageBox.Show("Fehler beim Erstellen der Szenariomatrix aufgetreten.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //else
            //{
            //    MessageBox.Show("Vorgang wurde abgebrochen.", "Abbruch", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        /// <summary>
        /// When new Scenarios got created
        /// </summary>
        /// <param name="obj"> Scenarios to be drawn </param>
        private void Matrix_scenariosCreated(System.Collections.Generic.List<Scenario> obj)
        {
            if (this.ScenariosCreated != null)
            {
                this.ScenariosCreated(obj);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}