using System;
using Autofac;
using fluentworkflow.core;
using fluentworkflow.core.Builder;
using fluentworkflow.core.Configuration;

namespace fluentworkflow.autofac
{
    public abstract class FluentWorkflowModule<TWorkflow, TState, TTrigger, TTriggerContext> : Module
    {
        public abstract void Configure(IWorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext> builder);

        protected sealed override void Load(ContainerBuilder builder)
        {
            var workflowBuilder = new WorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext>();

            Configure(workflowBuilder);

            var results = workflowBuilder.Compile();

            builder.RegisterInstance(results.WorkflowExecutionUniverse);

            foreach (var uniqueType in results.TransientTypes)
                builder.RegisterType(uniqueType);

            foreach (var item in results.WorkflowStepDeclarations)
            {
                var localItem = item;
                builder.Register(c => localItem);
            }

            builder.Register<Func<Type, object>>(c =>
                                                     {
                                                         var resolver = c.Resolve<IComponentContext>();
                                                         return named => resolver.Resolve(named);
                                                     });


            builder.RegisterAdapter
                <WorkflowStepDeclaration<TWorkflow, TState, TTrigger>,
                    WorkflowStepAdapter<TWorkflow, TState, TTrigger, TTriggerContext>>(
                        (ctx, c) =>
                        new WorkflowStepAdapter<TWorkflow, TState, TTrigger, TTriggerContext>(c,
                                                                                              ctx.Resolve
                                                                                                  <
                                                                                                  IStateStepDispatcher
                                                                                                  <TWorkflow, TState,
                                                                                                  TTrigger,
                                                                                                  TTriggerContext>>())).
                InstancePerDependency();

            builder.RegisterGeneric(typeof (StateStepDispatcher<,,,>)).As(typeof (IStateStepDispatcher<,,,>)).
                InstancePerDependency();

            builder.RegisterGeneric(typeof (StateMachineConfigurator<,,,>)).As(typeof (IStateMachineConfigurator<,,,>)).
                InstancePerDependency();
            //TODO
            builder.RegisterType<FluentStateEngine<TWorkflow, TState, TTrigger, TTriggerContext>>().As
                <IFluentStateEngine<TWorkflow, TState, TTrigger, TTriggerContext>>();
        }
    }
}
