using System;
using metaworkflow.core.Builder;
using Stateless;

namespace metaworkflow.core
{
    /// <summary>
    /// Contextual information for the current state transition process
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TTrigger">The type of the trigger.</typeparam>
    /// <typeparam name="TTriggerContext">The type of the trigger context.</typeparam>
    public class StateStepInfo<TState, TTrigger, TTriggerContext>
    {
        /// <summary>
        /// Gets the workflow context
        /// </summary>
        /// <value>The workflow context.</value>
        public TTriggerContext Context { get; private set; }

        /// <summary>
        /// Fires the specified trigger along with context information
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="triggerContext">The trigger context.</param>
        /// <exception cref="InvalidOperationException">thrown when a trigger has previously been set</exception>
        public void Fire(TTrigger trigger, TTriggerContext triggerContext)
        {
            _triggerTrip.SetTrigger(trigger, triggerContext);
        }

        /// <summary>
        /// Fires the specified trigger using the existing context
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        /// <exception cref="InvalidOperationException">thrown when a trigger has previously been set</exception>
        public void Fire(TTrigger trigger)
        {
            _triggerTrip.SetTrigger(trigger, Context);
        }

        /// <summary>
        /// Gets the state transition information
        /// </summary>
        /// <value>The transition info.</value>
        public TransitionInfo<TState, TTrigger> TransitionInfo { get; private set; }

        private readonly TriggerTrip<TTrigger, TTriggerContext> _triggerTrip;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateStepInfo&lt;TState, TTrigger, TTriggerContext&gt;"/> class.
        /// </summary>
        /// <param name="triggerContext">The trigger context.</param>
        /// <param name="transition">The transition information from the state machine.</param>
        /// <param name="triggerTrip">The trigger trip.</param>
        public StateStepInfo(TTriggerContext triggerContext, 
                             StateMachine<TState, TTrigger>.Transition transition, 
                             TriggerTrip<TTrigger, TTriggerContext> triggerTrip)
        {
            Context = triggerContext;
            _triggerTrip = triggerTrip;

            TransitionInfo = new TransitionInfo<TState, TTrigger>(transition);
        }
    }
}
