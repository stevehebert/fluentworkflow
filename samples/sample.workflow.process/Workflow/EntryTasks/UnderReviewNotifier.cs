using fluentworkflow.core;
using sample.workflow.process.Services;

namespace sample.workflow.process.Workflow.EntryTasks
{
    public class UnderReviewNotifier : IEntryStateTask<WorkflowState, StateTrigger, IDocumentContext>
    {
        private readonly INotificationService _notificationService;

        public UnderReviewNotifier(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public void Execute(EntryStateTaskInfo<WorkflowState, StateTrigger, IDocumentContext> entryStateTaskInfo)
        {
            _notificationService.Notify(entryStateTaskInfo.Context.UserName);
        }
    }
}
