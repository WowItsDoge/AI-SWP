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
        /// Prints the edges of the matrix into the console, that are not equal to the match object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix"></param>
        /// <param name="matchObject"></param>
        public void PrintEdgesInverse<T>(Matrix<T> matrix, T matchObject)
        {
            for (int row = 0; row < matrix.RowCount; row++)
            {
                for (int column = 0; column < matrix.ColumnCount; column++)
                {
                    if (!(matrix[row, column].Equals(matchObject)))
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
        public void BuildEmptyBasic()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB1Basic()
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

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 2] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB2BasicIfSpecificElseAbort()
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

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[1, 4] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[4, 5] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[1,2] = new Condition(allSteps[1].StepDescription, true);
            expectedConditionMatrix[1,4] = new Condition(allSteps[1].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB3BasicIfSpecificElseResume()
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

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[1, 2] = new Condition(allSteps[1].StepDescription, true);
            expectedConditionMatrix[1, 5] = new Condition(allSteps[1].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB4GlobalResume()
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
            globalSteps.Add(new Node("If global should be executed", flowIdentifierGlobal1));
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

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 3] = new Condition(globalSteps[0].StepDescription, true);
            expectedConditionMatrix[1, 3] = new Condition(globalSteps[0].StepDescription, true);
            expectedConditionMatrix[2, 3] = new Condition(globalSteps[0].StepDescription, true);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB5Basic2IfBoundedElseIfElseResumeAbort()
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
            expectedEdgeMatrix[6,8] = true;
            expectedEdgeMatrix[8,9] = true;
            expectedEdgeMatrix[9,5] = true;
            expectedEdgeMatrix[8,10] = true;
            expectedEdgeMatrix[10,11] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(basicSteps[0].StepDescription, true);
            expectedConditionMatrix[0,6] = new Condition(basicSteps[0].StepDescription, false);
            expectedConditionMatrix[3,4] = new Condition(basicSteps[3].StepDescription, true);
            expectedConditionMatrix[3,6] = new Condition(basicSteps[3].StepDescription, false);
            expectedConditionMatrix[6,7] = new Condition(allSteps[6].StepDescription, true);
            expectedConditionMatrix[6,8] = new Condition(allSteps[6].StepDescription, false);
            expectedConditionMatrix[8,9] = new Condition(allSteps[8].StepDescription, true);
            expectedConditionMatrix[8,10] = new Condition(allSteps[8].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB6Complex1()
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
            basicSteps.Add(new Node("You VALIDATES THAT clouds are falling", flowIdentifierBasic));
            basicSteps.Add(new Node("We VALIDATES THAT you go home", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            // automatically extended basic flow to not lose the last condition
            allSteps.Add(new Node(string.Empty, flowIdentifierBasic));

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
            boundedSteps2.Add(new Node("UNTIL there are peas on the floor", flowIdentifierBounded1));
            boundedSteps2.Add(new Node("ABORT", flowIdentifierBounded1));
            List<ReferenceStep> boundedReferenceSteps2 = new List<ReferenceStep>();
            boundedReferenceSteps2.Add(new ReferenceStep(flowIdentifierBasic, 10));
            boundedFlows.Add(new Flow(flowIdentifierBounded1, null, boundedSteps2, boundedReferenceSteps2));
            allSteps.AddRange(boundedSteps2);

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
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
            expectedEdgeMatrix[9,10] = true;

            // specific/bounded
            expectedEdgeMatrix[3,11] = true;
            expectedEdgeMatrix[11,0] = true;
            expectedEdgeMatrix[6,12] = true;
            expectedEdgeMatrix[12,5] = true;
            expectedEdgeMatrix[7,15] = true;
            expectedEdgeMatrix[8,15] = true;
            expectedEdgeMatrix[9, 16] = true;
            expectedEdgeMatrix[16, 17] = true;
            expectedEdgeMatrix[17, 18] = true;
            expectedEdgeMatrix[18, 16] = true;
            expectedEdgeMatrix[18, 19] = true;

            // globals
            expectedEdgeMatrix[0,13] = true;
            expectedEdgeMatrix[1,13] = true;
            expectedEdgeMatrix[2,13] = true;
            expectedEdgeMatrix[3,13] = true;
            expectedEdgeMatrix[4, 13] = true;
            expectedEdgeMatrix[5, 13] = true;
            expectedEdgeMatrix[6, 13] = true;
            expectedEdgeMatrix[7, 13] = true;
            expectedEdgeMatrix[8, 13] = true;
            expectedEdgeMatrix[9, 13] = true;
            expectedEdgeMatrix[10, 13] = true;

            expectedEdgeMatrix[0,14] = true;
            expectedEdgeMatrix[1, 14] = true;
            expectedEdgeMatrix[2, 14] = true;
            expectedEdgeMatrix[3, 14] = true;
            expectedEdgeMatrix[4, 14] = true;
            expectedEdgeMatrix[5, 14] = true;
            expectedEdgeMatrix[6, 14] = true;
            expectedEdgeMatrix[7, 14] = true;
            expectedEdgeMatrix[8, 14] = true;
            expectedEdgeMatrix[9, 14] = true;
            expectedEdgeMatrix[10, 14] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            // basic
            expectedConditionMatrix[0,1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0,2] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[3,4] = new Condition(allSteps[3].StepDescription, true);
            expectedConditionMatrix[6,7] = new Condition(allSteps[6].StepDescription, true);
            expectedConditionMatrix[7,8] = new Condition(allSteps[7].StepDescription, true);
            expectedConditionMatrix[8,9] = new Condition(allSteps[8].StepDescription, true);
            expectedConditionMatrix[9,10] = new Condition(allSteps[9].StepDescription, true);

            // specific/bounded
            expectedConditionMatrix[3,11] = new Condition(allSteps[3].StepDescription, false);
            expectedConditionMatrix[6,12] = new Condition(allSteps[6].StepDescription, false);
            expectedConditionMatrix[7,15] = new Condition(allSteps[7].StepDescription, false);
            expectedConditionMatrix[8,15] = new Condition(allSteps[8].StepDescription, false);
            expectedConditionMatrix[9,16] = new Condition(allSteps[9].StepDescription, false);

            expectedConditionMatrix[18, 16] = new Condition(allSteps[18].StepDescription, false);
            expectedConditionMatrix[18, 19] = new Condition(allSteps[18].StepDescription, true);

            // global
            expectedConditionMatrix[0,13] = new Condition(allSteps[13].StepDescription, true);
            expectedConditionMatrix[1, 13] = new Condition(allSteps[13].StepDescription, true);
            expectedConditionMatrix[2, 13] = new Condition(allSteps[13].StepDescription, true);
            expectedConditionMatrix[3, 13] = new Condition(allSteps[13].StepDescription, true);
            expectedConditionMatrix[4, 13] = new Condition(allSteps[13].StepDescription, true);
            expectedConditionMatrix[5, 13] = new Condition(allSteps[13].StepDescription, true);
            expectedConditionMatrix[6, 13] = new Condition(allSteps[13].StepDescription, true);
            expectedConditionMatrix[7, 13] = new Condition(allSteps[13].StepDescription, true);
            expectedConditionMatrix[8, 13] = new Condition(allSteps[13].StepDescription, true);
            expectedConditionMatrix[9, 13] = new Condition(allSteps[13].StepDescription, true);
            expectedConditionMatrix[10, 13] = new Condition(allSteps[13].StepDescription, true);

            expectedConditionMatrix[0, 14] = new Condition(allSteps[14].StepDescription, true);
            expectedConditionMatrix[1, 14] = new Condition(allSteps[14].StepDescription, true);
            expectedConditionMatrix[2, 14] = new Condition(allSteps[14].StepDescription, true);
            expectedConditionMatrix[3, 14] = new Condition(allSteps[14].StepDescription, true);
            expectedConditionMatrix[4, 14] = new Condition(allSteps[14].StepDescription, true);
            expectedConditionMatrix[5, 14] = new Condition(allSteps[14].StepDescription, true);
            expectedConditionMatrix[6, 14] = new Condition(allSteps[14].StepDescription, true);
            expectedConditionMatrix[7, 14] = new Condition(allSteps[14].StepDescription, true);
            expectedConditionMatrix[8, 14] = new Condition(allSteps[14].StepDescription, true);
            expectedConditionMatrix[9, 14] = new Condition(allSteps[14].StepDescription, true);
            expectedConditionMatrix[10, 14] = new Condition(allSteps[14].StepDescription, true);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB7BasicIf()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the use case starts THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Celebrate a party.", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[1, 2] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB8BasicEmptyIf()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the use case starts THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB9BasicIfElse()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the use case starts THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Celebrate a party.", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSE", flowIdentifierBasic));
            basicSteps.Add(new Node("Go in a corner and cry.", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[1, 4] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[3, 4] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB10BasicEmptyIf()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the use case starts THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSE", flowIdentifierBasic));
            basicSteps.Add(new Node("Go in a corner and cry.", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 3] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[2, 3] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[0, 3] = new Condition(allSteps[0].StepDescription, true);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB11BasicIfEmptyElse()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the use case starts THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Celebrate a party.", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSE", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[1, 3] = true;
            expectedEdgeMatrix[2, 3] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB12_1BasicEmptyIfEmptyElse()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the use case starts THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSE", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[1, 2] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, true);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB12_2BasicIfElseIf()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the apple is rotten THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Eat it", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSEIF the apple is golden THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Throw it in the well", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[1, 4] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[2, 4] = true;
            expectedEdgeMatrix[3, 4] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[2, 3] = new Condition(allSteps[2].StepDescription, true);
            expectedConditionMatrix[2, 4] = new Condition(allSteps[2].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB13BasicEmptyIfElseIf()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the apple is rotten THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSEIF the apple is golden THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Throw it in the well", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 3] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[1, 3] = true;
            expectedEdgeMatrix[2, 3] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[0, 3] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[1, 2] = new Condition(allSteps[1].StepDescription, true);
            expectedConditionMatrix[1, 3] = new Condition(allSteps[1].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB14BasicIfEmptyElseIf()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the apple is rotten THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Eat it", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSEIF the apple is golden THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[1, 3] = true;
            expectedEdgeMatrix[2, 3] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB15BasicEmptyIfEmptyElseIf()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the apple is rotten THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSEIF the apple is golden THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[1, 2] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, true);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB16BasicIfElseIfElse()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF there are traffic lights THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Watch the traffic lights", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSEIF there are traffic signs THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Watch the traffic signs", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSE", flowIdentifierBasic));
            basicSteps.Add(new Node("Give priority to the right", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[1, 6] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[2, 4] = true;
            expectedEdgeMatrix[3, 6] = true;
            expectedEdgeMatrix[4, 5] = true;
            expectedEdgeMatrix[5, 6] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[2, 3] = new Condition(allSteps[2].StepDescription, true);
            expectedConditionMatrix[2, 4] = new Condition(allSteps[2].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB17BasicEmptyIfElseIfElse()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF there are traffic lights THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSEIF there are traffic signs THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Watch the traffic signs", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSE", flowIdentifierBasic));
            basicSteps.Add(new Node("Give priority to the right", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 5] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[1, 3] = true;
            expectedEdgeMatrix[2, 5] = true;
            expectedEdgeMatrix[3, 4] = true;
            expectedEdgeMatrix[4, 5] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[0, 5] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[1, 2] = new Condition(allSteps[1].StepDescription, true);
            expectedConditionMatrix[1, 3] = new Condition(allSteps[1].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB18BasicIfEmptyElseIfElse()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF there are traffic lights THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Watch the traffic lights", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSEIF there are traffic signs THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSE", flowIdentifierBasic));
            basicSteps.Add(new Node("Give priority to the right", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[1, 5] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[2, 5] = true;
            expectedEdgeMatrix[3, 4] = true;
            expectedEdgeMatrix[4, 5] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[2, 3] = new Condition(allSteps[2].StepDescription, false);
            expectedConditionMatrix[2, 5] = new Condition(allSteps[2].StepDescription, true);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB19BasicEmptyIfEmptyElseIfElse()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF there are traffic lights THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSEIF there are traffic signs THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSE", flowIdentifierBasic));
            basicSteps.Add(new Node("Give priority to the right", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 4] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[1, 4] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[3, 4] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[0, 4] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[1, 2] = new Condition(allSteps[1].StepDescription, false);
            expectedConditionMatrix[1, 4] = new Condition(allSteps[1].StepDescription, true);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB20BasicIfElseIfEmptyElse()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF there are traffic lights THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Watch the traffic lights", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSEIF there are traffic signs THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Watch the traffic signs", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSE", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[1, 5] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[2, 4] = true;
            expectedEdgeMatrix[3, 5] = true;
            expectedEdgeMatrix[4, 5] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[2, 3] = new Condition(allSteps[2].StepDescription, true);
            expectedConditionMatrix[2, 4] = new Condition(allSteps[2].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB21BasicEmptyIfElseIfEmptyElse()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF there are traffic lights THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSEIF there are traffic signs THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Watch the traffic signs", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSE", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 4] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[1, 3] = true;
            expectedEdgeMatrix[2, 4] = true;
            expectedEdgeMatrix[3, 4] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[0, 4] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[1, 2] = new Condition(allSteps[1].StepDescription, true);
            expectedConditionMatrix[1, 3] = new Condition(allSteps[1].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB22BasicIfEmptyElseIfEmptyElse()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF there are traffic lights THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Watch the traffic lights", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSEIF there are traffic signs THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSE", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[1, 4] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[2, 4] = true;
            expectedEdgeMatrix[3, 4] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[2, 3] = new Condition(allSteps[2].StepDescription, false);
            expectedConditionMatrix[2, 4] = new Condition(allSteps[2].StepDescription, true);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB23BasicEmptyIfEmptyElseIfEmptyElse()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF there are traffic lights THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSEIF there are traffic signs THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSE", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 3] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[1, 3] = true;
            expectedEdgeMatrix[2, 3] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[0, 3] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[1, 2] = new Condition(allSteps[1].StepDescription, false);
            expectedConditionMatrix[1, 3] = new Condition(allSteps[1].StepDescription, true);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB24BasicIfElseIfElse()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF there are police men THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Watch the police men", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSEIF there are traffic lights THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Watch the traffic lights", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSEIF there are traffic signs THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Watch the traffic signs", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSE", flowIdentifierBasic));
            basicSteps.Add(new Node("Give priority to the right", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[1, 8] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[2, 4] = true;
            expectedEdgeMatrix[3, 8] = true;
            expectedEdgeMatrix[4, 5] = true;
            expectedEdgeMatrix[4, 6] = true;
            expectedEdgeMatrix[5, 8] = true;
            expectedEdgeMatrix[6, 7] = true;
            expectedEdgeMatrix[7, 8] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[2, 3] = new Condition(allSteps[2].StepDescription, true);
            expectedConditionMatrix[2, 4] = new Condition(allSteps[2].StepDescription, false);
            expectedConditionMatrix[4, 5] = new Condition(allSteps[4].StepDescription, true);
            expectedConditionMatrix[4, 6] = new Condition(allSteps[4].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB25BasicDoUntil()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("DO", flowIdentifierBasic));
            basicSteps.Add(new Node("Clap your hands", flowIdentifierBasic));
            basicSteps.Add(new Node("UNTIL you know you are unhappy", flowIdentifierBasic));
            basicSteps.Add(new Node("Care about your mood!", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[2, 0] = true;
            expectedEdgeMatrix[2, 3] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[2, 0] = new Condition(allSteps[2].StepDescription, false);
            expectedConditionMatrix[2, 3] = new Condition(allSteps[2].StepDescription, true);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB26BasicDoUntilLastStep()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("DO", flowIdentifierBasic));
            basicSteps.Add(new Node("Clap your hands", flowIdentifierBasic));
            basicSteps.Add(new Node("UNTIL you know you are unhappy", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);
            allSteps.Add(new Node(string.Empty, flowIdentifierBasic));

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[2, 0] = true;
            expectedEdgeMatrix[2, 3] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[2, 0] = new Condition(allSteps[2].StepDescription, false);
            expectedConditionMatrix[2, 3] = new Condition(allSteps[2].StepDescription, true);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB27BasicValidatesThatSpecificAbort()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("If the boss enthers the room", flowIdentifierBasic));
            basicSteps.Add(new Node("the first thing is that he VALIDATES THAT the personnel is working", flowIdentifierBasic));
            basicSteps.Add(new Node("and takes seat on his chair.", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();

            List<Node> specificSteps1 = new List<Node>();
            specificSteps1.Add(new Node("ABORT", flowIdentifierSpecific1));
            List<ReferenceStep> specificReferenceSteps1 = new List<ReferenceStep>();
            specificReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 2));
            specificFlows.Add(new Flow(flowIdentifierSpecific1, null, specificSteps1, specificReferenceSteps1));
            allSteps.AddRange(specificSteps1);

            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[1, 3] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[1, 2] = new Condition(allSteps[1].StepDescription, true);
            expectedConditionMatrix[1, 3] = new Condition(allSteps[1].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB28BasicValidatesThatLastStepSpecificAbort()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("If the boss enthers the room", flowIdentifierBasic));
            basicSteps.Add(new Node("the first thing is that he VALIDATES THAT the personnel is working", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);
            allSteps.Add(new Node(string.Empty, flowIdentifierBasic));

            List<Flow> specificFlows = new List<Flow>();

            List<Node> specificSteps1 = new List<Node>();
            specificSteps1.Add(new Node("ABORT", flowIdentifierSpecific1));
            List<ReferenceStep> specificReferenceSteps1 = new List<ReferenceStep>();
            specificReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 2));
            specificFlows.Add(new Flow(flowIdentifierSpecific1, null, specificSteps1, specificReferenceSteps1));
            allSteps.AddRange(specificSteps1);

            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[1, 3] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[1, 2] = new Condition(allSteps[1].StepDescription, true);
            expectedConditionMatrix[1, 3] = new Condition(allSteps[1].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB29BasicValidatesThatBoundedResume()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("If the boss enthers the room", flowIdentifierBasic));
            basicSteps.Add(new Node("the first thing is that he VALIDATES THAT the personnel is working", flowIdentifierBasic));
            basicSteps.Add(new Node("and takes seat on his chair.", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            List<Node> boundedSteps1 = new List<Node>();
            boundedSteps1.Add(new Node("RESUME 1", flowIdentifierBounded1));
            List<ReferenceStep> boundedReferenceSteps1 = new List<ReferenceStep>();
            boundedReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 2));
            boundedFlows.Add(new Flow(flowIdentifierBounded1, null, boundedSteps1, boundedReferenceSteps1));
            allSteps.AddRange(boundedSteps1);

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[1, 3] = true;
            expectedEdgeMatrix[3, 0] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[1, 2] = new Condition(allSteps[1].StepDescription, true);
            expectedConditionMatrix[1, 3] = new Condition(allSteps[1].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB30BasicValidatesThatMultiBoundedAbort()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("VALIDATES THAT the engine starts", flowIdentifierBasic));
            basicSteps.Add(new Node("Warm up the engine and tires", flowIdentifierBasic));
            basicSteps.Add(new Node("VALIDATES THAT the engine is running well", flowIdentifierBasic));
            basicSteps.Add(new Node("VALIDATES THAT the suspension is well", flowIdentifierBasic));
            basicSteps.Add(new Node("Start the race", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            List<Node> boundedSteps1 = new List<Node>();
            boundedSteps1.Add(new Node("ABORT", flowIdentifierBounded1));
            List<ReferenceStep> boundedReferenceSteps1 = new List<ReferenceStep>();
            boundedReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 1));
            boundedReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 3));
            boundedReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 4));
            boundedFlows.Add(new Flow(flowIdentifierBounded1, null, boundedSteps1, boundedReferenceSteps1));
            allSteps.AddRange(boundedSteps1);

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 5] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[2, 5] = true;
            expectedEdgeMatrix[3, 4] = true;
            expectedEdgeMatrix[3, 5] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 5] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[2, 3] = new Condition(allSteps[2].StepDescription, true);
            expectedConditionMatrix[2, 5] = new Condition(allSteps[2].StepDescription, false);
            expectedConditionMatrix[3, 4] = new Condition(allSteps[3].StepDescription, true);
            expectedConditionMatrix[3, 5] = new Condition(allSteps[3].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB31BasicValidatesThatSpecificAbortNestedInIf()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the pot is empty THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("The bear VALIDATES THAT the bees produced honey", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();

            List<Node> specificSteps1 = new List<Node>();
            specificSteps1.Add(new Node("ABORT", flowIdentifierSpecific1));
            List<ReferenceStep> specificReferenceSteps1 = new List<ReferenceStep>();
            specificReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 2));
            specificFlows.Add(new Flow(flowIdentifierSpecific1, null, specificSteps1, specificReferenceSteps1));
            allSteps.AddRange(specificSteps1);

            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[1, 3] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[1, 2] = new Condition(allSteps[1].StepDescription, true);
            expectedConditionMatrix[1, 3] = new Condition(allSteps[1].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB32BasicDoUntilNestedInIf()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the pot is empty THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("DO", flowIdentifierBasic));
            basicSteps.Add(new Node("Gather honey from the bees", flowIdentifierBasic));
            basicSteps.Add(new Node("UNTIL the pot is full again", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 4] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[3, 1] = true;
            expectedEdgeMatrix[3, 4] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 4] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[3, 1] = new Condition(allSteps[3].StepDescription, false);
            expectedConditionMatrix[3, 4] = new Condition(allSteps[3].StepDescription, true);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB33BasicIfSpecificElseAbortInside()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the carrots are grown THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Harvest the carrots", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();

            List<Node> specificSteps1 = new List<Node>();
            specificSteps1.Add(new Node("ELSE", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ABORT", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ENDIF", flowIdentifierSpecific1));
            List<ReferenceStep> specificReferenceSteps1 = new List<ReferenceStep>();
            specificReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 1));
            specificFlows.Add(new Flow(flowIdentifierSpecific1, null, specificSteps1, specificReferenceSteps1));
            allSteps.AddRange(specificSteps1);

            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 3] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[3, 4] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 3] = new Condition(allSteps[0].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB34BasicEmptyIfSpecificElseAbortOutside()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the carrots are grown THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();

            List<Node> specificSteps1 = new List<Node>();
            specificSteps1.Add(new Node("ELSE", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("Water the carrots", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ENDIF", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ABORT", flowIdentifierSpecific1));
            List<ReferenceStep> specificReferenceSteps1 = new List<ReferenceStep>();
            specificReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 1));
            specificFlows.Add(new Flow(flowIdentifierSpecific1, null, specificSteps1, specificReferenceSteps1));
            allSteps.AddRange(specificSteps1);

            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[3, 4] = true;
            expectedEdgeMatrix[4, 5] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB35BasicEmptyIfSpecificElseIfAbort()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the carrots are grown THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();

            List<Node> specificSteps1 = new List<Node>();
            specificSteps1.Add(new Node("ELSEIF carrots are planted THEN", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ABORT", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ENDIF", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ABORT", flowIdentifierSpecific1));
            List<ReferenceStep> specificReferenceSteps1 = new List<ReferenceStep>();
            specificReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 1));
            specificFlows.Add(new Flow(flowIdentifierSpecific1, null, specificSteps1, specificReferenceSteps1));
            allSteps.AddRange(specificSteps1);

            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[2, 4] = true;
            expectedEdgeMatrix[4, 5] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[2, 3] = new Condition(allSteps[2].StepDescription, true);
            expectedConditionMatrix[2, 4] = new Condition(allSteps[2].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB36BasicIfSpecificEmptyElseIfAbort()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the carrots are grown THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();

            List<Node> specificSteps1 = new List<Node>();
            specificSteps1.Add(new Node("ELSEIF carrots are planted THEN", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ENDIF", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ABORT", flowIdentifierSpecific1));
            List<ReferenceStep> specificReferenceSteps1 = new List<ReferenceStep>();
            specificReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 1));
            specificFlows.Add(new Flow(flowIdentifierSpecific1, null, specificSteps1, specificReferenceSteps1));
            allSteps.AddRange(specificSteps1);

            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[3, 4] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB37BasicIfSpecificEmptyElseAbort()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the carrots are grown THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();

            List<Node> specificSteps1 = new List<Node>();
            specificSteps1.Add(new Node("ELSE", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ENDIF", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ABORT", flowIdentifierSpecific1));
            List<ReferenceStep> specificReferenceSteps1 = new List<ReferenceStep>();
            specificReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 1));
            specificFlows.Add(new Flow(flowIdentifierSpecific1, null, specificSteps1, specificReferenceSteps1));
            allSteps.AddRange(specificSteps1);

            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[3, 4] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB38BasicIfSpecificElseIfElseAbort()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the carrots are grown THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();

            List<Node> specificSteps1 = new List<Node>();
            specificSteps1.Add(new Node("ELSEIF carrots are planted THEN", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("Water the carrots", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ELSE", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("Plant carrots", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ENDIF", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ABORT", flowIdentifierSpecific1));
            List<ReferenceStep> specificReferenceSteps1 = new List<ReferenceStep>();
            specificReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 1));
            specificFlows.Add(new Flow(flowIdentifierSpecific1, null, specificSteps1, specificReferenceSteps1));
            allSteps.AddRange(specificSteps1);

            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[2, 4] = true;
            expectedEdgeMatrix[3, 6] = true;
            expectedEdgeMatrix[4, 5] = true;
            expectedEdgeMatrix[5, 6] = true;
            expectedEdgeMatrix[6, 7] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[2, 3] = new Condition(allSteps[2].StepDescription, true);
            expectedConditionMatrix[2, 4] = new Condition(allSteps[2].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB39NestedBasic()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the cauldron is filled with water THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("DO", flowIdentifierBasic));
            basicSteps.Add(new Node("IF you need another wart THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("VALIDATES THAT they have an even scrumbled surface", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSE", flowIdentifierBasic));
            basicSteps.Add(new Node("VALIDATES THAT the water does not turn purple", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            basicSteps.Add(new Node("UNTIL the water turns red", flowIdentifierBasic));
            basicSteps.Add(new Node("ELSE", flowIdentifierBasic));
            basicSteps.Add(new Node("VALIDATES THAT the fire is out", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();
            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            List<Node> boundedSteps1 = new List<Node>();
            boundedSteps1.Add(new Node("ABORT", flowIdentifierBounded1));
            List<ReferenceStep> boundedReferenceSteps1 = new List<ReferenceStep>();
            boundedReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 4));
            boundedReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 6));
            boundedReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 10));
            boundedFlows.Add(new Flow(flowIdentifierBounded1, null, boundedSteps1, boundedReferenceSteps1));
            allSteps.AddRange(boundedSteps1);

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 8] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[2, 4] = true;
            expectedEdgeMatrix[3, 6] = true;
            expectedEdgeMatrix[3, 11] = true;
            expectedEdgeMatrix[4, 5] = true;
            expectedEdgeMatrix[5, 6] = true;
            expectedEdgeMatrix[5, 11] = true;
            expectedEdgeMatrix[6, 7] = true;
            expectedEdgeMatrix[7, 10] = true;
            expectedEdgeMatrix[7, 1] = true;
            expectedEdgeMatrix[8, 9] = true;
            expectedEdgeMatrix[9, 10] = true;
            expectedEdgeMatrix[9, 11] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 8] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[2, 3] = new Condition(allSteps[2].StepDescription, true);
            expectedConditionMatrix[2, 4] = new Condition(allSteps[2].StepDescription, false);
            expectedConditionMatrix[3, 6] = new Condition(allSteps[3].StepDescription, true);
            expectedConditionMatrix[3, 11] = new Condition(allSteps[3].StepDescription, false);
            expectedConditionMatrix[5, 6] = new Condition(allSteps[5].StepDescription, true);
            expectedConditionMatrix[5, 11] = new Condition(allSteps[5].StepDescription, false);
            expectedConditionMatrix[7, 1] = new Condition(allSteps[7].StepDescription, false);
            expectedConditionMatrix[7, 10] = new Condition(allSteps[7].StepDescription, true);
            expectedConditionMatrix[9, 10] = new Condition(allSteps[9].StepDescription, true);
            expectedConditionMatrix[9, 11] = new Condition(allSteps[9].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB40NestedSpecific()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the cauldron is filled with water THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("Make tea", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();

            List<Node> specificSteps1 = new List<Node>();
            specificSteps1.Add(new Node("ELSEIF hot water is needed THEN", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("DO", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("Add water", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("UNTIL the cauldron is filled", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ELSE", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("DO", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("UNTIL the cauldron is clean", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ENDIF", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ABORT", flowIdentifierSpecific1));
            List<ReferenceStep> specificReferenceSteps1 = new List<ReferenceStep>();
            specificReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 1));
            specificFlows.Add(new Flow(flowIdentifierSpecific1, null, specificSteps1, specificReferenceSteps1));
            allSteps.AddRange(specificSteps1);

            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 3] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[3, 4] = true;
            expectedEdgeMatrix[3, 7] = true;
            expectedEdgeMatrix[4, 5] = true;
            expectedEdgeMatrix[5, 6] = true;
            expectedEdgeMatrix[6, 4] = true;
            expectedEdgeMatrix[6, 10] = true;
            expectedEdgeMatrix[7, 8] = true;
            expectedEdgeMatrix[8, 9] = true;
            expectedEdgeMatrix[9, 8] = true;
            expectedEdgeMatrix[9, 10] = true;
            expectedEdgeMatrix[10, 11] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 3] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[3, 4] = new Condition(allSteps[3].StepDescription, true);
            expectedConditionMatrix[3, 7] = new Condition(allSteps[3].StepDescription, false);
            expectedConditionMatrix[6, 4] = new Condition(allSteps[6].StepDescription, false);
            expectedConditionMatrix[6, 10] = new Condition(allSteps[6].StepDescription, true);
            expectedConditionMatrix[9, 8] = new Condition(allSteps[9].StepDescription, false);
            expectedConditionMatrix[9, 10] = new Condition(allSteps[9].StepDescription, true);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
        }

        [Test]
        public void TB41BasicEmptyIfSpecificElseAbortInsideAndOutside()
        {
            // Arrange
            List<Node> allSteps = new List<Node>();

            List<Node> basicSteps = new List<Node>();
            basicSteps.Add(new Node("IF the carrots are grown THEN", flowIdentifierBasic));
            basicSteps.Add(new Node("ENDIF", flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>());
            allSteps.AddRange(basicSteps);

            List<Flow> specificFlows = new List<Flow>();

            List<Node> specificSteps1 = new List<Node>();
            specificSteps1.Add(new Node("ELSE", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ABORT", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ENDIF", flowIdentifierSpecific1));
            specificSteps1.Add(new Node("ABORT", flowIdentifierSpecific1));
            List<ReferenceStep> specificReferenceSteps1 = new List<ReferenceStep>();
            specificReferenceSteps1.Add(new ReferenceStep(flowIdentifierBasic, 1));
            specificFlows.Add(new Flow(flowIdentifierSpecific1, null, specificSteps1, specificReferenceSteps1));
            allSteps.AddRange(specificSteps1);

            List<Flow> globalFlows = new List<Flow>();
            List<Flow> boundedFlows = new List<Flow>();

            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(allSteps.Count, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[4, 5] = true;

            Matrix<Condition?> expectedConditionMatrix = new Matrix<Condition?>(allSteps.Count, null);
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);

            List<Node> steps;
            Matrix<bool> edgeMatrix;
            Matrix<Condition?> conditionMatrix;

            // Act
            GraphBuilder.BuildGraph(ref basicFlow, specificFlows, globalFlows, boundedFlows, out steps, out edgeMatrix, out conditionMatrix);

            // Assert
            Assert.AreEqual(allSteps, steps);
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, conditionMatrix);
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
            List<Tuple<int, Condition?>> expectedExitSteps = new List<Tuple<int, Condition?>>();
            expectedExitSteps.Add(new Tuple<int, Condition?>(2, null));

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<Tuple<int, Condition?>> exitSteps;
            Matrix<Condition?> conditonMatrix;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditonMatrix);

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
            List<Tuple<int, Condition?>> expectedExitSteps = new List<Tuple<int, Condition?>>();

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<Tuple<int, Condition?>> exitSteps;
            Matrix<Condition?> conditonMatrix;

            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditonMatrix);

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
            nodes.Add(new Node("RESUME STEP 12 ", flowIdentifierBasic));
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
            List<Tuple<int, Condition?>> expectedExitSteps = new List<Tuple<int, Condition?>>();

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<Tuple<int, Condition?>> exitSteps;
            Matrix<Condition?> conditonMatrix;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditonMatrix);

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
            List<Tuple<int, Condition?>> expectedExitSteps = new List<Tuple<int, Condition?>>();
            expectedExitSteps.Add(new Tuple<int, Condition?>(3, null));

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<Tuple<int, Condition?>> exitSteps;
            Matrix<Condition?> conditonMatrix;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditonMatrix);

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
            List<Tuple<int, Condition?>> expectedExitSteps = new List<Tuple<int, Condition?>>();
            expectedExitSteps.Add(new Tuple<int, Condition?>(3, null));

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<Tuple<int, Condition?>> exitSteps;
            Matrix<Condition?> conditonMatrix;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditonMatrix);

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
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(6, false);
            expectedEdgeMatrix[0,1] = true;
            expectedEdgeMatrix[1,2] = true;
            expectedEdgeMatrix[1,4] = true;
            expectedEdgeMatrix[2,3] = true;
            expectedEdgeMatrix[2,4] = true;
            expectedEdgeMatrix[3,4] = true;
            expectedEdgeMatrix[4,5] = true;
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            List<Tuple<int, Condition?>> expectedExitSteps = new List<Tuple<int, Condition?>>();
            expectedExitSteps.Add(new Tuple<int, Condition?>(5, null));

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<Tuple<int, Condition?>> exitSteps;
            Matrix<Condition?> conditonMatrix;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditonMatrix);

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
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(11, false);
            expectedEdgeMatrix[0,1] = true;
            expectedEdgeMatrix[1,2] = true;
            expectedEdgeMatrix[1,3] = true;
            expectedEdgeMatrix[2,9] = true;
            expectedEdgeMatrix[3,4] = true;
            expectedEdgeMatrix[3,5] = true;
            expectedEdgeMatrix[4,9] = true;
            expectedEdgeMatrix[5,6] = true;
            expectedEdgeMatrix[5,7] = true;
            expectedEdgeMatrix[6,9] = true;
            expectedEdgeMatrix[7,8] = true;
            expectedEdgeMatrix[8,9] = true;
            expectedEdgeMatrix[9,10] = true;
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            List<Tuple<int, Condition?>> expectedExitSteps = new List<Tuple<int, Condition?>>();
            expectedExitSteps.Add(new Tuple<int, Condition?>(10, null));

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<Tuple<int, Condition?>> exitSteps;
            Matrix<Condition?> conditonMatrix;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditonMatrix);

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
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(11, false);
            expectedEdgeMatrix[0,1] = true;
            expectedEdgeMatrix[1,2] = true;
            expectedEdgeMatrix[1,5] = true;
            expectedEdgeMatrix[2,3] = true;
            expectedEdgeMatrix[2,4] = true;
            expectedEdgeMatrix[3,4] = true;
            expectedEdgeMatrix[4,9] = true;
            expectedEdgeMatrix[5,6] = true;
            expectedEdgeMatrix[5,7] = true;
            expectedEdgeMatrix[7,8] = true;
            expectedEdgeMatrix[9,10] = true;
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            expectedExternalEdges.Add(new ExternalEdge(6, new ReferenceStep(new FlowIdentifier(FlowType.Basic, 0), 4)));
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            expectedPossibleInvalidIfEdges.Add(new InternalEdge(2, 4));
            List<Tuple<int, Condition?>> expectedExitSteps = new List<Tuple<int, Condition?>>();
            expectedExitSteps.Add(new Tuple<int, Condition?>(10, null));

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<Tuple<int, Condition?>> exitSteps;
            Matrix<Condition?> conditonMatrix;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditonMatrix);

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
            List<Tuple<int, Condition?>> expectedExitSteps = new List<Tuple<int, Condition?>>();
            expectedExitSteps.Add(new Tuple<int, Condition?>(2, null));

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<Tuple<int, Condition?>> exitSteps;
            Matrix<Condition?> conditonMatrix;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditonMatrix);

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
            expectedEdgeMatrix[0,2] = true;
            expectedEdgeMatrix[1, 4] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[2,4] = true;
            expectedEdgeMatrix[3, 4] = true;
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            List<Tuple<int, Condition?>> expectedExitSteps = new List<Tuple<int, Condition?>>();
            expectedExitSteps.Add(new Tuple<int, Condition?>(4, null));

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<Tuple<int, Condition?>> exitSteps;
            Matrix<Condition?> conditonMatrix;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditonMatrix);

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
        public void TryWirePossibleEndlessLoopInWrongIfFlowDescription()
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
                            List<Tuple<int, Condition?>> exitSteps;
                            Matrix<Condition?> conditonMatrix;
                            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditonMatrix);
                        });
        }

        /// <summary>
        /// Tests if an exception if thrown if a description is given that can create a loop.
        /// </summary>
        [Test]
        public void TryWirePossibleEndlessLoopInWrongIfFlowDescription2()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("ELSEIF the crystal is blue THEN", flowIdentifierBasic));
            nodes.Add(new Node("ELSE the crystal is red THEN", flowIdentifierBasic));

            // Act

            // Assert
            Assert.Catch(
                        typeof(Exception),
                        () =>
                        {
                            Matrix<bool> edgeMatrix;
                            List<ExternalEdge> externalEdges;
                            List<InternalEdge> possibleInvalidIfEdges;
                            List<Tuple<int, Condition?>> exitSteps;
                            Matrix<Condition?> conditonMatrix;
                            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditonMatrix);
                        });
        }

        /// <summary>
        /// Tests if an exception if thrown if a description is given that can create a loop.
        /// </summary>
        [Test]
        public void TryWirePossibleEndlessLoopInWrongDoUntilFlowDescription()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("DO", flowIdentifierBasic));

            // Act

            // Assert
            Assert.Catch(
                        typeof(Exception),
                        () =>
                        {
                            Matrix<bool> edgeMatrix;
                            List<ExternalEdge> externalEdges;
                            List<InternalEdge> possibleInvalidIfEdges;
                            List<Tuple<int, Condition?>> exitSteps;
                            Matrix<Condition?> conditonMatrix;
                            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditonMatrix);
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
            nodes.Add(new Node("ELSEIF the crystal is green THEN", flowIdentifierBasic));   // 0
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
            expectedEdgeMatrix[0,6] = true;
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
            List<Tuple<int, Condition?>> expectedExitSteps = new List<Tuple<int, Condition?>>();
            expectedExitSteps.Add(new Tuple<int, Condition?>(8, null));

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<Tuple<int, Condition?>> exitSteps;
            Matrix<Condition?> conditionMatrix;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditionMatrix);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        /// <summary>
        /// Tests the edge creation of a simple do-until statement with a simple nested block.
        /// </summary>
        [Test]
        public void WireSimpleDoUntilFlow()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("DO", flowIdentifierBasic));                         // 0
                nodes.Add(new Node("Stamp on the ground", flowIdentifierBasic));    // 1
                nodes.Add(new Node("Jump in the air", flowIdentifierBasic));        // 2
            nodes.Add(new Node("UNTIL you are tired", flowIdentifierBasic));        // 3
            Matrix<bool> expectedEdgeMatrix = new Matrix<bool>(4, false);
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[3, 0] = true;
            List<ExternalEdge> expectedExternalEdges = new List<ExternalEdge>();
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            List<Tuple<int, Condition?>> expectedExitSteps = new List<Tuple<int, Condition?>>();
            expectedExitSteps.Add(new Tuple<int, Condition?>(3, new Condition(nodes[3].StepDescription, true)));

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<Tuple<int, Condition?>> exitSteps;
            Matrix<Condition?> conditonMatrix;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditonMatrix);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        /// <summary>
        /// Tests the edge creation of a simple do-until statement with a complex nested block.
        /// </summary>
        [Test]
        public void WireComplexDoUntilFlow()
        {
            // Arrange
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node("DO", flowIdentifierBasic));                         // 0
                nodes.Add(new Node("DO", flowIdentifierBasic));                     // 1
                    nodes.Add(new Node("Ask pin", flowIdentifierBasic));            // 2
                nodes.Add(new Node("UNTIL pin is true", flowIdentifierBasic));      // 3
                nodes.Add(new Node("IF wants money THEN", flowIdentifierBasic));    // 4
                    nodes.Add(new Node("Give money", flowIdentifierBasic));         // 5
                nodes.Add(new Node("ENDIF", flowIdentifierBasic));                  // 6
                nodes.Add(new Node("Return card", flowIdentifierBasic));            // 7
            nodes.Add(new Node("UNTIL power on", flowIdentifierBasic));             // 8
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
            List<Tuple<int, Condition?>> expectedExitSteps = new List<Tuple<int, Condition?>>();
            expectedExitSteps.Add(new Tuple<int, Condition?>(8, new Condition(nodes[8].StepDescription, true)));

            // Act
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<Tuple<int, Condition?>> exitSteps;
            Matrix<Condition?> conditonMatrix;
            GraphBuilder.SetEdgesInStepBlock(nodes, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditonMatrix);

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
        /// Tests if the correct type for an until statement is returned.
        /// </summary>
        [Test]
        public void GetUntilType()
        {
            // Arrange
            string stepDescription = "UNTIL the light is read";
            StepType expectedStepType = StepType.Until;

            // Act

            // Assert
            Assert.AreEqual(expectedStepType, GraphBuilder.GetStepType(stepDescription));
        }

        /// <summary>
        /// Tests if the correct type for an resume statement is returned.
        /// </summary>
        [Test]
        public void GetResumeType1()
        {
            // Arrange
            string stepDescription = "RESUME STEP 13";
            StepType expectedStepType = StepType.Resume;

            // Act

            // Assert
            Assert.AreEqual(expectedStepType, GraphBuilder.GetStepType(stepDescription));
        }

        /// <summary>
        /// Tests if the correct type for an resume statement is returned.
        /// </summary>
        [Test]
        public void GetResumeType2()
        {
            // Arrange
            string stepDescription = "RESUME 13";
            StepType expectedStepType = StepType.Resume;

            // Act

            // Assert
            Assert.AreEqual(expectedStepType, GraphBuilder.GetStepType(stepDescription));
        }

        /// <summary>
        /// Tests if the correct value for an resume statement is returned.
        /// </summary>
        [Test]
        public void GetResumeValue1()
        {
            // Arrange
            string stepDescription = "RESUME STEP 4";
            ExternalEdge expectedExternalEdge = new ExternalEdge(0, new ReferenceStep(flowIdentifierBasic, 3));

            // Act
            ExternalEdge externalEdge = GraphBuilder.GetExternalEdgeForResumeStep(stepDescription, 0);

            // Assert
            Assert.AreEqual(expectedExternalEdge, externalEdge); ;
        }

        /// <summary>
        /// Tests if the correct value for an resume statement is returned.
        /// </summary>
        [Test]
        public void GetResumeValue2()
        {
            // Arrange
            string stepDescription = "RESUME 19";
            ExternalEdge expectedExternalEdge = new ExternalEdge(0, new ReferenceStep(flowIdentifierBasic, 18));

            // Act
            ExternalEdge externalEdge = GraphBuilder.GetExternalEdgeForResumeStep(stepDescription, 0);

            // Assert
            Assert.AreEqual(expectedExternalEdge, externalEdge);
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
        /// Tests an until step against the until step type pattern.
        /// </summary>
        [Test]
        public void StepEqualToUntilPattern()
        {
            // Arrange
            string stepDescription = "UNTIL Will is alive";
            StepType stepType = StepType.Until;

            // Act

            // Assert
            Assert.IsTrue(GraphBuilder.IsEqualsToAtLeastOnePatternOfStepType(stepDescription, stepType));
        }

        /// <summary>
        /// Tests a non until step against the until step type pattern.
        /// </summary>
        [Test]
        public void StepNotEqualToUntilPattern()
        {
            // Arrange
            string stepDescription = "ELSEIF Karl is happy THEN Hans is happy";
            StepType stepType = StepType.Until;

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
