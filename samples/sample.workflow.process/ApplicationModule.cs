using Autofac;

namespace sample.workflow.process
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new Services.Registration.SegmentModule());
            builder.RegisterModule(new Workflow.Registration.SegmentModule());
            builder.RegisterModule(new Repository.Registration.SegmentModule());
        }
    }
}
