using blogflow.Domain.Models;
using fluentworkflow.core;

namespace blogflow.Domain.Workflow.EntrySteps
{
    /// <summary>
    /// Responsible for notifying the context of the state change event
    /// </summary>
    public class DocumentPersister : IEntryStateStep<WorkflowState, StateTrigger, IDocumentContext>
    {
        /// <summary>
        /// hands the target state to the context and saves
        /// </summary>
        /// <param name="entryStateStepInfo">The state step info.</param>
        public void Execute(EntryStateStepInfo<WorkflowState, StateTrigger, IDocumentContext> entryStateStepInfo)
        {

            entryStateStepInfo.Context.SetState(entryStateStepInfo.StateEntryTransitionInfo.CurrentState);
            entryStateStepInfo.Context.Save();
        }
    }
}