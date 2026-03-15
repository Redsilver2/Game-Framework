
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public class AdvancedHoldInteractionModule : HoldInteractionModule
    {
        [Space]
        [SerializeField] private float interactionSpeed;
        [SerializeField] private float maxInteractionTime;

        [Space]
        [SerializeField] private bool canClearProgressOnRelease;

        [Space]
        [SerializeField] private UnityEvent<InteractionHandler>  onPressInteraction;
        [SerializeField] private UnityEvent<InteractionHandler>  onReleaseInteraction;

        [Space]
        [SerializeField] private UnityEvent<float, InteractionHandler> onProgressChanged;

        protected float interactionTime;

        public bool CanClearProgressOnRelease => canClearProgressOnRelease;
        public float Progress => Mathf.Clamp01(interactionTime / maxInteractionTime);

        protected virtual void Start() {
            interactionTime             = 0;

            AddOnInteractListener(handler => {
                if (handler == null) return;
                UpdateProgress(interactionSpeed, handler);
            });

            AddOnReleaseInteractionListener(handler =>
            {
                if(handler == null) return;

                if(this is not TimedInteractionModule)
                {
                    interactionTime = 0f;
                    onProgressChanged?.Invoke(Progress, handler);
                }
            });
        }


        public void AddOnProgressChanged(UnityAction<float, InteractionHandler> action) 
        {
            if(onProgressChanged != null && action != null)
                onProgressChanged.AddListener(action);
        }
        public void RemoveOnProgressChanged(UnityAction<float, InteractionHandler> action)
        {
            if (onProgressChanged != null && action != null)
                onProgressChanged.RemoveListener(action);
        }

        public void AddOnReleaseInteractionListener(UnityAction<InteractionHandler> action)
        {
            if (onReleaseInteraction != null && action != null)
                onReleaseInteraction.AddListener(action);
        }
        public void RemoveOnReleaseInteractionListener(UnityAction<InteractionHandler> action)
        {
            if (onReleaseInteraction != null && action != null)
                onReleaseInteraction.RemoveListener(action);
        }

        public void AddOnPressInteractionListener(UnityAction<InteractionHandler> action)
        {
            if (onPressInteraction != null && action != null)
                onPressInteraction.AddListener(action);
        }
        public void RemoveOnPressInteractionListener(UnityAction<InteractionHandler> action)
        {
            if (onPressInteraction != null && action != null)
                onPressInteraction.RemoveListener(action);
        }

        public sealed override void Interact(InteractionHandler handler) {
            if (handler != null && enabled)
            {
                if      (handler.IsInputPressed)  onPressInteraction?.Invoke(handler);
                else if (handler.IsInputReleased) onReleaseInteraction?.Invoke(handler);
                else    base.Interact(handler);
            }
        }

        public virtual void Release()
        {

        }

        protected void UpdateProgress(float updateSpeed, InteractionHandler handler)
        {
            interactionTime = Mathf.Clamp(Time.deltaTime * interactionSpeed + interactionTime, 0f, maxInteractionTime);
            onProgressChanged?.Invoke(Progress, handler);
        }

        public void SetInteractionTime(float time) {
            interactionTime = Mathf.Clamp(time, 0f, maxInteractionTime);
        }
    }
}
