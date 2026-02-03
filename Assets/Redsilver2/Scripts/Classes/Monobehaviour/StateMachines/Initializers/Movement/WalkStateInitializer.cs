using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class WalkStateInitializer : MovementStateInitializer
    {
        [SerializeField] private float walkSpeed;

        protected sealed override State GetInitializerState(StateMachineController controller) {
            if(controller == null || !CanAddOrRemoveState(controller))  return null;
            return new WalkState(controller.StateMachine as MovementStateMachine);
        }

        private UnityAction OnUpdateWalkState(MovementState state)
        {
            if(state == null) return null;
            return () =>
            {
                if (state == null) return;
                state.MovementHandler?.UpdateMoveSpeed(walkSpeed, 5f);
            };
        }

        protected sealed override void OnStateAdded(MovementState state)
        {
            state?.AddOnUpdateListener(OnUpdateWalkState(state));
        }

        protected sealed override void OnStateRemoved(MovementState state)
        {
            state?.AddOnUpdateListener(OnUpdateWalkState(state));
        }

        protected override MovementStateType[] GetIncludedStates()
        {
            return new MovementStateType[] { MovementStateType.Walk };
        }
    }
}
