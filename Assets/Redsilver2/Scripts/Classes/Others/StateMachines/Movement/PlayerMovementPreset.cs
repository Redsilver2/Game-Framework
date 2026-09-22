using RedSilver2.Framework.Interactions;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace RedSilver2.Framework.StateMachines.Presets
{
    #region Default
    [System.Serializable]
    public abstract partial class PlayerMovementPreset
    {
        [SerializeField, HideInInspector] private bool isEnabled;
        [SerializeField, SerializeReference, HideInInspector] private CameraController cameraController;
        [SerializeField, SerializeReference, HideInInspector] private InteractionHandler interactionHandler;
        [SerializeField, SerializeReference, HideInInspector] private PlayerMovementStateMachine stateMachine;

        [SerializeField, HideInInspector] private PresetType type;
       
        public PresetType Type => type;
        public bool IsEnabled => isEnabled;

        protected PlayerMovementPreset(PlayerMovementStateMachine stateMachine)
        {
            SetCameraController(GetOrCreateCameraController());
            SetInteractionHandler(GetOrCreateInteractionHandler());

            this.stateMachine = stateMachine;
        }

        protected void SetCameraController(CameraController cameraController)
        {
            this.cameraController = cameraController;
        }

        protected void SetInteractionHandler(InteractionHandler interactionHandler)
        {
            this.interactionHandler = interactionHandler;
        }

        protected void SetType(PresetType type)
        {
            this.type = type;
        }

        public void Enable() {
            if (!isEnabled) {
                stateMachine?.AddOnUpdateListener(Update);
                stateMachine?.AddOnLateUpdateListener(LateUpdate);
                isEnabled = true;
            }
        }

        public void Disable() {
            if (isEnabled) {
                stateMachine?.RemoveOnUpdateListener(Update);
                stateMachine?.RemoveOnLateUpdateListener(LateUpdate);
                isEnabled = false;
            }

        }

        protected virtual void Update() {
            cameraController?.Update();
            interactionHandler?.Update();
        }

        protected virtual void LateUpdate() {
            cameraController?.LateUpdate();

            if(stateMachine != null) {
                if     (stateMachine.IsSwimming) Swim(stateMachine, cameraController);
                else if(stateMachine.IsClimbing) Climb(stateMachine, cameraController);
                else if(stateMachine.IsMoving)   Move(stateMachine);
                else                             Fall(stateMachine);

                Rotate(stateMachine, cameraController);
            }
        }

        protected abstract void Swim(PlayerMovementStateMachine stateMachine , CameraController cameraController);
        protected abstract void Climb(PlayerMovementStateMachine stateMachine, CameraController cameraController);
        protected abstract void Move(PlayerMovementStateMachine stateMachine);
        protected abstract void Fall(PlayerMovementStateMachine stateMachine);

        protected abstract void Rotate(PlayerMovementStateMachine stateMachine, CameraController cameraController);

        protected CameraController   GetOrCreateCameraController()   { return GetOrCreateCameraController(cameraController); }
        protected InteractionHandler GetOrCreateInteractionHandler() { return GetOrCreateInteractionHandler(interactionHandler); }

        protected abstract CameraController   GetOrCreateCameraController(CameraController cameraController);
        protected abstract InteractionHandler GetOrCreateInteractionHandler(InteractionHandler interactionHandler);


        [System.Serializable]
        public enum PresetType {
            SideScroller,
            TopDown,
            FirstPerson,
            ThirdPerson
        }

    }

    [System.Serializable]
    public sealed partial class SideScrollerMovementPreset : PlayerMovementPreset
    {
        [SerializeField, HideInInspector] private bool is2DMovement;
        [SerializeField, HideInInspector] private Vector3 desiredRotation;


        [SerializeField, HideInInspector] private CameraType camera;
        [SerializeField, HideInInspector] private InteractionType interaction;
        [SerializeField, HideInInspector] private RotationType rotation;

        public bool Is2DMovement => is2DMovement;

        public CameraType Camera => camera;
        public InteractionType Interaction => interaction;
        public RotationType Rotation => rotation;

        public const PresetType _Type = PresetType.SideScroller;

        public SideScrollerMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            SetType(_Type);
        }


        public void SetIs2DMovement(bool is2DMovement)
        {
            bool isSimilarValue = this.is2DMovement == is2DMovement;
            this.is2DMovement = is2DMovement;

            if (!isSimilarValue) {
                SetCameraController(GetOrCreateCameraController());
            }
        }

        public void SetCameraType(CameraType camera)
        {

            bool isSimilarValue = this.camera == camera;
            this.camera = camera;

            if (!isSimilarValue)
            {
                SetCameraController(GetOrCreateCameraController());
            }

        }

        public void SetInteractionType(InteractionType interaction)
        {

            bool isSimilarValue = this.interaction == interaction;
            this.interaction = interaction;

            if (!isSimilarValue) {
                SetInteractionHandler(GetOrCreateInteractionHandler());
            }
        }

        public void SetRotationType(RotationType rotation)
        {
            this.rotation = rotation;
        }

        protected sealed override CameraController GetOrCreateCameraController(CameraController cameraController)
        {
            if (camera == CameraType.None) return null;
            else if (camera == CameraType.TargetFollow)
            {
                if (is2DMovement)
                {
                    if (cameraController as TargetFollow2DCameraController == null)
                        return new TargetFollow2DCameraController();
                }
                else if (cameraController as TargetFollow3DCameraController == null)
                    return new TargetFollow3DCameraController();
            }

            return cameraController;
        }

        protected sealed override InteractionHandler GetOrCreateInteractionHandler(InteractionHandler handler)
        {
            if (interaction == InteractionType.None) return null;
            else if (interaction == InteractionType.Mouse)
            {
                if (handler as MouseInteractionHandler == null) return new MouseInteractionHandler();
            }
            else if (interaction == InteractionType.Transform)
            {
                if (handler as TransformInteractionHandler == null) return new TransformInteractionHandler();
            }

            return handler;
        }

        protected sealed override void Swim(PlayerMovementStateMachine stateMachine, CameraController cameraController)
        {
            if(stateMachine != null) {
                Vector2 input = stateMachine.MoveInput;
                Vector2 result = Vector2.right * input.x * stateMachine.MoveSpeed +
                                 Vector2.up * input.y * stateMachine.MoveSpeed;

                if (input.y == 0f) Fall(stateMachine);
                stateMachine?.Move(Time.deltaTime * result);
            }
        }

        protected sealed override void Climb(PlayerMovementStateMachine stateMachine, CameraController cameraController)
        {
            if (stateMachine != null)
            {
                stateMachine?.Move(Time.deltaTime * Vector3.up * stateMachine.MoveInput.y * stateMachine.MoveSpeed);
            }
        }

        protected override void Move(PlayerMovementStateMachine stateMachine)
        {
            if (stateMachine != null)
            {
                Vector2 input = stateMachine.MoveInput;
                Vector2 result = Vector2.right * input.x * stateMachine.MoveSpeed;

                Fall(stateMachine);
                stateMachine?.Move(Time.deltaTime * result);
            }
        }

        protected override void Fall(PlayerMovementStateMachine stateMachine)
        {
            if (stateMachine != null) {
                Vector2 result = Vector2.up * stateMachine.FallSpeed;
                stateMachine?.Move(Time.deltaTime * result);
            }
        }

        protected sealed override void Rotate(PlayerMovementStateMachine stateMachine, CameraController cameraController)
        {
            if(stateMachine != null) {
                Transform transform = stateMachine.transform;

                if (is2DMovement) {
                    float inputX = stateMachine.MoveInput.x;
                    transform.localRotation = Quaternion.identity;

                    if (Mathf.Abs(inputX) > 0f) {
                        transform.localScale    = Vector3.right   * Mathf.Sign(inputX) * transform.localScale.x +
                                                  Vector3.up      * transform.localScale.y + 
                                                  Vector3.forward * transform.localScale.z;
                    }
                }
                else { transform.localRotation = Quaternion.Euler(desiredRotation); }
                
            }

        }

        [System.Serializable]
        public enum RotationType
        {
            Keyboard,
            Mouse
        }


        [System.Serializable]
        public enum CameraType
        {
            None,
            TargetFollow
        }

        [System.Serializable]
        public enum InteractionType
        {
            None,
            Mouse,
            Transform
        }
    }

    [System.Serializable]
    public sealed partial class TopDownMovementPreset : PlayerMovementPreset
    {
        [SerializeField, HideInInspector] private float rotationSpeed;


        [SerializeField, HideInInspector] private bool is2DMovement;
        [SerializeField, HideInInspector] private bool isTankControl;

        [SerializeField, HideInInspector] private CameraType camera;
        [SerializeField, HideInInspector] private InteractionType interaction;
        [SerializeField, HideInInspector] private RotationType rotation;


        public float RotationSpeed => rotationSpeed;
        public bool IsTankControl => isTankControl;
        public bool Is2DMovement => is2DMovement;

        public CameraType Camera => camera;
        public InteractionType Interaction => interaction;
        public RotationType Rotation => rotation;

        public const PresetType _Type = PresetType.TopDown;

        public TopDownMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine) {
            SetType(_Type);
        }

        public void SetRotationSpeed(float rotationSpeed)
        {
            this.rotationSpeed = Mathf.Clamp(rotationSpeed, 0f, float.MaxValue);
        }

        public void SetIs2DMovement(bool is2DMovement)
        {
            bool isSimilarValue = this.is2DMovement == is2DMovement;
            this.is2DMovement = is2DMovement;

            if (!isSimilarValue)
            {
                if(is2DMovement) isTankControl = false;
                SetCameraController(GetOrCreateCameraController());
            }
        }

        public void SetIsTankControl(bool isTankControl)
        {
            this.isTankControl = is2DMovement ? false : isTankControl;
        }


        public void SetCameraType(CameraType camera)
        {

            bool isSimilarValue = this.camera == camera;
            this.camera = camera;

            if (!isSimilarValue)
            {
                SetCameraController(GetOrCreateCameraController());
            }

        }

        public void SetInteractionType(InteractionType interaction)
        {

            bool isSimilarValue = this.interaction == interaction;
            this.interaction = interaction;

            if (!isSimilarValue)
            {
                SetInteractionHandler(GetOrCreateInteractionHandler());
            }
        }

        public void SetRotationType(RotationType rotation)
        {
            this.rotation = rotation;
        }


        protected sealed override CameraController GetOrCreateCameraController(CameraController cameraController)
        {
            if      (camera == CameraType.None) return null;
            else if (camera == CameraType.TopDown)
            {
                if (is2DMovement)
                {
                    if (cameraController as TopDown2DCameraController == null)
                        return new TopDown2DCameraController();
                }
                else if (cameraController as TopDown3DCameraController == null)
                    return new TopDown3DCameraController();
            }

            return cameraController;
        }

        protected sealed override InteractionHandler GetOrCreateInteractionHandler(InteractionHandler handler)
        {
            if (interaction == InteractionType.None) return null;
            else if (interaction == InteractionType.Transform)
            {
                if (handler as TransformInteractionHandler == null) return new TransformInteractionHandler();
            }
            else if (interaction == InteractionType.Mouse)
            {
                if (handler as MouseInteractionHandler == null) return new MouseInteractionHandler();
            }

            return handler;
        }

        protected sealed override void Swim(PlayerMovementStateMachine stateMachine, CameraController cameraController)
        {
            if (stateMachine != null)
            {
                Vector2 input = stateMachine.MoveInput;

                if (is2DMovement) {
                    stateMachine?.Move(Time.deltaTime * (Vector2.right * input.x * stateMachine.MoveSpeed +
                                                         Vector2.up    * input.y * stateMachine.MoveSpeed));
                }
                else {
                    stateMachine?.Move(Time.deltaTime * (Vector3.right   * input.x * stateMachine.MoveSpeed +
                                                         Vector3.forward * input.y * stateMachine.MoveSpeed));
                }
            }
        }

        protected sealed override void Climb(PlayerMovementStateMachine stateMachine, CameraController cameraController) {
            if (!is2DMovement) {
                stateMachine?.Move(Time.deltaTime * Vector3.up * stateMachine.MoveInput.y * stateMachine.MoveSpeed);
            }
        }

        protected sealed override void Move(PlayerMovementStateMachine stateMachine)
        {
            if(stateMachine != null) {
                Vector2 input = stateMachine.MoveInput;

                if (is2DMovement) {
                    stateMachine?.Move(Time.deltaTime * (Vector2.right * input.x * stateMachine.MoveSpeed +
                                                         Vector2.up * input.y * stateMachine.MoveSpeed));
                }
                else {
                    Fall(stateMachine);

                    if (!isTankControl) {
                        stateMachine?.Move(Time.deltaTime * (Vector3.right   * input.x * stateMachine.MoveSpeed +
                                                             Vector3.forward * input.y * stateMachine.MoveSpeed));
                    }
                    else if(isTankControl && Mathf.Abs(input.x) == 0f) {
                        stateMachine?.Move(Time.deltaTime * Vector3.forward * input.y * stateMachine.MoveSpeed);
                    }
                
                }
            }
        }

        protected sealed override void Fall(PlayerMovementStateMachine stateMachine)
        {
            if(!is2DMovement && stateMachine != null)
                stateMachine?.Move(Time.deltaTime * Vector3.up * stateMachine.FallSpeed);
        }

        protected sealed override void Rotate(PlayerMovementStateMachine stateMachine, CameraController cameraController)
        {
            if (stateMachine != null)
            {
                if (!isTankControl || (isTankControl && Mathf.Abs(stateMachine.MoveInput.y) == 0f)) {
                    if (rotation == RotationType.None) {
                        Transform transform = stateMachine.transform;
                        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime);
                    }
                    else if (is2DMovement) { Rotate2D(stateMachine, cameraController); }
                    else                   { Rotate3D(stateMachine, cameraController); }
                }
            }
        }

        private void Rotate2D(PlayerMovementStateMachine stateMachine, CameraController cameraController)
        {
            if (stateMachine != null) {
                Vector2 input = stateMachine.MoveInput;

                if      (rotation == RotationType.Keyboard) { KeyboardRotation2D(stateMachine); }
                else if (rotation == RotationType.Mouse) MouseRotation2D(stateMachine, cameraController);
            }
        }

        private void Rotate3D(PlayerMovementStateMachine stateMachine, CameraController cameraController)
        {
            (cameraController as TopDown3DCameraController)?.SetDesiredRotation(Vector3.right * 90f);

            if (stateMachine != null) {
                Vector2 input = stateMachine.MoveInput;

                if     (rotation == RotationType.Keyboard) { KeyboardRotation3D(stateMachine); }
                else if(rotation == RotationType.Mouse)    { MouseRotation3D(stateMachine, cameraController); }
            }
        }

        private void KeyboardRotation2D(PlayerMovementStateMachine stateMachine)
        {
            if (stateMachine != null) {
                Transform transform = stateMachine.transform;
                Vector3   scale     = transform.localScale;
               
                float     inputX    = stateMachine.MoveInput.x;
                float     sign      = Mathf.Sign(scale.x);

                if (inputX < 0f || inputX > 0f) sign = Mathf.Sign(inputX);

                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime);

                transform.localScale = Vector3.right   * sign * scale.x +
                                       Vector3.up      * scale.y + 
                                       Vector3.forward * scale.z;
            }
        }
        private void KeyboardRotation3D(PlayerMovementStateMachine stateMachine) {
            if (stateMachine != null) {
                stateMachine.transform.localRotation *= Quaternion.Euler(Time.deltaTime * stateMachine.MoveInput.x * rotationSpeed * Vector3.up);
            }   
        }

        private void MouseRotation2D(PlayerMovementStateMachine stateMachine, CameraController cameraController)
        {
            if (stateMachine != null && cameraController != null)
            {
                Camera camera = cameraController.Camera;

                if (camera != null && Mouse.current != null) {
                    Transform transform = stateMachine.transform;
                    Vector3 scale = transform.localScale;

                    float inputX = camera.ScreenToWorldPoint(Mouse.current.position.ReadDefaultValue()).x;
                    float sign = Mathf.Sign(scale.x);

                    if (inputX < transform.position.x|| inputX > transform.position.x) sign = Mathf.Sign(inputX);

                    transform.localScale = Vector3.right * sign * scale.x +
                                           Vector3.up * scale.y +
                                           Vector3.forward * scale.z;

                }
            }
        }

        private void MouseRotation3D(PlayerMovementStateMachine stateMachine, CameraController cameraController)
        {
            if (stateMachine != null && cameraController != null)
            {
                Camera camera = cameraController.Camera;
                if (camera != null && Mouse.current != null)
                {
                    Vector3 desiredPosition = camera.ScreenToWorldPoint(Mouse.current.position.ReadDefaultValue());
                    Quaternion desiredRotation = Quaternion.LookRotation(stateMachine.transform.position - desiredPosition, Vector3.up);
                    stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, desiredRotation, Time.deltaTime);
                }
            }
        }

        [System.Serializable]
        public enum RotationType
        {
            None,
            Keyboard,
            Mouse
        }

        [System.Serializable]
        public enum CameraType
        {
            None,
            TopDown
        }


        [System.Serializable]
        public enum InteractionType
        {
            None,
            Mouse,
            Transform
        }
    }

    [System.Serializable]
    public sealed partial class FirstPersonMovementPreset : PlayerMovementPreset
    {
        [SerializeField, HideInInspector] private float rotationSpeed;
        [SerializeField, HideInInspector] private bool isTankControl;

        [SerializeField, HideInInspector] private CameraType camera;
        [SerializeField, HideInInspector] private InteractionType interaction;
        [SerializeField, HideInInspector] private RotationType rotation;

        public bool IsTankControl => isTankControl;
        public float RotationSpeed => rotationSpeed;

        public CameraType      Camera      => camera;
        public InteractionType Interaction => interaction;
        public RotationType    Rotation    => rotation;

        public const PresetType _Type = PresetType.FirstPerson;

        public FirstPersonMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            SetType(_Type);
        }

        public void SetIsTankControl(bool isTankControl)
        {
            this.isTankControl = isTankControl;
        }

        public void SetRotationSpeed(float rotationSpeed) {  this.rotationSpeed = Mathf.Clamp(rotationSpeed, 0f, float.MaxValue); }

        public void SetCameraType(CameraType camera)
        {

            bool isSimilarValue = this.camera == camera;
            this.camera = camera;

            if (!isSimilarValue)
            {
                SetCameraController(GetOrCreateCameraController());
            }

        }

        public void SetInteractionType(InteractionType interaction)
        {

            bool isSimilarValue = this.interaction == interaction;
            this.interaction = interaction;

            if (!isSimilarValue)
            {
                SetInteractionHandler(GetOrCreateInteractionHandler());
            }
        }


        public void SetRotationType(RotationType rotation)
        {
            this.rotation = rotation;
        }



        protected sealed override CameraController GetOrCreateCameraController(CameraController cameraController)
        {
            if (camera == CameraType.None) return null;
            else if (camera == CameraType.FPS)
            {
                if (cameraController as FPSCameraController == null)
                    return new FPSCameraController();
            }
            else if (camera == CameraType.ClampedFPS)
            {
                if (cameraController as ClampedFPSCameraController == null)
                    return new ClampedFPSCameraController();
            }

            return cameraController;
        }

        protected sealed override InteractionHandler GetOrCreateInteractionHandler(InteractionHandler handler)
        {
            if (interaction == InteractionType.None) return null;
            else if (interaction == InteractionType.Camera)
            {
                if (handler as CameraInteractionHandler == null) return new CameraInteractionHandler();
            }
            else if (interaction == InteractionType.Mouse)
            {
                if (handler as MouseInteractionHandler == null) return new MouseInteractionHandler();
            }

            return handler;
        }

        protected sealed override void Swim(PlayerMovementStateMachine stateMachine, CameraController cameraController)
        {
            FPSCameraController fPSCameraController = cameraController as FPSCameraController;

            if (stateMachine != null) {
                Vector2 input  = stateMachine.MoveInput;
                Vector3 result = (Vector3.right   * input.x * stateMachine.MoveSpeed +
                                  Vector3.forward * input.y * stateMachine.MoveSpeed);

                if (Mathf.Abs(input.y) == 0f) result.y = stateMachine.FallSpeed;
                else if (fPSCameraController != null) {
                    Swim(ref result, stateMachine, fPSCameraController);
                }

                stateMachine?.Move(Time.deltaTime * result);
            }
        }

        private void Swim(ref Vector3 result, PlayerMovementStateMachine stateMachine, FPSCameraController fPSCameraController)
        {
            if(fPSCameraController != null && stateMachine != null) {
                float headRotation = fPSCameraController.HeadRotation;

                if (headRotation < 0f || headRotation > 0f) {
                    result.y = Time.deltaTime * Mathf.Sign(headRotation) * 
                               Mathf.Clamp01(Mathf.Abs(headRotation) / 
                               Mathf.Abs(headRotation < 0f  ? fPSCameraController.MinHeadRotation : fPSCameraController.MaxHeadRotation)) *
                               stateMachine.MoveSpeed;
                }
                else { result.y = 0f; }
            }
        }

        protected sealed override void Climb(PlayerMovementStateMachine stateMachine, CameraController cameraController)
        {
            Camera camera = cameraController != null ? cameraController.Camera : null;

            if(stateMachine != null) {
                float inputY = stateMachine.MoveInput.y;
                stateMachine?.Move(Time.deltaTime * Vector3.up * inputY * stateMachine.MoveSpeed);
            }
        }


        protected sealed override void Move(PlayerMovementStateMachine stateMachine)
        {
            Fall(stateMachine);

            if (stateMachine != null)
            {
                Vector2 input = stateMachine.MoveInput;

                if (!isTankControl) {
                    Debug.Log("???");

                    stateMachine?.Move(Time.deltaTime * (Vector3.right * input.x * stateMachine.MoveSpeed +
                                                         Vector3.forward * input.y * stateMachine.MoveSpeed));
                }
                else if(isTankControl && Mathf.Abs(input.x) == 0f) {
                    stateMachine?.Move(Time.deltaTime * Vector3.forward * input.y * stateMachine.MoveSpeed);
                }
            }
        }

        protected sealed override void Fall(PlayerMovementStateMachine stateMachine)
        {
            if(stateMachine != null) {
                stateMachine?.Move(Time.deltaTime * Vector3.up * stateMachine.FallSpeed);
            }
        }

        protected sealed override void Rotate(PlayerMovementStateMachine stateMachine, CameraController cameraController)
        {
            FPSCameraController fpsCameraController = cameraController as FPSCameraController;

            if (stateMachine != null &&  fpsCameraController != null) {
                if (!isTankControl || (isTankControl && Mathf.Abs(stateMachine.MoveInput.y) == 0f)) {
                    if   (rotation == RotationType.Keyboard) { KeyboardRotation(stateMachine, fpsCameraController); }
                    else if(rotation == RotationType.Camera) { CameraRotation(fpsCameraController);                 }
                }
            }
        }

        private void KeyboardRotation(PlayerMovementStateMachine stateMachine, FPSCameraController cameraController)
        {
            if (cameraController != null && stateMachine != null)
            {
                cameraController?.SetCanUpdateBodyRotation(false);

                if (isTankControl) {
                    Transform head = cameraController.Head;
                    Debug.Log(cameraController.CanUpdateHeadRotation);

                    cameraController?.SetCanUpdateHeadRotation(false);

                    if (head != null)
                        head.localRotation = Quaternion.Slerp(head.localRotation, Quaternion.identity, Time.deltaTime);
                }
                else { cameraController?.SetCanUpdateHeadRotation(true); }

                stateMachine.transform.localRotation *= Quaternion.Euler(Time.deltaTime * stateMachine.MoveInput.x * rotationSpeed * Vector3.up);
            }
        }

        private void CameraRotation(FPSCameraController cameraController)
        {
            cameraController?.SetCanUpdateBodyRotation(true);
            cameraController?.SetCanUpdateHeadRotation(true);
        }

        [System.Serializable]
        public enum RotationType
        {
            Keyboard,
            Camera
        }

        [System.Serializable]
        public enum CameraType
        {
            None,
            FPS,
            ClampedFPS
        }

        [System.Serializable]
        public enum InteractionType
        {
            None,
            Camera,
            Mouse
        }
    }

    [System.Serializable]
    public sealed partial class ThirdPersonMovementPreset : PlayerMovementPreset
    {
        [SerializeField, HideInInspector] private float rotationSpeed;
        [SerializeField, HideInInspector] private bool isTankControl;

        [SerializeField, HideInInspector] private CameraType camera;
        [SerializeField, HideInInspector] private InteractionType interaction;
        [SerializeField, HideInInspector] private RotationType rotation;

        public bool IsTankControl => isTankControl;
        public float RotationSpeed => rotationSpeed;

        public RotationType RotationMode => rotation;

        public const PresetType _Type = PresetType.ThirdPerson;


        public ThirdPersonMovementPreset(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            SetType(_Type);
        }

        public void SetRotationSpeed(float rotationSpeed) { this.rotationSpeed = Mathf.Clamp(rotationSpeed, 0f, float.MaxValue); }

        public void SetIsTankControl(bool isTankControl)
        {
            this.isTankControl = isTankControl;
        }


        public void SetCameraType(CameraType camera)
        {

            bool isSimilarValue = this.camera == camera;
            this.camera = camera;

            if (!isSimilarValue)
            {
                SetCameraController(GetOrCreateCameraController());
            }

        }

        public void SetInteractionType(InteractionType interaction)
        {

            bool isSimilarValue = this.interaction == interaction;
            this.interaction = interaction;

            if (!isSimilarValue)
            {
                SetInteractionHandler(GetOrCreateInteractionHandler());
            }
        }


        public void SetRotationType(RotationType rotation)
        {
            this.rotation = rotation;
        }

        protected sealed override CameraController GetOrCreateCameraController(CameraController cameraController)
        {
            if (camera == CameraType.None) return null;
            else if (camera == CameraType.ThirdPerson)
            {
                if (cameraController as TPSCameraController == null)
                    return new TPSCameraController();
            }

            return cameraController;
        }

        protected sealed override InteractionHandler GetOrCreateInteractionHandler(InteractionHandler handler)
        {
            if (interaction == InteractionType.None) return null;
            else if (interaction == InteractionType.Transform)
            {
                if (handler as TransformInteractionHandler == null) return new TransformInteractionHandler();
            }
            else if (interaction == InteractionType.Mouse)
            {
                if (handler as MouseInteractionHandler == null) return new MouseInteractionHandler();
            }

            return handler;
        }

        protected sealed override void Swim(PlayerMovementStateMachine stateMachine, CameraController cameraController)
        {
            FPSCameraController fPSCameraController = cameraController as FPSCameraController;

            if (stateMachine != null)
            {
                Vector2 input = stateMachine.MoveInput;
                Vector3 result = (Vector3.right * input.x * stateMachine.MoveSpeed +
                                  Vector3.forward * input.y * stateMachine.MoveSpeed);

                if (Mathf.Abs(input.y) == 0f) result.y = stateMachine.FallSpeed;
                else if (fPSCameraController != null) {
                    Swim(ref result, stateMachine, fPSCameraController);
                }

                stateMachine?.Move(Time.deltaTime * result);
            }
        }

        private void Swim(ref Vector3 result, PlayerMovementStateMachine stateMachine, FPSCameraController fPSCameraController)
        {
            if (fPSCameraController != null && stateMachine != null)
            {
                float headRotation = fPSCameraController.HeadRotation;

                if (headRotation < 0f || headRotation > 0f) {
                    result.y = Time.deltaTime * Mathf.Sign(headRotation) *
                               Mathf.Clamp01(Mathf.Abs(headRotation) /
                               Mathf.Abs(headRotation < 0f ? fPSCameraController.MinHeadRotation : fPSCameraController.MaxHeadRotation)) *
                               stateMachine.MoveSpeed;
                }
                else { result.y = 0f; }
            }
        }

        protected sealed override void Climb(PlayerMovementStateMachine stateMachine, CameraController cameraController)
        {
            Camera camera = cameraController != null ? cameraController.Camera : null;

            if (stateMachine != null)
            {
                float inputY = stateMachine.MoveInput.y;
                stateMachine?.Move(Time.deltaTime * Vector3.up * inputY * stateMachine.MoveSpeed);
            }
        }


        protected sealed override void Move(PlayerMovementStateMachine stateMachine)
        {
            Fall(stateMachine);

            if (stateMachine != null)
            {
                Vector2 input = stateMachine.MoveInput;

                if (!isTankControl) {
                    stateMachine?.Move(Time.deltaTime * (Vector3.right * input.x * stateMachine.MoveSpeed +
                                                         Vector3.forward * input.y * stateMachine.MoveSpeed));
                }
                else if(isTankControl && Mathf.Abs(input.x) == 0f) {
                    stateMachine?.Move(Time.deltaTime * Vector3.forward * input.y * stateMachine.MoveSpeed);
                }
            }
        }

        protected sealed override void Fall(PlayerMovementStateMachine stateMachine)
        {
            if (stateMachine != null) {
                stateMachine?.Move(Time.deltaTime * Vector3.up * stateMachine.FallSpeed);
            }
        }

        protected sealed override void Rotate(PlayerMovementStateMachine stateMachine, CameraController cameraController)
        {
             TPSCameraController tpsCameraController = cameraController as TPSCameraController;


            if (stateMachine != null && tpsCameraController != null)
            {
                Vector2 input = stateMachine.MoveInput;

                if (!isTankControl || (isTankControl && Mathf.Abs(input.y) == 0f))
                {
                    tpsCameraController?.SetBody(rotation == RotationType.Camera ? stateMachine.transform : null);

                    if      (rotation == RotationType.Keyboard) { KeyboardRotation(stateMachine, tpsCameraController); }
                    else if (rotation == RotationType.Camera)   { CameraRotation(tpsCameraController);                 }
                    else if (rotation == RotationType.Mouse)    { MouseRotation(stateMachine, tpsCameraController);    }
                }
            }
        }

        private void KeyboardRotation(PlayerMovementStateMachine stateMachine, TPSCameraController cameraController) {
            if(stateMachine != null && cameraController != null)
            {
                cameraController?.SetCanUpdateBodyRotation(false);
                stateMachine.transform.localRotation *= Quaternion.Euler(Time.deltaTime * stateMachine.MoveInput.x * rotationSpeed * Vector3.up);
            }     
        }

        private void CameraRotation(TPSCameraController cameraController)
        {
            cameraController?.SetCanUpdateBodyRotation(true);
            cameraController?.SetCanUpdateHeadRotation(true);
        }

        private void MouseRotation(PlayerMovementStateMachine stateMachine, TPSCameraController cameraController)
        {
            if (stateMachine != null && cameraController != null)
            {
                Camera camera = cameraController.Camera;
                if(camera != null && Mouse.current != null) {
                    Vector3    desiredPosition = camera.ScreenToWorldPoint(Mouse.current.position.ReadDefaultValue());
                    Quaternion desiredRotation = Quaternion.LookRotation(stateMachine.transform.position - desiredPosition, Vector3.up);
                    stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, desiredRotation, Time.deltaTime);
                }
            }
        }

        [System.Serializable]
        public enum RotationType
        {
            Keyboard,
            Mouse,
            Camera
        }


        [System.Serializable]
        public enum CameraType
        {
            None,
            ThirdPerson
        }


        [System.Serializable]
        public enum InteractionType
        {
            None,
            Mouse,
            Transform
        }
    }

    [System.Serializable]
    public class PlayerMovementPresetData {
        [SerializeField, SerializeReference, HideInInspector] private PlayerMovementPreset preset;
        public PlayerMovementPreset Preset => preset; 

        public PlayerMovementPresetData(PlayerMovementPreset preset)
        {
            this.preset = preset;
        }
    }

    #endregion

    #region Editor
    public abstract partial class PlayerMovementPreset
    {
#if UNITY_EDITOR
        [SerializeField, HideInInspector] private bool displayMovementPreset;
        [SerializeField, HideInInspector] private bool displayCameraSettings;
        [SerializeField, HideInInspector] private bool displayInteractionSettings;

        public virtual void DrawInspector(StateMachine.InspectorVisualizer visualizer)
        {
            if (visualizer == null) return;
            else if (EditorExtension.DisplayFoldout(GetFormattedType(), ref displayMovementPreset, visualizer.FoldoutColor))
            {
                bool canAddSpace = false;

                EditorExtension.IncrementIndent();
                EditorExtension.Space(10f);
               
                DrawMovementTypeInspector(visualizer);

                if(cameraController != null) {
                    EditorExtension.Space(10f);
                    canAddSpace = true;
                }

                cameraController?.DrawInspector(visualizer.FoldoutColor, visualizer.ButtonColor, visualizer.BackgroundColor);

                if (interactionHandler != null && !canAddSpace) {
                    EditorExtension.Space(10f);
                }

                interactionHandler?.DrawInspector(visualizer.FoldoutColor, visualizer.BackgroundColor, visualizer.ButtonColor);

                EditorExtension.DecrementIndent();
            }
        }

        private string GetFormattedType()
        {
            string result = string.Empty;
            char[] chars  = type.ToString().ToCharArray();

            for (int i = 0; i < chars.Length; i++) {
                if (i > 0 && char.IsUpper(chars[i])) result += ' ';
                result += chars[i];
            }

            return result;
        }

        protected abstract void DrawMovementTypeInspector(StateMachine.InspectorVisualizer visualizer);
#endif
    }

    public sealed partial class SideScrollerMovementPreset : PlayerMovementPreset
    {
#if UNITY_EDITOR
        protected sealed override void DrawMovementTypeInspector(StateMachine.InspectorVisualizer visualizer)
        {
            SetIs2DMovement(EditorExtension.DisplayToggle("Is 2D Movement", is2DMovement));
            EditorExtension.Space(10f);

            SetCameraType(EditorExtension.DisplayEnum("Camera Type", camera));
            SetInteractionType(EditorExtension.DisplayEnum("Interaction Type", interaction));
            EditorExtension.Space(10f);

            SetRotationType(EditorExtension.DisplayEnum("Rotation Type", rotation));
        }
#endif
    }
  
    public sealed partial class TopDownMovementPreset : PlayerMovementPreset
    {
#if UNITY_EDITOR
        protected sealed override void DrawMovementTypeInspector(StateMachine.InspectorVisualizer visualizer)
        {
            SetIs2DMovement(EditorExtension.DisplayToggle("Is 2D Movement", is2DMovement));

            if (!is2DMovement) {
                SetIsTankControl(EditorExtension.DisplayToggle("Is Tank Control", isTankControl));
                if (isTankControl) SetRotationSpeed(EditorExtension.DisplayFloatSlider("Rotation Speed", rotationSpeed, 0f, 100f));
            }

            EditorExtension.Space(10f);
            SetCameraType(EditorExtension.DisplayEnum("Camera Type", camera));
            SetInteractionType(EditorExtension.DisplayEnum("Interaction Type", interaction));
            EditorExtension.Space(10f);

            SetRotationType(EditorExtension.DisplayEnum("Rotation Type", rotation));
        }
#endif
    }

    public sealed partial class FirstPersonMovementPreset : PlayerMovementPreset
    {
#if UNITY_EDITOR
        protected sealed override void DrawMovementTypeInspector(StateMachine.InspectorVisualizer visualizer)
        {
            SetIsTankControl(EditorExtension.DisplayToggle("Is Tank Control", isTankControl));
            if (isTankControl) SetRotationSpeed(EditorExtension.DisplayFloatSlider("Rotation Speed", rotationSpeed, 0f, 100f));
          
            EditorExtension.Space(10f);
            SetCameraType(EditorExtension.DisplayEnum("Camera Type", camera));
           
            SetInteractionType(EditorExtension.DisplayEnum("Interaction Type", interaction));
            EditorExtension.Space(10f);

            SetRotationType(EditorExtension.DisplayEnum("Rotation Type", rotation));
        }
#endif
    }
  
    public sealed partial class ThirdPersonMovementPreset : PlayerMovementPreset
    {
#if UNITY_EDITOR
        protected sealed override void DrawMovementTypeInspector(StateMachine.InspectorVisualizer visualizer)
        {
            SetIsTankControl(EditorExtension.DisplayToggle("Is Tank Control", isTankControl));
            if (isTankControl) SetRotationSpeed(EditorExtension.DisplayFloatSlider("Rotation Speed", rotationSpeed, 0f, 100f));
           
            EditorExtension.Space(10f);
            SetCameraType(EditorExtension.DisplayEnum("Camera Type", camera));
           
            SetInteractionType(EditorExtension.DisplayEnum("Interaction Type", interaction));
            EditorExtension.Space(10f);

            SetRotationType(EditorExtension.DisplayEnum("Rotation Type", rotation));
        }
#endif
    }
    #endregion
}