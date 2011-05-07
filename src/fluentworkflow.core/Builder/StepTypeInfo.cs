using System;

namespace fluentworkflow.core.Builder
{
    public class StepTypeInfo<TWorkflow, TState>
    {
        /// <summary>
        /// Gets or sets the workflow.
        /// </summary>
        /// <value>The workflow.</value>
        public TWorkflow Workflow { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public TState State { get; set; }

        /// <summary>
        /// Gets or sets the type of the state step.
        /// </summary>
        /// <value>The type of the state step.</value>
        public Type StateStepType { get; set; }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>The type of the action.</value>
        public WorkflowTaskActionType ActionType { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>The priority.</value>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the dependency.
        /// </summary>
        /// <value>The dependency.</value>
        public Type Dependency { get; set; }
    }
}