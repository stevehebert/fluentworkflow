using System;
using System.Linq;
using blogflow.Domain.Models;
using blogflow.Domain.Repository;
using blogflow.Domain.Workflow;
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

        [Test]
        public void verify_retrieval_of_multiple_documents()
        {
            var item1 = new TestDocument { Id = Guid.NewGuid(), Name = "foo1" };
            var item2 = new TestDocument { Id = Guid.NewGuid(), Name = "foo2" };
            
            using (var sessionProvider = new TestSessionProvider())
            {
                var repo = new BlogFlowRepository(sessionProvider.Session);

                repo.Add(item1);
                repo.Add(item2);
                repo.Save();

                var retrievedItems = repo.All<TestDocument>();

                Assert.That(retrievedItems.Count(), Is.EqualTo(2));
                Assert.That(retrievedItems.Where(p => p.Name == "foo1").Count(), Is.EqualTo(1));
                Assert.That(retrievedItems.Where(p => p.Name == "foo2").Count(), Is.EqualTo(1));
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

        public WorkflowState State { get; set; }

        public string Name { get; set; }
    }
}
