﻿// <copyright file="Controller.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTool
{
    using UseCaseCore.XmlParser;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// Controller class
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// Creates an instance of the xml structure parser class
        /// </summary>
        private XmlStructureParser xmlParser = new XmlStructureParser();

        /// <summary>
        /// Creates an instance of the UseCase class
        /// </summary>
        private UseCase useCase = new UseCase();

        /// <summary>
        /// Tell the system path and file name of the XML UseCase file. The string para-meter contains the path to the new file.
        /// </summary>
        /// <param name="filePath">The currently selected path</param>
        public void CurrentXmlFilePath(string filePath)
        {
            this.xmlParser.LoadXmlFile(filePath);
            this.xmlParser.ParseXmlFile(out useCase);
        }

        /// <summary>
        /// Tell the system path and file name for the export of the scenario matrix and to trigger the export. 
        /// The parameter string contains the path under which the new file should be stored.
        /// </summary>
        /// <param name="filePath">destination path for the matrix</param>
        public void MatrixFilePath(string filePath)
        {
            ////  ...
        }

        /// <summary>
        /// Tell the system path and file name for the export of the defect report and to trigger this. 
        /// The parameter string contains the path under which the new file should be stored.
        /// </summary>
        /// <param name="filePath">destination path for the defect report</param>
        public void ReportFilePath(string filePath)
        {
            ////  ...
        }
    }
}
