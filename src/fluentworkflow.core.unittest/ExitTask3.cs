using fluentworkflow.core.unittest.enums;

namespace fluentworkflow.core.unittest
{
    public class ExitTask3 : IExitStateTask<StateType, TriggerType, TriggerContext>
    {
        public void Execute(ExitStateTaskInfo<StateType, TriggerType, TriggerContext> entryStateStepInfo)
        {
        }
    }
}
