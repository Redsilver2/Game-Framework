using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;

namespace RedSilver2.Framework.Inputs.Configurations
{
    [CreateAssetMenu(fileName = "New Movement Input Configuration", menuName = "Input Configuration/Movement Input Configuration")]
    public class MovementInputConfiguration : ScriptableObject
    {
        [Space]
        [Header("Walk Input Settings")]
        public KeyboardVector2InputSettings moveSetting;

        [Space]
        [Header("Run Input Settings")]
        [SerializeField] private PressInputSettings pressRunSettings;
        [SerializeField] private HoldInputSettings  holdRunSettings;

        [Space]
        [Header("Crouch Input Settings")]
        [SerializeField] private PressInputSettings pressCrouchSettings;
        [SerializeField] private HoldInputSettings  holdCrouchSettings;

        [Space]
        [Header("Jump Input Settings")]
        [SerializeField] private PressInputSettings pressJumpSettings;
        private const GamepadStick GAMEPAD_MOVE_STICK = GamepadStick.LeftStick;

        private const string MOVE_INPUT         = "MOVE INPUT";
        private const string PRESS_JUMP_INPUT   = "PRESS JUMP INPUT";

        private const string HOLD_CROUCH_INPUT  = "HOLD CROUCH INPUT";
        private const string PRESS_CROUCH_INPUT = "PRESS CROUCH INPUT";

        private const string HOLD_RUN_INPUT     = "HOLD RUN INPUT";
        private const string PRESS_RUN_INPUT    = "PRESS RUN INPUT";


        //public void SetConfiguration(MovementStateConfiguration configuration) {
        //    if (configuration == null || configuration == this.configuration) return;
        //    ResetConfiguration();

        //} 

        private void ResetConfiguration() {

        }

        private void InitializeRunInput()
        {
          //  OverrideableHoldInput holdRun = 
        }

        public bool IsRunning()
        {
            //if(configuration == null || !configuration.IsRunningAllowed) 
                return false;

            return false;
        }

    }
}
