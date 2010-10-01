using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using fluentworkflow.core.Analysis;
using fluentworkflow.core.Builder;
using fluentworkflow.core.Configuration;
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
                .OnEntry<Step1>()
                .OnEntry<Step2>().DependsOn<Step1>()
                .OnExit<Step1>();


            builder.ForWorkflow(WorkflowType.Comment, StateType.Complete)
                .OnEntry<Step1>();

            builder.ForWorkflow(WorkflowType.Comment, StateType.Rejected);
        }
    }

    [TestFixture]
    public class WorkflowModuleTests
    {

        [Test]
        public void verify_workflow_execution_through_entry_and_exit_phases()
        {
            var startingCount = Step1.ExecutionCount;
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MyFluentWorkflowModule());

            var container = builder.Build();

            var stateMachine =
                container.Resolve
                    <Func<WorkflowType, StateType, IFluentStateEngine<WorkflowType, StateType, TriggerType, TriggerContext>>>();

            var machine = stateMachine(WorkflowType.Comment, StateType.UnderReview);
            machine.Fire(TriggerType.Publish, new TriggerContext { DocumentId = 5 });

            Assert.That(machine.State, Is.EqualTo(StateType.Complete));
            Assert.That(Step1.ExecutionCount - startingCount, Is.EqualTo(2));
        }

        [Test]
        public void verify_workflow_execution_through_entry()
        {
            var startingCount = Step1.ExecutionCount;
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MyFluentWorkflowModule());

            var container = builder.Build();

            var stateMachine =
                container.Resolve
                    <Func<WorkflowType, StateType, IFluentStateEngine<WorkflowType, StateType, TriggerType, TriggerContext>>>();

            var machine = stateMachine(WorkflowType.Comment, StateType.New);
            machine.Fire(TriggerType.Submit, new TriggerContext { DocumentId = 5 });

            Assert.That(machine.State, Is.EqualTo(StateType.UnderReview));
            Assert.That(Step1.ExecutionCount - startingCount, Is.EqualTo(1));
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
        public void verify_workflow_state_step_registrations()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MyFluentWorkflowModule());

            var container = builder.Build();

            var set =
                container.Resolve<IEnumerable<Lazy<IStateStep<StateType, TriggerType, TriggerContext>, IStateActionMetadata<WorkflowType, StateType>>>>();

            Assert.That(set.Count(), Is.EqualTo(2));
            Assert.That(set.First().Metadata.StateActionInfos, Is.Not.Null);
            Assert.That(set.Where(p => p.Value.GetType() == typeof(Step1)).First().Metadata.StateActionInfos.Count(), Is.EqualTo(3));
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
                                    <IStateStep<StateType, TriggerType, TriggerContext>,
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
