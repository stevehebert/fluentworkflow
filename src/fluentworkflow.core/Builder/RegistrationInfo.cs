using System;
using System.Collections.Generic;
using fluentworkflow.core.Configuration;
using fluentworkflow.core.Configuration.v2;

namespace fluentworkflow.core.Builder
{
    public class RegistrationInfo<TWorkflow, TState, TTrigger, TWorkflowContext>
    {
        /// <summary>
        /// Gets or sets the workflow execution universe.
        /// </summary>
        /// <value>The workflow execution universe.</value>
        public WorkflowExecutionUniverse<TWorkflow, TState, TWorkflowContext> WorkflowExecutionUniverse { get; private set; }

        /// <summary>
        /// Gets or sets the transient types.
        /// </summary>
        /// <value>The transient types.</value>
        public IEnumerable<Type> TransientTypes { get; private set; }

        /// <summary>
        /// Gets or sets the workflow step declarations.
        /// </summary>
        /// <value>The workflow step declarations.</value>
        public IEnumerable<WorkflowStepDeclaration<TWorkflow, TState, TTrigger>>  WorkflowStepDeclarations { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationInfo&lt;TWorkflow, TState, TTrigger, TWorkflowContext&gt;"/> class.
        /// </summary>
        /// <param name="workflowExecutionUniverse">The workflow execution universe.</param>
        /// <param name="transientTypes">The transient types.</param>
        /// <param name="workflowStepDeclarations">The workflow step declarations.</param>
        public RegistrationInfo(WorkflowExecutionUniverse<TWorkflow, TState, TWorkflowContext> workflowExecutionUniverse,
                                IEnumerable<Type> transientTypes, IEnumerable<WorkflowStepDeclaration<TWorkflow, TState, TTrigger>> workflowStepDeclarations) {

            WorkflowExecutionUniverse = workflowExecutionUniverse;
            TransientTypes = transientTypes;
            WorkflowStepDeclarations = workflowStepDeclarations;
        }

    }
}