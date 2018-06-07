﻿// <copyright file="XmlStructureParser.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>
namespace UseCaseCore.XmlParser
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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
        private BasicFlow basicFlow;

        /// <summary>
        /// A list of the global alternative flows of the use case.
        /// </summary>
        private List<GlobalAlternativeFlow> globalAlternativeFlows;

        /// <summary>
        /// A list of the specific alternative flows of the use case.
        /// </summary>
        private List<SpecificAlternativeFlow> specificAlternativeFlows;

        /// <summary>
        /// A list of the bounded alternative flows of the use case.
        /// </summary>
        private List<BoundedAlternativeFlow> boundedAlternativeFlows;

        /// <summary>
        /// Creates an instance of the UseCase class
        /// </summary>
        private UseCase useCaseOutParameter = new UseCase();

        /// <summary>
        /// The word processing document for the use case file.
        /// </summary>
        private WordprocessingDocument useCaseFile;

        /// <summary>
        /// The loaded xml document.
        /// </summary>
        private XmlDocument useCaseXml;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlStructureParser" /> class.
        /// </summary>
        public XmlStructureParser()
        {
            this.errorMessage = string.Empty;
            this.useCaseName = string.Empty;
            this.briefDescription = string.Empty;
            this.precondition = string.Empty;
            this.primaryActor = string.Empty;
            this.secondaryActor = string.Empty;
            this.dependency = string.Empty;
            this.generalization = string.Empty;
            this.basicFlow = new BasicFlow();
            this.globalAlternativeFlows = new List<GlobalAlternativeFlow>();
            this.specificAlternativeFlows = new List<SpecificAlternativeFlow>();
            this.boundedAlternativeFlows = new List<BoundedAlternativeFlow>();
            this.useCaseFile = null;
            this.useCaseXml = new XmlDocument();
        }

        /// <summary>
        /// Loads external (word) xml file, which is stored on a storage medium. The absolute path to the file must be passed as parameter.
        /// </summary>
        /// <param name="path">Specifies the path for the xml file the user wants to load.</param>
        /// <returns>Returns true if the file exists and could be loaded, otherwise false.</returns>
        public bool LoadXmlFile(string path)
        {
            try
            {
                this.useCaseFile = WordprocessingDocument.Open(path, true);
                SimplifyMarkupSettings settings = new SimplifyMarkupSettings
                {
                    RemoveComments = true,
                    RemoveContentControls = true,
                    RemoveEndAndFootNotes = true,
                    RemoveFieldCodes = false,
                    RemoveLastRenderedPageBreak = true,
                    RemovePermissions = true,
                    RemoveProof = true,
                    RemoveRsidInfo = true,
                    RemoveSmartTags = true,
                    RemoveSoftHyphens = true,
                    ReplaceTabsWithSpaces = true,
                };
                MarkupSimplifier.SimplifyMarkup(this.useCaseFile, settings);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                this.errorMessage = ex.Message.ToString();
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
            try
            {
                this.useCaseXml.LoadXml(this.useCaseFile.MainDocumentPart.Document.InnerXml);
                if (this.useCaseXml.DocumentElement.ChildNodes == null)
                {
                    this.useCaseFile.Close();
                    useCase = null;
                    this.errorMessage = "Use case document corrupted (no child nodes found!)";
                    return false;
                }

                try
                {
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
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message.ToString());
                    this.useCaseFile.Close();
                    useCase = null;
                    this.errorMessage = ex.Message.ToString();
                    return false;
                }

                bool validationResult = this.ValidateRucmRules();
                this.useCaseFile.Close();
                if (validationResult == true)
                {
                    this.SetUseCaseOutParameter();
                }
                else
                {
                    this.errorMessage = "RUCM rule validation failed!";
                }

                useCase = this.useCaseOutParameter;
                return validationResult;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                this.useCaseFile.Close();
                useCase = null;
                this.errorMessage = ex.Message.ToString();
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
            RucmRuleValidator rucmRuleValidator = new RucmRuleValidator(RuleValidation.RucmRules.RuleRepository.Rules);
            rucmRuleValidator.Validate(this.basicFlow);
            foreach (GlobalAlternativeFlow globalAlternativeFlow in this.globalAlternativeFlows)
            {
                if (currentValidationResult == true)
                {
                    currentValidationResult = rucmRuleValidator.Validate(globalAlternativeFlow, this.basicFlow);
                }
                else
                {
                    break;
                }
            }

            foreach (SpecificAlternativeFlow specificAlternativeFlow in this.specificAlternativeFlows)
            {
                if (currentValidationResult == true)
                {
                    currentValidationResult = rucmRuleValidator.Validate(specificAlternativeFlow, this.basicFlow);
                }
                else
                {
                    break;
                }
            }

            foreach (BoundedAlternativeFlow boundedAlternativeFlow in this.boundedAlternativeFlows)
            {
                if (currentValidationResult == true)
                {
                    currentValidationResult = rucmRuleValidator.Validate(boundedAlternativeFlow, this.basicFlow);
                }
                else
                {
                    break;
                }
            }

            return currentValidationResult;
        }

        /// <summary>
        /// This function sets the parsed values for the use case.
        /// </summary>
        /// <returns>Returns the use case with the values you set.</returns>
        private UseCase SetUseCaseOutParameter()
        {
            this.useCaseOutParameter.UseCaseName = this.useCaseName;
            this.useCaseOutParameter.BriefDescription = this.briefDescription;
            this.useCaseOutParameter.Precondition = this.precondition;
            this.useCaseOutParameter.PrimaryActor = this.primaryActor;
            this.useCaseOutParameter.SecondaryActors = this.secondaryActor;
            this.useCaseOutParameter.Dependency = this.dependency;
            this.useCaseOutParameter.Generalization = this.generalization;
            this.useCaseOutParameter.SetBasicFlow(this.basicFlow.GetSteps(), this.basicFlow.GetPostcondition());
            int i = 0;
            foreach (GlobalAlternativeFlow globalAlternativeFlow in this.globalAlternativeFlows)
            {
                i++;
                this.useCaseOutParameter.AddGlobalAlternativeFlow(i, globalAlternativeFlow.GetSteps(), globalAlternativeFlow.GetPostcondition());
            }

            i = 0;
            foreach (SpecificAlternativeFlow specificAlternativeFlow in this.specificAlternativeFlows)
            {
                i++;
                this.useCaseOutParameter.AddSpecificAlternativeFlow(i, specificAlternativeFlow.GetSteps(), specificAlternativeFlow.GetPostcondition(), specificAlternativeFlow.GetReferenceStep());
            }

            i = 0;
            foreach (BoundedAlternativeFlow boundedAlternativeFlow in this.boundedAlternativeFlows)
            {
                i++;
                this.useCaseOutParameter.AddBoundedAlternativeFlow(i, boundedAlternativeFlow.GetSteps(), boundedAlternativeFlow.GetPostcondition(), boundedAlternativeFlow.GetReferenceSteps());
            }

            this.useCaseOutParameter.BuildGraph();
            return this.useCaseOutParameter;
        }

        /// <summary>
        /// Parses the RUCM properties.
        /// </summary>
        /// <param name="propertyName">Specifies the property name to parse.</param>
        /// <returns>Returns a string with the parsed RUCM property.</returns>
        private string ParseRucmProperty(string propertyName)
        {
            string xPathFilter = "//*/text()[normalize-space(.)='" + propertyName + "']/parent::*";
            XmlNode root = this.useCaseXml.DocumentElement;
            XmlNodeList propertyNode = root.SelectNodes(xPathFilter);
            if (propertyNode.Count == 0)
            {
                return string.Empty;
                throw new Exception("Error: Use-Case property not found");
            }

            if (propertyNode.Count > 1)
            {
                return string.Empty;
                throw new Exception("Error: More than one Use-Case property found");
            }

            try
            {
                string propertyContent = propertyNode[0].ParentNode.ParentNode.ParentNode.ParentNode.ChildNodes[1].InnerText;
                return propertyContent;
            }
            catch
            {
                return string.Empty;
                throw new Exception("Error: content not found");
            }
        }

        /// <summary>
        /// Gets the basic flow.
        /// </summary>
        private void GetBasicFlow()
        {
            XmlNodeList basicFlowNode = null;
            basicFlowNode = GetXmlNodeList(FlowType.Basic);

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
                while (basicFlowStepContent.ChildNodes[1].InnerText != "Postcondition")
                {
                    this.basicFlow.AddStep(basicFlowStepContent.ChildNodes[2].InnerText);
                    basicFlowStepContent = basicFlowStepContent.NextSibling;
                }

                this.basicFlow.SetPostcondition(basicFlowStepContent.ChildNodes[2].InnerText);
            }
            catch
            {
                throw new Exception("Error: content not found");
            }
        }

        /// <summary>
        /// Gets the global alternative flows.
        /// </summary>
        private void GetGlobalAlternativeFlows()
        {
            XmlNodeList globalAlternativeFlowNodes = null;
            globalAlternativeFlowNodes = GetXmlNodeList(FlowType.GlobalAlternative);

            if (globalAlternativeFlowNodes.Count == 0)
            {
                throw new Exception("Error: No Global Alternative Flow found");
            }

            try
            {
                for (int i = 1; i <= globalAlternativeFlowNodes.Count; i++)
                {
                    GlobalAlternativeFlow globalAlternativFlow = new GlobalAlternativeFlow();
                    XmlNode globalAlternativeFlowContent = globalAlternativeFlowNodes[i - 1].ParentNode.ParentNode.ParentNode.ParentNode;
                    XmlNode globlaAlternativeFlowStepContent = globalAlternativeFlowContent;
                    while (globlaAlternativeFlowStepContent.ChildNodes[1].InnerText != "Postcondition")
                    {
                        switch (globlaAlternativeFlowStepContent.ChildNodes.Count)
                        {
                            case 2:
                                globalAlternativFlow.AddStep(globlaAlternativeFlowStepContent.ChildNodes[1].InnerText);
                                break;
                            case 3:
                                globalAlternativFlow.AddStep(globlaAlternativeFlowStepContent.ChildNodes[2].InnerText);
                                break;
                            default:
                                break;
                        }

                        globlaAlternativeFlowStepContent = globlaAlternativeFlowStepContent.NextSibling;
                    }

                    globalAlternativFlow.SetPostcondition(globlaAlternativeFlowStepContent.ChildNodes[2].InnerText);
                    globalAlternativFlow.SetId(i);
                    this.globalAlternativeFlows.Add(globalAlternativFlow);
                }
            }
            catch
            {
                throw new Exception("Error: content not found");
            }
        }

        /// <summary>
        /// Gets the specific alternative flows.
        /// </summary>
        private void GetSpecificAlternativeFlows()
        {
            XmlNodeList specificAlternativeFlowNodes = null;
            specificAlternativeFlowNodes = GetXmlNodeList(FlowType.SpecificAlternative);

            if (specificAlternativeFlowNodes.Count == 0)
            {
                throw new Exception("Error: No Specific Alternative Flow found");
            }

            try
            {
                for (int i = 1; i <= specificAlternativeFlowNodes.Count; i++)
                {
                    SpecificAlternativeFlow specificAlternativFlow = new SpecificAlternativeFlow();
                    XmlNode specificAlternativeFlowContent = specificAlternativeFlowNodes[i - 1].ParentNode.ParentNode.ParentNode.ParentNode;
                    XmlNode specificAlternativeFlowStepContent = specificAlternativeFlowContent;
                    while (specificAlternativeFlowStepContent.ChildNodes[1].InnerText != "Postcondition")
                    {
                        switch (specificAlternativeFlowStepContent.ChildNodes.Count)
                        {
                            case 2:
                                string unparsedReferenceStep = specificAlternativeFlowStepContent.ChildNodes[1].InnerText;
                                int referenceStepNumber = int.Parse(unparsedReferenceStep.Replace("RFS Basic Flow ", string.Empty));
                                FlowIdentifier flowIdentifier = new FlowIdentifier(FlowType.SpecificAlternative, i);
                                ReferenceStep referenceStep = new ReferenceStep(flowIdentifier, referenceStepNumber);
                                specificAlternativFlow.AddReferenceStep(referenceStep);
                                break;
                            case 3:
                                specificAlternativFlow.AddStep(specificAlternativeFlowStepContent.ChildNodes[2].InnerText);
                                break;
                            default:
                                break;
                        }

                        specificAlternativeFlowStepContent = specificAlternativeFlowStepContent.NextSibling;
                    }

                    specificAlternativFlow.SetPostcondition(specificAlternativeFlowStepContent.ChildNodes[2].InnerText);
                    this.specificAlternativeFlows.Add(specificAlternativFlow);
                }
            }
            catch
            {
                throw new Exception("Error: content not found");
            }
        }

        /// <summary>
        /// Gets the bounded alternative flows.
        /// </summary>
        private void GetBoundedAlternativeFlows()
        {
            XmlNodeList boundedAlternativeFlowNodes = null;
            boundedAlternativeFlowNodes = GetXmlNodeList(FlowType.BoundedAlternative);

            if (boundedAlternativeFlowNodes.Count == 0)
            {
                throw new Exception("Error: No Bounded Alternative Flow found");
            }

            try
            {
                for (int i = 1; i <= boundedAlternativeFlowNodes.Count; i++)
                {
                    BoundedAlternativeFlow boundedAlternativFlow = new BoundedAlternativeFlow();
                    XmlNode boundedAlternativeFlowContent = boundedAlternativeFlowNodes[i - 1].ParentNode.ParentNode.ParentNode.ParentNode;
                    XmlNode boundedAlternativeFlowStepContent = boundedAlternativeFlowContent;
                    while (boundedAlternativeFlowStepContent.ChildNodes[1].InnerText != "Postcondition")
                    {
                        switch (boundedAlternativeFlowStepContent.ChildNodes.Count)
                        {
                            case 2:
                                string unparsedReferenceStep = boundedAlternativeFlowStepContent.ChildNodes[1].InnerText;
                                string referenceStepNumbers = unparsedReferenceStep.Replace("RFS Basic Flow ", string.Empty);
                                if (referenceStepNumbers.Contains("-") == true)
                                {
                                    int stepStartNumber = int.Parse(referenceStepNumbers.Split('-')[0]);
                                    int stepEndNumber = int.Parse(referenceStepNumbers.Split('-')[1]);
                                    for (int n = stepStartNumber; n <= stepEndNumber; n++)
                                    {
                                        FlowIdentifier flowIdentifier = new FlowIdentifier(FlowType.BoundedAlternative, i);
                                        ReferenceStep referenceStep = new ReferenceStep(flowIdentifier, n);
                                        boundedAlternativFlow.AddReferenceStep(referenceStep);
                                    }
                                }
                                else
                                {
                                    int referenceStepNumber = int.Parse(referenceStepNumbers);
                                    FlowIdentifier flowIdentifier = new FlowIdentifier(FlowType.BoundedAlternative, i);
                                    ReferenceStep referenceStep = new ReferenceStep(flowIdentifier, referenceStepNumber);
                                    boundedAlternativFlow.AddReferenceStep(referenceStep);
                                }

                                break;
                            case 3:
                                boundedAlternativFlow.AddStep(boundedAlternativeFlowStepContent.ChildNodes[2].InnerText);
                                break;
                            default:
                                break;
                        }

                        boundedAlternativeFlowStepContent = boundedAlternativeFlowStepContent.NextSibling;
                    }

                    boundedAlternativFlow.SetPostcondition(boundedAlternativeFlowStepContent.ChildNodes[2].InnerText);
                    this.boundedAlternativeFlows.Add(boundedAlternativFlow);
                }
            }
            catch
            {
                throw new Exception("Error: content not found");
            }
        }

        private XmlNodeList GetXmlNodeList(FlowType flowType)
        {
            XmlNode root = this.useCaseXml.DocumentElement;
            XmlNodeList flowNodeList = null;

            List<string> searchWords = new List<string>();
            switch (flowType)
            {
                case FlowType.Basic:
                    searchWords.Add("Basic Flow");
                    searchWords.Add("Basic");
                    break;
                case FlowType.BoundedAlternative:
                    searchWords.Add("Bounded Alternative Flows");
                    searchWords.Add("Bounded Alternative");
                    searchWords.Add("Bounded");
                    break;
                case FlowType.GlobalAlternative:
                    searchWords.Add("Global Alternative Flows");
                    searchWords.Add("Global Alternative");
                    searchWords.Add("Global");
                    break;
                case FlowType.SpecificAlternative:
                    searchWords.Add("Specific Alternative Flows");
                    searchWords.Add("Specific Alternative");
                    searchWords.Add("Specific");
                    break;
            }

            for (int i = 1; i <= searchWords.Count; i++)
            {
                string xPathFilter = "//*/text()[normalize-space(.)='" + searchWords[i - 1] + "']/parent::*";
                flowNodeList = root.SelectNodes(xPathFilter);
                if (flowNodeList.Count > 0)
                {
                    break;
                }
            }

            return flowNodeList;
        }

    }
}
