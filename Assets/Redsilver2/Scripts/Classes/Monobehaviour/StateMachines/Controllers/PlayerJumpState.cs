using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public class PlayerJumpState : JumpState
    {
        [Space]
        [SerializeField] private PressInputSettings pressInput;
        public PressInputSettings PressInput => pressInput;

        public PlayerJumpState(PlayerMovementStateMachine stateMachine) : base(stateMachine) {

        }
        
        public void SetPressInput(PressInputSettings pressInput) {
            this.pressInput = pressInput;
        }

        protected override void Update()
        {
            MovementStateMachine stateMachine = GetMovementStateMachine(this);
            base.Update();

            if(stateMachine != null && pressInput != null && CurrentJumpDelay <= 0f && JumpCount < MaxJumpCount) {
                if(pressInput.GetValue()) {
                    SetIsJumping(true);
                    if(JumpCount <= 0) { stateMachine?.ChangeState(this); }
                    else               { stateMachine?.ChangeState(this, false); }
                }
                else { SetIsJumping(false); }
            }
            else { SetIsJumping(false); }
        }
    }
}
