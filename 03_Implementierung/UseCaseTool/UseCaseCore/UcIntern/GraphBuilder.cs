// <copyright file="GraphBuilder.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
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
        /// <param name="nodes">The nodes of all flows.</param>
        /// <param name="edgeMatrix">The edge matrix for the nodes.</param>
        /// <param name="conditionMatrix">The condition matrix for the flows.</param>
        public static void BuildGraph(
            Flow basicFlow,
            IReadOnlyList<Flow> specificAlternativeFlows,
            IReadOnlyList<Flow> globalAlternativeFlows,
            IReadOnlyList<Flow> boundedAlternativeFlows,
            out IReadOnlyList<Node> nodes,
            out Matrix<bool> edgeMatrix,
            out Matrix<Condition?> conditionMatrix)
        {
            nodes = null;
            edgeMatrix = null;
            conditionMatrix = null;
        }

        /// <summary>
        /// Sets the edges in a block of nodes.
        /// </summary>
        /// <param name="nodes">The nodes to wire.</param>
        /// <param name="edgeMatrix">The matrix with the edges for the given nodes.</param>
        /// <param name="externalEdges">A list of edges whose target is located outside the given nodes.</param>
        /// <param name="possibleInvalidIfEdges">A list of edges between the given nodes that may be invalid, because they represent an edge for an if statement without an else statement in this node list for the case the condition is false. These edge may be invalid if the else/elseif is located in another block of nodes/alternative flow.</param>
        /// <param name="exitSteps">A list of steps of the given block that whose edges lead out of the block to the next step of the outer block. These are not abort edges!</param>
        public static void SetEdgesInNodeBlock(
            IReadOnlyList<Node> nodes,
            out Matrix<bool> edgeMatrix,
            out List<ExternalEdge> externalEdges,
            out List<InternalEdge> possibleInvalidIfEdges,
            out List<int> exitSteps)
        {
            // Initialize out varibles
            edgeMatrix = new Matrix<bool>(nodes.Count, false);
            externalEdges = new List<ExternalEdge>();
            possibleInvalidIfEdges = new List<InternalEdge>();
            exitSteps = new List<int>();

            // Cycle through all steps and handle their edges.
            for (int stepPos = 0; stepPos < nodes.Count; stepPos++)
            {
                string stepDescription = nodes[stepPos].StepDescription;
                StepType stepType = GraphBuilder.GetStepType(stepDescription);

                if (stepType.Equals(StepType.If))
                {
                }
                else if (stepType.Equals(StepType.Do))
                {
                }
                else if (stepType.Equals(StepType.Resume))
                {
                    externalEdges.Add(GraphBuilder.GetExternalEdgeForResumeStep(stepDescription, stepPos));
                    break;
                }
                else if (stepType.Equals(StepType.Abort))
                {
                    break;
                }
                else
                {
                    // Treat it as a normal/unmatched step

                    if (stepPos < (nodes.Count - 1))
                    {
                        // Not the last step.
                        edgeMatrix[stepPos, stepPos + 1] = true;
                    }
                    else
                    {
                        // The last step.
                        exitSteps.Add(stepPos);
                    }
                }
            }
        }

        /// <summary>
        /// Analyzes the step and returns the external edge for that resume step.
        /// </summary>
        /// <param name="stepDescription">The description of a resume step.</param>
        /// <param name="currentStep">The step number of the given resume step.</param>
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
