using System.Linq;
using fluentworkflow.core.Configuration.v2;
using fluentworkflow.core.unittest.enums;
using NUnit.Framework;

namespace fluentworkflow.core.unittest.Configuration.v2
{
    [TestFixture]
    public class WorkflowExecutionUniverseTests
    {
        [Test]
        public void verify_empty_type_return_when_workflow_type_not_found()
        {
            var item = new WorkflowExecutionUniverse<WorkflowType, StateType, TriggerContext>();

            var items = item.Retrieve(WorkflowType.NewOrder, StateType.Rejected, WorkflowTaskActionType.Entry);

            Assert.That(items.Any(), Is.False);
        }
         


        [Test]
        public void verify_proper_enumeration_when_workflow_type_found()
        {
            var item = new WorkflowExecutionUniverse<WorkflowType, StateType, TriggerContext>();
            item.Add(new WorkflowStateExecutionSet<WorkflowType, StateType>(WorkflowType.NewOrder, StateType.Rejected,
                                                                            WorkflowTaskActionType.Entry,
                                                                            new [] {typeof (string), typeof (int)}));

            var items = item.Retrieve(WorkflowType.NewOrder, StateType.Rejected, WorkflowTaskActionType.Entry);

            Assert.That(items.Count(), Is.EqualTo(2));
            Assert.That(items.Where(p => p == typeof(string)).Count(), Is.EqualTo(1));
            Assert.That(items.Where(p => p == typeof(int)).Count(), Is.EqualTo(1));
        }
    }
}
