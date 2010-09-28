using System;

namespace metaworkflow.core.Builder
{
    public interface ITriggerHandler<TTrigger, TTriggerContext>
    {
        /// <summary>
        /// Gets the declared trigger.
        /// </summary>
        /// <value>The trigger.</value>
        TTrigger Trigger { get; }

        /// <summary>
        /// Gets the declared context.
        /// </summary>
        /// <value>The trigger context.</value>
        TTriggerContext TriggerContext { get; }

        /// <summary>
        /// Gets the indicator of whether a trigger has been set
        /// </summary>
        /// <value><c>true</c> if this instance is set; otherwise, <c>false</c>.</value>
        bool IsSet { get; }

        /// <summary>
        /// Sets the trigger and associated context.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="triggerContext">The trigger context.</param>
        void SetTrigger(TTrigger trigger, TTriggerContext triggerContext);
    }

    public class TriggerHandler<TTrigger, TTriggerContext> : ITriggerHandler<TTrigger, TTriggerContext>
    {
        /// <summary>
        /// Gets the declared trigger.
        /// </summary>
        /// <value>The trigger.</value>
        public TTrigger Trigger { get; private set; }

        /// <summary>
        /// Gets the declared context.
        /// </summary>
        /// <value>The trigger context.</value>
        public TTriggerContext TriggerContext { get; private set; }

        /// <summary>
        /// Gets the indicator of whether a trigger has been set
        /// </summary>
        /// <value><c>true</c> if this instance is set; otherwise, <c>false</c>.</value>
        public bool IsSet { get; private set; }

        /// <summary>
        /// Sets the trigger and associated context.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="triggerContext">The trigger context.</param>
        public void SetTrigger(TTrigger trigger, TTriggerContext triggerContext)
        {
            if (IsSet)
                throw new InvalidOperationException("trigger has already been set");

            Trigger = trigger;
            TriggerContext = triggerContext;

            IsSet = true;
        }
    }
}
