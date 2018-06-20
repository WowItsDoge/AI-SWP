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
                //// copy file to windows temp folder to fix the problem with write access if file is opened
                string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                string newFilePath = Path.Combine(Path.GetTempPath(), fileName);
                File.Copy(filePath, newFilePath, true);
                this.useCaseFilePath = newFilePath;

                this.useCaseFile = WordprocessingDocument.Open(this.useCaseFilePath, true);
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
                this.errorMessage = ex.Message.ToString();
                File.Delete(this.useCaseFilePath);
                return false;
            }
        }

        /// <summary>
        /// If error(return value = false) has occurred at function “LoadXmlFile()” or “TryToFixMalformedXml()”, the error text can be read out.
        /// </summary>
        /// <returns>Returns a string with the error message if there was an error.</returns>
        public string GetError()
        {
            return this.errorMessage;
        }

        /// <summary>
        /// If error(return value = false) has occurred at function “LoadXmlFile()” or “TryToFixMalformedXml()”, the error text can be read out.
        /// </summary>
        /// <returns>Returns a string with the error message if there was an error.</returns>
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
        /// If external xml file contains easy structural errors (for example missing bracket on line 50), it can be tried to repair automatically. Attention: Source file on storage medium will be overwritten!
        /// </summary>
        /// <returns>Returns true if the malformed xml could be fixed, otherwise false.</returns>
        public bool TryToFixMalformedXml()
        {
            return false;
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
                this.useCaseXml.LoadXml(this.useCaseFile.MainDocumentPart.Document.InnerXml);
                this.useCaseXml.Normalize(); //// TODO: useful?
                if (this.useCaseXml.DocumentElement.ChildNodes == null)
                {
                    useCase = null;
                    this.errorMessage = "Use case document corrupted (no child nodes found!)";
                    this.useCaseFile.Close();
                    File.Delete(this.useCaseFilePath);
                    return false;
                }

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

                bool rucmValidationResult = this.ValidateRucmRules();

                if (rucmValidationResult == true)
                {
                    this.SetOutgoingUseCaseParameter();
                }
                else
                {
                    this.errorMessage = "RUCM rule validation failed!";
                }

                this.useCaseFile.Close();
                File.Delete(this.useCaseFilePath);

                useCase = this.outgoingUseCase;
                return rucmValidationResult;
            }
            catch (Exception ex)
            {
                useCase = null;
                this.errorMessage = ex.Message.ToString();
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
            bool currentValidationResult = true;
            bool functionReturnValue = true;
            this.rucmRuleValidator.Validate(this.basicFlow);
            foreach (var globalAlternativeFlow in this.globalAlternativeFlows)
            {
                /*
                if (currentValidationResult == true)
                {
                    currentValidationResult = this.rucmRuleValidator.Validate(globalAlternativeFlow, this.basicFlow);
                }
                else
                {
                    break;
                }
                */
                currentValidationResult = this.rucmRuleValidator.Validate(globalAlternativeFlow, this.basicFlow);
                if (currentValidationResult == false)
                {
                    functionReturnValue = false;
                }
            }

            foreach (var specificAlternativeFlow in this.specificAlternativeFlows)
            {
                /*
                if (currentValidationResult == true)
                {
                    currentValidationResult = this.rucmRuleValidator.Validate(specificAlternativeFlow, this.basicFlow);
                }
                else
                {
                    break;
                }
                */
                currentValidationResult = this.rucmRuleValidator.Validate(specificAlternativeFlow, this.basicFlow);
                if (currentValidationResult == false)
                {
                    functionReturnValue = false;
                }
            }

            foreach (var boundedAlternativeFlow in this.boundedAlternativeFlows)
            {
                /*
                if (currentValidationResult == true)
                {
                    currentValidationResult = this.rucmRuleValidator.Validate(boundedAlternativeFlow, this.basicFlow);
                }
                else
                {
                    break;
                }
                */
                currentValidationResult = this.rucmRuleValidator.Validate(boundedAlternativeFlow, this.basicFlow);
                if (currentValidationResult == false)
                {
                    functionReturnValue = false;
                }
            }

            ////return currentValidationResult;
            return functionReturnValue;
        }

        /// <summary>
        /// This function sets the parsed values for the use case.
        /// </summary>
        private void SetOutgoingUseCaseParameter()
        {
            this.outgoingUseCase.UseCaseName = this.useCaseName;
            this.outgoingUseCase.BriefDescription = this.briefDescription;
            this.outgoingUseCase.Precondition = this.precondition;
            this.outgoingUseCase.PrimaryActor = this.primaryActor;
            this.outgoingUseCase.SecondaryActors = this.secondaryActor;
            this.outgoingUseCase.Dependency = this.dependency;
            this.outgoingUseCase.Generalization = this.generalization;
            this.outgoingUseCase.SetBasicFlow(this.basicFlow);
            this.outgoingUseCase.AddGlobalAlternativeFlow(this.globalAlternativeFlows);
            this.outgoingUseCase.AddSpecificAlternativeFlow(this.specificAlternativeFlows);
            this.outgoingUseCase.AddBoundedAlternativeFlow(this.boundedAlternativeFlows);
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
            propertyNode = this.GetXmlNodeList(propertyName);

            if (propertyNode.Count == 0)
            {
                throw new Exception("Error: Use-Case property " + "\"" + propertyName + "\"" + " not found");
            }

            if (propertyNode.Count > 1)
            {
                throw new Exception("Error: More than one Use-Case property " + "\"" + propertyName + "\"" + " found");
            }

            try
            {
                string propertyContent = propertyNode[0].ParentNode.ParentNode.ParentNode.ParentNode.ChildNodes[1].InnerText.Trim();
                return propertyContent;
            }
            catch
            {
                throw new Exception("Error: Content for " + "\"" + propertyName + "\"" + " not found");
            }
        }

        /// <summary>
        /// Gets the basic flow.
        /// </summary>
        private void GetBasicFlow()
        {
            XmlNodeList basicFlowNode = null;
            basicFlowNode = this.GetXmlNodeList("Basic Flow");

            if (basicFlowNode.Count == 0)
            {
                throw new Exception("Error: No Basic Flow found");
            }

            if (basicFlowNode.Count > 1)
            {
                throw new Exception("Error: Document contains more than one Basic Flow");
            }

            try
            {
                XmlNode basicFlowContent = basicFlowNode[0].ParentNode.ParentNode.ParentNode.ParentNode;
                XmlNode basicFlowStepContent = basicFlowContent.NextSibling;
                List<Node> basicSteps = new List<Node>();
                FlowIdentifier basicIdentifier = new FlowIdentifier(FlowType.Basic, 0);
                while (basicFlowStepContent.ChildNodes[1].InnerText.Trim().ToLower() != "Postcondition".ToLower())
                {          
                    switch (basicFlowStepContent.ChildNodes.Count)
                    {
                        case 2:
                            basicSteps.Add(new Node(basicFlowStepContent.ChildNodes[1].InnerText.Trim(), basicIdentifier));
                            break;
                        case 3:
                            basicSteps.Add(new Node(basicFlowStepContent.ChildNodes[2].InnerText.Trim(), basicIdentifier));
                            break;
                        default:
                            break;
                    }

                    basicFlowStepContent = basicFlowStepContent.NextSibling;
                }

                string postcondition = basicFlowStepContent.ChildNodes[2].InnerText.Trim();
                this.basicFlow = new Flow(basicIdentifier, postcondition, basicSteps, new List<ReferenceStep>());
            }
            catch
            {
                throw new Exception("Error: Content for basic flow not found");
            }
        }

        /// <summary>
        /// Gets the global alternative flows.
        /// </summary>
        private void GetGlobalAlternativeFlows()
        {
            XmlNodeList globalAlternativeFlowNodes = null;
            globalAlternativeFlowNodes = this.GetXmlNodeList("Global Alternative Flow");

            if (globalAlternativeFlowNodes.Count == 0)
            {
                return;
            }

            try
            {
                List<Flow> tempList = new List<Flow>();
                for (int i = 1; i <= globalAlternativeFlowNodes.Count; i++)
                {
                    List<Node> globalSteps = new List<Node>();
                    FlowIdentifier globalIdentifier = new FlowIdentifier(FlowType.GlobalAlternative, i);
                    XmlNode globalAlternativeFlowContent = globalAlternativeFlowNodes[i - 1].ParentNode.ParentNode.ParentNode.ParentNode;
                    XmlNode globalAlternativeFlowStepContent = globalAlternativeFlowContent;
                    while (globalAlternativeFlowStepContent.ChildNodes[1].InnerText.Trim().ToLower() != "Postcondition".ToLower())
                    {
                        switch (globalAlternativeFlowStepContent.ChildNodes.Count)
                        {
                            case 2:
                                globalSteps.Add(new Node(globalAlternativeFlowStepContent.ChildNodes[1].InnerText.Trim(), globalIdentifier));
                                break;
                            case 3:
                                globalSteps.Add(new Node(globalAlternativeFlowStepContent.ChildNodes[2].InnerText.Trim(), globalIdentifier));
                                break;
                            default:
                                break;
                        }

                        globalAlternativeFlowStepContent = globalAlternativeFlowStepContent.NextSibling;
                    }

                    string postcondition = globalAlternativeFlowStepContent.ChildNodes[2].InnerText.Trim();                   
                    tempList.Add(new Flow(globalIdentifier, postcondition, globalSteps, new List<ReferenceStep>()));                   
                }

                this.globalAlternativeFlows = tempList;
            }
            catch
            {
                throw new Exception("Error: Content for global alternative flow not found");
            }
        }

        /// <summary>
        /// Gets the specific alternative flows.
        /// </summary>
        private void GetSpecificAlternativeFlows()
        {
            XmlNodeList specificAlternativeFlowNodes = null;
            specificAlternativeFlowNodes = this.GetXmlNodeList("Specific Alternative Flow");

            if (specificAlternativeFlowNodes.Count == 0)
            {
                return;
            }

            try
            {
                List<Flow> tempList = new List<Flow>();
                for (int i = 1; i <= specificAlternativeFlowNodes.Count; i++)
                {
                    List<Node> specificSteps = new List<Node>();
                    FlowIdentifier specificIdentifier = new FlowIdentifier(FlowType.SpecificAlternative, i);
                    ReferenceStep referenceStep = new ReferenceStep();
                    XmlNode specificAlternativeFlowContent = specificAlternativeFlowNodes[i - 1].ParentNode.ParentNode.ParentNode.ParentNode;
                    XmlNode specificAlternativeFlowStepContent = specificAlternativeFlowContent;
                    while (specificAlternativeFlowStepContent.ChildNodes[1].InnerText.Trim().ToLower() != "Postcondition".ToLower())
                    {
                        switch (specificAlternativeFlowStepContent.ChildNodes.Count)
                        {
                            case 2:
                                string unparsedReferenceStep = specificAlternativeFlowStepContent.ChildNodes[1].InnerText.Trim().ToLower();
                                //// help for rule 19
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
                                specificSteps.Add(new Node(specificAlternativeFlowStepContent.ChildNodes[2].InnerText.Trim(), specificIdentifier));
                                break;
                            default:
                                break;
                        }

                        specificAlternativeFlowStepContent = specificAlternativeFlowStepContent.NextSibling;
                    }

                    string postcondition = specificAlternativeFlowStepContent.ChildNodes[2].InnerText.Trim();                                       
                    tempList.Add(new Flow(specificIdentifier, postcondition, specificSteps, new List<ReferenceStep>() { referenceStep }));
                }

                this.specificAlternativeFlows = tempList;
            }
            catch
            {
                throw new Exception("Error: Content for specified alternative flow not found");
            }
        }

        /// <summary>
        /// Gets the bounded alternative flows.
        /// </summary>
        private void GetBoundedAlternativeFlows()
        {
            XmlNodeList boundedAlternativeFlowNodes = null;
            boundedAlternativeFlowNodes = this.GetXmlNodeList("Bounded Alternative Flow");

            if (boundedAlternativeFlowNodes.Count == 0)
            {
                return;
            }

            try
            {
                List<Flow> tempList = new List<Flow>();
                for (int i = 1; i <= boundedAlternativeFlowNodes.Count; i++)
                {
                    List<Node> boundedSteps = new List<Node>();
                    FlowIdentifier boundedIdentifier = new FlowIdentifier(FlowType.BoundedAlternative, i);
                    List<ReferenceStep> referenceSteps = new List<ReferenceStep>();
                    XmlNode boundedAlternativeFlowContent = boundedAlternativeFlowNodes[i - 1].ParentNode.ParentNode.ParentNode.ParentNode;
                    XmlNode boundedAlternativeFlowStepContent = boundedAlternativeFlowContent;
                    while (boundedAlternativeFlowStepContent.ChildNodes[1].InnerText.Trim().ToLower() != "Postcondition".ToLower())
                    {
                        switch (boundedAlternativeFlowStepContent.ChildNodes.Count)
                        {
                            case 2:
                                string unparsedReferenceStep = boundedAlternativeFlowStepContent.ChildNodes[1].InnerText.Trim().ToLower();
                                //// help for rule 19
                                if (unparsedReferenceStep.Contains("RFS".ToLower()) == false)   
                                {
                                    break;
                                }

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
                            case 3:
                                boundedSteps.Add(new Node(boundedAlternativeFlowStepContent.ChildNodes[2].InnerText.Trim(), boundedIdentifier));
                                break;
                            default:
                                break;
                        }

                        boundedAlternativeFlowStepContent = boundedAlternativeFlowStepContent.NextSibling;
                    }

                    string postcondition = boundedAlternativeFlowStepContent.ChildNodes[2].InnerText.Trim();
                    tempList.Add(new Flow(boundedIdentifier, postcondition, boundedSteps, referenceSteps));                
                }

                this.boundedAlternativeFlows = tempList;
            }
            catch
            {
                throw new Exception("Error: Content for bounded alternative flow not found");
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
                string xPathFilter = "//*/text()[normalize-space(.)='" + searchWordList[i - 1] + "']/parent::*";
                flowNodeList = root.SelectNodes(xPathFilter);
                if (flowNodeList.Count > 0)
                {
                    break;
                }
            }

            return flowNodeList;
        }

        /// <summary>
        /// Trim basic flow reference step number
        /// </summary>
        /// <param name="searchWord">Defines the search word with which you want to search</param>
        /// <returns>Returns the basic flow reference step number</returns>
        private string TrimReferenceStepNumber(string searchWord)
        {
            searchWord = searchWord.Replace("RFS".ToLower(), string.Empty);
            searchWord = searchWord.Replace("Basic".ToLower(), string.Empty);
            searchWord = searchWord.Replace("Flow".ToLower(), string.Empty);
            searchWord = searchWord.Trim();
            return searchWord;
        }
    }
}
