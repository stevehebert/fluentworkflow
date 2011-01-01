using Stateless;

namespace fluentworkflow.core
{
    /// <summary>
    /// Contextual information for the current state transition process
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TTrigger">The type of the trigger.</typeparam>
    /// <typeparam name="TTriggerContext">The type of the trigger context.</typeparam>
    public class EntryStateTaskInfo<TState, TTrigger, TTriggerContext>
    {
        /// <summary>
        /// Gets the workflow context
        /// </summary>
        /// <value>The workflow context.</value>
        public TTriggerContext Context { get; private set; }

        /// <summary>
        /// Gets the source state.
        /// </summary>
        /// <value>The source state.</value>
        public TState PriorState { get; private set; }

        /// <summary>
        /// Gets the target state.
        /// </summary>
        /// <value>The target state.</value>
        public TState CurrentState { get; private set; }

        /// <summary>
        /// Gets the trigger value intiating the state change
        /// </summary>
        /// <value>The trigger value.</value>
        public TTrigger Trigger { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntryStateTaskInfo{TState,TTrigger,TTriggerContext}"/> class.
        /// </summary>
        /// <param name="triggerContext">The trigger context.</param>
        /// <param name="transition">The transition information from the state machine.</param>
        public EntryStateTaskInfo(TTriggerContext triggerContext, 
                             StateMachine<TState, TTrigger>.Transition transition)
                             
        {
            Context = triggerContext;

            PriorState = transition.Source;
            CurrentState = transition.Destination;
            Trigger = transition.Trigger;
        }
    }
}
