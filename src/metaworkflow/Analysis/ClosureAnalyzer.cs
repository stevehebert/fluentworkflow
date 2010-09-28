using System.Collections.Generic;
using System.Linq;
using metaworkflow.core.Configuration;

namespace metaworkflow.core.Analysis
{
    public class ClosureAnalyzer
    {
        public IEnumerable<StepDeclarationClosureError<TWorkflow, TState, TTrigger>> ValidateStepDeclarationClosure<TWorkflow, TState, TTrigger>(IEnumerable<WorkflowStepDeclaration<TWorkflow, TState, TTrigger>> stepDeclarations)
        {
            var errorList = new List<StepDeclarationClosureError<TWorkflow, TState, TTrigger>>();
            
            var groups = from p in stepDeclarations group p by p.Workflow into g select g.Key;


            foreach (var workflow in groups)
            {
                var localWorkflow = workflow;

                var states = from q in stepDeclarations where q.Workflow.Equals(localWorkflow) select q;

                var destinationStates = from p in stepDeclarations
                                        from q in p.PermittedActions
                                        select new {q.DestinationState, p.State, q.Trigger};


                errorList.AddRange(from destinationState in destinationStates
                                   where (from p in states
                                          where p.State.Equals(destinationState.DestinationState)
                                          select p).FirstOrDefault() == null
                                   select new StepDeclarationClosureError<TWorkflow, TState, TTrigger>(localWorkflow, destinationState.State, destinationState.Trigger, destinationState.DestinationState));
            }

            return errorList;
        }
    }
}
