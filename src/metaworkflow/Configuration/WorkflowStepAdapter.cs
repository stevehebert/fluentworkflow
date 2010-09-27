using Stateless;

namespace metaworkflow.core.Configuration
{
    public class WorkflowStepAdapter<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        private readonly WorkflowStepDeclaration<TWorkflow, TState, TTrigger> _workflowStepDeclaration;
        private readonly IStateStepDispatcher<TWorkflow, TState, TTrigger, TTriggerContext> _stateStepDispatcher;

        public WorkflowStepAdapter(WorkflowStepDeclaration<TWorkflow, TState, TTrigger> workflowStepDeclaration, IStateStepDispatcher<TWorkflow, TState, TTrigger, TTriggerContext> stateStepDispatcher)
        {
            _workflowStepDeclaration = workflowStepDeclaration;
            _stateStepDispatcher = stateStepDispatcher;
        }

        public void ConfigureStep(StateMachine<TState, TTrigger> stateMachine)
        {
            var configuration = stateMachine.Configure(_workflowStepDeclaration.State);

            foreach (var path in _workflowStepDeclaration.PermittedActions)
                configuration.Permit(path.Trigger, path.DestinationState);

            configuration.OnEntry<TTriggerContext>(
                (context, transition) => _stateStepDispatcher.ExecuteStepActions(_workflowStepDeclaration,
                                                                                 context,
                                                                                 transition,
                                                                                 stateMachine,
                                                                                 WorkflowStepActionType.Entry));

            configuration.OnExit<TTriggerContext>(
                (transition, context) => _stateStepDispatcher.ExecuteStepActions(_workflowStepDeclaration, 
                                                                                 context, 
                                                                                 transition,
                                                                                 stateMachine,
                                                                                 WorkflowStepActionType.Exit));

        }
    }
}
