using metaworkflow.core.unittest.enums;
using NUnit.Framework;
using Stateless;

namespace metaworkflow.core.unittest
{
    [TestFixture]
    public class TransitionInfoTests
    {
        [Test]
        public void verify_information_passthrough()
        {
            var transition = new StateMachine<StateType, TriggerType>.Transition(StateType.Complete, StateType.Rejected,
                                                                                 TriggerType.Submit);

            var transitionInfo = new TransitionInfo<StateType, TriggerType>(transition);

            Assert.That(transitionInfo.SourceState, Is.EqualTo(StateType.Complete));
            Assert.That(transitionInfo.TargetState, Is.EqualTo(StateType.Rejected));
            Assert.That(transitionInfo.Trigger, Is.EqualTo(TriggerType.Submit));
        }
    }
}
