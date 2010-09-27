using System.Collections.Generic;

namespace metaworkflow.core.Builder
{
    /// <summary>
    /// configuration for a given state
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TTrigger">The type of the trigger.</typeparam>
    /// <typeparam name="TTriggerContext">The type of the trigger context.</typeparam>
    public class StateStepConfiguration<TState, TTrigger, TTriggerContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateStepConfiguration&lt;TState, TTrigger, TTriggerContext&gt;"/> class.
        /// </summary>
        /// <param name="state">The state.</param>
        public StateStepConfiguration(TState state)
        {
            State = state;
        }

        /// <summary>
        /// Gets the target state
        /// </summary>
        /// <value>The state.</value>
        public TState State { get; private set; }
        
        private readonly IDictionary<TTrigger, TState> _permittedTriggers = new Dictionary<TTrigger, TState>();
        /// <summary>
        /// Gets the permitted triggers and their destination state.
        /// </summary>
        /// <value>The permitted triggers and destination state values.</value>
        public IEnumerable<KeyValuePair<TTrigger, TState>> PermittedTriggers { get { return _permittedTriggers; } }

        private readonly IList<StateStepMetadata> _stateStepInfoList = new List<StateStepMetadata>();
        /// <summary>
        /// Gets the state step metadata.
        /// </summary>
        /// <value>The state step metadata.</value>
        public IEnumerable<StateStepMetadata> StateStepInfos { get { return _stateStepInfoList; } }

        /// <summary>
        /// declares a permitted trigger for the underlying state and the destination state.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="destinationState">State of the destination.</param>
        public StateStepConfiguration<TState, TTrigger, TTriggerContext> Permit(TTrigger trigger, TState destinationState)
        {
            _permittedTriggers.Add(trigger, destinationState);
            return this;
        }

        /// <summary>
        /// declares a step to be executed when entering the underlying state
        /// </summary>
        /// <typeparam name="TStateStep">The type of the state step.</typeparam>
        /// <param name="stepPriority">The step priority.</param>
        public StateStepConfiguration<TState, TTrigger, TTriggerContext> OnEntry<TStateStep>(StepPriority stepPriority) where TStateStep : IStateStep<TState, TTrigger, TTriggerContext>
        {
            _stateStepInfoList.Add(new StateStepMetadata(typeof(TStateStep), (int)stepPriority, WorkflowStepActionType.Entry));
            return this;
        }

        /// <summary>
        /// declares a step to tbe executed when exiting the underlying state
        /// </summary>
        /// <typeparam name="TStateStep">The type of the state step.</typeparam>
        /// <param name="stepPriority">The step priority.</param>
        public StateStepConfiguration<TState, TTrigger, TTriggerContext> OnExit<TStateStep>(StepPriority stepPriority) where TStateStep : IStateStep<TState, TTrigger, TTriggerContext>
        {
            _stateStepInfoList.Add(new StateStepMetadata(typeof(TStateStep), (int)stepPriority, WorkflowStepActionType.Exit));
            return this;
        }
    }
}
