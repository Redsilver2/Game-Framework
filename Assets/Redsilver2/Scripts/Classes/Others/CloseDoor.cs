using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    [CreateAssetMenu(fileName = "New Close Door Interaction", menuName = "Interaction/Door/Close")]
    public sealed class CloseDoor : DoorInteractionAction
    {
        public sealed override string GetInteractionName() {
            return "Close";
        }

        protected override UnityAction<InteractionHandler> GetBaseEvent(Door door)
        {
            if (door == null) return null;
            return handler => { door?.Close(); };
        }
    }
}
