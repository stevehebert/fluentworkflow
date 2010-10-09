namespace blogflow.Domain.BlogEntry
{
    public class BlogPostContent : IDocumentContent
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }
}