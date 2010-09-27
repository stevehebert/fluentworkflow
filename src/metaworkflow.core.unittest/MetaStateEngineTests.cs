using System.Linq;
using metaworkflow.core.unittest.enums;
using NUnit.Framework;

namespace metaworkflow.core.unittest
{
    [TestFixture]
    public class MetaStateEngineTests
    {
        [Test]
        public void verify_trigger_option_behavior()
        {
            var engine = new MetaStateEngine<WorkflowType, StateType, TriggerType, TriggerContext>(WorkflowType.Comment,
                                                                                                 StateType.New);

            var options = engine.GetTriggerOptions();

            Assert.That(options.Count, Is.EqualTo(3));

            Assert.That(options.Count(item => item.Value == false), Is.EqualTo(3));
        }
    }
}
