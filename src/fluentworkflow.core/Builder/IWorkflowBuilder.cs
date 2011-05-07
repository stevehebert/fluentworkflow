namespace fluentworkflow.core.Builder
{
    public interface IWorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        /// <summary>
        /// declare a step for a workflow and state
        /// </summary>
        /// <param name="workflow">The workflow step.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        StateTaskConfiguration<TState, TTrigger, TTriggerContext> ForWorkflow(TWorkflow workflow, TState state);
    }
}