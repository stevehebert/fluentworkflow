using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using fluentworkflow.autofac;
using fluentworkflow.core.Analysis;
using fluentworkflow.core.Builder;
using fluentworkflow.core.Configuration;
using fluentworkflow.core.Configuration.v2;
using fluentworkflow.core.unittest.enums;
using NUnit.Framework;

namespace fluentworkflow.core.unittest
{
    public class InvalidClosureFluentWorkflowModule : FluentWorkflowModule<WorkflowType, StateType, TriggerType, TriggerContext>
    {
        public override void Configure(IWorkflowBuilder<WorkflowType, StateType, TriggerType, TriggerContext> builder)
        {
            builder.ForWorkflow(WorkflowType.Comment, StateType.New)
                      .Permit(TriggerType.Publish, StateType.UnderReview);

            builder.ForWorkflow(WorkflowType.Comment, StateType.Complete)
                .Permit(TriggerType.Ignore, StateType.New);
        }
    }

    public class MySimpleFluentWorkflowModule : FluentWorkflowModule<WorkflowType, StateType, TriggerType, TriggerContext>
    {

        public override void Configure(IWorkflowBuilder<WorkflowType, StateType, TriggerType, TriggerContext> builder)
        {
            builder.ForWorkflow(WorkflowType.Comment, StateType.New);
        }
    }

    public class MyFluentWorkflowModule : FluentWorkflowModule<WorkflowType, StateType, TriggerType, TriggerContext>
    {
        public override void Configure(IWorkflowBuilder<WorkflowType, StateType, TriggerType, TriggerContext> builder)
        {
            builder.ForWorkflow(WorkflowType.Comment, StateType.New)
                .Permit(TriggerType.Submit, StateType.UnderReview);

            builder.ForWorkflow(WorkflowType.Comment, StateType.UnderReview)
                .Permit(TriggerType.Publish, StateType.Complete)
                .Permit(TriggerType.Ignore, StateType.Rejected)
                .OnEntry<Task1>()
                .OnEntry<Task2>().DependsOn<Task1>()
                .OnExit<ExitTask3>();


            builder.ForWorkflow(WorkflowType.Comment, StateType.Complete)
                .OnEntry<Task1>();

            builder.ForWorkflow(WorkflowType.Comment, StateType.Rejected);
        }
    }

    [TestFixture]
    public class WorkflowModuleTests
    {
        public class FooProducer
        {
            public int Value { get; private set; }
            public Task1 Task { get; private set; }

            public FooProducer(int i, Task1 task)
            {
                Task = task;
                Value = i;
            }
        }

        [Test]
        public void task1_test_from_func()
        {
            TaskDisposalTracker.Reset();

            var builder = new ContainerBuilder();
            builder.RegisterType<Task1>();
            builder.RegisterType<FooProducer>();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var item = scope.Resolve<Func<int, FooProducer>>();

                item(5).Task.Execute(null);
            }

            Assert.That(TaskDisposalTracker.DisposeCount, Is.EqualTo(1));
            
        }
        [Test]
        public void task1_test()
        {
            TaskDisposalTracker.Reset();

            var builder = new ContainerBuilder();
            builder.RegisterType<Task1>();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var item = scope.Resolve<Task1>();

                item.Execute(null);
            }

