using System;
using System.Globalization;
using System.Linq;
using fluentworkflow.core.Builder;
using fluentworkflow.core.Configuration.v2;
using Stateless;

namespace fluentworkflow.core.Configuration
{
    public class StateStepDispatcher<TWorkflow, TState, TTrigger, TTriggerContext> : IStateStepDispatcher<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        private readonly WorkflowExecutionUniverse<TWorkflow, TState, TTriggerContext> _workflowExecutionUniverse;
        private readonly Func<Type, object> _resolver;

        public StateStepDispatcher(WorkflowExecutionUniverse<TWorkflow, TState, TTriggerContext> workflowExecutionUniverse, Func<Type, object> resolver)
        {
            _workflowExecutionUniverse = workflowExecutionUniverse;
            _resolver = resolver;
        }


        public void ExecuteEntryStepActions(WorkflowStepDeclaration<TWorkflow, TState, TTrigger> stepDeclaration,
                                       TTriggerContext triggerContext,
                                       StateMachine<TState, TTrigger>.Transition transition,
                                       StateMachine<TState, TTrigger> stateMachine)
        {
            var items =
                _workflowExecutionUniverse.Retrieve(stepDeclaration.Workflow, stepDeclaration.State,
                                                    WorkflowTaskActionType.Entry);

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
            var items =
                _workflowExecutionUniverse.Retrieve(stepDeclaration.Workflow, stepDeclaration.State,
                                                    WorkflowTaskActionType.Exit);

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

        private void Dispatch(Type stateTaskType,
                               ExitStateTaskInfo<TState, TTrigger, TTriggerContext> entryStateTaskInfo)
        {
            var exitStep = _resolver(stateTaskType) as IExitStateTask<TState, TTrigger, TTriggerContext>;

            if (exitStep == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                                                                  "Internal error mismatch on entry type with state step {0}",
                                                                  stateTaskType));

            exitStep.Execute(entryStateTaskInfo);
        }

        private void Dispatch(Type stateTaskType,
                               EntryStateTaskInfo<TState, TTrigger, TTriggerContext> entryStateTaskInfo,
                               IFlowMutator<TTrigger, TTriggerContext> flowMutator)
        {
            var stateTask = _resolver(stateTaskType) as IStateTask<TState, TTrigger, TTriggerContext>;
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
                                                                  stateTaskType));

            entryStateStep.Execute(entryStateTaskInfo);
        }
    }
}
