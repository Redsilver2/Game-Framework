using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public abstract class SingleInteractionModule : InteractionModule
    {
        [Space]
        [SerializeField] private UnityEvent<InteractionHandler> onInteract;

        public void AddOnInteractListener(UnityAction<InteractionHandler> action){
            if (action != null) onInteract?.AddListener(action);
        }

        public void RemoveOnInteractListener(UnityAction<InteractionHandler> action) {
            if (action != null) onInteract?.RemoveListener(action);
        }

        public override void Interact(InteractionHandler handler) {
            onInteract?.Invoke(handler);
        }
    }
}
