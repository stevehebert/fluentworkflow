using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fluentworkflow.core.unittest.enums;

namespace fluentworkflow.core.unittest
{
    public class ExitStep3 : IExitStateStep<StateType, TriggerType, TriggerContext>
    {
        public void Execute(StateStepInfo<StateType, TriggerType, TriggerContext> stateStepInfo)
        {
        }
    }
}
