using System.Linq;
using fluentworkflow.core.Builder;
using fluentworkflow.core.unittest.enums;
using NUnit.Framework;

namespace fluentworkflow.core.unittest.Builder
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

        [Test]
        public void verify_state_entrance_and_exit_declarations()
        {
            var config = new StateStepConfiguration<StateType, TriggerType, TriggerContext>(StateType.Rejected);

            config.OnEntry<Task1>();
            config.OnExit<ExitTask3>();
            

            Assert.That(config.State, Is.EqualTo(StateType.Rejected));
            Assert.That(
                config.StateStepInfos.Where(
                    p =>
                    p.ActionType == WorkflowTaskActionType.Entry &&
                    p.StateStepType == typeof (Task1)).Count(), Is.EqualTo(1));


            Assert.That(
                config.StateStepInfos.Where(
                    p =>
                    p.ActionType == WorkflowTaskActionType.Exit && 
                    p.StateStepType == typeof(ExitTask3)).Count(), Is.EqualTo(1));
        }
    }
}
