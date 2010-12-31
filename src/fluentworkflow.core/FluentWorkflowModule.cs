using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using AutofacContrib.Attributed;
using fluentworkflow.core.Analysis;
using fluentworkflow.core.Builder;
using fluentworkflow.core.Configuration;

namespace fluentworkflow.core
{
    public abstract class FluentWorkflowModule<TWorkflow, TState, TTrigger, TTriggerContext> : Module
    {
        internal class FluentWorkflowMetadataModule : MetadataModule<IStateTask<TState, TTrigger, TTriggerContext>, IStateActionMetadata<TWorkflow, TState>>
        {
            private readonly IDictionary<Type, IStateActionMetadata<TWorkflow, TState>> _metadataValues =
                new Dictionary<Type, IStateActionMetadata<TWorkflow, TState>>();

            public void Add(Type type, IStateActionMetadata<TWorkflow, TState> metadataValues)
            {
                _metadataValues.Add(type, metadataValues);
            }

            public override void Register(IMetadataRegistrar<IStateTask<TState, TTrigger, TTriggerContext>, IStateActionMetadata<TWorkflow, TState>> registrar)
            {
                var results = new MetadataDependencyConstraintSolver().Analyze(_metadataValues);

                if (results.Any())
                    throw new StateStepDependencyException<TWorkflow, TState>(results);

                foreach (var value in _metadataValues)
                    RegisterType(value.Key, value.Value);
            }
        }

        public abstract void Configure(IWorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext> builder);

        protected sealed override void Load(ContainerBuilder builder)
        {
            var workflowBuilder = new WorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext>();

            Configure(workflowBuilder);

            var declarations = workflowBuilder.ProduceStepDeclarations();

            var errors = new ClosureAnalyzer().ValidateStepDeclarationClosure(declarations);

            if (errors.Any())
                throw new ClosureAnalysisException<TWorkflow, TState, TTrigger>(errors);

            var metadataModule = new FluentWorkflowMetadataModule();

            var uniqueTypes = from p in workflowBuilder.ProduceTypeRoles()
                              group p by p.StateStepType
                                  into g
                                  select g.Key;

            if (uniqueTypes.Any())
            {

                foreach (var type in uniqueTypes)
                {
                    var localType = type;
                    var metadata = new StateActionMetadata<TWorkflow, TState>();

                    var data = from p in workflowBuilder.ProduceTypeRoles()
                               where p.StateStepType == localType
                               select p;

                    foreach (var item in data)
                        metadata.Add(new StateActionInfo<TWorkflow, TState>(item.Workflow, item.State, item.ActionType,
                                                                            item.Priority, item.Dependency));


                    metadataModule.Add(type, metadata);
                }


                builder.RegisterModule(metadataModule);
            }

            foreach (var item in declarations)
            {
                var localItem = item;
                builder.Register(c => localItem);
            }

            builder.RegisterAdapter
                <WorkflowStepDeclaration<TWorkflow, TState, TTrigger>,
                    WorkflowStepAdapter<TWorkflow, TState, TTrigger, TTriggerContext>>((ctx, c)=> new WorkflowStepAdapter<TWorkflow, TState, TTrigger, TTriggerContext>(c, ctx.Resolve<IStateStepDispatcher<TWorkflow, TState, TTrigger, TTriggerContext>>()));

            builder.RegisterGeneric(typeof (StateStepDispatcher<,,,>)).As(typeof (IStateStepDispatcher<,,,>)).
                InstancePerDependency();

            builder.RegisterGeneric(typeof (StateMachineConfigurator<,,,>)).As(typeof (IStateMachineConfigurator<,,,>)).
                SingleInstance();
            //TODO
            builder.RegisterType<FluentStateEngine<TWorkflow, TState, TTrigger, TTriggerContext>>().As
                <IFluentStateEngine<TWorkflow, TState, TTrigger, TTriggerContext>>();
        }
    }
}
