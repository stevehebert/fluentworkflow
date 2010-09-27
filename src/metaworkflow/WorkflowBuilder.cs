using System;
using System.Collections.Generic;
using metaworkflow.core.Builder;

namespace metaworkflow.core
{
    public enum WorkflowStepActionType
    {
        Entry,
        Exit
    }
    public enum StepPriority
    {
        Highest = Int32.MaxValue,
        Medium = 0,
        Lowest = Int32.MinValue,
    }



    public interface IWorkflowBuilder <TWorkflow, TState, TTrigger, TTriggerContext>
    {
        StateStepConfiguration<TState, TTrigger, TTriggerContext> ForWorkflow(TWorkflow workflow, TState state);
    }

    public class WorkflowBuilder <TWorkflow, TState, TTrigger, TTriggerContext> : IWorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        private readonly IDictionary<TWorkflow, StateStepConfiguration<TState, TTrigger, TTriggerContext>> _workflowConfiguration =
            new Dictionary<TWorkflow, StateStepConfiguration<TState, TTrigger, TTriggerContext>>();

        public StateStepConfiguration<TState, TTrigger, TTriggerContext> ForWorkflow(TWorkflow workflow, TState state)
        {
            StateStepConfiguration<TState, TTrigger, TTriggerContext> stateStepConfiguration = null;

            if(!_workflowConfiguration.TryGetValue(workflow, out stateStepConfiguration))
            {
                stateStepConfiguration = new StateStepConfiguration<TState, TTrigger, TTriggerContext>(state);
                _workflowConfiguration.Add(workflow, stateStepConfiguration);
            }

            return stateStepConfiguration;
        }
    }
}
