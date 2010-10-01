using System;
using System.Collections.Generic;
using System.Linq;
using fluentworkflow.core.Configuration;
using Stateless;

namespace fluentworkflow.core
{
    public class MetaStateEngine <TWorkflow, TState, TTrigger, TTriggerContext> : IMetaStateEngine<TWorkflow, TState, TTrigger, TTriggerContext>
    {
        private readonly StateMachine<TState, TTrigger> _stateMachine;
        
        

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaStateEngine&lt;TWorkflow, TState, TTrigger, TTriggerContext&gt;"/> class.
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <param name="state">The state</param>
        /// <param name="stateMachineConfigurator">component responible for pulling together configuration of the state machine</param>
        public MetaStateEngine(TWorkflow workflow, TState state, IStateMachineConfigurator<TWorkflow,TState,TTrigger,TTriggerContext> stateMachineConfigurator )
        {
            Workflow = workflow;
            _stateMachine = stateMachineConfigurator.CreateStateMachine(workflow, state);
        }

        /// <summary>
        /// Fires the specified state trigger.
        /// </summary>
        /// <param name="stateTrigger">Thestate trigger.</param>
        /// <param name="triggerContext">The trigger context.</param>
        public void  Fire(TTrigger stateTrigger, TTriggerContext triggerContext)
        {
 	        _stateMachine.Fire(stateTrigger, triggerContext);
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
