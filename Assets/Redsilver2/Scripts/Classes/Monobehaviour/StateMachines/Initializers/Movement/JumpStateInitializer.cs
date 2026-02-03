using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.StateMachines
{
    [RequireComponent(typeof(FallStateInitializer))]
    public class JumpStateInitializer : MovementStateInitializer
    {
        [SerializeField] private float jumpForce;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            jumpForce = Mathf.Clamp(jumpForce, 10f, float.MaxValue);
        }
        #endif

        protected sealed override State GetInitializerState(StateMachineController controller) {
            if (controller == null || !CanAddOrRemoveState(controller)) return null;
            return new JumpState(controller.StateMachine as MovementStateMachine);
        }

        private UnityAction OnJumpStateEnter(MovementState state)
        {
            if(state == null)  return null;
            return () => {
                if (state == null) return;
                state.MovementHandler?.SetJumpHeight(jumpForce);
            };
        }

        protected sealed override void OnStateAdded(MovementState state)
        {
            state?.AddOnStateEnteredListener(OnJumpStateEnter(state));
        }

        protected sealed override void OnStateRemoved(MovementState state)
        {
            state?.RemoveOnStateEnteredListener(OnJumpStateEnter(state));
        }

        protected sealed override MovementStateType[] GetIncludedStates()
        {
            return new MovementStateType[] {
                MovementStateType.Jump
            };
        }
    }
}
