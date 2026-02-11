using RedSilver2.Framework.Inputs;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class JumpState : MovementState
    {
        private JumpStateInitializer initializer;
        private const string PRESS_JUMP_INPUT = "Press Jump";

        public JumpState(MovementStateMachine owner, JumpStateInitializer initializer) : base(owner) {
            this.initializer = initializer;
            
            AddOnStateEnteredListener(() => {
                if (this.initializer == null || MovementHandler == null) return;
                MovementHandler?.Jump(this.initializer.JumpForce);
            });

            owner?.AddOnStateModuleAddedListener(OnStateModuleAdded);
        }

        protected override void RemoveAllListenersFromOwner(StateMachine owner)
        {
            base.RemoveAllListenersFromOwner(owner);
            owner?.RemoveOnStateModuleAddedListener(OnStateModuleAdded);
        }

        private void OnStateModuleAdded(StateModule module)
        {
            if (module is JumpStateInitializer) initializer = module as JumpStateInitializer;
        }

        protected sealed override void SetIncompatibleStateTransitions(ref MovementStateType[] results) {
            results = GetExcludedStateTypes(new MovementStateType[] { MovementStateType.Fall });
        }

        protected sealed override void SetPlayerStateType(ref MovementStateType type) {
            type = MovementStateType.Jump;
        }

        public static OverrideablePressInput GetPressInput()
        {
            return InputManager.GetOrCreateOverrideablePressInput(PRESS_JUMP_INPUT, KeyboardKey.Space, GamepadButton.ButtonSouth);
        }
    }
}
