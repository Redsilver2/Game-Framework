using RedSilver2.Framework.Inputs;
using UnityEngine;


namespace RedSilver2.Framework.UI
{
    public sealed class GameUIController : MonoBehaviour
    {
        private const KeyboardKey   KEYBOARD_NAVIGATE_UP_KEY      = KeyboardKey.W;
        private const GamepadButton GAMEPAD_NAVIGATE_UP_BUTTON    = GamepadButton.DpadUp;

        private const KeyboardKey   KEYBOARD_NAVIGATE_DOWN_KEY    = KeyboardKey.S;
        private const GamepadButton GAMEPAD_NAVIGATE_DOWN_BUTTON  = GamepadButton.DpadDown;


        private const KeyboardKey   KEYBOARD_NAVIGATE_LEFT_KEY    = KeyboardKey.A;
        private const GamepadButton GAMEPAD_NAVIGATE_LEFT_BUTTON  = GamepadButton.DpadLeft;

        private const KeyboardKey   KEYBOARD_NAVIGATE_RIGHT_KEY   = KeyboardKey.D;
        private const GamepadButton GAMEPAD_NAVIGATE_RIGHT_BUTTON = GamepadButton.DpadRight;

        private const KeyboardKey   KEYBOARD_CONFIRM_KEY          = KeyboardKey.Enter;
        private const GamepadButton GAMEPAD_CONFIRM_BUTTON        = GamepadButton.ButtonSouth;


        public static bool GetNavigateUpState(bool getKeyDown)
        {
            if (getKeyDown)
                return InputManager.GetKeyDown(KEYBOARD_NAVIGATE_UP_KEY) || InputManager.GetKeyDown(GAMEPAD_NAVIGATE_UP_BUTTON);

            return InputManager.GetKey(KEYBOARD_NAVIGATE_UP_KEY) || InputManager.GetKey(GAMEPAD_NAVIGATE_UP_BUTTON);
        }

        public static bool GetNavigateDownState(bool getKeyDown)
        {
            if (getKeyDown)
                return InputManager.GetKeyDown(KEYBOARD_NAVIGATE_DOWN_KEY) || InputManager.GetKeyDown(GAMEPAD_NAVIGATE_DOWN_BUTTON);

            return InputManager.GetKey(KEYBOARD_NAVIGATE_DOWN_KEY) || InputManager.GetKey(GAMEPAD_NAVIGATE_DOWN_BUTTON);
        }

        public static bool GetNavigateLeftState(bool getKeyDown)
        {
            if (getKeyDown)
                return InputManager.GetKeyDown(KEYBOARD_NAVIGATE_LEFT_KEY) || InputManager.GetKeyDown(GAMEPAD_NAVIGATE_LEFT_BUTTON);

            return InputManager.GetKey(KEYBOARD_NAVIGATE_LEFT_KEY) || InputManager.GetKey(GAMEPAD_NAVIGATE_LEFT_BUTTON);
        }

        public static bool GetNavigateRightState(bool getKeyDown)
        {
            if (getKeyDown)
                return InputManager.GetKeyDown(KEYBOARD_NAVIGATE_RIGHT_KEY) || InputManager.GetKeyDown(GAMEPAD_NAVIGATE_RIGHT_BUTTON);

            return InputManager.GetKey(KEYBOARD_NAVIGATE_RIGHT_KEY) || InputManager.GetKey(GAMEPAD_NAVIGATE_RIGHT_BUTTON);
        }

        public static bool GetConfirmState()
        {
            return InputManager.GetKeyDown(KEYBOARD_CONFIRM_KEY) || InputManager.GetKeyDown(GAMEPAD_CONFIRM_BUTTON);
        }
    }

}
