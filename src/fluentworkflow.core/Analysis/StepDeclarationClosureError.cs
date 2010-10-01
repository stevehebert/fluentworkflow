namespace fluentworkflow.core.Analysis
{
    public class StepDeclarationClosureError<TWorkflow, TState, TTrigger>
    {
        /// <summary>
        /// Gets the workflow.
        /// </summary>
        /// <value>The workflow.</value>
        public TWorkflow Workflow { get; private set; }

        /// <summary>
        /// Gets the state declaring the errant trigger.
        /// </summary>
        /// <value>The state of the declaring.</value>
        public TState DeclaringState { get; private set; }

        /// <summary>
        /// Gets the errant declaring trigger.
        /// </summary>
        /// <value>The declaring trigger.</value>
        public TTrigger DeclaringTrigger { get; private set; }

        /// <summary>
        /// Gets destination state that is undefined in the workflow
        /// </summary>
        /// <value>The destination state.</value>
        public TState DestinationState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StepDeclarationClosureError&lt;TWorkflow, TState, TTrigger&gt;"/> class.
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <param name="declaringState">State of the declaring.</param>
        /// <param name="declaringTrigger">The declaring trigger.</param>
        /// <param name="destinationState">State of the destination.</param>
        public StepDeclarationClosureError(TWorkflow workflow, TState declaringState, TTrigger declaringTrigger, TState destinationState)
        {
            Workflow = workflow;
            DeclaringState = declaringState;
            DeclaringTrigger = declaringTrigger;
            DestinationState = destinationState;
        }
    }
}