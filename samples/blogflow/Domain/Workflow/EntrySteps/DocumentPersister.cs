using blogflow.Domain.Models;
using fluentworkflow.core;

namespace blogflow.Domain.Workflow.EntrySteps
{
    /// <summary>
    /// Responsible for notifying the context of the state change event
    /// </summary>
    public class DocumentPersister : IEntryStateTask<WorkflowState, StateTrigger, IDocumentContext>
    {
        /// <summary>
        /// hands the target state to the context and saves
        /// </summary>
        /// <param name="entryStateTaskInfo">The state step info.</param>
        public void Execute(EntryStateTaskInfo<WorkflowState, StateTrigger, IDocumentContext> entryStateTaskInfo)
        {

            entryStateTaskInfo.Context.SetState(entryStateTaskInfo.CurrentState);
            entryStateTaskInfo.Context.Save();
        }
    }
}