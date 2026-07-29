using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public class PressInteraction : Interaction
    {
        public PressInteraction(string name) : base(name) {
            
        }

        public PressInteraction(string name, string description) : base(name, description) {

        }

        public PressInteraction(string name, string description, UnityAction<InteractionHandler> onInteracted) : base(name, description, onInteracted) {

        }

        public PressInteraction(string name, string description, UnityEvent<InteractionHandler> onInteracted) : base(name, description, onInteracted) {

        }

        public sealed override bool Interact(InteractionHandler handler) {
            if (handler == null || !handler.IsPressed()) return false;
            return base.Interact(handler);
        }
    }
}
