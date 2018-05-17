using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCaseCore.XmlParser
{
    public class XmlStructureParser
    {

        private string useCaseName;
        private string briefDescription;
        private string precondition;
        private string primaryActor;
        private string secondaryActor;
        private string dependency;
        private string generalization;
        private BasicFlow basicFlow;
        private List<GlobalAlternativeFlow> globalAlternativeFlow = new List<GlobalAlternativeFlow>();
        private List<SpecificAlternativeFlow> specificAlternativeFlow = new List<SpecificAlternativeFlow>();
        private List<BoundedAlternativeFlow> boundedAlternativeFlow = new List<BoundedAlternativeFlow>();

        /// <summary>
        /// Loads external (word) xml file, which is stored on a storage medium. The absolute path to the file must be passed as parameter.
        /// Returns true if file was read sucessfully.
        /// Returns false if file was not read sucessfully.
        /// </summary>
        public bool LoadXmlFile(string path)
        {
            return false;
        }

        /// <summary>
        /// If error(return value = false) has occurred at function “LoadXmlFile()” or “TryToFixMalformedXml()”, the error text can be read out.
        /// </summary>
        public string GetError()
        {
            return "";
        }

        /// <summary>
        /// If external xml file contains easy structural errors (for example missing bracket on line 50), it can be tried to repair automatically. Attention: Source file on storage medium will be overwritten!
        /// Returns true if file was repaired successfully.
        /// Returns false if file was not repaired sucessfully.
        /// </summary>
        public bool TryToFixMalformedXml()
        {
            return false;
        }

        /// <summary>
        /// Analyzes the previously read xml file.
        /// Returns true if file was analyzed sucessfully.
        /// Returns false if file was not analyzed sucessfully.
        /// </summary>
        /*
        public bool ParseXmlFile(out UseCase useCase)
        {
        
        }
        */

        private string ParseRucmProperty(string propertyName)
        {
            return "";
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
