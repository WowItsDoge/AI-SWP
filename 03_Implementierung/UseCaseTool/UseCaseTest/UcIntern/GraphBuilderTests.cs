// <copyright file="GraphBuilderTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
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
        private FlowIdentifier flowIdentifierBasic;

        /// <summary>
        /// Setup method for private variables.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            flowIdentifierBasic = new FlowIdentifier(FlowType.Basic, 0);
        }

        // --------------------------------------------------------------------------------------------- SetEdgesInNodeBlock

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
            GraphBuilder.SetEdgesInStepBlock(nodes, out var edgeMatrix, out var externalEdges, out var possibleInvalidIfEdges, out var exitSteps);

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
            GraphBuilder.SetEdgesInStepBlock(nodes, out var edgeMatrix, out var externalEdges, out var possibleInvalidIfEdges, out var exitSteps);

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
            expectedExternalEdges.Add(new ExternalEdge(1, new ReferenceStep(new FlowIdentifier(FlowType.Basic, 0), 12)));
            List<InternalEdge> expectedPossibleInvalidIfEdges = new List<InternalEdge>();
            List<int> expectedExitSteps = new List<int>();

            // Act
            GraphBuilder.SetEdgesInStepBlock(nodes, out var edgeMatrix, out var externalEdges, out var possibleInvalidIfEdges, out var exitSteps);

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
            GraphBuilder.SetEdgesInStepBlock(nodes, out var edgeMatrix, out var externalEdges, out var possibleInvalidIfEdges, out var exitSteps);

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
            GraphBuilder.SetEdgesInStepBlock(nodes, out var edgeMatrix, out var externalEdges, out var possibleInvalidIfEdges, out var exitSteps);

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
            GraphBuilder.SetEdgesInStepBlock(nodes, out var edgeMatrix, out var externalEdges, out var possibleInvalidIfEdges, out var exitSteps);

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
            GraphBuilder.SetEdgesInStepBlock(nodes, out var edgeMatrix, out var externalEdges, out var possibleInvalidIfEdges, out var exitSteps);

            this.PrintMatrix(edgeMatrix);

            // Assert
            Assert.AreEqual(expectedEdgeMatrix, edgeMatrix);
            Assert.AreEqual(expectedExternalEdges, externalEdges);
            Assert.AreEqual(expectedPossibleInvalidIfEdges, possibleInvalidIfEdges);
            Assert.AreEqual(expectedExitSteps, exitSteps);
        }

        /// <summary>
        /// Tests the edge creation of an if flow like if-elseif-else-endif with blocks consising of Resume, Abort and other if statements inside the if statement.
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
            nodes.Add(new Node("RESUME 4", flowIdentifierBasic));                                   // 6
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
            GraphBuilder.SetEdgesInStepBlock(nodes, out var edgeMatrix, out var externalEdges, out var possibleInvalidIfEdges, out var exitSteps);

            this.PrintMatrix(edgeMatrix);

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
