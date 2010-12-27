
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
    }

    public interface IExitStateStep<TState, TTrigger, TTriggerContext> : IStateStep<TState, TTrigger, TTriggerContext>
    {
        void Execute(EntryStateStepInfo<TState, TTrigger, TTriggerContext> entryStateStepInfo);
    }

    public interface IEntryStateStep<TState, TTrigger, TTriggerContext> : IStateStep<TState, TTrigger, TTriggerContext>
    {
        void Execute(EntryStateStepInfo<TState, TTrigger, TTriggerContext> entryStateStepInfo);
    }

    public interface IMutatingEntryStateStep<TState, TTrigger, TTriggerContext> : IStateStep<TState, TTrigger, TTriggerContext>
    {
        void Execute(EntryStateStepInfo<TState, TTrigger, TTriggerContext> entryStateStepInfo,
                     IFlowMutator<TTrigger, TTriggerContext> flowMutator);
    }

}
