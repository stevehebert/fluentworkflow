using System;
using sample.workflow.process.Domain;
using sample.workflow.process.Repository;
using sample.workflow.process.Workflow;

namespace sample.workflow.process.Services.Document
{
    public class DocumentCreator
    {
        private readonly IRepository _repository;

        public DocumentCreator(IRepository repository)
        {
            _repository = repository;
        }

        public bool Create(int id)
        {
            Console.Write("Enter Comment: ");
            var content = Console.ReadLine();

            Console.WriteLine();
            Console.Write("Enter User Name: ");
            var user = Console.ReadLine();

            var document = new CommentDocument {Id = id, Message = content, UserName = user, State = WorkflowState.Create};

            _repository.Add(document);
            _repository.Save();
            return true;
        }
    }
}
