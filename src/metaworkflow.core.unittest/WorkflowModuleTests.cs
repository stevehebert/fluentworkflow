using System.Collections.Generic;
using System.Linq;
using Autofac;
using metaworkflow.core.Builder;
using metaworkflow.core.Configuration;
using metaworkflow.core.unittest.enums;
using NUnit.Framework;

namespace metaworkflow.core.unittest
{
    public class MyWorkflowModule : WorkflowModule<WorkflowType, StateType, TriggerType, TriggerContext>
    {
        public override void Configure(IWorkflowBuilder<WorkflowType, StateType, TriggerType, TriggerContext> builder)
        {
            builder.ForWorkflow(WorkflowType.Comment, StateType.New).Permit(TriggerType.Submit, StateType.UnderReview);
            builder.ForWorkflow(WorkflowType.Comment, StateType.UnderReview)
                .Permit(TriggerType.Publish, StateType.Complete)
                .Permit(TriggerType.Ignore, StateType.Rejected);
            builder.ForWorkflow(WorkflowType.Comment, StateType.Complete);
            builder.ForWorkflow(WorkflowType.Comment, StateType.Rejected);
        }
    }

    [TestFixture]
    public class WorkflowModuleTests
    {
        [Test]
        public void verify_workflow_step_declaration_registrations()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MyWorkflowModule());

            var container = builder.Build();

            var set = container.Resolve<IEnumerable<WorkflowStepDeclaration<WorkflowType, StateType, TriggerType>>>();

            Assert.That(set.Count(), Is.EqualTo(4));
            Assert.That(set.Where(p => p.Workflow == WorkflowType.Comment && p.State == StateType.UnderReview).FirstOrDefault().PermittedActions.Count(), Is.EqualTo(2));
            Assert.That(set.Where(p => p.Workflow == WorkflowType.Comment && p.State == StateType.Complete).FirstOrDefault().PermittedActions.Count(), Is.EqualTo(0));
            Assert.That(set.Where(p => p.Workflow == WorkflowType.Comment && p.State == StateType.New).FirstOrDefault().PermittedActions.Count(), Is.EqualTo(1));
        }
    }
}
