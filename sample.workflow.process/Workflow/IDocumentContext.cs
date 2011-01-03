namespace sample.workflow.process.Workflow
{
    public interface IDocumentContext
    {
        void RecordStateExit(WorkflowState workflowState);
        void SetCurrentState(WorkflowState workflowState);

        string UserName { get; }
        string Message { get; }

        WorkflowState State { get; }
        DocumentType DocumentType { get; }
    }
}
