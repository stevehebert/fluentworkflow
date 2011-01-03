using Autofac;
using Raven.Client;

namespace sample.workflow.process.Repository.Registration
{
    public class SegmentModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new DocumentStoreFactory().CreateStore()).SingleInstance();
            builder.Register(c => c.Resolve<IDocumentStore>().OpenSession()).InstancePerLifetimeScope();

            builder.RegisterType<BlogFlowRepository>().As<IRepository>().InstancePerLifetimeScope();
        }
    }
}
