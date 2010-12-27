using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using fluentworkflow.core.Builder;
using Stateless;

namespace fluentworkflow.core.Configuration
{
    public class StateStepDispatcher<TWorkflow, TState, TTrigger, TTriggerContext> : IStateStepDispatcher<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        private readonly
            IEnumerable<Lazy<IStateStep<TState, TTrigger, TTriggerContext>, IStateActionMetadata<TWorkflow, TState>>> _stateSteps;

        public StateStepDispatcher(IEnumerable<Lazy<IStateStep<TState, TTrigger, TTriggerContext>, IStateActionMetadata<TWorkflow, TState>>> stateSteps)
        {
            _stateSteps = stateSteps;
        }


        public void ExecuteStepActions(WorkflowStepDeclaration<TWorkflow, TState, TTrigger> stepDeclaration,
                                       TTriggerContext triggerContext,
                                       StateMachine<TState, TTrigger>.Transition transition,
                                       StateMachine<TState, TTrigger> stateMachine,
                                       WorkflowStepActionType workflowStepActionType)
        {
            var items = (from p in _stateSteps
                         from q in p.Metadata.StateActionInfos
                         where
                             (q.Workflow.Equals(stepDeclaration.Workflow) && q.State.Equals(stepDeclaration.State) &&
                             q.WorkflowStepActionType == workflowStepActionType)
                         orderby q.Priority
                         select p.Value);

            if (!items.Any())
                return;

            var flowMutator = new FlowMutator<TTrigger, TTriggerContext>(triggerContext);
            var stateStepInfo = new EntryStateStepInfo<TState, TTrigger, TTriggerContext>(triggerContext, 
                                                                                     transition);
            
            foreach(var item in items)
            {
                
                Dispatch(item, stateStepInfo, flowMutator, workflowStepActionType);

                if (!flowMutator.IsSet)
                    continue;

                stateMachine.Fire(flowMutator.Trigger, flowMutator.TriggerContext);
                break;
            }
        }

        private void Dispatch( IStateStep<TState, TTrigger, TTriggerContext> stateStep, 
                               EntryStateStepInfo<TState, TTrigger, TTriggerContext> entryStateStepInfo, 
                               IFlowMutator<TTrigger, TTriggerContext> flowMutator,
                               WorkflowStepActionType actionType)
        {
            if( actionType == WorkflowStepActionType.Entry)
            {
                var mutatingStateStep = stateStep as IMutatingEntryStateStep<TState, TTrigger, TTriggerContext>;

                if (mutatingStateStep != null)
                {
                    mutatingStateStep.Execute(entryStateStepInfo, flowMutator);
                    return;
                }

                var entryStateStep = stateStep as IEntryStateStep<TState, TTrigger, TTriggerContext>;

                if (entryStateStep == null)
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                                                                      "Internal error mismatch on entry type with state step {0}",
                                                                      stateStep.GetType()));

                entryStateStep.Execute(entryStateStepInfo);

                return;
            }

            var exitStep = stateStep as IExitStateStep<TState, TTrigger, TTriggerContext>;

            if(exitStep == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                                                                  "Internal error mismatch on entry type with state step {0}",
                                                                  stateStep.GetType()));

            exitStep.Execute(entryStateStepInfo);
        }
    }
}
