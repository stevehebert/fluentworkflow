using Stateless;

namespace metaworkflow.core.Configuration
{
    /// <summary>
    /// responsible for configuration the state machine given an individual state step declaration
    /// </summary>
    /// <typeparam name="TWorkflow">The type of the workflow.</typeparam>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TTrigger">The type of the trigger.</typeparam>
    /// <typeparam name="TTriggerContext">The type of the trigger context.</typeparam>
    public class WorkflowStepAdapter<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        private readonly WorkflowStepDeclaration<TWorkflow, TState, TTrigger> _workflowStepDeclaration;
        private readonly IStateStepDispatcher<TWorkflow, TState, TTrigger, TTriggerContext> _stateStepDispatcher;

        /// <summary>
        /// Gets the target workflow.
        /// </summary>
        /// <value>The target workflow.</value>
        public TWorkflow Workflow { get { return _workflowStepDeclaration.Workflow; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowStepAdapter&lt;TWorkflow, TState, TTrigger, TTriggerContext&gt;"/> class.
        /// </summary>
        /// <param name="workflowStepDeclaration">The workflow step declaration.</param>
        /// <param name="stateStepDispatcher">The state step dispatcher.</param>
        public WorkflowStepAdapter(WorkflowStepDeclaration<TWorkflow, TState, TTrigger> workflowStepDeclaration, IStateStepDispatcher<TWorkflow, TState, TTrigger, TTriggerContext> stateStepDispatcher)
        {
            _workflowStepDeclaration = workflowStepDeclaration;
            _stateStepDispatcher = stateStepDispatcher;
        }

        /// <summary>
        /// Configures the step.
        /// </summary>
        /// <param name="stateMachine">The state machine.</param>
        public void ConfigureStep(StateMachine<TState, TTrigger> stateMachine)
        {
            var configuration = stateMachine.Configure(_workflowStepDeclaration.State);

            foreach (var path in _workflowStepDeclaration.PermittedActions)
                configuration.Permit(path.Trigger, path.DestinationState);

            configuration.OnEntry<TTriggerContext>(
                (transition, context) => _stateStepDispatcher.ExecuteStepActions(_workflowStepDeclaration,
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
