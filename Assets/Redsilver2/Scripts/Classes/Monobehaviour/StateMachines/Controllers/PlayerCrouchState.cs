using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private IEnumerator inputUpdate;


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

        protected sealed override void OnDisabled()
        {
            base.OnDisabled();
            pressCrouch?.Disable();
            holdCrouch?.Disable();
        }

        protected sealed override void OnEnabled()
        {
            base.OnEnabled();
            pressCrouch?.Enable();
            holdCrouch?.Enable();
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

        protected sealed override void OnStateAdded(MovementState state)
        {
            base.OnStateAdded(state);
            if (state == this) StartInputUpdate();
        }

        protected sealed override void OnStateRemoved(MovementState state)
        {
            base.OnStateRemoved(state);
            if (state == this) StopInputUpdate();
        }

        private void StopInputUpdate()
        {
            if (inputUpdate != null) StopCoroutine(inputUpdate);
            inputUpdate = null;
        }

        private void StartInputUpdate()
        {
            StopInputUpdate();
            inputUpdate = InputUpdateCoroutine();
            StartCoroutine(inputUpdate);
        }

        private void UpdateInput(MovementStateMachine stateMachine)
        {
            if (stateMachine == null || !stateMachine.IsGrounded || RunState.GetIsRunning(stateMachine) || !enabled) SetIsCrouching(false);
            else if (hasToHoldInput) {
                SetIsCrouching(holdCrouch != null ? holdCrouch.GetValue() : false);
            }
            else if (!hasToHoldInput) {
                if (pressCrouch != null) {
                    if (pressCrouch.GetValue()) SetIsCrouching(!IsCrouching);
                }
                else SetIsCrouching(false);
            }
        }

        private IEnumerator InputUpdateCoroutine()
        {
            while (true)
            {
                UpdateInput(StateMachine);
                yield return null;
            }
        }

    }
}