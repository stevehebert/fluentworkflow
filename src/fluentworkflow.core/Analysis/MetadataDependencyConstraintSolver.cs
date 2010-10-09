using System;
using System.Collections.Generic;
using System.Linq;
using fluentworkflow.core.Configuration;

namespace fluentworkflow.core.Analysis
{
    /// <summary>
    /// This constraint solver is responsible for validating relationships between state steps
    /// and resolving the execution order
    /// </summary>
    public class MetadataDependencyConstraintSolver
    {
        /// <summary>
        /// Analyzes the specified type registrations.
        /// </summary>
        /// <typeparam name="TWorkflow">The type of the workflow.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="typeRegistrations">The type registrations.</param>
        /// <returns></returns>
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


            // next we'll reset the priority ordering equal on all state steps
            foreach (var item in from p in typeRegistrations
                                 from q in p.Value.StateActionInfos
                                 select q)
                item.Priority = 0;


            var pass = 0;
            var processedItems = 0;

            // next we'll grab the items that have dependencies as our starting point
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

                // here we increment order on references whose parents have the same value
                foreach (var item in targetItems.Where(p => p.q1.Priority >= p.q.Priority ))
                {
                    item.q.Priority = pass + 1;
                    processedItems++;
                }

                // the nature of this algorithm means that two passes with the same number
                // of processed items indicate cyclical reference points. in which
                // case we end processing after enumerating the errors.
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

                // increment our pass and select the subset of records that have been progressed in the current round
                var localPass = ++ pass;
                targetItems = (from p in targetItems where p.q.Priority == localPass select p).ToList();
            } while (processedItems > 0);

        }
    }
}
