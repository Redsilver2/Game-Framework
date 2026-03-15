using RedSilver2.Framework.Animations;
using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using UnityEngine;

namespace RedSilver2.Framework.Items
{
    public abstract class TurnFlashlightAction : FlashlightAction
    {
        [Space]
        [SerializeField] private AnimationData turnAnimationData;

        [Space]
        [SerializeField] private float defaultTurnEventTriggerTime;

        protected override void Awake()
        {
            base.Awake();
            turnAnimationData?.AddTimestampEvent(defaultTurnEventTriggerTime, OnTurnEventTriggered);
            AddInputAction(GetPressInputAction());
        }

        private void OnExecute()
        {
            if (TryGetFlashlight(out Flashlight flashlight))  {
                flashlight?.PlayAnimation(turnAnimationData);
            }
        }

        private PressInputAction GetPressInputAction()
        {
            PressInputAction pressInput = new PressInputAction("TURN_ACTION", InputManager.GetOrCreateOverrideablePressInput(
                                                                "PRESS_FLASHLIGHT_TURN_ACTION", KeyboardKey.F, GamepadButton.RightStickPress));

            pressInput.AddOnExecutedListener(OnExecute);
            return pressInput;
        }

        private void OnTurnEventTriggered()
        {
            if (TryGetFlashlight(out Flashlight flashlight))
            {
                Debug.Log("What?!");
                OnTurnEventTriggered(flashlight);
            }
        }

        protected abstract void OnTurnEventTriggered(Flashlight flashlight);
    }

}
