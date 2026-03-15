using UnityEngine;


namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class LandMovementSoundExtension : GroundMovementSoundExtension
    {
        protected sealed override void OnStateAdded(MovementState state)
        {
            if(state is LandState) {
                state?.AddOnStateEnteredListener(OnStateEntered);
            }
        }

        protected sealed override void OnStateRemoved(MovementState state)
        {
            if (state is LandState) {
                state.RemoveOnStateEnteredListener(OnStateEntered);
            }
        }

        private void OnStateEntered() {
            Play(GroundTag());
        }

        protected sealed override bool CanAddOrRemoveState(State state)
        {
            return state is LandState;
        }
    }
}