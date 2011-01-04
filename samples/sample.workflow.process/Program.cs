using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using sample.workflow.process.Services.Document;

namespace sample.workflow.process
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This application simulates the flow of a comment through a content");
            Console.WriteLine("system at its most basic level");
            Console.WriteLine("");
            Console.WriteLine("This goes along with the sample code in the documentation.");
            Console.WriteLine("-----");

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ApplicationModule());

            var container = builder.Build();

            if( ! container.Resolve<DocumentCreator>().Create(42))
            {
                Console.WriteLine("!!! abend");
                return;
            }

            var inputProcess = container.Resolve<InputProcessor>();

            while (inputProcess.ProcessStateMove(42)) ;

            Console.WriteLine("exiting");
        }
    }
}
