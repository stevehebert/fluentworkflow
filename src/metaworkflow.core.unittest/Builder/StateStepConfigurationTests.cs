using System.Linq;
using metaworkflow.core.Builder;
using metaworkflow.core.unittest.enums;
using NUnit.Framework;

namespace metaworkflow.core.unittest.Builder
{
    [TestFixture]
    public class StateStepConfigurationTests
    {
        [Test]
        public void verify_permitted_trigger_declarations()
        {
            var config = new StateStepConfiguration<StateType, TriggerType, TriggerContext>(StateType.Rejected);

            config.Permit(TriggerType.Ignore, StateType.UnderReview);
            config.Permit(TriggerType.Submit, StateType.New);

            Assert.That(config.State, Is.EqualTo(StateType.Rejected));
            Assert.That(config.PermittedTriggers.Where(p => p.Key == TriggerType.Ignore).First().Value, Is.EqualTo(StateType.UnderReview));
            Assert.That(config.PermittedTriggers.Where(p => p.Key == TriggerType.Submit).First().Value, Is.EqualTo(StateType.New));
        }

        
    }
}
