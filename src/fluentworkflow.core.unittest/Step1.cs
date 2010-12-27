using System;
using fluentworkflow.core.unittest.enums;

namespace fluentworkflow.core.unittest
{
    public class Step1 : IEntryStateStep<StateType, TriggerType, TriggerContext>
    {
        public static int ExecutionCount { get; private set; }
        public void Execute(EntryStateStepInfo<StateType, TriggerType, TriggerContext> entryStateStepInfo)
        {
            ExecutionCount++;
        }
    }
}
