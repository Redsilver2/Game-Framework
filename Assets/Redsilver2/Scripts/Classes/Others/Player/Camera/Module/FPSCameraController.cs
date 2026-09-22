using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;

namespace RedSilver2.Framework.Player
{
    [System.Serializable]
    public partial class FPSCameraController : CameraController
    {
        [SerializeField, HideInInspector] private MouseVector2InputSettings inputSettings;

        [Space]
        [SerializeField, HideInInspector] private Transform parent;
        [SerializeField, HideInInspector] private Transform body;
        [SerializeField, HideInInspector] private Transform head;

        [SerializeField, HideInInspector] private float defaultSensitivityX;
        [SerializeField, HideInInspector] private float defaultSensitivityY;


        [SerializeField, HideInInspector] private float minHeadRotation = -45f;
        [SerializeField, HideInInspector] private float maxHeadRotation = 45f;

        [SerializeField, HideInInspector] private bool  canLerpHeadRotation;
        [SerializeField, HideInInspector] private float headRotationReturnSpeed;

        [SerializeField, HideInInspector] private bool canDragHead;
        [SerializeField, HideInInspector] private float dragHeadSpeed;

        [SerializeField, HideInInspector] private bool canDragBody;
        [SerializeField, HideInInspector] private float dragBodySpeed;

        [SerializeField, HideInInspector] Vector3 originalHeadRotation;

        private Vector2 input;

        protected float headRotation;
        protected float bodyRotation;

        private bool canUpdateCameraTransform;
        private bool canUpdateHeadRotation;
        private bool canUpdateBodyRotation;

        public float MinHeadRotation => maxHeadRotation;
        public float MaxHeadRotation => maxHeadRotation;

        public float HeadRotation => headRotation;
        public float BodyRotation => bodyRotation;

        public bool CanUpdateCameraTransform => canUpdateCameraTransform;
        public bool CanUpdateHeadRotation    => canUpdateBodyRotation;
        public bool CanUpdateBodyRotation    => canUpdateBodyRotation;


        public Transform Body => body;
        public Transform Head => head;
        public Vector2 Input => input;

        public FPSCameraController() : base()
        {

        }

        public void SetMinHeadRotation(float minHeadRotation)
        {
            this.minHeadRotation = Mathf.Clamp(minHeadRotation, float.MinValue, 0f);
        }

        public void SetMaxHeadRotation(float maxHeadRotation)
        {
           this.maxHeadRotation = Mathf.Clamp(maxHeadRotation, 0f, float.MaxValue);
        }

        public void SetCanLerpHeadRotation(bool canLerpHeadRotation)
        {
            this.canLerpHeadRotation = canLerpHeadRotation;
        }

        public void SetCanUpdateCameraTransform(bool canUpdateCameraTransform)
        {
            this.canUpdateCameraTransform = canUpdateCameraTransform;
        }

        public void SetCanUpdateHeadRotation(bool canUpdateHeadRotation)
        {
            this.canUpdateHeadRotation = canUpdateHeadRotation;
        }

        public void SetCanUpdateBodyRotation(bool canUpdateBodyRotation)
        {
            this.canUpdateBodyRotation = canUpdateBodyRotation;
        }

        public void SetHeadRotationReturnSpeed(float headRotationReturnSpeed)
        {
            this.headRotationReturnSpeed = headRotationReturnSpeed;
        }

        public void SetParent(Transform parent)
        {
            this.parent = parent;
        }

        public void SetBody(Transform body) 
        {
            this.body = body; 
        }

        public void SetHead(Transform head)
        {
            this.head = head;
        }

        protected override void Update(Camera camera) {
            inputSettings?.Enable();
            input = inputSettings != null ? inputSettings.GetValue() : Vector2.zero;


            if (camera != null) camera.transform.SetParent(parent);

            if (canUpdateHeadRotation) UpdateHeadRotation(head);
            if (canUpdateBodyRotation) UpdateBodyRotation(body);
        }

        protected override void LateUpdate(Camera camera) {
            if(canUpdateCameraTransform) UpdateCameraTransform(camera, 1f);

            if (canUpdateHeadRotation) {
                Quaternion _headRotation = Quaternion.Euler(headRotation, originalHeadRotation.y, originalHeadRotation.z);
                UpdateTransform(canDragHead, dragHeadSpeed, _headRotation, head);
            }

            if (canUpdateBodyRotation) {
                Quaternion _bodyRotation = Quaternion.Euler(originalHeadRotation.x, bodyRotation, originalHeadRotation.z);
                UpdateTransform(canDragBody, dragBodySpeed, _bodyRotation, body);
            }
        }



