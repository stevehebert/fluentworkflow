namespace blogflow.Domain
{
    public enum DocumentType
    {
        Post,
        Comment
    }

    public enum StateType
    {
        New,
        Draft,
        Posted,
        Rejected,
        Deleted
    }

    public enum TriggerType
    {
        Submit,
        Approve,
        Reject,
        Delete
    }
}