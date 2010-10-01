using Stateless;

namespace fluentworkflow.core
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
        /// Gets the state transition information
        /// </summary>
        /// <value>The transition info.</value>
        public TransitionInfo<TState, TTrigger> TransitionInfo { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateStepInfo&lt;TState, TTrigger, TTriggerContext&gt;"/> class.
        /// </summary>
        /// <param name="triggerContext">The trigger context.</param>
        /// <param name="transition">The transition information from the state machine.</param>
        public StateStepInfo(TTriggerContext triggerContext, 
                             StateMachine<TState, TTrigger>.Transition transition)
                             
        {
            Context = triggerContext;

            TransitionInfo = new TransitionInfo<TState, TTrigger>(transition);
        }
    }
}
