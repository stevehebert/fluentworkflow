using Stateless;

namespace fluentworkflow.core
{
    /// <summary>
    /// the transition info passed to individual state steps
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TTrigger">The type of the trigger.</typeparam>
    public class StateExitTransitionInfo<TState, TTrigger>
    {
        /// <summary>
        /// Gets the next state that we are moving to.
        /// </summary>
        /// <value>The source state.</value>
        public TState NextState { get; private set; }

        /// <summary>
        /// Gets the current state we are executing on.
        /// </summary>
        /// <value>The target state.</value>
        public TState CurrentState { get; private set; }

        /// <summary>
        /// Gets the trigger value initiating the state change
        /// </summary>
        /// <value>The trigger value.</value>
        public TTrigger Trigger { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateEntryTransitionInfo{TState,TTrigger}"/> class.
        /// </summary>
        /// <param name="transition">underlying state engine transition</param>
        public StateExitTransitionInfo(StateMachine<TState, TTrigger>.Transition transition)
        {
            CurrentState = transition.Source;
            NextState = transition.Destination;
            Trigger = transition.Trigger;
        }
    }
}