        protected virtual void UpdateCameraTransform(Camera camera, float updateSpeed)
        {
            if (camera != null) {
                camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, Vector3.zero, Time.deltaTime * updateSpeed);
                camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, Quaternion.identity, Time.deltaTime * updateSpeed);
            }
        }

        public void RotateBody(float rotation)
        {
            Rotate(rotation, ref bodyRotation);
        }

        public void RotateHead(float rotation) {
            Rotate(rotation, ref headRotation);
        }


        protected void ReturnRotation(float value, float minValue, float maxValue, float rotationSpeed, ref float rotation) {
            value = Mathf.Clamp(value, minValue, maxValue);
            rotation = Mathf.Lerp(rotation, value, Time.deltaTime * rotationSpeed);
        }

        private void Rotate(float rotation, ref float current)
        {
            current += Time.deltaTime * rotation;
        }

        private void UpdateTransform(bool canDrag, float dragSpeed, Quaternion current, Transform transform)
        {
            if (transform != null)
            {
                if (canDrag) current = Quaternion.Slerp(transform.localRotation, current, Time.deltaTime * dragSpeed);
                transform.localRotation = current;
            }
        }

        protected virtual void UpdateBodyRotation(Transform body)
        {
            if (body == null)
            {
                bodyRotation = 0f;
                return;
            }

            bodyRotation += Time.deltaTime * Input.x * defaultSensitivityX;
        }

        protected virtual void UpdateHeadRotation(Transform head)
        {
            if (head == null) {
                headRotation = 0f;
                return;
            }

            headRotation += Time.deltaTime * -Input.y * defaultSensitivityY;

            if (!canLerpHeadRotation) { headRotation = Mathf.Clamp(headRotation, minHeadRotation, maxHeadRotation);  }
            else {
                if (headRotation > maxHeadRotation || headRotation < minHeadRotation) {
                    ReturnRotation(headRotation > maxHeadRotation ? maxHeadRotation : minHeadRotation,
                                   minHeadRotation, maxHeadRotation, headRotationReturnSpeed, ref headRotation);
                }
            }
        }

        public void SetOriginalHeadRotation(Vector3 rotation)
        {
            originalHeadRotation = rotation;
        }

        public void SetOriginalBodyRotation(Vector3 rotation)
        {
            originalHeadRotation = rotation;
        }

        public void SetCanDragHead(bool canDragHead)
        {
            this.canDragHead = canDragHead;
        }

        public void SetCanDragBody(bool canDragBody)
        {
            this.canDragBody = canDragBody;
        }

        public void SetDragHeadSpeed(float dragHeadSpeed)
        {
            this.dragHeadSpeed = dragHeadSpeed;
        }

        public void SetDragBodySpeed(float dragBodySpeed)
        {
            this.dragBodySpeed = dragBodySpeed;
        }
    }
    public partial class FPSCameraController : CameraController
    {
#if UNITY_EDITOR
        protected override void ShowBaseSettings(Color foldoutColor, Color buttonColor, Color backgroundColor)
        {
            base.ShowBaseSettings(foldoutColor, buttonColor, backgroundColor);

            EditorExtension.Space(10f);
            inputSettings = EditorExtension.DisplayCustomField("Input", false, inputSettings);

            EditorExtension.Space(10f);
            parent = EditorExtension.DisplayCustomField("Parent", true, parent);
            body   = EditorExtension.DisplayCustomField("Body", true, body);
            head   = EditorExtension.DisplayCustomField("Head", true, head);

            EditorExtension.Space(10f);
            defaultSensitivityX = EditorExtension.DisplayFloatSlider("Default Sensitivity X", defaultSensitivityX, 1f, 100f);
            defaultSensitivityY = EditorExtension.DisplayFloatSlider("Default Sensitivity Y", defaultSensitivityY, 1f, 100f);

            EditorExtension.Space(10f);
            minHeadRotation = EditorExtension.DisplayFloatSlider("Min Head Rotation", minHeadRotation, -45f, 0f);
            maxHeadRotation = EditorExtension.DisplayFloatSlider("Max Head Rotation", maxHeadRotation, 0f, 45f);

            EditorExtension.Space(10f);
            canLerpHeadRotation = EditorExtension.DisplayToggle("Can Lerp Head Rotation", canLerpHeadRotation);
            headRotationReturnSpeed = EditorExtension.DisplayFloatSlider("Head Rotation Return Speed", headRotationReturnSpeed, 1f, 100f);

            EditorExtension.Space(10f);
            canDragHead = EditorExtension.DisplayToggle("Can Drag Head", canDragHead);
            dragHeadSpeed = EditorExtension.DisplayFloatSlider("Drag Head Speed", dragHeadSpeed, 1f, 100f);

            EditorExtension.Space(10f);
            canDragBody = EditorExtension.DisplayToggle("Can Drag Body", canDragBody);
            dragBodySpeed = EditorExtension.DisplayFloatSlider("Drag Body Speed", dragBodySpeed, 1f, 100f);

            EditorExtension.Space(10f);
            originalHeadRotation = EditorExtension.DisplayVector3Field("Original Head Rotation", originalHeadRotation);
        }
#endif
    }

}
