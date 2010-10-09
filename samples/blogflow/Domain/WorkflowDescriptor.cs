namespace blogflow.Domain
{
    public class WorkflowDescriptor : IWorkflowDescriptor
    {
        public WorkflowDescriptor(DocumentType documentType, StateType state)
        {
            DocumentType = documentType;
            State = state;
        }

        public DocumentType DocumentType { get; set; }

        public StateType State { get; set; }
    }
}