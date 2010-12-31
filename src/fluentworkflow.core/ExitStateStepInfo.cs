using Stateless;

namespace fluentworkflow.core
{
    public class ExitStateStepInfo<TState, TTrigger, TTriggerContext>
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
        public StateExitTransitionInfo<TState, TTrigger> StateExitTransitionInfo { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitStateStepInfo{TState,TTrigger,TTriggerContext}"/> class.
        /// </summary>
        /// <param name="triggerContext">The trigger context.</param>
        /// <param name="transition">The transition information from the state machine.</param>
        public ExitStateStepInfo(TTriggerContext triggerContext, 
                             StateMachine<TState, TTrigger>.Transition transition)
                             
        {
            Context = triggerContext;

            StateExitTransitionInfo = new StateExitTransitionInfo<TState, TTrigger>(transition);
        }
    }
}
