using System;

namespace fluentworkflow.core.Metadata
{
    public class MetadataInstance<TWorkflow, TState, TDetail>
    {
        public TWorkflow Workflow { get; set; }
        public TState State { get; set; }
        public TDetail Detail { get; set; }
        public Type Type { get; set; }
    }
}
