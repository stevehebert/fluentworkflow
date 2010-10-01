namespace fluentworkflow.core.Builder
{
    public interface IFlowMutator<TTrigger, TTriggerContext>
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


        /// <summary>
        /// Sets the trigger implying that the context remains the same.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        void SetTrigger(TTrigger trigger);
    }


}
