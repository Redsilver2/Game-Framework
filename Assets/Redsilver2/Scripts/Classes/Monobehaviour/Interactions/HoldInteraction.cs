using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public sealed class HoldInteraction : Interaction {
        [SerializeField] private float interactionResetSpeed;

        [Space]
        [SerializeField] private float currentInteractionResetTime;
        [SerializeField] private float maxInteractionResetTime;

        [Space]
        [SerializeField] private float currentInteractionTime;
        [SerializeField] private float maxInteractionTime;

        [Space]
        [SerializeField] private bool canResetInteractionTime;

        private readonly UnityEvent<float> onInteractionProgressChanged;
        private readonly UnityEvent<float> onInteractionResetProgressChanged;

        public bool  CanResetInteractionTime => canResetInteractionTime;
        public float InteractionProgress => Mathf.Clamp01(currentInteractionTime / maxInteractionTime);
        public float InteractionResetProgress => Mathf.Clamp01(currentInteractionResetTime / maxInteractionResetTime);

        public HoldInteraction(string name) : base(name)
        {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            this.maxInteractionResetTime = 0f;
            this.canResetInteractionTime = false;

            currentInteractionTime = 0f;
            currentInteractionResetTime = this.maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, float maxInteractionResetTime) : base(name) {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            this.maxInteractionResetTime = Mathf.Clamp(maxInteractionResetTime, 0f, float.MaxValue);
            this.canResetInteractionTime = false;

            currentInteractionTime      = 0f;
            currentInteractionResetTime = this.maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, bool canResetInteractionTime) : base(name)
        {
            onInteractionProgressChanged      = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            this.maxInteractionResetTime = 0f;
            this.canResetInteractionTime = canResetInteractionTime;


            currentInteractionTime = 0f;
            currentInteractionResetTime = this.maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, float maxInteractionResetTime, bool canResetInteractionTime) : base(name)
        {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            this.maxInteractionResetTime = Mathf.Clamp(maxInteractionResetTime, 0f, float.MaxValue);
            this.canResetInteractionTime = canResetInteractionTime;

            currentInteractionTime = 0f;
            currentInteractionResetTime = this.maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, string description) : base(name, description)
        {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            this.maxInteractionResetTime = 0f;
            this.canResetInteractionTime = false;

            currentInteractionTime = 0f;
            currentInteractionResetTime = this.maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, string description, bool canResetInteractionTime) : base(name, description) {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();


            this.maxInteractionResetTime = 0f;
            this.canResetInteractionTime = canResetInteractionTime;

            currentInteractionTime      = 0f;
            currentInteractionResetTime = this.maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, string description, float maxInteractionResetTime) : base(name, description)
        {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            this.maxInteractionResetTime = Mathf.Clamp(maxInteractionResetTime, 0f, float.MaxValue);
            this.canResetInteractionTime = false;

            currentInteractionTime = 0f;
            currentInteractionResetTime = this.maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, string description, float maxInteractionResetTime, bool canResetInteractionTime) : base(name, description)
        {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            this.maxInteractionResetTime = Mathf.Clamp(maxInteractionResetTime, 0f, float.MaxValue);
            this.canResetInteractionTime = canResetInteractionTime;

            currentInteractionTime = 0f;
            currentInteractionResetTime = this.maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, string description, UnityAction<InteractionHandler> onInteract) : base(name, description, onInteract)
        {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            this.maxInteractionResetTime = 0f;
            this.canResetInteractionTime = false;

            currentInteractionTime = 0f;
            currentInteractionResetTime = maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, string description, float maxInteractionResetTime, UnityAction<InteractionHandler> onInteract) : base(name, description, onInteract)
        {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            this.maxInteractionResetTime = Mathf.Clamp(maxInteractionResetTime, 0f, float.MaxValue);
            this.canResetInteractionTime = false;

            currentInteractionTime = 0f;
            currentInteractionResetTime = this.maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, string description, bool canResetInteractionTime, UnityAction<InteractionHandler> onInteract) : base(name, description, onInteract)
        {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            this.maxInteractionResetTime = 0f;
            this.canResetInteractionTime = canResetInteractionTime;

            currentInteractionTime = 0f;
            currentInteractionResetTime = this.maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, string description, float maxInteractionResetTime, bool canResetInteractionTime, UnityAction<InteractionHandler> onInteract) : base(name, description, onInteract)
        {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            this.maxInteractionResetTime = Mathf.Clamp(maxInteractionResetTime, 0f, float.MaxValue);
            this.canResetInteractionTime = canResetInteractionTime;

            currentInteractionTime = 0f;
            currentInteractionResetTime = this.maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, string description, UnityEvent<InteractionHandler> onInteract) : base(name, description, onInteract)
        {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            this.maxInteractionResetTime = 0f;
            this.canResetInteractionTime = false;

            currentInteractionTime = 0f;
            currentInteractionResetTime = this.maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, string description, float maxInteractionResetTime, UnityEvent<InteractionHandler> onInteract) : base(name, description, onInteract)
        {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            this.maxInteractionResetTime = Mathf.Clamp(maxInteractionResetTime, 0f, float.MaxValue);
            this.canResetInteractionTime = false;

            currentInteractionTime = 0f;
            currentInteractionResetTime = this.maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, string description, bool canResetInteractionTime, UnityEvent<InteractionHandler> onInteract) : base(name, description, onInteract)
        {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            this.maxInteractionResetTime = 0f;
            this.canResetInteractionTime = canResetInteractionTime;

            currentInteractionTime = 0f;
            currentInteractionResetTime = this.maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, string description, float maxInteractionResetTime, bool canResetInteractionTime, UnityEvent<InteractionHandler> onInteract) : base(name, description, onInteract)
        {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            this.maxInteractionResetTime = Mathf.Clamp(maxInteractionResetTime, 0f, float.MaxValue);
            this.canResetInteractionTime = canResetInteractionTime;

            currentInteractionTime = 0f;
            currentInteractionResetTime = this.maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }



        public void SetInteractionResetSpeed(float interactionResetSpeed) {
            this.interactionResetSpeed = interactionResetSpeed;
        }

        public void SetMaxInteractionTime(float interactionTime) {
            this.maxInteractionTime = Mathf.Clamp(interactionTime, 0f, float.MaxValue);
        }

        public void SetMaxInteractionResetTime(float interactionResetITime) {
            this.maxInteractionResetTime = Mathf.Clamp(interactionResetITime, Mathf.Epsilon, float.MaxValue);
        }
        public void SetCanResetInteractionTime(bool canResetInteractionTime) {
            this.canResetInteractionTime = canResetInteractionTime;
        }

        public sealed override bool Interact(InteractionHandler handler) {
            if (handler == null || !handler.IsHeld() || !IsEnabled) {
                ResetInteractionTime();
                return false;
            }

            UpdateInteractionTime();
            if (currentInteractionTime >= maxInteractionTime) return base.Interact(handler);
          
            return true;
        }

        private void   UpdateInteractionTime() {
            currentInteractionResetTime = maxInteractionResetTime;
            currentInteractionTime = Mathf.Clamp(Time.deltaTime + currentInteractionTime, 0f, maxInteractionTime);
        }
        private void ResetInteractionTime() {
            currentInteractionResetTime = !canResetInteractionTime ? maxInteractionResetTime : 
                                          Mathf.Clamp(currentInteractionResetTime - Time.deltaTime, 0f, maxInteractionResetTime);
            
            if (currentInteractionResetTime <= 0f) {
                currentInteractionTime = Mathf.Clamp(currentInteractionTime - (Time.deltaTime * interactionResetSpeed), 0f, maxInteractionTime);
                onInteractionProgressChanged?.Invoke(InteractionProgress);
            }
            else {
                onInteractionResetProgressChanged?.Invoke(InteractionResetProgress);
            }         
        }

        public void InteractionReset() {
            currentInteractionTime = 0f;
            currentInteractionResetTime = maxInteractionResetTime;
        }
    }
}
