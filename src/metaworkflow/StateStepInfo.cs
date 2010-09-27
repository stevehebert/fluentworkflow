using Stateless;

namespace metaworkflow.core
{
    public class StateStepInfo<TState, TTrigger, TTriggerContext>
    {
        /// <summary>
        /// Gets the workflow context
        /// </summary>
        /// <value>The workflow context.</value>
        public TTriggerContext Context { get; private set; }

        public void Fire(TTrigger trigger, TTriggerContext triggerContext)
        {
        }

        public void Fire(TTrigger trigger)
        { }

        public TransitionInfo<TState, TTrigger> TransitionInfo { get; private set; }

        public StateStepInfo(TTriggerContext triggerContext, StateMachine<TState, TTrigger>.Transition transition)
        {
            Context = triggerContext;

            TransitionInfo = new TransitionInfo<TState, TTrigger>(transition);
        }
    }

}
