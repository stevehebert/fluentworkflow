using System;
using fluentworkflow.core.unittest.enums;

namespace fluentworkflow.core.unittest
{
    public class Task1 : IEntryStateTask<StateType, TriggerType, TriggerContext>
    {
        public static int ExecutionCount { get; private set; }
        public void Execute(EntryStateTaskInfo<StateType, TriggerType, TriggerContext> entryStateTaskInfo)
        {
            ExecutionCount++;
        }
    }
}
