using fluentworkflow.core.unittest.enums;

namespace fluentworkflow.core.unittest
{
    public class ExitStep3 : IExitStateStep<StateType, TriggerType, TriggerContext>
    {
        public void Execute(ExitStateStepInfo<StateType, TriggerType, TriggerContext> entryStateStepInfo)
        {
        }
    }
}
