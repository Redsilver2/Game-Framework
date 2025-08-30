using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs
{
    public abstract class Vector2Input : InputHandler
    {
        protected          GamepadStick    gamepadStick;

        private UnityEvent<Vector2> onUpdate;
        public GamepadStick GamepadStick => gamepadStick;
       

        public Vector2 Value { get; private set; }

        public const GamepadStick DEFAULT_GAMEPAD_STICK   = GamepadStick.LeftStick;


        protected Vector2Input(string name) : base(name)
        {
            onUpdate = new UnityEvent<Vector2>();
            this.gamepadStick = DEFAULT_GAMEPAD_STICK;
        }

        protected Vector2Input(string name, GamepadStick gamepadStick) : base(name)
        {
            onUpdate          = new UnityEvent<Vector2>();
            this.gamepadStick = gamepadStick;
        }

        public sealed override void Update()
        {
            Value = GetInputVector2();
            if(IsEnabled) { onUpdate.Invoke(Value); }
        }

        public void AddOnUpdateListener(UnityAction<Vector2> action)
        {
            if(onUpdate != null && action != null) onUpdate.AddListener(action);
        }

        public void RemoveOnUpdateListener(UnityAction<Vector2> action)
        {
            if (onUpdate != null && action != null) onUpdate.RemoveListener(action);
        }

        public string GetGamepadStickPath() => InputManager.GetPath(gamepadStick);

        public string GetGamepadStickInfos()
        {
            return "| Gamepad Paths |\n" +
                    $"Gamepad Stick: {gamepadStick.ToString()} | Path: {GetGamepadStickPath()}";
        }

        public override string GetPaths()
        {
            return "| Keys Paths | \n\n" + $"{GetGamepadStickInfos()}";
        }


        private bool TryGetGamepadVector2(out Vector2 result)
        {
            result = InputManager.GetVector2(gamepadStick);
            if(result.magnitude > 0f) { return true; }

            result = Vector2.zero;
            return false;
        }

        protected virtual Vector2 GetInputVector2()
        {
            Vector2 result;
            if (TryGetGamepadVector2(out result)) { return result; }
            return result;
        }
    }
}
