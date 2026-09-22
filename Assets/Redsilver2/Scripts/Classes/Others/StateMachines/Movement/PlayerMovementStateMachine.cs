
using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Events;
using RedSilver2.Framework.StateMachines.Presets;
using RedSilver2.Framework.StateMachines.States;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.StateMachines.Controllers {
    public abstract partial class PlayerMovementStateMachine : MovementStateMachine
    {
        [SerializeField, HideInInspector] private KeyboardVector2InputSettings moveInputSetting;
        [SerializeField, HideInInspector] private Dictionary<PlayerMovementPreset.PresetType, PlayerMovementPresetData>  movementPresetDatas;


        [SerializeField, HideInInspector] private UnityEvent<PlayerMovementPreset.PresetType> onMovementPresetChanged;
        [SerializeField, HideInInspector] private UnityEvent<PlayerMovementPreset.PresetType> onMovementPresetAdded;
        [SerializeField, HideInInspector] private UnityEvent<PlayerMovementPreset.PresetType> onMovementPresetRemoved;

        private int selectedIndex = 0;
        private Vector2 moveInput;
        private PlayerMovementPreset currentMovementPreset;

        public Vector2 MoveInput
        {
            get {
                moveInput.Normalize();
                return moveInput;
            }
        }

        public PlayerMovementPreset CurrentMovementPreset => currentMovementPreset;


        protected override void Awake() {
            base.Awake();
            AddOnMovementPresetChangedListener(OnMovementPresetChanged);
            if (enabled) moveInputSetting?.Enable();
        }

        protected override void Start()
        {
            base.Start();
            SetMovementPreset(PlayerMovementPreset.PresetType.FirstPerson);
        }

        protected virtual void OnMovementPresetChanged(PlayerMovementPreset.PresetType type) {
            GetPreset(type)?.Enable();
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
            moveInput = moveInputSetting != null ? moveInputSetting.GetValue() : Vector2.zero;
            
            SetIsMoving(moveInput.magnitude > 0f);

            if (InputManager.GetKeyDown(KeyboardKey.L)) {
                selectedIndex++;
                if (selectedIndex >= movementPresetDatas.Keys.Count) selectedIndex = 0;

                SetMovementPreset(movementPresetDatas.Keys.ToArray()[selectedIndex]);
            }

            base.OnUpdate();
        }

        protected sealed override void OnLateUpdate() {
            base.OnLateUpdate();
        }

        public void SetInputSetting(KeyboardVector2InputSettings inputSetting) {
            this.moveInputSetting = inputSetting;
        }

        public void AddMovementPreset(PlayerMovementPreset.PresetType controlType) {
            if (movementPresetDatas == null || GetPreset(controlType) != null) return;
            else {
                if (!movementPresetDatas.ContainsKey(controlType)) movementPresetDatas?.Add(controlType, default);

                switch (controlType) {
                    case PlayerMovementPreset.PresetType.SideScroller:  movementPresetDatas[controlType] = new PlayerMovementPresetData(new SideScrollerMovementPreset(this)); break;
                    case PlayerMovementPreset.PresetType.TopDown:       movementPresetDatas[controlType] = new PlayerMovementPresetData(new TopDownMovementPreset(this));      break;
                   
                    case PlayerMovementPreset.PresetType.FirstPerson:   movementPresetDatas[controlType] = new PlayerMovementPresetData(new FirstPersonMovementPreset(this)); break;
                    case PlayerMovementPreset.PresetType.ThirdPerson:   movementPresetDatas[controlType] = new PlayerMovementPresetData(new ThirdPersonMovementPreset(this)); break;
                }
                
                onMovementPresetAdded?.Invoke(controlType);
            }
        }

        public void RemoveMovementPreset(PlayerMovementPreset.PresetType presetType) {
            if (movementPresetDatas == null || !movementPresetDatas.ContainsKey(presetType)) return;
            movementPresetDatas?.Remove(presetType);

            if(Application.isPlaying) onMovementPresetRemoved?.Invoke(presetType);
        }

        public void SetMovementPreset(PlayerMovementPreset.PresetType presetType)
        {
            if(currentMovementPreset == null || currentMovementPreset.Type != presetType)
            {
                currentMovementPreset?.Disable();
                currentMovementPreset = GetPreset(presetType);
                onMovementPresetChanged?.Invoke(presetType);
            }
        }

        public void AddOnMovementPresetChangedListener(UnityAction<PlayerMovementPreset.PresetType> action) {
            if (action != null) onMovementPresetChanged?.AddListener(action);
        }
        public void RemoveOnMovementPresetChangedListener(UnityAction<PlayerMovementPreset.PresetType> action)
        {
            if (action != null) onMovementPresetChanged?.RemoveListener(action);
        }

        public void AddOnMovementPresetAddedListener(UnityAction<PlayerMovementPreset.PresetType> action)
        {
            if (action != null) onMovementPresetAdded?.AddListener(action);
        }
        public void RemoveOnMovementPresetAddedListener(UnityAction<PlayerMovementPreset.PresetType> action)
        {
            if (action != null) onMovementPresetAdded?.RemoveListener(action);
        }

        public void AddOnMovementPresetRemovedListener(UnityAction<PlayerMovementPreset.PresetType> action)
        {
            if (action != null) onMovementPresetRemoved?.AddListener(action);
        }
        public void RemoveOnMovementPresetRemovedListener(UnityAction<PlayerMovementPreset.PresetType> action)
        {
            if (action != null) onMovementPresetRemoved?.RemoveListener(action);
        }

        public PlayerMovementPreset GetPreset(PlayerMovementPreset.PresetType presetType)
        {
            if (movementPresetDatas == null || !movementPresetDatas.ContainsKey(presetType)) return null;
            return movementPresetDatas[presetType].Preset;
        }
    }

    public abstract partial class PlayerMovementStateMachine : MovementStateMachine
    {

#if UNITY_EDITOR
        [SerializeField, HideInInspector] private bool displayMovementPresetsFoldout;
        [SerializeField, HideInInspector] private bool showMovementPresetsFoldout;
        [SerializeField, HideInInspector] private bool addOrRemoveMovementPresetsFoldout;


        protected override void OnValidate()
        {
            base.OnValidate();
        }

        protected override void DisplayDebugger(InspectorVisualizer visualizer)
        {

            EditorExtension.Space(10f);

            EditorExtension.DrawHorizontal(() =>
            {
                EditorExtension.DisplayBoldLabel("Movement Preset");
                EditorExtension.DisplayBoldLabel(GetPresetName(currentMovementPreset != null ? currentMovementPreset.Type.ToString() : "None"));
            });

            EditorExtension.Space(10f);
            base.DisplayDebugger(visualizer);
       

        }

        protected override void DisplayDefaultSettings(InspectorVisualizer visualizer)
        {
            if(visualizer == null) return;
            base.DisplayDefaultSettings(visualizer);

            EditorExtension.Space(10f);
            SetInputSetting(EditorExtension.DisplayCustomField("Move Input Setting", false, moveInputSetting));
            EditorExtension.Space(10f);

            if (EditorExtension.DisplayFoldout("Movement Presets", ref displayMovementPresetsFoldout, visualizer.FoldoutColor))
            {
                EditorExtension.DrawVertical(() =>
                {
                    AddOrRemoveMovementPresets(visualizer);
                    DisplayMovementPresets(visualizer);
                }, true);
            }
        }

        private void AddOrRemoveMovementPresets(InspectorVisualizer visualizer)
        {
            if (visualizer == null || movementPresetDatas == null) return;
            else if (EditorExtension.DisplayFoldout("Add / Remove", ref addOrRemoveMovementPresetsFoldout, visualizer.FoldoutColor)) {
                PlayerMovementPreset.PresetType[] values = UnityExtension.GetEnumValues(PlayerMovementPreset.PresetType.FirstPerson);
                if (values == null) return;

                EditorExtension.DrawVertical(() => {

                    foreach(PlayerMovementPreset.PresetType value in values) DisplayPreset(value); 
                    EditorExtension.Space(10f);

                    EditorExtension.DrawHorizontal(() =>
                    {
                        if (movementPresetDatas.Values.Count != values.Length) EditorExtension.DisplayButton("Add All Presets", () => {
                            foreach (PlayerMovementPreset.PresetType value in values) AddMovementPreset(value);
                        });

                        if (movementPresetDatas.Values.Count > 0) EditorExtension.DisplayButton("Remove All Presets", () => {
                            movementPresetDatas?.Clear();
                        });
                    });
                }, true);

                EditorExtension.Space(10f);
            }
        }

        private void DisplayPreset(PlayerMovementPreset.PresetType controlType)
        {
            if (movementPresetDatas.ContainsKey(controlType))
                if(movementPresetDatas[controlType].Preset != null) return;

            EditorExtension.Space(10f);

            EditorExtension.DrawHorizontal(() =>
            {
                EditorExtension.DisplayBoldLabel(GetPresetName(controlType.ToString()));
                EditorExtension.DisplayButton($"Add Preset", () => { AddMovementPreset(controlType); });
            });
        }

        private static string GetPresetName(string presetName) {
            string result = string.Empty;
            char[] chars  = presetName.ToCharArray();    
            bool isDigit  = false;
      

            for(int i = 0; i < chars.Length; i++) {
                if (i > 0) {
                    if(char.IsUpper(chars[i]) && !isDigit) result += " ";
                    isDigit = char.IsDigit(chars[i]);

                    if (isDigit) { result += " "; }
                }
                result += chars[i];
            }
            return result;
        }


        private void DisplayMovementPresets(InspectorVisualizer visualizer)
        {
            if (visualizer == null || movementPresetDatas == null) return;
            else if (EditorExtension.DisplayFoldout("Show", ref showMovementPresetsFoldout, visualizer.FoldoutColor))  {
             
                var datas = movementPresetDatas.Values.ToArray();
                EditorExtension.IncrementIndent();

                if (datas != null) {
                    foreach (var data in datas)
                    {
                        data.Preset?.DrawInspector(visualizer);
                    }
                }

                EditorExtension.DecrementIndent();
            }
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