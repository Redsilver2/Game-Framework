using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    [System.Serializable]
    public abstract class DoorInteractionAction : InteractionAction
    {
        protected DoorInteractionAction(Door module, Interaction interaction) : base(module, interaction) {
       
        }
    }
}
