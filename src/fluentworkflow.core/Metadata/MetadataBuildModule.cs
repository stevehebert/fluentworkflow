using System;
using System.Collections.Generic;
using System.Linq;
using AutofacContrib.Attributed;
using Autofac;

namespace fluentworkflow.core.Metadata
{
    public interface IMetadataResolver<TType, TWorkflow, TState, TDetail>
    {
        IEnumerable<TType> For(TWorkflow workflow, TState state);
    }

    public class MetadataResolver<TType, TWorkflow, TState, TDetail> : IMetadataResolver<TType, TWorkflow, TState, TDetail>
    {
        private readonly IEnumerable<Lazy<TType, IEnumerable<MetadataInstance<TWorkflow, TState, TDetail>>>> _resolver;

        public MetadataResolver(IEnumerable<Lazy<TType, IEnumerable<MetadataInstance<TWorkflow, TState, TDetail>>>> resolver)
        {
            _resolver = resolver;

        }
        public IEnumerable<TType> For(TWorkflow workflow, TState state)
        {
            return
                from p in
                    _resolver.Where(
                        f =>
                        f.Metadata.FirstOrDefault(g => g.Workflow.Equals(workflow) && g.State.Equals(state)) != null)
                select p.Value;
        }
    }


    public class MetadataBuildModule<TType, TWorkflow, TState, TDetail> : MetadataModule<TType, IEnumerable<MetadataInstance<TWorkflow, TState, TDetail>>>
    {


        private readonly IList<MetadataInstance<TWorkflow, TState, TDetail>> _metadataInstances =
            new List<MetadataInstance<TWorkflow, TState, TDetail>>();

        public ITypeMapper<TType, TWorkflow, TState, TDetail> For(TWorkflow workflow, TState state)
        {
            return new TypeMapper<TType, TWorkflow, TState, TDetail>(workflow, state, _metadataInstances);
        }
        
        public override void Register(IMetadataRegistrar<TType, IEnumerable<MetadataInstance<TWorkflow, TState, TDetail>>> registrar)
        {
            var items = from p in _metadataInstances
                        group p by p.Type
                        into q
                        select q;

            foreach (var item in items)
                registrar.RegisterType(item.Key, item);

            registrar.ContainerBuilder.RegisterType<MetadataResolver<TType, TWorkflow, TState, TDetail>>().As<IMetadataResolver<TType, TWorkflow, TState, TDetail>>();
        }
    }

    public interface ITypeMapper<TType, TWorkflow, TState, TDetail>
    {
        IExtendedTypeMapper<TType, TWorkflow, TState, TDetail> Register<TConcreteType>() where TConcreteType : TType;
    }

    public interface IExtendedTypeMapper<TType, TWorkflow, TState, TDetail> : ITypeMapper<TType, TWorkflow, TState, TDetail>
    {
        ITypeMapper<TType, TWorkflow, TState, TDetail> AdditionalInfo(TDetail detail);
    }

    public class TypeMapper<TType, TWorkflow, TState, TDetail> : IExtendedTypeMapper<TType, TWorkflow, TState, TDetail>
    {
        private readonly IList<MetadataInstance<TWorkflow, TState, TDetail>> _metadataInstances;
        private readonly TWorkflow _workflow;
        private readonly TState _state;

        private MetadataInstance<TWorkflow, TState, TDetail> _currentInstance;

        internal TypeMapper(TWorkflow workflow, TState state, IList<MetadataInstance<TWorkflow, TState, TDetail>> metadatainstances)
        {
            _workflow = workflow;
            _state = state;
            _metadataInstances = metadatainstances;
        }


        public IExtendedTypeMapper<TType, TWorkflow, TState, TDetail> Register<TConcreteType>() where TConcreteType : TType
        {
            _currentInstance = new MetadataInstance<TWorkflow, TState, TDetail> { State = _state, Workflow = _workflow, Type = typeof(TConcreteType) };

            _metadataInstances.Add(_currentInstance);

            return this;
        }

        public ITypeMapper<TType, TWorkflow, TState, TDetail> AdditionalInfo(TDetail detail)
        {
            _currentInstance.Detail = detail;
            return this;
        }
    }


}
