using fluentworkflow.core.unittest.enums;

namespace fluentworkflow.core.unittest
{
    public class Task2 : IEntryStateTask<StateType, TriggerType, TriggerContext>
    {
        public void Execute(EntryStateTaskInfo<StateType, TriggerType, TriggerContext> entryStateTaskInfo)
        {
        }
    }
}
