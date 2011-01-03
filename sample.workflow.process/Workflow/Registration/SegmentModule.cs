using Autofac;

namespace sample.workflow.process.Workflow.Registration
{
    public class SegmentModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new WorkflowConfigurationModule());
            builder.RegisterType<DocumentContext>().As<IDocumentContext>();
        }
    }
}
