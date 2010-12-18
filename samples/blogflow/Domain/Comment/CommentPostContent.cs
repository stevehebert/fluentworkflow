namespace blogflow.Domain.Comment
{
    public class CommentPostContent : IDocumentContent
    {
        public string Comment { get; set; }
        public string CommenterName { get; set; }
        public string Email { get; set; }
   
    }
}