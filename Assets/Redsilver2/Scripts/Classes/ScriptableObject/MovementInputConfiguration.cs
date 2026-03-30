using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Settings;
using UnityEngine;

namespace RedSilver2.Framework.Inputs.Configurations
{
    [CreateAssetMenu(fileName = "New Movement Input Configuration", menuName = "Input Configuration/Movement Input Configuration")]
    public class MovementInputConfiguration : ScriptableObject
    {
        [Space]
        [Header("Walk Input Setting")]
        public KeyboardVector2InputSettings moveSetting;

        [Space]
        [Header("Crouch Input Setting")]
        [SerializeField] private KeyboardKey   keyboardCrouchKey = KeyboardKey.C;
        [SerializeField] private GamepadButton gamepadCrouchButton     = GamepadButton.ButtonEast;


        [Space]
        [Header("Jump Input Setting")]
        [SerializeField] private KeyboardKey   keyboardJumpKey   = KeyboardKey.Space;
        [SerializeField] private GamepadButton gamepadJumpButton = GamepadButton.ButtonSouth;


        private MovementConfiguration configuration;
        private const GamepadStick GAMEPAD_MOVE_STICK = GamepadStick.LeftStick;

        private const string MOVE_INPUT         = "MOVE INPUT";
        private const string PRESS_JUMP_INPUT   = "PRESS JUMP INPUT";

        private const string HOLD_CROUCH_INPUT  = "HOLD CROUCH INPUT";
        private const string PRESS_CROUCH_INPUT = "PRESS CROUCH INPUT";

        private const string HOLD_RUN_INPUT     = "HOLD RUN INPUT";
        private const string PRESS_RUN_INPUT    = "PRESS RUN INPUT";


        public void SetConfiguration(MovementConfiguration configuration) {
            if (configuration == null || configuration == this.configuration) return;
            ResetConfiguration();

        } 

        private void ResetConfiguration() {

        }

        private void InitializeRunInput()
        {
          //  OverrideableHoldInput holdRun = 
        }

        public bool IsRunning()
        {
            if(configuration == null || !configuration.IsRunningAllowed) 
                return false;

            return false;
        }

    }
}
