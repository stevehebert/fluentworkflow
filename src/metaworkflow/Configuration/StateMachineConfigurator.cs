using System.Collections.Generic;
using System.Linq;
using Stateless;

namespace fluentworkflow.core.Configuration
{
    public interface IStateMachineConfigurator<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        StateMachine<TState, TTrigger> CreateStateMachine(TWorkflow workflow, TState state);
    }

    public class StateMachineConfigurator<TWorkflow, TState, TTrigger, TTriggerContext> : IStateMachineConfigurator<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        private readonly IEnumerable<WorkflowStepAdapter<TWorkflow, TState, TTrigger, TTriggerContext>>
            _configurationAdapters;

        public StateMachineConfigurator(IEnumerable<WorkflowStepAdapter<TWorkflow, TState, TTrigger, TTriggerContext>> configurationAdapters)
        {
            _configurationAdapters = configurationAdapters;
        }

        public StateMachine<TState, TTrigger> CreateStateMachine(TWorkflow workflow, TState state)
        {
            var stateMachine = new StateMachine<TState, TTrigger>(state);
            foreach( var adapter in _configurationAdapters.Where(p => p.Workflow.Equals(workflow)))
                adapter.ConfigureStep(stateMachine);

            return stateMachine;
        }
    }
}
