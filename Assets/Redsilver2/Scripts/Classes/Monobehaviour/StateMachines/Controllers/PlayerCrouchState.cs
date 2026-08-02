using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class PlayerCrouchState : CrouchState
    {
        [Space]
        [SerializeField] private bool hasToHoldInput = true;


        [Space]
        [SerializeField] private PressInputSettings pressCrouch;

        [Space]
        [SerializeField] private HoldInputSettings holdCrouch;
        protected sealed override void Awake()
        {
            base.Awake();

            if (enabled) {
                pressCrouch?.Enable();
                holdCrouch?.Enable();
            }
            else {
                pressCrouch?.Disable();
                holdCrouch?.Disable();
            }
        }

        protected sealed override void OnDisabled(MovementStateMachine stateMachine)
        {
            pressCrouch?.Disable();
            holdCrouch?.Disable();

            base.OnDisabled(stateMachine);

            if (stateMachine == null || !stateMachine.ContainsState(this)) return;
            stateMachine?.RemoveOnUpdateListener(OnUpdateCrouchInput(stateMachine));
        }

        protected sealed override void OnEnabled(MovementStateMachine stateMachine)
        {
            if (stateMachine == null || !stateMachine.ContainsState(this)) return;
            stateMachine?.AddOnUpdateListener(OnUpdateCrouchInput(stateMachine));

            pressCrouch?.Enable();
            holdCrouch?.Enable();

            base.OnEnabled(stateMachine);
        }



        private UnityAction OnUpdateCrouchInput(MovementStateMachine stateMachine)
        {
            return () => {
                if (stateMachine == null || !stateMachine.IsGrounded) SetIsCrouching(false);
                else if(RunState.IsStateMachineRunning(stateMachine) || JumpState.IsStateMachineJumping(stateMachine)) SetIsCrouching(false);
                else if (hasToHoldInput) {
                    SetIsCrouching(holdCrouch != null ? holdCrouch.GetValue() : false);
                }
                else if (!hasToHoldInput) {
                    SetIsCrouching(pressCrouch != null ? (pressCrouch.GetValue() ? !IsCrouching : IsCrouching) : false);
                }
            };
        }

        public void SetPressCrouchSetting(PressInputSettings pressCrouchSetting)
        {
            this.pressCrouch?.Disable();
            this.pressCrouch = pressCrouchSetting;

            if (enabled) pressCrouchSetting?.Enable();
            else pressCrouchSetting?.Disable();
        }

        public void SetHoldCrouchSetting(HoldInputSettings holdCrouchSetting)
        {
            this.holdCrouch?.Disable();
            this.holdCrouch = holdCrouchSetting;

            if (enabled) this.holdCrouch?.Enable();
            else this.holdCrouch?.Disable();
        }

        protected sealed override int GetLayerToIgnore() {
            return GameManager.PlayerLayer;
        }
    }
}