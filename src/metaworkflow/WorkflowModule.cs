using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using AutofacContrib.Attributed;
using metaworkflow.core.Builder;
using metaworkflow.core.Configuration;

namespace metaworkflow.core
{
    public abstract class WorkflowModule<TWorkflow, TState, TTrigger, TTriggerContext> : Module
    {
        internal class WorkflowMetadataModule : MetadataModule<IStateStep<TState, TTrigger, TTriggerContext>, IStateActionMetadata<TWorkflow, TState>>
        {
            private readonly IDictionary<Type, IStateActionMetadata<TWorkflow, TState>> _metadataValues =
                new Dictionary<Type, IStateActionMetadata<TWorkflow, TState>>();

            public void Add(Type type, IStateActionMetadata<TWorkflow, TState> metadataValues)
            {
                _metadataValues.Add(type, metadataValues);
            }

            public override void Register(IMetadataRegistrar<IStateStep<TState, TTrigger, TTriggerContext>, IStateActionMetadata<TWorkflow, TState>> registrar)
            {
                foreach (var value in _metadataValues)
                    RegisterType(value.Key, value.Value);
            }
        }

        public abstract void Configure(IWorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext> builder);

        protected override void Load(ContainerBuilder builder)
        {
            var workflowBuilder = new WorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext>();

            Configure(workflowBuilder);

            foreach (var item in workflowBuilder.ProduceStepDeclarations())
            {
                var localItem = item;
                builder.Register(c => localItem);
            }

            builder.RegisterAdapter
                <WorkflowStepDeclaration<TWorkflow, TState, TTrigger>,
                    WorkflowStepAdapter<TWorkflow, TState, TTrigger, TTriggerContext>>(
                        (ctx, c) =>
                        ctx.Resolve<Func<WorkflowStepDeclaration<TWorkflow, TState, TTrigger>,WorkflowStepAdapter<TWorkflow, TState, TTrigger, TTriggerContext>>>()(c));

            builder.RegisterType<StateStepDispatcher<TWorkflow, TState, TTrigger, TTriggerContext>>().As
                <IStateStepDispatcher<TWorkflow, TState, TTrigger, TTriggerContext>>();


            var uniqueTypes = from p in workflowBuilder.ProduceTypeRoles()
                              group p by p.StateStepType
                              into g select g.Key;

            if (!uniqueTypes.Any()) return;

            var metadataModule = new WorkflowMetadataModule();

            foreach (var type in uniqueTypes)
            {
                var localType = type;
                var metadata = new StateActionMetadata<TWorkflow, TState>();

                var data = from p in workflowBuilder.ProduceTypeRoles()
                           where p.StateStepType == localType
                           select p;

                foreach (var item in data)
                    metadata.Add(new StateActionInfo<TWorkflow, TState>(item.Workflow, item.State, item.ActionType,
                                                                        item.Priority));


                metadataModule.Add(type, metadata);
            }

            builder.RegisterModule(metadataModule);
            
        }
    }
}
