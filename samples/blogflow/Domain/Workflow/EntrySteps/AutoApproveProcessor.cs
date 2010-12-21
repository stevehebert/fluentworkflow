using blogflow.Domain.Models;
using fluentworkflow.core;
using fluentworkflow.core.Builder;

namespace blogflow.Domain.Workflow.EntrySteps
{
    public class AutoApproveProcessor : IMutatingEntryStateStep<WorkflowState, StateTrigger, IDocumentContext>
    {
        public void Execute(StateStepInfo<WorkflowState, StateTrigger, IDocumentContext> stateStepInfo, IFlowMutator<StateTrigger, IDocumentContext> flowMutator)
        {
            if(stateStepInfo.Context.UserName == "foo")
                flowMutator.SetTrigger(StateTrigger.Approve);
        }
    }
}