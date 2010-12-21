using System;
using blogflow.Domain.Workflow;

namespace blogflow.Domain.Models
{
    public class StateChangeInfo : IDocumentModel
    {
        public Guid Id { get; set; }
        public WorkflowState State { get; set; }
        public DateTime Created { get; set; }

        public StateChangeInfo()
        {}

        public StateChangeInfo(WorkflowState workflowState, DateTime created)
        {
            Id = Guid.NewGuid();
            State = workflowState;
            Created = created;
        }
    }
}