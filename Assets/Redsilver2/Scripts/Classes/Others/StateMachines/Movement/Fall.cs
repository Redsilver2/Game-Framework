
namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class Fall : Movement
    {
        public FallState BaseState => GetState() as FallState;
        public Fall() : base() { }

#if UNITY_EDITOR
        public sealed override void Validate(MovementStateMachine stateMachine)
        {
            base.Validate(stateMachine);

            if(stateMachine != null) {
                if (!stateMachine.ContainsMovement(MovementStateType.Land)) stateMachine?.AddMovement(new Land());
            }
        }
#endif

        private void Update()
        {
         
            BaseState?.Update();
        }

        protected override void InitializeEvent(MovementState state, MovementStateMachine stateMachine)
        {
            base.InitializeEvent(state, stateMachine);
            stateMachine?.AddOnUpdateListener(Update);
        }

        protected override void UnInitializeEvent(MovementState state, MovementStateMachine stateMachine)
        {
            base.UnInitializeEvent(state, stateMachine);
            stateMachine?.RemoveOnUpdateListener(Update);
        }
        protected sealed override void SetBaseState(ref MovementState state)
        {
            state = new FallState();
        }
    }
}
