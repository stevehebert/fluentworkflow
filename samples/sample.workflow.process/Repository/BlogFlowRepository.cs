using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using sample.workflow.process.Domain;

namespace sample.workflow.process.Repository
{
    public class BlogFlowRepository : IRepository
    {
        private readonly IDocumentSession _documentSession;

        public BlogFlowRepository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public T SingleOrDefault<T>(Func<T, bool> predicate) where T : IDocumentModel
        {
            return _documentSession.Query<T>().SingleOrDefault(predicate);
        }

        public IEnumerable<T> All<T>() where T : IDocumentModel
        {
            return _documentSession.Query<T>();
        }

        public void Delete<T>(T item) where T : IDocumentModel
        {
            _documentSession.Delete(item);
        }

        public void Add<T>(T item) where T : IDocumentModel
        {
            _documentSession.Store(item);
        }

        public void Save()
        {
            _documentSession.SaveChanges();
        }
    }
}
