
using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.Interactions;
using RedSilver2.Framework.Player;
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
        [SerializeField, HideInInspector] private Dictionary<MovementPresetType, MovementPresetData> movementPresets;


        [SerializeField, HideInInspector] private UnityEvent<MovementPresetType> onMovementPresetChanged;
        [SerializeField, HideInInspector] private UnityEvent<MovementPresetType> onMovementPresetAdded;
        [SerializeField, HideInInspector] private UnityEvent<MovementPresetType> onMovementPresetRemoved;

        private int selectedIndex = 0;
        private Vector2 moveInput;
        private MovementPreset currentMovementPreset;


        public Vector2 MoveInput
        {
            get {
                moveInput.Normalize();
                return moveInput;
            }
        }

        public MovementPreset CurrentMovementPreset => currentMovementPreset;


        protected override void Awake() {
            base.Awake();
            if (enabled) moveInputSetting?.Enable();
        }

        protected override void Start()
        {
            base.Start();
            SetMovementPreset(MovementPresetType.FirstPerson);
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

            SetIsMoving(moveInputSetting != null ? moveInputSetting.GetValue().magnitude > 0f : false);

            if (InputManager.GetKeyDown(KeyboardKey.L)) {
                selectedIndex++;
                if (selectedIndex >= movementPresets.Keys.Count) selectedIndex = 0;

                SetMovementPreset(movementPresets.Keys.ToArray()[selectedIndex]);
            }

            currentMovementPreset?.Update();
            base.OnUpdate();
        }

        protected sealed override void OnLateUpdate() {

            currentMovementPreset?.LateUpdate();
            base.OnLateUpdate();
        }

        public void SetInputSetting(KeyboardVector2InputSettings inputSetting) {
            this.moveInputSetting = inputSetting;
        }

        public void AddMovementPreset(MovementPresetType controlType) {
            if (movementPresets == null || GetMovementPreset(controlType) != null) return;
            else {
                if (!movementPresets.ContainsKey(controlType)) movementPresets?.Add(controlType, null);

                switch (controlType) {
                    case MovementPresetType.SideScroller2D:         movementPresets[controlType] = new MovementPresetData(new SideScroller2DMovementPreset(this)); break;
                    case MovementPresetType.SideScroller3D:         movementPresets[controlType] = new MovementPresetData(new SideScroller3DMovementPreset(this)); break;
                   
                    case MovementPresetType.TopDown2D:              movementPresets[controlType] = new MovementPresetData(new TopDown2DMovementPreset(this)); break;
                    case MovementPresetType.TopDown2DTankControl:   movementPresets[controlType] = new MovementPresetData(new TopDown2DTankControlMovementPreset(this)); break;

                    case MovementPresetType.TopDown3D:              movementPresets[controlType] = new MovementPresetData(new TopDown3DMovementPreset(this)); break;
                    case MovementPresetType.TopDown3DTankControl:   movementPresets[controlType] = new MovementPresetData(new TopDown3DTankControlMovementPreset(this)); break;

                    case MovementPresetType.FirstPerson:            movementPresets[controlType] = new MovementPresetData(new FirstPersonMovementPreset(this)); break;
                    case MovementPresetType.ClampedFirstPerson:     movementPresets[controlType] = new MovementPresetData(new ClampedFirstPersonMovementPreset(this)); break;
                    case MovementPresetType.FirstPersonTankControl: movementPresets[controlType] = new MovementPresetData(new FirstPersonTankControlMovementPreset(this)); break;

                    case MovementPresetType.ThirdPerson: movementPresets[controlType] = new MovementPresetData(new ThirdPersonMovementPreset(this)); break;
                }

                if (movementPresets[controlType] != null && Application.isPlaying)
                    onMovementPresetAdded?.Invoke(controlType);
            }
        }

        public void RemoveMovementPreset(MovementPresetType controlType) {
            if (movementPresets == null || !movementPresets.ContainsKey(controlType)) return;
            movementPresets?.Remove(controlType);

            if(Application.isPlaying) onMovementPresetRemoved?.Invoke(controlType);
        }

        public void SetMovementPreset(MovementPresetType type)
        {
            if(currentMovementPreset == null || currentMovementPreset.Type != type)
            {
                currentMovementPreset = GetMovementPreset(type);
                onMovementPresetChanged?.Invoke(type);
            }
        }

        public void AddOnMovementPresetChangedListener(UnityAction<MovementPresetType> action) {
            if (action != null) onMovementPresetChanged?.AddListener(action);
        }
        public void RemoveOnMovementPresetChangedListener(UnityAction<MovementPresetType> action)
        {
            if (action != null) onMovementPresetChanged?.RemoveListener(action);
        }

        public void AddOnMovementPresetAddedListener(UnityAction<MovementPresetType> action)
        {
            if (action != null) onMovementPresetAdded?.AddListener(action);
        }
        public void RemoveOnMovementPresetAddedListener(UnityAction<MovementPresetType> action)
        {
            if (action != null) onMovementPresetAdded?.RemoveListener(action);
        }

        public void AddOnMovementPresetRemovedListener(UnityAction<MovementPresetType> action)
        {
            if (action != null) onMovementPresetRemoved?.AddListener(action);
        }
        public void RemoveOnMovementPresetRemovedListener(UnityAction<MovementPresetType> action)
        {
            if (action != null) onMovementPresetRemoved?.RemoveListener(action);
        }

        public MovementPreset GetMovementPreset(MovementPresetType type)
        {
            if (movementPresets == null || !movementPresets.ContainsKey(type)) return null;
            MovementPresetData data = movementPresets[type];
        
            return data != null ? data.Preset : null;
        }
    }

    public abstract partial class PlayerMovementStateMachine : MovementStateMachine
    {
        [System.Serializable]
        public enum MovementPresetType {
            SideScroller2D,
            SideScroller3D,

            TopDown2D,
            TopDown2DTankControl,

            TopDown3D,
            TopDown3DTankControl,

            FirstPerson,
            ClampedFirstPerson,
            FirstPersonTankControl,

            ThirdPerson,
            ClampedThirdPerson,
            ThirdPersonTankControl
        }

        [System.Serializable]
        public class MovementPresetData
        {
            [SerializeField, SerializeReference, HideInInspector] private MovementPreset preset;
            public MovementPreset Preset => preset;

            public MovementPresetData(MovementPreset preset)
            {
                this.preset = preset;
            }
        }



        [System.Serializable]
        public abstract partial class MovementPreset 
        {
            [SerializeField, SerializeReference, HideInInspector] private CameraController   cameraController;
            [SerializeField, SerializeReference, HideInInspector] private InteractionHandler interactionHandler;

            [SerializeField, SerializeReference, HideInInspector] private PlayerMovementStateMachine stateMachine;
            [SerializeField, HideInInspector] private MovementPresetType type;

            public CameraController CameraController {
                get { return cameraController; }
                protected set {  cameraController = value; }
            }

            public InteractionHandler InteractionHandler
            {
                get           { return interactionHandler; }
                protected set { interactionHandler = value; }
            }

            public MovementPresetType Type => type;
            protected PlayerMovementStateMachine StateMachine => stateMachine;

            protected MovementPreset(PlayerMovementStateMachine stateMachine) { 
                this.stateMachine = stateMachine;
                SetType(ref type);
            }

            public virtual void Update() {
                cameraController?.Update();
            }

            public virtual void LateUpdate() {
                if (stateMachine != null) {



                    if (stateMachine.IsMoving) { Move(stateMachine); }



                    Fall(stateMachine);
                }


                cameraController?.LateUpdate();
            }

            protected virtual void Swim(PlayerMovementStateMachine stateMachine) { }
            protected virtual void Climb(PlayerMovementStateMachine stateMachine) { }

            protected abstract void Fall(PlayerMovementStateMachine stateMachine);
            protected abstract void Move(PlayerMovementStateMachine stateMachine);

            protected abstract void SetType(ref MovementPresetType controlType);
        }

       
        [System.Serializable]
        public abstract partial class TankControlMovementPreset : MovementPreset {
            [SerializeField, HideInInspector] private float rotationSpeed;
            public float RotationSpeed => rotationSpeed;

            protected TankControlMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine) {

            }

            public sealed override void LateUpdate()
            {
                LateUpdateTankControls();
                base.LateUpdate();
            }

            private void LateUpdateTankControls() {
                PlayerMovementStateMachine stateMachine = StateMachine;

                if (stateMachine != null) {
                    Vector2   input     = stateMachine.MoveInput;
                    Transform transform = stateMachine.transform;

                    if (Mathf.Abs(input.x) > 0f && Mathf.Abs(input.y) == 0f && transform != null) {
                        LateUpdateTankControls(stateMachine.transform, rotationSpeed);
                    }
                }
            }

            public abstract void LateUpdateTankControls(Transform transform, float rotationSpeed);
        }

       
        [System.Serializable]
        public sealed partial class SideScroller2DMovementPreset : MovementPreset {
            public SideScroller2DMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine) {
                CameraController = new TargetFollow2DCameraController(); 
            }

            protected sealed override void Fall(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;
                stateMachine?.Move(Time.deltaTime * Vector2.up * stateMachine.FallSpeed);
            }

            protected sealed override void Move(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return; 
                float inputX = stateMachine.MoveInput.x;
               
                Transform transform = stateMachine.transform;     
                stateMachine?.Move(Time.deltaTime * (Vector2.right * inputX * stateMachine.MoveSpeed + Vector2.up * stateMachine.FallSpeed));
            }

            protected sealed override void SetType(ref MovementPresetType controlType) {
                controlType = MovementPresetType.SideScroller2D;
            }
        }
       
      
        [System.Serializable]
        public sealed partial class SideScroller3DMovementPreset : MovementPreset
        {
            public SideScroller3DMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine)
            {
                CameraController = new TargetFollow3DCameraController();
            }

            protected sealed override void Fall(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;
                stateMachine?.Move(Time.deltaTime * Vector2.up * stateMachine.FallSpeed);
            }

            protected sealed override void Move(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;
                float inputX = stateMachine.MoveInput.x;

                Transform transform = stateMachine.transform;
                stateMachine?.Move(Time.deltaTime * (Vector3.right * inputX * stateMachine.MoveSpeed + Vector3.up * stateMachine.FallSpeed));
            }

            protected sealed override void SetType(ref MovementPresetType controlType)
            {
                controlType = MovementPresetType.SideScroller2D;
            }
        }

      
        [System.Serializable]
        public sealed partial class TopDown2DMovementPreset : MovementPreset
        {
            public TopDown2DMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine)
            {
                CameraController = new TopDown2DCameraController();
            }

            protected sealed override void Fall(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;
                stateMachine?.Move(Time.deltaTime * Vector2.up * 0f);
            }

            protected sealed override void Move(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;

                Vector2 input = stateMachine.MoveInput;
                float moveSpeed = stateMachine.MoveSpeed;

                Transform transform = stateMachine.transform;
                stateMachine?.Move(Time.deltaTime * (Vector2.right * input.x * moveSpeed +
                                                     Vector2.up    * input.y * moveSpeed));
            }

            protected sealed override void SetType(ref MovementPresetType controlType)
            {
                controlType = MovementPresetType.TopDown2D;
            }
        }

       
        [System.Serializable]
        public sealed partial class TopDown2DTankControlMovementPreset : TankControlMovementPreset
        {
            public TopDown2DTankControlMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine) {
                CameraController = new TopDown2DCameraController();
            }

            protected sealed override void Fall(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;
                stateMachine?.Move(Time.deltaTime * Vector2.up * 0f);
            }


            public sealed override void LateUpdateTankControls(Transform transform, float rotationSpeed)
            {
                if (transform == null) return;
                transform.localRotation *= Quaternion.Euler(Time.deltaTime * rotationSpeed * transform.forward);

            }

            protected sealed override void Move(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;

                float inputY = stateMachine.MoveInput.y;
                float moveSpeed = stateMachine.MoveSpeed;

                Transform transform = stateMachine.transform;
                stateMachine?.Move(Time.deltaTime * (Vector2.up * inputY * moveSpeed));
            }

            protected sealed override void SetType(ref MovementPresetType controlType) {
                controlType = MovementPresetType.TopDown2DTankControl;
            }
        }

       
        [System.Serializable]
        public sealed partial class TopDown3DMovementPreset : MovementPreset
        {
            public TopDown3DMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine)
            {
                CameraController = new TopDown3DCameraController();
            }

            protected sealed override void Fall(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;
                stateMachine?.Move(Time.deltaTime * Vector2.up * stateMachine.FallSpeed);
            }


            protected sealed override void Move(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;

                Vector2 input   = stateMachine.MoveInput;
                float moveSpeed = stateMachine.MoveSpeed;

                Transform transform = stateMachine.transform;
                stateMachine?.Move(Time.deltaTime * (Vector3.right   * input.x * moveSpeed +
                                                     Vector3.forward * input.y * moveSpeed));

                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime);
            }

            protected sealed override void SetType(ref MovementPresetType controlType)
            {
                controlType = MovementPresetType.TopDown3D;
            }
        }

      
        [System.Serializable]
        public sealed partial class TopDown3DTankControlMovementPreset : TankControlMovementPreset
        {
            public TopDown3DTankControlMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine)
            {
                CameraController = new TopDown3DCameraController();
            }

            protected sealed override void Fall(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;
                stateMachine?.Move(Time.deltaTime * Vector2.up * stateMachine.FallSpeed);
            }


            public override void LateUpdateTankControls(Transform transform, float rotationSpeed)
            {
                transform.localRotation *= Quaternion.Euler(Time.deltaTime * rotationSpeed * Vector3.up);
            }

            protected sealed override void Move(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;

                Vector2 input = stateMachine.MoveInput;
                float moveSpeed = stateMachine.MoveSpeed;

                Transform transform = stateMachine.transform;
                stateMachine?.Move(Time.deltaTime * (Vector3.forward * input.y * moveSpeed));
            }

            protected sealed override void SetType(ref MovementPresetType controlType)
            {
                controlType = MovementPresetType.TopDown3DTankControl;
            }
        }

        
        [System.Serializable]
        public sealed partial class FirstPersonMovementPreset : MovementPreset
        {
            public FirstPersonMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine)
            {
                CameraController   = new FPSCameraController();
                InteractionHandler = new FPSInteractionHandler();
            }

            protected sealed override void Fall(PlayerMovementStateMachine stateMachine)
            {
                if(stateMachine == null) return;
                stateMachine?.Move(Time.deltaTime * Vector3.up * stateMachine.FallSpeed);
            }

            protected sealed override void Move(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;

                Vector2 input = stateMachine.MoveInput;
                float moveSpeed = stateMachine.MoveSpeed;

                Transform transform = stateMachine.transform;
                stateMachine?.Move(Time.deltaTime * (Vector3.right   * input.x * moveSpeed +
                                                     Vector3.forward * input.y * moveSpeed));
            }

            protected sealed override void SetType(ref MovementPresetType controlType)
            {
                controlType = MovementPresetType.FirstPerson;
            }
        }

        [System.Serializable]
        public sealed partial class FirstPersonTankControlMovementPreset : MovementPreset
        {
            public FirstPersonTankControlMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine) {
                CameraController = new FPSCameraController();
            }

            protected sealed override void Fall(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;
                stateMachine?.Move(Time.deltaTime * Vector3.up * stateMachine.FallSpeed);
            }

            protected sealed override void Move(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;

                Vector2 input = stateMachine.MoveInput;
                float moveSpeed = stateMachine.MoveSpeed;

                Transform transform = stateMachine.transform;
                stateMachine?.Move(Time.deltaTime * (Vector3.forward * input.y * moveSpeed));
            }

            protected sealed override void SetType(ref MovementPresetType controlType)
            {
                controlType = MovementPresetType.FirstPersonTankControl;
            }
        }


        [System.Serializable]
        public sealed partial class ClampedFirstPersonMovementPreset : MovementPreset
        {
            public ClampedFirstPersonMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine)
            {
                CameraController = new ClampedFPSCameraController();
            }

            protected sealed override void Fall(PlayerMovementStateMachine stateMachine)
            {
                stateMachine?.Move(Time.deltaTime * Vector3.up * stateMachine.FallSpeed);
            }

            protected sealed override void Move(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;

                Vector2 input = stateMachine.MoveInput;
                float moveSpeed = stateMachine.MoveSpeed;

                Transform transform = stateMachine.transform;
                stateMachine?.Move(Time.deltaTime * (Vector3.right * input.x * moveSpeed +
                                                     Vector3.forward * input.y * moveSpeed));
            }

            protected sealed override void SetType(ref MovementPresetType controlType)
            {
                controlType = MovementPresetType.ClampedFirstPerson;
            }
        }

       
        [System.Serializable]
        public sealed partial class ThirdPersonMovementPreset : MovementPreset
        {
            public ThirdPersonMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine)
            {
                CameraController = new TPSCameraController();
            }

            protected sealed override void Fall(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;
                stateMachine?.Move(Time.deltaTime * Vector2.up * stateMachine.FallSpeed);
            }

            protected sealed override void Move(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;

                Vector2 input = stateMachine.MoveInput;
                float moveSpeed = stateMachine.MoveSpeed;

                Transform transform = stateMachine.transform;
                stateMachine?.Move(Time.deltaTime * (Vector3.right * input.x * moveSpeed +
                                                     Vector3.forward * input.y * moveSpeed));
            }

            protected sealed override void SetType(ref MovementPresetType controlType)
            {
                controlType = MovementPresetType.ThirdPerson;
            }
        }


        [System.Serializable]
        public sealed partial class ThirdPersonTankControlMovementPreset : MovementPreset
        {
            public ThirdPersonTankControlMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine)
            {
                CameraController = new TPSCameraController();
            }

            protected sealed override void Fall(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;
                stateMachine?.Move(Time.deltaTime * Vector2.up * stateMachine.FallSpeed);
            }

            protected sealed override void Move(PlayerMovementStateMachine stateMachine)
            {
                if (stateMachine == null) return;

                Vector2 input   = stateMachine.MoveInput;
                float moveSpeed = stateMachine.MoveSpeed;

                Transform transform = stateMachine.transform;
                stateMachine?.Move(Time.deltaTime * (Vector3.forward * input.y * moveSpeed));
            }

            protected sealed override void SetType(ref MovementPresetType controlType)
            {
                controlType = MovementPresetType.ThirdPersonTankControl;
            }
        }
    }

    public abstract partial class PlayerMovementStateMachine : MovementStateMachine
    {
        public abstract partial class MovementPreset
        {
#if UNITY_EDITOR
            [SerializeField, HideInInspector] private bool displayMovementPreset;

            public virtual void DrawInspector(InspectorVisualizer visualizer) {
                if (visualizer == null) return;
                else if(EditorExtension.DisplayFoldout(GetPresetName(GetType().Name), ref displayMovementPreset, visualizer.FoldoutColor)) {
                    EditorExtension.IncrementIndent();

  

                    interactionHandler?.DrawInspector(visualizer.FoldoutColor, visualizer.BackgroundColor, visualizer.ButtonColor);
                    cameraController?.DrawInspector(visualizer.FoldoutColor, visualizer.ButtonColor, visualizer.BackgroundColor);

                    EditorExtension.DecrementIndent();
                }           
            }
#endif
        }

        public abstract partial class TankControlMovementPreset : MovementPreset
        {
#if UNITY_EDITOR
            public sealed override void DrawInspector(InspectorVisualizer visualizer)
            {
                base.DrawInspector(visualizer);

                EditorExtension.Space(10f);
                rotationSpeed = EditorExtension.DisplayFloatSlider("Rotation Speed", rotationSpeed,  0f, 100f);
            }
#endif
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
            if (visualizer == null || movementPresets == null) return;
            else if (EditorExtension.DisplayFoldout("Add / Remove", ref addOrRemoveMovementPresetsFoldout, visualizer.FoldoutColor)) {
                MovementPresetType[] values = Enum.GetValues(typeof(MovementPresetType)) as MovementPresetType[];
                if (values == null) return;

                EditorExtension.DrawVertical(() => {

                    foreach(MovementPresetType value in values) DisplayPreset(value); 
                    EditorExtension.Space(10f);

                    EditorExtension.DrawHorizontal(() =>
                    {
                        if (movementPresets.Values.Count != values.Length) EditorExtension.DisplayButton("Add All Presets", () => {
                            foreach (MovementPresetType value in values) AddMovementPreset(value);
                        });

                        if (movementPresets.Values.Count > 0) EditorExtension.DisplayButton("Remove All Presets", () => {
                            movementPresets?.Clear();
                        });
                    });
                }, true);
            }
        }

        private void DisplayPreset(MovementPresetType controlType)
        {
            if (movementPresets.ContainsKey(controlType))
                if (movementPresets[controlType] != null) return;

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
            if (visualizer == null || movementPresets == null) return;
            else if (EditorExtension.DisplayFoldout("Show", ref showMovementPresetsFoldout, visualizer.FoldoutColor))  {
             
                var datas = movementPresets.Values.ToArray();
                EditorExtension.IncrementIndent();

                if(datas != null) {
                    foreach(var data in datas) {
                        if(data == null) continue;
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