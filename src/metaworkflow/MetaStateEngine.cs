using System;
using System.Collections.Generic;
using System.Linq;
using Stateless;

namespace metaworkflow.core
{
    public class MetaStateEngine <TWorkflow, TState, TTrigger, TTriggerContext> : IMetaStateEngine<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        private readonly StateMachine<TState, TTrigger> _stateMachine;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaStateEngine&lt;TWorkflow, TState, TTrigger, TTriggerContext&gt;"/> class.
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <param name="state">The state</param>
        public MetaStateEngine(TWorkflow workflow, TState state)
        {
            Workflow = workflow;
            _stateMachine = new StateMachine<TState, TTrigger>(state);
        }

        /// <summary>
        /// Fires the specified state trigger.
        /// </summary>
        /// <param name="stateTrigger">Thestate trigger.</param>
        /// <param name="triggerContext">The trigger context.</param>
        public void  Fire(TTrigger stateTrigger, TTriggerContext triggerContext)
        {
 	        throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the trigger options.
        /// </summary>
        /// <returns></returns>
        public IDictionary<TTrigger,bool> GetTriggerOptions()
        {
            return Enum.GetValues(typeof(TTrigger)).Cast<TTrigger>().ToDictionary(trigger => trigger, trigger => _stateMachine.CanFire(trigger));
        }

        /// <summary>
        /// Gets the current state.
        /// </summary>
        /// <value>The state.</value>
        public TState State { get { return _stateMachine.State; } }

        /// <summary>
        /// Gets or sets the workflow.
        /// </summary>
        /// <value>The workflow.</value>
        public TWorkflow Workflow { get; private set; }
    }
}
