using System;

namespace sample.workflow.process.Services
{
    public class NotificationService : INotificationService
    {
        public void Notify(string userName)
        {
            Console.WriteLine("      ** simulating a message being sent that UserName-'{0}' submitted a comment", userName);
        }
    }
}
