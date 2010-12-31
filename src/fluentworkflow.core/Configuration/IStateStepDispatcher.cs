using Stateless;

namespace fluentworkflow.core.Configuration
{

    public interface IStateStepDispatcher<TWorkflow, TState, TTrigger, in TTriggerContext>
    {
        void ExecuteStepActions(WorkflowStepDeclaration<TWorkflow, TState, TTrigger> stepDeclaration,
                                TTriggerContext triggerContext,
                                StateMachine<TState, TTrigger>.Transition transition,
                                StateMachine<TState, TTrigger> stateMachine,
                                WorkflowStepActionType workflowStepActionType);
    }
}
