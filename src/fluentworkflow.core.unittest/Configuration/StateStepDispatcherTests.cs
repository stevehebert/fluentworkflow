using System;
using System.Collections.Generic;
using fluentworkflow.core.Builder;
using fluentworkflow.core.Configuration;
using fluentworkflow.core.unittest.enums;
using NUnit.Framework;
using Stateless;

namespace fluentworkflow.core.unittest.Configuration
{
    public class SimpleMockTask : IMutatingEntryStateTask<StateType, TriggerType, TriggerContext>
    {
        public void Execute(EntryStateTaskInfo<StateType, TriggerType, TriggerContext> entryStateTaskInfo, IFlowMutator<TriggerType, TriggerContext> flowMutator)
        {
            flowMutator.SetTrigger(TriggerType.Publish);
        }
    }
    
    [TestFixture]
    public class StateStepDispatcherTests
    {
        [Test]
        public void execute_state_with_redirect()
        {
            var metadata = new StateActionMetadata<WorkflowType, StateType>();

            metadata.Add(new StateActionInfo<WorkflowType, StateType>(WorkflowType.Comment,
                                                                      StateType.New,
                                                                      WorkflowTaskActionType.Entry,
                                                                      5, null));

            var dispatcher =
                new StateStepDispatcher<WorkflowType, StateType, TriggerType, TriggerContext>(new[]
                                                                                                  {
                                                                                                      new Lazy
                                                                                                          <
                                                                                                          IStateTask<StateType,TriggerType,TriggerContext>,
                                                                                                          IStateActionMetadata<WorkflowType,StateType>>(
                                                                                                          () =>new SimpleMockTask(),metadata)
                                                                                                  });

            var declaration = new WorkflowStepDeclaration<WorkflowType, StateType, TriggerType>(WorkflowType.Comment, StateType.New, new List<KeyValuePair<TriggerType, StateType>>());
            var context = new TriggerContext() {DocumentId = 5};

            var stateMachine = new StateMachine<StateType, TriggerType>(StateType.New);
            stateMachine.Configure(StateType.New).Permit(TriggerType.Publish, StateType.UnderReview);


            dispatcher.ExecuteEntryStepActions(declaration,
                                          context,
                                          new StateMachine<StateType, TriggerType>.Transition(StateType.New,
                                                                                              StateType.UnderReview,
                                                                                              TriggerType.Publish),
                                          stateMachine);


            Assert.That(stateMachine.State, Is.EqualTo(StateType.UnderReview));

        }
        //        private readonly
        //    IEnumerable<Lazy<IStateTask<TState, TTrigger, TTriggerContext>, IStateActionMetadata<TWorkflow, TState>>> _stateSteps;

        //public StateStepDispatcher(IEnumerable<Lazy<IStateTask<TState, TTrigger, TTriggerContext>, IStateActionMetadata<TWorkflow, TState>>> stateSteps)
        //{
        //    _stateSteps = stateSteps;
        //}


        //public void ExecuteEntryStepActions(WorkflowStepDeclaration<TWorkflow, TState, TTrigger> stepDeclaration,
        //                               TTriggerContext triggerContext,
        //                               StateMachine<TState, TTrigger>.Transition transition,
        //                               StateMachine<TState, TTrigger> stateMachine,
        //                               WorkflowTaskActionType WorkflowTaskActionType)
        //{
        //    var items = (from p in _stateSteps
        //                 from q in p.Metadata.StateActionInfos
        //                 where
        //                     (q.Workflow.Equals(stepDeclaration.Workflow) && q.State.Equals(stepDeclaration.State) &&
        //                     q.WorkflowTaskActionType == WorkflowTaskActionType)
        //                 orderby q.Priority descending
        //                 select p.Value);


    }
}
