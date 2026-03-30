using RedSilver2.Framework.Inputs.Configurations;
using RedSilver2.Framework.Settings;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player
{
    public abstract class CameraController
    {
        private bool isEnabled;

        protected float rotationClampX;
        protected float rotationClampY;

        protected readonly Transform head;
        protected readonly Transform body;

        private   readonly UnityEvent<Vector2> onUpdate;
        private   readonly UnityEvent          onLateUpdate;

        private readonly MouseVector2InputConfiguration Configuration;

        public bool IsEnabled => isEnabled;
        public Transform Head => head;
        public Transform Body => body;

        protected CameraController(MouseVector2InputConfiguration configuration, Transform body, Transform head)
        {
            Configuration = configuration;
            this.isEnabled = false;

            this.body = body;
            this.head = head;

            this.onUpdate     = new UnityEvent<Vector2>();
            this.onLateUpdate = new UnityEvent();

            AddOnUpdateListener(OnUpdate);
            AddOnLateUpdateListener(OnLateUpdate);
        }

        protected virtual void OnUpdate(Vector2 input) {
            rotationClampY += Time.deltaTime * SettingManager.GetSensitivityX() * input.x;
            rotationClampX -= Time.deltaTime * SettingManager.GetSensivitityY() * input.y;
        }

        protected virtual void OnLateUpdate() {
            if (body != null) body.localEulerAngles = Vector2.up    * rotationClampY;
            if (head != null) head.localEulerAngles = Vector2.right * rotationClampX;
        }

        private void Update(Vector2 vector)
        {
            onUpdate.Invoke(vector);
        }
        public void LateUpdate()
        {
            onLateUpdate.Invoke();
        }      

        public void AddOnUpdateListener(UnityAction<Vector2> action) 
        {
            if(action != null) onUpdate?.AddListener(action);
        }
        public void RemoveOnUpdateListener(UnityAction<Vector2> action)
        {
            if (action != null) onUpdate?.RemoveListener(action);
        }

        public void AddOnLateUpdateListener(UnityAction action)
        {
            if (action != null) onLateUpdate?.AddListener(action);
        }
        public void RemoveOnLateUpdateListener(UnityAction action)
        {
            if (action != null) onLateUpdate?.AddListener(action);
        }

        public virtual void Enable() {
            if (!isEnabled)
            {
                isEnabled = true;
                Debug.Log(Configuration);

                Configuration?.AddOnUpdatedListener(Update);
                Configuration?.Enable();
            }
        }

        public virtual void Disable() {
            if (isEnabled)
            {
                isEnabled = false;
                Configuration?.RemoveOnUpdatedListener(Update);
                Configuration?.Disable();
            }
        }
    }
}
