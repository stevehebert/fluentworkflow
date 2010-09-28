﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using metaworkflow.core.Configuration;
using metaworkflow.core.unittest.enums;
using Moq;
using NUnit.Framework;
using Stateless;

namespace metaworkflow.core.unittest.Configuration
{
    public class SimpleMockStep : IStateStep<StateType, TriggerType, TriggerContext>
    {
        public void Execute(StateStepInfo<StateType, TriggerType, TriggerContext> stateStepInfo)
        {
            stateStepInfo.Fire(TriggerType.Publish);
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
                                                                      WorkflowStepActionType.Entry,
                                                                      5));

            var dispatcher =
                new StateStepDispatcher<WorkflowType, StateType, TriggerType, TriggerContext>(new[]
                                                                                                  {
                                                                                                      new Lazy
                                                                                                          <
                                                                                                          IStateStep<StateType,TriggerType,TriggerContext>,
                                                                                                          IStateActionMetadata<WorkflowType,StateType>>(
                                                                                                          () =>new SimpleMockStep(),metadata)
                                                                                                  });

            var declaration = new WorkflowStepDeclaration<WorkflowType, StateType, TriggerType>(WorkflowType.Comment, StateType.New, new List<KeyValuePair<TriggerType, StateType>>());
            var context = new TriggerContext() {DocumentId = 5};

            var stateMachine = new StateMachine<StateType, TriggerType>(StateType.New);
            stateMachine.Configure(StateType.New).Permit(TriggerType.Publish, StateType.UnderReview);


            dispatcher.ExecuteStepActions(declaration,
                                          context,
                                          new StateMachine<StateType, TriggerType>.Transition(StateType.New,
                                                                                              StateType.UnderReview,
                                                                                              TriggerType.Publish),
                                          stateMachine,
                                          WorkflowStepActionType.Entry);


            Assert.That(stateMachine.State, Is.EqualTo(StateType.UnderReview));

        }
        //        private readonly
        //    IEnumerable<Lazy<IStateStep<TState, TTrigger, TTriggerContext>, IStateActionMetadata<TWorkflow, TState>>> _stateSteps;

        //public StateStepDispatcher(IEnumerable<Lazy<IStateStep<TState, TTrigger, TTriggerContext>, IStateActionMetadata<TWorkflow, TState>>> stateSteps)
        //{
        //    _stateSteps = stateSteps;
        //}


        //public void ExecuteStepActions(WorkflowStepDeclaration<TWorkflow, TState, TTrigger> stepDeclaration,
        //                               TTriggerContext triggerContext,
        //                               StateMachine<TState, TTrigger>.Transition transition,
        //                               StateMachine<TState, TTrigger> stateMachine,
        //                               WorkflowStepActionType workflowStepActionType)
        //{
        //    var items = (from p in _stateSteps
        //                 from q in p.Metadata.StateActionInfos
        //                 where
        //                     (q.Workflow.Equals(stepDeclaration.Workflow) && q.State.Equals(stepDeclaration.State) &&
        //                     q.WorkflowStepActionType == workflowStepActionType)
        //                 orderby q.Priority descending
        //                 select p.Value);


    }
}
