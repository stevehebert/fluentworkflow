using System;
using fluentworkflow.core.unittest.enums;

namespace fluentworkflow.core.unittest
{
    public class TaskDisposalTracker
    {
        public static void Reset()
        {
            DisposeCount = 0;
        }

        public static void Increment()
        {
            DisposeCount++;
        }

        public static int DisposeCount { get; private set; }
    }
    public class Task1 : IEntryStateTask<StateType, TriggerType, TriggerContext>,IDisposable 
    {
        public static int ExecutionCount { get; private set; }
        public void Execute(EntryStateTaskInfo<StateType, TriggerType, TriggerContext> entryStateTaskInfo)
        {
            ExecutionCount++;
        }

        public void Dispose()
        {
            TaskDisposalTracker.Increment();
        }
    }
}
