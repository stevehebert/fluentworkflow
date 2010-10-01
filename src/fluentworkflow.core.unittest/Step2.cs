using fluentworkflow.core.unittest.enums;

namespace fluentworkflow.core.unittest
{
    public class Step2 : IStateStep<StateType, TriggerType, TriggerContext>
    {
        public void Execute(StateStepInfo<StateType, TriggerType, TriggerContext> stateStepInfo)
        {
        }
    }
}
