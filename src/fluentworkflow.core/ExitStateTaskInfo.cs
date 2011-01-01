using Stateless;

namespace fluentworkflow.core
{
    public class ExitStateTaskInfo<TState, TTrigger, TTriggerContext>
    {
        /// <summary>
        /// Gets the workflow context
        /// </summary>
        /// <value>The workflow context.</value>
        public TTriggerContext Context { get; private set; }

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
        /// Initializes a new instance of the <see cref="ExitStateTaskInfo{TState,TTrigger,TTriggerContext}"/> class.
        /// </summary>
        /// <param name="triggerContext">The trigger context.</param>
        /// <param name="transition">The transition information from the state machine.</param>
        public ExitStateTaskInfo(TTriggerContext triggerContext, 
                             StateMachine<TState, TTrigger>.Transition transition)
                             
        {
            Context = triggerContext;

            CurrentState = transition.Source;
            NextState = transition.Destination;
            Trigger = transition.Trigger;
        }
    }
}
