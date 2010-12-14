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

    public class TypeMapperRoot<TType, TWorkflow, TState, TDetail>
    {
        private readonly IList<MetadataInstance<TWorkflow, TState, TDetail>> _metadataInstances;

        public TypeMapperRoot(IList<MetadataInstance<TWorkflow, TState, TDetail>> metadataInstances)
        {
            _metadataInstances = metadataInstances;
        }


        public ITypeMapper<TType, TWorkflow, TState, TDetail> For(TWorkflow workflow, TState state)
        {
            return new TypeMapper<TType, TWorkflow, TState, TDetail>(workflow, state, _metadataInstances);
        }
        
    }

    public interface IMetadataInfo<TWorkflow, TState, TDetail>
    {
        IEnumerable<MetadataInstance<TWorkflow, TState, TDetail>> MetadataInstances { get; set; }
    }

    public class MetadataInfo<TWorkflow, TState, TDetail> :IMetadataInfo<TWorkflow,TState, TDetail>
    {
        public IEnumerable<MetadataInstance<TWorkflow, TState, TDetail>> MetadataInstances { get; set; }
    }

    public abstract class  MetadataBuildModule<TType, TWorkflow, TState, TDetail> : MetadataModule<TType, IMetadataInfo<TWorkflow, TState, TDetail>>
    {

        private readonly IList<MetadataInstance<TWorkflow, TState, TDetail>> _metadataInstances =
            new List<MetadataInstance<TWorkflow, TState, TDetail>>();

        public abstract void BuildMapping(TypeMapperRoot<TType, TWorkflow, TState, TDetail> mapperRoot);
       
        public override void Register(IMetadataRegistrar<TType, IMetadataInfo<TWorkflow, TState, TDetail>> registrar)
        {
            BuildMapping(new TypeMapperRoot<TType, TWorkflow, TState, TDetail>(_metadataInstances));

            var items = from p in _metadataInstances
                        group p by p.Type
                        into q
                        select q;

            foreach (var item in items)
                registrar.RegisterType(item.Key,
                                       new MetadataInfo<TWorkflow, TState, TDetail>
                                           {MetadataInstances = from t in item select t});

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
