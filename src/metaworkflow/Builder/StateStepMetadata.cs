using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace metaworkflow.core.Builder
{
    /// <summary>
    /// State Step info used for metadata composition
    /// </summary>
    public class StateStepMetadata
    {
        /// <summary>
        /// Gets the type of the state step.
        /// </summary>
        /// <value>The type of the state step.</value>
        public Type StateStepType { get; private set; }

        /// <summary>
        /// Gets the step priority.
        /// </summary>
        /// <value>The priority.</value>
        public int Priority { get; private set; }

        /// <summary>
        /// Gets the type of the action.
        /// </summary>
        /// <value>The type of the action.</value>
        public WorkflowStepActionType ActionType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateStepMetadata"/> class.
        /// </summary>
        /// <param name="stateStepType">Type of the state step.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="actionType">Type of the action.</param>
        public StateStepMetadata(Type stateStepType, int priority, WorkflowStepActionType actionType)
        {
            StateStepType = stateStepType;
            Priority = priority;
            ActionType = actionType;
        }

        private IList<Type> _dependencies = new List<Type>();
        public IEnumerable<Type> Dependencies { get { return _dependencies; } }

        public void AddDependency(Type type)
        {
            _dependencies.Add(type);
        }
    }
}
