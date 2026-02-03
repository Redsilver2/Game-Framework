using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines {
    public class CrouchStateIntializer : MovementStateInitializer {
        [SerializeField] private float standingHeight;
        [SerializeField] private float crouchHeight;

        [Space]
        [SerializeField] private float crouchMoveSpeed;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            standingHeight  = Mathf.Clamp(standingHeight , 2f, float.MaxValue);
            crouchHeight    = Mathf.Clamp(crouchHeight   , 0f, standingHeight - 1f);
            crouchMoveSpeed = Mathf.Clamp(crouchMoveSpeed, 1f, float.MaxValue);
        }
        #endif

        protected sealed override State GetInitializerState(StateMachineController controller) {
            if(controller == null || !CanAddOrRemoveState(controller)) return null;
            return new CrouchState(controller.StateMachine as MovementStateMachine);
        }

        private UnityAction OnUpdateCrouchHeight(MovementState state) {
            if(state == null) return null;
            
            return () => {
                if (state == null) return;
                state.MovementHandler?.UpdateMoveSpeed(crouchMoveSpeed, 5f);
                UpdateHeight(state, crouchHeight, 5f);
            };
        }

        private UnityAction OnUpdateStandHeight(MovementState state) {
            if (state == null) return null;
            return () => { UpdateHeight(state, standingHeight, 5f);  };
        }

        private void UpdateHeight(MovementState state, float nextHeight, float updateSpeed)
        {
            if (state == null) return;
            state.MovementHandler?.UpdateHeight(nextHeight, updateSpeed);
        }

        protected sealed override void OnStateAdded(MovementState state) {

            if (state is CrouchState) {
                state?.AddOnUpdateListener(OnUpdateCrouchHeight(state));
            }
            else {
                state?.AddOnUpdateListener(OnUpdateStandHeight(state));
            }
        }

        protected sealed override void OnStateRemoved(MovementState state)
        {
            if (state is CrouchState) {
                state?.RemoveOnUpdateListener(OnUpdateCrouchHeight(state));
            }
            else {
                state?.RemoveOnUpdateListener(OnUpdateStandHeight(state));
            }
        }

        protected sealed override MovementStateType[] GetIncludedStates()
        {
            return new MovementStateType[] {
                MovementStateType.Idol, MovementStateType.Run, MovementStateType.Walk,
                MovementStateType.Land, MovementStateType.Jump, MovementStateType.Crouch,
                MovementStateType.Fall
            };
        }
    }
}
