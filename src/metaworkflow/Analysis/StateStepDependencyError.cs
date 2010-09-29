using System;

namespace metaworkflow.core.Analysis
{
    public enum StateDependencyErrorReason
    {
        UnknownDependency
    }

    public class StateStepDependencyError<TWorkflow, TState>
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
        /// Gets or sets the step.
        /// </summary>
        /// <value>The step.</value>
        public Type Step { get; set; }

        /// <summary>
        /// Gets or sets the dependency.
        /// </summary>
        /// <value>The dependency.</value>
        public Type Dependency { get; set; }

        /// <summary>
        /// Gets or sets the error reason.
        /// </summary>
        /// <value>The error reason.</value>
        public StateDependencyErrorReason ErrorReason { get; set; }
    }
}
