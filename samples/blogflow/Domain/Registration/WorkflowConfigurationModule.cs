using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using blogflow.Domain.Models;
using fluentworkflow.core;
using blogflow.Domain.Workflow;
using fluentworkflow.core.Builder;

namespace blogflow.Domain.Registration
{
    public class WorkflowConfigurationModule : FluentWorkflowModule<DocumentType, WorkflowState, StateTrigger, IDocumentContext>
    {
        public override void Configure(IWorkflowBuilder<DocumentType, WorkflowState, StateTrigger, IDocumentContext> builder)
        {
            builder.ForWorkflow(DocumentType.Comment, WorkflowState.Create)
                .OnExit<StateChangeRecorder>();

        }
    }
}