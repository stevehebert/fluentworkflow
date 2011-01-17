using fluentworkflow.core.Analysis;
using fluentworkflow.core.unittest.enums;
using NUnit.Framework;

namespace fluentworkflow.core.unittest.Analysis
{
	[TestFixture]
	public class StateStepDependencyExceptionTests
	{
		[Test]
		public void verify_message_composition()
		{
			var error = new StateTaskDependencyError<WorkflowType, StateType>()
			            	{
			            		Workflow = WorkflowType.Comment,
			            		Dependency = typeof (StateStepDependencyExceptionTests),
			            		ErrorReason = StateDependencyErrorReason.UnknownDependency,
			            		State = StateType.Rejected,
			            		Step = typeof (StateStepDependencyExceptionTests)
			            	};

			var exception = new StateStepDependencyException<WorkflowType, StateType>(new[] {error});

			Assert.That(exception.Message, Is.StringContaining("UnknownDependency"));
		}
	}
}
