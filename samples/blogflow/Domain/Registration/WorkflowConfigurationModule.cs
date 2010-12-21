﻿using blogflow.Domain.Models;
using blogflow.Domain.Workflow.EntrySteps;
using blogflow.Domain.Workflow.ExitSteps;
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
                .Permit(StateTrigger.Submit, WorkflowState.UnderReview)
                .OnExit<StateChangeRecorder>();

            builder.ForWorkflow(DocumentType.Comment, WorkflowState.UnderReview)
                .Permit(StateTrigger.Approve, WorkflowState.Published)
                .Permit(StateTrigger.Reject, WorkflowState.Rejected)
                .OnEntry<DocumentPersister>()
                .OnMutatableEntry<AutoApproveProcessor>().DependsOn<DocumentPersister>()
                .OnEntry<UnderReviewNotifier>().DependsOn<AutoApproveProcessor>()
                .OnExit<StateChangeRecorder>();

            builder.ForWorkflow(DocumentType.Comment, WorkflowState.Published)
                .OnEntry<DocumentPersister>();

            builder.ForWorkflow(DocumentType.Comment, WorkflowState.Rejected)
                .OnEntry<DocumentPersister>();

        }
    }
}