// <copyright file="XmlStructureParser.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>
namespace UseCaseCore.XmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Xml;
    using DocumentFormat.OpenXml.Packaging;
    using OpenXmlPowerTools;
    using RuleValidation;
    using UcIntern;

    /// <summary>
    /// The xml structure parser instance.
    /// </summary>
    public class XmlStructureParser
    {
        /// <summary>
        /// The current error message.
        /// </summary>
        private string errorMessage;

        /// <summary>
        /// The use case name.
        /// </summary>
        private string useCaseName;

        /// <summary>
        /// A short description of the use case.
        /// </summary>
        private string briefDescription;

        /// <summary>
        /// The precondition of the use case.
        /// </summary>
        private string precondition;

        /// <summary>
        /// The primary Actor of the use case.
        /// </summary>
        private string primaryActor;

        /// <summary>
        /// The secondary Actor of the use case.
        /// </summary>
        private string secondaryActor;

        /// <summary>
        /// The dependency of the use case.
        /// </summary>
        private string dependency;

        /// <summary>
        /// The generalization of the use case.
        /// </summary>
        private string generalization;

        /// <summary>
        /// The basic flow of the use case.
        /// </summary>
        private Flow basicFlow;

        /// <summary>
        /// A list of the global alternative flows of the use case.
        /// </summary>
        private List<Flow> globalAlternativeFlows;

        /// <summary>
        /// A list of the specific alternative flows of the use case.
        /// </summary>
        private List<Flow> specificAlternativeFlows;

        /// <summary>
        /// A list of the bounded alternative flows of the use case.
        /// </summary>
        private List<Flow> boundedAlternativeFlows;

        /// <summary>
        /// Creates an instance of the UseCase class
        /// </summary>
        private UseCase outgoingUseCase = new UseCase();

        /// <summary>
        /// The word processing document for the use case file.
        /// </summary>
        private WordprocessingDocument useCaseFile;

        /// <summary>
        /// The loaded xml document.
        /// </summary>
        private XmlDocument useCaseXml;

        /// <summary>
        /// The file path of the UseCase File
        /// </summary>
        private string useCaseFilePath;

        /// <summary>
        /// The RUCM rule validator is passed in the constructor
        /// </summary>
        private IRucmRuleValidator rucmRuleValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlStructureParser" /> class.
        /// </summary>
        /// <param name="rucmRuleValidator">The rule validator is passed in the constructor</param>
        public XmlStructureParser(IRucmRuleValidator rucmRuleValidator)
        {
            this.InitXmlParser();
            this.useCaseFile = null;
            this.useCaseFilePath = string.Empty;
            this.rucmRuleValidator = rucmRuleValidator;
        }

        /// <summary>
        /// Loads external (word) xml file, which is stored on a storage medium. The absolute path to the file must be passed as parameter.
        /// </summary>
        /// <param name="filePath">Specifies the path for the xml file the user wants to load.</param>
        /// <returns>Returns true if the file exists and could be loaded, otherwise false.</returns>
        public bool LoadXmlFile(string filePath)
        {
            try
            {
                //// Copy usecase xml-file to windows user temp folder to fix the problem that the file is opened in write access
                string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                string newFilePath = Path.Combine(Path.GetTempPath(), fileName);
                File.Copy(filePath, newFilePath, true);
                this.useCaseFilePath = newFilePath;

                //// Open and load usecase xml-file
                this.useCaseFile = WordprocessingDocument.Open(this.useCaseFilePath, true);

                //// Set and apply settings for opended and loaded usecase xml-file
                SimplifyMarkupSettings settings = new SimplifyMarkupSettings
                {
                    AcceptRevisions = false,
                    RemoveContentControls = true,
                    RemoveSmartTags = true,
                    RemoveRsidInfo = true,
                    RemoveComments = true,
                    RemoveEndAndFootNotes = true,
                    ReplaceTabsWithSpaces = true,
                    RemoveFieldCodes = false,
                    RemovePermissions = true,
                    RemoveProof = true,
                    RemoveSoftHyphens = true,
                    RemoveLastRenderedPageBreak = true,
                    RemoveBookmarks = true,
                    RemoveWebHidden = true,
                    RemoveGoBackBookmark = true,
                    RemoveMarkupForDocumentComparison = true,
                    NormalizeXml = true,
                };
                MarkupSimplifier.SimplifyMarkup(this.useCaseFile, settings);

                return true;
            }
            catch (Exception ex)
            {
                //// General error while loading the usecase xml-file

                //// Set the error message
                this.errorMessage = "Fehler beim Laden der UseCase-Datei: " + "\"" + ex.Message.ToString() + "\"";

                //// Close usecase file and delete temporary file from windows user temp folder
                File.Delete(this.useCaseFilePath);

                return false;
            }
        }

        /// <summary>
        /// If error has occurred, the error text can be read out.
        /// </summary>
        /// <returns>Returns a string with the error message, but only if there was an error.</returns>
        public string GetError()
        {
            return this.errorMessage;
        }

        /// <summary>
        /// Initializes the properties. Can be used to reset everything.
        /// </summary>
        public void InitXmlParser()
        {
            this.errorMessage = string.Empty;
            this.useCaseName = string.Empty;
            this.briefDescription = string.Empty;
            this.precondition = string.Empty;
            this.primaryActor = string.Empty;
            this.secondaryActor = string.Empty;
            this.dependency = string.Empty;
            this.generalization = string.Empty;
            this.basicFlow = new Flow();
            this.globalAlternativeFlows = new List<Flow>();
            this.specificAlternativeFlows = new List<Flow>();
            this.boundedAlternativeFlows = new List<Flow>();
            this.useCaseXml = new XmlDocument();
        }

        /// <summary>
        /// Analyzes the previously read xml file.
        /// </summary>
        /// <param name="useCase">Out parameter for the whole internal use case representation.</param>
        /// <returns>Returns true if the file was analyzed successfully, otherwise false.</returns>
        public bool ParseXmlFile(out UseCase useCase)
        {
            this.InitXmlParser();

            try
            {
                //// Load usecase xml-file and normalize xml-structure
                this.useCaseXml.LoadXml(this.useCaseFile.MainDocumentPart.Document.InnerXml);
                this.useCaseXml.Normalize();

                if (this.useCaseXml.DocumentElement.ChildNodes == null)
                {
                    //// Clear internal usecase structure
                    useCase = null;

                    //// Set the error message
                    this.errorMessage = "UseCase-Dateistruktur defekt! (Die Datei enthält keine XML-Child-Nodes)";

                    //// Close usecase file and delete temporary file from windows user temp folder
                    this.useCaseFile.Close();
                    File.Delete(this.useCaseFilePath);

                    return false;
                }

                //// Read out usecase properties
                this.useCaseName = this.ParseRucmProperty("Use Case Name");
                this.briefDescription = this.ParseRucmProperty("Brief Description");
                this.precondition = this.ParseRucmProperty("Precondition");
                this.primaryActor = this.ParseRucmProperty("Primary Actor");
                this.secondaryActor = this.ParseRucmProperty("Secondary Actors");
                this.dependency = this.ParseRucmProperty("Dependency");
                this.generalization = this.ParseRucmProperty("Generalization");
                this.GetBasicFlow();
                this.GetGlobalAlternativeFlows();
                this.GetSpecificAlternativeFlows();
                this.GetBoundedAlternativeFlows();

                //// Close usecase file and delete temporary file from windows user temp folder
                this.useCaseFile.Close();
                File.Delete(this.useCaseFilePath);

                //// Validate the usecase properties and flows with the rucm rules
                bool rucmValidationResult = this.ValidateRucmRules();
                if (rucmValidationResult == false)
                {
                    //// Set the error message
                    this.errorMessage = "UseCase RUCM-Validierung fehlerhaft!";

                    //// Clear internal usecase structure
                    useCase = null;

                    return false;
                }

                //// Create internal usecase structure
                this.SetOutgoingUseCaseParameter();

                //// Pass out the internal usecase structure
                useCase = this.outgoingUseCase;

                return true;

            }
            catch (Exception ex)
            {
                //// General error while reading the usecase xml-file

                //// Clear internal usecase structure
                useCase = null;

                //// Set the error message
                this.errorMessage = "Fehler beim Auslesen der UseCase-Datei: " + "\"" + ex.Message.ToString() + "\"";

                //// Close usecase file and delete temporary file from windows user temp folder
                this.useCaseFile.Close();
                File.Delete(this.useCaseFilePath);

                return false;
            }
        }

        /// <summary>
        /// This function validates the RUCM rules. 
        /// </summary>
        /// <returns>Returns true if the validation was successful.</returns>
        private bool ValidateRucmRules()
        {
            return this.rucmRuleValidator.Validate(this.basicFlow, this.globalAlternativeFlows, this.specificAlternativeFlows, this.boundedAlternativeFlows);        
        }

        /// <summary>
        /// This function sets the parsed values for the use case.
        /// </summary>
        private void SetOutgoingUseCaseParameter()
        {
            //// Create internal usecase structure
            this.outgoingUseCase.UseCaseName = this.useCaseName;
            this.outgoingUseCase.BriefDescription = this.briefDescription;
            this.outgoingUseCase.Precondition = this.precondition;
            this.outgoingUseCase.PrimaryActor = this.primaryActor;
            this.outgoingUseCase.SecondaryActors = this.secondaryActor;
            this.outgoingUseCase.Dependency = this.dependency;
            this.outgoingUseCase.Generalization = this.generalization;
            this.outgoingUseCase.BasicFlow = this.basicFlow;
            this.outgoingUseCase.GlobalAlternativeFlows = this.globalAlternativeFlows;
            this.outgoingUseCase.SpecificAlternativeFlows = this.specificAlternativeFlows;
            this.outgoingUseCase.BoundedAlternativeFlows = this.boundedAlternativeFlows;
            this.outgoingUseCase.BuildGraph();
        }

        /// <summary>
        /// Parses the RUCM properties.
        /// </summary>
        /// <param name="propertyName">Specifies the property name to parse.</param>
        /// <returns>Returns a string with the parsed RUCM property.</returns>
        private string ParseRucmProperty(string propertyName)
        {
            XmlNodeList propertyNode = null;

            //// Get the xml node list for the usecase property name (only for property name, not for flows!)
            propertyNode = this.GetXmlNodeList(propertyName);

            //// Check for errors: Only one equal property allowed
            if (propertyNode.Count == 0)
            {
                throw new Exception("UseCase-Eigenschaft " + "\"" + propertyName + "\"" + " nicht gefunden!");
            }

            if (propertyNode.Count > 1)
            {
                throw new Exception("Mehr als eine UseCase-Eigenschaft " + "\"" + propertyName + "\"" + " gefunden!");
            }

            try
            {
                //// Get the content for the property name
                string propertyContent = propertyNode[0].ParentNode.ParentNode.ParentNode.ParentNode.ChildNodes[1].InnerText.Trim();
                return propertyContent;
            }
            catch
            {
                throw new Exception("Inhalt für " + "\"" + propertyName + "\"" + " nicht gefunden!");
            }
        }

        /// <summary>
        /// Gets the basic flow.
        /// </summary>
        private void GetBasicFlow()
        {
            XmlNodeList basicFlowNode = null;

            //// Get the xml node list for the flow
            basicFlowNode = this.GetXmlNodeList("Basic Flow");

            //// Check for errors: Only one basic flow allowed
            if (basicFlowNode.Count == 0)
            {
                throw new Exception("Keinen Basic-Flow gefunden!");
            }

            if (basicFlowNode.Count > 1)
            {
                throw new Exception("Mehr als einen Basic-Flow gefunden!");
            }

            try
            {
                //// Get the content for the flow
                XmlNode basicFlowContent = basicFlowNode[0].ParentNode.ParentNode.ParentNode.ParentNode;

                //// Read out the next step content inside the flow
                XmlNode basicFlowStepContent = basicFlowContent.NextSibling;

                List<Node> basicSteps = new List<Node>();

                //// Identifier for the flow is always zero
                FlowIdentifier basicIdentifier = new FlowIdentifier(FlowType.Basic, 0);

                //// Check if flow has ended
                while (this.FlowHasEnded(basicFlowStepContent) == false)
                {    
                    //// Check how many child nodes exists in current step content and read out the content
                    switch (basicFlowStepContent.ChildNodes.Count)
                    {
                        case 2:
                            //// Content has no step number --> for example "DO" or "IF ..."
                            basicSteps.Add(new Node(basicFlowStepContent.ChildNodes[1].InnerText.Trim(), basicIdentifier));
                            break;
                        case 3:
                            //// Content has a step number
                            basicSteps.Add(new Node(basicFlowStepContent.ChildNodes[2].InnerText.Trim(), basicIdentifier));
                            break;
                        default:
                            break;
                    }

                    //// Read out the next step content inside the flow
                    basicFlowStepContent = basicFlowStepContent.NextSibling;
                }

                //// Read out the postcondition
                string postcondition = string.Empty;
                if (basicFlowStepContent.ChildNodes.Count == 3)
                {
                    postcondition = basicFlowStepContent.ChildNodes[2].InnerText.Trim();
                }

                //// Create the internal basic flow
                this.basicFlow = new Flow(basicIdentifier, postcondition, basicSteps, new List<ReferenceStep>());
            }
            catch
            {
                throw new Exception("Inhalt für den Basic-Flow nicht gefunden!");
            }
        }

        /// <summary>
        /// Gets the global alternative flows.
        /// </summary>
        private void GetGlobalAlternativeFlows()
        {
            XmlNodeList globalAlternativeFlowNodes = null;

            //// Get the xml node list for the flow
            globalAlternativeFlowNodes = this.GetXmlNodeList("Global Alternative Flow");

            //// Exit if no flow exists
            if (globalAlternativeFlowNodes.Count == 0)
            {
                return;
            }

            try
            {
                //// Create temporary flow list
                List<Flow> temporaryFlowList = new List<Flow>();

                //// Read out every existing flow
                for (int i = 1; i <= globalAlternativeFlowNodes.Count; i++)
                {
                    List<Node> globalSteps = new List<Node>();

                    //// Identifier for the flow is the current loop runs
                    FlowIdentifier globalIdentifier = new FlowIdentifier(FlowType.GlobalAlternative, i);

                    //// Get the content for the flow
                    XmlNode globalAlternativeFlowContent = globalAlternativeFlowNodes[i - 1].ParentNode.ParentNode.ParentNode.ParentNode;
                    XmlNode globalAlternativeFlowStepContent = globalAlternativeFlowContent;

                    //// Check if flow has ended
                    while (this.FlowHasEnded(globalAlternativeFlowStepContent) == false)
                    {
                        //// Check how many child nodes exists in current step content and read out the content
                        switch (globalAlternativeFlowStepContent.ChildNodes.Count)
                        {
                            case 2:
                                //// Content has no step number --> for example "DO" or "IF ..."
                                globalSteps.Add(new Node(globalAlternativeFlowStepContent.ChildNodes[1].InnerText.Trim(), globalIdentifier));
                                break;
                            case 3:
                                //// Content has a step number
                                globalSteps.Add(new Node(globalAlternativeFlowStepContent.ChildNodes[2].InnerText.Trim(), globalIdentifier));
                                break;
                            default:
                                break;
                        }

                        //// Read out the next step content inside the flow
                        globalAlternativeFlowStepContent = globalAlternativeFlowStepContent.NextSibling;
                    }

                    //// Read out the postcondition
                    string postcondition = string.Empty;
                    if (globalAlternativeFlowStepContent.ChildNodes.Count == 3)
                    {
                        postcondition = globalAlternativeFlowStepContent.ChildNodes[2].InnerText.Trim();
                    }   
                         
                    //// Add the flow to temporary list       
                    temporaryFlowList.Add(new Flow(globalIdentifier, postcondition, globalSteps, new List<ReferenceStep>()));                   
                }

                //// Create the internal global alternative flows
                this.globalAlternativeFlows = temporaryFlowList;
            }
            catch
            {
                throw new Exception("Inhalt für den Global-Alternative-Flow nicht gefunden!");
            }
        }

        /// <summary>
        /// Gets the specific alternative flows.
        /// </summary>
        private void GetSpecificAlternativeFlows()
        {
            XmlNodeList specificAlternativeFlowNodes = null;

            //// Get the xml node list for the flow
            specificAlternativeFlowNodes = this.GetXmlNodeList("Specific Alternative Flow");

            //// Exit if no flow exists
            if (specificAlternativeFlowNodes.Count == 0)
            {
                return;
            }

            try
            {
                //// Create temporary flow list
                List<Flow> temporaryFlowList = new List<Flow>();

                //// Read out every existing flow
                for (int i = 1; i <= specificAlternativeFlowNodes.Count; i++)
                {
                    List<Node> specificSteps = new List<Node>();

                    //// Identifier for the flow is the current loop runs
                    FlowIdentifier specificIdentifier = new FlowIdentifier(FlowType.SpecificAlternative, i);

                    ReferenceStep referenceStep = new ReferenceStep();

                    //// Get the content for the flow
                    XmlNode specificAlternativeFlowContent = specificAlternativeFlowNodes[i - 1].ParentNode.ParentNode.ParentNode.ParentNode;
                    XmlNode specificAlternativeFlowStepContent = specificAlternativeFlowContent;

                    //// Check if flow has ended
                    while (this.FlowHasEnded(specificAlternativeFlowStepContent) == false)
                    {
                        //// Check how many child nodes exists in current step content and read out the content
                        switch (specificAlternativeFlowStepContent.ChildNodes.Count)
                        {
                            case 2:
                                //// Content has no step number --> for example "DO" or "IF ..."
                                string unparsedReferenceStep = specificAlternativeFlowStepContent.ChildNodes[1].InnerText.Trim().ToLower();
                                
                                //// Help query for rucm rule 19
                                if (unparsedReferenceStep.Contains("RFS".ToLower()))   
                                {
                                    unparsedReferenceStep = this.TrimReferenceStepNumber(unparsedReferenceStep);
                                    int referenceStepNumber = int.Parse(unparsedReferenceStep);
                                    FlowIdentifier flowIdentifier = new FlowIdentifier(FlowType.Basic, 0);
                                    referenceStep = new ReferenceStep(flowIdentifier, referenceStepNumber);
                                    break;
                                }

                                specificSteps.Add(new Node(specificAlternativeFlowStepContent.ChildNodes[1].InnerText.Trim(), specificIdentifier));
                                break;
                            case 3:
                                //// Content has a step number
                                specificSteps.Add(new Node(specificAlternativeFlowStepContent.ChildNodes[2].InnerText.Trim(), specificIdentifier));
                                break;
                            default:
                                break;
                        }

                        //// Read out the next step content inside the flow
                        specificAlternativeFlowStepContent = specificAlternativeFlowStepContent.NextSibling;
                    }

                    //// Read out the postcondition
                    string postcondition = string.Empty;
                    if (specificAlternativeFlowStepContent.ChildNodes.Count == 3)
                    {
                        postcondition = specificAlternativeFlowStepContent.ChildNodes[2].InnerText.Trim();
                    }

                    //// Add the flow to temporary list                      
                    temporaryFlowList.Add(new Flow(specificIdentifier, postcondition, specificSteps, new List<ReferenceStep>() { referenceStep }));
                }

                //// Create the internal specific alternative flows
                this.specificAlternativeFlows = temporaryFlowList;
            }
            catch
            {
                throw new Exception("Inhalt für den Specific-Alternative-Flow nicht gefunden!");
            }
        }

        /// <summary>
        /// Gets the bounded alternative flows.
        /// </summary>
        private void GetBoundedAlternativeFlows()
        {
            XmlNodeList boundedAlternativeFlowNodes = null;

            //// Get the xml node list for the flow
            boundedAlternativeFlowNodes = this.GetXmlNodeList("Bounded Alternative Flow");

            //// Exit if no flow exists
            if (boundedAlternativeFlowNodes.Count == 0)
            {
                return;
            }

            try
            {
                //// Create temporary flow list
                List<Flow> temporaryFlowList = new List<Flow>();

                //// Read out every existing flow
                for (int i = 1; i <= boundedAlternativeFlowNodes.Count; i++)
                {
                    List<Node> boundedSteps = new List<Node>();

                    //// Identifier for the flow is the current loop runs
                    FlowIdentifier boundedIdentifier = new FlowIdentifier(FlowType.BoundedAlternative, i);

                    List<ReferenceStep> referenceSteps = new List<ReferenceStep>();

                    //// Get the content for the flow
                    XmlNode boundedAlternativeFlowContent = boundedAlternativeFlowNodes[i - 1].ParentNode.ParentNode.ParentNode.ParentNode;
                    XmlNode boundedAlternativeFlowStepContent = boundedAlternativeFlowContent;

                    //// Check if flow has ended
                    while (this.FlowHasEnded(boundedAlternativeFlowStepContent) == false)
                    {
                        //// Check how many child nodes exists in current step content and read out the content
                        switch (boundedAlternativeFlowStepContent.ChildNodes.Count)
                        {
                            case 2:
                                //// Content has no step number --> for example "DO" or "IF ..."
                                string unparsedReferenceStep = boundedAlternativeFlowStepContent.ChildNodes[1].InnerText.Trim().ToLower();

                                //// Help query for rucm rule 19
                                if (unparsedReferenceStep.Contains("RFS".ToLower()) == true)   
                                {
                                    //// Check if reference step numbers are separated by hyphen
                                    //// For example: "RFS Basic Flow 3-6"
                                    string referenceStepNumbers = this.TrimReferenceStepNumber(unparsedReferenceStep);
                                    if (referenceStepNumbers.Contains("-") == true)
                                    {
                                        int stepStartNumber = int.Parse(referenceStepNumbers.Split('-')[0]);
                                        int stepEndNumber = int.Parse(referenceStepNumbers.Split('-')[1]);
                                        for (int n = stepStartNumber; n <= stepEndNumber; n++)
                                        {
                                            FlowIdentifier flowIdentifier = new FlowIdentifier(FlowType.Basic, 0);
                                            ReferenceStep referenceStep = new ReferenceStep(flowIdentifier, n);
                                            referenceSteps.Add(referenceStep);
                                        }
                                    }
                                    else
                                    {
                                        int referenceStepNumber = int.Parse(referenceStepNumbers);
                                        FlowIdentifier flowIdentifier = new FlowIdentifier(FlowType.Basic, 0);
                                        ReferenceStep referenceStep = new ReferenceStep(flowIdentifier, referenceStepNumber);
                                        referenceSteps.Add(referenceStep);
                                    }

                                    break;
                                }

                                boundedSteps.Add(new Node(boundedAlternativeFlowStepContent.ChildNodes[1].InnerText.Trim(), boundedIdentifier));
                                break;
                            case 3:
                                //// Content has a step number
                                boundedSteps.Add(new Node(boundedAlternativeFlowStepContent.ChildNodes[2].InnerText.Trim(), boundedIdentifier));
                                break;
                            default:
                                break;
                        }

                        //// Read out the next step content inside the flow
                        boundedAlternativeFlowStepContent = boundedAlternativeFlowStepContent.NextSibling;
                    }

                    //// Read out the postcondition
                    string postcondition = string.Empty;
                    if (boundedAlternativeFlowStepContent.ChildNodes.Count == 3)
                    {
                        postcondition = boundedAlternativeFlowStepContent.ChildNodes[2].InnerText.Trim();
                    }

                    //// Add the flow to temporary list  
                    temporaryFlowList.Add(new Flow(boundedIdentifier, postcondition, boundedSteps, referenceSteps));                
                }

                //// Create the internal bounded alternative flows
                this.boundedAlternativeFlows = temporaryFlowList;
            }
            catch
            {
                throw new Exception("Inhalt für den Bounded-Alternative-Flow nicht gefunden!");
            }
        }

        /// <summary>
        /// Get the xml node list for specified flow type
        /// </summary>
        /// <param name="searchWord">Defines the search word with which you want to search</param>
        /// <returns>Returns the XmlNodeList with the founded Node.</returns>
        private XmlNodeList GetXmlNodeList(string searchWord)
        {
            XmlNode root = this.useCaseXml.DocumentElement;
            XmlNodeList flowNodeList = null;

            //// Create a list for search words by trimming the search word string at the blank signs from the right on
            //// Improves the readout stability for the usecase quite a lot !!!
            List<string> searchWordList = new List<string>();
            int charPosition = searchWord.Length;
            while (charPosition > 0)
            {
                searchWord = searchWord.Substring(0, charPosition);
                searchWordList.Add(searchWord);
                charPosition = searchWord.LastIndexOf(' ');
            }

            for (int i = 1; i <= searchWordList.Count; i++)
            {
                //// Find xml node list
                string xPathFilter = "//*/text()[normalize-space(.)='" + searchWordList[i - 1] + "']/parent::*";
                flowNodeList = root.SelectNodes(xPathFilter);
                if (flowNodeList.Count > 0)
                {
                    //// Xml node list was found
                    break;
                }
            }

            return flowNodeList;
        }

        /// <summary>
        /// Checks if the flow has ended.
        /// </summary>
        /// <param name="flowStepContent">The step content of the current flow.</param>
        /// <returns>Returns true if the flow has ended, false otherwise.</returns>
        private bool FlowHasEnded(XmlNode flowStepContent)
        {
            //// Create keyword list
            //// Flow can not be ending with a keyword
            List<string> keyWords = new List<string>();
            keyWords.Add("IF");
            keyWords.Add("THEN");
            keyWords.Add("ELSE");
            keyWords.Add("ELSEIF");
            keyWords.Add("ENDIF");
            keyWords.Add("DO");
            keyWords.Add("UNTIL");
            keyWords.Add("RESUME");
            keyWords.Add("RESUME STEP");
            keyWords.Add("ABORT");
            keyWords.Add("INCLUDE USE CASE");
            keyWords.Add("EXTENDED BY USE CASE");
            keyWords.Add("MEANWHILE");
            keyWords.Add("VALIDATE THAT");

            int n;

            //// Check if usecase xml document is ending
            if (flowStepContent.NextSibling == null)
            {
                return true;
            }

            //// Check if current step content is the postcondition
            if (flowStepContent.ChildNodes[1].InnerText.Trim().ToLower() == "Postcondition".ToLower())
            {
                return true;
            }

            //// Check if current step content contains a keyword
            foreach (var keyword in keyWords)
            {
                if (flowStepContent.ChildNodes[1].InnerText.Trim().Contains(keyword) == true)
                {
                    return false;
                }
            }

            //// Check if current step content is a flow step
            if (int.TryParse(flowStepContent.ChildNodes[1].InnerText.Trim(), out n) == true)
            {
                return false;
            }

            //// Check if next step content contains a keyword
            foreach (var keyword in keyWords)
            {
                if (flowStepContent.NextSibling.ChildNodes[1].InnerText.Trim().Contains(keyword) == true)
                {
                    return false;
                }
            }

            //// Check if next step content is a flow step
            if (int.TryParse(flowStepContent.NextSibling.ChildNodes[1].InnerText.Trim(), out n) == true)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Trim the flow reference step number
        /// </summary>
        /// <param name="searchWord">Defines the search word with which you want to search</param>
        /// <returns>Returns the flow reference step number</returns>
        private string TrimReferenceStepNumber(string searchWord)
        {
            //// Replace the static word expressions from the search word
            searchWord = searchWord.Replace("RFS".ToLower(), string.Empty);
            searchWord = searchWord.Replace("Basic".ToLower(), string.Empty);
            searchWord = searchWord.Replace("Flow".ToLower(), string.Empty);
            searchWord = searchWord.Trim();
            return searchWord;
        }

    }
}