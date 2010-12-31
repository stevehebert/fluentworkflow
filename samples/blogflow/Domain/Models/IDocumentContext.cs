using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using blogflow.Domain.Workflow;

namespace blogflow.Domain.Models
{
    public interface IDocumentContext
    {
        string UserName { get; }
        void SetState(WorkflowState workflowState);
        void Save();

    }
}