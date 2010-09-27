using System.Collections.Generic;

namespace metaworkflow.core.Configuration
{
    public class StateActionMetadata<TWorkflow, TState> : IStateActionMetadata<TWorkflow, TState>
    {
        private readonly IList<StateActionInfo<TWorkflow, TState>> _stateActionInfoSet =
            new List<StateActionInfo<TWorkflow, TState>>();

        /// <summary>
        /// Gets the state action info set.
        /// </summary>
        /// <value>The state action info set.</value>
        public IEnumerable<StateActionInfo<TWorkflow, TState>> StateActionInfos
        {
            get { return _stateActionInfoSet; }
        }

        /// <summary>
        /// Adds the specified state action info.
        /// </summary>
        /// <param name="stateActionInfo">The state action info.</param>
        public void Add(StateActionInfo<TWorkflow, TState> stateActionInfo)
        {
            _stateActionInfoSet.Add(stateActionInfo);
        }
    }
}
