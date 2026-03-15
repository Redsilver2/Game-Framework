using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public abstract class InteractionModule : MonoBehaviour
    {
        [SerializeField] private string interactionName;
        [SerializeField] private InteractionType type;

        private Collider _collider;

        public string InteractionName => interactionName;
        public Collider Collider      => _collider;
        public InteractionType Type   => type;

        protected virtual void Awake() {
            _collider = GetComponent<Collider>();
            InteractionHandler.AddInteractionModuleInstance(_collider, this);
        }

        protected virtual void OnEnable()
        {
            InteractionHandler.AddInteractionModuleInstance(_collider, this);
        }

        protected virtual void OnDisable()
        {
            InteractionHandler.RemoveInteractionModuleInstance(_collider);
        }

        public abstract void Interact(InteractionHandler handler);
    }
}
