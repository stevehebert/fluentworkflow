using fluentworkflow.core.unittest.enums;
using NUnit.Framework;
using Stateless;

namespace fluentworkflow.core.unittest
{
    [TestFixture]
    public class EntryStateTaskInfoTests
    {

        [Test]
        public void verify_context_passthrough()
        {
            var transition = new StateMachine<StateType, TriggerType>.Transition(StateType.New, StateType.UnderReview,
                                                                                 TriggerType.Ignore);

            var stateStepInfo = new EntryStateTaskInfo<StateType, TriggerType, TriggerContext>(new TriggerContext { DocumentId = 5 }, transition);

            Assert.That(stateStepInfo.Context.DocumentId, Is.EqualTo(5));
        }

        [Test]
        public void verify_transition_passthrough()
        {
            var transition = new StateMachine<StateType, TriggerType>.Transition(StateType.New, StateType.UnderReview,
                                                                                 TriggerType.Ignore);

            var stateStepInfo = new EntryStateTaskInfo<StateType, TriggerType, TriggerContext>(new TriggerContext { DocumentId = 5 }, transition);

            Assert.That(stateStepInfo.PriorState, Is.EqualTo(StateType.New));
            Assert.That(stateStepInfo.CurrentState, Is.EqualTo(StateType.UnderReview));
        }
    }
}