using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RedSilver2.Framework.Player
{
    public abstract class CameraController : Vector2MouseConfigurationEvent
    {

        [Space]
        [SerializeField] private string cameraName;

        [Space]
        [SerializeField] private Transform body;
        [SerializeField] private Transform head;

        [Space]
        [SerializeField] private float defaultSensitivityX;
        [SerializeField] private float defaultSensitivityY;


        [Space]
        [SerializeField] private bool canDragHead;
        [SerializeField] private float dragHeadSpeed;

        [Space]
        [SerializeField] private bool canDragBody;
        [SerializeField] private float dragBodySpeed; 

        protected float headRotation;
        protected float bodyRotation;

        private Vector3 originalHeadRotation;

        public float HeadRotation => headRotation;
        public float BodyRotation => bodyRotation;

        public Transform Body => body;
        public Transform Head => head;

        private static List<CameraController> modules;

        private static CameraController current;

        public static CameraController  Current => current;
        public static CameraController[] Modules
        {
            get
            {
                if (modules == null) return new CameraController[0];
                return modules.ToArray();
            }
        }

        public void RotateBody(float rotation) {
            Rotate(rotation, ref bodyRotation);
        }

        public void RotateHead(float rotation) {
            Rotate(rotation, ref headRotation); 
        }

        private void Rotate(float rotation, ref float current)
        {
           current += Time.deltaTime * rotation;
        }

        protected override void OnLateUpdate()
        {
            Quaternion _headRotation = Quaternion.Euler(headRotation, originalHeadRotation.y, originalHeadRotation.z);
            Quaternion _bodyRotation = Quaternion.Euler(originalHeadRotation.x, bodyRotation, originalHeadRotation.z);

            UpdateTransform(canDragHead, dragHeadSpeed, _headRotation, head);
            UpdateTransform(canDragBody, dragBodySpeed, _bodyRotation, body);
        }

        private void UpdateTransform(bool canDrag, float dragSpeed, Quaternion current, Transform transform)
        {
            if (transform != null)
            {
                if (canDrag) current = Quaternion.Slerp(transform.localRotation, current, Time.deltaTime * dragSpeed);
                transform.localRotation = current;
            }
        }

        protected override void OnUpdate(Vector2 vector)
        {
            base.OnUpdate(vector);
            UpdateBodyRotation(body);
            UpdateHeadRotation(head); 
        }

        protected virtual void UpdateBodyRotation(Transform body) {
            if (body == null) {
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
        }

        public void SetOriginalHeadRotation(Vector3 rotation) {
            originalHeadRotation = rotation;
        }

        public void SetOriginalBodyRotation(Vector3 rotation) {
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

        public bool IsActifCameraController(CameraController controller) {
            if (current == null) return false;
            return current.Equals(controller);
        }

        public static void SetCursorVisibility(bool isVisible)
        {
            Cursor.lockState = isVisible ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = isVisible;
        }

        public static void SetCurrent(int index) {
            SetCurrent(GetModule(index));
        }

        public static void SetCurrent(string moduleName) {
            SetCurrent(GetModule(moduleName));
        }


        public static void SetCurrent(CameraController module)
        {
            Disable();
            current = module;
            Enable();
        }

        public static void Enable() {
            if (current != null) current.enabled = true;
        }


        public static void Disable() {
            if(current != null) current.enabled = false;
        }

        public static void CleanModules() {
            if (modules != null) modules = modules.Where(x => x != null).ToList();
        }

        public static CameraController GetModule(int index) 
        {
            if(modules == null || index < 0 || index >= modules.Count) return null;
            return modules[index];
        }

        public static CameraController GetModule(string moduleName) 
        {
            if (modules == null || string.IsNullOrEmpty(moduleName)) return null;
            
            var results = modules.Where(x => x != null)
                                 .Where(x => !string.IsNullOrEmpty(x.cameraName))
                                 .Where(x => x.cameraName.ToLower().Equals(moduleName.ToLower()));

            if (results.Count() > 0) return results.First();
            return null;
        }
    }
}
