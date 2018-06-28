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
        /// <param name="basicFlow">The basic flow. It will be extended with a step, with an empty description, if the last step is a validates that or until step so that the condition does not get lost.</param>
        /// <param name="specificAlternativeFlowsUnnormalized">The specific alternative flows. Reference steps starting with 1.</param>
        /// <param name="globalAlternativeFlows">The global alternative flows.</param>
        /// <param name="boundedAlternativeFlowsUnnormalized">The bounded alternative flows. Reference steps starting with 1.</param>
        /// <param name="steps">The steps of all flows.</param>
        /// <param name="edgeMatrix">The edge matrix for the steps.</param>
        /// <param name="conditionMatrix">The condition matrix for the flows.</param>
        public static void BuildGraph(
            ref Flow basicFlow,
            IReadOnlyList<Flow> specificAlternativeFlowsUnnormalized,
            IReadOnlyList<Flow> globalAlternativeFlows,
            IReadOnlyList<Flow> boundedAlternativeFlowsUnnormalized,
            out List<Node> steps,
            out Matrix<bool> edgeMatrix,
            out Matrix<Condition?> conditionMatrix)
        {
            basicFlow = GraphBuilder.ExtendBasicFlowIfNecessary(basicFlow);

            IReadOnlyList<Flow> specificAlternativeFlows = GraphBuilder.NormalizeReferenceSteps(specificAlternativeFlowsUnnormalized);
            IReadOnlyList<Flow> boundedAlternativeFlows = GraphBuilder.NormalizeReferenceSteps(boundedAlternativeFlowsUnnormalized);

            steps = new List<Node>();
            List<Flow> allFlows = new List<Flow>();
            allFlows.Add(basicFlow);
            allFlows.AddRange(specificAlternativeFlows);
            allFlows.AddRange(globalAlternativeFlows);
            allFlows.AddRange(boundedAlternativeFlows);

            // Wire all flows individually. The tuples have as item 1 the offset for the steps list and edge matrix and then all out parameters of SetEdgesInStepBlock.
            List<Tuple<int, Flow, Matrix<bool>, List<ExternalEdge>, List<InternalEdge>, List<Tuple<int, Condition?>>, Matrix<Condition?>>> individuallyWiredFlows =
                GraphBuilder.WireFlowListIndividually(allFlows, 0);

            // Collect all steps in the steps list
            foreach (Flow flow in allFlows)
            {
                steps.AddRange(flow.Nodes);
            }

            // Copy each flows edge/condition matrix into the global one
            edgeMatrix = new Matrix<bool>(steps.Count, false);
            conditionMatrix = new Matrix<Condition?>(steps.Count, null);

            foreach (Tuple<int, Flow, Matrix<bool>, List<ExternalEdge>, List<InternalEdge>, List<Tuple<int, Condition?>>, Matrix<Condition?>> individuallyWiredFlow in individuallyWiredFlows)
            {
                GraphBuilder.InsertMatrix(ref edgeMatrix, individuallyWiredFlow.Item1, individuallyWiredFlow.Item1, individuallyWiredFlow.Item3);
                GraphBuilder.InsertMatrix(ref conditionMatrix, individuallyWiredFlow.Item1, individuallyWiredFlow.Item1, individuallyWiredFlow.Item7);
            }

            // Wire external edges.
            foreach (Tuple<int, Flow, Matrix<bool>, List<ExternalEdge>, List<InternalEdge>, List<Tuple<int, Condition?>>, Matrix<Condition?>> individuallyWiredFlow in individuallyWiredFlows)
            {
                List<ExternalEdge> externalEdges = individuallyWiredFlow.Item4;

                foreach (ExternalEdge externalEdge in externalEdges)
                {
                    // Get offset of target flow.
                    FlowIdentifier targetIdentifier = externalEdge.TargetStep.Identifier;

                    List<Tuple<int, Flow, Matrix<bool>, List<ExternalEdge>, List<InternalEdge>, List<Tuple<int, Condition?>>, Matrix<Condition?>>> targetFlow =
                        individuallyWiredFlows.Where((iwf) => iwf.Item2.Identifier.Equals(targetIdentifier)).ToList();

                    int targetFlowOffset = targetFlow[0].Item1,
                        targetStep = targetFlowOffset + externalEdge.TargetStep.Step,
                        sourceStep = individuallyWiredFlow.Item1 + externalEdge.SourceStepNumber;

                    edgeMatrix[sourceStep, targetStep] = true;
                }
            }

            // Wire alternative flows reference steps
            foreach (Tuple<int, Flow, Matrix<bool>, List<ExternalEdge>, List<InternalEdge>, List<Tuple<int, Condition?>>, Matrix<Condition?>> individuallyWiredFlow in individuallyWiredFlows)
            {
                Flow flow = individuallyWiredFlow.Item2;
                FlowType flowType = flow.Identifier.Type;
                int flowOffset = individuallyWiredFlow.Item1;

                switch (flowType)
                {
                    case FlowType.Basic:
                        // Does not have reference steps.
                        break;
                    case FlowType.SpecificAlternative:
                    case FlowType.BoundedAlternative:
                        // The flow can be entered only at the first step or at multiple ones if it is an outsourced elseif-else statement.
                        int targetStep = flowOffset;

                        foreach (ReferenceStep referenceStep in flow.ReferenceSteps)
                        {
                            FlowIdentifier sourceIdentifier = referenceStep.Identifier;

                            List<Tuple<int, Flow, Matrix<bool>, List<ExternalEdge>, List<InternalEdge>, List<Tuple<int, Condition?>>, Matrix<Condition?>>> sourceFlow =
                                individuallyWiredFlows.Where((iwf) => iwf.Item2.Identifier.Equals(sourceIdentifier)).ToList();

                            int sourceFlowOffset = sourceFlow[0].Item1;

                            int sourceStep = sourceFlowOffset + referenceStep.Step;

                            // Remove possible invalid if edge if present
                            List<InternalEdge> invalidIfEdges = sourceFlow[0].Item5.Where((piie) => piie.SourceStep == referenceStep.Step).ToList();
                            foreach (InternalEdge invalidIfEdge in invalidIfEdges)
                            {
                                int invalidTargetStep = sourceFlowOffset + invalidIfEdge.TargetStep;

                                // If the block is empty than the edges for fulfilled and not fulfilled condition are the same.
                                // In that case the invalid edge does not have a condition and thus its condition must be set to the fulfilled condition of the if/else if statement step.
                                if (conditionMatrix[sourceStep, invalidTargetStep] != null)
                                {
                                    conditionMatrix[sourceStep, invalidTargetStep] = null;
                                    edgeMatrix[sourceStep, invalidTargetStep] = false;
                                }
                                else
                                {
                                    conditionMatrix[sourceStep, invalidTargetStep] = new Condition(steps[sourceStep].StepDescription, true);
                                }
                            }

                            edgeMatrix[sourceStep, targetStep] = true;
                            conditionMatrix[sourceStep, targetStep] = new Condition(steps[sourceStep].StepDescription, false);
                        }

                        break;
                    case FlowType.GlobalAlternative:
                        // Gets an edge from every step of the basic flow.
                        // Basic flow is always at offset 0.
                        for (int sourceStep = 0; sourceStep < basicFlow.Nodes.Count; sourceStep++)
                        {
                            edgeMatrix[sourceStep, flowOffset] = true;
                            conditionMatrix[sourceStep, flowOffset] = new Condition(individuallyWiredFlow.Item2.Nodes[0].StepDescription, true);
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Extends the basic flow with a step at the end if it ends with a validates that or until step.
        /// This is because otherwise that last condition would get lost.
        /// The added step will have an empty string as description.
        /// </summary>
        /// <param name="basicFlow">The basic flow to check the last step of.</param>
        /// <returns>The extended basic flow like described above.</returns>
        public static Flow ExtendBasicFlowIfNecessary(Flow basicFlow)
        {
            StepType lastStepType = GraphBuilder.GetStepType(basicFlow.Nodes.LastOrDefault().StepDescription ?? string.Empty);

            if (lastStepType == StepType.Until || lastStepType == StepType.ValidatesThat)
            {
                List<Node> extendedNodes = new List<Node>();
                extendedNodes.AddRange(basicFlow.Nodes);
                extendedNodes.Add(new Node(string.Empty, basicFlow.Identifier));

                return new Flow(
                    basicFlow.Identifier,
                    basicFlow.Postcondition,
                    extendedNodes,
                    basicFlow.ReferenceSteps);
            }

            return basicFlow;
        }

        /// <summary>
        /// Normalizes the reference steps in all flows. In the description steps are numbered starting with 1. Within the builder for easier usage the numbering starts with 0.
        /// This method transforms all reference steps in the flows to match that condition.
        /// </summary>
        /// <param name="unnormalizedFlows">The flows to normalize.</param>
        /// <returns>A new list with the normalized flows.</returns>
        public static List<Flow> NormalizeReferenceSteps(IReadOnlyList<Flow> unnormalizedFlows)
        {
            List<Flow> normalizedFlows = new List<Flow>();

            foreach (Flow unnormalizedFlow in unnormalizedFlows)
            {
                IReadOnlyList<ReferenceStep> unnormalizedReferenceSteps = unnormalizedFlow.ReferenceSteps;
                List<ReferenceStep> normalizedReferenceSteps = new List<ReferenceStep>();

                foreach (ReferenceStep unnormalizedReferenceStep in unnormalizedReferenceSteps)
                {
                    normalizedReferenceSteps.Add(new ReferenceStep(unnormalizedReferenceStep.Identifier, unnormalizedReferenceStep.Step - 1));
                }

                normalizedFlows.Add(new Flow(unnormalizedFlow.Identifier, unnormalizedFlow.Postcondition, unnormalizedFlow.Nodes, normalizedReferenceSteps));
            }

            return normalizedFlows;
        }

        /// <summary>
        /// Calls <see cref="WireFlowIndividually(Flow, int)"/> on every flow in the list.
        /// </summary>
        /// <param name="flows">A list of flows to wire individually.</param>
        /// <param name="firstFlowOffset">The offset the first flow will later have in the edge matrix. It is set as item 1 for the first flow of the returned list. For the next flow it is increased by the number of steps in the first flow and so on.</param>
        /// <returns>A list of tuples with the information given by <see cref="WireFlowIndividually(Flow, int)"/>.</returns>
        public static List<Tuple<int, Flow, Matrix<bool>, List<ExternalEdge>, List<InternalEdge>, List<Tuple<int, Condition?>>, Matrix<Condition?>>> WireFlowListIndividually(
            IReadOnlyList<Flow> flows,
            int firstFlowOffset)
        {
            List<Tuple<int, Flow, Matrix<bool>, List<ExternalEdge>, List<InternalEdge>, List<Tuple<int, Condition?>>, Matrix<Condition?>>> individuallyWiredFlows =
                new List<Tuple<int, Flow, Matrix<bool>, List<ExternalEdge>, List<InternalEdge>, List<Tuple<int, Condition?>>, Matrix<Condition?>>>();
            int flowOffset = firstFlowOffset;

            foreach (Flow flow in flows)
            {
                individuallyWiredFlows.Add(GraphBuilder.WireFlowIndividually(flow, flowOffset));
                flowOffset += flow.Nodes.Count;
            }

            return individuallyWiredFlows;
        }

        /// <summary>
        /// Wires a flow individually meaning all steps inside. Basically the method <see cref="SetEdgesInStepBlock(IReadOnlyList{Node}, out Matrix{bool}, out List{ExternalEdge}, out List{InternalEdge}, out List{int})"/>
        /// is called for the flow.
        /// </summary>
        /// <param name="flow">The flow to wire.</param>
        /// <param name="flowOffset">The offset of the flows nodes. Later used to identify its edges in the edge matrix.</param>
        /// <returns>A tuple containing the offset as item 1, the flow as item 2 and then all the out parameters of <see cref="SetEdgesInStepBlock(IReadOnlyList{Node}, out Matrix{bool}, out List{ExternalEdge}, out List{InternalEdge}, out List{int})"/>.</returns>
        public static Tuple<int, Flow, Matrix<bool>, List<ExternalEdge>, List<InternalEdge>, List<Tuple<int, Condition?>>, Matrix<Condition?>> WireFlowIndividually(Flow flow, int flowOffset)
        {
            IReadOnlyList<Node> steps = flow.Nodes;
            Matrix<bool> edgeMatrix;
            List<ExternalEdge> externalEdges;
            List<InternalEdge> possibleInvalidIfEdges;
            List<Tuple<int, Condition?>> exitSteps;
            Matrix<Condition?> conditionMatrix;
            GraphBuilder.SetEdgesInStepBlock(steps, out edgeMatrix, out externalEdges, out possibleInvalidIfEdges, out exitSteps, out conditionMatrix);
            return new Tuple<int, Flow, Matrix<bool>, List<ExternalEdge>, List<InternalEdge>, List<Tuple<int, Condition?>>, Matrix<Condition?>>(
                flowOffset,
                flow,
                edgeMatrix,
                externalEdges,
                possibleInvalidIfEdges,
                exitSteps,
                conditionMatrix);
        }

        /// <summary>
        /// Sets the edges in a block of steps.
        /// </summary>
        /// <param name="steps">The steps to wire.</param>
        /// <param name="edgeMatrix">The matrix with the edges for the given steps.</param>
        /// <param name="externalEdges">A list of edges whose target is located outside the given steps.</param>
        /// <param name="possibleInvalidIfEdges">A list of edges between the given steps that may be invalid, because they represent an edge for an if statement without an else statement in this step list for the case the condition is false. These edge may be invalid if the else/else if is located in another block of steps/alternative flow.</param>
        /// <param name="exitSteps">A list of steps of the given block that whose edges lead out of the block to the next step of the outer block, with optional condition if not null. These are not abort edges!</param>
        /// <param name="conditionMatrix">A matrix that holds the conditions for edges. If a edge has a condition for being taken then its corresponding edge in the condition matrix holds the condition.</param>
        public static void SetEdgesInStepBlock(
            IReadOnlyList<Node> steps,
            out Matrix<bool> edgeMatrix,
            out List<ExternalEdge> externalEdges,
            out List<InternalEdge> possibleInvalidIfEdges,
            out List<Tuple<int, Condition?>> exitSteps,
            out Matrix<Condition?> conditionMatrix)
        {
            // Initialize out varibles
            edgeMatrix = new Matrix<bool>(steps.Count, false);
            externalEdges = new List<ExternalEdge>();
            possibleInvalidIfEdges = new List<InternalEdge>();
            exitSteps = new List<Tuple<int, Condition?>>();
            conditionMatrix = new Matrix<Condition?>(steps.Count, null);

            // Cycle through all steps and handle their edges.
            int lastStepIndex = -1;

            // If this is not null then the last step had a condition to continue and therefor provides it that way. Used for do-until statements.
            Condition? conditionFromPreviousStep = null;

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
                    GraphBuilder.SetEdgesInIfStatement(steps, ref edgeMatrix, ref externalEdges, ref possibleInvalidIfEdges, ref conditionMatrix, ref stepIndex);

                    // Revert the step index by one to one position before the end if so that in the next cycle it points to the end if step.
                    stepIndex--;
                }
                else if (stepType == StepType.Else || stepType == StepType.ElseIf)
                {
                    GraphBuilder.SetEdgesInElseElseIfStatement(steps, ref edgeMatrix, ref externalEdges, ref possibleInvalidIfEdges, ref conditionMatrix, ref stepIndex);

                    // Revert the step index by one to one position beofre the end if so that in the next cycle it points to the end if step.
                    stepIndex--;
                }
                else if (stepType == StepType.Do)
                {
                    GraphBuilder.SetEdgesInDoUntilStatement(steps, ref edgeMatrix, ref externalEdges, ref possibleInvalidIfEdges, ref conditionMatrix, ref stepIndex, out conditionFromPreviousStep);

                    // Revert the step index by one to one position beofre the until so that in the next cycle it points to the until step.
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
                else if (stepType == StepType.ValidatesThat)
                {
                    if (stepIndex < (steps.Count - 1))
                    {
                        // Not the last step.
                        edgeMatrix[stepIndex, stepIndex + 1] = true;
                        conditionMatrix[stepIndex, stepIndex + 1] = new Condition(stepDescription, true);
                    }
                    else
                    {
                        // The last step.
                        exitSteps.Add(new Tuple<int, Condition?>(stepIndex, new Condition(stepDescription, true)));
                    }
                }
                else
                {
                    // Treat it as a normal/unmatched step
                    if (stepIndex < (steps.Count - 1))
                    {
                        // Not the last step.
                        edgeMatrix[stepIndex, stepIndex + 1] = true;
                        conditionMatrix[stepIndex, stepIndex + 1] = conditionFromPreviousStep;
                    }
                    else
                    {
                        // The last step.
                        exitSteps.Add(new Tuple<int, Condition?>(stepIndex, conditionFromPreviousStep));
                    }

                    conditionFromPreviousStep = null;
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
        /// <param name="conditionMatrix">See <paramref name="conditionMatrix"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="stepIndex">On call the current index in the steps where the if start step is located and after the call the index of the end if step.</param>
        public static void SetEdgesInIfStatement(
            IReadOnlyList<Node> steps,
            ref Matrix<bool> edgeMatrix,
            ref List<ExternalEdge> externalEdges,
            ref List<InternalEdge> possibleInvalidIfEdges,
            ref Matrix<Condition?> conditionMatrix,
            ref int stepIndex)
        {
            List<int> importantIfSteps = GraphBuilder.GetImportantIfStatementSteps(steps, stepIndex);

            // Handle nested blocks
            int ifStepIndex = importantIfSteps.First(),
                lastBlockStartIndex = -1,
                endIfStepIndex = importantIfSteps.Last();

            for (int blockIndex = 0; blockIndex < importantIfSteps.Count - 1; blockIndex++)
            {
                int blockStartIndex = importantIfSteps[blockIndex],
                    blockEndIndex = importantIfSteps[blockIndex + 1],
                    blockSize = blockEndIndex - blockStartIndex - 1;
                StepType blockStartStepType = GraphBuilder.GetStepType(steps[blockStartIndex].StepDescription);

                // Set edge from last step to start of block
                if (blockIndex > 0)
                {
                    edgeMatrix[lastBlockStartIndex, blockStartIndex] = true;
                    conditionMatrix[lastBlockStartIndex, blockStartIndex] = new Condition(steps[lastBlockStartIndex].StepDescription, false);
                }

                if (blockSize > 0)
                {
                    GraphBuilder.SetEdgesInNestedBlock(
                        steps,
                        ref edgeMatrix,
                        ref externalEdges,
                        ref possibleInvalidIfEdges,
                        ref conditionMatrix,
                        blockStartIndex,
                        blockEndIndex,
                        endIfStepIndex,
                        blockStartStepType == StepType.Else ? (Condition?)null : new Condition(steps[blockStartIndex].StepDescription, true));
                }
                else
                {
                    // Wire block start step to endif step
                    edgeMatrix[blockStartIndex, endIfStepIndex] = true;

                    // If it is not the last block then the edge to the end if has a condition
                    if (blockIndex < importantIfSteps.Count - 2)
                    {
                        conditionMatrix[blockStartIndex, endIfStepIndex] = new Condition(steps[blockStartIndex].StepDescription, true);
                    }
                }

                lastBlockStartIndex = blockStartIndex;
            }

            // If last block is not a else block make an edge from the block start with false conditon to end if step.
            lastBlockStartIndex = importantIfSteps[importantIfSteps.Count - 2];
            int lastBlockEndIndex = importantIfSteps[importantIfSteps.Count - 1],
                lastBlockSize = lastBlockEndIndex - lastBlockStartIndex - 1;
            StepType lastBlockStartStepType = GraphBuilder.GetStepType(steps[lastBlockStartIndex].StepDescription);

            if (lastBlockStartStepType != StepType.Else && lastBlockSize > 0)
            {
                edgeMatrix[lastBlockStartIndex, lastBlockEndIndex] = true;
                conditionMatrix[lastBlockStartIndex, lastBlockEndIndex] = new Condition(steps[lastBlockStartIndex].StepDescription, false);
            }

            // If there are no else if or else blocks it might be that they are located in alternative flows or that there are none.
            // List that edge as possible invalid as it was previously created.
            if (importantIfSteps.Count <= 2)
            {
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
        /// <param name="conditionMatrix">See <paramref name="conditionMatrix"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="stepIndex">On call the current index in the steps where the else/else if start step is located and after the call the index of the end if step.</param>
        public static void SetEdgesInElseElseIfStatement(
            IReadOnlyList<Node> steps,
            ref Matrix<bool> edgeMatrix,
            ref List<ExternalEdge> externalEdges,
            ref List<InternalEdge> possibleInvalidIfEdges,
            ref Matrix<Condition?> conditionMatrix,
            ref int stepIndex)
        {
            List<int> importantIfSteps = GraphBuilder.GetImportantIfStatementSteps(steps, stepIndex);

            // Handle nested blocks
            int lastBlockStartIndex = -1,
                endIfStepIndex = importantIfSteps.Last();

            for (int blockIndex = 0; blockIndex < importantIfSteps.Count - 1; blockIndex++)
            {
                int blockStartIndex = importantIfSteps[blockIndex],
                    blockEndIndex = importantIfSteps[blockIndex + 1],
                    blockSize = blockEndIndex - blockStartIndex - 1;
                StepType blockStartStepType = GraphBuilder.GetStepType(steps[blockStartIndex].StepDescription);

                // Set edge from last step to start of block
                if (blockIndex > 0)
                {
                    edgeMatrix[lastBlockStartIndex, blockStartIndex] = true;
                    conditionMatrix[lastBlockStartIndex, blockStartIndex] = new Condition(steps[lastBlockStartIndex].StepDescription, false);
                }

                if (blockSize > 0)
                {
                    GraphBuilder.SetEdgesInNestedBlock(
                        steps,
                        ref edgeMatrix,
                        ref externalEdges,
                        ref possibleInvalidIfEdges,
                        ref conditionMatrix,
                        blockStartIndex,
                        blockEndIndex,
                        endIfStepIndex,
                        blockStartStepType == StepType.Else ? (Condition?)null : new Condition(steps[blockStartIndex].StepDescription, true));
                }
                else
                {
                    // Wire block start step to endif step
                    edgeMatrix[blockStartIndex, endIfStepIndex] = true;
                }

                lastBlockStartIndex = blockStartIndex;
            }

            // If last block is not a else block make an edge from the block start with false conditon to end if step.
            lastBlockStartIndex = importantIfSteps[importantIfSteps.Count - 2];
            int lastBlockEndIndex = importantIfSteps[importantIfSteps.Count - 1],
                lastBlockSize = lastBlockEndIndex - lastBlockStartIndex - 1;
            StepType lastBlockStartStepType = GraphBuilder.GetStepType(steps[lastBlockStartIndex].StepDescription);

            if (lastBlockStartStepType != StepType.Else && lastBlockSize > 0)
            {
                edgeMatrix[lastBlockStartIndex, lastBlockEndIndex] = true;
                conditionMatrix[lastBlockStartIndex, lastBlockEndIndex] = new Condition(steps[lastBlockStartIndex].StepDescription, false);
            }

            // Set current step index to end if step.
            stepIndex = endIfStepIndex;
        }

        /// <summary>
        /// Wires an do-until block. The block starts at <paramref name="stepIndex"/> in steps. The complete wiring is made and <paramref name="stepIndex"/> finally is set to
        /// the index of the until step.
        /// The given lists/matrices are correctly updated.
        /// </summary>
        /// <param name="steps">See <paramref name="steps"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="edgeMatrix">See <paramref name="edgeMatrix"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="externalEdges">See <paramref name="externalEdges"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="possibleInvalidIfEdges">See <paramref name="possibleInvalidIfEdges"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="conditionMatrix">See <paramref name="conditionMatrix"/> in <see cref="SetEdgesInstepBlock"/>.</param>
        /// <param name="stepIndex">On call the current index in the steps where the do start step is located and after the call the index of the until step.</param>
        /// <param name="exitCondition">A condition that belongs to the edge leading away from the until for that edge.</param>
        public static void SetEdgesInDoUntilStatement(
            IReadOnlyList<Node> steps,
            ref Matrix<bool> edgeMatrix,
            ref List<ExternalEdge> externalEdges,
            ref List<InternalEdge> possibleInvalidIfEdges,
            ref Matrix<Condition?> conditionMatrix,
            ref int stepIndex,
            out Condition? exitCondition)
        {
            Tuple<int, int> importantDoUntilSteps = GraphBuilder.GetImportantDoUntilStatementSteps(steps, stepIndex);

            // Handle nested block
            int doStepIndex = importantDoUntilSteps.Item1,
                untilStepIndex = importantDoUntilSteps.Item2,
                blockSize = untilStepIndex - doStepIndex - 1;

            if (blockSize > 0)
            {
                GraphBuilder.SetEdgesInNestedBlock(
                    steps,
                    ref edgeMatrix,
                    ref externalEdges,
                    ref possibleInvalidIfEdges,
                    ref conditionMatrix,
                    doStepIndex,
                    untilStepIndex,
                    untilStepIndex,
                    null);
            }
            else
            {
                // Wire do step to until step
                edgeMatrix[doStepIndex, untilStepIndex] = true;
            }

            // Set edge from until to do
            edgeMatrix[untilStepIndex, doStepIndex] = true;
            conditionMatrix[untilStepIndex, doStepIndex] = new Condition(steps[untilStepIndex].StepDescription, false);

            // Set current step index to end if step.
            stepIndex = untilStepIndex;
            exitCondition = new Condition(steps[untilStepIndex].StepDescription, true);
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
        /// <param name="conditionMatrix">The condition matrix.</param>
        /// <param name="blockStartIndex">The index of the block start. (The step before the nested block begins)</param>
        /// <param name="blockEndIndex">The index of the block end. (The step after the nested block ends)</param>
        /// <param name="exitStepsTargetStep">The step to where create the edges if the nested block has exit steps.</param>
        /// <param name="blockEntryCondition">The condition to be fulfilled to enter the nested block.</param>
        public static void SetEdgesInNestedBlock(
            IReadOnlyList<Node> steps,
            ref Matrix<bool> edgeMatrix,
            ref List<ExternalEdge> externalEdges,
            ref List<InternalEdge> possibleInvalidIfEdges,
            ref Matrix<Condition?> conditionMatrix,
            int blockStartIndex,
            int blockEndIndex,
            int exitStepsTargetStep,
            Condition? blockEntryCondition)
        {
            int blockSize = blockEndIndex - blockStartIndex - 1;

            // Set edge into the nested block
            edgeMatrix[blockStartIndex, blockStartIndex + 1] = true;
            conditionMatrix[blockStartIndex, blockStartIndex + 1] = blockEntryCondition;

            List<Node> nestedSteps = steps.Skip(blockStartIndex + 1).Take(blockSize).ToList();

            Matrix<bool> nestedEdgeMatrix;
            List<ExternalEdge> nestedExternalEdges;
            List<InternalEdge> nestedPossibleInvalidIfEdges;
            List<Tuple<int, Condition?>> nestedExitSteps;
            Matrix<Condition?> nestedConditionMatrix;
            GraphBuilder.SetEdgesInStepBlock(nestedSteps, out nestedEdgeMatrix, out nestedExternalEdges, out nestedPossibleInvalidIfEdges, out nestedExitSteps, out nestedConditionMatrix);

            // Unite matrices
            GraphBuilder.InsertMatrix(ref edgeMatrix, blockStartIndex + 1, blockStartIndex + 1, nestedEdgeMatrix);
            GraphBuilder.InsertMatrix(ref conditionMatrix, blockStartIndex + 1, blockStartIndex + 1, nestedConditionMatrix);

            // Unite externalEdges
            externalEdges.AddRange(nestedExternalEdges.ConvertAll((edge) => edge.NewWithIncreasedSourceStepNumber(blockStartIndex + 1)));

            // Unite possible invalid if edges
            possibleInvalidIfEdges.AddRange(nestedPossibleInvalidIfEdges.ConvertAll((edge) => edge.NewWithIncreasedSourceTargetStep(blockStartIndex + 1)));

            // Wire exit steps to end if step
            foreach (Tuple<int, Condition?> exitStep in nestedExitSteps)
            {
                edgeMatrix[exitStep.Item1 + blockStartIndex + 1, exitStepsTargetStep] = true;
                conditionMatrix[exitStep.Item1 + blockStartIndex + 1, exitStepsTargetStep] = exitStep.Item2;
            }
        }

        /// <summary>
        /// Inserts <paramref name="sourceMatrix"/> into <paramref name="targetMatrix"/>. The values in <paramref name="targetMatrix"/> are overridden. Make sure the dimensions match!
        /// <paramref name="sourceMatrix"/> is completely inserted into <paramref name="targetMatrix"/>.
        /// </summary>
        /// <typeparam name="T">The matrix content type.</typeparam>
        /// <param name="targetMatrix">The target matrix where to insert <paramref name="sourceMatrix"/>.</param>
        /// <param name="targetRow">The row in <paramref name="targetMatrix"/> where to start inserting.</param>
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
        /// The important steps are if, else if, else and end if. The given <paramref name="startStep"/> is assumed a valid if statement step and added automatically to the start of the list.
        /// It ends with the first end if that does not belong to a nested if statement.
        /// </summary>
        /// <param name="steps">The steps containing the if statement.</param>
        /// <param name="startStep">The index of the step in <paramref name="steps"/> where to start the search. Must be an if, else if, else step!</param>
        /// <returns>The indices of the important steps. Index 0 is the <paramref name="startStep"/> and the following are else if and else steps and an end if step as last index.</returns>
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
        /// Searches the important steps in an do-until statement starting in <paramref name="steps"/> at index <paramref name="startStep"/>.
        /// The important steps are do and until. The given <paramref name="startStep"/> is assumed a valid do statement step and added automatically to the start of the list.
        /// It ends with the first until that does not belong to a nested do-until statement.
        /// </summary>
        /// <param name="steps">The steps containing the do-until statement.</param>
        /// <param name="startStep">The index of the step in <paramref name="steps"/> where to start the search. Must be a do step!</param>
        /// <returns>The indices of the important steps. Item 1 is the do step index and item 2 the until step index.</returns>
        public static Tuple<int, int> GetImportantDoUntilStatementSteps(IReadOnlyList<Node> steps, int startStep)
        {
            // If this number is greater than 0 the index is currently in a nested do-until of the given depth.
            int numberNestedDoUntilStatements = 0,
                endStep = -1;

            for (int stepIndex = startStep + 1; stepIndex < steps.Count; stepIndex++)
            {
                StepType stepType = GraphBuilder.GetStepType(steps[stepIndex].StepDescription);

                if (stepType == StepType.Do)
                {
                    numberNestedDoUntilStatements++;
                    continue;
                }

                if (numberNestedDoUntilStatements > 0)
                {
                    // In nested if statement.
                    if (stepType == StepType.Until)
                    {
                        numberNestedDoUntilStatements--;
                    }
                }
                else
                {
                    // In root if statment.
                    if (stepType == StepType.Until)
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

            // Decrease by one because in the description the first step is numbered 1 but points to the index 0 in the list.
            int targetStepInBasicFlow = int.Parse(resumeMatch.Groups[1].Value) - 1;

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
        /// Tests if a string is equal to at least one keyword associated with the specified step type.
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
