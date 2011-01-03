using System;
using sample.workflow.process.Domain;
using sample.workflow.process.Repository;

namespace sample.workflow.process.Workflow
{
    public class DocumentContext : IDocumentContext
    {
        private readonly IRepository _repository;
        private readonly CommentDocument _commentDocument;


        public DocumentContext(int id, IRepository repository)
        {
            _repository = repository;

            _commentDocument = repository.SingleOrDefault<CommentDocument>(p => p.Id == id);
        }

        public void RecordStateExit(WorkflowState workflowState)
        {
            _commentDocument.AddHistoryChange(workflowState);
            _repository.Save();
        }

        public void SetCurrentState(WorkflowState workflowState)
        {
            _commentDocument.State = workflowState;
            _repository.Save();
        }

        public string UserName
        {
            get { return _commentDocument.UserName; }
        }

        public string Message
        {
            get { return _commentDocument.Message; }
        }

        public WorkflowState State
        {
            get { return _commentDocument.State; }
        }

        public DocumentType DocumentType
        {
            get { return _commentDocument.DocType; }
        }
    }
}
