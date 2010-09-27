using System.Collections.Generic;

namespace metaworkflow.core
{
    public interface IMetaStateEngine<out TWorkflow, out TState, TTrigger, in TTriggerContext>
    {
        /// <summary>
        /// Fires the specified pubs state trigger.
        /// </summary>
        /// <param name="trigger">The state trigger.</param>
        /// <param name="triggerContext">context for the trigger</param>
        void Fire(TTrigger trigger, TTriggerContext triggerContext);

        /// <summary>
        /// Gets the trigger options.
        /// </summary>
        /// <returns></returns>
        IDictionary<TTrigger, bool> GetTriggerOptions();

        /// <summary>
        /// Gets the current state.
        /// </summary>
        /// <value>The state.</value>
        TState State { get; }

        /// <summary>
        /// Gets the workflow.
        /// </summary>
        /// <value>The workflow.</value>
        TWorkflow Workflow { get; }
    }
}
