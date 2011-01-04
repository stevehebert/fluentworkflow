using Raven.Client;

namespace sample.workflow.process.Repository
{
    public class DocumentStoreFactory
    {
        public IDocumentStore CreateStore()
        {

            var documentStore = new Raven.Client.Client.EmbeddableDocumentStore
                                    {
                                        RunInMemory = true,
                                    };

            documentStore.Initialize();

            return documentStore;
        }
    }
}
