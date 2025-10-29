using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public abstract class SingleInteractionModule : InteractionModule
    {
        [Space]
        [SerializeField] private UnityEvent onInteract;

        public void AddOnInteractListener(UnityAction action){
            if (onInteract != null && action != null) onInteract.AddListener(action);
        }

        public void RemoveOnInteractListener(UnityAction action) {
            if (onInteract != null && action != null) onInteract.RemoveListener(action);
        }

        public void Interact() { if (onInteract != null) onInteract.Invoke(); }
    }
}
