using System.Linq;
using System.Collections.Generic;
using fluentworkflow.core.Analysis;
using fluentworkflow.core.Configuration;
using fluentworkflow.core.Configuration.v2;

namespace fluentworkflow.core.Builder
{
    /// <summary>
    /// The workflow builder used to configure state steps and action steps
    /// </summary>
    /// <typeparam name="TWorkflow">The type of the workflow.</typeparam>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TTrigger">The type of the trigger.</typeparam>
    /// <typeparam name="TTriggerContext">The type of the trigger context.</typeparam>
    public class WorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext> : IWorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        private readonly IDictionary<TWorkflow, IList<StateTaskConfiguration<TState, TTrigger, TTriggerContext>>> _workflowConfiguration =
            new Dictionary<TWorkflow, IList<StateTaskConfiguration<TState, TTrigger, TTriggerContext>>>();

        public RegistrationInfo<TWorkflow, TState, TTrigger, TTriggerContext> Compile()
        {
            var declarations = ProduceStepDeclarations();

            var errors = new ClosureAnalyzer().ValidateStepDeclarationClosure(declarations);

            if (errors.Any())
                throw new ClosureAnalysisException<TWorkflow, TState, TTrigger>(errors);

            var typeRoles = ProduceTypeRoles();

            var uniqueTypes = from p in typeRoles
                              group p by p.StateStepType
                                  into g
                                  select g.Key;

            var universe = new WorkflowExecutionUniverse<TWorkflow, TState, TTriggerContext>();

            if (uniqueTypes.Any())
            {
                var items = from p in typeRoles
                            group p by new { p.Workflow, p.State, p.ActionType }
                                into p1
                                select
                                    new
                                    {
                                        p1.Key.Workflow,
                                        p1.Key.State,
                                        p1.Key.ActionType,
                                        Types = from s in p1 orderby s.Priority select s.StateStepType
                                    };

                foreach (var item in items)
                    universe.Add(new WorkflowStateExecutionSet<TWorkflow, TState>(item.Workflow, item.State,
                                                                                  item.ActionType, item.Types));
            }

            return new RegistrationInfo<TWorkflow, TState, TTrigger, TTriggerContext>(universe, uniqueTypes,
                                                                                      declarations);

        }


        public IEnumerable<WorkflowStepDeclaration<TWorkflow, TState, TTrigger>> ProduceStepDeclarations()
        {
            return from p in _workflowConfiguration
                   from q in p.Value
                        select
                            new WorkflowStepDeclaration<TWorkflow, TState, TTrigger>(p.Key, q.State,
                                                                                     q.PermittedTriggers);
        }


        public IEnumerable<StepTypeInfo<TWorkflow, TState>> ProduceTypeRoles()
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
        public StateTaskConfiguration<TState, TTrigger, TTriggerContext> ForWorkflow(TWorkflow workflow, TState state)
        {
            IList<StateTaskConfiguration<TState, TTrigger, TTriggerContext>> stateStepConfigurationList = null;

            if (!_workflowConfiguration.TryGetValue(workflow, out stateStepConfigurationList))
            {
                stateStepConfigurationList = new List<StateTaskConfiguration<TState, TTrigger, TTriggerContext>>();
                _workflowConfiguration.Add(workflow, stateStepConfigurationList);
            }
            var configuration = new StateTaskConfiguration<TState, TTrigger, TTriggerContext>(state);
            stateStepConfigurationList.Add(configuration);
            return configuration;
        }
    }
}
