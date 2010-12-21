using System;
using System.Collections.Generic;
using blogflow.Domain.Models;

namespace blogflow.Domain
{
    public interface IRepository
    {
        T SingleOrDefault<T>(Func<T, bool> predicate) where T : IDocumentModel;
        IEnumerable<T> All<T>() where T : IDocumentModel;
        void Delete<T>(T item) where T : IDocumentModel;
        void Add<T>(T item) where T : IDocumentModel;
        void Save();
    }
}