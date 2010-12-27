using fluentworkflow.core.unittest.enums;

namespace fluentworkflow.core.unittest
{
    public class Step2 : IEntryStateStep<StateType, TriggerType, TriggerContext>
    {
        public void Execute(EntryStateStepInfo<StateType, TriggerType, TriggerContext> entryStateStepInfo)
        {
        }
    }
}
