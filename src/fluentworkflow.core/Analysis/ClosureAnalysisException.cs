using System;
using System.Collections.Generic;
using System.Text;

namespace fluentworkflow.core.Analysis
{
    public class ClosureAnalysisException<TWorkflow, TState, TTrigger> : Exception
    {
        public IEnumerable<StepDeclarationClosureError<TWorkflow, TState, TTrigger>> ClosureErrors { get; private set; }

        public ClosureAnalysisException(IEnumerable<StepDeclarationClosureError<TWorkflow, TState, TTrigger>> errors) : base(GenerateMessages(errors))
        {
            ClosureErrors = errors;
        }

        private static string GenerateMessages(IEnumerable<StepDeclarationClosureError<TWorkflow, TState, TTrigger>> errors)
        {
            var builder = new StringBuilder();

            builder.AppendLine("Fatal Error: workflow closure errors found");

            foreach (var error in errors)
                builder.AppendLine(error.ToString());

            return builder.ToString();
        }

    }
}
