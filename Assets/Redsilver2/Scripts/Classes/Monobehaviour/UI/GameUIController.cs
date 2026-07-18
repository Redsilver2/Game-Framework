using RedSilver2.Framework.Inputs;
using System.Collections;
using UnityEngine;


namespace RedSilver2.Framework.UI
{
    public sealed class GameUIController : MonoBehaviour
    {
        [SerializeField] private UISelector pauseMenuSelector;
        private IEnumerator selectorUpdater = null;
        private UISelector actifUISelector;

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

        private void UpdateUI(UISelector selector, float enableWaitTime) {
            StopUIUpdate();
            actifUISelector = selector;

            if(actifUISelector != null) {
                selectorUpdater = actifUISelector.UpdateSelections(enableWaitTime);
                StartCoroutine(selectorUpdater);
            }
        }

        private void StopUIUpdate()
        {
            actifUISelector?.StopUpdate();

            if (selectorUpdater != null)
            {
                StopCoroutine(selectorUpdater);
                selectorUpdater = null;
            }

            actifUISelector = null;
        }




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

        public static void UpdateSelector(UISelector selector)
        {
            UpdateSelector(selector, 0f);
        }

        public static void UpdateSelector(UISelector selector, float enableWaitTime) {
            if(!IsActifSelector(selector))
                GetInstance()?.UpdateUI(selector, Mathf.Clamp(enableWaitTime, 0f, float.MaxValue));     
        }

        public static void StopActifSelector(UISelector selector)
        {
            if (IsActifSelector(selector)) 
               GetInstance()?.StopUIUpdate();
        }

        public static bool IsActifSelector(UISelector selector)
        {
            GameUIController controller = GetInstance();
            if (controller == null || selector == null) return false;
            return selector == controller.actifUISelector;

        }
        private static GameUIController GetInstance() { return GameManager.UIController; }
    }

}
