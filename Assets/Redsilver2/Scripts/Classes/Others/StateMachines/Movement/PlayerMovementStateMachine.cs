               
using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Events;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.StateMachines.Controllers {
    public abstract partial class PlayerMovementStateMachine : MovementStateMachine
    {
        [Space]
        [SerializeField, HideInInspector] private KeyboardVector2InputSettings moveInputSetting;
        [SerializeField, SerializeReference, HideInInspector] protected PlayerMovementHandler handler;

        public Vector2                      MoveInput           => moveInputSetting != null ? moveInputSetting.GetValue() : Vector2.zero;
        public KeyboardVector2InputSettings MoveInputSetting    => moveInputSetting;

        protected override void Awake() {
            base.Awake();
            if (enabled) moveInputSetting?.Enable();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnEnabled() {

            base.OnEnabled();
            moveInputSetting?.Enable();
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            moveInputSetting?.Disable();
        }

        protected override void OnUpdate() {

            this.moveInputSetting?.Enable();
            SetIsMoving(moveInputSetting != null ? moveInputSetting.GetValue().magnitude > 0f : false);

            handler?.Update();
            base.OnUpdate();
         
        }

        protected sealed override void OnLateUpdate() {
            handler?.LateUpdate();
        }

        public void SetInputSetting(KeyboardVector2InputSettings inputSetting) {
            this.moveInputSetting = inputSetting;
        }
    }

    public abstract partial class PlayerMovementStateMachine : MovementStateMachine
    {

#if UNITY_EDITOR
        [SerializeField, HideInInspector] private int previousControlType = -1;

        protected override void OnValidate()
        {
            base.OnValidate();
        }

        protected override void DisplayDefaultSettings(InspectorVisualizer visualizer)
        {
            if(visualizer == null) return;
            base.DisplayDefaultSettings(visualizer);

            EditorExtension.Space(10f);
            SetInputSetting(EditorExtension.DisplayCustomField("Move Input Setting", false, moveInputSetting));
            EditorExtension.Space(10f);

            EditorExtension.DrawHorizontal(() => {
                DisplayMovementControlTypes(ref previousControlType);
            });

            handler?.DrawInspector(visualizer);
        }

        protected virtual void DisplayMovementControlTypes(ref int previousValue)
        {
            EditorExtension.DisplayBoldLabel("Movement Control Type");
        }

        protected override MovementState GetInspectorState(MovementStateType type)
        {
            if (ContainsState(type)) return null;

            switch (type)
            {
                case MovementStateType.Idol:   return new PlayerIdolState(this);
                case MovementStateType.Walk:   return new PlayerWalkState(this);
                case MovementStateType.Fall:   return new PlayerFallState(this);
                case MovementStateType.Jump:   return new PlayerJumpState(this);
                case MovementStateType.Land:   return new PlayerLandState(this);
                case MovementStateType.Crouch: return new PlayerCrouchState(this);
                case MovementStateType.Run:    return new PlayerRunState(this);
                default: return null;
            }
        }
#endif

    }
}