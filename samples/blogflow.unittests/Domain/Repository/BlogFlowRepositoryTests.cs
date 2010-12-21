using System;
using blogflow.Domain.Models;
using blogflow.Domain.Repository;
using NUnit.Framework;
using Raven.Client;

namespace blogflow.unittests.Domain.Repository
{
    [TestFixture]
    public class BlogFlowRepositoryTests
    {
        [Test]
        public void verify_storage_and_creation_of_a_test_document()
        {
            var item = new TestDocument {Id = Guid.NewGuid(), Name = "foo"};

            using( var sessionProvider = new TestSessionProvider())
            {
                var repo = new BlogFlowRepository(sessionProvider.Session);

                repo.Add(item);
                repo.Save();

                var retrievedItem = repo.SingleOrDefault<TestDocument>(p => p.Name == "foo");

                Assert.That(retrievedItem, Is.Not.Null);
                Assert.That(retrievedItem.Id, Is.EqualTo(item.Id));
                Assert.That(retrievedItem.Name, Is.EqualTo(item.Name));
            }
        }

        [Test]
        public void verify_deletion_of_a_test_document()
        {
            var item = new TestDocument { Id = Guid.NewGuid(), Name = "foo" };

            using (var sessionProvider = new TestSessionProvider())
            {
                var repo = new BlogFlowRepository(sessionProvider.Session);

                repo.Add(item);
                repo.Save();

                var retrievedItem = repo.SingleOrDefault<TestDocument>(p => p.Name == "foo");

                Assert.That(retrievedItem, Is.Not.Null);

                repo.Delete(retrievedItem);
                repo.Save();

                retrievedItem = repo.SingleOrDefault<TestDocument>(p => p.Name == "foo");

                Assert.That(retrievedItem, Is.Null);
            }
            
        }
    }

    public class TestSessionProvider : IDisposable
    {
        private bool _finalized;
        private IDocumentStore _documentStore;

        public TestSessionProvider()
        {
            _documentStore = new DocumentStoreFactory().CreateStore();
            Session = _documentStore.OpenSession();
        }

        public IDocumentSession Session { get; private set; }

        ~TestSessionProvider()
        {
            if( !_finalized)
                throw new InvalidOperationException("test session provider not disposed");

        }

        public void Dispose()
        {
            if (!_finalized)
            {
                _documentStore.Dispose();
                Session.Dispose();
                Session = null;
                _documentStore = null;

                GC.SuppressFinalize(this);
                _finalized = true;
            }
        }
    }

    public class TestDocument : IDocumentModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
