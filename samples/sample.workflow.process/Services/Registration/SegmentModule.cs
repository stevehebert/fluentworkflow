using Autofac;
using sample.workflow.process.Services.Document;

namespace sample.workflow.process.Services.Registration
{
    public class SegmentModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NotificationService>().As<INotificationService>();
            builder.RegisterType<DocumentCreator>();
            builder.RegisterType<InputProcessor>();
        }
    }
}
