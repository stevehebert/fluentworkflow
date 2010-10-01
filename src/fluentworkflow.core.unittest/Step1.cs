using System;
using fluentworkflow.core.unittest.enums;

namespace fluentworkflow.core.unittest
{
    public class Step1 : IStateStep<StateType, TriggerType, TriggerContext>
    {
        public static int ExecutionCount { get; private set; }
        public void Execute(StateStepInfo<StateType, TriggerType, TriggerContext> stateStepInfo)
        {
            ExecutionCount++;
        }
    }
}
