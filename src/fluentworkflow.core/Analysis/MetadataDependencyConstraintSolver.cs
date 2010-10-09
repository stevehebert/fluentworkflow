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

            var missingMatch = 0;
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
            {
                missingMatch += 1;
                yield return missingItem;
            }

            if (missingMatch > 0)
                yield break;

            foreach (var item in from p in typeRegistrations
                                 from q in p.Value.StateActionInfos
                                 select q)
                item.Priority = 0;


            var pass = 0;
            var processedItems = 0;

            var targetItems = (from p in typeRegistrations
                              from q in p.Value.StateActionInfos
                              from p1 in typeRegistrations
                              from q1 in p1.Value.StateActionInfos
                              where q.Dependency == p1.Key
                                    && q.Workflow.Equals(q1.Workflow)
                                    && q.State.Equals(q1.State)
                                    && q.WorkflowStepActionType == q1.WorkflowStepActionType
                              select new {q, q1, p.Key}).ToList();
                


            do
            {
                var previousProcessedItems = processedItems;
                processedItems = 0;

                foreach (var item in targetItems.Where(p => p.q1.Priority >= p.q.Priority ))
                {
                    item.q.Priority = pass + 1;
                    processedItems++;
                }

                if( processedItems == previousProcessedItems)
                {
                    foreach (var item in targetItems)
                        yield return
                            new StateStepDependencyError<TWorkflow, TState>
                            {
                                Workflow = item.q.Workflow,
                                State = item.q.State,
                                Dependency = item.q.Dependency,
                                Step = item.Key,
                                ErrorReason = StateDependencyErrorReason.ParticipatesInCyclicalReference
                            };
                    break;
                    
                }

                var localPass = ++ pass;

                targetItems = (from p in targetItems where p.q.Priority == localPass select p).ToList();
            } while (processedItems > 0);

        }
    }
}
