using System;
using System.Diagnostics;
using blogflow.Domain.Models;

namespace blogflow.Notification
{
    public interface IContextNotification
    {
        void NotifyUnderReviewStatus(IDocumentContext documentContext);
    }

    public class SimpleContextNotification : IContextNotification
    {
        public void NotifyUnderReviewStatus(IDocumentContext documentContext)
        {
            Trace.WriteLine("incoming comment pending review notice");
        }
    }
}