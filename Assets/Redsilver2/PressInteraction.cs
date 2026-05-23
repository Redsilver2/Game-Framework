using RedSilver2.Framework.Inputs;
using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public class PressInteraction : Interaction
    {
        public PressInteraction(string name) : base(name) { }

        public PressInteraction(string name, string description) : base(name, description)
        {
        }

        public sealed override bool Interact(InteractionHandler handler) {
            if (handler == null || !handler.IsPressed){
                return false;
            }

            return base.Interact(handler);
        }
    }
}
