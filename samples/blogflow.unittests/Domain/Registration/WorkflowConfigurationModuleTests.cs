using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using blogflow.Domain.Registration;
using blogflow.Domain.Workflow;
using fluentworkflow.core.Analysis;
using NUnit.Framework;

namespace blogflow.unittests.Domain.Registration
{
    [TestFixture]
    public class WorkflowConfigurationModuleTests
    {
        [Test]
        public void verify_registration()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new WorkflowConfigurationModule());

            try
            {
                builder.Build();
            }
            catch(ClosureAnalysisException<DocumentType,WorkflowState,StateTrigger> closureAnalysisException)
            {
                Console.WriteLine(closureAnalysisException.Message);
                Assert.Fail();
            }


        }
    }
}
