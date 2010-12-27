using blogflow.Domain.Models;
using fluentworkflow.core;
using fluentworkflow.core.Builder;

namespace blogflow.Domain.Workflow.EntrySteps
{
    public class AutoApproveProcessor : IMutatingEntryStateStep<WorkflowState, StateTrigger, IDocumentContext>
    {
        public void Execute(EntryStateStepInfo<WorkflowState, StateTrigger, IDocumentContext> entryStateStepInfo, IFlowMutator<StateTrigger, IDocumentContext> flowMutator)
        {
            if(entryStateStepInfo.Context.UserName == "foo")
                flowMutator.SetTrigger(StateTrigger.Approve);
        }
    }
}