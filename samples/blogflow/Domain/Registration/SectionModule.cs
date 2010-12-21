using Autofac;
using blogflow.Domain.Repository;
using Raven.Client;

namespace blogflow.Domain.Registration
{
    public class SectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new DocumentStoreFactory().CreateStore()).SingleInstance();
            builder.Register(c => c.Resolve<IDocumentStore>().OpenSession()).InstancePerLifetimeScope();

            builder.RegisterType<BlogFlowRepository>().As<IRepository>().InstancePerLifetimeScope();

            builder.RegisterModule(new WorkflowConfigurationModule());
        }
    }
}