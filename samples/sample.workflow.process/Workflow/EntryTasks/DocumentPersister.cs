using System;
using fluentworkflow.core;

namespace sample.workflow.process.Workflow.EntryTasks
{
    public class DocumentPersister : IEntryStateTask<WorkflowState, StateTrigger, IDocumentContext>
    {
        public void Execute(EntryStateTaskInfo<WorkflowState, StateTrigger, IDocumentContext> entryStateTaskInfo)
        {
            Console.WriteLine("   ** modifying the state to {0} and saving the document", entryStateTaskInfo.CurrentState);
            entryStateTaskInfo.Context.SetCurrentState(entryStateTaskInfo.CurrentState);
        }
    }
}
