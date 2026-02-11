using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public class FallState : MovementState
    {
        private FallStateInitializer initializer;

        public FallState(MovementStateMachine owner, FallStateInitializer initializer) : base(owner) {

            this.initializer = initializer;

            AddOnUpdateListener(() => {
                if(this.initializer == null || MovementHandler == null) return;
                this.initializer?.UpdateFallSpeed(this.initializer.FallSpeed, this.initializer.FallTransitionSpeed);
                MovementHandler?.UpdateMoveSpeed(this.initializer.MoveSpeed, this.initializer.MovementTransitionSpeed);
            });

            owner?.AddOnStateModuleAddedListener(OnStateModuleAdded);
            owner?.AddOnStateAddedListener(OnStateAdded);   
            owner?.AddOnStateRemovedListener(OnStateRemoved);
        }

        protected override void RemoveAllListenersFromOwner(StateMachine owner)
        {
            base.RemoveAllListenersFromOwner(owner);
            if (owner == null) return;

            owner?.RemoveOnStateModuleAddedListener(OnStateModuleAdded);
            owner?.RemoveOnStateAddedListener(OnStateAdded);
            owner?.RemoveOnStateRemovedListener(OnStateRemoved);
        }

        private void OnStateAdded(State state)
        {
            MovementState movementState = state as MovementState;

            if (initializer == null || !initializer.IsTransitionState(movementState))
                return;

            if(movementState is LandState) movementState?.AddOnUpdateListener(OnResetFallSpeed(movementState as LandState));
            else                           movementState?.AddOnUpdateListener(OnUpdateFallSpeed(movementState));
        }

        private void OnStateRemoved(State state) {
            MovementState movementState = state as MovementState;
            if (movementState is LandState) movementState?.AddOnUpdateListener(OnResetFallSpeed(movementState as LandState));
            else movementState?.AddOnUpdateListener(OnUpdateFallSpeed(movementState));
        }

        private UnityAction OnResetFallSpeed(LandState state)
        {
            if(state == null) return null;
          
            return () => {
                if (initializer == null) return;
                initializer?.SetFallSpeed(initializer.DefaultFallSpeed);
            };
        }

        private UnityAction OnUpdateFallSpeed(MovementState state)
        {
            if(state == null) return null;

            return () => {
                if (state == null || initializer == null) 
                    return;

                initializer?.UpdateFallSpeed(initializer.DefaultFallSpeed, initializer.DefaultFallTransitionSpeed);     
            };
        }

        private void OnStateModuleAdded(StateModule module)
        {
            if(module is FallStateInitializer) initializer = module as FallStateInitializer;
        }

        protected sealed override void SetIncompatibleStateTransitions(ref MovementStateType[] results) {
           results = GetExcludedStateTypes(new MovementStateType[] { MovementStateType.Land });
        }

        protected sealed override void SetPlayerStateType(ref MovementStateType type) {
            type = MovementStateType.Fall;
        }
    }
}
