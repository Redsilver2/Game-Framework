namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class WalkState : MovementState
    {
        private WalkStateInitializer initializer;

        public WalkState(MovementStateMachine owner, WalkStateInitializer initializer) : base(owner) {
            this.initializer = initializer;
            
            AddOnUpdateListener(() => {
                if (owner == null || this.initializer == null) return;
                owner.MovementHandler?.UpdateMoveSpeed(this.initializer.WalkSpeed, this.initializer.WalkTransitionSpeed);
            });

            AddOnStateAddedListener(() =>
            {
                owner?.AddOnStateModuleAddedListener(OnStateModuleAdded);
            });
        }

        protected sealed override void RemoveAllListenersFromOwner(StateMachine owner)
        {
            base.RemoveAllListenersFromOwner(owner);
            owner?.RemoveOnStateModuleAddedListener(OnStateModuleAdded);
        }

        private void OnStateModuleAdded(StateModule module)
        {
            if (module is WalkStateInitializer) {
                initializer = module as WalkStateInitializer;
            }
        }

        protected sealed override void SetIncompatibleStateTransitions(ref MovementStateType[] results) {
            results = GetExcludedStateTypes(new MovementStateType[] { MovementStateType.Fall, MovementStateType.Idol, MovementStateType.Run, MovementStateType.Crouch, MovementStateType.Jump });
        }

        protected sealed override void SetPlayerStateType(ref MovementStateType type)  {
            type = MovementStateType.Walk;
        }
    }
}
