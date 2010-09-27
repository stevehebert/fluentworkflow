using System;
using metaworkflow.core.unittest.enums;

namespace metaworkflow.core.unittest
{
    public class Step1 : IStateStep<StateType, TriggerType, TriggerContext>
    {
        public void Execute(StateStepInfo<StateType, TriggerType, TriggerContext> stateStepInfo)
        {
            throw new NotImplementedException();
        }
    }
}
