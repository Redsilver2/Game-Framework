using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public class  HoldInteraction : Interaction {
        private float interactionSpeed;
        private float interactionResetSpeed;

        private float currentInteractionResetTime;
        private float maxInteractionResetTime;

        private float currentInteractionTime;
        private float maxInteractionTime;
        
        private bool canResetInteractionTime;

        private UnityEvent<float> onInteractionProgressChanged;
        private UnityEvent<float> onInteractionResetProgressChanged;

        public float InteractionProgress => Mathf.Clamp01(currentInteractionTime / maxInteractionTime);
        public float InteractionResetProgress => Mathf.Clamp01(currentInteractionResetTime / maxInteractionResetTime);

        public HoldInteraction(string name) : base(name) {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public HoldInteraction(string name, string description) : base(name, description) {
            onInteractionProgressChanged = new UnityEvent<float>();
            onInteractionResetProgressChanged = new UnityEvent<float>();

            AddOnInteractedListener(handler => { InteractionReset(); });
        }

        public void SetInteractionSpeed(float interactionSpeed)
        {
            this.interactionSpeed = interactionSpeed;
        }

        public void SetInteractionResetSpeed(float interactionResetSpeed) {
            this.interactionResetSpeed = interactionResetSpeed;
        }

        public void SetMaxInteractionTime(float interactionTime) {
            this.maxInteractionTime = interactionTime;
        }

        public void SetMaxInteractionResetITime(float interactionResetITime) {
            this.maxInteractionResetTime = interactionResetITime;
        }

        public void SetCanResetInteractionTime(bool canResetInteractionTime) {
            this.canResetInteractionTime = canResetInteractionTime;
        }

        public sealed override bool Interact(InteractionHandler handler) {
            if (handler == null || !handler.IsHeld || !IsEnabled) {
                ResetInteractionTime();
                return false;
            }

            UpdateInteractionTime();
            if (currentInteractionTime >= maxInteractionTime) return base.Interact(handler);
            return true;
        }

        private void UpdateInteractionTime() {
            currentInteractionResetTime = maxInteractionResetTime;
            currentInteractionTime = Mathf.Clamp(Time.deltaTime + currentInteractionTime, 0f, maxInteractionTime);
        }

        protected void ResetInteractionTime() {
            currentInteractionResetTime = Mathf.Clamp(currentInteractionResetTime - Time.deltaTime, 0f, maxInteractionResetTime);
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

        public IEnumerator InteractionResetCoroutine() {
            while (canResetInteractionTime) {
                ResetInteractionTime();
                
                if (currentInteractionTime <= 0f || currentInteractionTime >= maxInteractionTime) 
                    break;

                yield return null;
            }
        }
    }
}
