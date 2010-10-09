using System;
using System.Collections.Generic;
using System.Linq;
using fluentworkflow.core.Configuration;

namespace fluentworkflow.core.Analysis
{
    public class MetadataDependencyConstraintSolver
    {
        public IEnumerable<StateStepDependencyError<TWorkflow, TState>> Analyze<TWorkflow, TState>(IDictionary<Type, IStateActionMetadata<TWorkflow, TState>> typeRegistrations)
        {
            // first, we'll verify that each workflow, state, action type is
            // a fully closed set
            var dependentItems = from p in typeRegistrations
                                 from q in p.Value.StateActionInfos
                                 where q.Dependency != null
                                 select new {q, p.Key};

            foreach (var missingItem in from p in dependentItems
                                        where !typeRegistrations.Any(q => q.Key == p.q.Dependency)
                                          || !typeRegistrations.Any(a => a.Key == p.q.Dependency 
                                                        && a.Value.StateActionInfos.Any(b => b.State.Equals(p.q.State) 
                                                        && b.Workflow.Equals(p.q.Workflow) 
                                                        && b.WorkflowStepActionType == p.q.WorkflowStepActionType))
                                        select new StateStepDependencyError<TWorkflow, TState>
                                                   {
                                                       ErrorReason = StateDependencyErrorReason.UnknownDependency,
                                                       Dependency = p.q.Dependency,
                                                       State = p.q.State,
                                                       Step = p.Key,
                                                       Workflow = p.q.Workflow
                                                   })
                yield return missingItem;

                           

           
                        
            var errorList = new List<StateStepDependencyError<TWorkflow, TState>>();



            var targetWorkflows = from p in typeRegistrations
                                  from q in p.Value.StateActionInfos
                                  group q by q.Workflow
                                      into g
                                      select g.Key;

            foreach (var workflow in targetWorkflows)
            {
                var targetStates = from p in typeRegistrations.Values
                                   from q in p.StateActionInfos
                                   group q by q.State
                                       into g
                                       select g.Key;

                foreach (var state in targetStates)
                {
                    var targetActionTypes = from p in typeRegistrations.Values
                                            from q in p.StateActionInfos
                                            group q by q.WorkflowStepActionType
                                                into g
                                                select g.Key;

                    foreach (var actionType in targetActionTypes)
                    {
                        TState state1 = state;
                        TWorkflow workflow1 = workflow;
                        WorkflowStepActionType type = actionType;
                        errorList.AddRange(Evaluate(from p in typeRegistrations
                                                    from q in p.Value.StateActionInfos
                                                    where q.Workflow.Equals(workflow1)
                                                          && q.State.Equals(state1)
                                                          && q.WorkflowStepActionType == type
                                                    select
                                                        new KeyValuePair<Type, StateActionInfo<TWorkflow, TState>>(
                                                        p.Key, q)));
                    }
                }
            }

            foreach (var item in errorList)
                yield return item;
            
        }

        public IEnumerable<StateStepDependencyError<TWorkflow, TState>> Evaluate<TWorkflow, TState>(IEnumerable<KeyValuePair<Type, StateActionInfo<TWorkflow, TState>>> actionSet)
        {
            var i = 0;
            var processedItems = 0;
            var previouslyProcessedItems = -1;
            do
            {
                var items = from p in actionSet
                            join q in actionSet on p.Key equals q.Value.Dependency
                            where p.Value.Priority >= i
                            select q;

                foreach (var item in items)
                    item.Value.Priority = i+1;

                processedItems = items.Count();

                if (processedItems == previouslyProcessedItems)
                    return from p in items
                           select
                               new StateStepDependencyError<TWorkflow, TState>
                                   {
                                       Step = p.Key,
                                       Dependency = p.Value.Dependency,
                                       State = p.Value.State,
                                       Workflow = p.Value.Workflow,
                                       ErrorReason = StateDependencyErrorReason.ParticipatesInCyclicalReference
                                   };

                previouslyProcessedItems = processedItems;

                i++;
            } while (processedItems > 0);

            return new StateStepDependencyError<TWorkflow, TState>[0];
        }

    }
}
