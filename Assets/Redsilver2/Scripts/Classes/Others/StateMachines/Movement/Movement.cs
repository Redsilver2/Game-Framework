using UnityEngine;


namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract class Movement
    {
#if UNITY_EDITOR
        [HideInInspector] public string name;
#endif
        [SerializeField, SerializeReference] private MovementState        baseState;
        [SerializeField, HideInInspector]    private MovementStateMachine stateMachine;

        private bool isInitialized = false;


        private MovementType type;

        protected Movement() {
            SetBaseState(ref baseState);
            SetType(ref type);

            name = baseState != null ? baseState.Name : string.Empty;
        }

        public void InitializeEvents()
        {
            if (!isInitialized) {
                InitializeEvent(baseState, stateMachine);
                isInitialized = true;
            }
        }

        public void UnInitializeEvents()
        {
            if (isInitialized) {
                UnInitializeEvent(baseState, stateMachine);
                isInitialized = false;
            }
        }

        protected virtual void InitializeEvent(MovementState state, MovementStateMachine stateMachine) { }
        protected virtual void UnInitializeEvent(MovementState state, MovementStateMachine stateMachine) { }

#if UNITY_EDITOR
        public virtual void Validate(MovementStateMachine stateMachine) {
            this.stateMachine =  stateMachine;
            baseState?.Validate(stateMachine);
            Validate(baseState);
        }

        protected virtual void Validate(MovementState state) { }
#endif

        protected virtual void SetType(ref MovementType type) { this.type = MovementType.Universal; }

        public virtual bool IsType(MovementType type)
        {
            if(this.type == MovementType.Universal) return true;
            return this.type == type;
        }

        public MovementState GetState() => baseState;

        protected abstract void SetBaseState(ref MovementState state);
    }
}
