// <copyright file="UseCaseTests.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTest.UcIntern
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using UseCaseCore.UcIntern;

    /// <summary>
    /// A class collecting all tests for the <see cref="UseCase"/> class.
    /// </summary>
    [TestFixture]
    public class UseCaseTests
    {
        /// <summary>
        /// Set and get use case name.
        /// </summary>
        [Test]
        public void SetGetUseCaseName()
        {
            // Arrange
            string expectedUseCaseName = "Use case name";
            UseCase useCase = new UseCase();

            // Act
            useCase.UseCaseName = expectedUseCaseName;

            // Assert
            Assert.AreEqual(expectedUseCaseName, useCase.UseCaseName);
        }

        /// <summary>
        /// Set and get brief description.
        /// </summary>
        [Test]
        public void SetGetBriefDescription()
        {
            // Arrange
            string expectedBriefDescription = "Brief description";
            UseCase useCase = new UseCase();

            // Act
            useCase.BriefDescription = expectedBriefDescription;

            // Assert
            Assert.AreEqual(expectedBriefDescription, useCase.BriefDescription);
        }

        /// <summary>
        /// Set and get precondition.
        /// </summary>
        [Test]
        public void SetGetPrecondition()
        {
            // Arrange
            string expectedPrecondition = "Precondition";
            UseCase useCase = new UseCase();

            // Act
            useCase.Precondition = expectedPrecondition;

            // Assert
            Assert.AreEqual(expectedPrecondition, useCase.Precondition);
        }

        /// <summary>
        /// Set and get primary actor.
        /// </summary>
        [Test]
        public void SetGetPrimaryActor()
        {
            // Arrange
            string expectedPrimaryActor = "Primary actor";
            UseCase useCase = new UseCase();

            // Act
            useCase.PrimaryActor = expectedPrimaryActor;

            // Assert
            Assert.AreEqual(expectedPrimaryActor, useCase.PrimaryActor);
        }

        /// <summary>
        /// Set and get SecondaryActors.
        /// </summary>
        [Test]
        public void SetGetSecondaryActors()
        {
            // Arrange
            string expectedSecondaryActors = "Secondary actors";
            UseCase useCase = new UseCase();

            // Act
            useCase.SecondaryActors = expectedSecondaryActors;

            // Assert
            Assert.AreEqual(expectedSecondaryActors, useCase.SecondaryActors);
        }

        /// <summary>
        /// Set and get Dependecy.
        /// </summary>
        [Test]
        public void SetGetDependecy()
        {
            // Arrange
            string expectedDependency = "Dependecy";
            UseCase useCase = new UseCase();

            // Act
            useCase.Dependency = expectedDependency;

            // Assert
            Assert.AreEqual(expectedDependency, useCase.Dependency);
        }

        /// <summary>
        /// Set and get generalization.
        /// </summary>
        [Test]
        public void SetGetGeneralization()
        {
            // Arrange
            string expectedGeneralization = "Generalization";
            UseCase useCase = new UseCase();

            // Act
            useCase.Generalization = expectedGeneralization;

            // Assert
            Assert.AreEqual(expectedGeneralization, useCase.Generalization);
        }

        /// <summary>
        /// Tests graph creation on interface class.
        /// Uses TB6 from GraphBuilderTests but with an additional step at the end.
        /// </summary>
        [Test]
        public void TestGraphCreation()
        {
            // Arrange
            FlowIdentifier flowIdentifierBasic = new FlowIdentifier(FlowType.Basic, 0),
                flowIdentifierSpecific1 = new FlowIdentifier(FlowType.SpecificAlternative, 1),
                flowIdentifierSpecific2 = new FlowIdentifier(FlowType.SpecificAlternative, 2),
                flowIdentifierGlobal1 = new FlowIdentifier(FlowType.GlobalAlternative, 1),
                flowIdentifierGlobal2 = new FlowIdentifier(FlowType.GlobalAlternative, 2),
                flowIdentifierBounded1 = new FlowIdentifier(FlowType.BoundedAlternative, 1),
                flowIdentiferBounded2 = new FlowIdentifier(FlowType.BoundedAlternative, 2);

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

            // Sadly not added bacause it would break parallels with TB6 and make debugging harder but I did not want to completely delete that step.
            //basicSteps.Add(new Node("And the use case ends on the country road into the West...", flowIdentifierBasic));
            List<Node> extendedBasicSteps = new List<Node>();
            extendedBasicSteps.AddRange(basicSteps);
            extendedBasicSteps.Add(new Node(string.Empty, flowIdentifierBasic));
            Flow basicFlow = new Flow(flowIdentifierBasic, null, basicSteps, new List<ReferenceStep>()),
                expectedBasicFlow = new Flow(flowIdentifierBasic, null, extendedBasicSteps, new List<ReferenceStep>());
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
            expectedEdgeMatrix[0, 1] = true;
            expectedEdgeMatrix[0, 2] = true;
            expectedEdgeMatrix[1, 2] = true;
            expectedEdgeMatrix[2, 3] = true;
            expectedEdgeMatrix[3, 4] = true;
            expectedEdgeMatrix[4, 5] = true;
            expectedEdgeMatrix[5, 6] = true;
            expectedEdgeMatrix[6, 7] = true;
            expectedEdgeMatrix[7, 8] = true;
            expectedEdgeMatrix[8, 9] = true;
            expectedEdgeMatrix[9, 10] = true;

            // specific/bounded
            expectedEdgeMatrix[3, 11] = true;
            expectedEdgeMatrix[11, 0] = true;
            expectedEdgeMatrix[6, 12] = true;
            expectedEdgeMatrix[12, 5] = true;
            expectedEdgeMatrix[7, 15] = true;
            expectedEdgeMatrix[8, 15] = true;
            expectedEdgeMatrix[9, 16] = true;
            expectedEdgeMatrix[16, 17] = true;
            expectedEdgeMatrix[17, 18] = true;
            expectedEdgeMatrix[18, 16] = true;
            expectedEdgeMatrix[18, 19] = true;

            // globals
            expectedEdgeMatrix[0, 13] = true;
            expectedEdgeMatrix[1, 13] = true;
            expectedEdgeMatrix[2, 13] = true;
            expectedEdgeMatrix[3, 13] = true;
            expectedEdgeMatrix[4, 13] = true;
            expectedEdgeMatrix[5, 13] = true;
            expectedEdgeMatrix[6, 13] = true;
            expectedEdgeMatrix[7, 13] = true;
            expectedEdgeMatrix[8, 13] = true;
            expectedEdgeMatrix[9, 13] = true;
            expectedEdgeMatrix[10, 13] = true;

            expectedEdgeMatrix[0, 14] = true;
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
            expectedConditionMatrix[0, 1] = new Condition(allSteps[0].StepDescription, true);
            expectedConditionMatrix[0, 2] = new Condition(allSteps[0].StepDescription, false);
            expectedConditionMatrix[3, 4] = new Condition(allSteps[3].StepDescription, true);
            expectedConditionMatrix[6, 7] = new Condition(allSteps[6].StepDescription, true);
            expectedConditionMatrix[7, 8] = new Condition(allSteps[7].StepDescription, true);
            expectedConditionMatrix[8, 9] = new Condition(allSteps[8].StepDescription, true);
            expectedConditionMatrix[9, 10] = new Condition(allSteps[9].StepDescription, true);

            // specific/bounded
            expectedConditionMatrix[3, 11] = new Condition(allSteps[3].StepDescription, false);
            expectedConditionMatrix[6, 12] = new Condition(allSteps[6].StepDescription, false);
            expectedConditionMatrix[7, 15] = new Condition(allSteps[7].StepDescription, false);
            expectedConditionMatrix[8, 15] = new Condition(allSteps[8].StepDescription, false);
            expectedConditionMatrix[9, 16] = new Condition(allSteps[9].StepDescription, false);

            expectedConditionMatrix[18, 16] = new Condition(allSteps[18].StepDescription, false);
            expectedConditionMatrix[18, 19] = new Condition(allSteps[18].StepDescription, true);

            // global
            expectedConditionMatrix[0, 13] = new Condition(allSteps[13].StepDescription, true);
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

            // Act
            UseCase useCase = new UseCase();
            useCase.BasicFlow = basicFlow;
            useCase.SpecificAlternativeFlows = specificFlows;
            useCase.GlobalAlternativeFlows = globalFlows;
            useCase.BoundedAlternativeFlows = boundedFlows;
            useCase.BuildGraph();

            // Assert
            Assert.AreEqual(expectedBasicFlow, useCase.BasicFlow);
            Assert.AreEqual(specificFlows, useCase.SpecificAlternativeFlows);
            Assert.AreEqual(globalFlows, useCase.GlobalAlternativeFlows);
            Assert.AreEqual(boundedFlows, useCase.BoundedAlternativeFlows);

            Assert.AreEqual(allSteps, useCase.Nodes);
            Assert.AreEqual(expectedEdgeMatrix, useCase.EdgeMatrix);
            Assert.AreEqual(expectedConditionMatrix, useCase.ConditionMatrix);
        }
    }
}
