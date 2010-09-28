using System;
using metaworkflow.core.Builder;
using metaworkflow.core.unittest.enums;
using NUnit.Framework;

namespace metaworkflow.core.unittest.Builder
{
    [TestFixture]
    public class TriggerTripTests
    {
        [Test]
        public void verify_unset_indicator()
        {
            var item = new TriggerHandler<TriggerType, TriggerContext>();

            Assert.That(item.IsSet, Is.False);
        }

        [Test]
        public void verify_set_indicator()
        {
            var item = new TriggerHandler<TriggerType, TriggerContext>();

            item.SetTrigger(TriggerType.Publish, new TriggerContext());

            Assert.That(item.IsSet, Is.True);
        }

        [Test]
        public void verify_inability_to_double_set_the_trigger_trip()
        {
            var item = new TriggerHandler<TriggerType, TriggerContext>();

            item.SetTrigger(TriggerType.Publish, new TriggerContext());
            Assert.Throws<InvalidOperationException>(() => item.SetTrigger(TriggerType.Ignore, new TriggerContext()));
        }

        [Test]
        public void verify_data_flowthrough()
        {
            var item = new TriggerHandler<TriggerType, TriggerContext>();

            item.SetTrigger(TriggerType.Publish, new TriggerContext {DocumentId =42});

            Assert.That(item.Trigger, Is.EqualTo(TriggerType.Publish));
            Assert.That(item.TriggerContext.DocumentId, Is.EqualTo(42));
        }
    }
}
