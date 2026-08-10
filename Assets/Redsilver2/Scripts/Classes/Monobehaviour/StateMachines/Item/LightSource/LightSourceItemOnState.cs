using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public class LightSourceItemOnState : LightSourceItemState
    {
        [Space]
        [SerializeField] private float lightActivationWaitTime;

        [Space]
        [SerializeField] private PressInputSettings inputSetting;

        public const LightSourceStateType TYPE = LightSourceStateType.On;

        public void SetLightEnableState(LightSourceItemStateMachine stateMachine)
        {
            Light light = stateMachine != null ? stateMachine.Light : null;
            if (light != null) light.enabled = true;
        }

        protected override void OnEntered(LightSourceItemStateMachine stateMachine)
        {
            stateMachine?.StartDrainingLightSource(lightActivationWaitTime);
            base.OnEntered(stateMachine);
        }

        public sealed override bool CanTransition(LightSourceItemStateMachine stateMachine)
        {
            if (stateMachine == null || inputSetting == null) return false;
            inputSetting?.Enable();

            return inputSetting.GetValue() && !stateMachine.IsOn && stateMachine.LifeTime > 0f;
        }

        protected sealed override void SetLightSourceStateType(ref LightSourceStateType type) {
            type = TYPE;
        }

        public void SetInputSetting(PressInputSettings inputSetting) {
            this.inputSetting = inputSetting;
        }

        protected sealed override void SetIncompatibleTransitionStates(ref string[] incompatibleStates)
        {
            incompatibleStates = new string[] { TYPE.ToString() };
            base.SetIncompatibleTransitionStates(ref incompatibleStates);
        }

        public static LightSourceItemOnState GetState(LightSourceItemStateMachine stateMachine)
        {
            if(stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as LightSourceItemOnState;
        }
    }
}