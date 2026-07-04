using RedSilver2.Framework.Interactions.Settings;
using UnityEngine;

namespace RedSilver2.Framework.Interactions.Actions
{
    [System.Serializable]
    public abstract class InteractionAction
    {
        [SerializeField] private string Name;
        public readonly Interaction Interaction;

        public InteractionAction(InteractionModule module, Interaction interaction) {
            this.Interaction = interaction;  
        }

        public void Update(InteractionHandler handler){
            Interaction?.Interact(handler);
        }

        public void Enable() {
            Interaction?.Enable();
        }

        public void Disable() {
            Interaction?.Disable();
        }
    }
}
