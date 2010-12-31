using System;
using blogflow.Domain.Workflow;

namespace blogflow.Domain.Models
{
    public interface IDocumentModel
    {
        Guid Id { get; set; }
        WorkflowState State { get; set; }
    }
}