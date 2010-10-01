using System.Linq;
using fluentworkflow.core.Configuration;
using fluentworkflow.core.unittest.enums;
using Moq;
using NUnit.Framework;
using Stateless;

namespace fluentworkflow.core.unittest
{
    [TestFixture]
    public class FluentStateEngineTests
    {
        [Test]
        public void verify_trigger_option_behavior()
        {
            var mockConfigurator =
                new Mock<IStateMachineConfigurator<WorkflowType, StateType, TriggerType, TriggerContext>>();

            mockConfigurator.Setup(e => e.CreateStateMachine(It.IsAny<WorkflowType>(), It.IsAny<StateType>())).Returns(
                new StateMachine<StateType, TriggerType>(StateType.New));


            var engine = new FluentStateEngine<WorkflowType, StateType, TriggerType, TriggerContext>(WorkflowType.Comment,
                                                                                                 StateType.New, mockConfigurator.Object );

            var options = engine.GetTriggerOptions();

            Assert.That(options.Count, Is.EqualTo(3));

            Assert.That(options.Count(item => item.Value == false), Is.EqualTo(3));
        }
    }
}
