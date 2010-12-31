using blogflow.Domain.Models;
using blogflow.Domain.Workflow;
using blogflow.Domain.Workflow.EntrySteps;
using fluentworkflow.core.testhelpers;
using Moq;
using NUnit.Framework;

namespace blogflow.unittests.Domain.Workflow.EntrySteps
{
    [TestFixture]
    public class AutoApproveProcessorTests
    {
        [Test]
        public void verify_successful_autoapproval()
        {
            // arrange
            var contextMock = new Mock<IDocumentContext>();
            contextMock.Setup(e => e.UserName).Returns("foo");

            var item = new AutoApproveProcessor();

            // act
            var mutator = item.ExecuteWithContext(WorkflowState.Create, WorkflowState.UnderReview, StateTrigger.Submit, contextMock.Object);

            // assert
            contextMock.VerifyAll();
            Assert.That(mutator.IsSet, Is.True);
            Assert.That(mutator.Trigger, Is.EqualTo(StateTrigger.Approve));
        }

        [Test]
        public void verify_non_autoapproval()
        {
            // arrange
            var contextMock = new Mock<IDocumentContext>();
            contextMock.Setup(e => e.UserName).Returns("bar");

            var item = new AutoApproveProcessor();

            // act
            var mutator = item.ExecuteWithContext(WorkflowState.Create, WorkflowState.UnderReview, StateTrigger.Submit, contextMock.Object);

            // assert
            contextMock.VerifyAll();
            Assert.That(mutator.IsSet, Is.False);
        }
    }
}
