using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class PlayerRunState : RunState {
        [Space]
        [SerializeField] private bool hasToHoldInput = true;


        [Space]
        [SerializeField] private PressInputSettings pressRun;

        [Space]
        [SerializeField] private HoldInputSettings holdRun;

        protected sealed override void Awake()
        {
            base.Awake();

            if (enabled) {
                pressRun?.Enable();
                holdRun?.Enable();
            }
            else {
                pressRun?.Disable();
                holdRun?.Disable();
            }
        }


        protected sealed override void OnEnabled(MovementStateMachine stateMachine)
        {
            base.OnEnabled(stateMachine);

            if (stateMachine == null || !stateMachine.ContainsState(this)) return;
            stateMachine?.AddOnUpdateListener(OnUpdateRunInput(stateMachine));
        }

        protected sealed override void OnDisabled(MovementStateMachine stateMachine)
        {
            base.OnDisabled(stateMachine);

            if (stateMachine == null || !stateMachine.ContainsState(this)) return;
            stateMachine?.RemoveOnUpdateListener(OnUpdateRunInput(stateMachine));
        }


        private UnityAction OnUpdateRunInput(MovementStateMachine stateMachine)
        {
            return () => {
                if (stateMachine == null || !stateMachine.IsGrounded || !stateMachine.IsMoving || !enabled)                 SetIsRunning(false);
                else if(CrouchState.IsStateMachineCrouching(stateMachine) || JumpState.IsStateMachineJumping(stateMachine)) SetIsRunning(false);
                else if (hasToHoldInput)  SetIsRunning(holdRun != null ? holdRun.GetValue() : false);
                else if (!hasToHoldInput) {
                    if (pressRun != null) {
                        if (pressRun.GetValue()) SetIsRunning(!IsRunning);
                    }
                    else SetIsRunning(false);
                }
            };
        }


        protected sealed override void OnDisabled()
        {
            base.OnDisabled();
            pressRun?.Disable();
            holdRun?.Disable();
        }

        protected sealed override void OnEnabled()
        {
            base.OnEnabled();
            pressRun?.Enable();
            holdRun?.Enable();
        }

        public void SetPressRunSetting(PressInputSettings pressRunSetting)
        {
            this.pressRun?.Disable();
            this.pressRun = pressRunSetting;

            if (enabled) pressRunSetting?.Enable();
            else pressRunSetting?.Disable();
        }

        public void SetHoldRunSetting(HoldInputSettings holdRunSetting) {
            this.holdRun?.Disable();
            this.holdRun = holdRunSetting;

            if (enabled) this.holdRun?.Enable();
            else         this.holdRun?.Disable();
        }

        public static PlayerRunState GetState(PlayerMovementStateMachine stateMachine) {
             return GetState(stateMachine as MovementStateMachine) as PlayerRunState;
        }
    }
}