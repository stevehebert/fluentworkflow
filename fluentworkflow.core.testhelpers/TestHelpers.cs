using fluentworkflow.core.Builder;
using Stateless;

namespace fluentworkflow.core.testhelpers
{
    public static class TestHelpers
    {
        public static void ExecuteWithContext<TState,TTrigger,TContext>( this IExitStateTask<TState,TTrigger,TContext> exitStateTask,
            TState source,
            TState destination,
            TTrigger trigger,
            TContext context)
        {
            var transition = new StateMachine<TState, TTrigger>.Transition(source, destination, trigger);

            var stateStepInfo = new ExitStateTaskInfo<TState, TTrigger, TContext>(context, transition);

            exitStateTask.Execute(stateStepInfo);
        }

        public static void ExecuteWithContext<TState, TTrigger, TContext>(this IEntryStateTask<TState, TTrigger, TContext> entryStateTask,
            TState source,
            TState destination,
            TTrigger trigger,
            TContext context)
        {
            var transition = new StateMachine<TState, TTrigger>.Transition(source, destination, trigger);

            var stateStepInfo = new EntryStateTaskInfo<TState, TTrigger, TContext>(context, transition);

            entryStateTask.Execute(stateStepInfo);
        }

        public static IFlowMutator<TTrigger, TContext> ExecuteWithContext<TState, TTrigger, TContext>(this IMutatingEntryStateTask<TState, TTrigger, TContext> entryStateTask,
            TState source,
            TState destination,
            TTrigger trigger,
            TContext context)
        {
            var transition = new StateMachine<TState, TTrigger>.Transition(source, destination, trigger);

            var stateStepInfo = new EntryStateTaskInfo<TState, TTrigger, TContext>(context, transition);

            var flowMutator = new FlowMutator<TTrigger, TContext>(context);

            entryStateTask.Execute(stateStepInfo, flowMutator);

            return flowMutator;
        }

    }
}
