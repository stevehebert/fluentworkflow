using System.Collections.Generic;

namespace fluentworkflow.core.Builder
{
    public class ActiveStateTaskConfiguration<TState, TTrigger, TTriggerContext> : StateTaskConfiguration<TState, TTrigger, TTriggerContext>
    {
        private readonly StateStepMetadata _stateStepMetadata;
        public ActiveStateTaskConfiguration(StateStepMetadata stateStepMetadata, TState state, IDictionary<TTrigger, TState> permittedTriggers, IList<StateStepMetadata> metadataSteps)
            : base(state, permittedTriggers, metadataSteps)
        {
            _stateStepMetadata = stateStepMetadata;
        }

        public StateTaskConfiguration<TState, TTrigger, TTriggerContext> DependsOn<TStateTask>() where TStateTask : IStateTask<TState, TTrigger, TTriggerContext> 
        {
            _stateStepMetadata.SetDependency(typeof (TStateTask));
            return this;
        }
        
    }

    /// <summary>
    /// configuration for a given state
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TTrigger">The type of the trigger.</typeparam>
    /// <typeparam name="TTriggerContext">The type of the trigger context.</typeparam>
    public class StateTaskConfiguration<TState, TTrigger, TTriggerContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateTaskConfiguration{TState,TTrigger,TTriggerContext}"/> class.
        /// </summary>
        /// <param name="state">The state.</param>
        public StateTaskConfiguration(TState state) : this(state, new Dictionary<TTrigger, TState>(), new List<StateStepMetadata>() )
        {
        }

        protected StateTaskConfiguration(TState state, IDictionary<TTrigger, TState> permittedTriggers, IList<StateStepMetadata> metadataSteps)
        {
            State = state;
            _permittedTriggers = permittedTriggers;
            _stateStepInfoList = metadataSteps;
        }

        /// <summary>
        /// Gets the target state
        /// </summary>
        /// <value>The state.</value>
        public TState State { get; private set; }
        
        private readonly IDictionary<TTrigger, TState> _permittedTriggers;
        /// <summary>
        /// Gets the permitted triggers and their destination state.
        /// </summary>
        /// <value>The permitted triggers and destination state values.</value>
        public IEnumerable<KeyValuePair<TTrigger, TState>> PermittedTriggers { get { return _permittedTriggers; } }

        private readonly IList<StateStepMetadata> _stateStepInfoList;
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
        public StateTaskConfiguration<TState, TTrigger, TTriggerContext> Permit(TTrigger trigger, TState destinationState)
        {
            _permittedTriggers.Add(trigger, destinationState);
            return this;
        }

        /// <summary>
        /// declares a step to be executed when entering the underlying state
        /// </summary>
        /// <typeparam name="TStateTask">The type of the state step.</typeparam>
        public ActiveStateTaskConfiguration<TState, TTrigger, TTriggerContext> OnEntry<TStateTask>() where TStateTask : IEntryStateTask<TState, TTrigger, TTriggerContext>
        {
            var metadata = new StateStepMetadata(typeof (TStateTask), 0, WorkflowTaskActionType.Entry);

            _stateStepInfoList.Add(metadata);
            return new ActiveStateTaskConfiguration<TState, TTrigger, TTriggerContext>(metadata, State, _permittedTriggers, _stateStepInfoList);
        }

        /// <summary>
        /// declares a step to be executed when entering the underlying state
        /// </summary>
        /// <typeparam name="TStateTask">The type of the state step.</typeparam>
        public ActiveStateTaskConfiguration<TState, TTrigger, TTriggerContext> OnMutatableEntry<TStateTask>() where TStateTask : IMutatingEntryStateTask<TState, TTrigger, TTriggerContext>
        {
            var metadata = new StateStepMetadata(typeof (TStateTask), 0, WorkflowTaskActionType.Entry);

            _stateStepInfoList.Add(metadata);
            return new ActiveStateTaskConfiguration<TState, TTrigger, TTriggerContext>(metadata, State,
                                                                                       _permittedTriggers,
                                                                                       _stateStepInfoList);
        }


        /// <summary>
        /// declares a step to tbe executed when exiting the underlying state
        /// </summary>
        /// <typeparam name="TStateTask">The type of the state step.</typeparam>
        public ActiveStateTaskConfiguration<TState, TTrigger, TTriggerContext> OnExit<TStateTask>() where TStateTask : IExitStateTask<TState, TTrigger, TTriggerContext>
        {
            var metadata = new StateStepMetadata(typeof (TStateTask), 0, WorkflowTaskActionType.Exit);

            _stateStepInfoList.Add(metadata);
            return new ActiveStateTaskConfiguration<TState, TTrigger, TTriggerContext>(metadata, State, _permittedTriggers, _stateStepInfoList);
        }
    }
}
