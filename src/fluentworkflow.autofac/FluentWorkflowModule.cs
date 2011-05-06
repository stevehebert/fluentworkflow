﻿using System;
using System.Linq;
using Autofac;
using fluentworkflow.core;
using fluentworkflow.core.Analysis;
using fluentworkflow.core.Builder;
using fluentworkflow.core.Configuration;
using fluentworkflow.core.Configuration.v2;

namespace fluentworkflow.autofac
{
    public abstract class FluentWorkflowModule<TWorkflow, TState, TTrigger, TTriggerContext> : Module
    {
        public abstract void Configure(IWorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext> builder);

        protected sealed override void Load(ContainerBuilder builder)
        {
            var workflowBuilder = new WorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext>();

            Configure(workflowBuilder);

            var declarations = workflowBuilder.ProduceStepDeclarations();

            var errors = new ClosureAnalyzer().ValidateStepDeclarationClosure(declarations);

            if (errors.Any())
                throw new ClosureAnalysisException<TWorkflow, TState, TTrigger>(errors);

            var uniqueTypes = from p in workflowBuilder.ProduceTypeRoles()
                              group p by p.StateStepType
                              into g
                              select g.Key;

            var universe = new WorkflowExecutionUniverse<TWorkflow, TState, TTriggerContext>();

            if (uniqueTypes.Any())
            {
                var items = from p in workflowBuilder.ProduceTypeRoles()
                            group p by new {p.Workflow, p.State, p.ActionType}
                            into p1
                            select
                                new
                                    {
                                        p1.Key.Workflow,
                                        p1.Key.State,
                                        p1.Key.ActionType,
                                        Types = from s in p1 orderby s.Priority select s.StateStepType
                                    };

                foreach (var item in items)
                    universe.Add(new WorkflowStateExecutionSet<TWorkflow, TState>(item.Workflow, item.State,
                                                                                  item.ActionType, item.Types));
            }

            builder.RegisterInstance(universe);

            foreach (var uniqueType in uniqueTypes)
                builder.RegisterType(uniqueType);

            foreach (var item in declarations)
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