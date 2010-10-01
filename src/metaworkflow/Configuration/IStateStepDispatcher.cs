using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stateless;

namespace fluentworkflow.core.Configuration
{

    public interface IStateStepDispatcher<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        void ExecuteStepActions(WorkflowStepDeclaration<TWorkflow, TState, TTrigger> stepDeclaration,
                                TTriggerContext triggerContext,
                                StateMachine<TState, TTrigger>.Transition transition,
                                StateMachine<TState, TTrigger> stateMachine,
                                WorkflowStepActionType workflowStepActionType);
    }
}
