using fluentworkflow.core.unittest.enums;
using NUnit.Framework;
using Stateless;

namespace fluentworkflow.core.unittest
{
    public class StateExitTransitionInfoTests
    {
        [Test]
        public void verify_information_passthrough()
        {
            var transition = new StateMachine<StateType, TriggerType>.Transition(StateType.Complete, StateType.Rejected,
                                                                                 TriggerType.Submit);

            var transitionInfo = new StateExitTransitionInfo<StateType, TriggerType>(transition);

            Assert.That(transitionInfo.CurrentState, Is.EqualTo(StateType.Complete));
            Assert.That(transitionInfo.NextState, Is.EqualTo(StateType.Rejected));
            Assert.That(transitionInfo.Trigger, Is.EqualTo(TriggerType.Submit));
        }
    }
}
