// <copyright file="XmlStructureParser.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>
namespace UseCaseCore.XmlParser
{
    using System.Collections.Generic;

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
        private List<GlobalAlternativeFlow> globalAlternativeFlow = new List<GlobalAlternativeFlow>();

        /// <summary>
        /// A list of the specific alternative flows of the use case.
        /// </summary>
        private List<SpecificAlternativeFlow> specificAlternativeFlow = new List<SpecificAlternativeFlow>();

        /// <summary>
        /// A list of the bounded alternative flows of the use case.
        /// </summary>
        private List<BoundedAlternativeFlow> boundedAlternativeFlow = new List<BoundedAlternativeFlow>();

        /// <summary>
        /// Loads external (word) xml file, which is stored on a storage medium. The absolute path to the file must be passed as parameter.
        /// Returns true if file was read successfully.
        /// Returns false if file was not read successfully.
        /// </summary>
        /// <param name="path">Specifies the path for the xml file the user wants to load.</param>
        /// <returns>Returns true if the file exists and could be loaded, otherwise false.</returns>
        public bool LoadXmlFile(string path)
        {
            return false;
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
        /// <returns>Returns true if the file was analyzed successfully, otherwise false.</returns>
        /*
        public bool ParseXmlFile(out UseCase useCase)
        {
        
        }
        */
        private string ParseRucmProperty(string propertyName)
        {
            return string.Empty;
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
