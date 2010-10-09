using System;

namespace blogflow.Domain
{
    public class DocumentHistory
    {
        public DocumentHistory()
        {
            Updated = DateTime.Now;
        }

        public DateTime Updated { get; set; }

        public string Action { get; set; }
    }
}