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
            XmlDocument useCaseXml = new XmlDocument();
            try
            {
                useCaseXml.LoadXml(this.useCaseFile.MainDocumentPart.Document.InnerXml);
                if (useCaseXml.DocumentElement.ChildNodes == null)
                {
                    this.useCaseFile.Close();
                    useCase = null;
                    this.errorMessage = "Use case document corrupted (no child nodes found!)";
                    return false;
                }

                try
                {
                    this.useCaseName = this.ParseRucmProperty(useCaseXml, "Use Case Name");
                    this.briefDescription = this.ParseRucmProperty(useCaseXml, "Brief Description");
                    this.precondition = this.ParseRucmProperty(useCaseXml, "Precondition");
                    this.primaryActor = this.ParseRucmProperty(useCaseXml, "Primary Actor");
                    this.secondaryActor = this.ParseRucmProperty(useCaseXml, "Secondary Actors");
                    this.dependency = this.ParseRucmProperty(useCaseXml, "Dependency");
                    this.generalization = this.ParseRucmProperty(useCaseXml, "Generalization");
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
        /// <param name="useCaseXml">Specifies the loaded use case xml file.</param>
        /// <param name="propertyName">Specifies the property name to parse.</param>
        /// <returns>Returns a string with the parsed RUCM property.</returns>
        private string ParseRucmProperty(XmlDocument useCaseXml, string propertyName)
        {
            string xPathFilter = "//*/text()[normalize-space(.)='" + propertyName + "']/parent::*";
            XmlNode root = useCaseXml.DocumentElement;
            XmlNodeList propertyNode = root.SelectNodes(xPathFilter);
            if (propertyNode.Count == 0)
            {
                return string.Empty;
                throw new Exception("Error: count = 0");
            }

            if (propertyNode.Count > 1)
            {
                return string.Empty;
                throw new Exception("Error: count > 1");
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
        /// <returns>Returns the basic flow.</returns>
        private BasicFlow GetBasicFlow()
        {
            this.basicFlow.AddStep("INCLUDE USE CASE Validate PIN.");
            this.basicFlow.AddStep("ATM customer selects Withdrawal through the system");
            this.basicFlow.AddStep("ATM customer enters the withdrawal amount through the system.");
            this.basicFlow.AddStep("ATM customer selects the account number through the system.");
            this.basicFlow.AddStep("The system VALIDATES THAT the account number is valid.");
            this.basicFlow.AddStep("The system VALIDATES THAT ATM customer has enough funds in the account.");
            this.basicFlow.AddStep("The system VALIDATES THAT the withdrawal amount does not exceed the daily limit of the account.");
            this.basicFlow.AddStep("The system VALIDATES THAT the ATM has enough funds.");
            this.basicFlow.AddStep("The system dispenses the cash amount.");
            this.basicFlow.AddStep("The system prints a receipt showing transaction number, transaction type, amount withdrawn, and account balance.");
            this.basicFlow.AddStep("The system ejects the ATM card.");
            this.basicFlow.AddStep("The system displays Welcome message.");
            this.basicFlow.SetPostcondition("ATM customer funds have been withdrawn.");
            return this.basicFlow;
        }

        /// <summary>
        /// Gets the global alternative flow.
        /// </summary>
        /// <returns>Returns the global alternative flow.</returns>
        private List<GlobalAlternativeFlow> GetGlobalAlternativeFlows()
        {
            GlobalAlternativeFlow globalAlternative = new GlobalAlternativeFlow();
            globalAlternative.AddStep("IF ATM customer enters Cancel THEN");
            globalAlternative.AddStep("The system cancels the transaction MEANWHILE the system ejects the ATM card.");
            globalAlternative.AddStep("ABORT.");
            globalAlternative.SetPostcondition("ATM customer funds have not been withdrawn. The system is idle. The system is displaying a Welcome message.");
            this.globalAlternativeFlows.Add(globalAlternative);
            return this.globalAlternativeFlows;
        }

        /// <summary>
        /// Gets the specific alternative flow.
        /// </summary>
        /// <returns>Returns the specific alternative flow.</returns>
        private List<SpecificAlternativeFlow> GetSpecificAlternativeFlows()
        {
            SpecificAlternativeFlow specificAlternative = new SpecificAlternativeFlow();
            FlowIdentifier flowidentifier = new FlowIdentifier(FlowType.SpecificAlternative, 1);
            ReferenceStep referenceStep = new ReferenceStep(flowidentifier, 8);
            specificAlternative.AddReferenceStep(referenceStep);
            specificAlternative.AddStep("The system displays an apology message MEANWHILE the system ejects the ATM card.");
            specificAlternative.AddStep("ABORT.");
            specificAlternative.SetPostcondition("ATM customer funds have not been withdrawn. The system is idle.The system is displaying a Welcome message.");
            this.specificAlternativeFlows.Add(specificAlternative);
            return this.specificAlternativeFlows;
        }

        /// <summary>
        /// Gets the bounded alternative flow.
        /// </summary>
        /// <returns>Returns the bounded alternative flow.</returns>
        private List<BoundedAlternativeFlow> GetBoundedAlternativeFlows()
        {
            BoundedAlternativeFlow boundedAlternative0 = new BoundedAlternativeFlow();
            FlowIdentifier flowidentifier0 = new FlowIdentifier(FlowType.BoundedAlternative, 1);
            ReferenceStep referenceStep0 = new ReferenceStep(flowidentifier0, 5);
            boundedAlternative0.AddStep("The system displays an apology message MEANWHILE the system ejects the ATM card.");
            boundedAlternative0.AddStep("The system shuts down.");
            boundedAlternative0.AddStep("ABORT.");
            boundedAlternative0.SetPostcondition("ATM customer funds have not been withdrawn. The system is shut down.");

            BoundedAlternativeFlow boundedAlternative1 = new BoundedAlternativeFlow();
            FlowIdentifier flowidentifier1 = new FlowIdentifier(FlowType.BoundedAlternative, 2);
            ReferenceStep referenceStep1 = new ReferenceStep(flowidentifier1, 6);
            boundedAlternative1.AddStep("The system displays an apology message MEANWHILE the system ejects the ATM card.");
            boundedAlternative1.AddStep("The system shuts down.");
            boundedAlternative1.AddStep("ABORT.");
            boundedAlternative1.SetPostcondition("ATM customer funds have not been withdrawn. The system is shut down.");

            BoundedAlternativeFlow boundedAlternative2 = new BoundedAlternativeFlow();
            FlowIdentifier flowidentifier2 = new FlowIdentifier(FlowType.BoundedAlternative, 3);
            ReferenceStep referenceStep2 = new ReferenceStep(flowidentifier2, 7);
            boundedAlternative2.AddStep("The system displays an apology message MEANWHILE the system ejects the ATM card.");
            boundedAlternative2.AddStep("The system shuts down.");
            boundedAlternative2.AddStep("ABORT.");
            boundedAlternative2.SetPostcondition("ATM customer funds have not been withdrawn. The system is shut down.");

            this.boundedAlternativeFlows.Add(boundedAlternative0);
            this.boundedAlternativeFlows.Add(boundedAlternative1);
            this.boundedAlternativeFlows.Add(boundedAlternative2);
            return this.boundedAlternativeFlows;
        }
    }
}
