using System;
using System.Collections.Generic;
using System.Linq;
using fluentworkflow.core.Analysis;
using fluentworkflow.core.Configuration;
using fluentworkflow.core.unittest.enums;
using NUnit.Framework;

namespace fluentworkflow.core.unittest.Analysis
{
        public static class Extensions
        {
            public static void Add ( this Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>> dictionary, KeyValuePair<Type, IStateActionMetadata<WorkflowType, StateType>> item)
            {
                dictionary.Add(item.Key, item.Value);
            }
        }



    [TestFixture]
    public class MetadataDependencyConstraintSolverTests
    {
        [Test]
        public void verify_priority_of_self_referencing_errors_in_lieu_of_other_errors()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            var metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowTaskActionType.Entry, 0, typeof(Task1)));
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.Rejected, WorkflowTaskActionType.Entry, 0, typeof(Task2)));

            typeRegistrations.Add(typeof(Task1), metadata);

            metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.Rejected, WorkflowTaskActionType.Entry, 0, null));

            typeRegistrations.Add(typeof(Task2), metadata);

            var analyzer = new MetadataDependencyConstraintSolver();
            var errorResults = analyzer.Analyze(typeRegistrations);

            Assert.That(errorResults.Count(), Is.EqualTo(1));
            Assert.That(errorResults.First().Step, Is.EqualTo(typeof(Task1)));
            Assert.That(errorResults.First().Dependency, Is.EqualTo(typeof(Task1)));
            Assert.That(errorResults.First().State, Is.EqualTo(StateType.New));
            Assert.That(errorResults.First().Workflow, Is.EqualTo(WorkflowType.Comment));
            Assert.That(errorResults.First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.ParticipatesInCyclicalReference));
        }


        [Test]
        public void verify_self_referencing_dependency_error()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            var metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowTaskActionType.Entry, 0, typeof(Task1)));

            typeRegistrations.Add(typeof(Task1), metadata);

            var analyzer = new MetadataDependencyConstraintSolver();
            var errorResults = analyzer.Analyze(typeRegistrations);

            Assert.That(errorResults.Count(), Is.EqualTo(1));
            Assert.That(errorResults.First().Step, Is.EqualTo(typeof(Task1)));
            Assert.That(errorResults.First().Dependency, Is.EqualTo(typeof(Task1)));
            Assert.That(errorResults.First().State, Is.EqualTo(StateType.New));
            Assert.That(errorResults.First().Workflow, Is.EqualTo(WorkflowType.Comment));
            Assert.That(errorResults.First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.ParticipatesInCyclicalReference));
        }


        [Test]
        public void verify_properly_closed_depedency_tree()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            var metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowTaskActionType.Entry, 0, typeof(Task2)));

            typeRegistrations.Add(typeof(Task1), metadata);

            metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowTaskActionType.Entry, 0, null));

            typeRegistrations.Add(typeof (Task2), metadata);

            var analyzer = new MetadataDependencyConstraintSolver();
            var errorResults = analyzer.Analyze(typeRegistrations);

            Assert.That(errorResults.Count(), Is.EqualTo(0));
        }

        [Test]
        public void verify_mismatched_dependency()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            var metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowTaskActionType.Entry, 0, typeof(Task2)));

            typeRegistrations.Add(typeof(Task1), metadata);

            var analyzer = new MetadataDependencyConstraintSolver();
            var errorResults = analyzer.Analyze(typeRegistrations);

            Assert.That(errorResults.Count(), Is.EqualTo(1));
            Assert.That(errorResults.First().Step, Is.EqualTo(typeof(Task1)));
            Assert.That(errorResults.First().Dependency, Is.EqualTo(typeof(Task2)));
            Assert.That(errorResults.First().State, Is.EqualTo(StateType.New));
            Assert.That(errorResults.First().Workflow, Is.EqualTo(WorkflowType.Comment));
            Assert.That(errorResults.First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.UnknownDependency));
        }

        [Test]
        public void verify_mismatched_dependency_in_midst_of_good_dependencies_in_other_states()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            var metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowTaskActionType.Entry, 0, typeof(Task2)));
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.Rejected, WorkflowTaskActionType.Entry, 0, typeof(Task2)));
 
            typeRegistrations.Add(typeof(Task1), metadata);

            metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.Rejected, WorkflowTaskActionType.Entry, 0, null));

            typeRegistrations.Add(typeof(Task2), metadata);

            var analyzer = new MetadataDependencyConstraintSolver();
            var errorResults = analyzer.Analyze(typeRegistrations);

            Assert.That(errorResults.Count(), Is.EqualTo(1));
            Assert.That(errorResults.First().Step, Is.EqualTo(typeof(Task1)));
            Assert.That(errorResults.First().Dependency, Is.EqualTo(typeof(Task2)));
            Assert.That(errorResults.First().State, Is.EqualTo(StateType.New));
            Assert.That(errorResults.First().Workflow, Is.EqualTo(WorkflowType.Comment));
            Assert.That(errorResults.First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.UnknownDependency));
        }

        private static KeyValuePair<Type, IStateActionMetadata<WorkflowType, StateType>> CreateInterdependentSteps<TStep, TDependency>()
        {
            var metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowTaskActionType.Entry, 0, typeof(TDependency)));

            return new KeyValuePair<Type, IStateActionMetadata<WorkflowType, StateType>>(typeof(TStep), metadata);
        }

        private static KeyValuePair<Type, IStateActionMetadata<WorkflowType, StateType>> CreateIndependentSteps<TStep>()
        {
            var metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowTaskActionType.Entry, 0, null));

            return new KeyValuePair<Type, IStateActionMetadata<WorkflowType, StateType>>(typeof(TStep), metadata);
        }
        


        [Test]
        public void verify_linear_step_dependency_relationship()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            typeRegistrations.Add(CreateInterdependentSteps<TestStep5, TestStep4>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep4, TestStep3>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep3, TestStep2>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep2, TestStep1>());
            typeRegistrations.Add(CreateIndependentSteps<TestStep1>());

            var results = new MetadataDependencyConstraintSolver().Analyze(typeRegistrations);

            Assert.That(results.Count(), Is.EqualTo(0));
            
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep1) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(0)) ;
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep2) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(1));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep3) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(2));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep4) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(3));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep5) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(4));

        }

        [Test]
        public void verify_reverse_linear_step_dependency_relationship()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            typeRegistrations.Add(CreateIndependentSteps<TestStep1>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep2, TestStep1>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep3, TestStep2>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep4, TestStep3>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep5, TestStep4>());

            var results = new MetadataDependencyConstraintSolver().Analyze(typeRegistrations);

            Assert.That(results.Count(), Is.EqualTo(0));

            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep1) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(0));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep2) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(1));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep3) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(2));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep4) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(3));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep5) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(4));
        }

        [Test]
        public void verify_reverse_common_dependency_resolution()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            typeRegistrations.Add(CreateIndependentSteps<TestStep1>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep2, TestStep1>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep3, TestStep1>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep4, TestStep3>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep5, TestStep4>());

            var results = new MetadataDependencyConstraintSolver().Analyze(typeRegistrations);

            Assert.That(results.Count(), Is.EqualTo(0));

            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep1) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(0));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep2) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(1));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep3) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(1));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep4) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(2));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep5) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(3));
        }


        [Test]
        public void verify_common_dependency_value_at_top_of_tree()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            typeRegistrations.Add(CreateIndependentSteps<TestStep1>());
            typeRegistrations.Add(CreateIndependentSteps<TestStep2>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep3, TestStep1>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep4, TestStep3>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep5, TestStep4>());

            var results = new MetadataDependencyConstraintSolver().Analyze(typeRegistrations);

            Assert.That(results.Count(), Is.EqualTo(0));

            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep1) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(0));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep2) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(0));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep3) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(1));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep4) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(2));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep5) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(3));
        }

        [Test]
        public void verify_common_dependency_value_when_no_dependencies_exist()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            typeRegistrations.Add(CreateIndependentSteps<TestStep1>());
            typeRegistrations.Add(CreateIndependentSteps<TestStep2>());
            typeRegistrations.Add(CreateIndependentSteps<TestStep3>());
            typeRegistrations.Add(CreateIndependentSteps<TestStep4>());
            typeRegistrations.Add(CreateIndependentSteps<TestStep5>());

            var results = new MetadataDependencyConstraintSolver().Analyze(typeRegistrations);

            Assert.That(results.Count(), Is.EqualTo(0));

            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep1) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(0));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep2) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(0));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep3) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(0));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep4) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(0));
            Assert.That((from p in typeRegistrations where p.Key == typeof(TestStep5) select p).First().Value.StateActionInfos.First().Priority, Is.EqualTo(0));
        }

        [Test]
        public void verify_behavior_of_large_cyclical_dependency_tree()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            typeRegistrations.Add(CreateInterdependentSteps<TestStep1, TestStep5>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep2, TestStep1>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep3, TestStep2>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep4, TestStep3>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep5, TestStep4>());

            var results = new MetadataDependencyConstraintSolver().Analyze(typeRegistrations);

            Assert.That(results.Count(), Is.EqualTo(5));
            Assert.That((from p in results where p.Step == typeof(TestStep1) select p).First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.ParticipatesInCyclicalReference));
            Assert.That((from p in results where p.Step == typeof(TestStep2) select p).First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.ParticipatesInCyclicalReference));
            Assert.That((from p in results where p.Step == typeof(TestStep3) select p).First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.ParticipatesInCyclicalReference));
            Assert.That((from p in results where p.Step == typeof(TestStep4) select p).First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.ParticipatesInCyclicalReference));
            Assert.That((from p in results where p.Step == typeof(TestStep5) select p).First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.ParticipatesInCyclicalReference));
        }

        [Test]
        public void verify_behavior_of_small_cyclical_dependency_tree()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            typeRegistrations.Add(CreateInterdependentSteps<TestStep1, TestStep2>());
            typeRegistrations.Add(CreateInterdependentSteps<TestStep2, TestStep1>());

            var results = new MetadataDependencyConstraintSolver().Analyze(typeRegistrations);

            Assert.That(results.Count(), Is.EqualTo(2));
            Assert.That((from p in results where p.Step == typeof(TestStep1) select p).First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.ParticipatesInCyclicalReference));
            Assert.That((from p in results where p.Step == typeof(TestStep2) select p).First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.ParticipatesInCyclicalReference));
        }

        [Test]
        public void verify_mismatched_dependency_in_midst_of_good_dependencies_in_other_workflows()
        {
            var typeRegistrations = new Dictionary<Type, IStateActionMetadata<WorkflowType, StateType>>();

            var metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowTaskActionType.Entry, 0, typeof(Task2)));
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.Rejected, WorkflowTaskActionType.Entry, 0, typeof(Task2)));
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.NewOrder, StateType.Rejected, WorkflowTaskActionType.Entry, 0, typeof(Task2)));

            typeRegistrations.Add(typeof(Task1), metadata);

            metadata = new StateActionMetadata<WorkflowType, StateType>();
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment, StateType.Rejected, WorkflowTaskActionType.Entry, 0, null));
            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.NewOrder, StateType.Rejected, WorkflowTaskActionType.Entry, 0, null));

            typeRegistrations.Add(typeof(Task2), metadata);

            var analyzer = new MetadataDependencyConstraintSolver();
            var errorResults = analyzer.Analyze(typeRegistrations);

            Assert.That(errorResults.Count(), Is.EqualTo(1));
            Assert.That(errorResults.First().Step, Is.EqualTo(typeof(Task1)));
            Assert.That(errorResults.First().Dependency, Is.EqualTo(typeof(Task2)));
            Assert.That(errorResults.First().State, Is.EqualTo(StateType.New));
            Assert.That(errorResults.First().Workflow, Is.EqualTo(WorkflowType.Comment));
            Assert.That(errorResults.First().ErrorReason, Is.EqualTo(StateDependencyErrorReason.UnknownDependency));
        }


        public class TestStep1 : IStateTask<StateType, TriggerType, TriggerContext>
        {
            public void Execute(EntryStateTaskInfo<StateType, TriggerType, TriggerContext> entryStateTaskInfo)
            {
            }
        }

        public class TestStep2 : IStateTask<StateType, TriggerType, TriggerContext>
        {
            public void Execute(EntryStateTaskInfo<StateType, TriggerType, TriggerContext> entryStateTaskInfo)
            {
            }
        }

        public class TestStep3 : IStateTask<StateType, TriggerType, TriggerContext>
        {
            public void Execute(EntryStateTaskInfo<StateType, TriggerType, TriggerContext> entryStateTaskInfo)
            {
            }
        }

        public class TestStep4 : IStateTask<StateType, TriggerType, TriggerContext>
        {
            public void Execute(EntryStateTaskInfo<StateType, TriggerType, TriggerContext> entryStateTaskInfo)
            {
            }
        }

        public class TestStep5 : IStateTask<StateType, TriggerType, TriggerContext>
        {
            public void Execute(EntryStateTaskInfo<StateType, TriggerType, TriggerContext> entryStateTaskInfo)
            {
            }
        }


    }
}
