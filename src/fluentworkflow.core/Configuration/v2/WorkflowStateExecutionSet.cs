using System;
using System.Collections.Generic;

namespace fluentworkflow.core.Configuration.v2
{
    public class WorkflowStateIndex<TWorkflow, TState>
    {
        public class EqualityComparer : IEqualityComparer<WorkflowStateIndex<TWorkflow, TState>>
        {
            /// <summary>
            /// verifies equality between instances
            /// </summary>
            /// <param name="x">first instance</param>
            /// <param name="y">second instance</param>
            /// <returns>true if functionally equal</returns>
            public bool Equals(WorkflowStateIndex<TWorkflow, TState> x, WorkflowStateIndex<TWorkflow, TState> y)
            {
                return GetHashCode(x) == GetHashCode(y);
            }

            public int GetHashCode(WorkflowStateIndex<TWorkflow, TState> obj)
            {
                return obj.GetHashCode();
            }
        }


        /// <summary>
        /// Gets or sets the workflow.
        /// </summary>
        /// <value>The workflow.</value>
        public TWorkflow Workflow { get; private set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public TState State { get; private set; }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>The type of the action.</value>
        public WorkflowTaskActionType ActionType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowStateIndex&lt;TWorkflow, TState&gt;"/> class.
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <param name="state">The state.</param>
        /// <param name="actionType">Type of the action.</param>
        public WorkflowStateIndex(TWorkflow workflow, TState state, WorkflowTaskActionType actionType)
        {
            Workflow = workflow;
            State = state;
            ActionType = actionType;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return string.Concat(Workflow.ToString(), ":", State.ToString(), ":", ActionType.ToString()).GetHashCode();
        }
    }


    public class WorkflowStateExecutionSet<TWorkflow, TState> 
    {
        /// <summary>
        /// Gets or sets the index of the workflow state.
        /// </summary>
        /// <value>The index of the workflow state.</value>
        public WorkflowStateIndex<TWorkflow, TState> WorkflowStateIndex { get; private set; }

        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        /// <value>The types.</value>
        public IEnumerable<Type> Types { get; private set; }

        public WorkflowStateExecutionSet(TWorkflow workflow, TState state, WorkflowTaskActionType actionType, IEnumerable<Type> types)
        {
            WorkflowStateIndex = new WorkflowStateIndex<TWorkflow, TState>(workflow, state, actionType);
            Types = types;
        }
    }
}
