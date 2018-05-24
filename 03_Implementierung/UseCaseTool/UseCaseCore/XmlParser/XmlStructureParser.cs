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
        private List<GlobalAlternativeFlow> globalAlternativeFlow;

        /// <summary>
        /// A list of the specific alternative flows of the use case.
        /// </summary>
        private List<SpecificAlternativeFlow> specificAlternativeFlow;

        /// <summary>
        /// A list of the bounded alternative flows of the use case.
        /// </summary>
        private List<BoundedAlternativeFlow> boundedAlternativeFlow;

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
            this.globalAlternativeFlow = new List<GlobalAlternativeFlow>();
            this.specificAlternativeFlow = new List<SpecificAlternativeFlow>();
            this.boundedAlternativeFlow = new List<BoundedAlternativeFlow>();
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
        /// Returns true if file was repaired successfully.
        /// Returns false if file was not repaired successfully.
        /// </summary>
        /// <returns>Returns true if the malformed xml could be fixed, otherwise false.</returns>
        public bool TryToFixMalformedXml()
        {
            return false;
        }

        /// <summary>
        /// Analyzes the previously read xml file.
        /// Returns true if file was analyzed successfully.
        /// Returns false if file was not analyzed successfully.
        /// </summary>
        /// <param name="useCase">Out parameter for the whole internal use case representation.</param>
        /// <returns>Returns true if the file was analyzed successfully, otherwise false.</returns>
        //// public bool ParseXmlFile(out UseCase useCase)
        public bool ParseXmlFile(out string useCase)
        {
            XmlDocument useCaseXml = new XmlDocument();
            try
            {
                useCaseXml.PreserveWhitespace = false;
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
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message.ToString());
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

        private BasicFlow GetBasicFlow()
        {

        }

        private List<GlobalAlternativeFlow> GetGlobalAlternativeFlow()
        {

        }

        private List<SpecificAlternativeFlow> GetSpecificAlternativeFlow()
        {

        }

        private List<BoundedAlternativeFlow> GetBoundedAlternativeFlow()
        {

        }
        */
    }
}
