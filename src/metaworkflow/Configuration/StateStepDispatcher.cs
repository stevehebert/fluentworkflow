﻿using System;
using System.Collections.Generic;
using System.Linq;
using metaworkflow.core.Builder;
using Stateless;

namespace metaworkflow.core.Configuration
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
                         orderby q.Priority descending
                         select p.Value);

            if (!items.Any())
                return;

            var triggerTrip = new TriggerTrip<TTrigger, TTriggerContext>();
            var stateStepInfo = new StateStepInfo<TState, TTrigger, TTriggerContext>(triggerContext, 
                                                                                     transition,
                                                                                     triggerTrip);
            foreach(var item in items)
            {
                item.Execute(stateStepInfo);

                if (!triggerTrip.IsSet)
                    continue;

                stateMachine.Fire(triggerTrip.Trigger, triggerTrip.TriggerContext);
                break;
            }
        }
    }
}