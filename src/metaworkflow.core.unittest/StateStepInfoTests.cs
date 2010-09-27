using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using metaworkflow.core.Builder;
using metaworkflow.core.unittest.enums;
using NUnit.Framework;
using Stateless;

namespace metaworkflow.core.unittest
{
    [TestFixture]
    public class StateStepInfoTests
    {
        [Test]
        public void set_trigger_execution_with_existing_context()
        {
            var triggerTrip = new TriggerTrip
                <TriggerType,
                    TriggerContext>();

            var transition = new StateMachine<StateType, TriggerType>.Transition(StateType.New, StateType.UnderReview,
                                                                     TriggerType.Ignore);

            var stateStepInfo = new StateStepInfo<StateType, TriggerType, TriggerContext>(new TriggerContext {DocumentId = 5}, transition, triggerTrip);

            stateStepInfo.Fire(TriggerType.Submit);

            Assert.That(triggerTrip.Trigger, Is.EqualTo(TriggerType.Submit));
            Assert.That(triggerTrip.TriggerContext.DocumentId, Is.EqualTo(5));
        }


        [Test]
        public void set_trigger_execution_with_new_context()
        {
            var triggerTrip = new TriggerTrip<TriggerType,TriggerContext>();

            var transition = new StateMachine<StateType, TriggerType>.Transition(StateType.New, StateType.UnderReview,
                                                                                 TriggerType.Ignore);

            var stateStepInfo = new StateStepInfo<StateType, TriggerType, TriggerContext>(new TriggerContext { DocumentId = 5 }, transition, triggerTrip);

            stateStepInfo.Fire(TriggerType.Submit, new TriggerContext{DocumentId=6});

            Assert.That(triggerTrip.Trigger, Is.EqualTo(TriggerType.Submit));
            Assert.That(triggerTrip.TriggerContext.DocumentId, Is.EqualTo(6));
        }

        [Test]
        public void verify_context_passthrough()
        {
            var triggerTrip = new TriggerTrip<TriggerType, TriggerContext>();

            var transition = new StateMachine<StateType, TriggerType>.Transition(StateType.New, StateType.UnderReview,
                                                                                 TriggerType.Ignore);

            var stateStepInfo = new StateStepInfo<StateType, TriggerType, TriggerContext>(new TriggerContext { DocumentId = 5 }, transition, triggerTrip);

            Assert.That(stateStepInfo.Context.DocumentId, Is.EqualTo(5));
        }


    }
}
