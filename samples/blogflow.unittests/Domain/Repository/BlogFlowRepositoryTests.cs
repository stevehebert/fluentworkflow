using System;
using blogflow.Domain.Models;
using blogflow.Domain.Repository;
using NUnit.Framework;

namespace blogflow.unittests.Domain.Repository
{
    [TestFixture]
    public class BlogFlowRepositoryTests
    {
        [Test]
        public void verify_storage_and_creation_of_a_test_document()
        {
            var item = new TestDocument {Id = Guid.NewGuid(), Name = "foo"};

            using( var store = new DocumentStoreFactory().CreateStore())
            using( var session = store.OpenSession())
            {
                var repo = new BlogFlowRepository(session);

                repo.Add(item);
                repo.Save();

                var retrievedItem = repo.SingleOrDefault<TestDocument>(p => p.Name == "foo");

                Assert.That(retrievedItem, Is.Not.Null);
                Assert.That(retrievedItem.Id, Is.EqualTo(item.Id));
                Assert.That(retrievedItem.Name, Is.EqualTo(item.Name));
            }
        }
    }

    public class TestDocument : IDocumentModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
