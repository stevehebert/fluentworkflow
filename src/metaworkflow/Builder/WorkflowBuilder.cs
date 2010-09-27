using System.Collections.Generic;

namespace metaworkflow.core.Builder
{
    public interface IWorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        /// <summary>
        /// declare a step for a workflow and state
        /// </summary>
        /// <param name="workflow">The workflow step.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        StateStepConfiguration<TState, TTrigger, TTriggerContext> ForWorkflow(TWorkflow workflow, TState state);
    }

    /// <summary>
    /// The workflow builder used to configure state steps and action steps
    /// </summary>
    /// <typeparam name="TWorkflow">The type of the workflow.</typeparam>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TTrigger">The type of the trigger.</typeparam>
    /// <typeparam name="TTriggerContext">The type of the trigger context.</typeparam>
    public class WorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext> : IWorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        private readonly IDictionary<TWorkflow, StateStepConfiguration<TState, TTrigger, TTriggerContext>> _workflowConfiguration =
            new Dictionary<TWorkflow, StateStepConfiguration<TState, TTrigger, TTriggerContext>>();

        /// <summary>
        /// declare a workflow state configuration
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public StateStepConfiguration<TState, TTrigger, TTriggerContext> ForWorkflow(TWorkflow workflow, TState state)
        {
            StateStepConfiguration<TState, TTrigger, TTriggerContext> stateStepConfiguration = null;

            if (!_workflowConfiguration.TryGetValue(workflow, out stateStepConfiguration))
            {
                stateStepConfiguration = new StateStepConfiguration<TState, TTrigger, TTriggerContext>(state);
                _workflowConfiguration.Add(workflow, stateStepConfiguration);
            }

            return stateStepConfiguration;
        }
    }
}
