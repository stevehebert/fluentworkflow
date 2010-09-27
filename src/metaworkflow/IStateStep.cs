
namespace metaworkflow.core
{
    /// <summary>
    /// The state step interface
    /// </summary>
    /// <typeparam name="TState">state type</typeparam>
    /// <typeparam name="TTrigger">state change trigger type</typeparam>
    /// <typeparam name="TTriggerContext">trigger context</typeparam>
    public interface IStateStep<TState, TTrigger, TTriggerContext>
    {
        /// <summary>
        /// Executes the specified state step utilizing the contextual
        /// information contained in state step info
        /// </summary>
        /// <param name="stateStepInfo">The state step info.</param>
        void Execute(StateStepInfo<TState, TTrigger, TTriggerContext> stateStepInfo);
    }

}
