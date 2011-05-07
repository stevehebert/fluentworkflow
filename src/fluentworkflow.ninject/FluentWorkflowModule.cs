using System;
using fluentworkflow.core;
using fluentworkflow.core.Builder;
using fluentworkflow.core.Configuration;
using fluentworkflow.core.Configuration.v2;
using Ninject;
using Ninject.Modules;

namespace fluentworkflow.ninject
{
    public abstract class FluentWorkflowModule<TWorkflow, TState, TTrigger, TTriggerContext> : NinjectModule 
    {
        public abstract void Configure(IWorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext> builder);

        public override void Load() {
 
            var workflowBuilder = new WorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext>();

            Configure(workflowBuilder);

            var results = workflowBuilder.Compile();

            Bind<Func<Type, object>>().ToMethod(ctx => named => ctx.Kernel.Get(named));

            Bind<WorkflowExecutionUniverse<TWorkflow, TState, TTriggerContext>>()
                .ToConstant(results.WorkflowExecutionUniverse);

            foreach (var uniqueType in results.TransientTypes)
                Bind(uniqueType);

            foreach (var item in results.WorkflowStepDeclarations)
            {
                Bind<WorkflowStepDeclaration<TWorkflow, TState, TTrigger>>().ToConstant(item);

                // doing the following to compenstate for Ninject's inability setup an adapter pattern
                // please prove me wrong!

                var localItem = item;
                Bind<WorkflowStepAdapter<TWorkflow, TState, TTrigger, TTriggerContext>>().ToMethod(
                    ctx =>
                    new WorkflowStepAdapter<TWorkflow, TState, TTrigger, TTriggerContext>(localItem,
                                                                                          ctx.Kernel.Get
                                                                                              <
                                                                                              IStateStepDispatcher
                                                                                              <TWorkflow, TState,
                                                                                              TTrigger, TTriggerContext>
                                                                                              >()));


            }

            Bind<IFluentStateEngine<TWorkflow, TState, TTrigger, TTriggerContext>>().To
                <FluentStateEngine<TWorkflow, TState, TTrigger, TTriggerContext>>();

            Bind(typeof (IStateMachineConfigurator<,,,>)).To(typeof (StateMachineConfigurator<,,,>));
            Bind(typeof (IStateStepDispatcher<,,,>)).To(typeof (StateStepDispatcher<,,,>));

            // TODO: resolve adapter registration discrepency


        }
    }
}
