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

            Assert.That(transition.Source, Is.EqualTo(StateType.Complete));
            Assert.That(transition.Destination, Is.EqualTo(StateType.Rejected));
            Assert.That(transition.Trigger, Is.EqualTo(TriggerType.Submit));
        }
    }
}
