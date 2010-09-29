using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using metaworkflow.core.Analysis;
using metaworkflow.core.Configuration;
using metaworkflow.core.unittest.enums;
using NUnit.Framework;

namespace metaworkflow.core.unittest.Analysis
{
    [TestFixture]
    public class MetadataDependencyConstraintSolverTests
    {
        [Test]
        public void verify_priority_of_self_referencing_errors_in_lieu_of_other_errors()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            var metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowStepActionType.Entry, 0, typeof(Step1)));
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.Rejected, WorkflowStepActionType.Entry, 0, typeof(Step2)));

            typeRegistrations.Add(typeof(Step1), metadata);

            metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.Rejected, WorkflowStepActionType.Entry, 0, null));

            typeRegistrations.Add(typeof(Step2), metadata);

            var analyzer = new MetadataDependencyConstraintSolver();
            var errorResults = analyzer.Analyze(typeRegistrations);

            Assert.That(errorResults.Count(), Is.EqualTo(1));
            Assert.That(errorResults.First().Step, Is.EqualTo(typeof(Step1)));
            Assert.That(errorResults.First().Dependency, Is.EqualTo(typeof(Step1)));
            Assert.That(errorResults.First().State, Is.EqualTo(StateType.New));
            Assert.That(errorResults.First().Workflow, Is.EqualTo(WorkflowType.Comment));
            Assert.That(errorResults.First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.SelfReferencingDependency));
        }


        [Test]
        public void verify_self_referencing_dependency_error()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            var metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowStepActionType.Entry, 0, typeof(Step1)));

            typeRegistrations.Add(typeof(Step1), metadata);

            var analyzer = new MetadataDependencyConstraintSolver();
            var errorResults = analyzer.Analyze(typeRegistrations);

            Assert.That(errorResults.Count(), Is.EqualTo(1));
            Assert.That(errorResults.First().Step, Is.EqualTo(typeof(Step1)));
            Assert.That(errorResults.First().Dependency, Is.EqualTo(typeof(Step1)));
            Assert.That(errorResults.First().State, Is.EqualTo(StateType.New));
            Assert.That(errorResults.First().Workflow, Is.EqualTo(WorkflowType.Comment));
            Assert.That(errorResults.First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.SelfReferencingDependency));
        }


        [Test]
        public void verify_properly_closed_depedency_tree()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            var metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowStepActionType.Entry, 0, typeof(Step2)));

            typeRegistrations.Add(typeof(Step1), metadata);

            metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowStepActionType.Entry, 0, null));

            typeRegistrations.Add(typeof (Step2), metadata);

            var analyzer = new MetadataDependencyConstraintSolver();
            var errorResults = analyzer.Analyze(typeRegistrations);

            Assert.That(errorResults.Count(), Is.EqualTo(0));
        }

        [Test]
        public void verify_mismatched_dependency()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            var metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowStepActionType.Entry, 0, typeof(Step2)));

            typeRegistrations.Add(typeof(Step1), metadata);

            var analyzer = new MetadataDependencyConstraintSolver();
            var errorResults = analyzer.Analyze(typeRegistrations);

            Assert.That(errorResults.Count(), Is.EqualTo(1));
            Assert.That(errorResults.First().Step, Is.EqualTo(typeof(Step1)));
            Assert.That(errorResults.First().Dependency, Is.EqualTo(typeof(Step2)));
            Assert.That(errorResults.First().State, Is.EqualTo(StateType.New));
            Assert.That(errorResults.First().Workflow, Is.EqualTo(WorkflowType.Comment));
            Assert.That(errorResults.First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.UnknownDependency));
        }

        [Test]
        public void verify_mismatched_dependency_in_midst_of_good_dependencies_in_other_states()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            var metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowStepActionType.Entry, 0, typeof(Step2)));
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.Rejected, WorkflowStepActionType.Entry, 0, typeof(Step2)));
 
            typeRegistrations.Add(typeof(Step1), metadata);

            metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.Rejected, WorkflowStepActionType.Entry, 0, null));

            typeRegistrations.Add(typeof(Step2), metadata);

            var analyzer = new MetadataDependencyConstraintSolver();
            var errorResults = analyzer.Analyze(typeRegistrations);

            Assert.That(errorResults.Count(), Is.EqualTo(1));
            Assert.That(errorResults.First().Step, Is.EqualTo(typeof(Step1)));
            Assert.That(errorResults.First().Dependency, Is.EqualTo(typeof(Step2)));
            Assert.That(errorResults.First().State, Is.EqualTo(StateType.New));
            Assert.That(errorResults.First().Workflow, Is.EqualTo(WorkflowType.Comment));
            Assert.That(errorResults.First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.UnknownDependency));
        }

        [Test]
        public void verify_mismatched_dependency_in_midst_of_good_dependencies_in_other_workflows()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            var metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowStepActionType.Entry, 0, typeof(Step2)));
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.Rejected, WorkflowStepActionType.Entry, 0, typeof(Step2)));
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.NewOrder, StateType.Rejected, WorkflowStepActionType.Entry, 0, typeof(Step2)));

            typeRegistrations.Add(typeof(Step1), metadata);

            metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.Rejected, WorkflowStepActionType.Entry, 0, null));
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.NewOrder, StateType.Rejected, WorkflowStepActionType.Entry, 0, null));

            typeRegistrations.Add(typeof(Step2), metadata);

            var analyzer = new MetadataDependencyConstraintSolver();
            var errorResults = analyzer.Analyze(typeRegistrations);

            Assert.That(errorResults.Count(), Is.EqualTo(1));
            Assert.That(errorResults.First().Step, Is.EqualTo(typeof(Step1)));
            Assert.That(errorResults.First().Dependency, Is.EqualTo(typeof(Step2)));
            Assert.That(errorResults.First().State, Is.EqualTo(StateType.New));
            Assert.That(errorResults.First().Workflow, Is.EqualTo(WorkflowType.Comment));
            Assert.That(errorResults.First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.UnknownDependency));
        }

    }
}
