

namespace metaworkflow.core.Configuration
{
    public class StateTriggerAction<TState, TTrigger>
    {
        public TTrigger Trigger { get; private set; }
        public TState DestinationState { get; private set; }

        public StateTriggerAction(TTrigger trigger, TState destinationState)
        {
            Trigger = trigger;
            DestinationState = destinationState;
        }
    }
}
