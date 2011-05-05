using System;
using System.Collections.Generic;
using fluentworkflow.core.Configuration.v2;
using fluentworkflow.core.unittest.enums;
using NUnit.Framework;

namespace fluentworkflow.core.unittest.Configuration.v2
{
    [TestFixture]
    public class WorkflowStateExecutionSetTests
    {
        [Test]
        public void verify_participation_in_dictionary()
        {
            var item = new WorkflowStateExecutionSet<WorkflowType, StateType>(WorkflowType.Comment, StateType.New,
                                                                              WorkflowTaskActionType.Entry, new Type[]{});

            var dictionary =
                new Dictionary<WorkflowStateIndex<WorkflowType, StateType>, IEnumerable<Type>>(
                    new WorkflowStateIndex<WorkflowType, StateType>.EqualityComparer()) {{item.WorkflowStateIndex, item.Types}};

            Assert.That(dictionary.ContainsKey(item.WorkflowStateIndex), Is.True);
            Assert.That(dictionary.ContainsKey(new WorkflowStateIndex<WorkflowType, StateType>(WorkflowType.Comment, StateType.New, WorkflowTaskActionType.Entry)), Is.True);
            Assert.That(dictionary.ContainsKey(new WorkflowStateIndex<WorkflowType, StateType>(WorkflowType.NewOrder, StateType.New, WorkflowTaskActionType.Entry)), Is.False);
        }
    }
}
