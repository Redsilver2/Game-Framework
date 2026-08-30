using RedSilver2.Framework.StateMachines.Extensions;

namespace RedSilver2.Framework.StateMachines.States {
    public sealed class Land : Movement
    {
        public LandState BaseState => GetState() as LandState;

        public Land() : base()
        {

        }

#if UNITY_EDITOR
        private bool wasInitialized = false;

        public override void Validate(MovementStateMachine stateMachine)
        {
            base.Validate(stateMachine);

            if (!wasInitialized) {
                BaseState?.AddEvent(new LandSound());
                wasInitialized = true;
            }

        }
#endif


        protected sealed override void SetBaseState(ref MovementState state)
        {
            state = new LandState();
        }
    }
}
