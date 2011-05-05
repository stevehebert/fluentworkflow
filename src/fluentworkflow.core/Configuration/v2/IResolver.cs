using System;

namespace fluentworkflow.core.unittest.Configuration.v2
{
    public interface IResolver
    {
        object Resolve(Type resolveType);
    }
}
