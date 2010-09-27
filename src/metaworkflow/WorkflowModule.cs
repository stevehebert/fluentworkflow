using Autofac;
using metaworkflow.core.Builder;

namespace metaworkflow.core
{
    public abstract class WorkflowModule<TWorkflow, TState, TTrigger, TTriggerContext> : Module
    {
        public abstract void Configure(IWorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext> builder);

        protected override void Load(ContainerBuilder builder)
        {
            var workflowBuilder = new WorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext>();

            Configure(workflowBuilder);

            var items = workflowBuilder.ProduceStepDeclarations();

            foreach (var item in items)
            {
                var localItem = item;
                builder.Register(c => localItem);
            }
        }
    }
}
