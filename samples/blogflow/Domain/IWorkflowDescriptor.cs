namespace blogflow.Domain
{
    public interface IWorkflowDescriptor
    {
        DocumentType DocumentType { get; }
        StateType State { get; set; }
    }
}