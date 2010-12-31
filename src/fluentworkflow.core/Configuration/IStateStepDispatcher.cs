using Stateless;

namespace fluentworkflow.core.Configuration
{

    public interface IStateStepDispatcher<TWorkflow, TState, TTrigger, in TTriggerContext>
    {
        /// <summary>
        /// Executes the entry step actions.
        /// </summary>
        /// <param name="stepDeclaration">The step declaration.</param>
        /// <param name="triggerContext">The trigger context.</param>
        /// <param name="transition">The transition.</param>
        /// <param name="stateMachine">The state machine.</param>
        void ExecuteEntryStepActions(WorkflowStepDeclaration<TWorkflow, TState, TTrigger> stepDeclaration,
                                TTriggerContext triggerContext,
                                StateMachine<TState, TTrigger>.Transition transition,
                                StateMachine<TState, TTrigger> stateMachine);

        /// <summary>
        /// Executes the exit step actions.
        /// </summary>
        /// <param name="stepDeclaration">The step declaration.</param>
        /// <param name="triggerContext">The trigger context.</param>
        /// <param name="transition">The transition.</param>
        /// <param name="stateMachine">The state machine.</param>
        void ExecuteExitStepActions(WorkflowStepDeclaration<TWorkflow, TState, TTrigger> stepDeclaration,
                        TTriggerContext triggerContext,
                        StateMachine<TState, TTrigger>.Transition transition,
                        StateMachine<TState, TTrigger> stateMachine);
    }
}
