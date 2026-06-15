

namespace RedSilver2.Framework.Interactions.Actions
{
    [System.Serializable]
    public sealed class CloseDoorAction : DoorInteractionAction
    {
        public CloseDoorAction(Door module, Interaction interaction) : base(module, interaction) {
            interaction?.AddOnInteractedListener(handler => { module?.Close(); });
        }
    }
}
