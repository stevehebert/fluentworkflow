using blogflow.Domain.Models;
using blogflow.Notification;
using fluentworkflow.core;

namespace blogflow.Domain.Workflow.EntrySteps
{
    public class UnderReviewNotifier : IEntryStateStep<WorkflowState, StateTrigger, IDocumentContext>
    {
        private readonly IContextNotification _contextNotification;
        public UnderReviewNotifier(IContextNotification contextNotification)
        {
            _contextNotification = contextNotification;
        }

        public void Execute(StateStepInfo<WorkflowState, StateTrigger, IDocumentContext> stateStepInfo)
        {
            _contextNotification.NotifyUnderReviewStatus(stateStepInfo.Context);
        }
    }
}