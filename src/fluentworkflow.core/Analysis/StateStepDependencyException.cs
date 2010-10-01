using System;
using System.Collections.Generic;
using System.Text;

namespace fluentworkflow.core.Analysis
{
    public class StateStepDependencyException<TWorkflow, TState> : Exception
    {
        public StateStepDependencyException(IEnumerable<StateStepDependencyError<TWorkflow, TState>> stateStepDependencyErrors) : base( CreateMessage(stateStepDependencyErrors))
        {
            StateStepDependencyErrors = stateStepDependencyErrors;
        }

        public IEnumerable<StateStepDependencyError<TWorkflow, TState>> StateStepDependencyErrors { get; private set; }

        private static string CreateMessage(IEnumerable<StateStepDependencyError<TWorkflow, TState>> stateStepDependencyErrors)
        {
            var builder = new StringBuilder();

            builder.Append("State Step Dependency Errors on:\n");

            foreach (var error in stateStepDependencyErrors)
                builder.AppendFormat("  {0}\n", error);

            return builder.ToString();
        }
    }

}
