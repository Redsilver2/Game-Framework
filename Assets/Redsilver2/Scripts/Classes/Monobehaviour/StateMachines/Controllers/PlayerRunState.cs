using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class PlayerRunState : RunState {
        [Space]
        [SerializeField] private bool hasToHoldInput = true;


        [Space]
        [SerializeField] private PressInputSettings pressRun;

        [Space]
        [SerializeField] private HoldInputSettings holdRun;

        private IEnumerator inputUpdate;

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

        protected sealed override void OnStateAdded(MovementState state)
        {
            base.OnStateAdded(state);
            if (state == this) StartInputUpdate();
        }

        protected sealed override void OnStateRemoved(MovementState state) {
            base.OnStateRemoved(state);
            if (state == this) StopInputUpdate();
        }

        private void StopInputUpdate() {
            if (inputUpdate != null) StopCoroutine(inputUpdate);
            inputUpdate = null;
        }

        private void StartInputUpdate() {
            StopInputUpdate();
            inputUpdate = InputUpdateCoroutine();
            StartCoroutine(inputUpdate);
        }

        private void UpdateInput(MovementStateMachine stateMachine) {
            if (stateMachine == null || !stateMachine.IsGrounded || !stateMachine.IsMoving || !enabled) SetIsRunning(false);
            else if (hasToHoldInput) { 
               SetIsRunning(holdRun != null ? holdRun.GetValue() : false); 
            }
            else if(!hasToHoldInput) {
                if (pressRun != null) {
                    if (pressRun.GetValue()) SetIsRunning(!IsRunning);
                }
                else SetIsRunning(false);
            }
        }

        private IEnumerator InputUpdateCoroutine()
        {
            while (true) {
                UpdateInput(StateMachine);
                yield return null;
            }
        }


        public static PlayerRunState GetState(PlayerMovementStateMachine stateMachine) {
             return GetState(stateMachine as MovementStateMachine) as PlayerRunState;
        }
    }
}