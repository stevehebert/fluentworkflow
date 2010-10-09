using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace blogflow.Domain.BlogEntry
{
    public class BlogEntryContent : IDocumentContent
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}