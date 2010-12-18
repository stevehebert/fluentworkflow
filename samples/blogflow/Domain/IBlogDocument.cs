using System.Collections.Generic;

namespace blogflow.Domain
{
    public interface IBlogDocument
    {
        /// <summary>
        /// Gets the workflow descriptor.
        /// </summary>
        /// <value>The workflow descriptor.</value>
        IWorkflowDescriptor WorkflowDescriptor { get; }

        /// <summary>
        /// Gets or sets the document history.
        /// </summary>
        /// <value>The document history.</value>
        IEnumerable<DocumentHistory> DocumentHistory { get; set; }

        /// <summary>
        /// Gets or sets the document contents.
        /// </summary>
        /// <value>The document contents.</value>
        IEnumerable<IDocumentContent> DocumentContents { get; set; }
    }
}