            Assert.That(TaskDisposalTracker.DisposeCount, Is.EqualTo(1));
        }
        [Test]
        public void verify_workflow_execution_through_entry_and_exit_phases()
        {
            TaskDisposalTracker.Reset();
           
            var startingCount = Task1.ExecutionCount;
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MyFluentWorkflowModule());

            var container = builder.Build();
            IFluentStateEngine<WorkflowType, StateType, TriggerType, TriggerContext> machine;
            using (var resolver = container.BeginLifetimeScope())
            {

                 var stateMachine =
                    resolver.Resolve
                        <
                            Func
                                <WorkflowType, StateType,
                                    IFluentStateEngine<WorkflowType, StateType, TriggerType, TriggerContext>>>();

                machine = stateMachine(WorkflowType.Comment, StateType.UnderReview);
                machine.Fire(TriggerType.Publish, new TriggerContext {DocumentId = 5});
            }

            Assert.That(machine.State, Is.EqualTo(StateType.Complete));
            Assert.That(Task1.ExecutionCount - startingCount, Is.EqualTo(1));

            Assert.That(TaskDisposalTracker.DisposeCount, Is.EqualTo(1));
        }

        [Test]
        public void verify_workflow_execution_through_entry()
        {
            var startingCount = Task1.ExecutionCount;
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MyFluentWorkflowModule());

            var container = builder.Build();

            var stateMachine =
                container.Resolve
                    <Func<WorkflowType, StateType, IFluentStateEngine<WorkflowType, StateType, TriggerType, TriggerContext>>>();

            var machine = stateMachine(WorkflowType.Comment, StateType.New);
            machine.Fire(TriggerType.Submit, new TriggerContext { DocumentId = 5 });

            Assert.That(machine.State, Is.EqualTo(StateType.UnderReview));
            Assert.That(Task1.ExecutionCount - startingCount, Is.EqualTo(1));
        }


        [Test]
        public void verify_workflow_step_declaration_registrations()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MyFluentWorkflowModule());

            var container = builder.Build();

            var set = container.Resolve<IEnumerable<WorkflowStepDeclaration<WorkflowType, StateType, TriggerType>>>();

            Assert.That(set.Count(), Is.EqualTo(4));
            Assert.That(set.Where(p => p.Workflow == WorkflowType.Comment && p.State == StateType.UnderReview).FirstOrDefault().PermittedActions.Count(), Is.EqualTo(2));
            Assert.That(set.Where(p => p.Workflow == WorkflowType.Comment && p.State == StateType.Complete).FirstOrDefault().PermittedActions.Count(), Is.EqualTo(0));
            Assert.That(set.Where(p => p.Workflow == WorkflowType.Comment && p.State == StateType.New).FirstOrDefault().PermittedActions.Count(), Is.EqualTo(1));
        }

        [Test]
        public void verify_workflow_state_step_partial_registration_1()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MyFluentWorkflowModule());

            var container = builder.Build();

            var set =
                container.Resolve<WorkflowExecutionUniverse<WorkflowType, StateType, TriggerContext>>();
            var underReviewTasks = set.Retrieve(WorkflowType.Comment, StateType.UnderReview, WorkflowTaskActionType.Entry);

            Assert.That(underReviewTasks.Count(), Is.EqualTo(2));
            Assert.That(underReviewTasks.Where(p => p == typeof(Task1)).Count(), Is.EqualTo(1));
            Assert.That(underReviewTasks.Where(p => p == typeof(Task2)).Count(), Is.EqualTo(1));
        }

        [Test]
        public void verify_workflow_state_step_partial_registration_2()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MyFluentWorkflowModule());

            var container = builder.Build();

            var set =
                container.Resolve<WorkflowExecutionUniverse<WorkflowType, StateType, TriggerContext>>();

            var underReviewTasks = set.Retrieve(WorkflowType.Comment, StateType.UnderReview, WorkflowTaskActionType.Exit);

            Assert.That(underReviewTasks.Count(), Is.EqualTo(1));
            Assert.That(underReviewTasks.Where(p => p == typeof(ExitTask3)).Count(), Is.EqualTo(1));
        }

        [Test]
        public void verify_workflow_state_step_partial_registration_3()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MyFluentWorkflowModule());

            var container = builder.Build();

            var set =
                container.Resolve<WorkflowExecutionUniverse<WorkflowType, StateType, TriggerContext>>();

            var underReviewTasks = set.Retrieve(WorkflowType.Comment, StateType.Complete, WorkflowTaskActionType.Entry);

            Assert.That(underReviewTasks.Count(), Is.EqualTo(1));
            Assert.That(underReviewTasks.Where(p => p == typeof(Task1)).Count(), Is.EqualTo(1));
        }

        [Test]
        public void verify_workflow_state_step_empty_partial_registration()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MyFluentWorkflowModule());

            var container = builder.Build();

            var set =
                container.Resolve<WorkflowExecutionUniverse<WorkflowType, StateType, TriggerContext>>();

            var rejectedCommentTasks = set.Retrieve(WorkflowType.Comment, StateType.Rejected, WorkflowTaskActionType.Entry);

            Assert.That(rejectedCommentTasks.Count(), Is.EqualTo(0));
        }


        [Test]
        public void verify_workflow_wireup_without_step_declarations()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MySimpleFluentWorkflowModule());

            var container = builder.Build();

            var set =
                container.Resolve
                    <IEnumerable<Lazy
                                    <IStateTask<StateType, TriggerType, TriggerContext>,
                                        IStateActionMetadata<WorkflowType, StateType>>>>();

            Assert.That(set.Count(), Is.EqualTo(0));
        }

        [Test]
        public void verify_invalid_closure_model()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new InvalidClosureFluentWorkflowModule());

            var exception = Assert.Throws<ClosureAnalysisException<WorkflowType, StateType, TriggerType>>(() => builder.Build());

            Assert.That(exception.ClosureErrors.Count(), Is.EqualTo(1));

            var item = exception.ClosureErrors.First();

            Assert.That(item.Workflow, Is.EqualTo(WorkflowType.Comment));
            Assert.That(item.DestinationState, Is.EqualTo(StateType.UnderReview));
            Assert.That(item.DeclaringTrigger, Is.EqualTo(TriggerType.Publish));
            Assert.That(item.DeclaringState, Is.EqualTo(StateType.New));
        }
    }
}
