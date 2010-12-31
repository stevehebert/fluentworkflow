using blogflow.Domain.Models;
using fluentworkflow.core;
using fluentworkflow.core.Builder;

namespace blogflow.Domain.Workflow.EntrySteps
{
    public class AutoApproveProcessor : IMutatingEntryStateTask<WorkflowState, StateTrigger, IDocumentContext>
    {
        public void Execute(EntryStateTaskInfo<WorkflowState, StateTrigger, IDocumentContext> entryStateTaskInfo, IFlowMutator<StateTrigger, IDocumentContext> flowMutator)
        {
            if(entryStateTaskInfo.Context.UserName == "foo")
                flowMutator.SetTrigger(StateTrigger.Approve);
        }
    }
}