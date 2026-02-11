using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.StateMachines.States;

namespace RedSilver2.Framework.StateMachines
{
    public class PlayerJumpStateInitializer : JumpStateInitializer {

        private PressInput input;
        public  PressInput Input => input;

        protected override void Awake()
        {
            base.Awake();
            input = JumpState.GetPressInput();
        }

        protected override void Start() {
            base.Start();
            input?.Enable();
        }

        protected sealed override void OnDisable() {
            base.OnDisable();
            input?.Disable();
        }


        protected sealed override void OnEnable() {
            base.OnEnable();
            input?.Disable();
        }

        protected sealed override string GetModuleName()
        {
            return  "Player " +  base.GetModuleName();
        }

        protected sealed override void OnUpdate()
        {
            if (input != null)
            {
                input.Update();
                transitionState = input.Value;
            }
            else
            {
                transitionState = false;
            }

            base.OnUpdate();
        }
    }
}
