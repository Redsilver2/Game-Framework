using RedSilver2.Framework.StateMachines.States;

namespace RedSilver2.Framework.StateMachines
{
    [System.Serializable]
    public class Walk : Movement
    {
        public WalkState BaseState => GetState() as WalkState;

        public Walk() : base() {

        }


#if UNITY_EDITOR
        public sealed override void Validate(MovementStateMachine stateMachine)
        {
            base.Validate(stateMachine);

            if (stateMachine != null) {
                if (!stateMachine.ContainsMovement(MovementStateType.Idol)) stateMachine?.AddMovement(new Idol());
            }
        }
#endif

        protected sealed override void SetBaseState(ref MovementState state)
        {
            state = new WalkState();
        }

    }
}
