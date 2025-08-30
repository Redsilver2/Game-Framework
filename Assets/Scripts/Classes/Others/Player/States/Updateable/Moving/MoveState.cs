using RedSilver2.Framework.Inputs;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public abstract class MoveState : UpdateablePlayerState
        {
            protected readonly CharacterController character;
            protected readonly Vector2Input movementInput;
            private UnityEvent<Vector2> onMovementInputUpdate;

            public const string MOVEMENT_SPEED_SETTING_NAME = "Movement Speed";
            public const string MOVEMENT_INPUT_NAME = "Movement Input";

            public static readonly string[] RequiredInputStates = { WalkState.STATE_NAME, RunState.STATE_NAME, CrouchState.STATE_NAME };

            protected MoveState()
            {

            }

            protected MoveState(PlayerStateMachine owner) : base(owner)
            {
                onMovementInputUpdate = new UnityEvent<Vector2>();
                movementInput = GetMovementInput();

                character = owner.character;

                movementInput.Enable();
                AddOnMovementInputUpdate(Move);
            }
            public void AddOnMovementInputUpdate(UnityAction<Vector2> action)
            {
                if (onMovementInputUpdate != null && action != null) onMovementInputUpdate.AddListener(action);
            }

            public void RemoveOnMovementInputUpdate(UnityAction<Vector2> action)
            {
                if (onMovementInputUpdate != null && action != null) onMovementInputUpdate.RemoveListener(action);
            }

            private void Move(Vector2 input)
            {
                Move(input, 3f);
            }
            protected void Move(Vector2 input, float speed)
            {
                if (character != null)
                {
                    input.Normalize();
                    character.Move(Time.deltaTime * speed * GetMovementMotion(input));
                }
            }

            protected virtual Vector3 GetMovementMotion(Vector2 input)
            {
                Transform transform;
                if (character == null) return Vector3.zero;

                transform = character.transform;
                return  transform.right * input.x + transform.forward * input.y;
            }

            protected override void OnStateEnter()
            {
                base.OnStateEnter();
                if (movementInput != null) movementInput.AddOnUpdateListener(OnUpdate);
            }
            protected override void OnStateExit()
            {
                base.OnStateExit();
                if (movementInput != null) movementInput.RemoveOnUpdateListener(OnUpdate);
            }
            private void OnUpdate(Vector2 vector)
            {
                onMovementInputUpdate.Invoke(vector);
            }

            public static void SetDefaultMovementInputEvent(PlayerStateMachine owner)
            {
                if(owner != null)
                {
                    owner.AddOnStateAddedListener(OnStateAdded);
                    owner.AddOnStateRemovedListener(OnStateRemoved);
                }
            }

            private static void OnStateAdded(PlayerState state)
            {
                if(state != null)
                {
                    EnableMovementInputEvent(state.Owner);
                }
            }

            private static void OnStateRemoved(PlayerState state)
            {
                if (state != null)
                {
                    DisableMovementInputEvent(state.Owner);
                }
            }

            private static void EnableMovementInputEvent(PlayerStateMachine owner)
            {
                if (owner != null)
                {
                    if (owner.GetStates(RequiredInputStates).Length == 1)
                    {
                        Vector2Input input = GetMovementInput();
                        owner.AddOnUpdateListener(input.Update);
                        input.Enable();
                    }
                }
            }

            private static void DisableMovementInputEvent(PlayerStateMachine owner)
            {
                if (owner != null)
                {
                    if (owner.GetStates(RequiredInputStates).Length == 0)
                    {
                        Vector2Input input = GetMovementInput();
                        owner.RemoveOnUpdateListener(input.Update);
                        input.Disable();
                    }
                }
            }

            public static OverrideableKeyboardVector2Input GetMovementInput()
            {
                OverrideableKeyboardVector2Input result = InputManager.GetInputHandler(MOVEMENT_INPUT_NAME) as OverrideableKeyboardVector2Input;
                if (result == null) return new OverrideableKeyboardVector2Input(MOVEMENT_INPUT_NAME);
                return result;
            }
        }

    }
}