using RedSilver2.Framework.Inputs.Configurations;
using RedSilver2.Framework.Inputs.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace RedSilver2.Framework.Inputs
{
    public static partial class InputManager
    {
        public static void Update() {
            UpdateInputConfigurations();
        }

        public static void LateUpdate() {
           LateUpdateInputConfigurations();
        }

        private static void UpdateInputConfigurations()
        {
            foreach (InputConfiguration configuration in GetActifConfigurations())
                configuration?.Update();
        }

        private static void LateUpdateInputConfigurations()
        {
            foreach (InputConfiguration configuration in GetActifConfigurations())
                configuration?.LateUpdate();
        }

        private static InputConfiguration[] GetActifConfigurations()
        {
            InputConfiguration[] configurations = GetInputConfigurations();
            return configurations.Where(x => x != null).Where(x => x.IsEnabled).ToArray();
        }
    }

    public static partial class InputManager
    {
        private static Dictionary<string, InputHandler> inputHandlers             = new Dictionary<string, InputHandler>();
        private static Dictionary<string, InputConfiguration> inputConfigurations = new Dictionary<string, InputConfiguration>();

        private static Dictionary<KeyboardKey  , InputControl> keyboardKeysControls   = GetKeyboardKeysControls();
        private static Dictionary<MouseButton  , InputControl> mouseButtonsControls   = GetMouseButtonsControls();
        private static Dictionary<GamepadButton, InputControl> gamepadButtonsControls = GetGamepadButtonsControls();

        private static readonly KeyboardKey[] excludedWritingKeys = GetExcludedWritingKey();

        public static bool AnyKeyboardKey
        {
            get
            {
                if (keyboardKeysControls == null) return false;
                return keyboardKeysControls.Keys.Where(x => GetKey(x)).Count() > 0;
            }
        }
        public static bool AnyMouseButton
        {
            get
            {
                if (mouseButtonsControls == null) return false;
                return mouseButtonsControls.Keys.Where(x => GetKey(x)).Count() > 0;
            }
        }

        public static bool AnyGamepadButton
        {
            get
            {
                if (gamepadButtonsControls == null) return false;
                return gamepadButtonsControls.Keys.Where(x => GetKey(x)).Count() > 0;
            }
        }
        public static bool AnyKeyboardKeyDown
        {
            get
            {
                if (keyboardKeysControls == null) return false;
                return keyboardKeysControls.Keys.Where(x => GetKeyDown(x)).Count() > 0;
            }
        }

        public static bool AnyMouseButtonDown
        {
            get
            {
                if (mouseButtonsControls == null) return false;
                return mouseButtonsControls.Keys.Where(x => GetKeyDown(x)).Count() > 0;
            }
        }
        public static bool AnyGamepadButtonDown
        {
            get
            {
                if (gamepadButtonsControls == null) return false;
                return gamepadButtonsControls.Keys.Where(x => GetKeyDown(x)).Count() > 0;
            }
        }

        public static bool AnyKeyboardKeyUp
        {
            get
            {
                if (keyboardKeysControls == null) return false;
                return keyboardKeysControls.Keys.Where(x => GetKeyUp(x)).Count() > 0;
            }
        }
        public static bool AnyMouseButtonUp
        {
            get
            {
                if (mouseButtonsControls == null) return false;
                return mouseButtonsControls.Keys.Where(x => GetKeyUp(x)).Count() > 0;
            }
        }

        public static bool AnyGamepadButtonUp
        {
            get
            {
                if (gamepadButtonsControls == null) return false;
                return gamepadButtonsControls.Keys.Where(x => GetKeyUp(x)).Count() > 0;
            }
        }

        public static bool AnyKey => AnyKeyboardKey || AnyMouseButton || AnyGamepadButton;
        public static bool AnyKeyDown => AnyKeyboardKeyDown || AnyMouseButtonDown || AnyGamepadButtonDown;
        public static bool AnyKeyUp => AnyKeyboardKeyUp || AnyMouseButtonUp || AnyGamepadButtonUp;


        public const string KEYBOARD_ICONS_PATH = "Sprites/Inputs/Keyboard/";
        public const string MOUSE_ICONS_PATH    = "Sprites/Inputs/Mouse/";

        public const string GAMEPAD_ICONS_PATH = "Sprites/Inputs/Gamepad/";
        public const string XR_ICONS_PATH      = "Sprites/Inputs/XR/";

        public const string KEYBOARD_ROOT_PATH = "<Keyboard>/";
        public const string MOUSE_ROOT_PATH    = "<Mouse>/";

        public const string GAMEPAD_ROOT_PATH       = "<Gamepad>/";
        public const string XR_CONTROLLER_ROOT_PATH = "<XRController>/";
        #region Input Datas

        public static InputControl GetInputControl(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            if (path.Contains("Keyboard", StringComparison.OrdinalIgnoreCase))
                return GetKeyboardControl(path);
            else if (path.Contains("Mouse", StringComparison.OrdinalIgnoreCase))
                return GetMouseControl(path);
            else if (path.Contains("Gamepad", StringComparison.OrdinalIgnoreCase))
                return GetGamepadControl(path);

            return null;
        }

        private static KeyboardKey[] GetExcludedWritingKey()
        {
            string[] excluded = new string[] {
                "F"    , "Alt"   , "Command" , "Ctrl", "Arrow", 
                "Lock", "Print"  , "Pause"   , "Home", "End",
                "Page", "Escape" , "Shift"   , "Tab", "Return"
            };

            return GetKeyboardKeys().Where(x => IsExcludedWritingKey(x, excluded))
                                    .ToArray();
        }

        private static bool IsExcludedWritingKey(KeyboardKey key, string[] excluded) {
            if (excluded == null || key == KeyboardKey.F) return false;

            return excluded.Where(x => key.ToString().Contains(x))
                           .Count() > 0;
        }

        public static bool IsExcludedWritingKey(KeyboardKey key) {
            if(excludedWritingKeys == null) return false;
            return excludedWritingKeys.Contains(key);
        }


        #region Initialization
        private static Dictionary<KeyboardKey, InputControl> GetKeyboardKeysControls()
        {
            Dictionary<KeyboardKey, InputControl> results = new Dictionary<KeyboardKey, InputControl>();

            foreach (KeyboardKey key in GetKeyboardKeys())
                results.Add(key, CreateKeyboardControl(key));

            return results;
        }

        private static Dictionary<MouseButton, InputControl> GetMouseButtonsControls()
        {
            Dictionary<MouseButton, InputControl> results = new Dictionary<MouseButton, InputControl>();

            foreach (MouseButton key in GetMouseButtons())
            {
                string keyString = key.ToString();
                results.Add(key, new InputButtonControl($"{MOUSE_ROOT_PATH}{GetFormattedKey(keyString)}", null));
            }

            return results;
        }

        private static Dictionary<GamepadButton, InputControl> GetGamepadButtonsControls()
        {
            Dictionary<GamepadButton, InputControl> results = new Dictionary<GamepadButton, InputControl>();

            foreach (GamepadButton key in GetGamepadButtons())
                AddGamepadButtonData(ref results, key);

            return results;
        }

        private static InputButtonControl CreateKeyboardControl(KeyboardKey key) {
            string path = GetFormattedKey(key);

            if (!IsExcludedWritingKey(key)) {
                if (TryGetCharacterInputControl(path, key, out InputStringCharacter result01) || TryGetNumberInputControl(path, key, out result01))
                    return result01;
                else if (TryGetSpecialCharacterInputControl(path, key, out result01))
                    return result01;
                else if (key == KeyboardKey.Backspace || key == KeyboardKey.Delete)
                    return new InputStringDelete(path);
            }

            return new InputButtonControl(path);
        }

        private static bool TryGetCharacterInputControl(string path, KeyboardKey key, out InputStringCharacter result) {
            result = null;

            if(key == KeyboardKey.Space) {
                result = new InputStringCharacter(path, ' ');
            }
            if (IsCharacterKey(key)) {
                char value = key.ToString().ToCharArray()[0];
                result = new InputStringCharacter(path, value);
            }
          
            return result != null;
        }

        private static bool TryGetSpecialCharacterInputControl(string path, KeyboardKey key, out InputStringCharacter result)
        {
            result = null;

            switch (key) {
                case KeyboardKey.Backquote:    result = new InputStringCharacter(path, '~', '`'); break;
                case KeyboardKey.Quote:        result = new InputStringCharacter(path, '\"', '\''); break;
                case KeyboardKey.LeftBracket:  result = new InputStringCharacter(path, '{', '['); break;    
                case KeyboardKey.RightBracket: result = new InputStringCharacter(path, '}', ']'); break;
                case KeyboardKey.Comma:        result = new InputStringCharacter(path, '<', ',');  break;   
                case KeyboardKey.Semicolon:    result = new InputStringCharacter(path, ':', ';'); break;
                case KeyboardKey.Minus:        result = new InputStringCharacter(path, '_', '-'); break;
                case KeyboardKey.Equals:       result = new InputStringCharacter(path, '+', '='); break;
                case KeyboardKey.Period:       result = new InputStringCharacter(path, '>', '.'); break;
                case KeyboardKey.Slash:        result = new InputStringCharacter(path, '?', '/'); break;
                case KeyboardKey.Backslash:    result = new InputStringCharacter(path, '|', '\\'); break;
                case KeyboardKey.NumpadMinus:  result = new InputStringCharacter(path, '_', '-');  break;
                case KeyboardKey.NumpadEquals: result = new InputStringCharacter(path, '+', '='); break;
                case KeyboardKey.NumpadPeriod: result = new InputStringCharacter(path, '>', '.'); break;   
            }

            return result != null;
        }

        private static bool TryGetNumberInputControl(string path, KeyboardKey key, out InputStringCharacter result)
        {
            result = null;
            if (!IsNumberKey(key)) return false;

            if (TryGetNumberInputControl(path, key.ToString(), out result)){
                return true;
            }

            return false;
        }
        private static bool TryGetNumberInputControl(string path, string key, out InputStringCharacter result)
        {
            char[] stringNumberExpressions = new char[] { ')', '!', '@', '#', '$', '%', '^', '&', '*', '(' };

            for (int i = 0; i < stringNumberExpressions.Length; i++) {
                char charNumber = i.ToString().ToCharArray()[0];

                if (key.Contains(charNumber, StringComparison.OrdinalIgnoreCase)) {
                    result = new InputStringCharacter(path, stringNumberExpressions[i], charNumber);
                    return true;
                }
            }

            result = null;
            return false;
        }

        public static bool IsAlphaNumberKey(KeyboardKey key) {
            return key >= KeyboardKey.Alpha0 && key <= KeyboardKey.Alpha9;
        }

        public static bool IsNumpadNumberKey(KeyboardKey key) {
            return key >= KeyboardKey.Numpad0 && key <= KeyboardKey.Numpad9;
        }

        public static bool IsNumpadKey(KeyboardKey key) {
            return IsNumberKey(key) ||
                   (key >= KeyboardKey.NumpadPeriod && key <= KeyboardKey.NumpadEquals);
        }

        public static bool IsNumberKey(KeyboardKey key) {
            return IsAlphaNumberKey(key) ||
                   IsNumpadNumberKey(key);
        }

        public static bool IsCharacterKey(KeyboardKey key) {
            return key >= KeyboardKey.Q && key <= KeyboardKey.M;
        }

        public static bool IsSpecialCharacterKey(KeyboardKey key) {
            if (IsExcludedWritingKey(key) || IsNumberKey(key) || IsCharacterKey(key))
                return false;

            return true;             
        }

        public static KeyboardKey[] GetCharacterKeys() {
            return GetKeyboardKeys().Where(x => IsCharacterKey(x)).ToArray();
        }

        public static KeyboardKey[] GetNumberKeys() {
            return GetKeyboardKeys().Where(x => IsNumberKey(x)).ToArray();
        }

        public static KeyboardKey[] GetAlphaNumberKeys(){
            return GetKeyboardKeys().Where(x => x.ToString().Contains("Alpha", StringComparison.OrdinalIgnoreCase))
                                    .ToArray();
        }


        public static KeyboardKey[] GetNumpadNumberKeys()
        {
            return GetKeyboardKeys().Where(x => x.ToString().Contains("Numpad", StringComparison.OrdinalIgnoreCase))
                                    .ToArray();
        }

        private static void AddGamepadButtonData(ref Dictionary<GamepadButton, InputControl> results, GamepadButton button)
        {
            if (results == null) results = new Dictionary<GamepadButton, InputControl>();

            string buttonString = button.ToString();
         

            if (buttonString.Contains("Stick", StringComparison.OrdinalIgnoreCase))
                AddGamepadStickButton(ref results, null, buttonString, button);
            else
                AddGamepadButton(ref results, null, button);
        }


        private static void AddGamepadStickButton(ref Dictionary<GamepadButton, InputControl> results, Sprite icon, string buttonString, GamepadButton button)
        {
            if (buttonString.Contains("Press", StringComparison.OrdinalIgnoreCase))
                AddGamepadButton(ref results, icon, button);
            else
            {
                InputControl control = GetGamepadStickDirectionControl(buttonString, icon); ;
                if (control != null) results?.Add(button, control);
            }
        }

        private static GamepadStickDirectionControl GetGamepadStickDirectionControl(string buttonString, Sprite icon)
        {
            if (string.IsNullOrEmpty(buttonString)) return null;
            bool isLeftStick = buttonString.ToLower().ToArray()[1].Equals('l');

            if (buttonString.Contains("Up", StringComparison.OrdinalIgnoreCase))
                return new GamepadStickUpControl(isLeftStick, icon);
            else if (buttonString.Contains("Down", StringComparison.OrdinalIgnoreCase))
                return new GamepadStickDownControl(isLeftStick, icon);
            else if (buttonString.Contains("Left", StringComparison.OrdinalIgnoreCase))
                return new GamepadStickLeftControl(isLeftStick, icon);

            return new GamepadStickRightControl(isLeftStick, icon);
        }

        private static void AddGamepadButton(ref Dictionary<GamepadButton, InputControl> results, Sprite icon, GamepadButton button)
        {
            results?.Add(button, new InputButtonControl(GetFormattedKey(button), icon));
        }
        #endregion

        #region Key Formatting

        private static string GetFormattedKey(string keyString)
        {
            if (string.IsNullOrEmpty(keyString)) return string.Empty;

            char character = char.ToLower(keyString.ToCharArray()[0]);
            keyString = keyString.Remove(0, 1);

            return $"{character}{keyString}";
        }

        #region Keyboard 
        private static string GetFormattedKey(KeyboardKey key)
        {
            string keyString = key.ToString();

            if (keyString.ToLower().Contains("alpha"))
                return $"{KEYBOARD_ROOT_PATH}{char.ToString(keyString[keyString.Length - 1])}";

            return $"{KEYBOARD_ROOT_PATH}{GetFormattedKey(keyString)}";
        }
        #endregion

        #region Gamepad
        private static string GetFormattedKey(GamepadButton button)
        {
            if (TryFormatGamepadDpadKey(button, out string currentDpad))
                return currentDpad;

            return GetFormattedKey($"{GAMEPAD_ROOT_PATH}{GetFormattedKey(button.ToString())}");
        }

        private static bool TryFormatGamepadDpadKey(GamepadButton key, out string result)
        {
            string currentKey = key.ToString();
            if (!currentKey.Contains("Dpad")) { result = string.Empty; return false; }

            result = $"{GAMEPAD_ROOT_PATH}dpad/{GetFormattedKey(currentKey.Split("Dpad")[1])}";
            return true;
        }
        #endregion


        #endregion

        #region Get

        #region Keyboard

        public static InputControl GetKeyboardControl(string path)
        {
            if (string.IsNullOrEmpty(path) || keyboardKeysControls == null)
                return null;

            var results = keyboardKeysControls.Values.Where(x => x.ComparePath(path));

            if (results.Count() > 0) return results.First();
            return null;
        }

        public static InputControl GetKeyboardControl(KeyboardKey key)
        {
            if (keyboardKeysControls == null || !keyboardKeysControls.ContainsKey(key))
                return null;

            return keyboardKeysControls[key];
        }


        public static InputControl[] GetKeyboardControls() => GetKeyboardControls(null);
        public static InputControl[] GetKeyboardControls(KeyboardKey[] excludedKeys)
        {
            List<InputControl> results = new List<InputControl>();
            if (excludedKeys != null) excludedKeys = excludedKeys.Distinct().ToArray();

            foreach (KeyboardKey key in Enum.GetValues(typeof(KeyboardKey)))
            {
                if (excludedKeys != null)
                    if (excludedKeys.Contains(key)) continue;

                results.Add(keyboardKeysControls[key]);
            }

            return results.ToArray();
        }
        #endregion


        #region Mouse

        private static InputControl GetMouseControl(string path)
        {
            if (string.IsNullOrEmpty(path) || mouseButtonsControls == null)
                return null;

            var results = mouseButtonsControls.Values.Where(x => x.ComparePath(path));

            if (results.Count() > 0) return results.First();
            return null;
        }

        public static InputControl GetMouseControl(MouseButton button)
        {
            if (mouseButtonsControls == null || !mouseButtonsControls.ContainsKey(button))
                return null;
            return mouseButtonsControls[button];
        }

        public static InputControl[] GetMouseControls() => GetMouseControls(null);
        public static InputControl[] GetMouseControls(MouseButton[] excludedKeys)
        {
            List<InputControl> results = new List<InputControl>();
            if (excludedKeys != null) excludedKeys = excludedKeys.Distinct().ToArray();

            foreach (MouseButton key in Enum.GetValues(typeof(MouseButton)))
            {
                if (excludedKeys != null)
                {
                    if (excludedKeys.Contains(key)) continue;
                }
                results?.Add(mouseButtonsControls[key]);
            }

            return results.ToArray();
        }
        #endregion


        #region Gamepad
        private static InputControl GetGamepadControl(string path)
        {
            if (string.IsNullOrEmpty(path) || gamepadButtonsControls == null)
                return null;

            var results = gamepadButtonsControls.Values.Where(x => x.ComparePath(path));

            if (results.Count() > 0) return results.First();
            return null;
        }

        public static InputControl GetGamepadControl(GamepadButton button)
        {
            if (gamepadButtonsControls == null || !gamepadButtonsControls.ContainsKey(button))
                return null;
            return gamepadButtonsControls[button];
        }


        public static InputControl[] GetGamepadControls() => GetGamepadControls(null);
        public static InputControl[] GetGamepadControls(GamepadButton[] excludedKeys)
        {
            List<InputControl> results = new List<InputControl>();
            if (excludedKeys != null) excludedKeys = excludedKeys.Distinct().ToArray();

            foreach (GamepadButton key in Enum.GetValues(typeof(GamepadButton)))
            {
                if (excludedKeys != null)
                {
                    if (excludedKeys.Contains(key)) continue;
                }

                results.Add(gamepadButtonsControls[key]);
            }

            return results.ToArray();
        }
        #endregion

        #endregion

        #endregion

        #region Input Path

        #region Keyboard 
        public static string GetPath(KeyboardKey key)
        {
            if (keyboardKeysControls == null || !keyboardKeysControls.ContainsKey(key)) return string.Empty;
            return keyboardKeysControls[key].Path;
        }


        public static string[] GetKeyboardPaths() => GetKeyboardPaths(null);
        public static string[] GetKeyboardPaths(KeyboardKey[] excludedKeys)
        {
            List<string> results = new List<string>();

            foreach (var result in GetKeyboardControls(excludedKeys)) results.Add(result.Path);
            return results.ToArray();
        }
        #endregion

        #region Mouse 
        public static string GetPath(MouseButton button)
        {
            if (mouseButtonsControls == null || !mouseButtonsControls.ContainsKey(button)) return string.Empty;
            return mouseButtonsControls[button].Path;
        }

        public static string[] GetMousePaths() => GetMousePaths(null);
        public static string[] GetMousePaths(MouseButton[] excludedKeys)
        {
            List<string> results = new List<string>();

            foreach (var result in GetMouseControls(excludedKeys)) results.Add(result.Path);
            return results.ToArray();
        }
        #endregion

        #region Gamepad
        public static string GetPath(GamepadButton button)
        {
            if (gamepadButtonsControls == null || !gamepadButtonsControls.ContainsKey(button)) return string.Empty;
            return gamepadButtonsControls[button].Path;
        }

        public static string[] GetGamepadPaths() => GetGamepadPaths(null);
        public static string[] GetGamepadPaths(GamepadButton[] excludedKeys)
        {
            List<string> results = new List<string>();

            foreach (var result in GetGamepadControls(excludedKeys)) results.Add(result.Path);
            return results.ToArray();
        }
        #endregion


        #endregion

        #region Input Icon
        private static Sprite GetKeyIconFromResources(string fileName, string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            return Resources.Load<Sprite>($"{path}{fileName}");
        }

        #region Keyboard 

        public static Sprite GetKeyIcon(KeyboardKey key)
        {
            if (keyboardKeysControls == null || !keyboardKeysControls.ContainsKey(key)) return null;
            return keyboardKeysControls[key].Icon;
        }

        public static void OverrideKeyIcon(KeyboardKey key, Sprite icon)
        {
            if (keyboardKeysControls == null || !keyboardKeysControls.ContainsKey(key)) return;
            keyboardKeysControls[key].OverrideIcon(icon);
        }

        #endregion

        #region Gamepad

        public static Sprite GetKeyIcon(GamepadButton button)
        {
            if (gamepadButtonsControls == null || !gamepadButtonsControls.ContainsKey(button)) return null;
            return gamepadButtonsControls[button].Icon;
        }

        public static void OverrideKeyIcon(GamepadButton button, Sprite icon)
        {
            if (keyboardKeysControls == null || !gamepadButtonsControls.ContainsKey(button)) return;
            gamepadButtonsControls[button].OverrideIcon(icon);
        }

        #endregion


        #endregion


        public static bool Write(ref string text) {
           return Write(int.MaxValue, ref text);
        }
        public static bool Write(int wordslimit, ref string text)
        {
            return Write(InputManager.GetKey(KeyboardKey.LeftShift) || 
                         InputManager.GetKey(KeyboardKey.RightShift), wordslimit, ref text);
        }

        public static bool Write(bool isUpperCase, int wordslimit, ref string text) {
            if(keyboardKeysControls == null) return false;
            return Write(GetActifWritteableInput(), isUpperCase, wordslimit, ref text);
        }

        public static bool Write(KeyboardKey key, bool isUpperCase, ref string text) {
            return Write(key, isUpperCase, int.MaxValue, ref text);
        }
        public static bool Write(KeyboardKey key, bool isUpperCase, int wordslimit, ref string text) {
            if (IsExcludedWritingKey(key)) return false;
            return Write(GetWriteableInput(key), isUpperCase, wordslimit, ref text);
        }

        private static bool Write(IWriteableInput writteable, bool isUpperCase, int wordsLimit, ref string text) {
            if(writteable == null) return false;

            writteable.Write(isUpperCase, wordsLimit, ref text, out bool wasExecuted);
            return wasExecuted;
        }

        public static char GetWriteableInputChar(KeyboardKey key, bool getUpperCase) {

            if (keyboardKeysControls == null || IsExcludedWritingKey(key)) return '\0';

            if (keyboardKeysControls.ContainsKey(key)) {
                IWriteableInput writteable = keyboardKeysControls[key] as IWriteableInput;
                if (writteable != null) return writteable.GetValue(getUpperCase);
            }
            
            return '\0';
        }

        public static string GetWriteableInputString(KeyboardKey key, bool getUpperCase)
        {
            string result = InputManager.GetWriteableInputChar(key, getUpperCase).ToString();

            if (string.IsNullOrEmpty(result) || result.Contains('\0') || result.Contains(' '))
                return GetWriteableInputString(key);

            return result;
        }

        private static string GetWriteableInputString(KeyboardKey key)
        {
            if      (key == KeyboardKey.Backspace || key == KeyboardKey.Delete) { return "Delete"; }
            else if (key == KeyboardKey.Enter || key == KeyboardKey.Insert) { return "Enter"; }
            else if (key == KeyboardKey.Space) { return "Space"; }



            return key.ToString();
        }

        public static char GetInputChar(bool getUpperCase) {
            if (keyboardKeysControls == null) return '\0';
            IWriteableInput writteable = GetActifWritteableInput();

            if (writteable != null) return writteable.GetValue(getUpperCase);
            return '\0';
        }

        public static char GetInputChar() {
            return GetInputChar(InputManager.GetKey(KeyboardKey.LeftShift) ||
                                InputManager.GetKey(KeyboardKey.RightShift));
        }

        public static bool TryGetWritteableInputKey(IWriteableInput writteable, out KeyboardKey key)
        {
            key = default;

            if (writteable == null || keyboardKeysControls == null) return false;
            var results = keyboardKeysControls.Where(x => x.Value.Equals(writteable));

            if (results.Count() > 0) {
                key = results.First().Key;
                return true;
            }

            return false;
        }

        public static bool TryGetActifWritteableInputKey(out KeyboardKey key) {
            return TryGetWritteableInputKey(GetActifWritteableInput(), out key);
        }


        public static IWriteableInput GetActifWritteableInput()
        {
            var results = GetActifWriteableInputs();

            return results == null || results.Count() <= 0 ? null :
                   results.OrderBy(x => (int)x.GetStringOperationType())
                          .First();  
        }

        public static IWriteableInput GetWriteableInput(KeyboardKey key)
        {
            if (keyboardKeysControls == null) return null;

            if (keyboardKeysControls.ContainsKey(key)) return keyboardKeysControls[key] as IWriteableInput;
            return null;
        }

        private static IWriteableInput[] GetActifWriteableInputs()
        {
            if(keyboardKeysControls == null) return new  IWriteableInput[0];
            var pressedInputs = GetWritingKeys().Where(x => InputManager.GetKeyDown(x));

            var dictonnary       = keyboardKeysControls.Where(x => pressedInputs.Contains(x.Key))
                                                    .ToDictionary(x => x.Key, x => x.Value);

            List<IWriteableInput> results = new List<IWriteableInput>();

            foreach(InputControl control in dictonnary.Values)
                 if(control is IWriteableInput) results.Add(control as IWriteableInput);
                
            return results.ToArray();   
        }

        public static IWriteableInput[] GetWriteableInputs()
        {
            if (keyboardKeysControls == null) return new IWriteableInput[0];
            return keyboardKeysControls.Values.Where(x => x is IWriteableInput).ToArray() as IWriteableInput[];
        }

        public static KeyboardKey[] GetWritingKeys()
        {
            var results = GetKeyboardKeys();
            if (excludedWritingKeys == null) return results;

            return results.Where(x => !IsExcludedWritingKey(x))
                          .ToArray();
        }

        public static KeyboardKey[] GetKeyboardKeys()
        {
            return Enum.GetValues(typeof(KeyboardKey)) as KeyboardKey[];  
        }

        public static MouseButton[] GetMouseButtons()
        {
            return Enum.GetValues(typeof(MouseButton)) as MouseButton[];
        }

        public static GamepadButton[] GetGamepadButtons() {
            return Enum.GetValues(typeof(GamepadButton)) as GamepadButton[];
        }

        public static void AddInputConfiguration(string name, InputConfiguration configuration)
        {
            if (string.IsNullOrEmpty(name) || configuration == null || inputConfigurations == null)
                return;

            name = name.ToLower();

            if (!inputConfigurations.ContainsKey(name))
                inputConfigurations?.Add(name, configuration);
        }

        public static void AddInputHandler(string name, InputHandler handler)
        {
            if (handler == null || inputHandlers == null || string.IsNullOrEmpty(name))
                return;

            name = name.ToLower();

            if (!inputHandlers.ContainsKey(name))
                inputHandlers?.Add(name, handler);
        }

        public static string GetInputHandlerName(InputHandler handler)
        {
            if (handler == null || inputHandlers == null) return string.Empty;

            foreach (KeyValuePair<string, InputHandler> pair in inputHandlers) {
                if (IsSameInputHandler(pair.Value, handler))
                    return pair.Key;
            }

            return string.Empty;
        }

        private static bool IsSameInputHandler(InputHandler current, InputHandler next)
        {
            if (current == null || next == null) return false;
            return current.Equals(next);
        }

        public static string[] GetInputHandlerNames()
        {
            if (inputHandlers == null) { return null; }
            return inputHandlers.Keys.Distinct().ToArray();
        }

        public static SingleInputConfiguration GetSingleInputConfiguration(string name) {
            return GetInputConfiguration(name) as SingleInputConfiguration;
        }

        public static KeyboardVector2InputConfiguration GetKeyboardInputConfiguration(string name) {
            return GetInputConfiguration(name) as KeyboardVector2InputConfiguration;
        }

        public static MouseVector2InputConfiguration GetMouseInputConfiguration(string name) {
            return GetInputConfiguration(name) as MouseVector2InputConfiguration;
        }

        public static SingleInputConfiguration GetOrCreateSingleInputConfiguration(string name, SingleInputSettings settings) {
            if (string.IsNullOrEmpty(name) || settings == null) return null;

            SingleInputConfiguration configuration =  GetSingleInputConfiguration(name);
            return configuration != null ? configuration : new SingleInputConfiguration(settings);
        }

        public static KeyboardVector2InputConfiguration GetOrCreateKeyboardInputConfiguration(string name, KeyboardVector2InputSettings settings)
        {
            if (string.IsNullOrEmpty(name) || settings == null) return null;

            KeyboardVector2InputConfiguration configuration =  GetKeyboardInputConfiguration(name);
            return configuration != null ? configuration : new KeyboardVector2InputConfiguration(settings);
        }


        public static MouseVector2InputConfiguration GetOrCreateMouseInputConfiguration(string name, MouseVector2InputSettings settings)
        {
            if (string.IsNullOrEmpty(name) || settings == null) return null;

            MouseVector2InputConfiguration configuration =  GetMouseInputConfiguration(name);
            return configuration != null ? configuration :  new MouseVector2InputConfiguration(settings);
        }


        public static SingleInputConfiguration[] GetSingleInputConfigurations()
        {
            return GetInputConfigurations().Where(x => x is SingleInputConfiguration)
                   .ToArray() as SingleInputConfiguration[];
        }

        public static KeyboardVector2InputConfiguration[] GetKeyboardInputConfigurations()
        {
            return GetInputConfigurations().Where(x => x is KeyboardVector2InputConfiguration)
                   .ToArray() as KeyboardVector2InputConfiguration[];
        }

        public static MouseVector2InputConfiguration[] GetMouseInputConfigurations()
        {
            return GetInputConfigurations().Where(x => x is MouseVector2InputConfiguration)
                  .ToArray() as MouseVector2InputConfiguration[];
        }


        private static InputConfiguration[] GetInputConfigurations()
        {
            if(inputConfigurations == null)  return new InputConfiguration[0];
            return inputConfigurations.Values.Where(x => x != null).ToArray();
        }

        public static InputConfiguration GetInputConfiguration(string name, InputConfiguration[] configurations)
        {
            if (string.IsNullOrEmpty(name) || configurations == null) return null;
            InputConfiguration[] results;

            name = name.ToLower();
            results = configurations.Where(x => x != null).Where(x => x.InputName.ToLower().Equals(name)).ToArray();

            if (results.Length > 0) return results.First();
            return null;
        }

        private static InputConfiguration GetInputConfiguration(string name)
        {
            if (string.IsNullOrEmpty(name) || inputConfigurations == null) return null;
            name = name.ToLower();

            if(inputConfigurations.ContainsKey(name)) return inputConfigurations[name];
            return null;
        }

        public static InputHandler GetInputHandler(string name)
        {
            if (inputHandlers != null && !string.IsNullOrEmpty(name))  {
                name = name.ToLower();

                if (inputHandlers.ContainsKey(name))
                    return inputHandlers[name];
            }

            return null;
        }

        public async static Awaitable<InputHandler[]> GetInputHandlers(string[] names)
        {
            List<InputHandler> results = new List<InputHandler>();
            if (names == null) { return results.ToArray(); }

            await Awaitable.BackgroundThreadAsync();

            foreach (string name in names)
            {
                InputHandler current = GetInputHandler(name);
                if (current != null) results.Add(current);
            }

            await Awaitable.MainThreadAsync();
            return results.ToArray();
        }

        public static bool GetKey(KeyboardKey key)
        {
            if (keyboardKeysControls == null) return false;
            return keyboardKeysControls[key].GetKey();
        }

        public static bool GetKey(MouseButton button)
        {
            if (mouseButtonsControls == null) return false;
            return mouseButtonsControls[button].GetKey();
        }

        public static bool GetKey(GamepadButton button)
        {
            if (gamepadButtonsControls == null) return false;
            return gamepadButtonsControls[button].GetKey();
        }

        public static bool GetKey(KeyboardKey[] keys)
        {
            if (keys != null) keys = keys.Distinct().ToArray();
            return keys.Where(x => GetKey(x)).Count() > 0;
        }
        public static bool GetKey(MouseButton[] buttons)
        {
            if (buttons != null) buttons = buttons.Distinct().ToArray();
            else return false;

            return buttons.Where(x => GetKey(x)).Count() > 0;
        }

        public static bool GetKey(GamepadButton[] buttons)
        {
            if (buttons != null) buttons = buttons.Distinct().ToArray();
            else return false;

            return buttons.Where(x => GetKey(x)).Count() > 0;
        }

        public static bool GetKey(KeyboardKey keyboardKey, GamepadButton gamepadKey)
        {
            if (GetKey(keyboardKey) || GetKey(gamepadKey)) return true;
            return false;
        }

        public static bool GetKeyDown(KeyboardKey key)
        {
            if (keyboardKeysControls == null) return false;
            return keyboardKeysControls[key].GetKeyDown();
        }

        public static bool GetKeyDown(MouseButton button)
        {
            if (mouseButtonsControls == null) return false;
            return mouseButtonsControls[button].GetKeyDown();
        }


        public static bool GetKeyDown(GamepadButton button)
        {
            if (gamepadButtonsControls == null) return false;
            return gamepadButtonsControls[button].GetKeyDown();
        }

        public static bool GetKeyDown(KeyboardKey[] keys)
        {
            if (keys != null) keys = keys.Distinct().ToArray();
            return keys.Where(x => GetKeyDown(x)).Count() > 0;
        }

        public static bool GetKeyDown(GamepadButton[] buttons)
        {
            if (buttons != null) buttons = buttons.Distinct().ToArray();
            else return false;

            return buttons.Where(x => GetKeyDown(x)).Count() > 0;
        }
        public static bool GetKeyDown(MouseButton[] buttons)
        {
            if (buttons != null) buttons = buttons.Distinct().ToArray();
            else return false;

            return buttons.Where(x => GetKeyDown(x)).Count() > 0;
        }

        public static bool GetKeyUp(KeyboardKey key)
        {
            if (keyboardKeysControls == null) return false;
            return keyboardKeysControls[key].GetKeyUp();
        }

        public static bool GetKeyUp(MouseButton button)
        {
            if (mouseButtonsControls == null) return false;
            return mouseButtonsControls[button].GetKeyUp();
        }

        public static bool GetKeyUp(GamepadButton button)
        {
            if (gamepadButtonsControls == null) return false;
            return gamepadButtonsControls[button].GetKeyUp();
        }

        public static bool GetKeyUp(KeyboardKey[] keys)
        {
            if (keys != null) keys = keys.Distinct().ToArray();
            return keys.Where(x => GetKeyUp(x)).Count() > 0;
        }
        public static bool GetKeyUp(MouseButton[] buttons)
        {
            if (buttons != null) buttons = buttons.Distinct().ToArray();
            else return false;

            return buttons.Where(x => GetKeyUp(x)).Count() > 0;
        }

        public static bool GetKeyUp(GamepadButton[] buttons)
        {
            if (buttons != null) buttons = buttons.Distinct().ToArray();
            else return false;

            return buttons.Where(x => GetKeyUp(x)).Count() > 0;
        }

        public static float GetAxis(KeyboardKey posititveKey, KeyboardKey negativeKey)
        {
            float result = 0f;
            if (GetKey(posititveKey)) result += 1f;
            if (GetKey(negativeKey)) result -= 1f;
            return result;
        }
        public static float GetAxis(GamepadButton posititveButton, GamepadButton negativeButton)
        {
            float result = 0f;
            if (GetKey(posititveButton)) result += 1f;
            if (GetKey(negativeButton)) result -= 1f;
            return result;
        }

        public static float GetAxis(GamepadStick stick, bool getAxisX)
        {
            return GetAxis(GetStickControl(stick), getAxisX);
        }

        private static float GetAxis(Vector2Control control, bool getAxisX)
        {
            float result = 0f;
            if (control == null) return 0f;

            if (getAxisX == true)
            {
                if (control.x.value > 0.5f) result += 1f;
                if (control.x.value < -0.5f) result -= 1f;
            }
            else
            {
                if (control.y.value > 0.5f) result += 1f;
                if (control.y.value < -0.5f) result -= 1f;
            }

            return result;
        }

        public static float GetAxis(bool readLeftStick, bool getAxisX)
        {
            Vector2 result = GetGamepadVector2(readLeftStick);
            if (getAxisX) return result.x;
            return result.y;
        }

        public static float GetAxisX(KeyboardKey left, KeyboardKey right) => GetAxis(right, left);
        public static float GetAxisY(KeyboardKey up, KeyboardKey down) => GetAxis(up, down);

        public static float GetAxisX(GamepadButton left, GamepadButton right) => GetAxis(right, left);
        public static float GetAxisY(GamepadButton up, GamepadButton down) => GetAxis(up, down);

        public static float GetAxisX(GamepadStick stick) => GetAxis(stick, true);
        public static float GetAxisY(GamepadStick stick) => GetAxis(stick, false);

        public static float GetAxisX(bool readLeftStick) => GetAxisX(readLeftStick ? GamepadStick.LeftStick : GamepadStick.RightStick);
        public static float GetAxisY(bool readLeftStick) => GetAxisY(readLeftStick ? GamepadStick.LeftStick : GamepadStick.RightStick);

        public static Vector2 GetKeyboardVector2(KeyboardKey up, KeyboardKey down, KeyboardKey left, KeyboardKey right) => Vector2.right * GetAxisX(left, right) + Vector2.up * GetAxisY(up, down);
        public static Vector2 GetKeyboardVector2(KeyboardVector2Input.Vector2Keyboard keys) => GetKeyboardVector2(keys.Up, keys.Down, keys.Left, keys.Right);

        public static Vector2 GetMouseVector2()
        {
            Mouse mouse = Mouse.current;
            if (mouse == null) return Vector2.zero;
            return mouse.delta.value;
        }

        public static Vector2 GetGamepadVector2(GamepadButton up, GamepadButton down, GamepadButton left, GamepadButton right) => Vector2.right * GetAxisX(left, right) + Vector2.up * GetAxisY(up, down);

        public static Vector2 GetGamepadVector2(GamepadStick stick)
        {
            return GetGamepadVector2(stick == GamepadStick.LeftStick ? true : false);
        }
        public static Vector2 GetGamepadVector2(bool readLeftStick)
        {
            Gamepad gamepad = Gamepad.current;
            if (gamepad == null) return Vector2.zero;
            return readLeftStick ? gamepad.leftStick.value : gamepad.rightStick.value;
        }

        private static Vector2Control GetStickControl(GamepadStick stick)
        {
            return InputSystem.FindControl($"{GAMEPAD_ROOT_PATH}{GetFormattedKey(stick.ToString())}") as Vector2Control;
        }

        public static PressInput GetPressInput(string name)
        {
            return GetInputHandler(name) as PressInput;
        }
        public static HoldInput GetHoldInput(string name)
        {
            return GetInputHandler(name) as HoldInput;
        }
        public static ReleaseInput GetReleaseInput(string name)
        {
            return GetInputHandler(name) as ReleaseInput;
        }
        public static Vector2Input GetVector2Input(string name)
        {
            return GetInputHandler(name) as Vector2Input;
        }
        public static MouseVector2Input GetMouseVector2Input(string name)
        {
            return GetInputHandler(name) as MouseVector2Input;
        }
        public static KeyboardVector2Input GetKeyboardVector2Input(string name)
        {
            return GetInputHandler(name) as KeyboardVector2Input;
        }

        public static OverrideablePressInput GetOverrideablePressInput(string name)
        {
            return GetPressInput(name) as OverrideablePressInput;
        }
        public static OverrideableHoldInput GetOverrideableHoldInput(string name)
        {
            return GetHoldInput(name) as OverrideableHoldInput;
        }
        public static OverrideableReleaseInput GetOverrideableReleaseInput(string name)
        {
            return GetReleaseInput(name) as OverrideableReleaseInput;
        }
        public static OverrideableVector2Input GetOverrideableVector2Input(string name)
        {
            return GetInputHandler(name) as OverrideableVector2Input;
        }

        // I need to add OverreadbleMouseInput Class !!!
        public static OverrideableKeyboardVector2Input GetOverrideableKeyboardVector2Input(string name)
        {
            return GetInputHandler(name) as OverrideableKeyboardVector2Input;
        }

        public static SingleInput GetOrCreateSingleInput(string name, KeyboardKey defaultKey, GamepadButton defaultButton, SingleInputType inputType)
        {
            switch (inputType)
            {
                case SingleInputType.Press: return GetOrCreatePressInput(name, defaultKey, defaultButton);
                case SingleInputType.Hold: return GetOrCreateHoldInput(name, defaultKey, defaultButton);
                case SingleInputType.Release: return GetOrCreateReleaseInput(name, defaultKey, defaultButton);
                case SingleInputType.OverrideablePress: return GetOrCreateOverrideablePressInput(name, defaultKey, defaultButton);
                case SingleInputType.OverrideableHold: return GetOrCreateOverrideableHoldInput(name, defaultKey, defaultButton);
                case SingleInputType.OverrideableRelease: return GetOrCreateOverrideableReleaseInput(name, defaultKey, defaultButton);
            }

            return null;
        }


        public static SingleInput GetOrCreateSingleInput(string name, MouseButton defaultMouseButton, GamepadButton defaultGamepadButton, SingleInputType inputType)
        {
            switch (inputType)
            {
                case SingleInputType.Press: return GetOrCreatePressInput(name, defaultMouseButton, defaultGamepadButton);
                case SingleInputType.Hold: return GetOrCreateHoldInput(name, defaultMouseButton, defaultGamepadButton);
                case SingleInputType.Release: return GetOrCreateReleaseInput(name, defaultMouseButton, defaultGamepadButton);
                case SingleInputType.OverrideablePress: return GetOrCreateOverrideablePressInput(name, defaultMouseButton, defaultGamepadButton);
                case SingleInputType.OverrideableHold: return GetOrCreateOverrideableHoldInput(name, defaultMouseButton, defaultGamepadButton);
                case SingleInputType.OverrideableRelease: return GetOrCreateOverrideableReleaseInput(name, defaultMouseButton, defaultGamepadButton);
            }

            return null;
        }

        public static PressInput GetOrCreatePressInput(string name, KeyboardKey defaultKey, GamepadButton defaultButton)
        {
            PressInput result = GetPressInput(name);
            if (result != null) return result;
            return new PressInput(name, defaultKey, defaultButton);
        }

        public static PressInput GetOrCreatePressInput(string name, MouseButton defaultMouseButton, GamepadButton defaultGamepadButton)
        {
            PressInput result = GetPressInput(name);
            if (result != null) return result;
            return new PressInput(name, defaultMouseButton, defaultGamepadButton);
        }


        public static HoldInput GetOrCreateHoldInput(string name, KeyboardKey defaultKey, GamepadButton defaultButton)
        {
            HoldInput result = GetHoldInput(name);
            if (result != null) return result;
            return new HoldInput(name, defaultKey, defaultButton);
        }
        public static HoldInput GetOrCreateHoldInput(string name, MouseButton defaultMouseButton, GamepadButton defaultGamepadButton)
        {
            HoldInput result = GetHoldInput(name);
            if (result != null) return result;
            return new HoldInput(name, defaultMouseButton, defaultGamepadButton);
        }

        public static ReleaseInput GetOrCreateReleaseInput(string name, KeyboardKey defaultKey, GamepadButton defaultButton)
        {
            ReleaseInput result = GetReleaseInput(name);
            if (result != null) return result;
            return new ReleaseInput(name, defaultKey, defaultButton);
        }


        public static ReleaseInput GetOrCreateReleaseInput(string name, MouseButton defaultMouseButton, GamepadButton defaultGamepadButton)
        {
            ReleaseInput result = GetReleaseInput(name);
            if (result != null) return result;
            return new ReleaseInput(name, defaultMouseButton, defaultGamepadButton);
        }

        public static MouseVector2Input GetOrCreateMouseVector2Input(string name, GamepadStick stick)
        {
            MouseVector2Input result = GetMouseVector2Input(name);
            if (result != null) return result;
            return new MouseVector2Input(name, stick);
        }
        public static KeyboardVector2Input GetOrCreateKeyboardVector2Input(string name, KeyboardVector2Input.Vector2Keyboard keyboard, GamepadStick stick)
        {
            KeyboardVector2Input result = GetKeyboardVector2Input(name);
            if (result != null) return result;
            return new KeyboardVector2Input(name, keyboard, stick);
        }

        public static OverrideablePressInput GetOrCreateOverrideablePressInput(string name, KeyboardKey defaultKeyboardKey, GamepadButton defaultGamepadButton)
        {
            OverrideablePressInput result = GetOverrideablePressInput(name);
            if (result != null) return result;
            return new OverrideablePressInput(name, defaultKeyboardKey, defaultGamepadButton);
        }

        public static OverrideablePressInput GetOrCreateOverrideablePressInput(string name, MouseButton defaultMouseButton, GamepadButton defaultGamepadButton)
        {
            OverrideablePressInput result = GetOverrideablePressInput(name);
            if (result != null) return result;
            return new OverrideablePressInput(name, defaultMouseButton, defaultGamepadButton);
        }

        public static OverrideableHoldInput GetOrCreateOverrideableHoldInput(string name, KeyboardKey defaultKeyboardKey, GamepadButton defaultGamepadButton)
        {
            OverrideableHoldInput result = GetOverrideableHoldInput(name);
            if (result != null) return result;
            return new OverrideableHoldInput(name, defaultKeyboardKey, defaultGamepadButton);
        }

        public static OverrideableHoldInput GetOrCreateOverrideableHoldInput(string name, MouseButton defaultMouseButton, GamepadButton defaultGamepadButton)
        {
            OverrideableHoldInput result = GetOverrideableHoldInput(name);
            if (result != null) return result;
            return new OverrideableHoldInput(name, defaultMouseButton, defaultGamepadButton);
        }

        public static OverrideableReleaseInput GetOrCreateOverrideableReleaseInput(string name, KeyboardKey defaultKeyboardKey, GamepadButton defaultGamepadButton)
        {
            OverrideableReleaseInput result = GetOverrideableReleaseInput(name);
            if (result != null) return result;
            return new OverrideableReleaseInput(name, defaultKeyboardKey, defaultGamepadButton);
        }
        public static OverrideableReleaseInput GetOrCreateOverrideableReleaseInput(string name, MouseButton defaultMouseButton, GamepadButton defaultGamepadButton)
        {
            OverrideableReleaseInput result = GetOverrideableReleaseInput(name);
            if (result != null) return result;
            return new OverrideableReleaseInput(name, defaultMouseButton, defaultGamepadButton);
        }


        public static OverrideableKeyboardVector2Input GetOrCreateOverrideableKeyboardVector2Input(string name, KeyboardVector2Input.Vector2Keyboard keyboard, GamepadStick stick)
        {
            OverrideableKeyboardVector2Input result = GetOverrideableKeyboardVector2Input(name);
            if (result != null) return result;
            return new OverrideableKeyboardVector2Input(name, keyboard, stick);
        }

        public static OverrideableKeyboardVector2Input GetOrCreateOverrideableKeyboardVector2Input(string name, KeyboardVector2Input.Vector2Keyboard keyboard)
        {
            return GetOrCreateOverrideableKeyboardVector2Input(name,
                                                               keyboard,
                                                               GamepadStick.LeftStick);
        }

        public static OverrideableKeyboardVector2Input GetOrCreateOverrideableKeyboardVector2Input(string name, GamepadStick stick)
        {
            return GetOrCreateOverrideableKeyboardVector2Input(name,
                                                               new KeyboardVector2Input.Vector2Keyboard(KeyboardKey.W, KeyboardKey.S, KeyboardKey.A, KeyboardKey.D),
                                                               stick);
        }

        public static OverrideableKeyboardVector2Input GetOrCreateOverrideableKeyboardVector2Input(string name)
        {
            return GetOrCreateOverrideableKeyboardVector2Input(name,
                   new KeyboardVector2Input.Vector2Keyboard(KeyboardKey.W, KeyboardKey.S, KeyboardKey.A, KeyboardKey.D),
                   GamepadStick.LeftStick);
        }
    }
}