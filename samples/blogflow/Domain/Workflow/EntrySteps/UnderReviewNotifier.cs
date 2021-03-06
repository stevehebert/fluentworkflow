﻿using blogflow.Domain.Models;
using blogflow.Notification;
using fluentworkflow.core;

namespace blogflow.Domain.Workflow.EntrySteps
{
    /// <summary>
    /// Responsible for initiating notifications based on a document entering the under review state
    /// </summary>
    public class UnderReviewNotifier : IEntryStateTask<WorkflowState, StateTrigger, IDocumentContext>
    {
        private readonly IContextNotification _contextNotification;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnderReviewNotifier"/> class.
        /// </summary>
        /// <param name="contextNotification">The context notification.</param>
        public UnderReviewNotifier(IContextNotification contextNotification)
        {
            _contextNotification = contextNotification;
        }

        /// <summary>
        /// Executes the specified state step notifying when a document is under review
        /// </summary>
        /// <param name="entryStateTaskInfo">The state step info.</param>
        public void Execute(EntryStateTaskInfo<WorkflowState, StateTrigger, IDocumentContext> entryStateTaskInfo)
        {
            _contextNotification.NotifyUnderReviewStatus(entryStateTaskInfo.Context);
        }
    }
}