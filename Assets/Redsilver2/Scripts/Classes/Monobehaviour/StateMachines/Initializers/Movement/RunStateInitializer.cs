using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    [RequireComponent(typeof(WalkStateInitializer))]
    public sealed class RunStateInitializer : MovementStateInitializer
    {
        [SerializeField] private float runSpeed;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            runSpeed = Mathf.Clamp(runSpeed, 1f, float.MaxValue);
        }
        #endif

        protected sealed override State GetInitializerState(StateMachineController controller) {
            if(controller == null || !CanAddOrRemoveState(controller)) return null;
            return new RunState(controller.StateMachine as MovementStateMachine);
        }

        private UnityAction OnUpdateRunState(MovementState state) {
            if (state == null) return null;
            return () =>
            {
                if (state == null) return;
                state.MovementHandler?.UpdateMoveSpeed(runSpeed, 5f);
            };
        }

        protected sealed override void OnStateAdded(MovementState state) {
            state?.AddOnUpdateListener(OnUpdateRunState(state));
        }

        protected sealed override void OnStateRemoved(MovementState state) {
            state?.RemoveOnUpdateListener(OnUpdateRunState(state));
        }

        protected sealed override MovementStateType[] GetIncludedStates() {
            return new MovementStateType[] { MovementStateType.Run };
        }

    }
}
