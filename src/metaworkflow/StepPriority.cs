using System;

namespace fluentworkflow.core
{
    public enum StepPriority
    {
        Highest = Int32.MaxValue,
        Medium = 0,
        Lowest = Int32.MinValue,
    }
}
