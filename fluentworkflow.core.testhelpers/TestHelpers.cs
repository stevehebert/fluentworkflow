using fluentworkflow.core.Builder;
using Stateless;

namespace fluentworkflow.core.testhelpers
{
    public static class TestHelpers
    {
        public static void ExecuteWithContext<TState,TTrigger,TContext>( this IExitStateStep<TState,TTrigger,TContext> exitStateStep,
            TState source,
            TState destination,
            TTrigger trigger,
            TContext context)
        {
            var transition = new StateMachine<TState, TTrigger>.Transition(source, destination, trigger);

            var stateStepInfo = new EntryStateStepInfo<TState, TTrigger, TContext>(context, transition);

            exitStateStep.Execute(stateStepInfo);
        }

        public static void ExecuteWithContext<TState, TTrigger, TContext>(this IEntryStateStep<TState, TTrigger, TContext> entryStateStep,
            TState source,
            TState destination,
            TTrigger trigger,
            TContext context)
        {
            var transition = new StateMachine<TState, TTrigger>.Transition(source, destination, trigger);

            var stateStepInfo = new EntryStateStepInfo<TState, TTrigger, TContext>(context, transition);

            entryStateStep.Execute(stateStepInfo);
        }

        public static IFlowMutator<TTrigger, TContext> ExecuteWithContext<TState, TTrigger, TContext>(this IMutatingEntryStateStep<TState, TTrigger, TContext> entryStateStep,
            TState source,
            TState destination,
            TTrigger trigger,
            TContext context)
        {
            var transition = new StateMachine<TState, TTrigger>.Transition(source, destination, trigger);

            var stateStepInfo = new EntryStateStepInfo<TState, TTrigger, TContext>(context, transition);

            var flowMutator = new FlowMutator<TTrigger, TContext>(context);

            entryStateStep.Execute(stateStepInfo, flowMutator);

            return flowMutator;
        }

    }
}
