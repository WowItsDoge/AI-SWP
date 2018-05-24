// <copyright file="XmlStructureParser.cs" company="Team B">
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

    /// <summary>
    /// The xml structure parser instance.
    /// </summary>
    public class XmlStructureParser
    {
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
        /// The word processing document for the use case file.
        /// </summary>
        private WordprocessingDocument useCaseFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlStructureParser" /> class.
        /// </summary>
        public XmlStructureParser()
        {
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
        /// Returns true if file was read successfully.
        /// Returns false if file was not read successfully.
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
                return false;
            }
        }

        /// <summary>
        /// If error(return value = false) has occurred at function “LoadXmlFile()” or “TryToFixMalformedXml()”, the error text can be read out.
        /// </summary>
        /// <returns>Returns a string with the error message if there was an error.</returns>
        public string GetError()
        {
            return string.Empty;
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
        //// public bool ParseXmlFile(out UseCase useCase)
        public bool ParseXmlFile(out string useCase)
        {
            XmlDocument useCaseXml = new XmlDocument();
            try
            {
                useCaseXml.LoadXml(this.useCaseFile.MainDocumentPart.Document.InnerXml);
                if (useCaseXml.DocumentElement.ChildNodes == null)
                {
                    this.useCaseFile.Close();
                    useCase = string.Empty;
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
                    useCase = string.Empty;
                    return false;
                }

                this.useCaseFile.Close();
                useCase = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                this.useCaseFile.Close();
                useCase = string.Empty;
                return false;
            }
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

        /*
         *  TODO: do we really need this?
        private List<ReferenceStep> GetReferenceSteps(string referenceStepString)
        {

        }

        private List<string> GetSteps(string flowName)
        {
        
        }

        private string GetPostcondition(string flowName)
        {
            return "";
        }
        */

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
            specificAlternative.AddStep("RFS Basic Flow 8");
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
            BoundedAlternativeFlow boundedAlternative1 = new BoundedAlternativeFlow();
            BoundedAlternativeFlow boundedAlternative2 = new BoundedAlternativeFlow();
            boundedAlternative0.AddStep("RFS Basic Flow 5");
            boundedAlternative0.AddStep("The system displays an apology message MEANWHILE the system ejects the ATM card.");
            boundedAlternative0.AddStep("The system shuts down.");
            boundedAlternative0.AddStep("ABORT.");
            boundedAlternative1.AddStep("RFS Basic Flow 6");
            boundedAlternative1.AddStep("The system displays an apology message MEANWHILE the system ejects the ATM card.");
            boundedAlternative1.AddStep("The system shuts down.");
            boundedAlternative1.AddStep("ABORT.");
            boundedAlternative2.AddStep("RFS Basic Flow 7");
            boundedAlternative2.AddStep("The system displays an apology message MEANWHILE the system ejects the ATM card.");
            boundedAlternative2.AddStep("The system shuts down.");
            boundedAlternative2.AddStep("ABORT.");
            boundedAlternative0.SetPostcondition("ATM customer funds have not been withdrawn. The system is shut down.");
            boundedAlternative1.SetPostcondition("ATM customer funds have not been withdrawn. The system is shut down.");
            boundedAlternative2.SetPostcondition("ATM customer funds have not been withdrawn. The system is shut down.");
            this.boundedAlternativeFlows.Add(boundedAlternative0);
            this.boundedAlternativeFlows.Add(boundedAlternative1);
            this.boundedAlternativeFlows.Add(boundedAlternative2);
            return this.boundedAlternativeFlows;
        }
    }
}
