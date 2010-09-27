using System;
using System.Collections.Generic;
using metaworkflow.core.Builder;
using Stateless;

namespace metaworkflow.core
{
    public class TriggerTrip<TTrigger, TTriggerContext>
    {
        public TTrigger Trigger { get; private set; }

        public TTriggerContext TriggerContext { get; private set; }

        public bool IsSet { get; private set; }

        public void SetTrigger(TTrigger trigger, TTriggerContext triggerContext)
        {
            if(IsSet)
                throw new InvalidOperationException("trigger has already been set");

            Trigger = trigger;
            TriggerContext = triggerContext;

            IsSet = true;
        }
    }

    public class StateStepInfo<TState, TTrigger, TTriggerContext>
    {
        public TTriggerContext Context { get; private set; }

        public void Fire(TTrigger trigger, TTriggerContext triggerContext)
        {
        }

        public void Fire(TTrigger trigger)
        {}

        public TransitionInfo<TState, TTrigger> TransitionInfo { get; private set; }

        public StateStepInfo(TTriggerContext triggerContext, StateMachine<TState, TTrigger>.Transition transition)
        {
            Context = triggerContext;

            TransitionInfo = new TransitionInfo<TState, TTrigger>(transition);
        }
    }

    public interface IStateStep<TState, TTrigger, TTriggerContext>
    {
        void Execute(StateStepInfo<TState, TTrigger, TTriggerContext> stateStepInfo);
    }

    public enum WorkflowStepActionType
    {
        Entry,
        Exit
    }
    public enum StepPriority
    {
        Highest = Int32.MaxValue,
        Medium = 0,
        Lowest = Int32.MinValue,
    }

    public class StateStepInfo
    {
        public Type StateStepType { get; private set; }

        public int Priority { get; private set; }

        public WorkflowStepActionType ActionType { get; private set; }

        public StateStepInfo(Type stateStepType, int priority, WorkflowStepActionType actionType)
        {
            StateStepType = stateStepType;
            Priority = priority;
            ActionType = actionType;
        }
    }

    public class StateStepConfiguration<TState, TTrigger, TTriggerContext>
    {
        public TState State { get; private set; }

        private readonly IDictionary<TTrigger, TState> _permittedTriggers = new Dictionary<TTrigger, TState>();
        public  IEnumerable<KeyValuePair<TTrigger, TState>> PermittedTriggers { get { return _permittedTriggers; } }

        private readonly IList<StateStepInfo> _stateStepInfoList = new List<StateStepInfo>();
        public IEnumerable<StateStepInfo> StateStepInfos { get { return _stateStepInfoList; } }

        public StateStepConfiguration(TState state)
        {
            State = state;
        }

        public void Permit(TTrigger trigger, TState destinationState)
        {
            _permittedTriggers.Add(trigger, destinationState);
        }

        public void OnEntry<TStateStep>(StepPriority stepPriority) where TStateStep : IStateStep<TState, TTrigger, TTriggerContext>
        {
            _stateStepInfoList.Add(new StateStepInfo(typeof(TStateStep), (int)stepPriority, WorkflowStepActionType.Entry ));
        }

        public void OnExit<TStateStep>(StepPriority stepPriority) where TStateStep : IStateStep<TState, TTrigger, TTriggerContext>
        {
            _stateStepInfoList.Add(new StateStepInfo(typeof(TStateStep), (int)stepPriority, WorkflowStepActionType.Exit));
        }
    }


    public interface IWorkflowBuilder <TWorkflow, TState, TTrigger, TTriggerContext>
    {
        StateStepConfiguration<TState, TTrigger, TTriggerContext> ForWorkflow(TWorkflow workflow, TState state);
    }

    public class WorkflowBuilder <TWorkflow, TState, TTrigger, TTriggerContext> : IWorkflowBuilder<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        private readonly IDictionary<TWorkflow, StateStepConfiguration<TState, TTrigger, TTriggerContext>> _workflowConfiguration =
            new Dictionary<TWorkflow, StateStepConfiguration<TState, TTrigger, TTriggerContext>>();

        public StateStepConfiguration<TState, TTrigger, TTriggerContext> ForWorkflow(TWorkflow workflow, TState state)
        {
            StateStepConfiguration<TState, TTrigger, TTriggerContext> stateStepConfiguration = null;

            if(!_workflowConfiguration.TryGetValue(workflow, out stateStepConfiguration))
            {
                stateStepConfiguration = new StateStepConfiguration<TState, TTrigger, TTriggerContext>(state);
                _workflowConfiguration.Add(workflow, stateStepConfiguration);
            }

            return stateStepConfiguration;
        }
    }
}
