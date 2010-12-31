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
            IEnumerable<Lazy<IStateTask<TState, TTrigger, TTriggerContext>, IStateActionMetadata<TWorkflow, TState>>> _stateSteps;

        public StateStepDispatcher(IEnumerable<Lazy<IStateTask<TState, TTrigger, TTriggerContext>, IStateActionMetadata<TWorkflow, TState>>> stateSteps)
        {
            _stateSteps = stateSteps;
        }


        public void ExecuteEntryStepActions(WorkflowStepDeclaration<TWorkflow, TState, TTrigger> stepDeclaration,
                                       TTriggerContext triggerContext,
                                       StateMachine<TState, TTrigger>.Transition transition,
                                       StateMachine<TState, TTrigger> stateMachine)
        {
            var items = (from p in _stateSteps
                         from q in p.Metadata.StateActionInfos
                         where
                             (q.Workflow.Equals(stepDeclaration.Workflow) && q.State.Equals(stepDeclaration.State) &&
                             q.WorkflowTaskActionType == WorkflowTaskActionType.Entry)
                         orderby q.Priority
                         select p.Value);

            if (!items.Any())
                return;

            var flowMutator = new FlowMutator<TTrigger, TTriggerContext>(triggerContext);
            var stateStepInfo = new EntryStateTaskInfo<TState, TTrigger, TTriggerContext>(triggerContext,
                                                                                     transition);

            foreach (var item in items)
            {

                Dispatch(item, stateStepInfo, flowMutator);

                if (!flowMutator.IsSet)
                    continue;

                stateMachine.Fire(flowMutator.Trigger, flowMutator.TriggerContext);
                break;
            }
        }

        public void ExecuteExitStepActions(WorkflowStepDeclaration<TWorkflow, TState, TTrigger> stepDeclaration, TTriggerContext triggerContext, StateMachine<TState, TTrigger>.Transition transition, StateMachine<TState, TTrigger> stateMachine)
        {
            var items = (from p in _stateSteps
                         from q in p.Metadata.StateActionInfos
                         where
                             (q.Workflow.Equals(stepDeclaration.Workflow) && q.State.Equals(stepDeclaration.State) &&
                             q.WorkflowTaskActionType == WorkflowTaskActionType.Exit)
                         orderby q.Priority
                         select p.Value);

            if (!items.Any())
                return;

            var flowMutator = new FlowMutator<TTrigger, TTriggerContext>(triggerContext);
            var stateStepInfo = new ExitStateTaskInfo<TState, TTrigger, TTriggerContext>(triggerContext,
                                                                                         transition);

            foreach (var item in items)
            {

                Dispatch(item, stateStepInfo);

                if (!flowMutator.IsSet)
                    continue;

                stateMachine.Fire(flowMutator.Trigger, flowMutator.TriggerContext);
                break;
            }

        }

        private void Dispatch(IStateTask<TState, TTrigger, TTriggerContext> stateTask,
                               ExitStateTaskInfo<TState, TTrigger, TTriggerContext> entryStateTaskInfo)
        {
            var exitStep = stateTask as IExitStateTask<TState, TTrigger, TTriggerContext>;

            if (exitStep == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                                                                  "Internal error mismatch on entry type with state step {0}",
                                                                  stateTask.GetType()));

            exitStep.Execute(entryStateTaskInfo);
        }

        private void Dispatch(IStateTask<TState, TTrigger, TTriggerContext> stateTask,
                               EntryStateTaskInfo<TState, TTrigger, TTriggerContext> entryStateTaskInfo,
                               IFlowMutator<TTrigger, TTriggerContext> flowMutator)
        {
            var mutatingStateStep = stateTask as IMutatingEntryStateTask<TState, TTrigger, TTriggerContext>;

            if (mutatingStateStep != null)
            {
                mutatingStateStep.Execute(entryStateTaskInfo, flowMutator);
                return;
            }

            var entryStateStep = stateTask as IEntryStateTask<TState, TTrigger, TTriggerContext>;

            if (entryStateStep == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                                                                  "Internal error mismatch on entry type with state step {0}",
                                                                  stateTask.GetType()));

            entryStateStep.Execute(entryStateTaskInfo);

        }
    }
}
