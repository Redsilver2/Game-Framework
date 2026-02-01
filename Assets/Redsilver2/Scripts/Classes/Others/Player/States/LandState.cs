
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class LandState : PlayerState
    {
        private readonly UnityEvent<RaycastHit> onLanded;

        public LandState(PlayerStateMachine owner) : base(owner) {
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

        protected sealed override void AddRequiredTransitionStates(PlayerStateMachine stateMachine) {
            if (stateMachine == null) return;
            if (!stateMachine.ContainsState(PlayerStateType.Fall) && IsValidTransitionState(PlayerStateType.Fall))
            {
                new FallState(stateMachine);
                AddTransitionState(stateMachine.GetState(PlayerStateType.Fall));
            }
        }

        protected sealed override void SetIncompatibleStateTransitions(ref PlayerStateType[] results) {
            results = GetStateTypes(new PlayerStateType[] { PlayerStateType.Walk, PlayerStateType.Run, PlayerStateType.Idol });
        }

        protected sealed override void SetPlayerStateType(ref PlayerStateType type) {
            type = PlayerStateType.Land;
        }
    }
}
