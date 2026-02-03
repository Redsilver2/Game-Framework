
using RedSilver2.Framework.StateMachines.States.Movement;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class LandState : MovementState
    {
        private readonly UnityEvent<RaycastHit> onLanded;

        public LandState(MovementStateMachine owner) : base(owner) {
            onLanded = new UnityEvent<RaycastHit>();
            AddOnStateEnteredListener(OnStateAdded);
        }

        private void OnStateAdded()
        {
            if (MovementHandler == null) return;

            Transform transform = MovementHandler.GetTransform();
            float groundCheckRange = MovementHandler.GetGroundCheckRange();

            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo, groundCheckRange, ~transform.gameObject.layer))
                onLanded?.Invoke(hitInfo);
        }

        public void AddOnLandedListener(UnityAction<RaycastHit> action){
            if (action != null) onLanded?.AddListener(action);
        }

        public void RemoveOnLandedListener(UnityAction<RaycastHit> action) {
            if (action != null) onLanded?.RemoveListener(action);
        }

        public sealed override bool IsValidTransition() {
            if(MovementHandler == null) return false;
            return MovementHandler.IsGrounded;
        }

        protected sealed override void AddRequiredTransitionStates(MovementStateMachine stateMachine) {
            if (stateMachine == null) return;
            if (!stateMachine.ContainsState(MovementStateType.Fall) && IsValidTransitionState(MovementStateType.Fall))
            {
                new FallState(stateMachine);
                AddTransitionState(stateMachine.GetState(MovementStateType.Fall));
            }
        }

        protected sealed override void SetIncompatibleStateTransitions(ref MovementStateType[] results) {
            results = GetExcludedStateTypes(new MovementStateType[] { MovementStateType.Walk, MovementStateType.Run, MovementStateType.Idol });
        }

        protected sealed override void SetPlayerStateType(ref MovementStateType type) {
            type = MovementStateType.Land;
        }

        protected sealed override void SetPlayerInputsEvents(PlayerMovementHandler handler) {

        }
    }
}
