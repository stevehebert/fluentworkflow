using System;
using metaworkflow.core.unittest.enums;

namespace metaworkflow.core.unittest
{
    public class Step2 : IStateStep<StateType, TriggerType, TriggerContext>
    {
        public void Execute(StateStepInfo<StateType, TriggerType, TriggerContext> stateStepInfo)
        {
        }
    }
}
