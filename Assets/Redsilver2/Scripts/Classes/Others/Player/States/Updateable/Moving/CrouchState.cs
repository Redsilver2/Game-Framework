using RedSilver2.Framework.Inputs;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public sealed class CrouchState : MoveState
        {
            private bool isCrouching;

            private readonly PressInput pressInput;
            private readonly HoldInput  holdInput;

            private readonly UnityEvent       onPressInputUpdate;
            private readonly UnityEvent<bool> onHoldInputUpdate;


            public const string CROUCH_PRESS_INPUT_NAME = "Crouch Press Input";
            public const string CROUCH_HOLD_INPUT_NAME  = "Crouch Hold Input";      
            public const string STATE_NAME              = "Crouch";

            private CrouchState() { }

            public CrouchState(PlayerStateMachine owner) : base(owner)
            {
                pressInput  = GetCrouchPressInput();
                holdInput   = GetCrouchHoldInput();
                isCrouching = false;
            }

            protected override void SetObligatoryTransition(PlayerState[] states, bool removeTransition)
            {
                if (states != null)
                {
                    foreach (PlayerState state in states.Where(x => x != this))
                    {
                        if (removeTransition) { }
                        else                  { }
                    }
                }
            }

            public sealed override string GetStateName() => STATE_NAME;

            public static OverrideableHoldInput GetCrouchHoldInput() {
                return InputManager.GetOrCreateOverrideableHoldInput(CROUCH_HOLD_INPUT_NAME, KeyboardKey.C, GamepadButton.ButtonEast) ;
            }

            public static OverrideablePressInput GetCrouchPressInput() {
                return InputManager.GetOrCreateOverrideablePressInput(CROUCH_PRESS_INPUT_NAME, KeyboardKey.C, GamepadButton.ButtonEast);
            }
        }
    }
}
