using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public class  HoldInteraction : Interaction {
        private float interactionResetSpeed;

        private float currentInteractionResetTime;
        private float maxInteractionResetTime;

        private float currentInteractionTime;
        private float maxInteractionTime;
        
        private bool canResetInteractionTime;

        private readonly UnityEvent<float> onInteractionProgressChanged;
        private readonly UnityEvent<float> onInteractionResetProgressChanged;

        public bool  CanResetInteractionTime => canResetInteractionTime;
        public float InteractionProgress => Mathf.Clamp01(currentInteractionTime / maxInteractionTime);
        public float InteractionResetProgress => Mathf.Clamp01(currentInteractionResetTime / maxInteractionResetTime);

        public HoldInteraction(string name) : base(name) {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();
           
            currentInteractionTime = 0f;
            currentInteractionResetTime = maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, string description) : base(name, description) {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            currentInteractionTime      = 0f;
            currentInteractionResetTime = maxInteractionResetTime;

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public void SetInteractionResetSpeed(float interactionResetSpeed) {
            this.interactionResetSpeed = interactionResetSpeed;
        }
        public void SetMaxInteractionTime(float interactionTime) {
            this.maxInteractionTime = Mathf.Clamp(interactionTime, 0f, float.MaxValue);
        }

        public void SetMaxInteractionResetITime(float interactionResetITime) {
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
        protected void ResetInteractionTime() {
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
