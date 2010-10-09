using System.Collections.Generic;

namespace blogflow.Domain
{
    public interface IBlogDocument
    {
        IWorkflowDescriptor WorkflowDescriptor { get; }

        IEnumerable<IDocumentContent> DocumentContents { get; set; }
    }
}