using System;
using System.Collections.Generic;

namespace blogflow.Domain.Comment
{
    public class CommentPost : IBlogDocument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentPost"/> class.
        /// </summary>
        public CommentPost()
        {
            WorkflowDescriptor = new WorkflowDescriptor(DocumentType.Post, StateType.New);
            DocumentHistory = new[] { new DocumentHistory { Action = "Created", Updated = DateTime.Now } };
            DocumentContents = new[] { new CommentPostContent() };
        }

        /// <summary>
        /// Gets the workflow descriptor.
        /// </summary>
        /// <value>The workflow descriptor.</value>
        public IWorkflowDescriptor WorkflowDescriptor { get; set; }

        /// <summary>
        /// Gets or sets the document history.
        /// </summary>
        /// <value>The document history.</value>
        public IEnumerable<DocumentHistory> DocumentHistory { get; set; }

        /// <summary>
        /// Gets or sets the document contents.
        /// </summary>
        /// <value>The document contents.</value>
        public IEnumerable<IDocumentContent> DocumentContents { get; set; }
    }
}