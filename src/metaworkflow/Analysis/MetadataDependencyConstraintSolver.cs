using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using metaworkflow.core.Configuration;

namespace metaworkflow.core.Analysis
{
    public class MetadataDependencyConstraintSolver
    {
        public IEnumerable<StateStepDependencyError<TWorkflow, TState>> Analyze<TWorkflow, TState>(IDictionary<Type, IStateActionMetadata<TWorkflow, TState>> typeRegistrations)
        {
            var errorList = new List<StateStepDependencyError<TWorkflow, TState>>();

            var targetWorkflows = from p in typeRegistrations
                                  from q in p.Value.StateActionInfos
                                  group q by q.Workflow
                                  into g
                                  select g.Key;

            foreach( var workflow in targetWorkflows)
            {
                var targetStates = from p in typeRegistrations.Values
                                   from q in p.StateActionInfos
                                   group q by q.State
                                   into g
                                   select g.Key;

                foreach( var state in targetStates )
                {
                    var targetActionTypes = from p in typeRegistrations.Values
                                            from q in p.StateActionInfos
                                            group q by q.WorkflowStepActionType
                                            into g
                                            select g.Key;

                    foreach (var actionType in targetActionTypes)
                        errorList.AddRange(Evaluate(from p in typeRegistrations
                                                    from q in p.Value.StateActionInfos
                                                    where q.Workflow.Equals(workflow)
                                                          && q.State.Equals(state)
                                                          && q.WorkflowStepActionType == actionType
                                                    select
                                                        new KeyValuePair<Type, StateActionInfo<TWorkflow, TState>>(
                                                        p.Key, q)));

                        
                }
            }

            return errorList;

        }

        public IEnumerable<StateStepDependencyError<TWorkflow, TState>> Evaluate<TWorkflow, TState>(IEnumerable<KeyValuePair<Type, StateActionInfo<TWorkflow, TState>>> actionSet)
        {

            var dependencies = from p in actionSet select new {p.Value.Dependency, p.Key, p.Value.Workflow, p.Value.State};
            var hostTypes = from p in actionSet select p.Key;

            var unmatchedDendencies = from p in dependencies
                                      where p.Dependency != null
                                      where !hostTypes.Contains(p.Dependency)
                                      select p;

            
            foreach (var unmatched in unmatchedDendencies)
                yield return
                    new StateStepDependencyError<TWorkflow, TState>
                        {
                            Workflow = unmatched.Workflow,
                            State = unmatched.State,
                            Step = unmatched.Key,
                            Dependency = unmatched.Dependency
                        };
        }
    }
}
