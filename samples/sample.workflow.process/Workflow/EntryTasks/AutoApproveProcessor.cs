using System;
using fluentworkflow.core;
using fluentworkflow.core.Builder;

namespace sample.workflow.process.Workflow.EntryTasks
{
    public class AutoApproveProcessor : IMutatingEntryStateTask<WorkflowState, StateTrigger, IDocumentContext>
    {
        private const string MagicUserName = "foo";

        public void Execute(EntryStateTaskInfo<WorkflowState, StateTrigger, IDocumentContext> entryStateTaskInfo, 
                            IFlowMutator<StateTrigger, IDocumentContext> flowMutator)
        {
            if (entryStateTaskInfo.Context.UserName != MagicUserName)
            {
                Console.WriteLine("   ** auto approval not applied since username is not '{0}'", MagicUserName);
                return;
            }

            Console.WriteLine("   ** auto approving comment based on user name");
            flowMutator.SetTrigger(StateTrigger.Approve);
        }
    }
}
