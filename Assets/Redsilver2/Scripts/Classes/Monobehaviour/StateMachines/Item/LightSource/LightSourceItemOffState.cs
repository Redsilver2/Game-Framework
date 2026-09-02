using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States {
    public class LightSourceItemOffState : LightSourceItemState
    {
        [Space]
        [SerializeField] private PressInputSettings inputSetting;

        public const LightSourceItemStateType TYPE = LightSourceItemStateType.Off;

        public LightSourceItemOffState(LightSourceItemStateMachine stateMachine) : base(stateMachine)
        {
        }

        public void SetLightEnableState(LightSourceItemStateMachine stateMachine)
        {
            Light light = stateMachine != null ? stateMachine.Light : null;
            if(light != null) light.enabled = false;
        }

        public sealed override bool CanTransition()
        {
            if (StateMachine == null|| inputSetting == null) return false;
            inputSetting?.Enable();

            return inputSetting.GetValue() && StateMachine.IsOn;
        }

        protected override void OnEntered()
        {
            StateMachine?.StopDrainingLightSource();

            base.OnEntered();
        }

        protected sealed override void SetLightSourceStateType(ref LightSourceItemStateType type)
        {
            type = LightSourceItemStateType.Off;
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

        public static LightSourceItemOffState GetState(LightSourceItemStateMachine stateMachine)
        {
            if (stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as LightSourceItemOffState;
        }
    }
}
