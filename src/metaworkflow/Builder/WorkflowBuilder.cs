using System;
using System.Linq;
using System.Collections.Generic;
using fluentworkflow.core.Configuration;

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
        StateStepConfiguration<TState, TTrigger, TTriggerContext> ForWorkflow(TWorkflow workflow, TState state);
    }

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
        public WorkflowStepActionType ActionType { get; set; }

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

    /// <summary>
    /// The workflow builder used to configure state steps and action steps
    /// </summary>
    /// <typeparam name="TWorkflow">The type of the workflow.</typeparam>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TTrigger">The type of the trigger.</typeparam>
    /// <typeparam name="TTriggerContext">The type of the trigger context.</typeparam>
    public class WorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext> : IWorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        private readonly IDictionary<TWorkflow, IList<StateStepConfiguration<TState, TTrigger, TTriggerContext>>> _workflowConfiguration =
            new Dictionary<TWorkflow, IList<StateStepConfiguration<TState, TTrigger, TTriggerContext>>>();

        internal IEnumerable<WorkflowStepDeclaration<TWorkflow, TState, TTrigger>> ProduceStepDeclarations()
        {
            return from p in _workflowConfiguration
                   from q in p.Value
                        select
                            new WorkflowStepDeclaration<TWorkflow, TState, TTrigger>(p.Key, q.State,
                                                                                     q.PermittedTriggers);
        }


        internal IEnumerable<StepTypeInfo<TWorkflow, TState>> ProduceTypeRoles()
        {
            return from p in _workflowConfiguration
                   from q in p.Value
                   from r in q.StateStepInfos
                   select
                       new StepTypeInfo<TWorkflow, TState>
                           {
                               Workflow = p.Key,
                               State = q.State,
                               StateStepType = r.StateStepType,
                               ActionType = r.ActionType,
                               Priority = r.Priority,
                               Dependency = r.Dependency
                           };

        }

        /// <summary>
        /// declare a workflow state configuration
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public StateStepConfiguration<TState, TTrigger, TTriggerContext> ForWorkflow(TWorkflow workflow, TState state)
        {
            IList<StateStepConfiguration<TState, TTrigger, TTriggerContext>> stateStepConfigurationList = null;

            if (!_workflowConfiguration.TryGetValue(workflow, out stateStepConfigurationList))
            {
                stateStepConfigurationList = new List<StateStepConfiguration<TState, TTrigger, TTriggerContext>>();
                _workflowConfiguration.Add(workflow, stateStepConfigurationList);
            }
            var configuration = new StateStepConfiguration<TState, TTrigger, TTriggerContext>(state);
            stateStepConfigurationList.Add(configuration);
            return configuration;
        }
    }
}
