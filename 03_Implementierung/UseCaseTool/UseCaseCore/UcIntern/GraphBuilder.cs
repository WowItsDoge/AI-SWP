// <copyright file="GraphBuilder.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using RuleValidation;

    /// <summary>
    /// This class transforms the use case description consisting of the different flow types into a graph representation.
    /// It is assumed that the use case description is validated by a <see cref="IRucmRuleValidator"/>.
    /// </summary>
    public static class GraphBuilder
    {
        /// <summary>
        /// Builds the graph.
        /// </summary>
        /// <param name="basicFlow">The basic flow.</param>
        /// <param name="specificAlternativeFlows">The specific alternative flows.</param>
        /// <param name="globalAlternativeFlows">The global alternative flows.</param>
        /// <param name="boundedAlternativeFlows">The bounded alternative flows.</param>
        /// <param name="steps">The steps of all flows.</param>
        /// <param name="edgeMatrix">The edge matrix for the steps.</param>
        /// <param name="conditionMatrix">The condition matrix for the flows.</param>
        public static void BuildGraph(
            Flow basicFlow,
            IReadOnlyList<Flow> specificAlternativeFlows,
            IReadOnlyList<Flow> globalAlternativeFlows,
            IReadOnlyList<Flow> boundedAlternativeFlows,
            out IReadOnlyList<Node> steps,
            out Matrix<bool> edgeMatrix,
            out Matrix<Condition?> conditionMatrix)
        {
            steps = null;
            edgeMatrix = null;
            conditionMatrix = null;
        }

        /// <summary>
        /// Sets the edges in a block of steps.
        /// </summary>
        /// <param name="steps">The steps to wire.</param>
        /// <param name="edgeMatrix">The matrix with the edges for the given steps.</param>
        /// <param name="externalEdges">A list of edges whose target is located outside the given steps.</param>
        /// <param name="possibleInvalidIfEdges">A list of edges between the given steps that may be invalid, because they represent an edge for an if statement without an else statement in this step list for the case the condition is false. These edge may be invalid if the else/elseif is located in another block of steps/alternative flow.</param>
        /// <param name="exitSteps">A list of steps of the given block that whose edges lead out of the block to the next step of the outer block. These are not abort edges!</param>
        public static void SetEdgesInStepBlock(
            IReadOnlyList<Node> steps,
            out Matrix<bool> edgeMatrix,
            out List<ExternalEdge> externalEdges,
            out List<InternalEdge> possibleInvalidIfEdges,
            out List<int> exitSteps)
        {
            // Initialize out varibles
            edgeMatrix = new Matrix<bool>(steps.Count, false);
            externalEdges = new List<ExternalEdge>();
            possibleInvalidIfEdges = new List<InternalEdge>();
            exitSteps = new List<int>();

            // Cycle through all steps and handle their edges.
            int lastStepIndex = -1;
            for (int stepIndex = 0; stepIndex < steps.Count; stepIndex++)
            {
                if (lastStepIndex >= stepIndex)
                {
                    throw new Exception("Invalid graph description!");
                }

                lastStepIndex = stepIndex;

                string stepDescription = steps[stepIndex].StepDescription;
                StepType stepType = GraphBuilder.GetStepType(stepDescription);

                if (stepType == StepType.If)
                {
                    GraphBuilder.SetEdgesInIfStatement(steps, ref edgeMatrix, ref externalEdges, ref possibleInvalidIfEdges, ref stepIndex);

                    // Revert the step index by one to one position before the end if so that in the next cycle it points to the end if step.
                    stepIndex--;
                }
                else if (stepType == StepType.Else || stepType == StepType.ElseIf)
                {
                    GraphBuilder.SetEdgesInElseElseIfStatement(steps, ref edgeMatrix, ref externalEdges, ref possibleInvalidIfEdges, ref stepIndex);

                    // Revert the step index by one to one position beofre the end if so that in the next cycle it points to the end if step.
                    stepIndex--;
                }
                else if (stepType == StepType.Do)
                {
                    GraphBuilder.SetEdgesInDoWhileStatement(steps, ref edgeMatrix, ref externalEdges, ref possibleInvalidIfEdges, ref stepIndex);

                    // Revert the step index by one to one position beofre the while so that in the next cycle it points to the while step.
                    stepIndex--;
                }
                else if (stepType == StepType.Resume)
                {
                    externalEdges.Add(GraphBuilder.GetExternalEdgeForResumeStep(stepDescription, stepIndex));
                    break;
                }
                else if (stepType == StepType.Abort)
                {
                    break;
                }
                else
                {
                    // Treat it as a normal/unmatched step
                    if (stepIndex < (steps.Count - 1))
                    {
                        // Not the last step.
                        edgeMatrix[stepIndex, stepIndex + 1] = true;
                    }
                    else
                    {
                        // The last step.
                        exitSteps.Add(stepIndex);
                    }
                }
            }
        }

        /// <summary>
        /// Wires an if block. The if block starts at <paramref name="stepIndex"/> in steps. The complete wiring is made and <paramref name="stepIndex"/> finally is set to the index of the end if step.
        /// The given lists/matrices are correctly updated.
        /// <para/>
        /// To visualize what is assumed a block if talking about it:
        /// <para/>
        /// IF
        /// <para/>
        /// Nested block step
        /// <para/>
        /// ELSE
        /// </summary>
        /// <param name="steps">See <paramref name="steps"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="edgeMatrix">See <paramref name="edgeMatrix"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="externalEdges">See <paramref name="externalEdges"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="possibleInvalidIfEdges">See <paramref name="possibleInvalidIfEdges"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="stepIndex">On call the current index in the steps where the if start step is located and after the call the index of the end if step.</param>
        public static void SetEdgesInIfStatement(
            IReadOnlyList<Node> steps,
            ref Matrix<bool> edgeMatrix,
            ref List<ExternalEdge> externalEdges,
            ref List<InternalEdge> possibleInvalidIfEdges,
            ref int stepIndex)
        {
            List<int> importantIfSteps = GraphBuilder.GetImportantIfStatementSteps(steps, stepIndex);

            // Handle nested blocks
            int ifStepIndex = importantIfSteps.First(),
                endIfStepIndex = importantIfSteps.Last();

            for (int blockIndex = 0; blockIndex < importantIfSteps.Count - 1; blockIndex++)
            {
                int blockStartIndex = importantIfSteps[blockIndex],
                    blockEndIndex = importantIfSteps[blockIndex + 1],
                    blockSize = blockEndIndex - blockStartIndex - 1;

                // Set edge from if to start of block
                if (blockIndex > 0)
                {
                    edgeMatrix[ifStepIndex, blockStartIndex] = true;
                }

                if (blockSize > 0)
                {
                    GraphBuilder.SetEdgesInNestedBlock(steps, ref edgeMatrix, ref externalEdges, ref possibleInvalidIfEdges, blockStartIndex, blockEndIndex, endIfStepIndex);
                }
                else
                {
                    // Wire block start step to endif step
                    edgeMatrix[blockStartIndex, endIfStepIndex] = true;
                }
            }

            // If there are no else if or else blocks it might be that they are located in alternative flows or that there are none.
            // To be safe assume that there are none, create the edge and list that edge as possible invalid.
            if (importantIfSteps.Count <= 2)
            {
                edgeMatrix[ifStepIndex, endIfStepIndex] = true;

                possibleInvalidIfEdges.Add(new InternalEdge(ifStepIndex, endIfStepIndex));
            }

            // Set current step index to end if step.
            stepIndex = endIfStepIndex;
        }

        /// <summary>
        /// Wires an else/else if block. The block starts at <paramref name="stepIndex"/> in steps. The complete wiring is made and <paramref name="stepIndex"/> finally is set to the index of the end if step.
        /// The given lists/matrices are correctly updated.
        /// </summary>
        /// <param name="steps">See <paramref name="steps"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="edgeMatrix">See <paramref name="edgeMatrix"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="externalEdges">See <paramref name="externalEdges"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="possibleInvalidIfEdges">See <paramref name="possibleInvalidIfEdges"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="stepIndex">On call the current index in the steps where the else/else if start step is located and after the call the index of the end if step.</param>
        public static void SetEdgesInElseElseIfStatement(
            IReadOnlyList<Node> steps,
            ref Matrix<bool> edgeMatrix,
            ref List<ExternalEdge> externalEdges,
            ref List<InternalEdge> possibleInvalidIfEdges,
            ref int stepIndex)
        {
            List<int> importantIfSteps = GraphBuilder.GetImportantIfStatementSteps(steps, stepIndex);

            // Handle nested blocks
            int endIfStepIndex = importantIfSteps.Last();

            for (int blockIndex = 0; blockIndex < importantIfSteps.Count - 1; blockIndex++)
            {
                int blockStartIndex = importantIfSteps[blockIndex],
                    blockEndIndex = importantIfSteps[blockIndex + 1],
                    blockSize = blockEndIndex - blockStartIndex - 1;

                if (blockSize > 0)
                {
                    GraphBuilder.SetEdgesInNestedBlock(steps, ref edgeMatrix, ref externalEdges, ref possibleInvalidIfEdges, blockStartIndex, blockEndIndex, endIfStepIndex);
                }
                else
                {
                    // Wire block start step to endif step
                    edgeMatrix[blockStartIndex, endIfStepIndex] = true;
                }
            }

            // Set current step index to end if step.
            stepIndex = endIfStepIndex;
        }

        /// <summary>
        /// Wires an do-while block. The block starts at <paramref name="stepIndex"/> in steps. The complete wiring is made and <paramref name="stepIndex"/> finally is set to
        /// the index of the while step.
        /// The given lists/matrices are correctly updated.
        /// </summary>
        /// <param name="steps">See <paramref name="steps"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="edgeMatrix">See <paramref name="edgeMatrix"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="externalEdges">See <paramref name="externalEdges"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="possibleInvalidIfEdges">See <paramref name="possibleInvalidIfEdges"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="stepIndex">On call the current index in the steps where the do start step is located and after the call the index of the while step.</param>
        public static void SetEdgesInDoWhileStatement(
            IReadOnlyList<Node> steps,
            ref Matrix<bool> edgeMatrix,
            ref List<ExternalEdge> externalEdges,
            ref List<InternalEdge> possibleInvalidIfEdges,
            ref int stepIndex)
        {
            Tuple<int, int> importantDoWhileSteps = GraphBuilder.GetImportantDoWhileStatementSteps(steps, stepIndex);

            // Handle nested block
            int doStepIndex = importantDoWhileSteps.Item1,
                whileStepIndex = importantDoWhileSteps.Item2,
                blockSize = whileStepIndex - doStepIndex - 1;

            if (blockSize > 0)
            {
                GraphBuilder.SetEdgesInNestedBlock(steps, ref edgeMatrix, ref externalEdges, ref possibleInvalidIfEdges, doStepIndex, whileStepIndex, whileStepIndex);
            }
            else
            {
                // Wire do step to while step
                edgeMatrix[doStepIndex, whileStepIndex] = true;
            }

            // Set edge from while to do
            edgeMatrix[whileStepIndex, doStepIndex] = true;

            // Set current step index to end if step.
            stepIndex = whileStepIndex;
        }

        /// <summary>
        /// Handles a nested block. A nested block is explained with an if statement as example.
        /// The given lists/matrices are correctly updated.
        /// <para/>
        /// Consider the following block. Only the first block is shown. The texts in [] describe the role of the important steps for that statement.
        /// <para/>
        /// IF George can fly THEN [block start]
        /// <para/>
        /// Go fly George. [nested block step]
        /// <para/>
        /// And don't come back. [nested block step]
        /// <para/>
        /// ELSE [block end]
        /// <para/>
        /// You're trapped to this world. :(
        /// <para/>
        /// ENDIF [exit target step]
        /// </summary>
        /// <param name="steps">The steps containing the block.</param>
        /// <param name="edgeMatrix">The edge matrix.</param>
        /// <param name="externalEdges">The external edges list.</param>
        /// <param name="possibleInvalidIfEdges">The possible invalid if edge list.</param>
        /// <param name="blockStartIndex">The index of the block start. (The step before the nested block begins)</param>
        /// <param name="blockEndIndex">The index of the block end. (The step after the nested block ends)</param>
        /// <param name="exitStepsTargetStep">The step to where create the edges if the nested block has exit steps.</param>
        public static void SetEdgesInNestedBlock(
            IReadOnlyList<Node> steps,
            ref Matrix<bool> edgeMatrix,
            ref List<ExternalEdge> externalEdges,
            ref List<InternalEdge> possibleInvalidIfEdges,
            int blockStartIndex,
            int blockEndIndex,
            int exitStepsTargetStep)
        {
            int blockSize = blockEndIndex - blockStartIndex - 1;

            // Set edge into the nested block
            edgeMatrix[blockStartIndex, blockStartIndex + 1] = true;

            List<Node> nestedSteps = steps.Skip(blockStartIndex + 1).Take(blockSize).ToList();

            GraphBuilder.SetEdgesInStepBlock(nestedSteps, out var nestedEdgeMatrix, out var nestedExternalEdges, out var nestedPossibleInvalidIfEdges, out var nestedExitSteps);

            // Unite matrices
            GraphBuilder.InsertMatrix(ref edgeMatrix, blockStartIndex + 1, blockStartIndex + 1, nestedEdgeMatrix);

            // Unite externalEdges
            externalEdges.AddRange(nestedExternalEdges.ConvertAll((edge) => edge.NewWithIncreasedSourceStepNumber(blockStartIndex + 1)));

            // Unite possible invalid if edges
            possibleInvalidIfEdges.AddRange(nestedPossibleInvalidIfEdges.ConvertAll((edge) => edge.NewWithIncreasedSourceTargetStep(blockStartIndex + 1)));

            // Wire exit steps to end if step
            foreach (int exitStep in nestedExitSteps)
            {
                edgeMatrix[exitStep + blockStartIndex + 1, exitStepsTargetStep] = true;
            }
        }

        /// <summary>
        /// Inserts <paramref name="sourceMatrix"/> into <paramref name="targetMatrix"/>. The values in <paramref name="targetMatrix"/> are overriden. Make sure the dimensions match!
        /// <paramref name="sourceMatrix"/> is completely inserted into <paramref name="targetMatrix"/>.
        /// </summary>
        /// <typeparam name="T">The matrix content type.</typeparam>
        /// <param name="targetMatrix">The target matrix where to insert <paramref name="sourceMatrix"/>.</param>
        /// <param name="targetRow">The row in <paramref name="targetMatrix"/> where to start insterting.</param>
        /// <param name="targetColumn">The column in <paramref name="targetMatrix"/> where to start inserting.</param>
        /// <param name="sourceMatrix">The source matrix to insert into <paramref name="targetMatrix"/>.</param>
        public static void InsertMatrix<T>(ref Matrix<T> targetMatrix, int targetRow, int targetColumn, Matrix<T> sourceMatrix)
        {
            for (int sourceRow = 0; sourceRow < sourceMatrix.RowCount; sourceRow++)
            {
                for (int sourceColumn = 0; sourceColumn < sourceMatrix.ColumnCount; sourceColumn++)
                {
                    targetMatrix[targetRow + sourceRow, targetColumn + sourceColumn] = sourceMatrix[sourceRow, sourceColumn];
                }
            }
        }

        /// <summary>
        /// Searches the important steps in an if statement starting in <paramref name="steps"/> at index <paramref name="startStep"/>.
        /// The important steps are if, else if, else and end if. The given <paramref name="startStep"/> is assumed a valid if statment step and added automatically to the start of the list.
        /// It ends with the first end if that does not belong to a nested if statement.
        /// </summary>
        /// <param name="steps">The steps containing the if statement.</param>
        /// <param name="startStep">The index of the step in <paramref name="steps"/> where to start the search. Must be an if, elseif, else step!</param>
        /// <returns>The indices of the important steps. Index 0 is the <paramref name="startStep"/> and the following are elseif and else steps and an endif step as last index.</returns>
        public static List<int> GetImportantIfStatementSteps(IReadOnlyList<Node> steps, int startStep)
        {
            List<int> importantSteps = new List<int>();
            importantSteps.Add(startStep);

            // If this number is greater than 0 the index is currently in a nested if of the given depth.
            int numberNestedIfStatements = 0;

            for (int stepIndex = startStep + 1; stepIndex < steps.Count; stepIndex++)
            {
                StepType stepType = GraphBuilder.GetStepType(steps[stepIndex].StepDescription);

                if (stepType == StepType.If)
                {
                    numberNestedIfStatements++;
                    continue;
                }

                if (numberNestedIfStatements > 0)
                {
                    // In nested if statement.
                    if (stepType == StepType.EndIf)
                    {
                        numberNestedIfStatements--;
                    }
                }
                else
                {
                    // In root if statment.
                    if (stepType == StepType.ElseIf || stepType == StepType.Else || stepType == StepType.EndIf)
                    {
                        importantSteps.Add(stepIndex);
                    }

                    if (stepType == StepType.EndIf)
                    {
                        break;
                    }
                }
            }

            return importantSteps;
        }

        /// <summary>
        /// Searches the important steps in an do-while statement starting in <paramref name="steps"/> at index <paramref name="startStep"/>.
        /// The important steps are do and while. The given <paramref name="startStep"/> is assumed a valid do statment step and added automatically to the start of the list.
        /// It ends with the first while that does not belong to a nested do-while statement.
        /// </summary>
        /// <param name="steps">The steps containing the do-while statement.</param>
        /// <param name="startStep">The index of the step in <paramref name="steps"/> where to start the search. Must be a do step!</param>
        /// <returns>The indices of the important steps. Item 1 is the do step index and item 2 the while step index.</returns>
        public static Tuple<int, int> GetImportantDoWhileStatementSteps(IReadOnlyList<Node> steps, int startStep)
        {
            // If this number is greater than 0 the index is currently in a nested do-while of the given depth.
            int numberNestedDoWhileStatements = 0,
                endStep = -1;

            for (int stepIndex = startStep + 1; stepIndex < steps.Count; stepIndex++)
            {
                StepType stepType = GraphBuilder.GetStepType(steps[stepIndex].StepDescription);

                if (stepType == StepType.Do)
                {
                    numberNestedDoWhileStatements++;
                    continue;
                }

                if (numberNestedDoWhileStatements > 0)
                {
                    // In nested if statement.
                    if (stepType == StepType.While)
                    {
                        numberNestedDoWhileStatements--;
                    }
                }
                else
                {
                    // In root if statment.
                    if (stepType == StepType.While)
                    {
                        endStep = stepIndex;
                        break;
                    }
                }
            }

            return new Tuple<int, int>(startStep, endStep);
        }

        /// <summary>
        /// Analyzes the step and returns the external edge for that resume step.
        /// </summary>
        /// <param name="stepDescription">The description of a resume step.</param>
        /// <param name="resumeStepNumber">The step number of the given resume step.</param>
        /// <returns>The external edge for the given resume step.</returns>
        public static ExternalEdge GetExternalEdgeForResumeStep(string stepDescription, int resumeStepNumber)
        {
            Regex resumeRegex = new Regex(GraphBuilder.GetMatchingPatternForStepType(stepDescription, StepType.Resume));

            Match resumeMatch = resumeRegex.Match(stepDescription);

            int targetStepInBasicFlow = int.Parse(resumeMatch.Groups[0].Value);

            return new ExternalEdge(resumeStepNumber, new ReferenceStep(new FlowIdentifier(FlowType.Basic, 0), targetStepInBasicFlow));
        }

        /// <summary>
        /// Returns the type of a step.
        /// </summary>
        /// <param name="stepDescription">The description of the step.</param>
        /// <returns>The type of the step.</returns>
        public static StepType GetStepType(string stepDescription)
        {
            foreach (StepType stepType in StepType.All)
            {
                if (stepDescription.IsEqualsToAtLeastOnePatternOfStepType(stepType))
                {
                    return stepType;
                }
            }

            return StepType.Unmatched;
        }

        /// <summary>
        /// Tests if a string is equal to at least one keyword asociated with the specified step type.
        /// </summary>
        /// <param name="testString">The string to test.</param>
        /// <param name="stepType">The type specifying the patterns.</param>
        /// <returns>If at least one pattern for the step type matches to the test string.</returns>
        public static bool IsEqualsToAtLeastOnePatternOfStepType(this string testString, StepType stepType)
        {
            return GraphBuilder.GetMatchingPatternForStepType(testString, stepType) != null;
        }

        /// <summary>
        /// Returns the pattern that matches the given string from that step type.
        /// </summary>
        /// <param name="testString">The string to test.</param>
        /// <param name="stepType">The type specifying the patterns.</param>
        /// <returns>The pattern that first matches the <paramref name="testString"/> from the patterns of the step type; null if none matches.</returns>
        public static string GetMatchingPatternForStepType(this string testString, StepType stepType)
        {
            foreach (string pattern in stepType.Patterns)
            {
                Regex regex = new Regex(pattern);

                if (regex.IsMatch(testString))
                {
                    return pattern;
                }
            }

            return null;
        }
    }
}
