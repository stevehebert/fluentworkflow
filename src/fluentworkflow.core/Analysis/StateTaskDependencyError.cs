using System;
using System.Globalization;

namespace fluentworkflow.core.Analysis
{
    public enum StateDependencyErrorReason
    {
        UnknownDependency,
        SelfReferencingDependency,
        ParticipatesInCyclicalReference
    }

    public class StateTaskDependencyError<TWorkflow, TState>
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

        public override string ToString()
        {
            return
                string.Format(CultureInfo.InvariantCulture,
                    "Error: {0} - StepType: {1} on Workflow {2}, State {3} declares a dependency on type {4}",
                    ErrorReason, Step.Name, Workflow, State, Dependency.Name);

        }
    }
}
