namespace metaworkflow.core.Configuration
{
    public class StateActionInfo<TWorkflow, TState>
    {
        /// <summary>
        /// Gets the workflow value.
        /// </summary>
        /// <value>The workflow value.</value>
        public TWorkflow Workflow { get; private set; }

        /// <summary>
        /// Gets the state value.
        /// </summary>
        /// <value>The state value.</value>
        public TState State { get; private set; }

        /// <summary>
        /// Gets workflow step action value.
        /// </summary>
        /// <value>The workflow step action value.</value>
        public WorkflowStepActionType WorkflowStepActionType { get; private set; }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        /// <value>The priority.</value>
        public StepPriority Priority { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateActionInfo&lt;TWorkflow, TState&gt;"/> class.
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <param name="state">The state.</param>
        /// <param name="workflowStepActionType">Type of the workflow step action.</param>
        /// <param name="stepPriority">execution priority of the step</param>
        public StateActionInfo(TWorkflow workflow, TState state, WorkflowStepActionType workflowStepActionType, StepPriority stepPriority)
        {
            Workflow = workflow;
            State = state;
            WorkflowStepActionType = workflowStepActionType;
            Priority = stepPriority;
        }
    }
}
