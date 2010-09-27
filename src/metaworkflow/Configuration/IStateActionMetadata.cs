using System.Collections.Generic;

namespace metaworkflow.core.Configuration
{
    public interface IStateActionMetadata<TWorkflow, TState>
    {
        /// <summary>
        /// Gets the state action info set.
        /// </summary>
        /// <value>The state action infos.</value>
        IEnumerable<StateActionInfo<TWorkflow, TState>> StateActionInfos { get; }
    }
}