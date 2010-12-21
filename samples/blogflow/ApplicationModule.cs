using Autofac;

namespace blogflow
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new Notification.Registration.SectionModule());
            builder.RegisterModule(new Domain.Registration.SectionModule());
        }
    }
}