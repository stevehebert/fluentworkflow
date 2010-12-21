using Autofac;

namespace blogflow.Notification.Registration
{
    public class SectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SimpleContextNotification>().As<IContextNotification>();
        }
    }
}