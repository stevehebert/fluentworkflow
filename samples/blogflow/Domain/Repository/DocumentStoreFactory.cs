using Raven.Client;
using Raven.Client.Client;

namespace blogflow.Domain.Repository
{
    public class DocumentStoreFactory
    {
        public IDocumentStore CreateStore()
        {
            var documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = "c:\\Raven",

                RunInMemory = true,
                UseEmbeddedHttpServer = true,
            };

            documentStore.Initialize();

            return documentStore;
        }
    }
}