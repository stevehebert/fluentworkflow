using blogflow.Domain.Models;
using blogflow.Domain.Workflow;
using blogflow.Domain.Workflow.EntrySteps;
using blogflow.Notification;
using fluentworkflow.core.testhelpers;
using Moq;
using NUnit.Framework;

namespace blogflow.unittests.Domain.Workflow.EntrySteps
{
    [TestFixture]
    public class UnderReviewNotifierTests
    {
        [Test]
        public void verify_notification_message()
        {
            // arrange
            var contextMock = new Mock<IDocumentContext>();
            var contextNotificationMock = new Mock<IContextNotification>();
            contextNotificationMock.Setup(e => e.NotifyUnderReviewStatus(It.Is<IDocumentContext>(p => p != null)));

            var item = new UnderReviewNotifier(contextNotificationMock.Object);

            // act
            item.ExecuteWithContext(WorkflowState.Create, 
                                    WorkflowState.UnderReview, 
                                    StateTrigger.Submit,
                                    contextMock.Object);

            // assert
            contextNotificationMock.VerifyAll();
        }
    }
}
