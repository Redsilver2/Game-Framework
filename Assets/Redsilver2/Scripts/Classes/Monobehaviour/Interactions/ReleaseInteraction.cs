using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public sealed class ReleaseInteraction : Interaction
    {
        public ReleaseInteraction(string name) : base(name) {

        }

        public ReleaseInteraction(string name, string description) : base(name, description) {

        }

        public ReleaseInteraction(string name, string description, UnityAction<InteractionHandler> onInteracted) : base(name, description, onInteracted) {

        }

        public ReleaseInteraction(string name, string description, UnityEvent<InteractionHandler> onInteracted) : base(name, description, onInteracted) {

        }

        public sealed override bool Interact(InteractionHandler handler)
        {
            if(handler == null || !handler.IsReleased()) return false;
            return base.Interact(handler);
        }
    }
}
