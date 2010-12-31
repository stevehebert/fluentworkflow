using Stateless;

namespace fluentworkflow.core
{
    /// <summary>
    /// the transition info passed to individual state steps
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TTrigger">The type of the trigger.</typeparam>
    public class StateEntryTransitionInfo<TState, TTrigger>
    {
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
        /// Initializes a new instance of the <see cref="StateEntryTransitionInfo{TState,TTrigger}"/> class.
        /// </summary>
        /// <param name="transition">underlying state engine transition</param>
        public StateEntryTransitionInfo(StateMachine<TState, TTrigger>.Transition transition)
        {
            PriorState = transition.Source;
            CurrentState = transition.Destination;
            Trigger = transition.Trigger;
        }
    }
}
