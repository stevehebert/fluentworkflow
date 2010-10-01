
using fluentworkflow.core.Builder;

namespace fluentworkflow.core
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

    public interface IActionableStateStep<TState, TTrigger, TTriggerContext> : IStateStep<TState, TTrigger, TTriggerContext>
    {
        /// <summary>
        /// provides state to the step prior to calling the execute method. provided
        /// state can be held by the object for interaction during the execute
        /// phase.
        /// </summary>
        /// <param name="flowMutator">The trigger trip.</param>
        void PreExecute(IFlowMutator<TTrigger, TTriggerContext> flowMutator);
    }

}
