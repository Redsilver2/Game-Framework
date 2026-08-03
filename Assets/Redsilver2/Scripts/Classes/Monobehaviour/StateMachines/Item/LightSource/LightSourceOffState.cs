
using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States {
    public class LightSourceOffState : LightSourceState
    {
        [Space]
        [SerializeField] private PressInputSettings inputSetting;

        public const LightSourceStateType TYPE = LightSourceStateType.Off;

        public void SetLightEnableState(LightSourceStateMachine stateMachine)
        {
            Light light = stateMachine != null ? stateMachine.Light : null;
            if(light != null) light.enabled = false;
        }

        public sealed override bool CanTransition(LightSourceStateMachine stateMachine)
        {
            if (stateMachine == null || inputSetting == null) return false;
            inputSetting?.Enable();

            return inputSetting.GetValue() && stateMachine.CurrentType == LightSourceStateType.On;
        }

        protected sealed override void SetLightSourceStateType(ref LightSourceStateType type)
        {
            type = LightSourceStateType.Off;
        }

        protected sealed override void SetIncompatibleTransitionStates(ref string[] incompatibleStates)
        {
            incompatibleStates = new string[] { TYPE.ToString() };
            base.SetIncompatibleTransitionStates(ref incompatibleStates);
        }

        public void SetInputSetting(PressInputSettings inputSetting)
        {
            this.inputSetting = inputSetting;
        }

        public static LightSourceOffState GetState(LightSourceStateMachine stateMachine)
        {
            if (stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as LightSourceOffState;
        }
    }
}
