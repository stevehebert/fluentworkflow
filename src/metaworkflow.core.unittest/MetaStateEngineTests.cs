using System.Linq;
using metaworkflow.core.Configuration;
using metaworkflow.core.unittest.enums;
using Moq;
using NUnit.Framework;
using Stateless;

namespace metaworkflow.core.unittest
{
    [TestFixture]
    public class MetaStateEngineTests
    {
        [Test]
        public void verify_trigger_option_behavior()
        {
            var mockConfigurator =
                new Mock<IStateMachineConfigurator<WorkflowType, StateType, TriggerType, TriggerContext>>();

            mockConfigurator.Setup(e => e.CreateStateMachine(It.IsAny<WorkflowType>(), It.IsAny<StateType>())).Returns(
                new StateMachine<StateType, TriggerType>(StateType.New));


            var engine = new MetaStateEngine<WorkflowType, StateType, TriggerType, TriggerContext>(WorkflowType.Comment,
                                                                                                 StateType.New, mockConfigurator.Object );

            var options = engine.GetTriggerOptions();

            Assert.That(options.Count, Is.EqualTo(3));

            Assert.That(options.Count(item => item.Value == false), Is.EqualTo(3));
        }
    }
}
