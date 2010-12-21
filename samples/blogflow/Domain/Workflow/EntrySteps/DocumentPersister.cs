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
        /// <param name="stateStepInfo">The state step info.</param>
        public void Execute(StateStepInfo<WorkflowState, StateTrigger, IDocumentContext> stateStepInfo)
        {
            stateStepInfo.Context.SetState(stateStepInfo.TransitionInfo.TargetState);
            stateStepInfo.Context.Save();
        }
    }
}