using System.Collections.Generic;
using System.Linq;

namespace fluentworkflow.core.Configuration
{
    public class WorkflowStepDeclaration<TWorkflow, TState, TTrigger>
    {
        public WorkflowStepDeclaration(TWorkflow workflow, TState state, IEnumerable<KeyValuePair<TTrigger, TState>> permittedActions)
        {
            Workflow = workflow;
            State = state;

            PermittedActions = from p in permittedActions
                               select new StateTriggerAction<TState, TTrigger>(p.Key, p.Value);
        }

        public TWorkflow Workflow { get; private set; }

        public TState State { get; private set; }

        public IEnumerable<StateTriggerAction<TState, TTrigger>> PermittedActions { get; private set; }
    }
}
