using System;
using Autofac;

namespace fluentworkflow.core.unittest.Configuration.v2
{
    public interface IResolver
    {
        object Resolve(Type resolveType);
    }

    public class Resolver : IResolver
    {
        private IComponentContext _container;

        public Resolver(IComponentContext container )
        {
            _container = container;
        }

        public object Resolve(Type resolveType)
        {
            return _container.Resolve(resolveType);
        }
    }
}
