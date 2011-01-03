using System;
using System.Collections.Generic;
using sample.workflow.process.Workflow;

namespace sample.workflow.process.Domain
{
    public class StateHistory
    {
        public WorkflowState State { get; set; }
        public DateTime Changed { get; set; }
    }

    public class CommentDocument : IDocumentModel
    {
        public CommentDocument()
        {
            StateChangeHistory = new List<StateHistory>();
        }

        public int Id { get; set; }

        public string UserName { get; set; }
        public string Message { get; set; }

        public WorkflowState State { get; set; }
        public DocumentType DocType { get { return DocumentType.Comment; } }

        public IList<StateHistory> StateChangeHistory { get; set; }

        public void AddHistoryChange(WorkflowState state)
        {
            StateChangeHistory.Add(new StateHistory {State = state, Changed = DateTime.Now});
        }
    }
}
