using fluentworkflow.core.Builder;
using fluentworkflow.core.unittest.enums;
using NUnit.Framework;
using Stateless;

namespace fluentworkflow.core.unittest
{
    [TestFixture]
    public class StateStepInfoTests
    {

        [Test]
        public void verify_context_passthrough()
        {
            var transition = new StateMachine<StateType, TriggerType>.Transition(StateType.New, StateType.UnderReview,
                                                                                 TriggerType.Ignore);

            var stateStepInfo = new StateStepInfo<StateType, TriggerType, TriggerContext>(new TriggerContext { DocumentId = 5 }, transition);

            Assert.That(stateStepInfo.Context.DocumentId, Is.EqualTo(5));
        }

        [Test]
        public void verify_transition_passthrough()
        {
            var transition = new StateMachine<StateType, TriggerType>.Transition(StateType.New, StateType.UnderReview,
                                                                                 TriggerType.Ignore);

            var stateStepInfo = new StateStepInfo<StateType, TriggerType, TriggerContext>(new TriggerContext { DocumentId = 5 }, transition);

            Assert.That(stateStepInfo.TransitionInfo.SourceState, Is.EqualTo(StateType.New));
            Assert.That(stateStepInfo.TransitionInfo.TargetState, Is.EqualTo(StateType.UnderReview));
        }
    }
}