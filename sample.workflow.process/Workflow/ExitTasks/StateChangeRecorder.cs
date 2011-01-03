using System;
using fluentworkflow.core;

namespace sample.workflow.process.Workflow.ExitTasks
{
    public class StateChangeRecorder : IExitStateTask<WorkflowState, StateTrigger, IDocumentContext>
    {
        public void Execute(ExitStateTaskInfo<WorkflowState, StateTrigger, IDocumentContext> exitStateTaskInfo)
        {
            Console.WriteLine("   ** recording state exit from {0}", exitStateTaskInfo.CurrentState);

            exitStateTaskInfo.Context.RecordStateExit(exitStateTaskInfo.CurrentState);
        }
    }
}
