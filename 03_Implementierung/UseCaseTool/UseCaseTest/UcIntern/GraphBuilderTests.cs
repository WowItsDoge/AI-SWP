// <copyright file="GraphBuilderTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using NUnit.Framework;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A class collecting all tests for the <see cref="GraphBuilder"/> class.
    /// </summary>
    [TestFixture]
    public class GraphBuilderTests
    {
        private FlowIdentifier flowIdentifierBasic,
            flowIdentifierSpecific1,
            flowIdentifierSpecific2,
            flowIdentifierGlobal1,
            flowIdentifierGlobal2,
            flowIdentifierBounded1,
            flowIdentifierBounded2;

        /// <summary>
        /// Prints a matrix into the console.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix"></param>
        public void PrintMatrix<T>(Matrix<T> matrix)
        {
            for (int row = 0; row < matrix.RowCount; row++)
            {
                for (int column = 0; column < matrix.ColumnCount; column++)
                {
                    Debug.Write(matrix[row, column]);
                    Debug.Write(", ");
                }
                Debug.Write('\n');
            }
        }

        /// <summary>
        /// Prints the edges of the matrix into the console, that are equal to the match object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix"></param>
        /// <param name="matchObject"></param>
        public void PrintEdges<T>(Matrix<T> matrix, T matchObject)
        {
            for (int row = 0; row < matrix.RowCount; row++)
            {
                for (int column = 0; column < matrix.ColumnCount; column++)
                {
                    if (matrix[row, column].Equals(matchObject))
                    {
                        Debug.WriteLine($"{row} -> {column}");
                    }
                }
            }
        }

        /// <summary>
        /// Setup method for private variables.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            flowIdentifierBasic = new FlowIdentifier(FlowType.Basic, 0);
            flowIdentifierSpecific1 = new FlowIdentifier(FlowType.SpecificAlternative, 1);
            flowIdentifierSpecific2 = new FlowIdentifier(FlowType.SpecificAlternative, 2);
            flowIdentifierGlobal1 = new FlowIdentifier(FlowType.GlobalAlternative, 1);
            flowIdentifierGlobal2 = new FlowIdentifier(FlowType.GlobalAlternative, 2);
            flowIdentifierBounded1 = new FlowIdentifier(FlowType.BoundedAlternative, 1);
            flowIdentifierBounded2 = new FlowIdentifier(FlowType.BoundedAlternative, 2);
        }

        // --------------------------------------------------------------------------------------------- BuildGraph
        // For the graphes see in 04_Test/UcIntern/GraphsForUnitTests.pdf

        [Test]
        public void BuildBasic()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("The use case starts.", flowIdentifierBasic));
            basicSteps.Add(new Node("A random step.", flowIdentifierBasic));
            basicSteps.Add(new Node("The use case starts.", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(3, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 2] = true;

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
        }

        [Test]
        public void BuildBasicWithSpecificAborted()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("The use case starts.", flowIdentifierBasic));
            basicSteps.Add(new Node("IF I can fly THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Fly, my bird!", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Node> specificSteps = new List<Node>();
            specificSteps.Add(new Node("ELSE", flowIdentifierSpecific1));
            specificSteps.Add(new Node("ABORT", flowIdentifierSpecific1));
            specificSteps.Add(new Node("ENDIF", flowIdentifierSpecific1));
            List<Flow> specificFlows = new List<Flow>();
            List<ReferenceStep> specificReferenceSteps = new List<ReferenceStep>();
            specificReferenceSteps.Add(new ReferenceStep(flowIdentifierBasic, 2));
            specificFlows.Add(new Flow(flowIdentifierSpecific1, null, specificSteps, specificReferenceSteps));
            allSteps.AddRange(specificSteps);

            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(7, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[1, 4] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[4, 5] = true;

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
        }

        [Test]
        public void BuildBasicWithSpecific()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("The use case starts.", flowIdentifierBasic));
            basicSteps.Add(new Node("IF I can fly THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Fly, my bird!", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            basicSteps.Add(new Node("Gandalf ends.", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Node> specificSteps = new List<Node>();
            specificSteps.Add(new Node("ELSE", flowIdentifierSpecific1));
            specificSteps.Add(new Node("Fly, you fool!", flowIdentifierSpecific1));
            specificSteps.Add(new Node("RESUME 4", flowIdentifierSpecific1));
            specificSteps.Add(new Node("ENDIF", flowIdentifierSpecific1));
            List<Flow> specificFlows = new List<Flow>();
            List<ReferenceStep> specificReferenceSteps = new List<ReferenceStep>();
            specificReferenceSteps.Add(new ReferenceStep(flowIdentifierBasic, 2));
            specificFlows.Add(new Flow(flowIdentifierSpecific1, null, specificSteps, specificReferenceSteps));
            allSteps.AddRange(specificSteps);

            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(9, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[1, 5] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[3, 4] = true;
            expectedEdgeMatrix[5, 6] = true;
            expectedEdgeMatrix[6, 7] = true;
            expectedEdgeMatrix[7, 3] = true;

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
        }

        [Test]
        public void BuildBasicWithGlobalResuming()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("The use case starts.", flowIdentifierBasic));
            basicSteps.Add(new Node("Do something.", flowIdentifierBasic));
            basicSteps.Add(new Node("The use case ends.", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();

            List<Node> globalSteps = new List<Node>();
            globalSteps.Add(new Node("Global 1", flowIdentifierGlobal1));
            globalSteps.Add(new Node("RESUME 2", flowIdentifierGlobal1));
            List<Flow> globalFlows = new List<Flow>();
            globalFlows.Add(new Flow(flowIdentifierGlobal1, null, globalSteps, new List<ReferenceStep>()));
            allSteps.AddRange(globalSteps);

            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(5, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[0, 3] = true;
            expectedEdgeMatrix[1, 3] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[3, 4] = true;
            expectedEdgeMatrix[4, 1] = true;

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);
            
            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
        }

        [Test]
        public void BuildBasicWithBounded()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF A THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Write A", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            basicSteps.Add(new Node("IF B THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Write B", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();

            List<Node> boundedSteps = new List<Node>();
            boundedSteps.Add(new Node("ELSEIF C THEN", flowIdentifierBounded1));
            boundedSteps.Add(new Node("RESUME 1", flowIdentifierBounded1));
            boundedSteps.Add(new Node("ELSEIF D THEN", flowIdentifierBounded1));
            boundedSteps.Add(new Node("RESUME 6", flowIdentifierBounded1));
            boundedSteps.Add(new Node("ELSE", flowIdentifierBounded1));
            boundedSteps.Add(new Node("ABORT", flowIdentifierBounded1));
            boundedSteps.Add(new Node("ENDIF", flowIdentifierBounded1));
            List<Flow> boundedFlows = new List<Flow>();
            List<ReferenceStep> boundedReferenceSteps = new List<ReferenceStep>();
            boundedReferenceSteps.Add(new ReferenceStep(flowIdentifierBasic, 1));
            boundedReferenceSteps.Add(new ReferenceStep(flowIdentifierBasic, 4));
            boundedFlows.Add(new Flow(flowIdentifierBounded1, null, boundedSteps, boundedReferenceSteps));
            allSteps.AddRange(boundedSteps);

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(13, false);
            expectedEdgeMatrix[0,1] = true;
            expectedEdgeMatrix[1,2] = true;
            expectedEdgeMatrix[2,3] = true;
            expectedEdgeMatrix[3,4] = true;
            expectedEdgeMatrix[4,5] = true;
            expectedEdgeMatrix[0,6] = true;
            expectedEdgeMatrix[3,6] = true;
            expectedEdgeMatrix[6,7] = true;
            expectedEdgeMatrix[7,0] = true;
            expectedEdgeMatrix[0,8] = true;
            expectedEdgeMatrix[3,8] = true;
            expectedEdgeMatrix[8,9] = true;
            expectedEdgeMatrix[9,5] = true;
            expectedEdgeMatrix[0,10] = true;
            expectedEdgeMatrix[3,10] = true;
            expectedEdgeMatrix[10,11] = true;

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
        }

        [Test]
        public void BuildComplexGraph()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF basic THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("do something", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            basicSteps.Add(new Node("IF fortran THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("do some other things", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            basicSteps.Add(new Node("He VALIDATES THAT it is correct!", flowIdentifierBasic));
            basicSteps.Add(new Node("I VALIDATES THAT I can't fly", flowIdentifierBasic));
            basicSteps.Add(new Node("You VALIDTAES THAT clouds are falling", flowIdentifierBasic));
            basicSteps.Add(new Node("Go home", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();

            List<Node> specificSteps1 = new List<Node>();
            specificSteps1.Add(new Node("RESUME 1", flowIdentifierSpecific1));
            List<ReferenceStep> specificReferenceSteps1 = new List<ReferenceStep>();
            specificReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 4));
            specificFlows.Add(new Flow(flowIdentifierSpecific1, null, specificSteps1, specificReferenceSteps1));
            allSteps.AddRange(specificSteps1);

            List<Node> specificSteps2 = new List<Node>();
            specificSteps2.Add(new Node("RESUME 6", flowIdentifierSpecific1));
            List<ReferenceStep> specificReferenceSteps2 = new List<ReferenceStep>();
            specificReferenceSteps2.Add(new ReferenceStep(flowIdentifierBasic, 7));
            specificFlows.Add(new Flow(flowIdentifierSpecific1, null, specificSteps2, specificReferenceSteps2));
            allSteps.AddRange(specificSteps2);

            List<Flow> globalFlows = new List<Flow>();

            List<Node> globalSteps1 = new List<Node>();
            globalSteps1.Add(new Node("ABORT", flowIdentifierGlobal1));
            globalFlows.Add(new Flow(flowIdentifierGlobal1, null, globalSteps1, new List<ReferenceStep>()));
            allSteps.AddRange(globalSteps1);

            List<Node> globalSteps2 = new List<Node>();
            globalSteps2.Add(new Node("ABORT", flowIdentifierGlobal1));
            globalFlows.Add(new Flow(flowIdentifierGlobal1, null, globalSteps2, new List<ReferenceStep>()));
            allSteps.AddRange(globalSteps2);

            List<Flow> boundedFlows = new List<Flow>();

            List<Node> boundedSteps1 = new List<Node>();
            boundedSteps1.Add(new Node("ABORT", flowIdentifierBounded1));
            List<ReferenceStep> boundedReferenceSteps1 = new List<ReferenceStep>();
            boundedReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 8));
            boundedReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 9));
            boundedFlows.Add(new Flow(flowIdentifierBounded1, null, boundedSteps1, boundedReferenceSteps1));
            allSteps.AddRange(boundedSteps1);

            List<Node> boundedSteps2 = new List<Node>();
            boundedSteps2.Add(new Node("DO", flowIdentifierBounded1));
            boundedSteps2.Add(new Node("Count peas", flowIdentifierBounded1));
            boundedSteps2.Add(new Node("WHILE there are peas on the floor", flowIdentifierBounded1));
            boundedSteps2.Add(new Node("ABORT", flowIdentifierBounded1));
            List<ReferenceStep> boundedReferenceSteps2 = new List<ReferenceStep>();
            boundedReferenceSteps2.Add(new ReferenceStep(flowIdentifierBasic, 7));
            boundedReferenceSteps2.Add(new ReferenceStep(flowIdentifierBasic, 8));
            boundedFlows.Add(new Flow(flowIdentifierBounded1, null, boundedSteps2, boundedReferenceSteps2));
            allSteps.AddRange(boundedSteps2);

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(19, false);
            // basic
            expectedEdgeMatrix[0,1] = true;
            expectedEdgeMatrix[0,2] = true;
            expectedEdgeMatrix[1,2] = true;
            expectedEdgeMatrix[2,3] = true;
            expectedEdgeMatrix[3,4] = true;
            expectedEdgeMatrix[4,5] = true;
            expectedEdgeMatrix[5,6] = true;
            expectedEdgeMatrix[6,7] = true;
            expectedEdgeMatrix[7,8] = true;
            expectedEdgeMatrix[8,9] = true;

            // specific/bounded
            expectedEdgeMatrix[3,10] = true;
            expectedEdgeMatrix[10,0] = true;
            expectedEdgeMatrix[6,11] = true;
            expectedEdgeMatrix[11,5] = true;
            expectedEdgeMatrix[7,14] = true;
            expectedEdgeMatrix[8,14] = true;
            expectedEdgeMatrix[6,15] = true;
            expectedEdgeMatrix[7, 15] = true;
            expectedEdgeMatrix[15, 16] = true;
            expectedEdgeMatrix[16, 17] = true;
            expectedEdgeMatrix[17, 15] = true;
            expectedEdgeMatrix[17, 18] = true;

            // globals
            expectedEdgeMatrix[0,12] = true;
            expectedEdgeMatrix[1,12] = true;
            expectedEdgeMatrix[2,12] = true;
            expectedEdgeMatrix[3,12] = true;
            expectedEdgeMatrix[4, 12] = true;
            expectedEdgeMatrix[5, 12] = true;
            expectedEdgeMatrix[6, 12] = true;
            expectedEdgeMatrix[7, 12] = true;
            expectedEdgeMatrix[8, 12] = true;
            expectedEdgeMatrix[9, 12] = true;

            expectedEdgeMatrix[0,13] = true;
            expectedEdgeMatrix[1, 13] = true;
            expectedEdgeMatrix[2, 13] = true;
            expectedEdgeMatrix[3, 13] = true;
            expectedEdgeMatrix[4, 13] = true;
            expectedEdgeMatrix[5, 13] = true;
            expectedEdgeMatrix[6, 13] = true;
            expectedEdgeMatrix[7, 13] = true;
            expectedEdgeMatrix[8, 13] = true;
            expectedEdgeMatrix[9, 13] = true;


            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);
            PrintEdges(edgeMatrix, true);
            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
        }

        // --------------------------------------------------------------------------------------------- SetEdgesInNodeBlock

        /// <summary>
        /// Tests the edge creation of a linear flow without special steps.
        /// </summary>
        [Test]
        public void WireNormalFlow()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("The use case starts.", flowIdentifierBasic));
            nodes.Add(new Node("A random step.", flowIdentifierBasic));
            nodes.Add(new Node("The use case starts.", flowIdentifierBasic));
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(
                new bool[,]
                {
                    { false, true, false },
                    { false, false, true },
                    { false, false, false }
                });
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            List<int> expectedExitSteps = new List<int>();
            expectedExitSteps.Add(2);

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<int> exitSteps;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        /// <summary>
        /// Tests the edge creation of a linear flow with an abort step.
        /// </summary>
        [Test]
        public void WireAbortFlow()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("The use case starts.", flowIdentifierBasic));
            nodes.Add(new Node("ABORT", flowIdentifierBasic));
            nodes.Add(new Node("The use case starts.", flowIdentifierBasic));
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(
                new bool[,]
                {
                    { false, true, false },
                    { false, false, false },
                    { false, false, false }
                });
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            List<int> expectedExitSteps = new List<int>();

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<int> exitSteps;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        /// <summary>
        /// Tests the edge creation of a linear flow with a resume step.
        /// </summary>
        [Test]
        public void WireResumeFlow()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("The use case starts.", flowIdentifierBasic));
            nodes.Add(new Node("RESUME 12 ", flowIdentifierBasic));
            nodes.Add(new Node("The use case starts.", flowIdentifierBasic));
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(
                new bool[,]
                {
                    { false, true, false },
                    { false, false, false },
                    { false, false, false }
                });
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            expectedExternalEdges.Add(new ExternalEdge(1, new ReferenceStep(new FlowIdentifier(FlowType.Basic, 0), 11)));
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            List<int> expectedExitSteps = new List<int>();

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<int> exitSteps;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        /// <summary>
        /// Tests the edge creation of an if flow like if-endif with a block in the if.
        /// </summary>
        [Test]
        public void WireSimpleIfFlow()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("The use case starts.", flowIdentifierBasic));       // 0
            nodes.Add(new Node("IF it is hot outside THEN", flowIdentifierBasic));  // 1
            nodes.Add(new Node("Activate the fan.", flowIdentifierBasic));          // 2
            nodes.Add(new Node("ENDIF", flowIdentifierBasic));                      // 3
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(
                new bool[,]
                {
                    { false, true, false, false },
                    { false, false, true, true },
                    { false, false, false, true },
                    { false, false, false, false }
                });
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            expectedPossibleInvalidIfEdges.Add(new InternalEdge(1, 3));
            List<int> expectedExitSteps = new List<int>();
            expectedExitSteps.Add(3);

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<int> exitSteps;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        /// <summary>
        /// Tests the edge creation of an if flow like if-endif with no block inside the.
        /// </summary>
        [Test]
        public void WireSimpleEmptyIfFlow()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("The use case starts.", flowIdentifierBasic));       // 0
            nodes.Add(new Node("IF it is hot outside THEN", flowIdentifierBasic));  // 1
            nodes.Add(new Node("ENDIF", flowIdentifierBasic));                      // 2
            nodes.Add(new Node("The use case ends.", flowIdentifierBasic));         // 3
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(
                new bool[,]
                {
                    { false, true, false, false },
                    { false, false, true, false },
                    { false, false, false, true },
                    { false, false, false, false }
                });
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            expectedPossibleInvalidIfEdges.Add(new InternalEdge(1, 2));
            List<int> expectedExitSteps = new List<int>();
            expectedExitSteps.Add(3);

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<int> exitSteps;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        /// <summary>
        /// Tests the edge creation of an if flow like if-elseif-else-endif with no block inside the.
        /// </summary>
        [Test]
        public void WireSimpleEmptyIfElseIfElseFlow()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("The use case starts.", flowIdentifierBasic));           // 0
            nodes.Add(new Node("IF it is hot outside THEN", flowIdentifierBasic));      // 1
            nodes.Add(new Node("ELSEIF it is colde outside THEN", flowIdentifierBasic));// 2
            nodes.Add(new Node("ELSE", flowIdentifierBasic));                           // 3
            nodes.Add(new Node("ENDIF", flowIdentifierBasic));                          // 4
            nodes.Add(new Node("The use case ends.", flowIdentifierBasic));             // 5
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(
                new bool[,]
                {
                    { false, true, false, false, false, false },
                    { false, false, true, true, true, false },
                    { false, false, false, false, true, false },
                    { false, false, false, false, true, false },
                    { false, false, false, false, false, true },
                    { false, false, false, false, false, false }
                });
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            List<int> expectedExitSteps = new List<int>();
            expectedExitSteps.Add(5);

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<int> exitSteps;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        /// <summary>
        /// Tests the edge creation of an if flow like if-elseif-elseif-else-endif with simple blocks inside the if statement.
        /// </summary>
        [Test]
        public void WireComplexIfFlow()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("The use case starts.", flowIdentifierBasic));                       // 0
            nodes.Add(new Node("IF it is hot outside THEN", flowIdentifierBasic));                  // 1
            nodes.Add(new Node("Start the fan.", flowIdentifierBasic));                             // 2
            nodes.Add(new Node("ELSEIF it is cold outside THEN", flowIdentifierBasic));             // 3
            nodes.Add(new Node("Start the AC", flowIdentifierBasic));                               // 4
            nodes.Add(new Node("ELSEIF it is warm outside THEN", flowIdentifierBasic));             // 5
            nodes.Add(new Node("Don't worry be happy", flowIdentifierBasic));                       // 6
            nodes.Add(new Node("ELSE", flowIdentifierBasic));                                       // 7
            nodes.Add(new Node("Was soll das für eine Temperatur sein?", flowIdentifierBasic));     // 8
            nodes.Add(new Node("ENDIF", flowIdentifierBasic));                                      // 9
            nodes.Add(new Node("The use case ends.", flowIdentifierBasic));                         // 10
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(
                new bool[,]
                {
                    { false, true, false, false, false, false, false, false, false, false, false },
                    { false, false, true, true, false, true, false, true, false, false, false },
                    { false, false, false, false, false, false, false, false, false, true, false },
                    { false, false, false, false, true, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false, false, true, false },
                    { false, false, false, false, false, false, true, false, false, false, false },
                    { false, false, false, false, false, false, false, false, false, true, false },
                    { false, false, false, false, false, false, false, false, true, false, false },
                    { false, false, false, false, false, false, false, false, false, true, false },
                    { false, false, false, false, false, false, false, false, false, false, true },
                    { false, false, false, false, false, false, false, false, false, false, false },
                });
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            List<int> expectedExitSteps = new List<int>();
            expectedExitSteps.Add(10);

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<int> exitSteps;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        /// <summary>
        /// Tests the edge creation of an if flow like if-elseif-else-endif with blocks consisting of Resume, Abort and other if statements inside the if statement.
        /// </summary>
        [Test]
        public void WireComplexNestedIfFlow()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("The use case starts.", flowIdentifierBasic));                       // 0
            nodes.Add(new Node("IF it is hot outside THEN", flowIdentifierBasic));                  // 1
            nodes.Add(new Node("IF the fan is off THEN", flowIdentifierBasic));                     // 2
            nodes.Add(new Node("Switch the fan on.", flowIdentifierBasic));                         // 3
            nodes.Add(new Node("ENDIF", flowIdentifierBasic));                                      // 4
            nodes.Add(new Node("ELSEIF it is cold outside THEN", flowIdentifierBasic));             // 5
            nodes.Add(new Node("RESUME 5", flowIdentifierBasic));                                   // 6
            nodes.Add(new Node("ELSE", flowIdentifierBasic));                                       // 7
            nodes.Add(new Node("ABORT", flowIdentifierBasic));                                      // 8
            nodes.Add(new Node("ENDIF", flowIdentifierBasic));                                      // 9
            nodes.Add(new Node("The use case ends.", flowIdentifierBasic));                         // 10
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(
                new bool[,]
                {
                    { false, true, false, false, false, false, false, false, false, false, false },
                    { false, false, true, false, false, true, false, true, false, false, false },
                    { false, false, false, true, true, false, false, false, false, false, false },
                    { false, false, false, false, true, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false, false, true, false },
                    { false, false, false, false, false, false, true, false, false, false, false },
                    { false, false, false, false, false, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false, true, false, false },
                    { false, false, false, false, false, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false, false, false, true },
                    { false, false, false, false, false, false, false, false, false, false, false }
                });
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            expectedExternalEdges.Add(new ExternalEdge(6, new ReferenceStep(new FlowIdentifier(FlowType.Basic, 0), 4)));
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            expectedPossibleInvalidIfEdges.Add(new InternalEdge(2, 4));
            List<int> expectedExitSteps = new List<int>();
            expectedExitSteps.Add(10);

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<int> exitSteps;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        /// <summary>
        /// Tests the edge creation of a splited if flow like else-endif in an alternative flow with with simple blocks inside the else statement.
        /// </summary>
        [Test]
        public void WireSimpleElseFlow()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("ELSE", flowIdentifierBasic));           // 0
            nodes.Add(new Node("Step inside.", flowIdentifierBasic));   // 1
            nodes.Add(new Node("ENDIF", flowIdentifierBasic));          // 2
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(
                new bool[,]
                {
                    { false, true, false },
                    { false, false, true },
                    { false, false, false }
                });
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            List<int> expectedExitSteps = new List<int>();
            expectedExitSteps.Add(2);

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<int> exitSteps;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        /// <summary>
        /// Tests the edge creation of a splited if flow like elseif-elseif-endif in an alternative flow with with simple blocks inside the else if statements.
        /// </summary>
        [Test]
        public void WireSimpleElseIfFlow()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("ELSEIF the crystal is blue THEN", flowIdentifierBasic));    // 0
            nodes.Add(new Node("Dissolve it in water.", flowIdentifierBasic));              // 1
            nodes.Add(new Node("ELSEIF the crystal is red THEN", flowIdentifierBasic));     // 2
            nodes.Add(new Node("Burn it in fire.", flowIdentifierBasic));                   // 3
            nodes.Add(new Node("ENDIF", flowIdentifierBasic));                              // 4
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(5, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 4] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[3, 4] = true;
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            List<int> expectedExitSteps = new List<int>();
            expectedExitSteps.Add(4);

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<int> exitSteps;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        /// <summary>
        /// Tests if an exception if thrown if a description is given that can create a loop.
        /// </summary>
        [Test]
        public void TryWirePossibleEndlessLoopInWrongFlowDescription()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("IF the crystal is blue THEN", flowIdentifierBasic));
            nodes.Add(new Node("Dissolve it in water.", flowIdentifierBasic));
            nodes.Add(new Node("IF the crystal is red THEN", flowIdentifierBasic));
            nodes.Add(new Node("Burn it in fire.", flowIdentifierBasic));
            nodes.Add(new Node("ENDIF", flowIdentifierBasic));

            // Act

            // Assert
            Assert.Catch(
                        typeof(Exception),
                        () =>
                        {
                            Matrix<bool> edgeMatrix;
                            List<ExternalEdge> externalEdges;
                            List<InternalEdge> possibleInvalidIfEdges;
                            List<int> exitSteps;
                            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps);
                        });
        }

        /// <summary>
        /// Tests the edge creation of a splited if flow like elseif-else-endif in an alternative flow with with complex blocks inside the else/else if statements.
        /// </summary>
        [Test]
        public void WireComplexeElseIfElseFlow()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("ELSEIF the crystal is green THEN", flowIdentifierBasic));  // 0
                nodes.Add(new Node("IF the crystal glows THEN", flowIdentifierBasic));      // 1
                    nodes.Add(new Node("Don't eat it!", flowIdentifierBasic));              // 2
                nodes.Add(new Node("ELSE", flowIdentifierBasic));                           // 3
                    nodes.Add(new Node("Eat it.", flowIdentifierBasic));                    // 4
                nodes.Add(new Node("ENDIF", flowIdentifierBasic));                          // 5
            nodes.Add(new Node("ELSE", flowIdentifierBasic));                               // 6
                nodes.Add(new Node("RESUME 6", flowIdentifierBasic));                       // 7
            nodes.Add(new Node("ENDIF", flowIdentifierBasic));                              // 8
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(9, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[1, 3] = true;
            expectedEdgeMatrix[2, 5] = true;
            expectedEdgeMatrix[3, 4] = true;
            expectedEdgeMatrix[4, 5] = true;
            expectedEdgeMatrix[5, 8] = true;
            expectedEdgeMatrix[6, 7] = true;
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            expectedExternalEdges.Add(new ExternalEdge(7, new ReferenceStep(new FlowIdentifier(FlowType.Basic, 0), 5)));
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            List<int> expectedExitSteps = new List<int>();
            expectedExitSteps.Add(8);

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<int> exitSteps;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        /// <summary>
        /// Tests the edge creation of a simple do-while statement with a simple nested block.
        /// </summary>
        [Test]
        public void WireSimpleDoWhileFlow()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("DO", flowIdentifierBasic));                         // 0
                nodes.Add(new Node("Stamp on the ground", flowIdentifierBasic));    // 1
                nodes.Add(new Node("Jump in the air", flowIdentifierBasic));        // 2
            nodes.Add(new Node("WHILE you are not tired", flowIdentifierBasic));    // 3
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(4, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[3, 0] = true;
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            List<int> expectedExitSteps = new List<int>();
            expectedExitSteps.Add(3);

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<int> exitSteps;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        /// <summary>
        /// Tests the edge creation of a simple do-while statement with a complex nested block.
        /// </summary>
        [Test]
        public void WireComplexDoWhileFlow()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("DO", flowIdentifierBasic));                         // 0
                nodes.Add(new Node("DO", flowIdentifierBasic));                     // 1
                    nodes.Add(new Node("Ask pin", flowIdentifierBasic));            // 2
                nodes.Add(new Node("WHILE pin is false", flowIdentifierBasic));     // 3
                nodes.Add(new Node("IF wants money THEN", flowIdentifierBasic));    // 4
                    nodes.Add(new Node("Give money", flowIdentifierBasic));         // 5
                nodes.Add(new Node("ENDIF", flowIdentifierBasic));                  // 6
                nodes.Add(new Node("Return card", flowIdentifierBasic));            // 7
            nodes.Add(new Node("WHILE power on", flowIdentifierBasic));             // 8
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(9, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[3, 4] = true;
            expectedEdgeMatrix[3, 1] = true;
            expectedEdgeMatrix[4, 5] = true;
            expectedEdgeMatrix[4, 6] = true;
            expectedEdgeMatrix[5, 6] = true;
            expectedEdgeMatrix[6, 7] = true;
            expectedEdgeMatrix[7, 8] = true;
            expectedEdgeMatrix[8, 0] = true;
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            expectedPossibleInvalidIfEdges.Add(new InternalEdge(4, 6));
            List<int> expectedExitSteps = new List<int>();
            expectedExitSteps.Add(8);

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<int> exitSteps;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        // --------------------------------------------------------------------------------------------- InsertMatrix

        /// <summary>
        /// Inserts a matrix into another one.
        /// </summary>
        [Test]
        public void InsertMatrix()
        {
            // Arrange
            Matrix<bool> targetMatrix = new Matrix<bool>(
                    new bool[,]
                    {
                        { false, false, false, true },
                        { false, false, true, false },
                        { false, true, false, false },
                        { true, false, false, false }
                    }), 
                sourceMatrix = new Matrix<bool>(
                    new bool[,]
                    {
                        { true, false },
                        { false, true }
                    }),
                expectedMatrix = new Matrix<bool>(
                    new bool[,]
                    {
                        { false, false, false, true },
                        { false, true, false, false },
                        { false, false, true, false },
                        { true, false, false, false }
                    });
            int targetRow = 1,
                targetColumn = 1;

            // Act
            GraphBuilder.InsertMatrix(ref targetMatrix, targetRow, targetColumn, sourceMatrix);

            // Assert
            Assert.AreEqual(expectedMatrix, targetMatrix);
        }

        // --------------------------------------------------------------------------------------------- GetImportantIfStatementSteps

        /// <summary>
        /// Tests if the important steps of a simple if statment are correctly detected.
        /// </summary>
        [Test]
        public void GetImportantStepsOfSimpleIfStatement()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("The use case starts.", flowIdentifierBasic));
            nodes.Add(new Node("IF Hans can fly THEN", flowIdentifierBasic));
            nodes.Add(new Node("A step in between.", flowIdentifierBasic));
            nodes.Add(new Node("ENDIF", flowIdentifierBasic));
            nodes.Add(new Node("The use case end.", flowIdentifierBasic));
            List<int> expectedImportantSteps = new List<int>();
            expectedImportantSteps.Add(1);
            expectedImportantSteps.Add(3);

            // Act
            List<int> importantSteps = GraphBuilder.GetImportantIfStatementSteps(nodes, 1);

            // Assert
            Assert.AreEqual(expectedImportantSteps, importantSteps);
        }

        /// <summary>
        /// Tests if the important steps of a complex, nested if statment are correctly detected.
        /// </summary>
        [Test]
        public void GetImportantStepsOfComplexNestedIfStatement()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("The use case starts.", flowIdentifierBasic));
            nodes.Add(new Node("IF Hans can fly THEN", flowIdentifierBasic));
            nodes.Add(new Node("A step in between.", flowIdentifierBasic));
            nodes.Add(new Node("IF he can fly THEN", flowIdentifierBasic));
            nodes.Add(new Node("1", flowIdentifierBasic));
            nodes.Add(new Node("2", flowIdentifierBasic));
            nodes.Add(new Node("ELSE", flowIdentifierBasic));
            nodes.Add(new Node("1", flowIdentifierBasic));
            nodes.Add(new Node("ENDIF", flowIdentifierBasic));
            nodes.Add(new Node("IF flying hans THEN", flowIdentifierBasic));
            nodes.Add(new Node("1", flowIdentifierBasic));
            nodes.Add(new Node("ENDIF", flowIdentifierBasic));
            nodes.Add(new Node("1", flowIdentifierBasic));
            nodes.Add(new Node("ELSEIF gunther can swim THEN", flowIdentifierBasic));
            nodes.Add(new Node("IF true THEN", flowIdentifierBasic));
            nodes.Add(new Node("ELSEIF", flowIdentifierBasic));
            nodes.Add(new Node("ELSE", flowIdentifierBasic));
            nodes.Add(new Node("ENDIF", flowIdentifierBasic));
            nodes.Add(new Node("ELSEIF false THEN", flowIdentifierBasic));
            nodes.Add(new Node("1", flowIdentifierBasic));
            nodes.Add(new Node("ELSE", flowIdentifierBasic));
            nodes.Add(new Node("4", flowIdentifierBasic));
            nodes.Add(new Node("ENDIF", flowIdentifierBasic));
            nodes.Add(new Node("The use case end.", flowIdentifierBasic));
            List<int> expectedImportantSteps = new List<int>();
            expectedImportantSteps.Add(1);
            expectedImportantSteps.Add(13);
            expectedImportantSteps.Add(18);
            expectedImportantSteps.Add(20);
            expectedImportantSteps.Add(22);

            // Act
            List<int> importantSteps = GraphBuilder.GetImportantIfStatementSteps(nodes, 1);

            // Assert
            Assert.AreEqual(expectedImportantSteps, importantSteps);
        }

        // --------------------------------------------------------------------------------------------- GetStepType

        /// <summary>
        /// Tests if the correct type for an unmatched statement is returned.
        /// </summary>
        [Test]
        public void GetUnmatchedType()
        {
            // Arrange
            string stepDescription = "Hans can fly.";
            StepType expectedStepType = StepType.Unmatched;

            // Act

            // Assert
            Assert.AreEqual(expectedStepType, GraphBuilder.GetStepType(stepDescription));
        }

        /// <summary>
        /// Tests if the correct type for an if statement is returned.
        /// </summary>
        [Test]
        public void GetIfType()
        {
            // Arrange
            string stepDescription = "IF the light is read THEN";
            StepType expectedStepType = StepType.If;

            // Act

            // Assert
            Assert.AreEqual(expectedStepType, GraphBuilder.GetStepType(stepDescription));
        }

        /// <summary>
        /// Tests if the correct type for an else statement is returned.
        /// </summary>
        [Test]
        public void GetElseType()
        {
            // Arrange
            string stepDescription = "ELSE";
            StepType expectedStepType = StepType.Else;

            // Act

            // Assert
            Assert.AreEqual(expectedStepType, GraphBuilder.GetStepType(stepDescription));
        }

        /// <summary>
        /// Tests if the correct type for an elseif statement is returned.
        /// </summary>
        [Test]
        public void GetElseIfType()
        {
            // Arrange
            string stepDescription = "ELSEIF the light is read THEN";
            StepType expectedStepType = StepType.ElseIf;

            // Act

            // Assert
            Assert.AreEqual(expectedStepType, GraphBuilder.GetStepType(stepDescription));
        }

        /// <summary>
        /// Tests if the correct type for an endif statement is returned.
        /// </summary>
        [Test]
        public void GetEndIfType()
        {
            // Arrange
            string stepDescription = "ENDIF";
            StepType expectedStepType = StepType.EndIf;

            // Act

            // Assert
            Assert.AreEqual(expectedStepType, GraphBuilder.GetStepType(stepDescription));
        }

        /// <summary>
        /// Tests if the correct type for an do statement is returned.
        /// </summary>
        [Test]
        public void GetDoType()
        {
            // Arrange
            string stepDescription = "DO";
            StepType expectedStepType = StepType.Do;

            // Act

            // Assert
            Assert.AreEqual(expectedStepType, GraphBuilder.GetStepType(stepDescription));
        }

        /// <summary>
        /// Tests if the correct type for an while statement is returned.
        /// </summary>
        [Test]
        public void GetWhileType()
        {
            // Arrange
            string stepDescription = "WHILE the light is read";
            StepType expectedStepType = StepType.While;

            // Act

            // Assert
            Assert.AreEqual(expectedStepType, GraphBuilder.GetStepType(stepDescription));
        }

        /// <summary>
        /// Tests if the correct type for an resume statement is returned.
        /// </summary>
        [Test]
        public void GetResumeType()
        {
            // Arrange
            string stepDescription = "RESUME 13";
            StepType expectedStepType = StepType.Resume;

            // Act

            // Assert
            Assert.AreEqual(expectedStepType, GraphBuilder.GetStepType(stepDescription));
        }

        /// <summary>
        /// Tests if the correct type for an abort statement is returned.
        /// </summary>
        [Test]
        public void GetAbortType()
        {
            // Arrange
            string stepDescription = "ABORT";
            StepType expectedStepType = StepType.Abort;

            // Act

            // Assert
            Assert.AreEqual(expectedStepType, GraphBuilder.GetStepType(stepDescription));
        }

        /// <summary>
        /// Tests if the correct type for an validates that statement is returned.
        /// </summary>
        [Test]
        public void GetValidatesThatType()
        {
            // Arrange
            string stepDescription = "The fireman VALIDATES THAT the door is open.";
            StepType expectedStepType = StepType.ValidatesThat;

            // Act

            // Assert
            Assert.AreEqual(expectedStepType, GraphBuilder.GetStepType(stepDescription));
        }

        // --------------------------------------------------------------------------------------------- IsEqualsToAtLeastOnePatternOfStepType

        /// <summary>
        /// Tests an if step against the if step type pattern.
        /// </summary>
        [Test]
        public void StepEqualToIfPattern()
        {
            // Arrange
            string stepDescription = "IF the light is read THEN";
            StepType stepType = StepType.If;

            // Act

            // Assert
            Assert.IsTrue(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests a non if step against the if step type pattern.
        /// </summary>
        [Test]
        public void StepNotEqualToIfPattern()
        {
            // Arrange
            string stepDescription = "ELSEIF Karl is happy THEN Hans is happy";
            StepType stepType = StepType.If;

            // Act

            // Assert
            Assert.IsFalse(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests an else step against the else step type pattern.
        /// </summary>
        [Test]
        public void StepEqualToElsePattern()
        {
            // Arrange
            string stepDescription = "ELSE";
            StepType stepType = StepType.Else;

            // Act

            // Assert
            Assert.IsTrue(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests a non else step against the else step type pattern.
        /// </summary>
        [Test]
        public void StepNotEqualToElsePattern()
        {
            // Arrange
            string stepDescription = "ELSEIF";
            StepType stepType = StepType.Else;

            // Act

            // Assert
            Assert.IsFalse(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests an elseif step against the elseif step type pattern.
        /// </summary>
        [Test]
        public void StepEqualToElseIfPattern()
        {
            // Arrange
            string stepDescription = "ELSEIF Hans flies THEN";
            StepType stepType = StepType.ElseIf;

            // Act

            // Assert
            Assert.IsTrue(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests a non elseif step against the elseif step type pattern.
        /// </summary>
        [Test]
        public void StepNotEqualToElseIfPattern()
        {
            // Arrange
            string stepDescription = "ELSE IF Hans can";
            StepType stepType = StepType.ElseIf;

            // Act

            // Assert
            Assert.IsFalse(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests an do step against the do step type pattern.
        /// </summary>
        [Test]
        public void StepEqualToDoPattern()
        {
            // Arrange
            string stepDescription = "DO";
            StepType stepType = StepType.Do;

            // Act

            // Assert
            Assert.IsTrue(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests a non do step against the do step type pattern.
        /// </summary>
        [Test]
        public void StepNotEqualToDoPattern()
        {
            // Arrange
            string stepDescription = "ELSEIF Karl is happy THEN Hans is happy";
            StepType stepType = StepType.Do;

            // Act

            // Assert
            Assert.IsFalse(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests an while step against the while step type pattern.
        /// </summary>
        [Test]
        public void StepEqualToWhilePattern()
        {
            // Arrange
            string stepDescription = "WHILE Will is alive";
            StepType stepType = StepType.While;

            // Act

            // Assert
            Assert.IsTrue(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests a non while step against the while step type pattern.
        /// </summary>
        [Test]
        public void StepNotEqualToWhilePattern()
        {
            // Arrange
            string stepDescription = "ELSEIF Karl is happy THEN Hans is happy";
            StepType stepType = StepType.While;

            // Act

            // Assert
            Assert.IsFalse(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests an resume step against the resume step type pattern.
        /// </summary>
        [Test]
        public void StepEqualToResumePattern()
        {
            // Arrange
            string stepDescription = "RESUME 3";
            StepType stepType = StepType.Resume;

            // Act

            // Assert
            Assert.IsTrue(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests a non resume step against the resume step type pattern.
        /// </summary>
        [Test]
        public void StepNotEqualToResumePattern()
        {
            // Arrange
            string stepDescription = "RESUMEE";
            StepType stepType = StepType.Resume;

            // Act

            // Assert
            Assert.IsFalse(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests an abort step against the abort step type pattern.
        /// </summary>
        [Test]
        public void StepEqualToAbortPattern()
        {
            // Arrange
            string stepDescription = "ABORT";
            StepType stepType = StepType.Abort;

            // Act

            // Assert
            Assert.IsTrue(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests a non abort step against the abort step type pattern.
        /// </summary>
        [Test]
        public void StepNotEqualToAbortPattern()
        {
            // Arrange
            string stepDescription = "Franz";
            StepType stepType = StepType.Abort;

            // Act

            // Assert
            Assert.IsFalse(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests an validates that step against the validates that step type pattern.
        /// </summary>
        [Test]
        public void StepEqualToValidatesThatPattern()
        {
            // Arrange
            string stepDescription = "The fireman VALIDATES THAT the engine is started!";
            StepType stepType = StepType.ValidatesThat;

            // Act

            // Assert
            Assert.IsTrue(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests a non validates that step against the validates that step type pattern.
        /// </summary>
        [Test]
        public void StepNotEqualToValidatesThatPattern()
        {
            // Arrange
            string stepDescription = "Franz";
            StepType stepType = StepType.ValidatesThat;

            // Act

            // Assert
            Assert.IsFalse(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        // --------------------------------------------------------------------------------------------- GetMatchingPatternForStepType

        /// <summary>
        /// Tests if a pattern is returned for a valid if statement when compared to the if step type.
        /// </summary>
        [Test]
        public void PatternFromIfStepTypeReturnedForValidIfStatement()
        {
            // Arrange
            string stepDescription = "IF Hans can fly THEN";
            StepType stepType = StepType.If;

            // Act

            // Assert
            Assert.IsNotNull(GraphBuilder.GetMatchingPatternForStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests if null is returned for a not if statement when compared to the if step type.
        /// </summary>
        [Test]
        public void PatternFromIfStepTypeReturnedForinvalidIfStatement()
        {
            // Arrange
            string stepDescription = "ELSEIF Hans can fly THEN";
            StepType stepType = StepType.If;

            // Act

            // Assert
            Assert.IsNull(GraphBuilder.GetMatchingPatternForStepType(stepDescription, stepType));
        }
    }
}
