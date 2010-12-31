using blogflow.Domain.Repository;
using NUnit.Framework;

namespace blogflow.unittests.Domain.Repository
{
    [TestFixture]
    public class DocumentStoreFactoryTests
    {
        [Test]
        public void verify_store_creation()
        {
            var store = new DocumentStoreFactory().CreateStore();

            var session = store.OpenSession();

            Assert.That(session, Is.Not.Null);
            session.Dispose();
            store.Dispose();
        }
    }
}
