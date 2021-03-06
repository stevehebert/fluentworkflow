﻿using System;
using fluentworkflow.core.Builder;
using fluentworkflow.core.unittest.enums;
using NUnit.Framework;

namespace fluentworkflow.core.unittest.Builder
{
    [TestFixture]
    public class TriggerTripTests
    {
        [Test]
        public void verify_unset_indicator()
        {
            var item = new FlowMutator<TriggerType, TriggerContext>(new TriggerContext());

            Assert.That(item.IsSet, Is.False);
        }

        [Test]
        public void verify_set_indicator()
        {
            var item = new FlowMutator<TriggerType, TriggerContext>(new TriggerContext());

            item.SetTrigger(TriggerType.Publish, new TriggerContext());

            Assert.That(item.IsSet, Is.True);
        }

        [Test]
        public void verify_inability_to_double_set_the_trigger_trip()
        {
            var item = new FlowMutator<TriggerType, TriggerContext>(new TriggerContext());

            item.SetTrigger(TriggerType.Publish, new TriggerContext());
            Assert.Throws<InvalidOperationException>(() => item.SetTrigger(TriggerType.Ignore, new TriggerContext()));
        }

        [Test]
        public void verify_inability_to_double_set_the_trigger_trip_on_non_context_trigger()
        {
            var item = new FlowMutator<TriggerType, TriggerContext>(new TriggerContext());

            item.SetTrigger(TriggerType.Publish, new TriggerContext());
            Assert.Throws<InvalidOperationException>(() => item.SetTrigger(TriggerType.Ignore));
        }


        [Test]
        public void verify_data_flowthrough()
        {
            var item = new FlowMutator<TriggerType, TriggerContext>(new TriggerContext());

            item.SetTrigger(TriggerType.Publish, new TriggerContext {DocumentId =42});

            Assert.That(item.Trigger, Is.EqualTo(TriggerType.Publish));
            Assert.That(item.TriggerContext.DocumentId, Is.EqualTo(42));
        }
    }
}
