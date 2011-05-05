using System;
using System.Collections.Generic;

namespace fluentworkflow.core.Configuration.v2
{
    public class WorkflowExecutionUniverse<TWorkflow, TState, TWorkflowContext>
    {
        private readonly IDictionary<WorkflowStateIndex<TWorkflow, TState>, IEnumerable<Type>> _stateExecutionSets;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowExecutionUniverse&lt;TWorkflow, TState, TWorkflowContext&gt;"/> class.
        /// </summary>
        public WorkflowExecutionUniverse()
        {
            _stateExecutionSets = new Dictionary<WorkflowStateIndex<TWorkflow, TState>, IEnumerable<Type>>(new WorkflowStateIndex<TWorkflow, TState>.EqualityComparer());
        }

        /// <summary>
        /// Adds the specified set.
        /// </summary>
        /// <param name="set">The set.</param>
        public void Add(WorkflowStateExecutionSet<TWorkflow, TState> set)
        {
            _stateExecutionSets.Add(set.WorkflowStateIndex, set.Types);
        }

        /// <summary>
        /// Retrieves the specified workflow.
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <param name="state">The state.</param>
        /// <param name="actionType">Type of the action.</param>
        /// <returns></returns>
        public IEnumerable<Type> Retrieve(TWorkflow workflow, TState state, WorkflowTaskActionType actionType)
        {
            IEnumerable<Type> types;

            try
            {
                types = _stateExecutionSets[new WorkflowStateIndex<TWorkflow, TState>(workflow, state, actionType)];
            }
            catch (KeyNotFoundException)
            {
                yield break;
            }
            
            foreach( var item in types)
                yield return item;
        }
    }
}
