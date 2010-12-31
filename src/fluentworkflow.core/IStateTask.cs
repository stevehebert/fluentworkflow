
using fluentworkflow.core.Builder;

namespace fluentworkflow.core
{
    /// <summary>
    /// The state step interface.  This interface exists to unify storage in metadata and evaluation
    /// </summary>
    /// <typeparam name="TState">state type</typeparam>
    /// <typeparam name="TTrigger">state change trigger type</typeparam>
    /// <typeparam name="TTriggerContext">trigger context</typeparam>
    public interface IStateTask<TState, TTrigger, TTriggerContext>
    {
    }

    /// <summary>
    /// The exit state step interface
    /// </summary>
    /// <typeparam name="TState">The state type.</typeparam>
    /// <typeparam name="TTrigger">The trigger type.</typeparam>
    /// <typeparam name="TTriggerContext">The type of the trigger context.</typeparam>
    public interface IExitStateTask<TState, TTrigger, TTriggerContext> : IStateTask<TState, TTrigger, TTriggerContext>
    {
        /// <summary>
        /// Executes the specified exit state step info.
        /// </summary>
        /// <param name="exitStateTaskInfo">The exit state step info.</param>
        void Execute(ExitStateTaskInfo<TState, TTrigger, TTriggerContext> exitStateTaskInfo);
    }

    public interface IEntryStateTask<TState, TTrigger, TTriggerContext> : IStateTask<TState, TTrigger, TTriggerContext>
    {
        /// <summary>
        /// Executes the specified entry state step info.
        /// </summary>
        /// <param name="entryStateTaskInfo">The entry state step info.</param>
        void Execute(EntryStateTaskInfo<TState, TTrigger, TTriggerContext> entryStateTaskInfo);
    }

    public interface IMutatingEntryStateTask<TState, TTrigger, TTriggerContext> : IStateTask<TState, TTrigger, TTriggerContext>
    {
        /// <summary>
        /// Executes the specified entry state step info.
        /// </summary>
        /// <param name="entryStateTaskInfo">The entry state step info.</param>
        /// <param name="flowMutator">The flow mutator.</param>
        void Execute(EntryStateTaskInfo<TState, TTrigger, TTriggerContext> entryStateTaskInfo,
                     IFlowMutator<TTrigger, TTriggerContext> flowMutator);
    }

}
