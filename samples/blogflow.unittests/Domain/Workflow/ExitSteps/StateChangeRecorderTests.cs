using blogflow.Domain.Models;
using blogflow.Domain.Repository;
using blogflow.Domain.Workflow;
using blogflow.Domain.Workflow.ExitSteps;
using fluentworkflow.core.testhelpers;
using Moq;
using NUnit.Framework;

namespace blogflow.unittests.Domain.Workflow.ExitSteps
{
    [TestFixture]
    public class StateChangeRecorderTests
    {
        [Test]
        public void state_change_recorder()
        {
            // arrange
            var mockRepository = new Mock<IRepository>();
            mockRepository.Setup(e => e.Add(It.Is<StateChangeInfo>(p => p.State == WorkflowState.Create)));
            mockRepository.Setup(e => e.Save());

            var item = new StateChangeRecorder(mockRepository.Object);

            // act
            item.ExecuteWithContext(WorkflowState.Create, WorkflowState.UnderReview, StateTrigger.Submit, null);

            // assert
            mockRepository.VerifyAll();
        }

    }
}
