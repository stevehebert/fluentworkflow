using System;
using System.Collections.Generic;
using System.Text;

namespace fluentworkflow.core.Analysis
{
    public class StateStepDependencyException<TWorkflow, TState> : Exception
    {
        public StateStepDependencyException(IEnumerable<StateTaskDependencyError<TWorkflow, TState>> stateStepDependencyErrors) : base( CreateMessage(stateStepDependencyErrors))
        {
            StateStepDependencyErrors = stateStepDependencyErrors;
        }

        public IEnumerable<StateTaskDependencyError<TWorkflow, TState>> StateStepDependencyErrors { get; private set; }

        private static string CreateMessage(IEnumerable<StateTaskDependencyError<TWorkflow, TState>> stateStepDependencyErrors)
        {
            var builder = new StringBuilder();

            builder.Append("State Step Dependency Errors on:\n");

            foreach (var error in stateStepDependencyErrors)
                builder.AppendLine("  " + error);

            return builder.ToString();
        }
    }

}
