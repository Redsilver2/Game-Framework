using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public abstract class InteractionModule : MonoBehaviour
    {
        [SerializeField] private string interactionName;
        public string InteractionName => interactionName;

        protected virtual void Awake() {
            InteractionHandler.AddInteractionModuleInstance(GetComponent<Collider>(), this);
        }

        public abstract void Interact(InteractionHandler handler);
    }
}
