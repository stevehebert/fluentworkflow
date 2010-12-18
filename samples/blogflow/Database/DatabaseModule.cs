using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Raven.Client;
using Raven.Client.Document;

namespace blogflow.Database
{
    public class DatabaseModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance( new DocumentStore()).SingleInstance() ;
            builder.Register(c => c.Resolve<DocumentStore>().OpenSession()).As<IDocumentSession>().
                InstancePerLifetimeScope();
        }
    }
}