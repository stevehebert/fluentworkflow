using blogflow.Domain.Models;
using blogflow.Domain.Workflow;
using blogflow.Domain.Workflow.EntrySteps;
using fluentworkflow.core.testhelpers;
using Moq;
using NUnit.Framework;

namespace blogflow.unittests.Domain.Workflow.EntrySteps
{
    [TestFixture]
    public class DocumentPersisterTests
    {
        [Test]
        public void verify_modification_of_state()
        {
            // arrange
            var mockContext = new Mock<IDocumentContext>();
            mockContext.Setup(e => e.SetState(It.Is<WorkflowState>(p => p == WorkflowState.UnderReview)));
            mockContext.Setup(e => e.Save());

            var item = new DocumentPersister();

            // act
            item.ExecuteWithContext(WorkflowState.Create, 
                                    WorkflowState.UnderReview, 
                                    StateTrigger.Submit,
                                    mockContext.Object);

            // assert
            mockContext.VerifyAll();
        }
    }
}
