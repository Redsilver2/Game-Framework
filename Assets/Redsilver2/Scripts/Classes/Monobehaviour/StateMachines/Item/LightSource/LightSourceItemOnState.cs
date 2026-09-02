using RedSilver2.Framework.Animations;
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

        public const LightSourceItemStateType TYPE = LightSourceItemStateType.On;

        public LightSourceItemOnState(LightSourceItemStateMachine stateMachine) : base(stateMachine) {

        }

        protected override void OnEntered() {
            base.OnEntered();
            StateMachine?.StartDrainingLightSource(lightActivationWaitTime);
        }

        public sealed override bool CanTransition()
        {
            if (StateMachine == null || inputSetting == null) return false;
            inputSetting?.Enable();

            return inputSetting.GetValue() && !StateMachine.IsOn && StateMachine.LifeTime > 0f;
        }

        protected sealed override void SetLightSourceStateType(ref LightSourceItemStateType type) {
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