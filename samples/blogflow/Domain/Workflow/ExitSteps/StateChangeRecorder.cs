using System;
using blogflow.Domain.Models;
using blogflow.Domain.Repository;
using fluentworkflow.core;

namespace blogflow.Domain.Workflow.ExitSteps
{
    /// <summary>
    /// Responsible for recording a state change on an exit state transition
    /// </summary>
    public class StateChangeRecorder : IExitStateStep<WorkflowState, StateTrigger, IDocumentContext>
    {
        private readonly IRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateChangeRecorder"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public StateChangeRecorder(IRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Executes the specified state step.
        /// </summary>
        /// <param name="entryStateStepInfo">The state step info.</param>
        public void Execute(EntryStateStepInfo<WorkflowState, StateTrigger, IDocumentContext> entryStateStepInfo)
        {
            var item = new StateChangeInfo(entryStateStepInfo.StateEntryTransitionInfo.PriorState, DateTime.Now);
            _repository.Add(item);
            _repository.Save();
        }
    }
}