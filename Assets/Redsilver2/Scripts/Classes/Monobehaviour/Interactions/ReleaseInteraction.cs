using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public sealed class ReleaseInteraction : Interaction
    {
        public ReleaseInteraction(string name) : base(name) {

        }

        public ReleaseInteraction(string name, string description) : base(name, description) {

        }

        public sealed override bool Interact(InteractionHandler handler)
        {
            if(handler == null || !handler.IsReleased()) {
                return false;
            }

            return base.Interact(handler);
        }
    }
}
