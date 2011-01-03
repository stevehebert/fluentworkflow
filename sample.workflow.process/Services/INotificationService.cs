using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sample.workflow.process.Services
{
    public interface INotificationService
    {
        void Notify(string userName);
    }
}
